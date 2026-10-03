FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

COPY ./src ./src
WORKDIR /src
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0 AS final

RUN apt-get update && apt-get install -y --no-install-recommends \
  unixodbc odbc-postgresql && \
  rm -rf /var/lib/apt/lists/* && \
  useradd -m -u 10001 -U appuser

COPY odbcinst.ini /etc/odbcinst.ini
COPY --from=build --chown=appuser:appuser --chmod=500 /app/publish/odbc-api /app/odbc-api

WORKDIR /app
USER appuser
ENTRYPOINT ["./odbc-api"]