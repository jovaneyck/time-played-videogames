#r "nuget: Unquote"
#r "nuget: xunit"
#r "nuget: FSharp.Data"
#r "nuget: FSharpx.Async"

#load "Grouvee.fs"
#load "HowLongToBeat.fs"

open FSharp.Data
open Swensen.Unquote
open FSharpx.Control

let filepath =
    @"C:\Users\Jo.VanEyck\source\repos\time-played-videogames\time-played-videogames\data\praGmatic_28044_grouvee_export.csv"

let games = Grouvee.parseFile filepath

let finishedGames =
    games
    |> List.filter (fun g -> g.Shelf = Grouvee.Shelf.Played)

let responses =
    finishedGames
    |> List.map
        (fun game ->
            printfn "Querying HLTB for %s" game.Title
            let result = game.Title |> HowLongToBeat.getHtml
            printfn "Finished querying HLTB for %s" game.Title
            result)
    |> Async.ParallelWithThrottle 10
    |> Async.RunSynchronously
