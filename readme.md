banco de dados SQL SERVER em DOCKER
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SenhaForte123!" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
