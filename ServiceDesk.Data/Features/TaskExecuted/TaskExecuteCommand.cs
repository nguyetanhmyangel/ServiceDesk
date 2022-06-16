using System;

namespace ServiceDesk.Data.Features.TaskExecuted
{
    public class TaskExecuteCommand//: BaseEntity
    {
        public int Id { get; set; } 
        public int TaskId { get; set; }
        //public int IssueId { get; set; }
        public string UserId { get; set; }
        public int Progress { get; set; }
        public DateTime? FinishDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; } 
        public string UpdateUser { get; set; } 
        public DateTime UpdateDate { get; set; } 
        public string Description { get; set; }
        public int StatusId { get; set; }
    }
}