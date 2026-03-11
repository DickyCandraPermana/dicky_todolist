FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY backend/dicky_todolist/*.csproj backend/dicky_todolist/

RUN dotnet restore backend/dicky_todolist/dicky_todolist.csproj

COPY backend/. backend/
WORKDIR /src/backend/dicky_todolist
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5170
ENTRYPOINT ["dotnet", "dicky_todolist.dll"]
