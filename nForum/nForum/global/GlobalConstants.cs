using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace nForum.global
{
    public  class GlobalConstants
    {
        public const string SiteRootName = "Creative Learning Community"; 
        public const string SiteRootTemplate = "CLC Homepage"; 
        public const string DiscussionAlias = "CLC-Discussion"; 
        public const string FolderAlias = "CLC-Folder"; 

        // membergroup
        public const string MembergroupAlias = "CLC-Membergroup";
        public const string MembergroupFolderName = "Kennisgroepen"; 
        public const string MembergroupTemplateName = "CLCMembergroup";       

        // project
        public const string ProjectFolderName = "Projecten"; 
        public const string ProjectAlias = "CLC-Project"; 
        public const string ProjectTemplateName = "CLCProject"; 

        // default membergroup / project settings
        public const string PermissionKarmaAmount = "forumCategoryPermissionKarmaAmount"; 
        public const int PermissionKarmaAmountDefaultValue = 0; 
        public const string PermissionPostKarmaAmount = "forumCategoryPostPermissionKarmaAmount"; 
        public const int PermissionPostKarmaAmountDefaultValue = 0; 
        public const string IsMainCategory = "forumCategoryIsMainCategory"; 
        public const string DateFolderAlias = "ForumDateFolder";
        public const string DescriptionField = "forumCategoryDescription"; 
     
        // membership
        public const string MemberTypeAlias = "ForumUser"; 

        // notices
        public const string NoticeAlias = "CLC-Notice"; 
        public const string NoticeBoardFolder = "Noticeboard"; 
        public const string NoticeBoardAlias = "CLC-Noticeboard"; 

        // agenda
        public const string AgendaItemAlias = "CLC-AgendaItem"; 
        public const string AgendaFolder = "Agenda"; 
        public const string AgendaAlias = "CLC-Agenda"; 
        public const string AgendaTemplateAlias = "CLCAgenda"; 

        // nforum
        public const int SummaryMaxLength = 150;
    }
}