#!/bin/sh
set -eu

if [ -x /opt/mssql-tools18/bin/sqlcmd ]; then
  SQLCMD="/opt/mssql-tools18/bin/sqlcmd"
elif [ -x /opt/mssql-tools/bin/sqlcmd ]; then
  SQLCMD="/opt/mssql-tools/bin/sqlcmd"
else
  echo "sqlcmd was not found in the tools container." >&2
  exit 1
fi

DB_HOST="${DB_HOST:-db}"
DB_PORT="${DB_PORT:-1433}"
DB_NAME="${DB_NAME:-DeviceManagementDB}"
SA_PASSWORD="${SA_PASSWORD:?SA_PASSWORD is required}"
SEED_SAMPLE_DATA="${SEED_SAMPLE_DATA:-true}"

echo "Waiting for SQL Server at ${DB_HOST},${DB_PORT}..."
until "${SQLCMD}" -S "${DB_HOST},${DB_PORT}" -U sa -P "${SA_PASSWORD}" -C -Q "SELECT 1" >/dev/null 2>&1; do
  sleep 2
done

echo "Creating schema if needed..."
"${SQLCMD}" -S "${DB_HOST},${DB_PORT}" -U sa -P "${SA_PASSWORD}" -d master -C -b -i /scripts/01_create_database.sql

echo "Waiting for DeviceManagementDB to come online..."
until [ "$("${SQLCMD}" -S "${DB_HOST},${DB_PORT}" -U sa -P "${SA_PASSWORD}" -d master -h -1 -W -Q "SET NOCOUNT ON; SELECT state_desc FROM sys.databases WHERE name = 'DeviceManagementDB';" | tr -d '\r' | tail -n 1)" = "ONLINE" ]; do
  sleep 2
done

echo "Creating tables if needed..."
"${SQLCMD}" -S "${DB_HOST},${DB_PORT}" -U sa -P "${SA_PASSWORD}" -d "${DB_NAME}" -C -b -i /scripts/02_create_tables.sql

if [ "${SEED_SAMPLE_DATA}" = "true" ]; then
  USER_COUNT="$("${SQLCMD}" -S "${DB_HOST},${DB_PORT}" -U sa -P "${SA_PASSWORD}" -C -d "${DB_NAME}" -h -1 -W -Q "SET NOCOUNT ON; SELECT COUNT(*) FROM Users;" | tr -d '\r' | tail -n 1)"

  if [ "${USER_COUNT}" = "0" ]; then
    echo "Seeding sample data..."
    "${SQLCMD}" -S "${DB_HOST},${DB_PORT}" -U sa -P "${SA_PASSWORD}" -C -b -i /scripts/03_populate_database.sql
  else
    echo "Skipping seed because Users already contains data."
  fi
else
  echo "Sample data seeding disabled."
fi

echo "Database initialization complete."
