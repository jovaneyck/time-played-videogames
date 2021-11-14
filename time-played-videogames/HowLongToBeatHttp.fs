module HowLongToBeatHttp

open FSharp.Data

type HttpSearchResponse = HttpSearchResponse of string

let getHtml gameTitle : Async<HttpSearchResponse> =
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
            | Text t -> HttpSearchResponse t
            | unhandled -> failwithf "Unhandled HLTB http response: %A" unhandled

        return response
    }
