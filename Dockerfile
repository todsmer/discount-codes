FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY Discounts/Discounts.csproj ./Discounts/
RUN dotnet restore ./Discounts/Discounts.csproj

COPY . ./

RUN dotnet build ./Discounts/Discounts.csproj --configuration Release
RUN dotnet test ./Discounts/Discounts.csproj --verbosity normal
RUN dotnet publish ./Discounts/Discounts.csproj --configuration Release --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 5001

ENTRYPOINT ["dotnet", "Discounts.dll"]
