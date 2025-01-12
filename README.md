# discount-codes

## Description

This project is a simple API that allows the creation of discount codes and the application of these codes to a shopping cart.
It's using gRPC and protobuf to define the API and the messages exchanged between the client and the server.
The server is using EF Core to interact with a MSSQL database.

## Running

### docker-compose

In order to run the project using provided `docker compose` file, you need to have docker and docker-compose installed on your machine.
After that, you can run the following command:

```bash
docker-compose up -d
```

This command will build the images and start the containers. The API will be available at `http://localhost:5001`.
Database has no external ports exposed, so it will only be accessible from the API container.

### With existing DB

If you want to run the project using an existing db, you can use create a `appsettings.Local.json` file and update the connection string.
You can set the migration flag in settings to `true` or migrate the database first.
You can either use the `dotnet` CLI or Visual Studio to run the project.

## Clients

Two simple clients are provided. One is called `Discounts.Client` and it only generates 10 codes and uses them.
Another client is called `Discounts.PerformanceTests`, which calls the API to test its performance.
