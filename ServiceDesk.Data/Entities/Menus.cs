using System;

namespace ServiceDesk.Data.Entities
{
    public class Menus : BaseEntity
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string Url { get; set; }
        public string FriendlyUrl { get; set; }
        public int? MenuLevel { get; set; }
        public int? Sort { get; set; }
        public int? ParentId { get; set; }
        public int? MenuIconId { get; set; }
        public bool? Active { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}