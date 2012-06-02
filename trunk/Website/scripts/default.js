$(document).ready(function () {
	//Search watermark
	$("#searchText").watermark('Zoeken...');
	$("#gebruikersnaam").watermark('Gebruikersnaam');
	$("#wachtwoord").watermark('Wachtwoord');

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
});