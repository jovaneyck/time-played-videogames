﻿module HowLongToBeat

open FSharp.Data

type SearchResponse = SearchResponse of string

let getHtml gameTitle : Async<SearchResponse> =
    async {

        let headers = [ ("User-Agent", "scraper") ]
        //"queryString=darksiders%20III&t=games&sorthead=popular&sortd=0&plat=&length_type=main&length_min=&length_max=&v=&f=&g=&detail=&randomize=0"
        let body =
            HttpRequestBody.FormValues [ ("queryString", gameTitle)
                                         ("t", "games")
                                         ("sorthead", "popular")
                                         ("sortd", "0")
                                         ("length_type", "main")
                                         ("randomize", "0") ]

        let! response =
            Http.AsyncRequest(
                url = "https://howlongtobeat.com/search_results",
                httpMethod = "POST",
                headers = headers,
                body = body
            )

        let response =
            match response.Body with
            | Text t -> SearchResponse t
            | unhandled -> failwithf "Unhandled HLTB http response: %A" unhandled

        return response
    }

type Category =
    | MainStory
    | MainExtras
    | Completionist

type Playtime = (Category * decimal option)

type SearchResult =
    { Title: string
      PlayTimes: Playtime list }

let parsePlaytime (text: string) : decimal option =
    printfn "Parsing: %s" text

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
    printfn "Parsing: %s" title

    let playtimeDivs =
        search_list_details
        |> HtmlNode.descendants false (fun n -> n |> HtmlNode.hasClass "search_list_tidbit")
        |> Seq.chunkBySize 2

    let playtimes =
        playtimeDivs
        |> Seq.toList
        |> List.map (List.ofArray >> parsePlaytimeDiv)

    { Title = title; PlayTimes = playtimes }

let parseSearchResult (SearchResponse html) : SearchResult list =
    html
    |> HtmlDocument.Parse
    |> HtmlDocument.descendantsNamed false [ "li" ]
    |> Seq.map parseSearchResultNode
    |> Seq.toList
