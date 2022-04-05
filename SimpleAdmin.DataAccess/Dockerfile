FROM mcr.microsoft.com/mssql/server:2019-latest as build
ENV ACCEPT_EULA="Y"
ENV SA_PASSWORD="Test123$"

WORKDIR /tmp
COPY SimpleAdmin.DataAccess/SimpleAdmin.bak .
COPY SimpleAdmin.DataAccess/DBRestore.sql .

RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" \
 && /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P 'Test123$' -d master -i /tmp/DBRestore.sql

FROM mcr.microsoft.com/mssql/server:2019-latest as relase
ENV ACCEPT_EULA="Y"
ENV SA_PASSWORD="Test123$"

COPY --from=build /var/opt/mssql/data /var/opt/mssql/data




