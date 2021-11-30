namespace ConvenientGiraffeApiTemplate

module Models =

    open System.Text.Json.Serialization
    open System

    [<CLIMutable>]
    type AppError = { Msg: string }

    [<CLIMutable>]
    type AppSuccess<'T> = { data: 'T }

    [<CLIMutable>]
    [<JsonFSharpConverter>]
    type Message =
        { Id: Guid
          Content: string
          DateCreated: Option<DateTime>
          DateDeleted: Option<DateTime> }

module Factories =
    let mkAppResp<'T> (data: 'T) : Models.AppSuccess<'T> = { data = data }
    let mkAppErr (msg: string) : Models.AppError = { Msg = msg }
