module App

open FSharpx.Control

let run cleaner grouveeCsvPath =
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
                    let! result = game.Title |> cleaner |> HowLongToBeatHttp.getHtml
                    printfn "Finished querying HLTB for %s" game.Title
                    return game, result
                })
        |> Async.ParallelWithThrottle 10
        |> Async.RunSynchronously

    let matches =
        responses
        |> Seq.map (fun (game, searchResult) -> (game, HowLongToBeatParsing.parseSearchResult searchResult))
        |> Seq.map (fun (game, searchResults) -> (game, Matcher.findMatch game searchResults))
        |> Seq.toList

    let totalTimes = matches |> List.map snd |> Tally.tally
    finishedGames, totalTimes
