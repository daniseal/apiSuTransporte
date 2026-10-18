# Build from the repository root. Runtime configuration is supplied through environment variables.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "apiSuTransporte/apiSuTransporte.csproj"
RUN dotnet publish "apiSuTransporte/apiSuTransporte.csproj" -c Release -o /app/publish --no-restore /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
USER $APP_UID
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "apiSuTransporte.dll"]
