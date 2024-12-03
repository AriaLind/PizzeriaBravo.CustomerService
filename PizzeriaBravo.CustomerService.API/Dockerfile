FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /App

COPY . ./

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App

# Copy the app from the build stage
COPY --from=build-env /App/out ./

EXPOSE 3002
ENV ASPNETCORE_URLS=http://+:3002
ENV MSSQL_SA_PASSWORD=Password1*

# Ensure dotnet application runs as the main foreground process
ENTRYPOINT ["dotnet", "PizzeriaBravo.CustomerService.API.dll"]
