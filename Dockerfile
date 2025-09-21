FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

COPY ./src ./src
WORKDIR /src
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:9.0 AS final

RUN apt-get update && \
  apt-get install -y --no-install-recommends \
    unixodbc=2.3.11-2+deb12u1 \
    odbc-postgresql=1:13.02.0000-2+b1 \
  && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish/odbc-api /usr/local/bin/app
WORKDIR /usr/local/bin/
ENTRYPOINT ["app"]
