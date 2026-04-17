# Dicky TodoList

A full-stack Todo List application with JWT authentication, built with **Vue 3** (frontend) and **ASP.NET Core 8** (backend), backed by **PostgreSQL** and fully containerized with Docker.

---

## Tech Stack

| Layer       | Technology                                                            |
| ----------- | --------------------------------------------------------------------- |
| Frontend    | Vue 3, TypeScript, Pinia, Vue Router 5, TailwindCSS 4, Axios          |
| Backend     | ASP.NET Core 8, Entity Framework Core 8, FluentValidation, JWT Bearer |
| Database    | PostgreSQL 16                                                         |
| Build Tools | Vite 7, pnpm, dotnet SDK 8                                            |
| Container   | Docker, Docker Compose                                                |
| Testing     | xUnit, Moq, EF Core InMemory                                          |

---

## Features

- User registration and login with JWT authentication
- Create, read, update, and delete personal todos
- Mark todos as complete/incomplete
- Route guards (protected routes require authentication, guest-only routes redirect authenticated users)
- Responsive UI with mobile hamburger navigation
- Global exception handling middleware
- Input validation via FluentValidation
- Swagger UI for API exploration
- Fully containerized — runs with a single `docker compose up`

---

## Project Structure

```
dicky_todolist/
├── docker-compose.yml
├── .dockerignore
├── backend/
│   ├── Dockerfile
│   ├── dotnet.slnx
│   └── dicky_todolist/
│       ├── Controllers/        # AuthController, TodosController, UsersController
│       ├── Data/               # AppDbContext (EF Core)
│       ├── DTOs/               # Request/response data transfer objects
│       ├── Exceptions/         # Custom exception types
│       ├── Middlewares/        # Global exception handler
│       ├── Migrations/         # EF Core migrations
│       ├── Models/             # User, Todo entities
│       ├── Services/           # AuthService (JWT generation)
│       ├── Validators/         # FluentValidation validators
│       └── Program.cs
├── frontend/
│   ├── Dockerfile
│   ├── nginx.conf
│   └── src/
│       ├── api/                # Axios instance + interceptors
│       ├── components/         # Navbar, button, footer
│       ├── composables/
│       ├── layouts/            # MainLayout, AuthLayout
│       ├── pages/              # Home, Todos, Profile, Login, Register
│       ├── router/             # Routes + navigation guards
│       ├── services/           # AuthService (API calls)
│       ├── stores/             # Pinia stores
│       └── types/
└── backend/ToDoList.Tests/     # xUnit unit tests
```

---

## Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (recommended)

**Or for local development without Docker:**

- [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 20+](https://nodejs.org/) + [pnpm](https://pnpm.io/)
- [PostgreSQL 16](https://www.postgresql.org/download/)

---

### Running with Docker (Recommended)

```bash
docker compose up --build
```

This starts three containers:

| Container                 | Description                | Port   |
| ------------------------- | -------------------------- | ------ |
| `dicky_todolist`          | PostgreSQL 16 database     | `5432` |
| `dicky_todolist_backend`  | ASP.NET Core API           | `5170` |
| `dicky_todolist_frontend` | Vue 3 app served via Nginx | `5173` |

Access the app at **http://localhost:5173**

API Swagger UI is available at **http://localhost:5170/swagger**

> The backend waits for PostgreSQL to be healthy before starting, and automatically creates the database schema on first run.

---

### Running Locally (Without Docker)

#### Backend

1. Ensure PostgreSQL is running locally on port `5432` with the database `dicky_todolist`.
2. Update the connection string in `backend/dicky_todolist/appsettings.Development.json` if needed.

```bash
cd backend/dicky_todolist
dotnet run
```

The API will be available at **http://localhost:5170**

#### Frontend

```bash
cd frontend
pnpm install
pnpm dev
```

The app will be available at **http://localhost:5173**

---

## API Endpoints

### Auth — `/api/auth`

| Method | Endpoint             | Auth   | Description                          |
| ------ | -------------------- | ------ | ------------------------------------ |
| `POST` | `/api/auth/register` | Public | Register a new user                  |
| `POST` | `/api/auth/login`    | Public | Login and receive a JWT token        |
| `GET`  | `/api/auth/me`       | Bearer | Get the currently authenticated user |

### Todos — `/api/todos`

| Method   | Endpoint                   | Auth   | Description                        |
| -------- | -------------------------- | ------ | ---------------------------------- |
| `GET`    | `/api/todos`               | Bearer | Get all todos for the current user |
| `POST`   | `/api/todos`               | Bearer | Create a new todo                  |
| `GET`    | `/api/todos/{id}`          | Bearer | Get a specific todo                |
| `PUT`    | `/api/todos/{id}`          | Bearer | Update a todo                      |
| `DELETE` | `/api/todos/{id}`          | Bearer | Delete a todo                      |
| `PATCH`  | `/api/todos/{id}/complete` | Bearer | Toggle todo completion             |

### Users — `/api/users`

| Method   | Endpoint          | Auth   | Description         |
| -------- | ----------------- | ------ | ------------------- |
| `GET`    | `/api/users`      | Bearer | Get all users       |
| `GET`    | `/api/users/{id}` | Bearer | Get a specific user |
| `PUT`    | `/api/users/{id}` | Bearer | Update a user       |
| `DELETE` | `/api/users/{id}` | Bearer | Delete a user       |

---

## Configuration

### Backend (`appsettings.json`)

```json
{
  "Jwt": {
    "Key": "your-secret-key",
    "Issuer": "dicky-todolist-api",
    "Audience": "dicky-todolist-users"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=dicky_todolist;Username=postgres;Password=postgres"
  }
}
```

> **Important:** Change `Jwt:Key` to a strong, unique secret before deploying to production.

### Frontend

The API base URL is configured via the `VITE_API_BASE_URL` environment variable. When running via Docker, Nginx proxies `/api/` requests to the backend container automatically.

---

## Running Tests

```bash
dotnet test backend/dotnet.slnx
```

The test suite covers `TodosController` CRUD operations using an in-memory database and mock auth context.

---

## Docker Details

### Services

```yaml
# PostgreSQL
dicky_todolist: postgres:16-alpine, port 5432

# ASP.NET Core API
dicky_todolist_backend: built from backend/Dockerfile, port 5170

# Vue 3 + Nginx
dicky_todolist_frontend: built from frontend/Dockerfile, port 5173
```

### Nginx (Frontend)

The frontend container uses Nginx to:

- Serve the Vue 3 SPA with `try_files` fallback for client-side routing
- Reverse proxy `/api/` requests to the backend container

---

## Pages

| Route             | Description       | Auth Required |
| ----------------- | ----------------- | ------------- |
| `/`               | Home page         | No            |
| `/login`          | Login form        | Guest only    |
| `/register`       | Registration form | Guest only    |
| `/todos`          | Todo list         | Yes           |
| `/todos/create`   | Create todo form  | Yes           |
| `/todos/:id/edit` | Edit todo form    | Yes           |
| `/profile`        | User profile      | Yes           |
