FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AutofacTesting/AutofacTesting.csproj", "AutofacTesting/"]
RUN dotnet restore "AutofacTesting/AutofacTesting.csproj"
COPY . .
WORKDIR "/src/AutofacTesting"
RUN dotnet build "AutofacTesting.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AutofacTesting.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AutofacTesting.dll"]
