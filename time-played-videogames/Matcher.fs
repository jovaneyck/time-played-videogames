module Matcher

let cleanWith whitelist title =
    whitelist
    |> Map.tryFind title
    |> Option.defaultValue title

let findMatch
    (
        game: Grouvee.GrouveeGame,
        searchResponses: HowLongToBeatParsing.SearchResult list
    ) : HowLongToBeatParsing.SearchResult =
    let closestMatch = searchResponses.[0]
    closestMatch
