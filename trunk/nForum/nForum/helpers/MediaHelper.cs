using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.media;
using nForum.global;
using umbraco.BusinessLogic;

namespace nForum.helpers
{
    public class MediaHelper
    {
        public static Media GetMediaFolderByName(string mediaName, string parentFolder)
        {
            foreach (Media mediaFolder in GetRootMedia().GetDescendants())
            {
                if (mediaFolder.Text == parentFolder)
                {
                    foreach (Media mediaItem in mediaFolder.GetDescendants())
                    {
                        if (mediaItem.Text == mediaName)
                        {
                            return mediaItem;
                        }                        
                    }
                }
            }
            return null;
        }

        public static Media GetOrCreateMembergroupCategory()
        {
            return GetOrCreateCategory(GlobalConstants.MembergroupFolderName);            
        }

        public static Media GetOrCreateProjectCategory()
        {
            return GetOrCreateCategory(GlobalConstants.ProjectFolderName);
        }

        private static Media GetOrCreateCategory(string folderName)
        {
            Media rootMedia = GetRootMedia();

            if (rootMedia == null)
            {
                Media.MakeNew(GlobalConstants.SiteRootName, MediaType.GetByAlias("folder"), User.GetUser(0), 0);
            }

            // if rootMedia is null, create root Node
            foreach (Media document in rootMedia.GetDescendants())
            {
                if (document.Text == folderName)
                {
                    return document;
                }
            }

            // folder document does not exists, create it
            Media newMembergroupFolder = Media.MakeNew(folderName, MediaType.GetByAlias("folder"), User.GetUser(0), rootMedia.Id);

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