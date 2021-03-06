using System;
using ServiceDesk.Data.Entities;

namespace ServiceDesk.Data.ViewModels
{
    public class MenuViewModel //: BaseEntity
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string RussianMenuName { get; set; }
        public string Url { get; set; }
        public string FriendlyUrl { get; set; }
        public int? Sort { get; set; }
        public int? ParentId { get; set; }
        public int? MenuIconId { get; set; }
        public string IconName { get; set; }
        public bool Active { get; set; }
        public string LanguageId { get; set; }
        public string CreateUser { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}