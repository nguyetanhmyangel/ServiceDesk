using System;

namespace ServiceDesk.Data.ViewModels
{
    [Serializable]
    public class DepartmentViewModel //: BaseEntity
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentRussianName { get; set; }
        public int DivisionId { get; set; }
    }
}