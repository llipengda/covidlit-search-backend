# CovidLit Search (Backend)

## :book: Description

This is the backend of the CovidLit Search project. It is a RESTful API that provides endpoints to search for scientific articles related to COVID-19.

We are using the [CORD-19 dataset](https://www.kaggle.com/datasets/allen-institute-for-ai/CORD-19-research-challenge) to provide the articles. And we are using [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0) to build the RESTful API, [postgresql](https://www.postgresql.org/) as the database and [Docker](https://www.docker.com/) to containerize the application.

We have published this API on Docker Hub. You can find it [here](https://hub.docker.com/r/pdli/covidlit-search).

## :rocket: Quick Start

### Using Docker

1. pull the image from Docker Hub:

```bash
docker pull pdli/covidlit-search
```

2. prepare a `.env` file with the following content:

```bash
# PostgreSQL connection string
CONNECTIONSTRINGS_DBPROJECT="Host=YourHost:Port;Database=YourDatabase;Username=YourDbUsername;Password=YourDbUserPassword;"

# JWT configuration
## The issuer of the token, default is "CovidLitSearch"
JWT_ISSUER="CovidLitSearch"
## The audience of the token, default is "User"
JWT_AUDIENCE="User"
## The secret key to sign the token, must be a string with at least 16 characters
JWT_SECRETKEY="YourSecretKey"

# SMTP configuration
SMTP_HOST="YourSmtpHost"
SMTP_PORT="YourSmtpPort"
SMTP_USERNAME="YourSmtpUsername"
SMTP_PASSWORD="YourSmtpPassword"

# CORS configuration
## The allowed origins, separated by commas
## If not set, all origins are allowed
CORS_ORIGINS="https://allowed-origin.com,https://another-allowed-origin.com"
```

3. run the container:

```bash
docker run -d -p 8080:80 --env-file .env pdli/covidlit-search
```

### Using Local Machine

1. Clone the repository:

```bash
git clone https://github.com/llipengda/covidlit-search-backend.git
```

2. Modify `AppSettings.json` file in the `CovidLitSearch` project:

```json
{
  "ConnectionStrings": {
    "DbProject": "Host=YourHost:Port;Database=YourDatabase;Username=YourDbUsername;Password=YourDbUserPassword;"
  },
  "Jwt": {
    "Issuer": "CovidLitSearch",
    "Audience": "User",
    "SecretKey": "YourSecretKey"
  },
  "Smtp": {
    "Host": "YourSmtpHost",
    "Port": "YourSmtpPort",
    "Username": "YourSmtpUsername",
    "Password": "YourSmtpPassword"
  },
  "Cors": {
    "Origins": "https://allowed-origin.com,https://another-allowed-origin.com"
  }
}
```

3. Run the project:

```bash
dotnet run --project CovidLitSearch
```

1. Then you can see the swagger page at `/swagger/index.html`.

## :link: Links

- [Frontend](https://github.com/llipengda/covidlit-search)
- [Docker Hub](https://hub.docker.com/r/pdli/covidlit-search)
- [CORD-19 dataset](https://www.kaggle.com/datasets/allen-institute-for-ai/CORD-19-research-challenge)
- [Vercel Deployment (Frontend)](https://covidlit-search.vercel.app/)