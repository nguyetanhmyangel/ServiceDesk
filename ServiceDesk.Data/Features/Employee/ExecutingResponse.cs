using System;

namespace ServiceDesk.Data.Features.Employee
{
    public class ExecutingResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Progress { get; set; }
        public string Description { get; set; }
    }
}