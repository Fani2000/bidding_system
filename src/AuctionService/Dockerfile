FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY bidding_system.sln ./

COPY src/AuctionService/AuctionService.csproj ./src/AuctionService/
COPY src/BiddingService/BiddingService.csproj ./src/BiddingService/
COPY src/Contracts/Contracts.csproj ./src/Contracts/

RUN dotnet restore ./src/AuctionService/AuctionService.csproj

COPY src/AuctionService/. ./src/AuctionService/
COPY src/Contracts/. ./src/Contracts/
COPY src/BiddingService/. ./src/BiddingService/

RUN dotnet publish ./src/AuctionService/AuctionService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet" , "AuctionService.dll" ]

