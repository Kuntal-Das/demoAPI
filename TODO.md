- dotnet user-secrets init
- dotnet user-secrets set "Movies:ServiceApiKey" "12345"
- dotnet user-secrets list
- dotnet user-secrets remove "Movies:ConnectionString"
- dotnet user-secrets clear

[more info](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows)