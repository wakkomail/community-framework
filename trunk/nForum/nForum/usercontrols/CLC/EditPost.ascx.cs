using System;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.web;

namespace nForum.usercontrols.CLC
{
    public partial class EditPost : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                LoadPostDetails();
            }
        }

        private void LoadPostDetails()
        {
            if(CurrentMember != null && !IsBanned && !Settings.IsClosed)
            {
                var cPost = Mapper.MapForumPost(CurrentNode);

                //Check this user owns the post
                if (cPost.Owner.MemberId == CurrentMember.MemberId | CurrentMember.MemberIsAdmin)
                {
                    // User owns post to fill text box
                    var txtPost = (TextBox)lvEditPost.FindControl("txtPost");
                    txtPost.Text = cPost.Content;

					var litTitle = (Literal)lvEditPost.FindControl("litTitle");
					litTitle.Text = "Bewerk reactie";

                    if(cPost.IsTopicStarter)
                    {
						litTitle.Text = "Bewerk discussie";

                        // show the topic title
                        var pnlTopicTitle = (Panel)lvEditPost.FindControl("pnlTopicTitle");
                        pnlTopicTitle.Visible = true;

                        //now get the topic
                        var topic = cPost.ParentTopic();

                        //Find the topic textbox
                        var tbTopicTitle = (TextBox)pnlTopicTitle.FindControl("tbTopicTitle");
                        tbTopicTitle.Text = topic.Name;

                        //Now see if we show admin buttons for topics
                        if (CurrentMember.MemberIsAdmin)
                        {
                            var pnlMakeSticky = (Panel)lvEditPost.FindControl("pnlMakeSticky");
                            var cbSticky = (CheckBox)lvEditPost.FindControl("cbSticky");
                            var cbLockTopic = (CheckBox)lvEditPost.FindControl("cbLockTopic");
                            pnlMakeSticky.Visible = true;
                            cbSticky.Checked = topic.IsSticky;
                            cbLockTopic.Checked = topic.IsClosed;
                        }
                    }
                }
                else
                {
                    // Member doesn't own this post so hide it
                    lvEditPost.Visible = false;
                }
            }
            else
            {
                lvEditPost.Visible = false;
            }
        }

        protected void BtnSubmitPostClick(object sender, EventArgs e)
        {
            var txtPost = (TextBox)lvEditPost.FindControl("txtPost");
            var btnSubmitPost = (Button)lvEditPost.FindControl("btnSubmitPost");


            if (!string.IsNullOrEmpty(txtPost.Text))
            {
                // If the user was on a deep page, redirect them to the same page
                var addPage = Request.QueryString["p"] != null ? string.Concat("&p=", Request.QueryString["p"].ToInt32()) : null;

                // Disable button so they don't try and post it twice!
                btnSubmitPost.Enabled = false;

                // Create a user we can use for both
                var u = new User(0);

                // Create the topic
                var p = new Document(Convert.ToInt32(CurrentNode.Id));
                
                // Add the document properties
                p.getProperty("forumPostContent").Value = Helpers.HtmlEncode(Helpers.GetSafeHtml(txtPost.Text));
                p.getProperty("forumPostLastEdited").Value = DateTime.Now;

                // publish application node
                p.Publish(u);

                // update the document cache so its available in the XML
                library.UpdateDocumentCache(p.Id);

                //############ Only do this is the post is a topic starter

                if (p.getProperty("forumPostIsTopicStarter").Value.ToString() == "1")
                {
                    //Get the controls we need
                    var pnlTopicTitle = (Panel)lvEditPost.FindControl("pnlTopicTitle");
                    var tbTopicTitle = (TextBox)pnlTopicTitle.FindControl("tbTopicTitle");
                    

                    // This post is the topic starter, so get the topic
                    var t = new Document(p.ParentId)
                                {
                                    Text = tbTopicTitle.Text
                                };

                    //Now see if we show admin buttons for topics
                    if (CurrentMember.MemberIsAdmin)
                    {
                        var cbSticky = (CheckBox)lvEditPost.FindControl("cbSticky");
                        t.getProperty("forumTopicIsSticky").Value = cbSticky.Checked ? 1 : 0;

                        var cbLockTopic = (CheckBox)lvEditPost.FindControl("cbLockTopic");
                        t.getProperty("forumTopicClosed").Value = cbLockTopic.Checked ? 1 : 0;
                    }

                    // publish application node
                    t.Publish(u);

                    // update the document cache so its available in the XML
                    library.UpdateDocumentCache(t.Id);

                }

                //########################################################

                // redirect to the topic now both are created
                Response.Redirect(string.Concat(library.NiceUrl(p.ParentId), "?nf=true", addPage, "#comment", p.Id));
            }
            else
            {
                // Bah.. Show an error as no data in the fields
                Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("PleaseAddSomething")));
            }
        }
    }
}