FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /App

COPY . ./

# RUN chmod +x wait-for-it.sh

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App

COPY --from=build-env /App/out ./

EXPOSE 3002
ENV ASPNETCORE_URLS=http://+:3002

ENTRYPOINT ["dotnet", "PizzeriaBravo.CustomerService.API.dll"]