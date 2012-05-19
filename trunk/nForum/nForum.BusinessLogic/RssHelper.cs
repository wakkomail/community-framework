using System;
using System.Xml;

namespace nForum.BusinessLogic
{
    public class RssHelper
    {
        /// <summary>
        /// Writes the beginning of a RSS document to an XmlTextWriter
        /// </summary>
        /// <param name="writer">The XmlTextWriter to be written to</param>
        /// <param name="title"> </param>
        /// <param name="link"> </param>
        /// <returns>The XmlTextWriter with the header info written to it</returns>
        public XmlTextWriter WriteRssPrologue(XmlTextWriter writer, string title, string link)
        {
            writer.WriteStartDocument();
            writer.WriteStartElement("rss");
            writer.WriteAttributeString("version", "2.0");
            writer.WriteStartElement("channel");
            writer.WriteElementString("title", title);
            writer.WriteElementString("link", link);
            return writer;
        }

        /// <summary>
        /// Adds a single item to the XmlTextWriter supplied
        /// </summary>
        /// <param name="writer">The XmlTextWriter to be written to</param>
        /// <param name="sItemTitle">The title of the RSS item</param>
        /// <param name="sItemLink">The URL of the RSS item</param>
        /// <param name="sItemDescription">The RSS item text itself</param>
        /// <param name="sPubDate"></param>
        /// <returns>The XmlTextWriter with the item info written to it</returns>
        public XmlTextWriter AddRssItem(XmlTextWriter writer, string sItemTitle, string sItemLink, string sItemDescription, DateTime sPubDate)
        {
            writer.WriteStartElement("item");
            writer.WriteElementString("title", sItemTitle);
            writer.WriteElementString("link", sItemLink);
            writer.WriteElementString("description", sItemDescription);
            writer.WriteElementString("pubDate", sPubDate.ToString("r"));
            writer.WriteEndElement();

            return writer;
        }

        /// <summary>
        /// Adds a single item to the XmlTextWriter supplied
        /// </summary>
        /// <param name="writer">The XmlTextWriter to be written to</param>
        /// <param name="sItemTitle">The title of the RSS item</param>
        /// <param name="sItemLink">The URL of the RSS item</param>
        /// <param name="sItemDescription">The RSS item text itself</param>
        /// <param name="sPubDate"></param>
        /// <param name="bDescAsCdata">Write description as CDATA</param>
        /// <returns>The XmlTextWriter with the item info written to it</returns>
        public XmlTextWriter AddRssItem(XmlTextWriter writer, string sItemTitle, string sItemLink, string sItemDescription, DateTime sPubDate, bool bDescAsCdata)
        {
            writer.WriteStartElement("item");
            writer.WriteElementString("title", sItemTitle);
            writer.WriteElementString("link", sItemLink);

            if (bDescAsCdata)
            {
                // Now we can write the description as CDATA to support html content.
                // We find this used quite often in aggregators

                writer.WriteStartElement("description");
                writer.WriteCData(sItemDescription);
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteElementString("description", sItemDescription);
            }

            writer.WriteElementString("pubDate", sPubDate.ToString("r"));
            writer.WriteEndElement();

            return writer;
        }

        /// <summary>
        /// Finishes up the XmlTextWriter by closing open elements and the document itself
        /// </summary>
        /// <param name="writer">The XmlTextWriter to be written to</param>
        /// <returns>The XmlTextWriter with the footer info written to it</returns>
        public XmlTextWriter WriteRssClosing(XmlTextWriter writer)
        {
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndDocument();

            return writer;
        }
    }
}
