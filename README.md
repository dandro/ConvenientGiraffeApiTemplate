# Convenient Giraffe API Template

A convenient template project to quickly get started with a Giraffe API in F#. It includes:

- Docker & Docker Compose
- FAKE
- PostgreSQL
- DB Migrations
- Formatting

*This project has only been tested in Windows with a POSIX command line*

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