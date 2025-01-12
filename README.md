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
Another client is called `Discounts.PerformanceTests`, which calls the API to test its performance, using the benchmark library.
This project needs to be run in `Release` configuration.

## Assumptions

Due to time constraints, I made some assumptions there not described in the task.

1. I assumed that the discount codes should be returned from the API when generated.
2. I assumed that using a code is only possible once.
3. Due to performance issues, I rely on DB to detect duplicates. Duplicates won't occur very often, due to the number of combinations possible.
But if they do, I've split the number of codes into batches and check each batch for duplicates. In case there are some, I regenerate the batch, up to five times.
4. Docker compose has explicit passwords in the file. In a real-world scenario, the passwords would be stored securely.
5. There's no authorization and authentication in the API. In a real-world scenario, this would be implemented.
6. I've used `int32` instead of `uint32` because it's easier to work with in C#, and both signs are not used in this case.
