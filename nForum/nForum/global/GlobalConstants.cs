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

        // default membergroup / project settings
        public static string PermissionKarmaAmount { get { return "forumCategoryPermissionKarmaAmount"; } }
        public static int PermissionKarmaAmountDefaultValue { get { return 30; } }
        public static string PermissionPostKarmaAmount { get { return "forumCategoryPostPermissionKarmaAmount"; } }
        public static int PermissionPostKarmaAmountDefaultValue { get { return 30; } }
        public static string IsMainCategory { get { return "forumCategoryIsMainCategory"; } }
     
    }
}