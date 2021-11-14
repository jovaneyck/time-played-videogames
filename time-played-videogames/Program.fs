﻿module Main

open FSharpx.Control

[<EntryPoint>]
let main argv =
    let grouveeCsvPath = argv.[0]
    let games = Grouvee.parseFile grouveeCsvPath

    let finishedGames =
        games
        |> List.filter (fun g -> g.Shelf = Grouvee.Shelf.Played)

    let responses =
        finishedGames
        |> List.map
            (fun game ->
                async {
                    printfn "Querying HLTB for %s" game.Title
                    let! result = game.Title |> HowLongToBeat.getHtml
                    printfn "Finished querying HLTB for %s" game.Title
                    return result
                })
        |> Async.ParallelWithThrottle 10
        |> Async.RunSynchronously

    let parsed =
        responses
        |> Seq.map HowLongToBeat.parseSearchResult
        |> Seq.toList

    printfn "Wow, you completed %d games! You badass!" (finishedGames |> Seq.length)
    0
