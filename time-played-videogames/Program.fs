module Main

[<EntryPoint>]
let main argv =
    let grouveeCsvPath = argv.[0]
    let games = Grouvee.parseFile grouveeCsvPath

    let finishedGames =
        games
        |> List.filter (fun g -> g.Shelf = Grouvee.Shelf.Played)

    let first = finishedGames |> Seq.head

    let searchResult =
        first.Title
        |> HowLongToBeat.getHtml
        |> HowLongToBeat.parseSearchResult

    printfn "Search result: %A" searchResult

    printfn "Wow, you completed %d games! You badass!" (finishedGames |> Seq.length)
    0
