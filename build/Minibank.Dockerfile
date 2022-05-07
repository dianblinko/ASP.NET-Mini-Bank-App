FROM mcr.microsoft.com/dotnet/sdk:5.0 AS src
WORKDIR /src
COPY . .

RUN dotnet build src/Minibank.Web -c Release -r linux-x64
RUN dotnet test src/Tests/Minibank.Core.Tests --no-build
RUN dotnet publish src/Minibank.Web -c Release -r linux-x64 --no-build -o /dist

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as final
WORKDIR /app
COPY --from=src /dist .
ENV ASPNETCORE_URLS=http://*:5001;http://*:5000 
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DataBaseSource="Host=host.docker.internal;Port=5432;Database=minibank-demo;Username=postgres;Password=123456"
EXPOSE 5000 5001
ENTRYPOINT ["dotnet", "Minibank.Web.dll"]