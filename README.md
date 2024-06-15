# covidlit-search

## init

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DBProject" "Host=YourHost:Port;Database=YourDatabase;Username=YourUsername;Password=YourPassword;"
dotnet user-secrets set "Jwt:Issuer" "YourIssuer"
dotnet user-secrets set "Jwt:Audience" "YourAudience"
dotnet user-secrets set "Jwt:SecretKey" "YourSecretKey"
dotnet user-secrets set "Email:Host" "YourSmtpServer"
dotnet user-secrets set "Email:Port" "YourSmtpServerPort"
dotnet user-secrets set "Email:Username" "YourEmailAddress"
dotnet user-secrets set "Email:Password" "YourEmailPassword"
```

## generate models from database(if needed)

```bash
dotnet ef dbcontext scaffold "Name=ConnectionStrings:DBProject" Npgsql.EntityFrameworkCore.PostgreSQL --project DockerTest --data-annotations --output-dir Models -f
```
