# Usar a imagem leve baseada no Alpine Linux para a runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 5000

# Usar o SDK do .NET no Alpine para compilar a aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar os arquivos de projeto para o container
COPY src/LibraSoft.Api/LibraSoft.Api.csproj src/LibraSoft.Api/
COPY src/LibraSoft.Core/LibraSoft.Core.csproj src/LibraSoft.Core/

# Restaurar as dependências
RUN dotnet restore "src/LibraSoft.Api/LibraSoft.Api.csproj"

# Copiar o código restante
COPY . .

# Compilar a aplicação
WORKDIR /src/src/LibraSoft.Api
RUN dotnet build "LibraSoft.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar a aplicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LibraSoft.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Criar a imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Initialize
CMD ASPNETCORE_URLS="http://*:$PORT" dotnet LibraSoft.Api.dll

