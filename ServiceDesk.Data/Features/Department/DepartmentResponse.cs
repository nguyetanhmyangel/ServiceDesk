using System;

namespace ServiceDesk.Data.Features.Department
{
    [Serializable]
    public class DepartmentResponse //: BaseEntity
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentRussianName { get; set; }
        public int DivisionId { get; set; }
    }
}