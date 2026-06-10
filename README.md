# AstronomyClubManagementSystem — Full Stack Astronomy Management System

A modern, scalable platform for managing astronomy club operations, observations, equipment, and data processing.

Built with a **hybrid architecture** combining relational and document databases, object storage, and a modular .NET backend.

---

##  Stack

* **Backend:** .NET 10 (ASP.NET Core Web API)
* **Frontend:** Blazor Server
* **Relational DB:** SQL Server 2022 (Dockerized)
* **NoSQL DB:** MongoDB 7
* **Object Storage:** MinIO (S3-compatible)
* **Cache:** Redis
* **ORM:** Entity Framework Core (Database First)
* **Auth:** ASP.NET Identity + JWT
* **Background Jobs:** Hangfire
* **Other:** MailKit, Telegram Bot API, Serilog

---

## Architecture 

```
AstronomyClubManagementSystem/
│
├── Data           # EF Core + MongoDB models
├── Domain         # Core business models
├── Application    # Business logic/services
├── Infrastructure # External services (MinIO, Email, etc.)
├── Api            # REST API
├── Web.Club       # Blazor frontend for club-specific work
└── Web.Public     # Blazor frontend for public access
```

---

## Infrastructure (Docker)

The system runs a full backend stack using Docker:

* SQL Server
* MongoDB
* MinIO
* Redis

### Start Services

```bash
docker compose up -d
```

### Check SQL Server

```bash
docker exec astro_sqlserver ./opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "AstroClub2025" \
  -Q "SELECT @@VERSION"
```

---

## Database Setup (Using EF Core Migrations)

Instead of running manual SQL scripts, the database is created and managed using **Entity Framework Core migrations**.

### 1. Create Database

```bash
docker exec astro_sqlserver /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P "AstroClub2025" -C \
  -Q "CREATE DATABASE AstroDbClub"
```

---

### 2. Apply Migrations

Ensure your API project is configured with the correct connection string, then run:

```bash
cd Data/
dotnet ef database update --context AstroClubDbContext
```

This will:

* Create all tables
* Apply schema
* Configure Identity tables
* Keep DB schema in sync with code

---

## EF Core (Database First for Domain Models)

### Install CLI

```bash
dotnet tool install --global dotnet-ef
```

### Scaffold Database (if needed)

```bash
dotnet ef dbcontext scaffold \
"Server=localhost,1433;Database=AstroDbClub;User Id=sa;Password=AstroClub2025;TrustServerCertificate=True" \
Microsoft.EntityFrameworkCore.SqlServer \
--output-dir Entities/Generated \
--context-dir Context \
--context AstroClubDbContext \
--namespace Data.Entities \
--context-namespace Data.Context \
--data-annotations \
--no-onconfiguring \
--force
```

---

## Authentication

* ASP.NET Identity (Users, Roles)
* JWT-based authentication
* Refresh token support
* Backend-For-Frontend in Blazor server
---

## MongoDB Integration

Used for flexible, unstructured data:

* Observation details
* Image metadata (FITS headers, WCS, processing)
* Forecast plans
* Equipment specifications

### Example Collections

* `observation_detail`
* `image_document`
* `equipment_specs`
* `forecast_plan`

---

## MinIO Object Storage

Used for storing:

* FITS files
* Raw images
* Thumbnails
* User uploads
## Redis Caching 
* BFF Session Store
* Notification Worker Distributed Lock
* Caching Slowly-Changing Astronomy Data
* Astronomy Event Visibility Cache
### Access

* API: http://localhost:9000
* Console: http://localhost:9001

---

## Configuration

### `appsettings.json`

```json
{
  "ConnectionStrings": {
    "SqlServer": "Server=localhost,1433;Database=AstroDbClub;User Id=sa;Password=AstroClub2025;TrustServerCertificate=True",
    "MongoDB": "mongodb://admin:AstroMongo2025@localhost:27017/AstroClubMongo?authSource=admin"
  },
  "MinIO": {
    "Endpoint":        "localhost:9000",
    "AccessKey":       "astro_admin",
    "SecretKey":       "AstroMinio2025",
    "UseSSL":          false,
    "DefaultBuckets":  ["fits","raw","previews","thumbs","sketches","profiles","horizons","maintenance"]
  },
  "Jwt": {
    "Key":      "your-256-bit-secret-key-here-change-this",
    "Issuer":   "AstroPlatform",
    "Audience": "AstroPlatformUsers",
    "AccessTokenExpiryMinutes":  60,
    "RefreshTokenExpiryDays":    30
  },
  "Serilog": {
    "MinimumLevel": "Information",
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "File",
        "Args": { "path": "logs/astro-.log", "rollingInterval": "Day" } }
    ],
    "Enrich": ["FromLogContext", "WithMachineName"]
  },
  "Redis": {
    "ConnectionString": "localhost:6379,password=AstroRedis2025,ssl=false,abortConnect=false",
    "KeyPrefix": "astro:session:",
    "SessionExpiryDays": 30
  }
}
```

---

## Key Features

* 🔭 Observation session management
* 🛰️ Astronomical event tracking
* 🛠 Equipment lifecycle & maintenance
* 🖼 Image metadata processing (MongoDB)
* ☁️ Object storage via MinIO
* 🔐 Secure authentication (JWT + Identity)
* 📬 Notifications (Email + Telegram)
* ⚙️ Background processing (Hangfire)

---

## Development Workflow

1. Start Docker services
2. Create database (`AstroDbClub`)
3. Run EF Core migrations
4. Start API
5. Start Blazor frontends

---

## Future Improvements

* Real-time updates (SignalR)
* AI-based observation recommendations
* Data visualization dashboards
* Integration with external astronomy APIs

---

## License

This project is for educational and development purposes.

---

## Author

**Akram Fadel**
Full-stack developer | Software engineering student

---

## Contributing

Contributions are welcome. Open issues or submit pull requests.

---
