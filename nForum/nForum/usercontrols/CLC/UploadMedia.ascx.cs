using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using umbraco.cms.businesslogic.member;
using umbraco.cms.businesslogic.media;
using nForum.BusinessLogic.Models;
using nForum.BusinessLogic;
using umbraco.BusinessLogic;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Xml;
using System.Configuration;
using nForum.helpers;
using umbraco.NodeFactory;

namespace nForum.usercontrols.CLC
{
    public partial class UploadMedia : BaseForumUsercontrol
    {
        ForumCategory currentCategory = new ForumCategory();

        protected void Page_Load(object sender, EventArgs e)
        {
            // set permissions

            if (CurrentMember != null && (MembershipHelper.IsMember(currentCategory.Name) || CurrentMember.MemberIsAdmin))
            {
                Initialize();
            }
            else
            {
                this.pnlUpload.Visible = false;
            }
            
        }

        private void Initialize()
        {
            currentCategory = Mapper.MapForumCategory(CurrentNode);
        }

        // Helper Method to upload and store your image in the media tree
        protected Media SaveFile(FileUpload uploadControl)
        {
            bool isImage = false;
            string mediaPath = "";
            Media mediaFile = null;
            Media parentMedia = null;
            bool categoryExists = false;

            if (uploadControl.PostedFile != null)
            {
                if (uploadControl.PostedFile.FileName != "")
                {
                    // Find filename

                    string filename = uploadControl.PostedFile.FileName; ;
                    string fullFilePath;

                    filename = filename.Substring(filename.LastIndexOf("\\") + 1, filename.Length - filename.LastIndexOf("\\") - 1).ToLower();

                    // create the Media Node

                    // get file extension
                    string orgExt = ((string)filename.Substring(filename.LastIndexOf(".") + 1, filename.Length - filename.LastIndexOf(".") - 1));
                    orgExt = orgExt.ToLower();
                    string ext = orgExt.ToLower();

                    MediaType type;

                    if (",jpeg,jpg,gif,bmp,png,tiff,tif,".IndexOf("," + ext + ",") > 0)
                    {
                        isImage = true;
                        type = MediaType.GetByAlias("image");
                    }
                    else
                    {
                        type = MediaType.GetByAlias("file");
                    }

                    //check if category allready exists in library, create it if it does not extists
                    parentMedia = MediaHelper.GetMediaFolderByName(currentCategory.Name, Node.GetCurrent().Parent.Name);
                    categoryExists = (parentMedia != null);

                    if (categoryExists == false)
                    {
                        parentMedia = Media.MakeNew(currentCategory.Name, MediaType.GetByAlias("folder"), User.GetUser(0), -1);
                    }

                    mediaFile = Media.MakeNew(filename, type, User.GetUser(0), parentMedia.Id);

                    // Create a new folder in the /media folder with the name /media/propertyid
                    // string mediaRootPath = HttpContext.GetGlobalResourceObject("appSettings", "MediaFilePath") as string;
                    // string mediaRootPath = ConfigurationSettings.AppSettings["MediaFilePath"];
                    //string mediaRootPath = System.Configuration.ConfigurationManager.AppSettings["MediaFilePath"];
					string mediaRootPath = Server.MapPath("~/media/");

                    string storagePath = mediaRootPath + mediaFile.Id.ToString();
                    System.IO.Directory.CreateDirectory(storagePath);
                    fullFilePath = storagePath + "\\" + filename;
                    uploadControl.PostedFile.SaveAs(fullFilePath);

                    // Save extension
                    try
                    {
                        mediaFile.getProperty("umbracoExtension").Value = ext;
                    }
                    catch { }

                    // Save file size
                    try
                    {
                        System.IO.FileInfo fi = new FileInfo(fullFilePath);
                        mediaFile.getProperty("umbracoBytes").Value = fi.Length.ToString();
                    }
                    catch { }

                    // Check if image and then get sizes, make thumb and update database
                    if (isImage == true)
                    {
                        int fileWidth;
                        int fileHeight;

                        FileStream fs = new FileStream(fullFilePath,
                                FileMode.Open, FileAccess.Read, FileShare.Read);

                        System.Drawing.Image image = System.Drawing.Image.FromStream(fs);
                        fileWidth = image.Width;
                        fileHeight = image.Height;
                        fs.Close();
                        try
                        {
                            mediaFile.getProperty("umbracoWidth").Value = fileWidth.ToString();
                            mediaFile.getProperty("umbracoHeight").Value = fileHeight.ToString();
                        }
                        catch { }

                        // Generate thumbnails
                        string fileNameThumb = fullFilePath.Replace("." + orgExt, "_thumb");
                        generateThumbnail(image, 100, fileWidth, fileHeight, fullFilePath, ext, fileNameThumb + ".jpg");

                        
                        image.Dispose();
                    }
                    mediaPath = "/media/" + mediaFile.Id.ToString() + "/" + filename;

                    mediaFile.getProperty("umbracoFile").Value = mediaPath;
                    mediaFile.XmlGenerate(new XmlDocument());
                }
            }

            // return the media...
            return mediaFile;
        }

        protected void generateThumbnail(System.Drawing.Image image, int maxWidthHeight, int fileWidth, int fileHeight, string fullFilePath, string ext, string thumbnailFileName)
        {
            // Generate thumbnail
            float fx = (float)fileWidth / (float)maxWidthHeight;
            float fy = (float)fileHeight / (float)maxWidthHeight;
            // must fit in thumbnail size
            float f = Math.Max(fx, fy); //if (f < 1) f = 1;
            int widthTh = (int)Math.Round((float)fileWidth / f); int heightTh = (int)Math.Round((float)fileHeight / f);

            // fixes for empty width or height
            if (widthTh == 0)
                widthTh = 1;
            if (heightTh == 0)
                heightTh = 1;

            // Create new image with best quality settings
            Bitmap bp = new Bitmap(widthTh, heightTh);
            Graphics g = Graphics.FromImage(bp);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Copy the old image to the new and resized
            Rectangle rect = new Rectangle(0, 0, widthTh, heightTh);
            g.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

            // Copy metadata
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo codec = null;
            for (int i = 0; i < codecs.Length; i++)
            {
                if (codecs[i].MimeType.Equals("image/jpeg"))
                    codec = codecs[i];
            }

            // Set compression ratio to 90%
            EncoderParameters ep = new EncoderParameters();
            ep.Param[0] = new EncoderParameter(Encoder.Quality, 90L);

            // Save the new image
            bp.Save(thumbnailFileName, codec, ep);
            bp.Dispose();
            g.Dispose();

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            int mediaId = SaveFile(uploadMedia).Id;

            Response.Redirect(Request.RawUrl);

        }

    }
}