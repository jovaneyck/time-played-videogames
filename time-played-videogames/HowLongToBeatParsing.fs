module HowLongToBeatParsing

open FSharp.Data

type Category =
    | MainStory
    | MainExtras
    | Completionist

type Playtimes =
    { MainStory: decimal option
      MainExtras: decimal option
      Completionist: decimal option }

type Playtime = (Category * decimal option)

type SearchResult = { Title: string; PlayTimes: Playtimes }

let parsePlaytime (text: string) : decimal option =
    let parseHours (text: string) =
        let number =
            text.Split(" Hours").[0].Replace("½", ".5")

        number |> System.Decimal.Parse |> Some

    let parseMinutes (text: string) =
        let number = text.Split(" Mins").[0]

        number
        |> System.Decimal.Parse
        |> (fun n -> n / 60m)
        |> Some

    match text with
    | "--" -> None
    | hours when text.Contains("Hours") -> parseHours text
    | minutes when text.Contains("Mins") -> parseMinutes text
    | unknown -> failwithf "Unrecognized play time format: %s" text

let parseCategory =
    function
    | "Main Story" -> MainStory
    | "Main + Extra" -> MainExtras
    | "Completionist" -> Completionist
    | invalid -> failwithf "Unknown category: %s" invalid

let private parsePlaytimeDiv (divs: HtmlNode list) : Playtime =
    match divs with
    | categoryDiv :: playtimeDiv :: [] ->
        let category =
            categoryDiv |> HtmlNode.innerText |> parseCategory

        let playtime =
            playtimeDiv |> HtmlNode.innerText |> parsePlaytime

        category, playtime

    | invalid ->
        failwithf
            "I expected 2 divs, one containing the category, the other containing the playtime. Instead I got: %A"
            invalid

let private parseSearchResultNode (node: HtmlNode) =
    let search_list_details =
        node
        |> HtmlNode.descendants false (fun n -> n |> HtmlNode.hasClass "search_list_details")
        |> Seq.head

    let a =
        search_list_details
        |> HtmlNode.descendantsNamed false [ "a" ]
        |> Seq.head

    let title = a |> HtmlNode.innerText

    let playtimeDivs =
        search_list_details
        |> HtmlNode.descendants false (fun n -> n |> HtmlNode.hasClass "search_list_tidbit")
        |> Seq.chunkBySize 2

    let playtimes =
        playtimeDivs
        |> Seq.toList
        |> List.map (List.ofArray >> parsePlaytimeDiv)
        |> Map.ofList

    { Title = title
      PlayTimes =
          { MainStory =
                playtimes
                |> Map.tryFind MainStory
                |> Option.defaultValue None
            MainExtras =
                playtimes
                |> Map.tryFind MainExtras
                |> Option.defaultValue None
            Completionist =
                playtimes
                |> Map.tryFind Completionist
                |> Option.defaultValue None } }

let parseSearchResult (HowLongToBeatHttp.HttpSearchResponse html) : SearchResult list =
    if html.Contains("No results for ") then
        []
    else
        html
        |> HtmlDocument.Parse
        |> HtmlDocument.descendantsNamed false [ "li" ]
        |> Seq.map parseSearchResultNode
        |> Seq.toList
