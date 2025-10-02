FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY bidding_system.sln ./

COPY src/BiddingService/BiddingService.csproj ./src/BiddingService/
COPY src/Contracts/Contracts.csproj ./src/Contracts/

RUN dotnet restore ./src/BiddingService/BiddingService.csproj

COPY src/BiddingService/. ./src/BiddingService/
COPY src/Contracts/. ./src/Contracts/

RUN dotnet publish ./src/BiddingService/BiddingService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet" , "BiddingService.dll" ]

