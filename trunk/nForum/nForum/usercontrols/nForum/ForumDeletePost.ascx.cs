using System;
using System.Web;
using nForum.BusinessLogic;
using umbraco;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.web;

namespace nForum.usercontrols.nForum
{
    public partial class ForumDeletePost : BaseForumUsercontrol
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DeletePost();
        }

        private void DeletePost()
        {
            // Make sure we have a post id available to us
            int? pId = null;
            if (Request.QueryString["p"] != null)
                pId = Request.QueryString["p"].ToInt32();

            // get the referring page
            var previouspage = HttpContext.Current.Request.UrlReferrer.AbsoluteUri.Split('?')[0];
            

            if (pId != null && MembershipHelper.IsAuthenticated())
            {
                var postID = pId.ToInt32();
                // great we have a postid, now check the user is allowed to delete it
                var posttodelete = Mapper.MapForumPost(new Node(postID));

                if (posttodelete.Owner.MemberId == CurrentMember.MemberId | CurrentMember.MemberIsAdmin)
                {                    
                    // These are used either way
                    var topic = new Document(posttodelete.ParentId.ToInt32());
                    
                    // We know we can delete this, but if its a topic starter then delete entire topic
                    if(posttodelete.IsTopicStarter)
                    {
                        var topicId = topic.Id;
                        // Its a topic starter so delete entire topic
                        if (topic.Published)
                        {
                            topic.UnPublish();
                            library.UnPublishSingleNode(topicId);
                        }
                        topic.delete();

                        library.RefreshContent();

                        // Redirect and show message
                        Response.Redirect(string.Concat(Settings.Url, "?nf=true&m=", library.GetDictionaryItem("DeletedText")));
                    }
                    else
                    {
                        // Its just a normal post, delete it
                        var p = new Document(postID);
                        if (p.Published)
                        {
                            p.UnPublish();
                            library.UnPublishSingleNode(postID);                            
                        }
                        p.delete();

                        // Little bit of a hack to update the topic with the new latest post date
                        Factory.UpdateTopicWithLastPostDate(topic.Id);

                        library.RefreshContent();

                        // Redirect and show message
                        Response.Redirect(string.Concat(previouspage, "?nf=true&m=", library.GetDictionaryItem("DeletedText")));
                    }
                }
             
            }
            //Error so just redirect back to the page
            Response.Redirect(previouspage);

        }
    }
}