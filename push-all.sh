#!/bin/bash
set -e

USERNAME="keorapetse159"
SERVICES=("auction-svc" "search-svc" "identity-svc" "gateway-svc" "bid-svc" "notify-svc" "web-app")

docker compose build

for SERVICE in "${SERVICES[@]}"; do
  docker tag ${SERVICE}:latest ${USERNAME}/${SERVICE}:latest
  docker push ${USERNAME}/${SERVICE}:latest
done
