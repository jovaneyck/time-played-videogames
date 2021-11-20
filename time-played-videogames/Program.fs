module Main

open FSharpx.Control

let clean (title: string) =
    let whitelist =
        [ ("Pokémon: Let's Go, Pikachu!/Eevee!", "Pokémon: Let's Go, Pikachu! and Let's Go, Eevee!")
          ("Pokémon X/Y", "Pokémon X and Y")
          ("Pokémon Sword/Shield", "Pokémon Sword and Shield")
          ("Red Dead Redemption II", "Red Dead Redemption 2")
          ("Danganronpa 1•2 Reload", "Danganronpa 1 & 2 Reload")
          ("Pokémon Gold/Silver", "Pokémon Gold and Silver")
          ("Pokémon Sword/Shield", "Pokémon Sword and Shield")
          ("Pokémon Red/Blue", "Pokémon Red and Blue")
          ("Phoenix Wright: Ace Attorney – Spirit of Justice", "Phoenix Wright: Ace Attorney Spirit of Justice")
          ("The Walking Dead: Season Two", "The Walking Dead: Season 2")
          ("Xenoblade Chronicles 2: Torna - The Golden Country", "Xenoblade Chronicles 2: Torna ~ The Golden Country")
          ("Mario + Rabbids: Kingdom Battle", "Mario + Rabbids Kingdom Battle")
          ("Abzû", "ABZU") ]
        |> Map.ofList

    Matcher.cleanWith whitelist title

[<EntryPoint>]
let main argv =
    let grouveeCsvPath = argv.[0]
    let games = Grouvee.parseFile grouveeCsvPath

    let finishedGames =
        games
        |> List.filter (fun g -> g.Shelf = Grouvee.Shelf.Played)
        |> List.take 10

    let responses =
        finishedGames
        |> List.map
            (fun game ->
                async {
                    printfn "Querying HLTB for %s" game.Title
                    let! result = game.Title |> clean |> HowLongToBeatHttp.getHtml
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

    printf
        $"Wow, you completed {(finishedGames |> Seq.length)} games! You spent {totalTimes} hours playing videogames. Wow."

    0
