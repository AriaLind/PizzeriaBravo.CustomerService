# Use the official SQL Server image
FROM mcr.microsoft.com/mssql/server:latest AS sqlserver
ENV ACCEPT_EULA=Y
ENV MSSQL_PID='Developer'
# ENV MSSQL_SA_PASSWORD=Password1*

# Add optional environment variable for database name
ENV MSSQL_DBNAME=PizzeriaBravo.CustomerService