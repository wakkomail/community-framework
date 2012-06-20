using System;
using System.Text.RegularExpressions;

namespace nForum.helpers
{
    public static class HtmlHelper
    {
        public static string StripHTML(this String input)
        {
            return Regex.Replace(input, @"<(.|\n)*?>", string.Empty);
        }
    }
}