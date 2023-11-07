# Dashboard API

## Dev Environment

Install .Net sdk6 6.0.124 or higher:  
[https://dotnet.microsoft.com/en-us/download/dotnet/6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)📌

### User Secrets

```json
{
  "JwtOptions": [
    { "IssuerSigningKey": "d88660b7fd584cacb16f182eb3d94cdb" },
    { "ValidIssuer": "apiWithAuthBackend" },
    { "ValidAudience": "apiWithAuthBackend" }
  ]
}
```
### Database/Migrations

```bash
dotnet ef database update --project ./src/Dashboard.Api/Dashboard.Api.csproj
```

### Running the API

Browser should execute the url after the API launch:
[https://localhost:7108/index.html](https://localhost:7108/index.html)📌

# **Thank you!**