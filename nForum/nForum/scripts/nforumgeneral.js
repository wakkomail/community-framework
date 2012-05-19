$(document).ready(function () {

    // Set this for validation
    $("#mainform").validate({
        errorLabelContainer: $("div.error")
    });

    // Sort the mark as solution button
    $(".marksolution").click(function () {
        var postid = $(this).attr('rel');
        var forumpost = "forumpost" + postid;
        $(".marksolution").remove();
        // Yes this is qualified on the server side, so don't even bother!
        $.get("/base/Solution/MarkAsSolution/" + postid + ".aspx",
        function (data) {
            $('.' + forumpost).removeClass().addClass('forumpost solutionTrue ' + forumpost);
        });
        return false;
    });

    // Sort the thumbs up on a post
    $(".thumbuplink").click(function () {
        var postid = $(this).attr('rel');
        var thumbsholder = "karmathumbs" + postid;
        var karmascore = "karma" + postid;
        $("." + thumbsholder).remove();
        // Yes this is qualified on the server side, so don't even bother!
        $.get("/base/Solution/ThumbsUpPost/" + postid + ".aspx",
        function (data) {
            var newkarma = $('value', data).text();
            $('.' + karmascore).text(newkarma);
        });
        return false;
    });

    // Sort the thumbs up on a post
    $(".thumbdownlink").click(function () {
        var postid = $(this).attr('rel');
        var thumbsholder = "karmathumbs" + postid;
        var karmascore = "karma" + postid;
        $("." + thumbsholder).remove();
        // Yes this is qualified on the server side, so don't even bother!
        $.get("/base/Solution/ThumbsDownPost/" + postid + ".aspx",
        function (data) {
            var newkarma = $('value', data).text();
            $('.' + karmascore).text(newkarma);
        });
        return false;
    });

    // Set Members
    $(".btnSetMember").click(function (event) {
        event.preventDefault();

        jQuery.each($('.isMember'), function () {
            alert((this).val());
        });
    });

    // Create a new post
    $(".btnsubmitpost").click(function (event) {
        event.preventDefault();
        var topicid = $(this).attr('rel');
        var sbody = tinyMCE.get('txtPost').getContent();
        $('.topicpostlistnewpost').remove();
        $('.postsuccess').show();
        // Yes this is qualified on the server side, so don't even bother!
        $.post("/base/Solution/NewForumPost/" + topicid + ".aspx",
			   { "postcontent": sbody },
				function (data) {
				    // Add a little delay to help examine catch up
				    var returnUrl = $("value", data).delay(3000).text();
				    window.location.href = returnUrl;
				    //window.location.reload();
				});
    });

    // Create a new topic
    $(".btnsubmittopic").click(function (event) {
        event.preventDefault();
        var catid = $(this).attr('rel');
        var sbody = tinyMCE.get('txtPost').getContent();
        var stitle = $('#tbTopicTitle').val();
        var sticky = $('#cbSticky').is(':checked');
        var locked = $('#cbLockTopic').is(':checked');
        $('.createnewtopicform').remove();
        $('.postsuccess').show();
        // Yes this is qualified on the server side, so don't even bother!
        $.post("/base/Solution/NewForumTopic/" + catid + ".aspx",
			   { "postcontent": sbody, "posttitle": stitle, "poststicky": sticky, "postlocked": locked },
				function (data) {
				    // Add a little delay to help examine catch up
				    var returnUrl = $("value", data).delay(3000).text();
				    window.location.href = returnUrl;
				    //window.location.reload();
				});

    });

    // Subscribe to topic subscribedtotopic
    $(".notsubscribedtotopic").click(function () {
        var topicid = $(this).attr('rel');
        $(this).slideUp('fast');
        $(this).html('Unsubscribe From Topic');
        $(this).removeClass('notsubscribedtotopic').addClass('subscribedtotopic');
        $(this).slideDown();
        $.get("/base/Solution/SubScribeToTopic/" + topicid + ".aspx",
        function (data) {
            var result = $('value', data).text();
        });
        return false;
    });

    // UnSubscribe to topic subscribedtotopic
    $(".subscribedtotopic").click(function () {
        var topicid = $(this).attr('rel');
        $(this).slideUp('fast');
        $(this).html('Subscribe To Topic');
        $(this).removeClass('subscribedtotopic').addClass('notsubscribedtotopic');
        $(this).slideDown();
        $.get("/base/Solution/UnSubScribeToTopic/" + topicid + ".aspx",
        function (data) {
            var result = $('value', data).text();
        });
        return false;
    });

    // UnSubscribe to topic subscribedtotopic
    $(".quotebutton").click(function () {
        //event.preventDefault();
        var postid = $(this).attr('rel');
        var postcontentid = "#postcontent" + postid;
        var postmemnameid = ".postmember" + postid;
        var postmemname = jQuery.trim($(postmemnameid).text());
        var postcontent = "<pre><em>" + postmemname + "</em><br>" + jQuery.trim($(postcontentid).text()) + "</pre>";
        tinyMCE.execCommand('mceInsertContent', false, postcontent);
        return true;
    });

});
