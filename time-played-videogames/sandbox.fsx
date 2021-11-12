#r "nuget: Unquote"
#r "nuget: xunit"
#r "nuget: FSharp.Data"

#load "Grouvee.fs"

let filepath =
    @"C:\Users\Jo.VanEyck\source\repos\time-played-videogames\time-played-videogames\data\praGmatic_28044_grouvee_export.csv"

let csvContents =
    "id,name,shelves,platforms,rating,review,dates,statuses,genres,franchises,developers,publishers,release_date,url,giantbomb_id
57334,Darksiders III,\"{\"\"Played\"\": {\"\"date_added\"\": \"\"2020-03-09T14:54:40Z\"\", \"\"url\"\": \"\"https://www.grouvee.com/user/praGmatic/shelves/147651-played/\"\"}}\",{},1,,[],[],\"{\"\"Action\"\": {\"\"url\"\": \"\"https://www.grouvee.com/games/?genre=action\"\"}, \"\"Action-Adventure\"\": {\"\"url\"\": \"\"https://www.grouvee.com/games/?genre=action-adventure\"\"}, \"\"Role-Playing\"\": {\"\"url\"\": \"\"https://www.grouvee.com/games/?genre=role-playing\"\"}}\",\"{\"\"Darksiders\"\": {\"\"url\"\": \"\"https://www.grouvee.com/games/franchise/1830-darksiders/\"\"}}\",{},{},2018-11-27,https://www.grouvee.com/games/57334-darksiders-iii/,59346"


let rows = Grouvee.parseFile filepath
let rows2 = Grouvee.parseContents csvContents
