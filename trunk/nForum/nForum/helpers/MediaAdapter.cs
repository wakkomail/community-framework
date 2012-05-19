using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.media;

namespace nForum.helpers
{
    public class MediaAdapter : Media
    {
        public MediaAdapter(int id)
            : base(id)
        {
            
        }

        public static Media GetRootMediaByName(string mediaName)
        {
            foreach (Media mediaItem in Media.GetRootMedias())
            {
                if (mediaItem.Text == mediaName)
                {
                    return mediaItem;
                }
            }
            return null;
        }
    }
}