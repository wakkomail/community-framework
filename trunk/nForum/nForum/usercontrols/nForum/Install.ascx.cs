using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using nForum.BusinessLogic;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.datatype;
using umbraco.cms.businesslogic.member;

namespace nForum.usercontrols.nForum
{
    public partial class Install : BaseForumUsercontrol
    {
        private static MemberType CreateMemberType(string alias, string name)
        {
            MemberType.MakeNew(User.GetUser(0), alias);
            var mt = MemberType.GetByAlias(alias);
            mt.Text = name;
            return mt;
        }

        private static ContentType.TabI GetTabByCaption(MemberType mt, string caption)
        {
            return mt.getVirtualTabs.FirstOrDefault(t => t.Caption == caption);
        }

        protected void Button1Click(object sender, EventArgs e)
        {
            const string memberTypeAlias = "ForumUser";
            const string memberTypeName = "Forum User";
            const string memberTabName = "Content";
            const int textStringId = -88;
            const int numericId = -51;
            const int truefalseId = -49;
            const int datetimeId = -36;
            const string forumUserTwitterUrl = "forumUserTwitterUrl";
            const string forumUserPosts = "forumUserPosts";
            const string forumUserKarma = "forumUserKarma";
            const string forumUserAllowPrivateMessages = "forumUserAllowPrivateMessages";
            const string forumUserLastPrivateMessage = "forumUserLastPrivateMessage";
            const string forumUserIsAdmin = "forumUserIsAdmin";
            const string forumUserIsBanned = "forumUserIsBanned";
            const string forumUserIsAuthorised = "forumUserIsAuthorised";

            // First Create the MemberType
            var mt = CreateMemberType(memberTypeAlias, memberTypeName);

            // Now create the tab
            mt.AddVirtualTab(memberTabName);

            // Just trying to stop any timeouts
            Server.ScriptTimeout = 1000;

            // Add the properties            
            // forumUserTwitterUrl > textString
            var dt = DataTypeDefinition.GetDataTypeDefinition(textStringId);
            mt.AddPropertyType(dt, forumUserTwitterUrl, "Twitter Username");
            var prop = mt.getPropertyType(forumUserTwitterUrl);
            prop.Mandatory = false;
            prop.TabId = GetTabByCaption(mt, memberTabName).Id;
            prop.Save();

            // forumUserPosts > Numeric
            dt = DataTypeDefinition.GetDataTypeDefinition(numericId);
            mt.AddPropertyType(dt, forumUserPosts, "Users Post Amount");
            prop = mt.getPropertyType(forumUserPosts);
            prop.Mandatory = true;
            prop.TabId = GetTabByCaption(mt, memberTabName).Id;
            prop.Description = "Make sure this has a value, 0 by default";
            prop.Save();

            // forumUserKarma > Numeric
            dt = DataTypeDefinition.GetDataTypeDefinition(numericId);
            mt.AddPropertyType(dt, forumUserKarma, "Users Karma Amount");
            prop = mt.getPropertyType(forumUserKarma);
            prop.Mandatory = false;
            prop.TabId = GetTabByCaption(mt, memberTabName).Id;
            prop.Save();

            // forumUserAllowPrivateMessages > True/False
            dt = DataTypeDefinition.GetDataTypeDefinition(truefalseId);
            mt.AddPropertyType(dt, forumUserAllowPrivateMessages, "Users Allow Private Messages");
            prop = mt.getPropertyType(forumUserAllowPrivateMessages);
            prop.Mandatory = false;
            prop.TabId = GetTabByCaption(mt, memberTabName).Id;
            prop.Save();

            // Just trying to stop any timeouts
            Server.ScriptTimeout = 1000;

            // forumUserLastPrivateMessage > Date With time
            dt = DataTypeDefinition.GetDataTypeDefinition(datetimeId);
            mt.AddPropertyType(dt, forumUserLastPrivateMessage, "Date/Time Of Last Private Message");
            prop = mt.getPropertyType(forumUserLastPrivateMessage);
            prop.Mandatory = true;
            prop.TabId = GetTabByCaption(mt, memberTabName).Id;
            prop.Description = "This must have a value, default value should be the date and time the user registered/was created";
            prop.Save();

            // forumUserIsAdmin > True/False
            dt = DataTypeDefinition.GetDataTypeDefinition(truefalseId);
            mt.AddPropertyType(dt, forumUserIsAdmin, "Forum User Is Admin");
            prop = mt.getPropertyType(forumUserIsAdmin);
            prop.Mandatory = false;
            prop.TabId = GetTabByCaption(mt, memberTabName).Id;
            prop.Save();

            // forumUserIsBanned > True/False
            dt = DataTypeDefinition.GetDataTypeDefinition(truefalseId);
            mt.AddPropertyType(dt, forumUserIsBanned, "Forum User Is Banned From Forum");
            prop = mt.getPropertyType(forumUserIsBanned);
            prop.Mandatory = false;
            prop.TabId = GetTabByCaption(mt, memberTabName).Id;
            prop.Save();

            // forumUserIsAuthorised > True/False
            dt = DataTypeDefinition.GetDataTypeDefinition(truefalseId);
            mt.AddPropertyType(dt, forumUserIsAuthorised, "Forum User Is Authorised");
            prop = mt.getPropertyType(forumUserIsAuthorised);
            prop.Mandatory = false;
            prop.TabId = GetTabByCaption(mt, memberTabName).Id;
            prop.Save();

            // Just trying to stop any timeouts
            Server.ScriptTimeout = 1000;

            // Lastly create the group
            MemberGroup.MakeNew(memberTypeAlias, User.GetUser(0));

            // ----------- Web.Config ------------


            // set resetpassword to 'true' in web.config
            var config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
            var section = (MembershipSection)config.GetSection("system.web/membership");

            var defaultProvider = section.DefaultProvider;
            var providerSettings = section.Providers[defaultProvider];
            providerSettings.Parameters.Set("enablePasswordReset", "true");
            config.Save();

            // Just trying to stop any timeouts
            Server.ScriptTimeout = 1000;

            // !!!!!!!!!!!!! >>> NOT NEEDED NOW <<< !!!!!!!!!!!!!!!!
            //------ Write the rest extensions to the restExtensions.config
            // look for <RestExtensions>
            //var configfilepath = Server.MapPath(@"~/config/restExtensions.config");
            //var configfiledata = File.ReadAllText(configfilepath);
            //var keytolookfor = "<RestExtensions>";
            //// Get the rest extensions in a string
            //var sb = new StringBuilder();
            //sb.Append(Environment.NewLine).Append("<ext assembly=\"nForum.BusinessLogic\" type=\"nForum.BusinessLogic.nForumBaseExtensions\" alias=\"Solution\">").Append(Environment.NewLine);
            //sb.Append("<permission method=\"MarkAsSolution\" allowAll=\"true\" />").Append(Environment.NewLine);
            //sb.Append("<permission method=\"ThumbsUpPost\" allowAll=\"true\" />").Append(Environment.NewLine);
            //sb.Append("<permission method=\"ThumbsDownPost\" allowAll=\"true\" />").Append(Environment.NewLine);
            //sb.Append("<permission method=\"NewForumPost\" allowAll=\"true\" />").Append(Environment.NewLine);
            //sb.Append("<permission method=\"NewForumTopic\" allowAll=\"true\" />").Append(Environment.NewLine);
            //sb.Append("<permission method=\"SubScribeToTopic\" allowAll=\"true\" />").Append(Environment.NewLine);
            //sb.Append("<permission method=\"UnSubScribeToTopic\" allowAll=\"true\" />").Append(Environment.NewLine);
            //sb.Append("</ext>").Append(Environment.NewLine);
            //// add the rest file data to the current file
            //configfiledata = configfiledata.Replace(keytolookfor, keytolookfor + sb);
            //// now save it!
            //File.WriteAllText(configfilepath, configfiledata);

            // Just trying to stop any timeouts
            Server.ScriptTimeout = 1000;

            //----- Write the url rewriting to the UrlRewriting.config
            // look for <rewrites>
            var configfilepath = Server.MapPath(@"~/config/UrlRewriting.config");
            var configfiledata = File.ReadAllText(configfilepath);
            var keytolookfor = "<rewrites>";
            // Get the rest extensions in a string
            var sb = new StringBuilder();
            sb.Clear();
            sb.Append(Environment.NewLine).Append("<add name=\"memberprofilerewrite\" ").Append(Environment.NewLine);
            sb.Append("virtualUrl=\"^~/member/(.*).aspx\" ").Append(Environment.NewLine);
            sb.Append("rewriteUrlParameter=\"ExcludeFromClientQueryString\" ").Append(Environment.NewLine);
            sb.Append("destinationUrl=\"~/memberprofile.aspx?mem=$1\" ").Append(Environment.NewLine);
            sb.Append("ignoreCase=\"true\" />").Append(Environment.NewLine);
            // add the rest file data to the current file
            configfiledata = configfiledata.Replace(keytolookfor, keytolookfor + sb);
            // now save it!
            File.WriteAllText(configfilepath, configfiledata);

            //------ Write the nforum index to the ExamineIndex.config
            //look for <ExamineLuceneIndexSets>
            configfilepath = Server.MapPath(@"~/config/ExamineIndex.config");
            configfiledata = File.ReadAllText(configfilepath);
            keytolookfor = "<ExamineLuceneIndexSets>";
            sb = new StringBuilder();
            sb.Append(Environment.NewLine).Append("<IndexSet SetName=\"nForumEntrySet\" IndexPath=\"~/App_Data/TEMP/ExamineIndexes/nForumEntryIndexSet/\">").Append(Environment.NewLine);
            sb.Append("<IndexAttributeFields>").Append(Environment.NewLine);
            sb.Append("<add Name=\"id\" EnableSorting=\"true\" Type=\"Number\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"nodeName\" EnableSorting=\"true\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"updateDate\" EnableSorting=\"true\" Type=\"DateTime\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"createDate\" EnableSorting=\"true\" Type=\"DateTime\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"writerName\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"path\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"nodeTypeAlias\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"parentID\" EnableSorting=\"true\" Type=\"Number\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"sortOrder\" EnableSorting=\"true\" Type=\"Number\" />").Append(Environment.NewLine);          
            sb.Append("</IndexAttributeFields>").Append(Environment.NewLine);
            sb.Append("<IndexUserFields>").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumCategoryDescription\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumCategoryIsMainCategory\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumCategoryIsPrivate\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumCategoryPermissionKarmaAmount\" Type=\"Number\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumCategoryPostPermissionKarmaAmount\" Type=\"Number\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumCategorySubscribedList\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumCategoryParentID\" />").Append(Environment.NewLine);            
            sb.Append("<add Name=\"forumPostContent\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumPostOwnedBy\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumPostLastEdited\" EnableSorting=\"true\" Type=\"DateTime\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumPostInReplyTo\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumPostIsSolution\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumPostIsTopicStarter\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumPostKarma\" EnableSorting=\"true\" Type=\"Number\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumPostUsersVoted\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumPostParentID\" />").Append(Environment.NewLine);            
            sb.Append("<add Name=\"forumTopicOwnedBy\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumTopicClosed\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumTopicSolved\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumTopicParentCategoryID\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumTopicIsSticky\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumTopicSubscribedList\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"forumTopicLastPostDate\" EnableSorting=\"true\" Type=\"DateTime\" />").Append(Environment.NewLine);            
            sb.Append("</IndexUserFields>").Append(Environment.NewLine);
            sb.Append("<IncludeNodeTypes>").Append(Environment.NewLine);
            sb.Append("<add Name=\"ForumCategory\"/>").Append(Environment.NewLine);
            sb.Append("<add Name=\"ForumPost\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"ForumTopic\" />").Append(Environment.NewLine);
            sb.Append("<add Name=\"Forum\" />").Append(Environment.NewLine);         
            sb.Append("</IncludeNodeTypes>").Append(Environment.NewLine);
            sb.Append("<ExcludeNodeTypes />").Append(Environment.NewLine);
            sb.Append("</IndexSet>").Append(Environment.NewLine);
            configfiledata = configfiledata.Replace(keytolookfor, keytolookfor + sb);
            File.WriteAllText(configfilepath, configfiledata);
            Server.ScriptTimeout = 1000;

            //------ Write the nforum settings to the ExamineSettings.config
            configfilepath = Server.MapPath(@"~/config/ExamineSettings.config");
            configfiledata = File.ReadAllText(configfilepath).Replace("<providers>", "");
            keytolookfor = "<ExamineIndexProviders>";
            sb = new StringBuilder();
            sb.Append(Environment.NewLine).Append("<#providers#><add name=\"nForumEntryIndexer\" ").Append(Environment.NewLine);
            sb.Append("type=\"UmbracoExamine.UmbracoContentIndexer, UmbracoExamine\" ").Append(Environment.NewLine);
            sb.Append("dataService=\"UmbracoExamine.DataServices.UmbracoDataService, UmbracoExamine\" ").Append(Environment.NewLine);
            sb.Append("indexSet=\"nForumEntrySet\" ").Append(Environment.NewLine);
            sb.Append("supportUnpublished=\"false\" ").Append(Environment.NewLine);
            sb.Append("supportProtected=\"false\" ").Append(Environment.NewLine);
            sb.Append("runAsync=\"true\" ").Append(Environment.NewLine);
            sb.Append("interval=\"10\" ").Append(Environment.NewLine);
            sb.Append("analyzer=\"Lucene.Net.Analysis.Standard.StandardAnalyzer, Lucene.Net\" ").Append(Environment.NewLine);
            sb.Append("enableDefaultEventHandler=\"true\"/>").Append(Environment.NewLine);
            configfiledata = configfiledata.Replace(keytolookfor, keytolookfor + sb);
            File.WriteAllText(configfilepath, configfiledata);
            Server.ScriptTimeout = 1000;

            //------ Write the nforum settings to the ExamineSettings.config
            configfilepath = Server.MapPath(@"~/config/ExamineSettings.config");
            configfiledata = File.ReadAllText(configfilepath).Replace("<providers>", "").Replace("<#providers#>", "<providers>");
            keytolookfor = "<ExamineSearchProviders defaultProvider=\"InternalSearcher\">";
            sb = new StringBuilder();
            sb.Append(Environment.NewLine).Append("<providers><add name=\"nForumEntrySearcher\" type=\"UmbracoExamine.UmbracoExamineSearcher, UmbracoExamine\" ").Append(Environment.NewLine);
            sb.Append("indexSet=\"nForumEntrySet\" analyzer=\"Lucene.Net.Analysis.WhitespaceAnalyzer, Lucene.Net\"/>").Append(Environment.NewLine);
            configfiledata = configfiledata.Replace(keytolookfor, keytolookfor + sb);
            File.WriteAllText(configfilepath, configfiledata);
            Server.ScriptTimeout = 1000;

            // -- All done hide and show panels
            pnlFirstInfo.Visible = false;
            btnComplete.Visible = false;
            pnlSecondInfor.Visible = true;

        }


    }
}