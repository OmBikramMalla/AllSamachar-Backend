FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["AllSamachar.Api/AllSamachar.Api.csproj", "AllSamachar.Api/"]
COPY ["AllSamachar.Application/AllSamachar.Application.csproj", "AllSamachar.Application/"]
COPY ["AllSamachar.Infrastructure/AllSamachar.Infrastructure.csproj", "AllSamachar.Infrastructure/"]
COPY ["AllSamachar.Domain/AllSamachar.Domain.csproj", "AllSamachar.Domain/"]
RUN dotnet restore "AllSamachar.Api/AllSamachar.Api.csproj"

COPY . .
WORKDIR "/src/AllSamachar.Api"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "AllSamachar.Api.dll"]