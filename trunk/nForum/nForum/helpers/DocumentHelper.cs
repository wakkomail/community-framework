using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.cms.businesslogic.web;
using nForum.global;
using umbraco.BusinessLogic;

namespace nForum.helpers
{
    public class DocumentHelper
    {

        /// <summary>
        /// check membergroup categoryfolder allready exists in contenttree, create it if it does not extists
        /// </summary>
        /// <returns></returns>
        public static Document GetOrCreateMembergroupCategory()
        {
            return GetOrCreateCategory(GlobalConstants.MembergroupFolderName);
        }

        /// <summary>
        /// check projectgroup categoryfolder allready exists in contenttree, create it if it does not exists
        /// </summary>
        /// <returns></returns>
        public static Document GetOrCreateProjectgroupCategory()
        {
            return GetOrCreateCategory(GlobalConstants.ProjectFolderName);
        }


        private static Document GetOrCreateCategory(string categoryName)
        {
            Document rootDoc = GetRootDocument();
            foreach (Document document in rootDoc.GetDescendants())
            {
                if (document.Text == categoryName)
                {
                    return document;
                }
            }

            // folder document does not exists, create it
            Document newCategoryFolder = Document.MakeNew(categoryName, DocumentType.GetByAlias(GlobalConstants.FolderAlias), User.GetUser(0), rootDoc.Id);
            // publish membergroupfolder
            newCategoryFolder.Publish(User.GetUser(0));
            // clear document cache
            umbraco.library.UpdateDocumentCache(newCategoryFolder.Id);

            return newCategoryFolder;
        }


        public static Document GetRootDocument()
        {
            foreach (Document document in Document.GetRootDocuments())
            {
                if (document.Text == GlobalConstants.SiteRootName )
                {
                    return document;
                }
            }
            return null;
        }

        public static Document GetRootFolderByName(string folderName)
        {
            foreach (Document document in GetRootDocument().GetDescendants())
            {
                if (document.Text == folderName)
                {
                    return document;
                }
            }
            return null;
        }
    }
}