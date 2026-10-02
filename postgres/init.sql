
#!/bin/bash
set -e

psql \
  --username "$POSTGRES_USER" \
  --dbname "$POSTGRES_DB" \
  --set=order_db="$ORDER_DB" \
  --set=kitchen_db="$KITCHEN_DB" \
  --set=delivery_db="$DELIVERY_DB" \
  --set=analytics_db="$ANALYTICS_DB" \
  <<'EOSQL'

CREATE DATABASE :"order-db";
CREATE DATABASE :"kitchen-db";
CREATE DATABASE :"delivery-db";
CREATE DATABASE :"analytics-db";

EOSQL