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

namespace nForum.usercontrols.nForum
{
    public partial class MediaList : BaseForumUsercontrol
    {
        ForumCategory currentCategory = new ForumCategory();

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Initialize();
            this.SetMediaList();
        }

        private void SetMediaList()
        {
            Media folder = MediaAdapter.GetMediaFolderByName(currentCategory.Name);

            this.rptMedia.DataSource = folder.GetDescendants();
            this.rptMedia.DataBind();                     
        }

        private void Initialize()
        {
            currentCategory = Mapper.MapForumCategory(CurrentNode);
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
    }
}