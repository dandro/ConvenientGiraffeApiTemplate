namespace ConvenientGiraffeApiTemplate

module Connection =
    open FSharp.Data.Sql

#if DOCKER
    [<Literal>]
    let typeProviderConnStr =
        "Host=db;Port=5432;User ID=postgres;Password=insecuredbpassword;Database=localdb"
#else
    [<Literal>]
    let typeProviderConnStr =
        "Host=localhost;Port=5432;User ID=postgres;Password=insecuredbpassword;Database=localdb"
#endif
    type Sql = SqlDataProvider<Common.DatabaseProviderTypes.POSTGRESQL, typeProviderConnStr, UseOptionTypes=true>

    type DatabaseSettings() =
        member val ConnectionString: string = "" with get, set

    let mkDbCtx (settings: DatabaseSettings) =
        Sql.GetDataContext(settings.ConnectionString)

module Repositories =
    let toDomainMessage (data: Connection.Sql.dataContext.``public.messagesEntity``) : Models.Message =
        { Id = data.Id
          Content = data.Content
          DateCreated = data.Datecreated
          DateDeleted = data.Datedeleted }

    let makeMessagesRepository (ctx: Connection.Sql.dataContext) =
        { new Contracts.MessagesRepository with
            member this.GetMessages() =
                query {
                    for message in ctx.Public.Messages do
                        where (message.Datedeleted.IsNone)
                        select (message)
                }
                |> Seq.map toDomainMessage
                |> Seq.toList

            member this.CreateMessage(data) = failwith "Not implemented" }
