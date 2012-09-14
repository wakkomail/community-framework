$(document).ready(function () {
    //Search watermark
    $("#searchText").watermark('Zoeken...');
    $(".gebruikersnaam").watermark('Gebruikersnaam');
    $(".wachtwoord").watermark('Wachtwoord');

    //Target Blank
    $("a.blank").attr("target", "_blank");

    //Brandbox
    $('#brandboxItems').cycle({
        pager: '#brandboxPager',
        pagerEvent: 'mouseover',
        timeout: 6000,
        speed: 600,
        pauseOnPagerHover: true,
        pagerAnchorBuilder: function (idx, slide) {
            return '<div class="brandboxPagerItem"></div>';
        }
    });

    $('.anchorItem').click(function () {
        window.location = $(this).find('a').attr('href');
    });

    // Search Click    
    $("#searchButton").click(function () {
        window.location = "/CLCSearchResult.aspx?search=" + $("#searchText").val();
        return false;
    });

    $('#searchText').keypress(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);

        if (code == 13) {
            $('#searchButton').click();
        }
    });

    // Fancybox hides and calls
    $('.deleteMedia').click(function () {
        $.fancybox.open($(this).find('.confirmDeleteMedia'));
    });

    $('.cancel').click(function () {
        $.fancybox.close();
    });
    // Fancybox hides and calls
    $('#setMemberButton').click(function () {
        var groupId = $(this).attr('name');

        $.fancybox.open({
            href: 'CLCSetMembers.aspx?groupid=' + groupId,
            type: 'iframe',
            width: 600,
            height: '90%',
            scrolling: 'no',
            padding: 5
        });
    });
	
	//Add alpha to every first and omega to every third column.
	$('.threeColumns .grid_3:nth-child(3n-2)').addClass('alpha');
	$('.threeColumns .grid_3:nth-child(3n)').addClass('omega');

});