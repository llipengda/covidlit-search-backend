# covidlit-search

## init

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionString:DBProject"  "Host=YourHost;Database=YourDatabase;Username=YourUsername;Password=YourPassword;"
dotnet user-secrets set "Jwt:Issuer" "YourIssuer"
dotnet user-secrets set "Jwt:Audience" "YourAudience"
dotnet user-secrets set "Jwt:SecretKey" "YourSecretKey"
```

## generate models from database(if needed)

```bash
dotnet ef dbcontext scaffold "Name=ConnectionStrings:DBProject" Npgsql.EntityFrameworkCore.PostgreSQL --project DockerTest --data-annotations --output-dir Models -f
```
