# Convenient Giraffe API Template

*WARNING: This is a work in progress. Check the TODO section to know what is missing.*

A convenient template project to quickly get started with a Giraffe API in F#. It includes:

- Docker & Docker Compose
- FAKE
- PostgreSQL
- DB Migrations
- Formatting

*This project has only been tested in Windows with a POSIX command line*

## Dependencies

You must have .Net SDK 5, Docker and a command line tool to use this template.

## How to use

Clone the project then rename all instances of `ConvenientGiraffeApiTemplate` to the name of 
your project. Modify the `dev.fsx` to point to the project you want to watch.

### Restore Tools & Dependencies

Run `dotnet tool restore` to install the tools. Then `dotnet restore` to install dependencies.

### Run It

To start the services run `docker compose -f docker-compose.dev.yml up` or 
`docker compose -f docker-compose.dev.yml up --build` to rebuild the images.

## TODO

- Fix and enhance testing project
- Add convenient infrastructure setup for AWS
- Add prod FAKE, Docker and DB configs

## Stack Documentation

- [F#](https://fsharp.org/)
- [Giraffe](https://github.com/giraffe-fsharp/Giraffe)
- [FAKE](https://fake.build/fake-gettingstarted.html)
- [Docker](https://docs.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [Fantomas](https://github.com/fsprojects/fantomas)
- [PostgreSQL](https://www.postgresql.org/)
- [DbUp](https://dbup.github.io/)
- [SQLProvider](https://fsprojects.github.io/SQLProvider/)