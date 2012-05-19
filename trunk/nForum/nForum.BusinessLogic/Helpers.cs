using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Microsoft.Security.Application;
using nForum.BusinessLogic.Models;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.NodeFactory;
using umbraco.interfaces;

namespace nForum.BusinessLogic
{
    public static class Helpers
    {

        #region Social Helpers
        public static string GetGravatarImage(string email, int size)
        {
            return IsValidEmail(email) ? string.Format("http://www.gravatar.com/avatar/{0}?s={1}", library.md5(email), size) : "";
        } 
        #endregion

        #region Date Helpers
        public static string FormatLongDate(DateTime theDate)
        {
            return theDate.ToString("dd MMMM yyyy");
        }

        public static DateTime ParseDate(object theDate)
        {
            DateTime date;
            return DateTime.TryParse(theDate.ToString(), out date) ? date : DateTime.Now;
        }

        public static string GetPrettyDate(string date)
        {
            return GetPrettyDate(date, "dd MMMM yyyy");
        }

        public static string GetPrettyDate(string date, string format)
        {
            DateTime time;
            if (DateTime.TryParse(date, out time))
            {
                var span = DateTime.Now.Subtract(time);
                var totalDays = (int)span.TotalDays;
                var totalSeconds = (int)span.TotalSeconds;
                if ((totalDays < 0) || (totalDays >= 0x1f))
                {
                    return FormatDateTime(date, format);
                }
                if (totalDays == 0)
                {
                    if (totalSeconds < 60)
                    {
                        return library.GetDictionaryItem("JustNow");
                    }
                    if (totalSeconds < 120)
                    {
                        return library.GetDictionaryItem("OneMinuteAgo");
                    }
                    if (totalSeconds < 0xe10)
                    {
                        return string.Format(library.GetDictionaryItem("MinutesAgoFormat"), Math.Floor((double)(((double)totalSeconds) / 60.0)));
                    }
                    if (totalSeconds < 0x1c20)
                    {
                        return library.GetDictionaryItem("OneHourAgo");
                    }
                    if (totalSeconds < 0x15180)
                    {
                        return string.Format(library.GetDictionaryItem("HoursAgoFormat"), Math.Floor((double)(((double)totalSeconds) / 3600.0)));
                    }
                }
                if (totalDays == 1)
                {
                    return library.GetDictionaryItem("Yesterday");
                }
                if (totalDays < 7)
                {
                    return string.Format(library.GetDictionaryItem("DaysAgoFormat"), totalDays);
                }
                if (totalDays < 0x1f)
                {
                    return string.Format(library.GetDictionaryItem("WeeksAgoFormat"), Math.Ceiling((double)(((double)totalDays) / 7.0)));
                }
            }
            return date;
        }

        public static string FormatDateTime(string date, string format)
        {
            DateTime time;
            if (DateTime.TryParse(date, out time) && !string.IsNullOrEmpty(format))
            {
                format = Regex.Replace(format, @"(?<!\\)((\\\\)*)(S)", "$1" + GetDayNumberSuffix(time));
                return time.ToString(format);
            }
            return string.Empty;
        }

        private static string GetDayNumberSuffix(DateTime date)
        {
            switch (date.Day)
            {
                case 1:
                case 0x15:
                case 0x1f:
                    return @"\s\t";

                case 2:
                case 0x16:
                    return @"\n\d";

                case 3:
                case 0x17:
                    return @"\r\d";
            }
            return @"\t\h";
        }

        public static double TimeDifferenceInMinutes(DateTime dateone, DateTime datetwo)
        {
            var duration = dateone - datetwo;
            return duration.TotalMinutes;
        }

        #endregion

        #region String Helpers

        /// <summary>
        /// Gets the current domain name of the site
        /// </summary>
        /// <returns></returns>
        public static string ReturnSiteDomainName()
        {
            var sPath = HttpContext.Current.Request.Url.ToString().Replace("http://", "");
            string url;
            if (sPath.Contains("/"))
            {
                var strarry = sPath.Split('/');
                url = strarry[0];
            }
            else
            {
                url = sPath;
            }
            return string.Concat("http://", url);
        }

        public static List<int> StringArrayToIntList(string stringarray)
        {
            // Empty string
            var list = string.IsNullOrEmpty(stringarray) ? null : stringarray.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            return list;
        }

        public static string CreateTwitterLinkFromUsername(string username)
        {
            return username != null ? string.Format("<a href='http://twitter.com/{0}'>{0}</a>", username) : null;
        }

        public static string CalculateSha1(string userPassword) 
        { return BitConverter.ToString(SHA1.Create().ComputeHash(Encoding.Default.GetBytes(userPassword))).Replace("-", ""); }

        private static readonly Regex DigitRegex = new Regex(@"[^\d]");

        public static string StripNonNumerics(string source)
        {
            return DigitRegex.Replace(source, "");
        }

        public static List<string> ReturnDaysInMonth()
        {
            var l = new List<string>();
            for (var i = 1; i < 32; i++)
            {
                l.Add(i.ToString());
            }
            return l;
        }

        public static List<string> ReturnYearsForCreditCard()
        {
            var l = new List<string>();
            var yend = (DateTime.Now.Year + 16);
            var ystart = (DateTime.Now.Year - 15);
            for (var i = ystart; i < yend; i++)
            {
                l.Add(i.ToString());
            }
            return l;
        }

        public static bool IsNumeric(object expression)
        {
            double retNum;
            var isNum = Double.TryParse(Convert.ToString(expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        public static string StringReverse(string str)
        {
            var len = str.Length;
            var arr = new char[len];
            for (var i = 0; i < len; i++)
            {
                arr[i] = str[len - 1 - i];
            }
            return new string(arr);
        }

        public static int CountWordsInString(string text)
        {
            if (String.IsNullOrEmpty(text))
            { return 0; }
            var tmpStr = text.Replace("\t", " ").Trim();
            tmpStr = tmpStr.Replace("\n", " ");
            tmpStr = tmpStr.Replace("\r", " ");
            while (tmpStr.IndexOf("  ") != -1)
                tmpStr = tmpStr.Replace("  ", " ");
            return tmpStr.Split(' ').Length;
        }

        public static string ReturnAmountWordsFromString(string text, int wordAmount)
        {
            string tmpStr;
            string[] stringArray;
            var tmpStrReturn = "";
            tmpStr = text.Replace("\t", " ").Trim();
            tmpStr = tmpStr.Replace("\n", " ");
            tmpStr = tmpStr.Replace("\r", " ");

            while (tmpStr.IndexOf("  ") != -1)
            {
                tmpStr = tmpStr.Replace("  ", " ");
            }
            stringArray = tmpStr.Split(' ');

            if (stringArray.Length < wordAmount)
            {
                wordAmount = stringArray.Length;
            }
            for (int i = 0; i < wordAmount; i++)
            {
                tmpStrReturn += stringArray[i] + " ";
            }
            return tmpStrReturn;
        }

        public static string GetSafeHtml(string input)
        {
            return Sanitizer.GetSafeHtmlFragment(input);
        }

        public static string HtmlEncode(string input)
        {
            return Microsoft.Security.Application.Encoder.HtmlEncode(input);
        }

        public static string HtmlDecode(string input)
        {
            return WebUtility.HtmlDecode(input);
        }

        public static string UrlEncode(string input)
        {
            return Microsoft.Security.Application.Encoder.UrlEncode(input);
        }

        public static string StripHtmlFromString(string input)
        {
            //var regex = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", RegexOptions.Singleline);
            input = Regex.Replace(input, @"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>", String.Empty, RegexOptions.Singleline);
            return input;
        }

        public static string SafePlainText(string input)
        {
            input = StripHtmlFromString(input);
            input = GetSafeHtml(input);
            return input;
        }

        public static string ReturnHtmlLink(string href, string anchortext)
        {
            return string.Format("<a href=\"{0}\">{1}</a>", href, anchortext);
        }

        public static string StripNonAlphaNumeric(string strInput, string replaceWith)
        {
            strInput = Regex.Replace(strInput, "[^\\w]", replaceWith);
            return strInput;
        }

        public static string CreateUrl(string strInput, string replaceWith)
        {
            return StripNonAlphaNumeric(strInput, replaceWith).ToLower();
        }

        public static string FormatCurrency(int? amount)
        {
            return amount != null ? string.Format("{0:C}", amount) : "n/a";
        }

        public static string FormatCurrency(int amount)
        {
            return string.Format("{0:C}", amount);
        }

        public static List<string> ReturnNodeIdsFromCsv(string csv)
        {
            var s = csv.TrimEnd(',').Split(',');
            return s.ToList();
        }

        /// <summary>
        /// A method to convert basic BBCode to HTML
        /// </summary>
        /// <param name="str">A string formatted in BBCode</param>
        /// <returns>The HTML representation of the BBCode string</returns>
        public static string ConvertBbCodeToHtml(string str)
        {
            // format the bold tags: [b][/b]
            // becomes: <strong></strong>
            var exp = new Regex(@"\[b\](.+?)\[/b\]");
            str = exp.Replace(str, "<strong>$1</strong>");

            // format the italic tags: [i][/i]
            // becomes: <em></em>
            exp = new Regex(@"\[i\](.+?)\[/i\]");
            str = exp.Replace(str, "<em>$1</em>");

            // format the underline tags: [u][/u]
            // becomes: <u></u>
            exp = new Regex(@"\[u\](.+?)\[/u\]");
            str = exp.Replace(str, "<u>$1</u>");

            // format the code tags: [code][/code]
            // becomes: <pre></pre>
            exp = new Regex(@"\[code\](.+?)\[/code\]");
            str = exp.Replace(str, "<pre>$1</pre>");

            // format the code tags: [quote][/quote]
            // becomes: <blockquote></blockquote>
            exp = new Regex(@"\[quote\](.+?)\[/quote\]");
            str = exp.Replace(str, "<blockquote>$1</blockquote>");

            // format the strike tags: [s][/s]
            // becomes: <strike></strike>
            exp = new Regex(@"\[s\](.+?)\[/s\]");
            str = exp.Replace(str, "<strike>$1</strike>");

            //### Before this replace links without http ###
            str.Replace("[url=www.", "[url=http://www.");
            // format the url tags: [url=www.website.com]my site[/url]
            // becomes: <a href="www.website.com">my site</a>
            exp = new Regex(@"\[url\=([^\]]+)\]([^\]]+)\[/url\]");
            str = exp.Replace(str, "<a rel=\"nofollow\" href=\"$1\">$2</a>");

            // format the img tags: [img]www.website.com/img/image.jpeg[/img]
            // becomes: <img src="www.website.com/img/image.jpeg" />
            exp = new Regex(@"\[img\]([^\]]+)\[/img\]");
            str = exp.Replace(str, "<img src=\"$1\" />");

            // format img tags with alt: [img=www.website.com/img/image.jpeg]this is the alt text[/img]
            // becomes: <img src="www.website.com/img/image.jpeg" alt="this is the alt text" />
            exp = new Regex(@"\[img\=([^\]]+)\]([^\]]+)\[/img\]");
            str = exp.Replace(str, "<img src=\"$1\" alt=\"$2\" />");

            //format the colour tags: [color=red][/color]
            // becomes: <font color="red"></font>
            // supports UK English and US English spelling of colour/color
            exp = new Regex(@"\[color\=([^\]]+)\]([^\]]+)\[/color\]");
            //str = exp.Replace(str, "<font color=\"$1\">$2</font>");
            str = exp.Replace(str, "<span style=\"color:$1;\">$2</span>");
            exp = new Regex(@"\[colour\=([^\]]+)\]([^\]]+)\[/colour\]");
            //str = exp.Replace(str, "<font color=\"$1\">$2</font>");
            str = exp.Replace(str, "<span style=\"color:$1;\">$2</span>");

            // format the size tags: [size=1.2][/size]
            // becomes: <span style="font-size:1.2em;"></span>
            exp = new Regex(@"\[size\=([^\]]+)\]([^\]]+)\[/size\]");
            str = exp.Replace(str, "<span style=\"font-size:$1em;\">$2</span>");

            // YouTube Insert Video, just add the video ID and it inserts video into post
            exp = new Regex(@"\[youtube\]([^\]]+)\[/youtube\]");
            str = exp.Replace(str, "<iframe title=\"YouTube video player\" width=\"640\" height=\"390\" src=\"http://www.youtube.com/embed/$1\" frameborder=\"0\" allowfullscreen></iframe>");

            // YouTube Insert Video, just add the video ID and it inserts video into post
            exp = new Regex(@"\[vimeo\]([^\]]+)\[/vimeo\]");
            str = exp.Replace(str, "<iframe src=\"http://player.vimeo.com/video/$1?portrait=0\" width=\"591\" height=\"332\" frameborder=\"0\"></iframe>");

            return str;
        }

        #endregion

        #region Validation Helpers
        private static bool IsValidEmail(string email)
        {
            var r = new Regex(@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$");

            return !string.IsNullOrEmpty(email) && r.IsMatch(email);
        } 
        #endregion

        #region XML Helpers
        public static XmlDocument ReturnXmlFromUrl(string url)
        {

            //request the particular web page
            var request = (HttpWebRequest)WebRequest.Create(url);

            ////define the login credentials of the requested file/page
            //request.Credentials = new NetworkCredential("User Name", "Password");

            //get the response from the request
            var response = (HttpWebResponse)request.GetResponse();

            //create a stream to hold the contents of the response (in this case it is the contents of the XML file
            var receiveStream = response.GetResponseStream();

            //create your XML document
            var mySourceDoc = new XmlDocument();

            //load the file from the stream
            mySourceDoc.Load(receiveStream);

            //close the stream
            receiveStream.Close();

            return mySourceDoc;

        }

        public static string ReturnRssFeedInList(string url, int howMany)
        {
            // Get the rss feed
            var x = ReturnXmlFromUrl(url);

            // Create a string builder for the feed
            var sb = new StringBuilder();

            // Try and get the movie
            var nodes = x.GetElementsByTagName("item");
            var i = 0;
            sb.Append("<ul>");
            foreach (XmlNode node in nodes)
            {
                if (i <= (howMany - 1))
                {
                    sb.Append("<li>");
                    if (node != null)
                        sb.Append(String.Format("<a target='_blank' href='{0}'>{1}</a>", node["link"].InnerText, node["title"].InnerText));
                    sb.Append("</li>");
                }
                i++;
            }
            sb.Append("</ul>");
            return sb.ToString().Trim();
        }
        #endregion

        #region Email Helpers

        public static string EmailTemplate(string emailMessage)
        {
            using (var sr = File.OpenText(HttpContext.Current.Server.MapPath(@"~/nforum/emailtemplates/main.txt")))
            {
                var sb = sr.ReadToEnd();
                sr.Close();
                sb = sb.Replace("#CONTENT#", emailMessage);
                return sb;
            }
        }

        public static void SendMail(string mailto, string mailSubject, string mailBody)
        {
            var e = MainForumSettings().EmailAdmin;
            SendMail(e, mailto, mailSubject, mailBody);
        }

        public static void SendMail(string mailfrom, string mailto, string mailSubject, string mailBody)
        {
            library.SendMail(mailfrom, mailto, mailSubject, EmailTemplate(mailBody), true);
        }

        public static void SendMail(string mailfrom, List<string> mailto, string mailSubject, string mailBody)
        {
            try
            {
                var smtpSec = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                if (smtpSec == null) return;

                var mySmtpClient = new SmtpClient(smtpSec.Network.Host);
                mySmtpClient.Credentials = new NetworkCredential(smtpSec.Network.UserName, smtpSec.Network.Password);
                var msg = new MailMessage
                {
                    IsBodyHtml = true,
                    Body = mailBody,
                    From = new MailAddress(mailfrom),
                    Subject = mailSubject
                };

                foreach (var email in mailto)
                {
                    msg.To.Add(email);
                }

                mySmtpClient.Send(msg);
            }
            catch (Exception)
            {
                Log.Add(LogTypes.Debug, Node.GetCurrent().Id, string.Format("Error sending forum emails, check your SMTP settings"));
            }
        }

        #endregion

        #region Umbraco Specific

        public static string NiceTopicUrl(int topicid, string nodename)
        {
            var extension = ".aspx";
            if(GlobalSettings.UseDirectoryUrls)
            {
                extension = "";
            }
            return string.Format("/t{0}/{1}{2}",
                                 topicid,
                                 CreateUrl(nodename, "-"),
                                 extension);
        }

        public static string GetDictionaryItem(string item, int lang)
        {
            string translation;
            try
            {
                translation = new umbraco.cms.businesslogic.Dictionary.DictionaryItem(item).Value(lang);
            }
            catch (Exception)
            {
                translation = string.Empty;
            }
            // need to check for empty string to show keyname when item not translated 
            if (string.IsNullOrEmpty(translation))
                translation = item;
            return translation;
        }

        /// <summary>
        /// Finds the top level forum node
        /// </summary>
        /// <returns></returns>
        public static INode FindForumRoot()
        {
            return FindForumRoot(Node.GetCurrent());
        }
        private static INode FindForumRoot(INode currentNode)
        {
            return currentNode.NodeTypeAlias == "CLC-Homepage" ? currentNode : FindForumRoot(currentNode.Parent);
        }

        /// <summary>
        /// Returns the main forum cached for speed, used as the general settings for the site
        /// </summary>
        /// <returns></returns>
        public static Forum MainForumSettings()
        {
            Forum settings;
            if (!CacheHelper.Get(CacheHelper.CacheNameSettings(), out settings))
            {
                settings = new Forum();
                CacheHelper.Add(settings, CacheHelper.CacheNameSettings());
            }
            return settings;
        }

        /// <summary>
        /// Returns the correct alternate template Url
        /// </summary>
        /// <param name="altTemplateUrl"></param>
        /// <param name="currentpage"></param>
        /// <returns></returns>
        public static string AlternateTemplateUrlFix(string altTemplateUrl, string currentpage)
        {
            if (!GlobalSettings.UseDirectoryUrls)
            {
                const string pageExt = ".aspx";
                currentpage = currentpage.Replace(pageExt, altTemplateUrl + pageExt);
            }
            else
            {
                if (currentpage.EndsWith("/"))
                {
                    currentpage = currentpage + altTemplateUrl.Replace("/", "");
                }
                else
                {
                    currentpage = currentpage + altTemplateUrl;
                }
            }
            return currentpage;
        }

        /// <summary>
        /// Helper to return null if a date property has no value
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime? InternalDateFixer(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                return null;
            }

            return Convert.ToDateTime(date);
        }

        /// <summary>
        /// Helpers to find nodes anywhere in the site
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<Node> FindChildren(Node currentNode, Func<Node, bool> predicate)
        {
            var result = new List<Node>();

            var nodes = currentNode
                .Children
                .OfType<Node>().Where(predicate);
            if (nodes.Count() != 0)
                result.AddRange(nodes);
            foreach (var child in currentNode.Children.OfType<Node>())
            {
                nodes = FindChildren(child, predicate);
                if (nodes.Count() != 0)
                    result.AddRange(nodes);
            }
            return result;
        }

        #endregion
    }
}
