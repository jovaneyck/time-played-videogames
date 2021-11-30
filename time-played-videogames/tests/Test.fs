module Test

open Xunit
open Swensen.Unquote

module GrouveeParsingTests =
    let csvContents =
        "id,name,shelves,platforms,rating,review,dates,statuses,genres,franchises,developers,publishers,release_date,url,giantbomb_id
    57334,Darksiders III,\"{\"\"Played\"\": {\"\"date_added\"\": \"\"2020-03-09T14:54:40Z\"\", \"\"url\"\": \"\"https://www.grouvee.com/user/praGmatic/shelves/147651-played/\"\"}}\",{},1,,[],[],\"{\"\"Action\"\": {\"\"url\"\": \"\"https://www.grouvee.com/games/?genre=action\"\"}, \"\"Action-Adventure\"\": {\"\"url\"\": \"\"https://www.grouvee.com/games/?genre=action-adventure\"\"}, \"\"Role-Playing\"\": {\"\"url\"\": \"\"https://www.grouvee.com/games/?genre=role-playing\"\"}}\",\"{\"\"Darksiders\"\": {\"\"url\"\": \"\"https://www.grouvee.com/games/franchise/1830-darksiders/\"\"}}\",{},{},2018-11-27,https://www.grouvee.com/games/57334-darksiders-iii/,59346"

    [<Fact>]
    let ``Parses a basic grouvee csv`` () =
        test
            <@ let games = Grouvee.parseContents csvContents

               games = [ { Title = "Darksiders III"
                           Shelf = Grouvee.Shelf.Played } ] @>

module HLTBTests =
    open HowLongToBeatParsing

    [<Fact>]
    let ``Parsing HLTB playtime text in hours`` () =
        test <@ parsePlaytime "18 Hours " = Some 18m @>
        test <@ parsePlaytime "18½ Hours " = Some 18.5m @>

    [<Fact>]
    let ``Parsing HLTB playtime text in minutes`` () =
        test <@ parsePlaytime "15 Mins " = Some 0.25m @>
        test <@ parsePlaytime "30 Mins " = Some 0.5m @>

    [<Fact>]
    let ``Parsing HLTB playtime text missing data`` () = test <@ parsePlaytime "--" = None @>

    [<Fact>]
    let ``Parsing HLTB responses with actual results`` () =
        let html =
            "


    			<div class=\"global_padding shadow_box back_blue center\">
    				<h3> We Found 3 Games for \"Darksiders III\" </h3> 			</div>

    			<ul>
    				<div class=\"clear\"></div>
    	<li class=\"back_darkish\" 			style=\"background-image:linear-gradient(rgb(31, 31, 31), rgba(31, 31, 31, 0.9)), url('/games/45524_Darksiders_3.jpg')\"> 				<div class=\"search_list_image\">
    					<a aria-label=\"Darksiders III\" title=\"Darksiders III\" href=\"game?id=45524\">
    						<img alt=\"Box Art\" src=\"/games/45524_Darksiders_3.jpg\" />
    					</a>
    				</div> 			<div class=\"search_list_details\">					<h3 class=\"shadow_text\">
    						<a class=\"text_green\" title=\"Darksiders III\" href=\"game?id=45524\">Darksiders III</a>
    											</h3> 					<div class=\"search_list_details_block\"> 								<div>
    									<div class=\"search_list_tidbit text_white shadow_text\">Main Story</div>
    									<div class=\"search_list_tidbit center time_100\">14 Hours </div>
    									<div class=\"search_list_tidbit text_white shadow_text\">Main + Extra</div>
    									<div class=\"search_list_tidbit center time_100\">18&#189; Hours </div>
    									<div class=\"search_list_tidbit text_white shadow_text\">Completionist</div>
    									<div class=\"search_list_tidbit center time_100\">30 Hours </div>
    								</div>					</div> 			</div>	</li>
    	<li class=\"back_darkish\" 			style=\"background-image:linear-gradient(rgb(70, 70, 70), rgba(70, 70, 70, 0.9)), url('/games/65273_Darksiders_III_-_The_Crucible.jpg')\"> 				<div class=\"search_list_image\">
    					<a aria-label=\"Darksiders III  The Crucible\" title=\"Darksiders III  The Crucible\" href=\"game?id=65273\">
    						<img alt=\"Box Art\" src=\"/games/65273_Darksiders_III_-_The_Crucible.jpg\" />
    					</a>
    				</div> 			<div class=\"search_list_details\">					<h3 class=\"shadow_text\">
    						<a class=\"text_white\" title=\"Darksiders III  The Crucible\" href=\"game?id=65273\">Darksiders III - The Crucible</a>
    											</h3> 					<div class=\"search_list_details_block\"> 								<div>
    									<div class=\"search_list_tidbit text_white shadow_text\">Main Story</div>
    									<div class=\"search_list_tidbit center time_40\">1&#189; Hours </div>
    									<div class=\"search_list_tidbit text_white shadow_text\">Main + Extra</div>
    									<div class=\"search_list_tidbit center time_40\">1&#189; Hours </div>
    									<div class=\"search_list_tidbit text_white shadow_text\">Completionist</div>
    									<div class=\"search_list_tidbit center time_40\">2 Hours </div>
    								</div>					</div> 			</div>	</li> 						<div class=\"clear\"></div>
    	<li class=\"back_darkish\" 			style=\"background-image:linear-gradient(rgb(70, 70, 70), rgba(70, 70, 70, 0.9)), url('/games/69219_Darksiders_III_-_Keepers_of_the_Void.jpg')\"> 				<div class=\"search_list_image\">
    					<a aria-label=\"Darksiders III  Keepers of the Void\" title=\"Darksiders III  Keepers of the Void\" href=\"game?id=69219\">
    						<img alt=\"Box Art\" src=\"/games/69219_Darksiders_III_-_Keepers_of_the_Void.jpg\" />
    					</a>
    				</div> 			<div class=\"search_list_details\">					<h3 class=\"shadow_text\">
    						<a class=\"text_white\" title=\"Darksiders III  Keepers of the Void\" href=\"game?id=69219\">Darksiders III - Keepers of the Void</a>
    											</h3> 					<div class=\"search_list_details_block\"> 								<div>
    									<div class=\"search_list_tidbit text_white shadow_text\">Main Story</div>
    									<div class=\"search_list_tidbit center time_50\">4 Hours </div>
    									<div class=\"search_list_tidbit text_white shadow_text\">Main + Extra</div>
    									<div class=\"search_list_tidbit center time_40\">4 Hours </div>
    									<div class=\"search_list_tidbit text_white shadow_text\">Completionist</div>
    									<div class=\"search_list_tidbit center time_50\">4&#189; Hours </div>
    								</div>					</div> 			</div>	</li> 				<div class=\"clear\"></div>
    			</ul> "

        test
            <@ parseSearchResult (HowLongToBeatHttp.HttpSearchResponse html) = [ { Title = "Darksiders III"
                                                                                   PlayTimes =
                                                                                       { MainStory = Some 14M
                                                                                         MainExtras = Some 18.5M
                                                                                         Completionist = Some 30M } }
                                                                                 { Title =
                                                                                       "Darksiders III - The Crucible"
                                                                                   PlayTimes =
                                                                                       { MainStory = Some 1.5M
                                                                                         MainExtras = Some 1.5M
                                                                                         Completionist = Some 2M } }
                                                                                 { Title =
                                                                                       "Darksiders III - Keepers of the Void"
                                                                                   PlayTimes =
                                                                                       { MainStory = Some 4M
                                                                                         MainExtras = Some 4M
                                                                                         Completionist = Some 4.5M } } ] @>

    [<Fact>]
    let ``Parsing HLTB responses with empty playtimes`` () =
        let html =
            "<div class=\"global_padding shadow_box back_blue center\">
            <h3> We Found 4 Games for \"zelda: breath of the wild\" </h3>
            </div>

            <ul>
                <div class=\"clear\"></div>
                <li class=\"back_darkish\"
                    style=\"background-image:linear-gradient(rgb(31, 31, 31), rgba(31, 31, 31, 0.9)), url('/games/no_boxart.png')\">
                    <div class=\"search_list_image\">
                        <a aria-label=\"The Legend of Zelda Breath of the Wild 2\" title=\"The Legend of Zelda Breath of the Wild 2\"
                            href=\"game?id=72589\">
                            <img alt=\"Box Art\" src=\"/games/no_boxart.png\" />
                        </a>
                    </div>
                    <div class=\"search_list_details\">
                        <h3 class=\"shadow_text\">
                            <a class=\"text_white\" title=\"The Legend of Zelda Breath of the Wild 2\" href=\"game?id=72589\">The Legend
                                of Zelda: Breath of the Wild 2</a>
                        </h3>
                        <div class=\"search_list_details_block\">
                            <div>
                                <div class=\"search_list_tidbit text_white shadow_text\">Main Story</div>
                                <div class=\"search_list_tidbit center time_00\">--</div>
                                <div class=\"search_list_tidbit text_white shadow_text\">Main + Extra</div>
                                <div class=\"search_list_tidbit center time_00\">--</div>
                                <div class=\"search_list_tidbit text_white shadow_text\">Completionist</div>
                                <div class=\"search_list_tidbit center time_00\">--</div>
                            </div>
                        </div>
                    </div>
                </li>
                <div class=\"clear\"></div>
            </ul>"

        test
            <@ parseSearchResult (HowLongToBeatHttp.HttpSearchResponse html) = [ { Title =
                                                                                       "The Legend of Zelda: Breath of the Wild 2"
                                                                                   PlayTimes =
                                                                                       { MainStory = None
                                                                                         MainExtras = None
                                                                                         Completionist = None } } ] @>

    [<Fact>]
    let ``Parsing HLTB responses with no results for a game`` () =
        let html =
            "<li class='global_padding back_primary shadow_box'>No results for <strong>Pok&eacute;mon: Let's Go,
            Pikachu!/Eevee!</strong> in <u>games</u>.</li>
            <div class='clear'></div>"

        test <@ parseSearchResult (HowLongToBeatHttp.HttpSearchResponse html) = [] @>

    [<Fact>]
    let ``Parsing HLTB responses with missing playtime categories`` () =
        let html =
            "<div class=\"global_padding shadow_box back_blue center\">
        <h3> We Found 2 Games for \"Just Cause 2\" </h3>
</div>

<ul>
        <li class=\"back_darkish\"
            style=\"background-image:linear-gradient(rgb(70, 70, 70), rgba(70, 70, 70, 0.9)), url('/games/JustCause2MultiplayerMod.jpg')\">
            <div class=\"search_list_image\">
                <a aria-label=\"Just Cause 2 Multiplayer Mod\" title=\"Just Cause 2 Multiplayer Mod\" href=\"game?id=18305\">
                    <img alt=\"Box Art\" src=\"/games/JustCause2MultiplayerMod.jpg\" />
                </a>
            </div>
            <div class=\"search_list_details\">
                <h3 class=\"shadow_text\">
                    <a class=\"text_white\" title=\"Just Cause 2 Multiplayer Mod\" href=\"game?id=18305\">Just Cause 2:
                        Multiplayer Mod</a>
                </h3>
                <div class=\"search_list_details_block\">
                    <div class=\"search_list_tidbit_short text_white shadow_text\">Co-Op</div>
                    <div class=\"search_list_tidbit_long center time_40\">26&#189; Hours </div>
                    <div class=\"search_list_tidbit_short text_white shadow_text\">Vs.</div>
                    <div class=\"search_list_tidbit_long center time_40\">57 Hours </div>
                </div>
            </div>
        </li>
        <div class=\"clear\"></div>
</ul>"

        test
            <@ parseSearchResult (HowLongToBeatHttp.HttpSearchResponse html) = [ { Title =
                                                                                       "Just Cause 2: Multiplayer Mod"
                                                                                   PlayTimes =
                                                                                       { MainStory = None
                                                                                         MainExtras = None
                                                                                         Completionist = None } } ] @>

module ScrubbingTests =
    let mapOf l = l |> Map.ofList

    [<Fact>]
    let ``Scrubs dirty titles`` () =
        test <@ Matcher.cleanWith (mapOf [ ("dirty", "clean!") ]) "dirty" = "clean!" @>

    [<Fact>]
    let ``Leaves already clean titles intact`` () =
        test <@ Matcher.cleanWith (mapOf [ ("dirty", "clean") ]) ":clean:title:" = ":clean:title:" @>

module TallyTests =
    open HowLongToBeatParsing
    open Tally

    [<Fact>]
    let ``Can tally up an empty collection`` () =
        test
            <@ tally [] = { MainStory = 0m
                            MainExtras = 0m
                            Completionist = 0m } @>

    [<Fact>]
    let ``Can tally up a collection consisting of a single game`` () =
        test
            <@ tally [ { Title = ":a:game:"
                         PlayTimes =
                             { MainStory = Some 1m
                               MainExtras = Some 2m
                               Completionist = Some 3m } } ] = { MainStory = 1m
                                                                 MainExtras = 2m
                                                                 Completionist = 3m } @>

    [<Fact>]
    let ``Can tally up a collection consisting of multiple games`` () =
        test
            <@ tally [ { Title = ":a:game:"
                         PlayTimes =
                             { MainStory = Some 1m
                               MainExtras = Some 2m
                               Completionist = Some 3m } }
                       { Title = ":another:game:"
                         PlayTimes =
                             { MainStory = Some 100m
                               MainExtras = Some 200m
                               Completionist = Some 300m } } ] = { MainStory = 101m
                                                                   MainExtras = 202m
                                                                   Completionist = 303m } @>

    [<Fact>]
    let ``Can tally up a collection with games with missing categories. The missing category is completionist (perfectly reasonable scenario)``
        ()
        =
        test
            <@ tally [ { Title = ":a:game:"
                         PlayTimes =
                             { MainStory = Some 1m
                               MainExtras = Some 2m
                               Completionist = None } } ] = { MainStory = 1m
                                                              MainExtras = 2m
                                                              Completionist = 2m } @>


    [<Fact>]
    let ``Can tally up a collection with games with missing categories. The missing cateogry is main and extras`` () =
        test
            <@ tally [ { Title = ":a:game:"
                         PlayTimes =
                             { MainStory = Some 1m
                               MainExtras = None
                               Completionist = Some 3m } } ] = { MainStory = 1m
                                                                 MainExtras = 1m
                                                                 Completionist = 3m } @>

    [<Fact>]
    let ``Can tally up a collection with games with missing categories. The missing cateogry is mainstory (gasp)`` () =
        test
            <@ tally [ { Title = ":a:game:"
                         PlayTimes =
                             { MainStory = None
                               MainExtras = Some 2m
                               Completionist = Some 3m } } ] = { MainStory = 0m
                                                                 MainExtras = 2m
                                                                 Completionist = 3m } @>
