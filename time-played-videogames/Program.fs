module Main

[<EntryPoint>]
let main argv =
    let grouveeCsvPath = argv.[0]
    let games = Grouvee.parseFile grouveeCsvPath

    let finishedGames =
        games
        |> List.filter (fun g -> g.Shelf = Grouvee.Shelf.Played)

    printfn "Wow, you completed %d games! You badass!" (finishedGames |> Seq.length)
    0
