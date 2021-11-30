#r "paket:
nuget Fake.DotNet.Cli
nuget Fake.Core.Target //
nuget DbUp-PostgreSQL"
#load "./.fake/dev.fsx/intellisense.fsx"

open Fake.DotNet
open Fake.Core
open Fake.Core.TargetOperators
open DbUp

let sdk =
    lazy (DotNet.install DotNet.Versions.FromGlobalJson)

let inline dotnetSimple args = DotNet.Options.lift sdk.Value args

// Targets
Target.create
    "dev:restore"
    (fun _ ->
        Trace.trace "Installing deps"
        DotNet.exec dotnetSimple "restore" "" |> ignore)

Target.create
    "dev:db"
    (fun _ ->
        Trace.trace "Running DB Migrations"

        let result: Engine.DatabaseUpgradeResult =
            DeployChanges
                .To
                .PostgresqlDatabase("Host=db;Port=5432;User ID=postgres;Password=insecuredbpassword;Database=localdb")
                .WithScriptsFromFileSystem(@"migrations")
                .LogToConsole()
                .Build()
                .PerformUpgrade()

        match result with
        | r when r.Successful = false -> failwith r.Error.Message
        | _ -> result |> ignore)

Target.create
    "dev:watch"
    (fun _ ->
        Trace.trace "Watching project..."

        DotNet.exec dotnetSimple "watch" "-p ./src/ConvenientGiraffeApiTemplate/ConvenientGiraffeApiTemplate.fsproj run"
        |> ignore)

"dev:restore"
==> "dev:db"
==> "dev:watch"

// Start Dev Task
Target.runOrDefault "dev:watch"
