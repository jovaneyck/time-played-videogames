module Test

open Xunit
open Swensen.Unquote

[<Fact>]
let ``Using unquote assertions`` () = test <@ 1 + 2 + 3 = 6 @>
