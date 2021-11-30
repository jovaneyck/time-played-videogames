module Tally

open HowLongToBeatParsing

type Tally =
    { MainStory: decimal
      MainExtras: decimal
      Completionist: decimal }

let rec private nextBestPlaytime (playtimes: Playtimes) (currentCategory: Category) : decimal option =
    match currentCategory with
    | Completionist ->
        playtimes.MainExtras
        |> Option.orElse (nextBestPlaytime playtimes MainExtras)
    | MainExtras -> playtimes.MainStory |> Option.orElse None
    | MainStory -> None

let tally (results: SearchResult seq) =
    let initial =
        { MainStory = 0m
          MainExtras = 0m
          Completionist = 0m }

    let folder tally searchresult : Tally =
        { tally with
              MainStory =
                  tally.MainStory
                  + (searchresult.PlayTimes.MainStory
                     |> Option.defaultValue 0m)
              MainExtras =
                  tally.MainExtras
                  + (searchresult.PlayTimes.MainExtras
                     |> Option.orElse (nextBestPlaytime searchresult.PlayTimes MainExtras)
                     |> Option.defaultValue 0m)
              Completionist =
                  tally.Completionist
                  + (searchresult.PlayTimes.Completionist
                     |> Option.orElse (nextBestPlaytime searchresult.PlayTimes Completionist)
                     |> Option.defaultValue 0m) }

    results |> Seq.fold folder initial
