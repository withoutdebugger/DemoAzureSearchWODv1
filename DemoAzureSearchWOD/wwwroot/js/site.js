
var itemPage = "<li class='page-item item-page {1}'><a class='page-link' href='#'>{0}</a></li>";
var itemList = "<div class='card' >" + 
                "<img class='card-img-top' src='{thumbnail}' alt='Card image cap'>" + 
                "<div class='card-body'>" + 
                "<h5 class='card-title'>{number} {street}, {city}</h5>" + 
                "<p class='card-text'>{description}</p>" + 
                "</div>" + 
                "<ul class='list-group list-group-flush'>" + 
                "<li class='list-group-item'>" + 
                "<span class='badge badge-{active}'>available</span>" + 
                "</li>" + 
                "<li class='list-group-item'>" + 
                "{tags}" + 
                "</li>" +
                "</ul>" + 
                "<div class='card-body'><a href='#' class='btn btn-primary'>Buy</a></div>" + 
    "</div>";
var itemListTag = "<span class='badge badge-info'>{0}</span> ";
var itemTags = "<span class='badge badge-dark p-2 m-1' style='text-transform: capitalize'>" +
                "<a href= '#' class='btnPill' origin='pills-{0}' value='{1}' text='{2} ({3})' >{2} ({3})</a></span >";

$(function () {

    $("#pills-filter .btnPill").click(ClickPill);
    $("#pills-city .btnPill").click(ClickPill);    
    $("#pills-type .btnPill").click(ClickPill);
    $("#pills-tags .btnPill").click(ClickPill);

    $("#btnSearch").click(function () {

        FindHotel();
        return false;

    });


});

function ServiceSearch(text, fCity, fType, fTags, aPage) {

    $.get(
        '/api/search',
        {
            searchtext: text,
            cityFacet: fCity,
            typeFacet: fType,
            tags: fTags,
            actualPage: aPage
        },
        function (json) {
            ReDrawPager(1, json.count);
            ReDrawPills("city", json.facets.city);
            ReDrawPills("type", json.facets.type);
            ReDrawPills("tags", json.facets.tags);
            ReDrawList(json.results);

            $("#cardsId").removeClass("invisible");
            $("#pillsId").removeClass("invisible");
            $("#pagerId").removeClass("invisible");
            
        });

}

function FindHotel() {

    var facetCity = "", facetType = "", facetTags = "";
    var textSearch = $("#txtSearch").val();
    var lblSearch = $("#lblSearch");

    lblSearch.html(textSearch);
    $("#txtSearch").val("");
    var filterBag = $("#pills-filter .badge").remove();
    ServiceSearch(textSearch, "", "", "", 1);

}

function FindHotelFilter(page) {

        var text = $("#lblSearch").html();
        var fCity = "", fType = "", fTags = "";
        var filterBag = $("#pills-filter .bag")    

        for (i = 0; i < filterBag.length; i++) {

            var item = $(filterBag[i]);
            var origin = item.attr("origin").replace("pills-", "");
            var value = item.attr("value");

            if (origin == "city") {
                fCity = item.attr("value");
            }
            if (origin == "type") {
                fType = item.attr("value");
            }
            if (origin == "tags") {
                fTags = item.attr("value");
            }
        }

        ServiceSearch(text, fCity, fType, fTags, page);
}

function ReDrawPills(to, pills) {

    $("#pills-" + to + " .badge").remove();

    for (i = 0; i < pills.length; i++) {

        var pill = $("#pills-filter [origin = 'pills-" + to + "']");
        var value = pill.attr("value");
        if (pills[i].value == value) { continue; }

        $("#pills-" + to).append(
            itemTags
                .replace("{0}", to)
                .replace("{1}", pills[i].value)
                .replace("{2}", pills[i].value)
                .replace("{2}", pills[i].value)
                .replace("{3}", pills[i].count)
                .replace("{3}", pills[i].count)
        );

    };

    $("#pills-" + to + " .btnPill").click(ClickPill);

}

function ReDrawPager(currentPage, totalItems) {

    var itemPerPage = 10;
    var totalPages = totalItems / itemPerPage; 

    $(".pagination .item-page").remove();

    for (i = 0; i < totalPages; i++) {

        var page = "";
        page = currentPage == (i + 1) ? itemPage.replace("{1}", "active") : itemPage.replace("{1}", "");

        if (totalPages > 20) {

            if (i < 12) {
                $(".pagination").append(page.replace("{0}", (i + 1)));
                continue;
            }

            if (i == 13) {
                $(".pagination").append(page.replace("{0}", "..."));
                continue;
            }

            if (i > (totalPages - 10)) {
                $(".pagination").append(page.replace("{0}", (i + 1)));
                continue;
            }
        } else {
            $(".pagination").append(page.replace("{0}", (i + 1)));
        } 
    }

    $(".pagination .page-link").click(function () {

        var fCity = "", fType = "", fTags = "";
        var filterBag = $("#pills-filter .bag")    

        for (i = 0; i < filterBag.length; i++) {

            var item = $(filterBag[i]);
            var origin = item.attr("origin").replace("pills-", "");
            var value = item.attr("value");

            if (origin == "city") {
                fCity = item.attr("value");
            }
            if (origin == "type") {
                fType = item.attr("value");
            }
            if (origin == "tags") {
                fTags = item.attr("value");
            }
        }

        var text = $("#lblSearch").html();
        var page = $(this).html();

        $.get(
            '/api/search',
            {
                searchtext: text,
                cityFacet: fCity,
                typeFacet: fType,
                tags: fTags,
                actualPage: page
            },
            function (json) {
                ReDrawList(json.results);
            });

        $(".pagination .item-page").removeClass("active");
        $(".pagination .item-page")
            .filter(function () {
                return $(this).text() === page;
            })
            .addClass("active");

    });
    
}

function ReDrawList(results) {

    $(".card-columns .card").remove();
    
    for (i = 0; i < results.length; i++) {

        var tags = "";

        for (x = 0; x < results[i].document.tags.length; x++) {
            tags += itemListTag.replace("{0}", results[i].document.tags[x]);
        }            

        $(".card-columns").append(
            itemList
                .replace("{thumbnail}", results[i].document.thumbnail)
                .replace("{number}", results[i].document.number)
                .replace("{street}", results[i].document.street)
                .replace("{city}", results[i].document.city)
                .replace("{description}", results[i].document.description)
                .replace("{tags}", tags)
                .replace("{active}", results[i].document.status == " active" ? "primary" : "secondary")
        );

    };

}

function ClickPill() {

    var obj = $(this);
    var from = obj.parent().parent().attr("id");
    var to = from != "pills-filter" ? "pills-filter" : obj.attr("origin");

    obj.parent().removeClass("badge-dark")
    obj.parent().removeClass("badge-primary");
    
    if (from == "pills-filter") {
        
        to == "pills-filter" ? obj.parent().addClass("badge-primary") : obj.parent().addClass("badge-dark");
        $("#" + to).append(obj.parent());

    } else {

        to == "pills-filter" ? obj.parent().addClass("badge-primary") : obj.parent().addClass("badge-dark");
        $("#" + to).append( obj.parent() );
    }

    FindHotelFilter(1);
  
}
