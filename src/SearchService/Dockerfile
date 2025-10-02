FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY bidding_system.sln ./

COPY src/SearchService/SearchService.csproj ./src/SearchService/
COPY src/Contracts/Contracts.csproj ./src/Contracts/
COPY src/BiddingService/BiddingService.csproj ./src/BiddingService/

RUN dotnet restore ./src/SearchService/SearchService.csproj

COPY src/SearchService/. ./src/SearchService/
COPY src/Contracts/. ./src/Contracts/
COPY src/BiddingService/. ./src/BiddingService/


RUN dotnet publish ./src/SearchService/SearchService.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

EXPOSE 80
COPY --from=build /app/publish .
ENTRYPOINT [ "dotnet" , "SearchService.dll" ]

