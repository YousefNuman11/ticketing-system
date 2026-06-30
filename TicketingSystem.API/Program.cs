using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TicketingSystem.API.Common;
using TicketingSystem.API.Middleware;
using TicketingSystem.Repository.Data;
using TicketingSystem.Repository.Models;
using TicketingSystem.Repository.Search;
using TicketingSystem.Repository.UnitOfWork;
using TicketingSystem.Repository.UnitOfWork.Abstraction;
using TicketingSystem.Services.AI;
using TicketingSystem.Services.AI.Abstraction;
using TicketingSystem.Services.Mapping;
using TicketingSystem.Services.Settings;

var builder = WebApplication.CreateBuilder(args);

// Structured logging (console + rolling file) for the Web API layer.
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig
    .ReadFrom.Configuration(context.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day));

const string SpaCorsPolicy = "SpaCors";

builder.Services
    .AddControllers(options =>
    {
        // Wrap every response in the standard ApiResponse envelope.
        options.Filters.Add<ApiResponseWrapperFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull;
    });

// Return model-validation errors in the standard envelope.
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value!.Errors.Count > 0)
            .SelectMany(e => e.Value!.Errors.Select(x => x.ErrorMessage))
            .ToList();

        return new BadRequestObjectResult(
            ApiResponse.Fail(StatusCodes.Status400BadRequest, "Validation failed", errors));
    };
});

// Swagger / OpenAPI with JWT bearer support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ticketing System API",
        Version = "v1",
        Description = "Client support ticketing system API"
    });

    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste your JWT token here (without the 'Bearer ' prefix).",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    options.AddSecurityDefinition("Bearer", jwtScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtScheme, Array.Empty<string>() }
    });
});

// DbContext
builder.Services.AddDbContext<TicketingSystemDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// CORS — allow any origin. AllowAnyOrigin() is the correct wildcard;
// WithOrigins("*") does NOT work (it is treated as a literal origin).
// Note: AllowAnyOrigin cannot be combined with AllowCredentials, but the API
// authenticates with a JWT in the Authorization header (not cookies), so this is fine.
builder.Services.AddCors(options =>
    options.AddPolicy(SpaCorsPolicy, policy => policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()));

// Health checks (includes a database connectivity probe)
builder.Services
    .AddHealthChecks()
    .AddDbContextCheck<TicketingSystemDbContext>("database");

// AutoMapper (commercial v16 — license key optional; a missing key only logs)
builder.Services.AddAutoMapper(cfg =>
{
    var licenseKey = builder.Configuration["AutoMapper:LicenseKey"];
    if (!string.IsNullOrWhiteSpace(licenseKey))
        cfg.LicenseKey = licenseKey;
}, typeof(MappingProfile));

// MediatR (registers every command/query handler in the Services assembly)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

// Settings
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// JWT authentication
var jwtSettings = builder.Configuration
    .GetSection("JwtSettings")
    .Get<JwtSettings>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key))
        };
    });


//LuceneSearch

builder.Services.AddSingleton<IUserSearchService>(_ =>
    new LuceneUserSearchService(
        Path.Combine(Directory.GetCurrentDirectory(), "LuceneIndex", "Users")));


//OpenAI embeddings
builder.Services.Configure<OpenAiSettings>(
    builder.Configuration.GetSection("OpenAi"));

builder.Services.AddHttpClient<IEmbeddingService, OpenAiEmbeddingService>();


builder.Services.AddHttpClient<IChatCompletionService, OpenAiChatCompletionService>();
builder.Services.AddSingleton<ICosineSimilarityService, CosineSimilarityService>();

var app = builder.Build();

// Seeding the manager
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TicketingSystemDbContext>();

    if (!context.Users.Any())
    {
        var hasher = new PasswordHasher<User>();

        var manager = new User
        {
            FullName = "Yusef Alnuman",
            Email = "yusef@company.com",
            MobileNumber = "0790000000",
            DateOfBirth = new DateTime(2003, 9, 11),
            Role = UserRole.Manager,
            Address = "Amman",
            IsActive = true
        };

        manager.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Yusef123");

        context.Users.Add(manager);
        context.SaveChanges();
    }
}

// Backfill: embed any already-Resolved tickets that predate the RAG feature
// (i.e. have no embedding yet). Safe to run on every startup — only processes
// tickets where EmbeddingJson is still null, so already-indexed tickets are skipped.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TicketingSystemDbContext>();
    var embeddingService = scope.ServiceProvider.GetRequiredService<IEmbeddingService>();

    var unembeddedResolvedTickets = context.Tickets
        .Include(t => t.TicketsComments)
        .Where(t => t.Status == TicketStatus.Resolved && t.EmbeddingJson == null)
        .ToList();

    var embeddedCount = 0;

    foreach (var ticket in unembeddedResolvedTickets)
    {
        try
        {
            var textToEmbed = TicketEmbeddingTextBuilder.Build(ticket);
            var embedding = await embeddingService.EmbedAsync(textToEmbed);
            ticket.EmbeddingJson = System.Text.Json.JsonSerializer.Serialize(embedding);
            embeddedCount++;
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to embed ticket {TicketId} during backfill", ticket.Id);
        }
    }

    if (embeddedCount > 0)
    {
        await context.SaveChangesAsync();
    }

    Log.Information("RAG backfill: embedded {Count} of {Total} resolved tickets",
        embeddedCount, unembeddedResolvedTickets.Count);
}


// Log each request, then convert thrown exceptions into the standard ApiResponse envelope.
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticketing System API v1");
        options.DocumentTitle = "Ticketing System API";
    });
}

app.UseHttpsRedirection();

app.UseCors(SpaCorsPolicy);

// Serve uploaded attachments from {ProjectRoot}/Uploads at /uploads/*
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
    RequestPath = "/uploads"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Simple liveness ping
app.MapGet("/api/ping", () => Results.Ok(new
{
    status = "pong",
    timestamp = DateTime.UtcNow
}))
.AllowAnonymous();

// Health endpoint for monitoring (JSON with per-check status)
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var payload = JsonSerializer.Serialize(new
        {
            status = report.Status.ToString(),
            totalDurationMs = report.TotalDuration.TotalMilliseconds,
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                description = e.Value.Description
            })
        });
        await context.Response.WriteAsync(payload);
    }
});

app.Run();