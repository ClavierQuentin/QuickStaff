# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copier les fichiers projet et restore les dépendances
COPY ["QuickStaff.API/QuickStaff.API.csproj", "QuickStaff.API/"]
COPY ["QuickStaff.Core/QuickStaff.Core.csproj", "QuickStaff.Core/"]
COPY ["QuickStaff.Infrastructure/QuickStaff.Infrastructure.csproj", "QuickStaff.Infrastructure/"]
RUN dotnet restore "QuickStaff.API/QuickStaff.API.csproj"

# Copier le reste du code
COPY . .

# Build
WORKDIR "/src/QuickStaff.API"
RUN dotnet build "QuickStaff.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "QuickStaff.API.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Générer et configurer le certificat HTTPS
RUN apt-get update && \
    apt-get install -y openssl && \
    openssl req -x509 -nodes -days 365 -newkey rsa:2048 \
    -keyout /app/cert.key -out /app/cert.crt \
    -subj "/CN=localhost" && \
    openssl pkcs12 -export -out /app/cert.pfx \
    -inkey /app/cert.key -in /app/cert.crt -password pass:YourSecurePassword

# Variables d'environnement pour le certificat
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/cert.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=YourSecurePassword
ENV ASPNETCORE_URLS=https://+:443;http://+:80

ENTRYPOINT ["dotnet", "QuickStaff.API.dll"]