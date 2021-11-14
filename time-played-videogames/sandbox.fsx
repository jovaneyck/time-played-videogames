#r "nuget: Unquote"
#r "nuget: xunit"
#r "nuget: FSharp.Data"
#r "nuget: FSharpx.Async"

#load "Grouvee.fs"
#load "HowLongToBeatHttp.fs"
#load "HowLongToBeatParsing.fs"

open FSharp.Data
open Swensen.Unquote
open FSharpx.Control

let grouveeCsvPath =
    @"C:\Users\Jo.VanEyck\source\repos\time-played-videogames\time-played-videogames\data\praGmatic_28044_grouvee_export.csv"

let games = Grouvee.parseFile grouveeCsvPath

let finishedGames =
    games
    |> List.filter (fun g -> g.Shelf = Grouvee.Shelf.Played)

finishedGames |> Seq.length

let responses =
    finishedGames
    |> List.map
        (fun game ->
            async {
                printfn "Querying HLTB for %s" game.Title
                let! result = game.Title |> HowLongToBeatHttp.getHtml
                printfn "Finished querying HLTB for %s" game.Title
                return result
            })
    |> Async.ParallelWithThrottle 10
    |> Async.RunSynchronously

let parsed =
    responses
    |> Seq.map HowLongToBeatParsing.parseSearchResult
    |> Seq.toList
