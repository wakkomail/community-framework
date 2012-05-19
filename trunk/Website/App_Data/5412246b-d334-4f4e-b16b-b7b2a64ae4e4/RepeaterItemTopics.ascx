<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RepeaterItemTopics.ascx.cs" Inherits="nForum.usercontrols.nForum.templates.RepeaterItemTopics" %>
<%@ Import Namespace="nForum.BusinessLogic" %>

            <div class="forumtopic">

                <div class='forumtopiclink link<%#FTopic.IsSolved %>'>

                        <h3><a href="<%# FTopic.Url%>"<%# ShowAjaxPostLink(FTopic.Id)%>><%# FTopic.Name%></a></h3>
                        <p class="topicsubtext">
                            Started By: <%# MembershipHelper.ReturnMemberProfileLink(FTopic.Owner.MemberLoginName, FTopic.Owner.MemberId, null)%> - 
                                                            <%# Helpers.GetPrettyDate(FTopic.CreatedOn.ToString())%>
                        </p>
                    
                </div>
                
                <div class="forumtopicreplies">
                    <span><%# (FTopic.PostCount() - 1) %></span>
                    Replies
                </div>

                <div class="forumtopicvotes">
                    <span><%# FTopic.PostVotesTotal() %></span>Votes
                </div>

                <div class="forumtopiclatestpost">
                    <%# GetLastPostInfo(FTopic)%>
                </div>

            </div>