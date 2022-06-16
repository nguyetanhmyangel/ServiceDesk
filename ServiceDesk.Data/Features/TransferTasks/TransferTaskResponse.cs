using System;

namespace ServiceDesk.Data.Features.TransferTasks
{
    [Serializable]
    public class TransferTaskResponse //: BaseEntity
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool Active { get; set; }
    }
}