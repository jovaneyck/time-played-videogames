#r "nuget: Unquote"
#r "nuget: xunit"
#r "nuget: FSharp.Data"
#r "nuget: FSharpx.Async"

#load "Grouvee.fs"
#load "HowLongToBeatHttp.fs"
#load "HowLongToBeatParsing.fs"
#load "Matcher.fs"

open FSharp.Data
open Swensen.Unquote
open Xunit
open FSharpx.Control

let grouveeCsvPath =
    @"C:\Users\Jo.VanEyck\source\repos\time-played-videogames\time-played-videogames\data\praGmatic_28044_grouvee_export.csv"

let games = Grouvee.parseFile grouveeCsvPath

let finishedGames =
    games
    |> List.filter (fun g -> g.Shelf = Grouvee.Shelf.Played)

finishedGames |> Seq.length

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

let parsed =
    responses
    |> Seq.map (fun (req, resp) -> req, HowLongToBeatParsing.parseSearchResult resp)
    |> Seq.toList

parsed |> List.map Matcher.findMatch |> ignore
