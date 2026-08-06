# Client Support Ticketing System

A full-stack support ticket management platform built with **.NET 8** and **React**, designed to streamline client-to-employee communication through structured ticket workflows, role-based access control, and intelligent automation.

---

## Features

### Core Workflow
- **Three-role system** — Manager, Employee, and Client with distinct dashboards and permissions
- **Ticket lifecycle management** — Create, assign, escalate, resolve, and close tickets
- **File attachments** — Upload and retrieve files associated with tickets
- **Comment threads** — Structured communication on each ticket

### Search & Discovery
- **Full-text search** powered by **Lucene.NET** — prefix matching, startup backfill for existing data
- **Paginated list views** — Consistent `PagedResult<T>` pattern across all entities

### Async Messaging
- **RabbitMQ integration** — Welcome emails dispatched asynchronously on user registration via a `BackgroundService` consumer

### AI-Powered RAG Chatbot *(Manager-gated)*
- **Intent classification** using `gpt-4o-mini`
- **Semantic search** over resolved tickets via `text-embedding-3-small` embeddings + cosine similarity
- **Analytics gating** — certain insights restricted to Manager role

### Developer Experience
- **Serilog** structured logging
- **Global exception middleware** for consistent error responses
- **dotnet user-secrets** for all sensitive configuration — no hardcoded credentials

---

## Tech Stack

### Backend
| Layer | Technology |
|---|---|
| Framework | .NET 8, ASP.NET Core |
| Architecture | Clean Architecture, CQRS + MediatR |
| ORM | Entity Framework Core + SQL Server |
| Mapping | AutoMapper |
| Auth | JWT + Refresh Token Rotation |
| Messaging | RabbitMQ (Client 7.x async API) |
| Search | Lucene.NET 4.8.0-beta |
| AI | OpenAI API (`gpt-4o-mini`, `text-embedding-3-small`) |
| Logging | Serilog |

### Frontend
| Layer | Technology |
|---|---|
| Framework | React + Vite |
| UI | MUI + Tailwind CSS |
| Layout | CSS Grid |
| i18n | Arabic/RTL support |
| Theme | Dark mode via `ThemeContext` |

---

## Architecture Overview

```
src/
├── Domain/              # Entities, value objects, domain events
├── Application/         # CQRS commands/queries, DTOs, interfaces
│   ├── Features/
│   │   ├── Tickets/
│   │   ├── Users/
│   │   ├── Comments/
│   │   └── ...
│   └── Common/          # PagedResult, PaginationHelper, BaseSpecification
├── Infrastructure/      # EF Core, Lucene, RabbitMQ, OpenAI, file storage
└── API/                 # Controllers, middleware, DI registration
```

All EF queries use `BaseSpecification<T>` with `Criteria` + `AddInclude`. All list endpoints return `PagedResult<TDto>` via `PaginationHelper.ToPagedResultAsync`.

---

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server
- RabbitMQ
- Node.js 18+
- OpenAI API key *(optional — required for RAG chatbot)*

### Backend Setup

```bash
# Clone the repo
git clone <repo-url>
cd TicketingSystem.API

# Set secrets
dotnet user-secrets set "ConnectionStrings:Default" "Server=...;Database=TicketingDb;..."
dotnet user-secrets set "Jwt:Key" "<your-jwt-secret>"
dotnet user-secrets set "RabbitMQ:Host" "localhost"
dotnet user-secrets set "OpenAI:ApiKey" "<your-openai-key>"

# Apply migrations
dotnet ef database update

# Run
dotnet run
```

### Frontend Setup

```bash
cd ticketing-client
npm install
npm run dev
```

---

## Key Design Decisions

- **Clean Architecture** separates domain logic from infrastructure concerns, making the codebase testable and maintainable.
- **CQRS + MediatR** enforces a clear read/write separation and keeps handlers focused.
- **Lucene.NET as a singleton** with `FSDirectory` backing and startup backfill ensures search is consistent with the database state.
- **RabbitMQ consumer as `BackgroundService`** with `IServiceScopeFactory` safely resolves scoped EF Core services inside a long-running hosted service.
- **RAG pipeline** stores `EmbeddingJson` on resolved tickets; at query time, the user's question is embedded and compared via cosine similarity to find the most relevant historical resolutions before prompting the LLM.

---

## API Highlights

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/auth/register` | Register a new user |
| `POST` | `/api/auth/login` | Authenticate and receive JWT |
| `POST` | `/api/auth/refresh` | Rotate refresh token |
| `GET` | `/api/tickets` | List tickets (paginated, role-filtered) |
| `POST` | `/api/tickets` | Create a ticket |
| `PATCH` | `/api/tickets/{id}/assign` | Assign ticket to employee |
| `PATCH` | `/api/tickets/{id}/resolve` | Resolve ticket + generate embedding |
| `GET` | `/api/users/search?q=` | Full-text user search (Lucene) |
| `POST` | `/api/chatbot/query` | RAG chatbot (Manager only) |

---

## 👨‍💻 Author

**Yousef Numan**

- GitHub: https://github.com/YousefNuman11
- LinkedIn: www.linkedin.com/in/yusefnuman
