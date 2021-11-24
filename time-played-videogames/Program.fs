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
    let finishedGames, tally = App.run clean grouveeCsvPath

    printf $"Wow, you completed {(finishedGames |> Seq.length)} games! You spent {0} hours playing videogames. Wow."

    0
