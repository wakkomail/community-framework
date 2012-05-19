using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace nForum.BusinessLogic
{
    public class PaginatedList<T> : List<T>
    {

        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(IEnumerable<T> source, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = source.Count();
            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            AddRange(source.Skip(PageIndex * PageSize).Take(PageSize));
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex + 1 < TotalPages);
            }
        }

        public string ReturnPager()
        {
            var page = HttpContext.Current.Request.Path;
            const string qs = "?p=";
            var url = page + qs;
            string addcurrentclass;
            var sb = new StringBuilder();
            sb.AppendFormat("<ul class='{0}'>", "pager");
            for (var i = 0; i < TotalPages; i++)
            {
                addcurrentclass = PageIndex == i ? " class='current'" : string.Empty;
                sb.AppendFormat("<li{2}><a href='{0}'>{1}</a></li>", url + i, (i + 1), addcurrentclass);
            }
            sb.Append("</ul>");
            return sb.ToString();
        }
    }
}