module Main

[<EntryPoint>]
let main argv =
    async {
        let grouveeCsvPath = argv.[0]
        let games = Grouvee.parseFile grouveeCsvPath

        let finishedGames =
            games
            |> List.filter (fun g -> g.Shelf = Grouvee.Shelf.Played)

        let first = finishedGames |> Seq.item 1

        let! html = first.Title |> HowLongToBeat.getHtml
        let searchResult = html |> HowLongToBeat.parseSearchResult


        printfn "Search result: %A" searchResult

        printfn "Wow, you completed %d games! You badass!" (finishedGames |> Seq.length)
        return 0
    }
    |> Async.RunSynchronously
