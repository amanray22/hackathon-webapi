# AmanTech Hackathon — Web API

A lightweight ASP.NET Core Web API for the AmanTech Hackathon sample project. This API exposes CRUD and query endpoints for Challenges, Members and Regions backed by SQLite. The project includes database initialization, sample data seeding, concurrency handling and basic DTO mapping.

---

## Key facts
- Technology: .NET 9 (ASP.NET Core) + Entity Framework Core (SQLite)
- Project: `AmanTechHackathonWebApi` (solution `AmanTechHacathonWebApi.sln`)
- Database: SQLite file `Hacathon.db` (created/seeded automatically by the app)
- Swagger UI: available for interactive API exploration

## What this project contains
- Controllers: `ChallengeController`, `MemberController`, `RegionController` — REST endpoints for each entity.
- Data access: `HacathonContext` (EF Core DbContext) and `HacathondbInitializer` for creating and seeding the DB.
- Models & DTOs: `Model/` contains domain models, `DTOs/` contains DTO classes with validation metadata.
- Concurrency: row-version implemented via triggers that set `RowVersion` blob values in SQLite.

## Quick start (Windows / PowerShell)

Prerequisites
- .NET 9 SDK installed (verify with `dotnet --info`).

Clone and open

1. Clone the repo and open in your IDE (Visual Studio / VS Code) or via CLI.

Build and run

From project folder (`AmanTechHacathon`) run:

```powershell
dotnet restore
dotnet run --launch-profile https
```

Notes:
- The app ships configured to use SQLite. Connection string is in `appsettings.json`:

  "ConnectionStrings": { "HacathonContext": "Filename=Hacathon.db" }

- On startup the app calls `HacathondbInitializer.Initialize(...)`. By default (current code) it calls with `DeleteDatabase: true` and `SeedSampleData: true`, which recreates and seeds the database every run (useful for development/demo). For production change `DeleteDatabase` to `false` and consider `UseMigrations: true`.

Open Swagger UI
- When running locally with the provided launch profile Swagger UI will open at:

- HTTPS profile (recommended): https://localhost:7287/swagger
- HTTP profile: http://localhost:5035/swagger

You can inspect endpoints and try requests directly from that UI.

## API endpoints

Base path: `/api` (examples below assume `https://localhost:7287`)

Challenge
- GET `/api/Challenge` — list challenges (DTO)
- GET `/api/Challenge/{id}` — get single challenge
- GET `/api/Challenge/inc` — list challenges including member DTO summaries
- GET `/api/Challenge/inc/{id}` — single challenge including members

Region
- GET `/api/Region` — list regions (DTO)
- GET `/api/Region/{id}` — get region
- GET `/api/Region/inc` — list regions including member DTOs
- GET `/api/Region/inc/{id}` — region including member DTOs

Member
- GET `/api/Member` — list members
- GET `/api/Member/{id}` — single member
- POST `/api/Member` — create member (accepts MemberDTO)
- PUT `/api/Member/{id}` — update member (MemberDTO, uses RowVersion for concurrency)
- DELETE `/api/Member/{id}` — delete member
- GET `/api/Member/ByRegion/{id}` — members filtered by region id
- GET `/api/Member/ByChallenge/{id}` — members filtered by challenge id
- GET `/api/Member/inc/Region` — members including region DTO
- GET `/api/Member/inc/Challenge` — members including challenge DTO
- GET `/api/Member/inc/Full` — members including both region and challenge

Note: Some write endpoints for Region and Challenge are intentionally commented out in controllers — this API is primarily read-focused for those resources and fully writable for Members.

## DTOs and validation
- The API uses DTO classes (`DTOs/`) to shape responses and validate input.
- `MemberDTO` implements `IValidatableObject` enforcing age between 12 and 30.
- `RowVersion` is provided as a `byte[]` and used for optimistic concurrency when updating entities.

## Database & migrations
- The project uses EF Core SQLite (`Microsoft.EntityFrameworkCore.Sqlite`).
- The initializer uses `EnsureCreated` and raw SQL triggers to populate `RowVersion` values (randomblob(8)) on insert/update.
- To use EF Migrations instead of `EnsureCreated`, update the initializer call to `UseMigrations: true` and run migration commands:

```powershell
cd AmanTechHacathon
dotnet ef migrations add Initial --project . --startup-project .
dotnet ef database update --project . --startup-project .
```

Make sure the `Microsoft.EntityFrameworkCore.Tools` package is installed (it is in the project file) and you run commands from the project directory.

## Notes & recommendations
- Startup currently calls the initializer with `DeleteDatabase: true` — change to `false` before deploying or you will lose data every restart.
- Consider switching to migrations for production (set `UseMigrations: true` in initializer call) and remove the custom trigger creation when using EF migrations unless you need those triggers.
- Add authentication and authorization if you plan to expose write endpoints publicly.

## Development doc / code structure
- `Program.cs` — app bootstrapping, middleware and initializer call
- `Controllers/` — API controllers
- `Data/` — EF Core DbContext and initializer
- `Model/` — domain classes + auditing support
- `DTOs/` — request/response DTOs and validation
- `MetaData/` — attribute-based metadata for models

## How to contribute
- Fork the repo, create a feature branch, and send PRs against `main`.
- Keep changes small and include unit tests where appropriate.

## License
Added a LICENSE file to the repository root (MIT) to state permissions.

---

