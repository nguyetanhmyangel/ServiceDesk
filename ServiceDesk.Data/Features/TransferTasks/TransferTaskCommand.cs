using System;

namespace ServiceDesk.Data.Features.TransferTasks
{
    public class TransferTaskCommand //: BaseEntity
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int DepartmentId { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool Active { get; set; }
    }
}