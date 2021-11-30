# Convenient Giraffe API Template

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

# Stack Documentation

A [Giraffe](https://github.com/giraffe-fsharp/Giraffe) web application, which has been created via the `dotnet new giraffe` command.

## Build and test the application

### Windows

Run the `build.bat` script in order to restore, build and test (if you've selected to include tests) the application:

```
> ./build.bat
```

### Linux/macOS

Run the `build.sh` script in order to restore, build and test (if you've selected to include tests) the application:

```
$ ./build.sh
```

## Run the application

After a successful build you can start the web application by executing the following command in your terminal:

```
dotnet run -p src/ConvenientGiraffeApiTemplate
```

After the application has started visit [http://localhost:5000](http://localhost:5000) in your preferred browser.