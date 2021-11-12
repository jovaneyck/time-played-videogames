module Grouvee

open FSharp.Data

[<Literal>]
let url =
    $"data\praGmatic_28044_grouvee_export.csv"

type private GrouveeCsv = CsvProvider<url, ResolutionFolder=__SOURCE_DIRECTORY__>

type Shelf =
    | Played
    | Playing
    | Backlog
    | Wishlist

type GrouveeGame = { Title: string; Shelf: Shelf }

let parse (row: GrouveeCsv.Row) : GrouveeGame =
    let parseCategory =
        function
        | "Played" -> Played
        | "Playing" -> Playing
        | "Wish List" -> Wishlist
        | "Backlog" -> Backlog
        | unknown -> failwithf "Unknown category: %s" unknown

    let title = row.Name
    let rawCategoryName = row.Shelves.Split([| '"' |]).[1]
    let category = parseCategory rawCategoryName
    { Title = title; Shelf = category }

let parseFile (path: string) : GrouveeGame list =
    let rows = GrouveeCsv.Load(path).Rows
    rows |> Seq.map parse |> Seq.toList

let parseContents (contents: string) : GrouveeGame list =
    let rows = GrouveeCsv.Parse(contents).Rows
    rows |> Seq.map parse |> Seq.toList
