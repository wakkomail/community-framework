using System;
using System.Linq;
using System.Web;
using System.Xml;
using nForum.BusinessLogic;

namespace nForum.usercontrols.nForum
{
    public partial class ForumTopicRss : BaseForumUsercontrol
    {
        private void Page_Load(object sender, EventArgs e)
        {
            CreateTopicRssFeed();
        }

        private void CreateTopicRssFeed()
        {
            var rss = new RssHelper();
            var siteUrl = Url();

            var writer = new XmlTextWriter(Response.OutputStream, System.Text.Encoding.UTF8);

            rss.WriteRssPrologue(writer, Settings.Name, siteUrl);

            var maintopics = (from t in Factory.ReturnAllTopicsInCategory(CurrentNode.Id)
                              select t).Take(Settings.TopicsPerPage);

            foreach (var topic in maintopics)
            {
                var latestPost = topic.GetLatestPost();
                rss.AddRssItem(writer, topic.Name, siteUrl + topic.Url, latestPost.Content, latestPost.CreatedOn, true);
            }

            rss.WriteRssClosing(writer);

            writer.Flush();
            writer.Close();

            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "text/xml";
            Response.Cache.SetCacheability(HttpCacheability.Public);

            Response.End();
        }

    }
}