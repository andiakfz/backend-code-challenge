# Port Locator (AE Backend Code Challenge)

This code is a solution for AE Backend Code Challenge, developed using .NET 8 and a basic docker support.

## Installation

Clone this project and navigate to the repo directory
```bash
git clone "https://github.com/andiakfz/backend-code-challenge"
```

Install Docker: <br>
Docker Desktop => https://www.docker.com/ <br>
Docker Engine => https://docs.docker.com/engine/install/

## Guide

Navigate to the clone directory and run docker compose to build the application
```cmd
cd //directory

docker compose  -f "docker-compose.yml" -p portlocator --ansi never up -d --build --remove-orphans
```
Once docker compose is finished, verify that the application is running: <br>

Docker Desktop:
![Docker Desktop](/assets/docker-desktop-portlocator.jpg "Port Locator Docker")

Application is running on port 5000 <br>
Database is running on port 5432 <br>

![Api](/assets/api-portlocator.jpg "Port Locator Api")

## Testing

Unit test use a dedicated database for testing and could only be run in local machine. The test database could be run on docker on port 5400.

Execute Create Extension Script first on the test database
```sql
CREATE EXTENSION IF NOT EXISTS "uuid-ossp"
```

To run the unit test, set the connection in testsettings.json of Test Project to localhost and use port 4000 <br>

run dotnet test

```cmd
cd //project directory

dotnet test test/portlocator.Tests/portlocator.Tests.csproj --verbosity normal
```

## Running in Local

Change the appsetting connection string to localhost
```json
{
    "ConnectionStrings":{
        "Database": "Host=localhost;Port=5432;Database=dbname;Username=postgres;Password=postgres;Include Error Detail=true",
    }
}
```

Make sure postgres is installed on your system or deploy only the database service using docker compose
