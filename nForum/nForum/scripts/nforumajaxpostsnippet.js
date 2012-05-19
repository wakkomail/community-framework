$(document).ready(function () {


    $(function () {
        var hideDelay = 500;
        var currentID;
        var hideTimer = null;
        var ajax = null;
        var hideFunction = function () {
            if (hideTimer)
                clearTimeout(hideTimer);
            hideTimer = setTimeout(function () {
                currentPosition = { left: '0px', top: '0px' };
                container.css('display', 'none');
            }, hideDelay);
        };

        var currentPosition = { left: '0px', top: '0px' };

        // One instance that's reused to show info for the current post
        var container = $('<div id="ajaxpostcontainer">'
        + '<div id="ajaxpostcontent"><img src="/nforum/img/ajax-loader.gif" alt="Loading.." /></div>'
        + '</div>');

        $('body').append(container);

        $('.postpreview').live('mouseover', function () {
            if (!$(this).data('hoverIntentAttached')) {
                $(this).data('hoverIntentAttached', true);
                $(this).hoverIntent
            (
                // hoverIntent mouseOver
                function () {
                    if (hideTimer)
                        clearTimeout(hideTimer);

                    // get the post id from the rel tag
                    currentID = $(this).attr('rel');

                    // If no post in rel tag, don't popup blank
                    if (currentID == '')
                        return;

                    var pos = $(this).offset();
                    var width = $(this).width();
                    var reposition = { left: (pos.left + width + 5) + 'px', top: pos.top - 5 + 'px' };

                    // If the same popup is already shown, then don't requery
                    if (currentPosition.left == reposition.left &&
                        currentPosition.top == reposition.top)
                        return;

                    container.css({
                        left: reposition.left,
                        top: reposition.top
                    });

                    currentPosition = reposition;

                    $('#ajaxpostcontent').html('&nbsp;');

                    if (ajax) {
                        ajax.abort();
                        ajax = null;
                    }

                    ajax = $.ajax({
                        type: 'GET',
                        url: '/forumpostajaxview.aspx',
                        data: 'postid=' + currentID,
                        success: function (data) {
                            // Verify that we're pointed to a page that returned the expected results.
                            if (data.indexOf('personPopupResult') < 0) {
                                $('#ajaxpostcontent').html('<span style="color:red;">Error getting post information.</span>');
                            }

                            // Verify requested post is this post since we could have multiple ajax
                            // requests out if the server is taking a while.
                            if (data.indexOf(currentID) > 0) {
                                $('#ajaxpostcontent').html(data);
                            }
                        }
                    });

                    container.css('display', 'block');
                },
                // hoverIntent mouseOut
                hideFunction
            );
                // Fire mouseover so hoverIntent can start doing its magic
                $(this).trigger('mouseover');
            }
        });

        // Allow mouse over of details without hiding details
        $('#postpreview').mouseover(function () {
            if (hideTimer)
                clearTimeout(hideTimer);
        });

        // Hide after mouseout
        $('#postpreview').mouseout(hideFunction);
    });


});
