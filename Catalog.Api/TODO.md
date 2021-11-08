- dotnet user-secrets init
- dotnet user-secrets set "Movies:ServiceApiKey" "12345"
- dotnet user-secrets list
- dotnet user-secrets remove "Movies:ConnectionString"
- dotnet user-secrets clear

[more info](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows)


- docker build -t IMAGE_NAME:TAG "path"

- docker run -it --rm -p exposed_PORT:80 -e "CONF_NAME:X1"="xxxx" -e "CONF_NAME:X2"="xxxx*" -e "CONF_NAME:X3"="xxxx" IMAGE_NAME:TAG --network=NETWORK_NAME

- docker network create NETWORK_NAME
- docker network ls
