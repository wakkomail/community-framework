<%@ Control Language="C#" EnableViewState="true" AutoEventWireup="true" CodeBehind="ForumEditorIncludes.ascx.cs" Inherits="nForum.usercontrols.nForum.includes.ForumEditorIncludes" %>

<script type="text/javascript" src="/umbraco_client/tinymce3/tiny_mce_src.js"></script>
<script type="text/javascript">
    tinyMCE.init({
        // General options
    	mode: "textareas",
        elements: "txtPost",
        theme: "advanced",
        plugins: "insertcode,media",
        // Theme options
        theme_advanced_buttons1: "bold,italic,underline,strikethrough,|,formatselect,|,bullist,numlist,|,link,unlink,image,media,insertcode",
        theme_advanced_buttons2 : "",
        theme_advanced_buttons3 : "",
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "center",
        theme_advanced_resizing: true,
        remove_linebreaks: false,
        relative_urls: false,
        content_css: "/css/nforumeditor.css"
    });
</script>