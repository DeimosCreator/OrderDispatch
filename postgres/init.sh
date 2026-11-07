#!/bin/bash
set -e

for db in "$ORDER_DB" "$KITCHEN_DB" "$DELIVERY_DB" "$ANALYTICS_DB"; do
  if [ -n "$db" ]; then
    echo "Creating database: $db"
    psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
        CREATE DATABASE "$db";
EOSQL
  fi
done