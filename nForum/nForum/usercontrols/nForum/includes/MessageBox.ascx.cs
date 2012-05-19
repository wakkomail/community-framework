using System;

namespace nForum
{
    public partial class MessageBox : System.Web.UI.UserControl
    {
        #region Properties
        public bool ShowCloseButton { get; set; }

        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ShowCloseButton)
                CloseButton.Attributes.Add("onclick", "document.getElementById('" + pnlMessageBox.ClientID + "').style.display = 'none'");
        }
        #endregion

        #region Wrapper methods
        public void ShowError(string message)
        {
            Show(MessageType.Error, message);
        }

        public void ShowInfo(string message)
        {
            Show(MessageType.Info, message);
        }

        public void ShowSuccess(string message)
        {
            Show(MessageType.Success, message);
        }

        public void ShowWarning(string message)
        {
            Show(MessageType.Warning, message);
        }
        #endregion

        #region Show control
        public void Show(MessageType messageType, string message)
        {
            CloseButton.Visible = ShowCloseButton;
            litMessage.Text = message;

            pnlMessageBox.CssClass = messageType.ToString().ToLower();
            this.Visible = true;
        }
        #endregion

        #region Enum
        public enum MessageType
        {
            Error = 1,
            Info = 2,
            Success = 3,
            Warning = 4
        }
        #endregion
    }
}