using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.media;
using nForum.BusinessLogic.Models;
using nForum.BusinessLogic;
using nForum.helpers;
using umbraco.NodeFactory;
using umbraco;

namespace nForum.usercontrols.CLC
{
    public partial class MediaList : BaseForumUsercontrol
    {
		public bool ShowDelete { get; set; }
		public bool ShowAll { get; set; }

        ForumCategory currentCategory = new ForumCategory();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Initialize();
            if (!IsPostBack)
            {
                this.SetMediaList();
            }            
        }

        private void SetMediaList()
        {
            Media folder = MediaHelper.GetMediaFolderByName(currentCategory.Name, Node.GetCurrent().Parent.Name);
			
            this.rptMedia.DataSource = ShowAll ? folder.GetDescendants() : folder.GetDescendants().Cast<Media>().Take(10);
            this.rptMedia.DataBind();                     
        }

        private void Initialize()
        {
            currentCategory = Mapper.MapForumCategory(CurrentNode);

			btnShowAll.Visible = !ShowAll;

			var url = library.NiceUrl(CurrentNode.Id);
			btnShowAll.NavigateUrl = Helpers.AlternateTemplateUrlFix("/Documenten", url);
        }

        protected bool IsImage(string filename)
        {
            bool result = false;
            string orgExt = ((string)filename.Substring(filename.LastIndexOf(".") + 1, filename.Length - filename.LastIndexOf(".") - 1));
            orgExt = orgExt.ToLower();
            string ext = orgExt.ToLower();

            if (",jpeg,jpg,gif,bmp,png,tiff,tif,".IndexOf("," + ext + ",") > 0)
            {
                result = true;
            }

            return result;
        }

        protected string GetThumbnail(string filepath)
        {
            string result = string.Empty;            
            string orgExt = ((string)filepath.Substring(filepath.LastIndexOf(".") + 1, filepath.Length - filepath.LastIndexOf(".") - 1));
            orgExt = orgExt.ToLower();
            string ext = orgExt.ToLower();

            result = filepath.Replace(".", "_thumb.").Replace(ext, "jpg");

            return result;
        }

        protected void rptMedia_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int id = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "delete")
            {
                Media deleteFile = new Media(id);
                deleteFile.delete();
                Response.Redirect(Request.RawUrl);
            }
        }
    }
}