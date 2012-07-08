using System;
using System.Linq;
using nForum.BusinessLogic;
using nForum.BusinessLogic.Models;
using umbraco;
using umbraco.cms.businesslogic.member;

namespace nForum.usercontrols.CLC.membership
{
    public partial class MemberProfile : BaseForumUsercontrol
    {
        public ForumMember ProfileMember { get; set; }
        protected void Page_Load(object sender, EventArgs e)
        {
            GetMemberDetails();
            if(!Page.IsPostBack)
            {
                GetMemberProfile();
            }
        }

        private void GetMemberDetails()
        {
            // Get the member id
            int? m = null;
            if (Request.QueryString["mem"] != null)
                m = Convert.ToInt32(Request.QueryString["mem"]);

            // if its not null then get the member details
            if (m != null)
            {
                // Get the member whos profile this is
                ProfileMember = MembershipHelper.ReturnMember(m);
            }
        }

        private void GetMemberProfile()
        {

            // if its not null then get the member details
            if (ProfileMember != null)
            {
                try
                {

                    // Populate the fields
                    litMemberName.Text = ProfileMember.MemberLoginName;
					litFullName.Text = ProfileMember.MemberName;
                    litJoined.Text = ProfileMember.MemberDateRegistered.ToShortDateString();
                    //litKarma.Text = ProfileMember.MemberKarmaAmount.ToString();
                    litPosts.Text = ProfileMember.MemberPostAmount.ToString();
                    var userAllowMessages = ProfileMember.MemberAllowPrivateMessages;
                    //litTwitter.Text = Helpers.CreateTwitterLinkFromUsername(ProfileMember.MemberTwitterUrl);
					litEmail.Text = ProfileMember.MemberEmail;

                    // Get the lastest 30 posts from this user
                    var posts = (from p in Factory.ReturnAllPostsByMemberId(ProfileMember.MemberId.ToInt32())
                                 select p.ParentId).Take(15);

                    // Now get topics where the user has posted
                    var maintopics = from t in Factory.ReturnAllTopics(true)
                                     where posts.Contains(t.Id)
                                     select t;

					litBreadCrumbsName.Text = MembershipHelper.ReturnMemberProfileLink(ProfileMember.MemberLoginName, "c2", ProfileMember.MemberId, null);

                    // Now bind
                    rptTopicList.DataSource = maintopics;
                    rptTopicList.DataBind();

                    if(MembershipHelper.IsAuthenticated() && CurrentMember.MemberIsAdmin && CurrentMember.MemberId != ProfileMember.MemberId && !ProfileMember.MemberIsAdmin)
                    {
                        linkBanMember.Visible = true;
                        linkBanMember.Text = ProfileMember.MemberIsBanned ? library.GetDictionaryItem("RemoveBan") : library.GetDictionaryItem("BanMember");
                    }
                }
                catch (Exception)
                {
                    // Can't get member id so hide the page
                    memberprofile.Visible = false;
                }

            }
            else
            {
                // Can't get member id so hide the page
                memberprofile.Visible = false;
            }
        }

        protected void LinkBanMemberClick(object sender, EventArgs e)
        {
            if (CurrentMember.MemberIsAdmin && ProfileMember.MemberId != null)
            {
                string usermessage;
                var cMem = new Member((int)ProfileMember.MemberId);

                if(ProfileMember.MemberIsBanned)
                {
                    //Un ban member
                    cMem.getProperty("forumUserIsBanned").Value = "0";
                    usermessage = library.GetDictionaryItem("MemberIsNowNotBanned");
                }
                else
                {
                    // Ban the member
                    cMem.getProperty("forumUserIsBanned").Value = "1";
                    usermessage = library.GetDictionaryItem("MemberIsNowBanned");
                }

                //Save member
                cMem.Save();

                //Generate member Xml Cache
                cMem.XmlGenerate(new System.Xml.XmlDocument());

                // Redirect to show message
                Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", usermessage));
            }
        }
    }
}