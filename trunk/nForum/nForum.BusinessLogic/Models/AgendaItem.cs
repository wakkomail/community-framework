using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nForum.BusinessLogic.Models
{
    public class AgendaItem
    {
        public AgendaItem()
        {

        }

        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }
    }
}
