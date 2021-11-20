module Matcher

let cleanWith whitelist title =
    whitelist
    |> Map.tryFind title
    |> Option.defaultValue title

let findMatch
    (game: Grouvee.GrouveeGame)
    (searchResponses: HowLongToBeatParsing.SearchResult seq)
    : HowLongToBeatParsing.SearchResult =

    let closestMatch = searchResponses |> Seq.head
    closestMatch
