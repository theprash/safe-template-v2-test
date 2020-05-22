open System.IO

open FSharp.Control.Tasks.V2
open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Giraffe
open Saturn

open Shared

let getEnvVar (name: string) (defaultValue: string) =
    System.Environment.GetEnvironmentVariable name
    |> function
    | null
    | "" -> defaultValue
    | x -> x

let publicPath = getEnvVar "public_path" "../Client/public" |> Path.GetFullPath

let counterApi = { GetInitialCounter = fun () -> async { return { Value = 42 } } }

let webApp =
    Remoting.createApi()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue counterApi
    |> Remoting.buildHttpHandler

let app =
    application {
        url "http://0.0.0.0:8085/"
        use_router webApp
        memory_cache
        use_static publicPath
        use_json_serializer (Thoth.Json.Giraffe.ThothSerializer())
        use_gzip
    }

run app
