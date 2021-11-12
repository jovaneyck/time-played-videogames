module HowLongToBeat

open FSharp.Data

let getHtml gameTitle : string =
    let headers = [ ("User-Agent", "scraper") ]
    //"queryString=darksiders%20III&t=games&sorthead=popular&sortd=0&plat=&length_type=main&length_min=&length_max=&v=&f=&g=&detail=&randomize=0"
    let body =
        HttpRequestBody.FormValues [ ("queryString", gameTitle)
                                     ("t", "games")
                                     ("sorthead", "popular")
                                     ("sortd", "0")
                                     ("length_type", "main")
                                     ("randomize", "0") ]

    let response =
        Http.Request(
            url = "https://howlongtobeat.com/search_results",
            httpMethod = "POST",
            headers = headers,
            body = body
        )

    match response.Body with
    | Text t -> t
    | unhandled -> failwithf "Unhandled HLTB http response: %A" unhandled

type SearchResult = { Title: string; PlayTime: decimal }

let parsePlaytime (text: string) =
    let number =
        text.Split(" Hours").[0].Replace("½", ".5")

    System.Decimal.Parse number

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

    let playtimeText =
        search_list_details
        |> HtmlNode.descendants false (fun n -> n |> HtmlNode.hasClass "search_list_tidbit")
        |> Seq.map HtmlNode.innerText
        |> Seq.item 3

    let playtime = parsePlaytime playtimeText
    { Title = title; PlayTime = playtime }

let parseSearchResult html : SearchResult list =
    html
    |> HtmlDocument.Parse
    |> HtmlDocument.descendantsNamed false [ "li" ]
    |> Seq.map parseSearchResultNode
    |> Seq.toList
