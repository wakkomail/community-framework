using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nForum.global
{
    public static class GlobalConstants
    {
        public static string SiteRootName { get { return "Creative Learning Community"; } }
        public static string SiteRootTemplate { get { return "CLC Homepage"; } }
        public static string DiscussionAlias { get { return "CLC-Discussion"; } }
        public static string FolderAlias { get { return "CLC-Folder"; } }

        // membergroup
        public static string MembergroupAlias { get { return "CLC-Membergroup"; } }
        public static string MembergroupFolderName { get { return "Membergroups"; } }
        public static string MembergroupTemplateName { get { return "CLCMembergroup"; } }

        // project
        public static string ProjectFolderName { get { return "Projects"; } }
        public static string ProjectAlias { get { return "CLC-Project"; } }
        public static string ProjectTemplateName { get { return "CLCProject"; } }

        // default membergroup / project settings
        public static string PermissionKarmaAmount { get { return "forumCategoryPermissionKarmaAmount"; } }
        public static int PermissionKarmaAmountDefaultValue { get { return 0; } }
        public static string PermissionPostKarmaAmount { get { return "forumCategoryPostPermissionKarmaAmount"; } }
        public static int PermissionPostKarmaAmountDefaultValue { get { return 0; } }
        public static string IsMainCategory { get { return "forumCategoryIsMainCategory"; } }
     
        // membership
        public static string MemberTypeAlias { get { return "ForumUser"; } }

        // notices
        public static string NoticeAlias { get { return "CLC-Notice"; } }
        public static string NoticeBoardFolder { get { return "Noticeboard"; } }
        public static string NoticeBoardAlias { get { return "CLC-Noticeboard"; } }

        // agenda
        public static string AgendaItemAlias { get { return "CLC-AgendaItem"; } }
        public static string AgendaBoardFolder { get { return "Agenda"; } }
        public static string AgendaAlias { get { return "CLC-Agenda"; } }
    }
}