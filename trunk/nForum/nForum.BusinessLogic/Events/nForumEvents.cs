using System;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.member;
using umbraco.cms.businesslogic.web;
using umbraco.NodeFactory;

namespace nForum.BusinessLogic.Events
{
    public class NForumEvents : ApplicationBase
    {
        public NForumEvents()
        {
            Document.New += DocumentNew;
            Document.AfterMove += DocumentAfterMove;
            Document.BeforePublish += DocumentBeforePublish;
            Member.AfterSave += MemberAfterSave;
        }

        static void MemberAfterSave(Member sender, SaveEventArgs e)
        {
            // Clear the member cache for this member, so if new properties are added
            // or profile updated its available straight away
            CacheHelper.Clear(CacheHelper.CacheNameMember(sender.Id));
        }

        static void DocumentAfterMove(object sender, MoveEventArgs e)
        {
            #region Forum Topic Move Events
            var sid = (CMSNode)sender;
            var s = new Document(sid.Id);

            if (s.ContentType.Alias == "CLC-Discussion" && s.ParentId != -20)
            {
                if (s.Parent != null)  //If top of tree, something is wrong.  Skip.
                {
                    try
                    {
                        var postDate = DateTime.Now;

                        string[] strArray = { postDate.Year.ToString(), postDate.Month.ToString(), postDate.Day.ToString() };
                        if (strArray.Length == 3)
                        {
                            var topPostLevel = new Node(s.Parent.Id);
                            //Traverse up the tree to find a forum category since we are likely in a Date Folder path
                            while (topPostLevel != null && topPostLevel.NodeTypeAlias != "CLC-Membergroup")
                            {
                                topPostLevel = topPostLevel.Parent != null ? new Node(topPostLevel.Parent.Id) : null;
                            }

                            if (topPostLevel != null)
                            {
                                Document document;
                                Node folderNode = null;
                                foreach (Node ni in topPostLevel.Children)
                                {
                                    if (ni.Name == strArray[0])
                                    {
                                        folderNode = new Node(ni.Id);
                                        document = new Document(ni.Id);
                                        break;
                                    }
                                }
                                if (folderNode == null)
                                {
                                    document = Document.MakeNew(strArray[0], DocumentType.GetByAlias("ForumDateFolder"), s.User, topPostLevel.Id);
                                    document.Publish(s.User);
                                    library.UpdateDocumentCache(document.Id);
                                    folderNode = new Node(document.Id);
                                }

                                Node folderNode2 = null;
                                foreach (Node ni in folderNode.Children)
                                {
                                    if (ni.Name == strArray[1])
                                    {
                                        folderNode2 = new Node(ni.Id);
                                        break;
                                    }
                                }
                                if (folderNode2 == null)
                                {
                                    var document2 = Document.MakeNew(strArray[1], DocumentType.GetByAlias("ForumDateFolder"), s.User, folderNode.Id);
                                    document2.Publish(s.User);
                                    library.UpdateDocumentCache(document2.Id);
                                    folderNode2 = new Node(document2.Id);
                                }

                                if (s.Parent.Id != folderNode2.Id)
                                {
                                    s.Move(folderNode2.Id);
                                }
                            }
                            else
                            {
                                Log.Add(LogTypes.Debug, s.User, s.Id, string.Format("Unable to determine top category for forum topic {0} while attempting to move to new Topic", s.Id));
                            }

                        }
                    }
                    catch (Exception exp)
                    {
                        Log.Add(LogTypes.Debug, s.User, s.Id, string.Format("Error while Finding Forum Folders for Forum Topic {0} while trying to move to new Topic.  Error: {1}", s.Id, exp.Message));
                    }

                    library.RefreshContent();
                }
            }
            #endregion

            #region Category

            if (s.ContentType.Alias == "CLC-Membergroup" && s.ParentId != -20)
            {
                s.getProperty("forumCategoryParentID").Value = s.ParentId;
            }

            #endregion
        }


        static void DocumentNew(Document sender, NewEventArgs e)
        {
            #region Forum Topic Events
            if (sender.ContentType.Alias == "CLC-Discussion")
            {
                if (sender.Parent != null)  //If top of tree, something is wrong.  Skip.
                {
                    try
                    {
                        var postDate = DateTime.Now;

                            string[] strArray = { postDate.Year.ToString(), postDate.Month.ToString(), postDate.Day.ToString() };
                            if (strArray.Length == 3)
                            {
                                var topPostLevel = new Node(sender.Parent.Id);
                                //Traverse up the tree to find a forum category since we are likely in a Date Folder path
                                while (topPostLevel != null && topPostLevel.NodeTypeAlias != "CLC-Membergroup")
                                {
                                    topPostLevel = topPostLevel.Parent != null ? new Node(topPostLevel.Parent.Id) : null;
                                }

                                if (topPostLevel != null)
                                {
                                    Document document;
                                    Node folderNode = null;
                                    foreach (Node ni in topPostLevel.Children)
                                    {
                                        if (ni.Name == strArray[0])
                                        {
                                            folderNode = new Node(ni.Id);
                                            document = new Document(ni.Id);
                                            break;
                                        }
                                    }
                                    if (folderNode == null)
                                    {
                                        document = Document.MakeNew(strArray[0], DocumentType.GetByAlias("ForumDateFolder"), sender.User, topPostLevel.Id);
                                        document.Publish(sender.User);
                                        library.UpdateDocumentCache(document.Id);
                                        folderNode = new Node(document.Id);
                                    }

                                    Node folderNode2 = null;
                                    foreach (Node ni in folderNode.Children)
                                    {
                                        if (ni.Name == strArray[1])
                                        {
                                            folderNode2 = new Node(ni.Id);
                                            break;
                                        }
                                    }
                                    if (folderNode2 == null)
                                    {
                                        var document2 = Document.MakeNew(strArray[1], DocumentType.GetByAlias("ForumDateFolder"), sender.User, folderNode.Id);
                                        document2.Publish(sender.User);
                                        library.UpdateDocumentCache(document2.Id);
                                        folderNode2 = new Node(document2.Id);
                                    }

                                    if (sender.Parent.Id != folderNode2.Id)
                                    {
                                        sender.Move(folderNode2.Id);
                                    }
                                }
                                else
                                {
                                    Log.Add(LogTypes.Debug, sender.User, sender.Id, string.Format("Unable to determine top category for forum topic {0} while attempting to move to new Topic", sender.Id));
                                }

                            }
                    }
                    catch (Exception exp)
                    {
                        Log.Add(LogTypes.Debug, sender.User, sender.Id, string.Format("Error while Finding Forum Folders for Forum Topic {0} while trying to move to new Topic.  Error: {1}", sender.Id, exp.Message));
                    }

                    library.RefreshContent();
                }
            }
            #endregion

            #region Category

            if (sender.ContentType.Alias == "CLC-Membergroup" && sender.ParentId != -20)
            {
                sender.getProperty("forumCategoryParentID").Value = sender.ParentId;
            }

            #endregion
        }

        static void DocumentBeforePublish(Document sender, PublishEventArgs e)
        {
            #region Forum Events
            if ((sender.ContentType.Alias == "CLC-Homepage"))
            {
                CacheHelper.Clear(CacheHelper.CacheNameSettings());
            }
            #endregion
        }
    }
}
