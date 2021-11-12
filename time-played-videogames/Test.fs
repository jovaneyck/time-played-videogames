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
    [<Fact>]
    let ``Parsing HLTB playtime text`` () =
        test <@ HowLongToBeat.parsePlaytime "18 Hours " = 18m @>
        test <@ HowLongToBeat.parsePlaytime "18½ Hours " = 18.5m @>

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
            <@ HowLongToBeat.parseSearchResult html = [ { Title = "Darksiders III"
                                                          PlayTime = 18.5M }
                                                        { Title = "Darksiders III - The Crucible"
                                                          PlayTime = 1.5M }
                                                        { Title = "Darksiders III - Keepers of the Void"
                                                          PlayTime = 4M } ] @>
