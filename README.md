# bidding_system

## Services

1. Auction 
* uses Postgres, asp.net with controllers, efcore, AutoMapper

2. Search
* Uses Mongodb(for quick read-operations), asp.net with controllers, grpc, 

### Dockerize

1. Make sure you run from the root, 
2. use command like this `$ docker build -f src/AuctionService/Dockerfile -t auction-service:latest .`