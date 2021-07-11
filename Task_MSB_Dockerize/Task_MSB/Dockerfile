FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Task_MSB/Task_MSB.csproj", "Task_MSB/"]
RUN dotnet restore "Task_MSB/Task_MSB.csproj"
COPY . .
WORKDIR "/src/Task_MSB"
RUN dotnet build "Task_MSB.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Task_MSB.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Task_MSB.dll"]