using System;
using System.Text;
using System.Web.UI.WebControls;
using nForum.BusinessLogic;
using umbraco;
using umbraco.cms.businesslogic.member;

namespace nForum.usercontrols.nForum.membership
{
    public partial class ForumMessageUser : BaseForumUsercontrol
    {
        public Member MessageToMember { get; set; }
        public int? MemberId { get; set; }
        public string ReplySubject { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
                GetMessageCreds();
        }

        private void GetMessageCreds()
        {
            // Make sure we have a member id available to us
            MemberId = null;
            if (Request.QueryString["mem"] != null)
                MemberId = Request.QueryString["mem"].ToInt32();

            if (MemberId != null)
            {
                try
                {
                    MessageToMember = new Member((Int32)MemberId);

                    if (MembershipHelper.IsAuthenticated() && 
                        Settings.EnablePrivateMessaging && 
                        MessageToMember.getProperty("forumUserAllowPrivateMessages").Value.ToString() == "1" && 
                        !IsBanned)
                    {

                        //quick check to make sure the member is not PM'ing themselves
                        if (MessageToMember.Id == CurrentMember.MemberId) Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("CantMessageYourself")));

                        if (!Page.IsPostBack)
                        {
                            // See if there is a subject querystring, if so we know this is a reply
                            ReplySubject = null;
                            if (Request.QueryString["r"] != null)
                                ReplySubject = "RE: " + Request.QueryString["r"];
                            lvPrivateMessage.Visible = true;
                            //Now if this is a reply, prepopulate the subject textbox
                            var tbMessageSubject = (TextBox)lvPrivateMessage.FindControl("tbMessageSubject");
                            tbMessageSubject.Text = ReplySubject;
                        }

                            var litMemberTo = (Literal)lvPrivateMessage.FindControl("litMemberTo");
                            if (litMemberTo != null)
                                litMemberTo.Text = MessageToMember.LoginName;
                    }
                }
                catch(Exception)
                {
                    // Show friendly message
                    Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", "Unable to find that member"));
                }      
            }           
        }


        protected void BtnSubmitMessageClick(object sender, EventArgs e)
        {

            // Check user isn't spammer by checking flood control settings
            if (Helpers.TimeDifferenceInMinutes(DateTime.Now, CurrentMember.MemberLastPrivateMessageTime) > Settings.PrivateMessagingFloodControlTimeSpan)
            {
                var sb = new StringBuilder();

                var tbMessageSubject = (TextBox)lvPrivateMessage.FindControl("tbMessageSubject");
                var tbMessage = (TextBox)lvPrivateMessage.FindControl("txtPost");

                var replylink = string.Concat(Url(),
                                                "/privatemessage.aspx?mem=" + CurrentMember.MemberId,
                                                "&r=" + Helpers.GetSafeHtml(tbMessageSubject.Text).UrlEncode());

                replylink = string.Format("<a href='{0}'>{0}</a>", replylink);

                sb.AppendFormat(library.GetDictionaryItem("PrivateMessageEmailText"), 
                                CurrentMember.MemberLoginName, 
                                DateTime.Now.ToShortDateString(), 
                                Helpers.GetSafeHtml(tbMessageSubject.Text));

                sb.Append(Helpers.GetSafeHtml(tbMessage.Text).ConvertBbCode());

                sb.AppendFormat("<p><b>{0}</b></p><p>{1}</p>", library.GetDictionaryItem("ClickLinkToReply"), replylink);
                Helpers.SendMail(Settings.EmailNotification, MessageToMember.Email, string.Concat(library.GetDictionaryItem("PrivateMessageOn"), Settings.Name), sb.ToString());

                // Lastly update last private message sent on this user
                var cMem = new Member(Convert.ToInt32(CurrentMember.MemberId));
                cMem.getProperty("forumUserLastPrivateMessage").Value = DateTime.Now;
                cMem.Save();
                cMem.XmlGenerate(new System.Xml.XmlDocument());

                // Show friendly message
                Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=", library.GetDictionaryItem("MessageSent")));
            }
            else
            {
                // Show friendly message
                Response.Redirect(string.Concat(CurrentPageAbsoluteUrl, "?m=",
                                                string.Format(library.GetDictionaryItem("SorryMustWaitBetween"), 
                                                Settings.PrivateMessagingFloodControlTimeSpan)));
            }
        }
    }
}