FROM mcr.microsoft.com/dotnet/core/sdk:3.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./ .
RUN dotnet restore

RUN dotnet publish -c Release -o out -r linux-x64

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_ENVIRONMENT Development
ENTRYPOINT ["dotnet", "Users.Web.dll"]
