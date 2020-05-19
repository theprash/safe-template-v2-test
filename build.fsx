#r "paket: groupref build //"
#load "./.fake/build.fsx/intellisense.fsx"

#r "netstandard"

open System

open Fake.Core
open Fake.DotNet
open Fake.IO

Target.initEnvironment ()

let serverPath = Path.getFullName "./src/Server"
let clientPath = Path.getFullName "./src/Client"
let clientDeployPath = Path.combine clientPath "deploy"
let deployDir = Path.getFullName "./deploy"

let release = ReleaseNotes.load "RELEASE_NOTES.md"

let platformTool tool winTool =
    let tool = if Environment.isUnix then tool else winTool
    match ProcessUtils.tryFindFileOnPath tool with
    | Some t -> t
    | _ ->
        let errorMsg =
            tool + " was not found in path. " +
            "Please install it and make sure it's available from your path. " +
            "See https://safe-stack.github.io/docs/quickstart/#install-pre-requisites for more info"
        failwith errorMsg

let nodeTool = platformTool "node" "node.exe"
