#r "nuget: Unquote"
#r "nuget: xunit"
#r "nuget: FSharp.Data"

#load "Grouvee.fs"

let filepath =
    @"C:\Users\Jo.VanEyck\source\repos\time-played-videogames\time-played-videogames\data\praGmatic_28044_grouvee_export.csv"

let rows = Grouvee.parseFile filepath
