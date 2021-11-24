module Tally

open HowLongToBeatParsing

type Tally =
    { MainStory: decimal
      MainExtras: decimal
      Completionist: decimal }

let optionIfNone (ifNone: 'a option) (subject: 'a option) =
    match subject with
    | None -> ifNone
    | v -> v

let rec private nextBestPlaytime (playtimes: Playtimes) (currentCategory: Category) : decimal option =
    match currentCategory with
    | Completionist ->
        playtimes.MainExtras
        |> optionIfNone (nextBestPlaytime playtimes MainExtras)
    | MainExtras -> playtimes.MainStory |> optionIfNone None
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
                     |> optionIfNone (nextBestPlaytime searchresult.PlayTimes MainExtras)
                     |> Option.defaultValue 0m)
              Completionist =
                  tally.Completionist
                  + (searchresult.PlayTimes.Completionist
                     |> optionIfNone (nextBestPlaytime searchresult.PlayTimes Completionist)
                     |> Option.defaultValue 0m) }

    results |> Seq.fold folder initial
