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
            Document rootDoc = GetRootDocument(); 
            foreach (Document document in rootDoc.GetDescendants())
            {
                if (document.Text == GlobalConstants.MembergroupFolderName)
                {
                    return document;
                }
            }

            // folder document does not exists, create it
            Document newMembergroupFolder = Document.MakeNew(GlobalConstants.MembergroupFolderName, DocumentType.GetByAlias(GlobalConstants.FolderAlias), User.GetUser(0), rootDoc.Id);

            return newMembergroupFolder;
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
    }
}