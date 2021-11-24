module EndToEndTests

open Xunit
open Swensen.Unquote

open App

[<Fact>]
let ``Can run the app end to end for a very small collection`` () =
    let games, results = run id @".\tests\e2e-grouvee.csv"

    test <@ games |> List.length = 1 @>
    test <@ games.[0].Title = "Super Mario Odyssey" @>
    test <@ results.Completionist > 50m @>
    test <@ results.Completionist < 100m @>
