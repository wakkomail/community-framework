using System;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.web;

namespace nForum.usercontrols.nForum
{
    public partial class ForumMoveTopic : BaseForumUsercontrol
    {
        public int? ChildPostNode { get; set; }
        public ForumPost Post { get; set; }
        public ForumTopic Topic { get; set; }
        public ForumCategory Category { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Get current node
            ChildPostNode = null;
            if (Request.QueryString["n"] != null)
                ChildPostNode = Convert.ToInt32(Request.QueryString["n"]);

            GetForumNodes();
            if(!Page.IsPostBack)
            {
                AdminCheck();
            }
        }

        private void GetForumNodes()
        {
            Post = Mapper.MapForumPost(new Node(ChildPostNode.ToInt32()));
            Topic = Post.ParentTopic();
            Category = Topic.ParentCategory();
        }

        private void AdminCheck()
        {
            if (CurrentMember.MemberIsAdmin)
            {   
                if (ChildPostNode != null)
                {
                    forumtopicmove.Visible = true;
                    BindCategories();
                    litCategory.Text = Category.Name;
                }

            }            
        }

        private void BindCategories()
        {
            var cats = Factory.ReturnAllCategories();
            ddlCategories.DataTextField = "Name";
            ddlCategories.DataValueField = "Id";
            ddlCategories.DataSource = cats;
            ddlCategories.DataBind();
        }

        protected void BtnMoveClick(object sender, EventArgs e)
        {
            if (Category.Id != ddlCategories.SelectedValue.ToInt32())
            {
                // Get the document you will move by its ID
                var doc = new Document(Topic.Id);

                // Create a user we can use for both
                var user = new User(0);

                // The new parent ID
                var newParentId = ddlCategories.SelectedValue.ToInt32();

                // Now update the topic parent category ID
                doc.getProperty("forumTopicParentCategoryID").Value = newParentId;

                // publish application node
                doc.Publish(user);

                // Move the document the new parent
                doc.Move(newParentId);

                // update the document cache so its available in the XML
                umbraco.library.UpdateDocumentCache(doc.Id);

                // Redirect and show message
                Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("TopicHasBeenMovedText")));
            }
            else
            {
                // Can't move as they have selected the category that the topic is already in
                Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("TopicAlreadyInThisCategory")));
            }
        }
    }
}