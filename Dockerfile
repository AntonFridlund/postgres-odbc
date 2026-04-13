FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

COPY ./src ./src
WORKDIR /src
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0 AS final

RUN apt-get update && \
  apt-get install -y --no-install-recommends \
  unixodbc odbc-postgresql && rm -rf /var/lib/apt/lists/*

RUN odbcinst -q -d | grep -q "PostgreSQL Unicode" || exit 1

RUN useradd -m appuser
COPY --from=build --chown=appuser:appuser /app/publish/odbc-api /app/odbc-api
WORKDIR /app
USER appuser

ENTRYPOINT ["./odbc-api"]
