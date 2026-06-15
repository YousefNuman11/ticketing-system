# Support Ticketing System — Web Client

React (Vite) single-page app for the Client Support Ticketing System, styled with a
TailAdmin-inspired Tailwind theme. It talks to the `TicketingSystem.API` backend.

## Tech stack
- React 18 + Vite (JavaScript)
- React Router v6 (role-based protected routes)
- Axios (JWT auth + standard `ApiResponse` envelope handling)
- Tailwind CSS (TailAdmin color palette)
- Recharts (manager dashboard charts)

## Prerequisites
- Node.js 18+
- The `TicketingSystem.API` running (default `http://localhost:5233`)

## Setup
```bash
cd client
npm install
cp .env.example .env   # adjust VITE_API_URL if your API runs elsewhere
npm run dev
```
The app starts on http://localhost:5173 (already allow-listed in the API CORS policy).

## Build
```bash
npm run build      # outputs to dist/
npm run preview    # preview the production build
```

## Roles & screens
- **Client** — register/login, list/create/edit own tickets, comment, upload attachments, close a resolved ticket.
- **Support Employee** — see assigned tickets, comment, mark resolved.
- **Support Manager** — dashboard (status / trend / top employees), all tickets with filters, assign tickets, manage employees (add / activate / deactivate), view clients with ticket counts, manage products.

## Notes
- The seeded manager account is `yusef@company.com` / `Yusef123` (created by the API on first run).
- A manager must add at least one **Product** before clients can open tickets.
- Auth token + user are stored in `localStorage`; a 401 response clears them and redirects to login.

## Project structure
```
src/
  api/         Axios client + endpoint modules
  components/  layout (sidebar/header), ui kit, charts, ticket panels
  context/     AuthContext (login/register/logout)
  pages/       auth / manager / client / employee screens
  routes/      ProtectedRoute (role guard)
  utils/       constants + formatters
```
