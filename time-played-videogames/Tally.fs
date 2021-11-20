module Tally

open HowLongToBeatParsing

type Tally =
    { MainStory: decimal
      MainExtras: decimal
      Completionist: decimal }

let tally (results: SearchResult seq) =
    let initial =
        { MainStory = 0m
          MainExtras = 0m
          Completionist = 0m }

    let folder tally searchresult =
        let (_, main) =
            searchresult.PlayTimes
            |> List.find (fun (cat, _) -> cat = MainStory)

        let (_, mainExtras) =
            searchresult.PlayTimes
            |> List.find (fun (cat, _) -> cat = MainExtras)

        let (_, completionist) =
            searchresult.PlayTimes
            |> List.find (fun (cat, _) -> cat = Completionist)

        { tally with
              MainStory = tally.MainStory + (main |> Option.defaultValue 0m)
              MainExtras =
                  tally.MainExtras
                  + (mainExtras |> Option.defaultValue 0m)
              Completionist =
                  tally.Completionist
                  + (completionist |> Option.defaultValue 0m) }

    results |> Seq.fold folder initial
