using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TicketingSystem.Services.AI.Abstraction;
using TicketingSystem.Services.Settings;

namespace TicketingSystem.Services.AI
{
    public class OpenAiChatCompletionService : IChatCompletionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string Model = "gpt-40-mini";

        public OpenAiChatCompletionService(HttpClient httpClient, IOptions<OpenAiSettings> settings)
        {
            _httpClient = httpClient;
            _apiKey = settings.Value.ApiKey;
        }

        public async Task<string> CompleteAsync(
            string systemPrompt, string userMessage, CancellationToken cancellationToken = default)
        {
            var requestBody = new
            {
                model = Model,
                messages = new object[]
                {
                    new {role = "system", content = systemPrompt},
                    new { role = "user", content = userMessage }
                },
                temperature = 0.2
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
            {
                Content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            using var response = await _httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
            using var doc = JsonDocument.Parse(responseJson);

            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? string.Empty;
        }
    }
}