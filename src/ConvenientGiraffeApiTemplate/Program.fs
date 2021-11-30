module ConvenientGiraffeApiTemplate.App

open System
open System.Text.Json.Serialization
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection
open Giraffe
open ConvenientGiraffeApiTemplate.HttpHandlers

// ---------------------------------
// Web app
// ---------------------------------
let defaultHost = "http://localhost:3000"
let hosts = [| defaultHost |]

let (|AllowedHost|_|) origin =
    origin
    |> Option.bind (fun (url: string) -> Array.tryFind (fun url' -> url' = url) hosts)

let setCorsHeaders: HttpHandler =
    (fun (next: HttpFunc) (ctx: HttpContext) ->
        match ctx.TryGetRequestHeader("Origin") with
        | AllowedHost origin -> origin
        | _ -> defaultHost
        |> (fun o -> ctx.SetHttpHeader("Access-Control-Allow-Origin", o))

        next ctx)
    >=> setHttpHeader "Access-Control-Allow-Methods" "POST, OPTIONS"
    >=> setHttpHeader "Access-Control-Allow-Headers" "Content-Type"

let webApp =
    choose [ subRoute
                 "/api"
                 (choose [ GET
                           >=> choose [ route "/messages" >=> handleGetMessages ] ])
             setStatusCode 404
             >=> json (Factories.mkAppErr "Not Found") ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(ex, "An unhandled exception has occurred while executing the request.")

    clearResponse
    >=> setStatusCode 500
    >=> json (Factories.mkAppErr ex.Message)

// ---------------------------------
// Config and Main
// ---------------------------------

let configureCors (builder: CorsPolicyBuilder) =
    builder
        .WithOrigins(hosts)
        .AllowAnyMethod()
        .AllowAnyHeader()
    |> ignore

let configureApp (app: IApplicationBuilder) =
    app
        .UseGiraffeErrorHandler(errorHandler)
        .UseHttpsRedirection()
        .UseCors(configureCors)
        .UseGiraffe(webApp)

let configureServices (ctx: WebHostBuilderContext) (services: IServiceCollection) =
    services.AddCors() |> ignore
    services.AddGiraffe() |> ignore

    let jsonOptions = SystemTextJson.Serializer.DefaultOptions

    JsonFSharpConverter(
        unionEncoding =
            (JsonUnionEncoding.Untagged
             ||| JsonUnionEncoding.UnwrapOption
             ||| JsonUnionEncoding.UnwrapFieldlessTags
             ||| JsonUnionEncoding.UnwrapRecordCases)
    )
    |> jsonOptions.Converters.Add

    services.AddSingleton<Json.ISerializer>(SystemTextJson.Serializer(jsonOptions))
    |> ignore

    services.AddSingleton<Connection.Sql.dataContext>
        (fun (services: IServiceProvider) ->
            let settings =
                ctx
                    .Configuration
                    .GetSection("DataSettings")
                    .Get<Connection.DatabaseSettings>()

            Connection.mkDbCtx (settings))
    |> ignore

    services.AddScoped<Contracts.MessagesRepository>
        (fun (services: IServiceProvider) ->
            let ctx =
                services.GetService<Connection.Sql.dataContext>()

            Repositories.makeMessagesRepository (ctx))
    |> ignore

let configureLogging (builder: ILoggingBuilder) =
    builder.AddConsole().AddDebug() |> ignore

let configureAppConfig (ctx: WebHostBuilderContext) (builder: IConfigurationBuilder) =
    builder
        .SetBasePath(ctx.HostingEnvironment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{ctx.HostingEnvironment.EnvironmentName}.json", true, true)
    |> ignore

[<EntryPoint>]
let main args =
    Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(fun webHostBuilder ->
            webHostBuilder
                .Configure(Action<IApplicationBuilder> configureApp)
                .ConfigureAppConfiguration(configureAppConfig)
                .ConfigureServices(configureServices)
                .ConfigureLogging(configureLogging)
            |> ignore)
        .Build()
        .Run()

    0
