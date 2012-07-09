using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using umbraco.NodeFactory;
using nForum.global;

namespace nForum.helpers.businessobjects
{
    public class AgendaHelper
    {
        public static IEnumerable<Node> GetUpcomingEvents(int nodeId)
        {
            return NodeHelper.GetAllNodesByType(nodeId, GlobalConstants.AgendaItemAlias).Where(n => Convert.ToDateTime(n.GetProperty("date").Value).ToOADate() > System.DateTime.Now.AddDays(-1).ToOADate()).OrderBy(n => n.GetProperty("date").Value);
        }
    }
}