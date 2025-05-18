FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["LangDiscord/LangDiscord.csproj", "LangDiscord/"]
RUN dotnet restore "LangDiscord/LangDiscord.csproj"

COPY . .
WORKDIR "/src/LangDiscord"
RUN dotnet publish "LangDiscord.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "LangDiscord.dll"]
