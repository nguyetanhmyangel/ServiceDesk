using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDesk.Data.Features.Menu
{
    public class SidebarResponse
    {
        //a."MenuId", a."Url", a."FriendlyUrl", a."Sort", a."ParentId", a."MenuIconId", a."Active", 
        //c."MenuName" , b."IconName"
        public int MenuId { get; set; }
        public string Url { get; set; }
        public string FriendlyUrl { get; set; }
        public int? Sort { get; set; }
        public int? ParentId { get; set; }
        public int? MenuIconId  { get; set; }
        public bool Active { get; set; }
        public string MenuName { get; set; }
        public string IconName { get; set; }
    }
}
