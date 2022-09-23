FROM mcr.microsoft.com/dotnet/sdk:6.0 as RESTORE
WORKDIR /app
COPY LootbeersBackend.sln ./
COPY .config ./.config
COPY LootbeersBackend/LootbeersBackend.csproj ./LootbeersBackend/LootbeersBackend.csproj
COPY LootbeersBackend.Anchorgen/LootbeersBackend.Anchorgen.csproj ./LootbeersBackend.Anchorgen/LootbeersBackend.Anchorgen.csproj
COPY LootbeersBackend.Tests/LootbeersBackend.Tests.csproj ./LootbeersBackend.Tests/LootbeersBackend.Tests.csproj

RUN dotnet restore
RUN dotnet tool restore

FROM RESTORE AS BUILD

COPY LootbeersBackend ./LootbeersBackend
COPY LootbeersBackend.Anchorgen ./LootbeersBackend.Anchorgen
COPY LootbeersBackend.Tests ./LootbeersBackend.Tests

RUN dotnet build -c release

FROM BUILD AS TEST
ARG postgres_password
ARG admin_token
ARG reward_authority

ENV PostgresPassword=$postgres_password
ENV Administration__Token=$admin_token
ENV LootboxOpenerClient__RewardAuthority=$reward_authority

RUN dotnet test --no-restore --no-build

FROM BUILD AS PUBLISH
RUN dotnet publish -c release -o /publish LootbeersBackend

FROM mcr.microsoft.com/dotnet/aspnet:6.0 as RUNTIME
ARG postgres_password
ARG admin_token
ARG reward_authority

ENV PostgresPassword=$postgres_password
ENV Administration__Token=$admin_token
ENV LootboxOpenerClient__RewardAuthority=$reward_authority

WORKDIR /app
COPY --from=PUBLISH /publish ./
EXPOSE 80
ENTRYPOINT ["dotnet", "LootbeersBackend.dll"]
