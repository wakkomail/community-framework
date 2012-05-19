using System;
using System.IO;
using System.Text;
using System.Web.UI;
using nForum.BusinessLogic;

namespace nForum.usercontrols.nForum.includes
{
    public partial class Emoticons : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GetSmileys();
        }

        private void GetSmileys()
        {
            const string emoticonPath = @"/nforum/emoticons/";
            var sb = new StringBuilder();

            FileInfo[] rgFiles;
            if (!CacheHelper.Get(CacheHelper.CacheNameSmileys(), out rgFiles))
            {
                var di = new DirectoryInfo(Server.MapPath(emoticonPath));
                rgFiles = di.GetFiles("*.png");

                CacheHelper.Add(rgFiles, CacheHelper.CacheNameSmileys());
            }

            foreach (var fi in rgFiles)
            {
                var freindlyName = fi.Name.Replace(".png", "").Replace("-", " ");
                var jsFileName = (emoticonPath + fi.Name).Replace("\\", "\\\\");
                sb.Append("<span>");
                sb.AppendFormat("<a href=\"javascript:;\" onmousedown=\"tinyMCE.execCommand('mceInsertContent',false,'<img src=&quot;{0}&quot; alt=&quot;{1}&quot;>');\"><img src=\'{0}\' alt=\'{1}\'></a>", jsFileName, freindlyName);
                sb.Append("</span>");
            }

            litEmoticons.Text = sb.ToString();
        }
    }
}