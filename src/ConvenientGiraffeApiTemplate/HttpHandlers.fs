namespace ConvenientGiraffeApiTemplate

open System

module Contracts =
    type CreateMessageApiModel = { Content: string }

    type MessageApiModel =
        { Id: Guid
          Content: string
          DateCreated: Option<DateTime> }

    type MessagesRepository =
        abstract GetMessages: unit -> List<Models.Message>
        abstract CreateMessage: CreateMessageApiModel -> Result<string, string>


module HttpHandlers =
    open Microsoft.AspNetCore.Http
    open FSharp.Control.Tasks
    open Giraffe

    let private toMessageApiModel (data: Models.Message) : Contracts.MessageApiModel =
        { Id = data.Id
          Content = data.Content
          DateCreated = data.DateCreated }

    let handleGetMessages =
        fun (next: HttpFunc) (ctx: HttpContext) ->
            task {
                let repository =
                    ctx.GetService<Contracts.MessagesRepository>()

                let messages =
                    repository.GetMessages()
                    |> Seq.map toMessageApiModel

                return! json (Factories.mkAppResp messages) next ctx
            }
