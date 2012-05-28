using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.media;
using nForum.global;
using umbraco.BusinessLogic;

namespace nForum.helpers
{
    public class MediaAdapter
    {
        public static Media GetMediaFolderByName(string mediaName)
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

        public static Media GetOrCreateMembergroupCategory()
        {
            Media rootMedia = GetRootMedia();
            foreach (Media document in rootMedia.GetDescendants())
            {
                if (document.Text == GlobalConstants.MembergroupFolderName)
                {
                    return document;
                }
            }

            // folder document does not exists, create it
            Media newMembergroupFolder = Media.MakeNew(GlobalConstants.MembergroupFolderName, MediaType.GetByAlias(GlobalConstants.FolderAlias), User.GetUser(0), rootMedia.Id);

            return newMembergroupFolder;
        }

        public static Media GetRootMedia()
        {
            foreach (Media media in Media.GetRootMedias())
            {
                if (media.Text == GlobalConstants.SiteRootName)
                {
                    return media;
                }
            }
            return null;
        }


    }
}