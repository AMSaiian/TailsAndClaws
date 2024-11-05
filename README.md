TailsAndClaws
--------

## To run with docker (recommended)

You have to clarify that your machine has got next software:

- Docker
- Docker Compose
- Git

### Steps:

1. To clone this repository, run the following command:

```bash
git clone https://github.com/AMSaiian/TailsAndClaws.git
```

2. Navigate to the repository backend source code folder

``` bash
cd TailsAndClaws
```

3. Copy .env.example as .env at the same folder - it must be exactly near *docker.compose.yml*. All generic environment
   variables have been configured, however you can change it with your own
4. Run predefined `docker-compose` script:

``` bash
docker-compose up --build
```

## To run locally

You have to clarify that your machine has got next software:

- .NET Core 8
- PostgresSQL 16
- Git

### Steps:

1. To clone this repository, run the following command:

```bash
git clone https://github.com/AMSaiian/TailsAndClaws.git
```

2. Navigate to the repository backend source code folder

``` bash
cd TailsAndClaws
```

3. Fill next placeholders in configuration using appsetting.json or .NET user secrets:

- `ConnectionStrings -> Application`

4. Run next commands using Powershell or Command line:

```bash
dotnet restore TailsAndClaws.sln
dotnet build TailsAndClaws.sln --configuration Release --no-restore
dotnet run --project src/TailsAndClaws/TailsAndClaws.csproj --no-build
```

## Since now you can access backend using next URIs (if configuration hasn't been changed):

- Swagger UI (HTTPS): https://localhost/swagger/index.html (with conventional HTTPS port 443)
- Swagger UI (HTTP): http://localhost/swagger/index.html (with conventional HTTP port 80) !!! due to Swagger UI can't
  handle redirect, use HTTPS
- The PostgreSQL database is accessible on port 5433 (using Docker run) or on port 5432 (using local run)

------------

## Generic user flow:
1. You can get paginated, ordered list of dogs using *api/dogs* `GET`-endpoint:
   By default it paginated with amount provided in configuration. Using query params you can define by what property
   dogs will be sorted; amount of dogs; direction of order. Available order properties:
    - name
    - color
    - weight
    - tail_length
2. You can create a new dog using api/dogs `POST`-endpoint providing next json body payload: 
`
{
  // required, not empty
  "name": "DogName",
  // required, not empty
  "color": "SomeColor",
  // required, greater than zero
  "tail_length_in_meters": 1, 
  // required, greater than zero
  "weight_in_kg": 1
}
`
3. You can fetch welcome message using api/ping `GET`-endpoint

#### P.S. All endpoints configured with rate limiting. 
------------

## Solution Setup

### Architecture - Clean Architecture

### Storage - EF Core (Code First) via PostgresSQL

### Application - CQRS with MediatoR

### Unit - xUnit using Moq, Fluent Assertions, Bogus
------------

## Implemented:

- #### Cross-cuttings:

1. Structured logging using Serilog
2. Validation pipelines
3. Errors and exceptions handlers/filters
4. Enhanced solution structure divided on necessary assemblies
5. Enhanced configuration observability and its DI for customizing the app behaviour at different layers

- #### Testing

1. Unit testing using InMemory DbContext and Moq Mocks. Most non-trivial logic was covered with unit-tests

## Q&A

1. If you encounter issues with usage of default 80 and 443 ports at `docker compose up` - just change `APP_HTTP_PORT`
   and `APP_HTTPS_PORT` ports with free ports at your machine
2. Don't pay attention to application exceptions, related to error with connection to Postgres DB, during first start.
   This is not issue due to during first start there is init of Postgres volume and other heavy processes,
   which can make Db container unavailable during some time. Docker Compose of project has been designed to handle this
   situation with `"Restart always"`, so just wait some time and then application is going to be ready to work properly

## Possible enhancements

1. Considering the controversial nature of the EF Core usage, these project ignores *Repository pattern* wrapper for EF
   DbContext for simplicity and straightforwardness, however author doesn't refuse *Repository pattern* usage
   possibility and agrees implementing and maintaining it
2. Considering usage of *DDD principles* in this project, it would be nice to migrate from *Anemic domain model* *Rich
   domain model* by implementing domain services. However, this step has been skipped for simplicity
