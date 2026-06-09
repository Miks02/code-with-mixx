
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

RUN apt-get update && \
    apt-get install -y --no-install-recommends curl && \
    curl -fsSL https://deb.nodesource.com/setup_22.x | bash - && \
    apt-get install -y --no-install-recommends nodejs && \
    apt-get clean && rm -rf /var/lib/apt/lists/*

WORKDIR /src

COPY CodeWithMixx.sln ./
COPY CodeWithMixx/CodeWithMixx.csproj ./CodeWithMixx/
RUN dotnet restore ./CodeWithMixx/CodeWithMixx.csproj

COPY CodeWithMixx/package.json CodeWithMixx/package-lock.json ./CodeWithMixx/
RUN cd CodeWithMixx && npm ci

COPY . .
RUN dotnet publish ./CodeWithMixx/CodeWithMixx.csproj \
        -c Release \
        -o /app/publish \
        --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

RUN addgroup --system --gid 1001 appgroup && \
    adduser  --system --uid 1001 --ingroup appgroup --no-create-home appuser

COPY --from=build /app/publish ./
RUN chown -R appuser:appgroup /app

USER appuser

ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 10000
CMD dotnet CodeWithMixx.dll --urls "http://+:${PORT:-10000}"
