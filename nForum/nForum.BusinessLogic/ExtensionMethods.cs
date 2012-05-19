using System;

namespace nForum.BusinessLogic
{
    public static class ExtensionMethods
    {
        public static int ToInt32(this string theInput)
        {
            return !string.IsNullOrEmpty(theInput) ? Convert.ToInt32(theInput) : 0;
        }
        public static int ToInt32(this int? theInput)
        {
            return theInput != null ? Convert.ToInt32(theInput) : 0;
        }
        public static DateTime? ToDateTime(this string theInput)
        {
            if(theInput != null)
            {
                return Convert.ToDateTime(theInput);
            }
            return null;
        }

        public static string ConvertBbCode (this string theInput)
        {
            // If you need to do more text transformations just add them here
            return !string.IsNullOrEmpty(theInput) ? Helpers.ConvertBbCodeToHtml(theInput).Trim() : theInput;
        }

        public static string UrlEncode(this string theInput)
        {
            return !string.IsNullOrEmpty(theInput) ? Helpers.UrlEncode(theInput) : theInput;
        }

    }


}
