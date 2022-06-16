using Dapper;
using Dapper.Transaction;
using Npgsql;
using ServiceDesk.Data.Features.TaskExecuted;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ServiceDesk.Data.Repositories
{
    public class TaskExecuteRepository : ITaskExecuteRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public TaskExecuteRepository()
        {
        }

        public bool Add(TaskExecuteCommand model)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool Update(TaskExecuteCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        dbConnection.Execute("Update \"TaskExecutes\" set \"Progress\" = @Progress, " +
                                             "\"FinishDate\" = @FinishDate,\"Description\" = @Description where \"Id\" = @Id",
                            new
                            {
                                FinishDate = model.FinishDate,
                                Progress = model.Progress,
                                Description = model.Description,
                                Id = model.Id
                            }, transaction: transaction);

                        //if (model.Progress == 100)
                        //{
                        //    transaction.Execute("update \"Tasks\" set \"StatusId\" = 5 " +
                        //                        "where \"Id\" = @Id and not EXISTS(select 1 from \"TaskExecutes\" where \"Progress\" <> 100 and  \"TaskId\" = @TaskId)",
                        //        new { Id = model.TaskId, DepartmentIssueId = model.TaskId });
                        //}

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        //ex.Message.ToString();
                        transaction.Rollback();
                        return false;
                    }
                    return true;
                }
            }
        }

        //public IEnumerable<TaskExecuteResponse> FindByTaskId(int taskId)
        //{
        //    using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
        //    {
        //        if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
        //        return dbConnection.Query<TaskExecuteResponse>("select a.\"Id\", a.\"Progress\", a.\"Description\", " +
        //               "a1.\"UserName\", a1.\"FullName\" from \"TaskExecutes\" a " +
        //               "inner join \"Users\" a1 on a.\"UserId\" = a1.\"UserId\" " +
        //               "where \"TaskId\" = @TaskId", new { TaskId = taskId });
        //    }
        //}

        public IEnumerable<TaskExecuteResponse> FindByTaskId(int taskId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<TaskExecuteResponse>("select a.\"Id\", a.\"Progress\", a.\"Description\", " +
                       "a1.\"UserName\", a1.\"FullName\", a2.\"StartDate\", a2.\"EndDate\", a.\"FinishDate\", a.\"StatusId\", a3.\"StatusName\" from \"TaskExecutes\" a " +
                       "inner join \"Users\" a1 on a.\"UserId\" = a1.\"UserId\" " +
                       "inner join \"Tasks\" a2 on a.\"TaskId\" = a2.\"Id\" " +
                       "inner join \"Status\" a3 on a.\"StatusId\" = a3.\"Id\" " +
                       "where \"TaskId\" = @TaskId", new { TaskId = taskId });
            }
        }

        public IEnumerable<TaskExecuteResponse> FindByUserId(string userName, int statusId, string languageId, DateTime fromDate, DateTime toDate)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", userName);
                parameters.Add("@LanguageId", languageId);
                parameters.Add("@StatusId", statusId);
                parameters.Add("@FromDate", fromDate);
                parameters.Add("@ToDate", toDate);
                return dbConnection.Query<TaskExecuteResponse>("select * from \"TaskExecuteList\"(@UserName, @StatusId, @LanguageId,  @FromDate, @ToDate) ", parameters);
            }
        }

        public IEnumerable<TaskExecuteTimeLife> FindForTimLife(int taskId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<TaskExecuteTimeLife>("select b.\"FullName\",a.\"Progress\", " +
                   "to_char(a.\"CreateDate\", 'DD/MM/YYYY') as \"CreateDate\", " +
                   "a.\"Description\", c.\"StatusName\" " +
                   "from \"TaskExecutes\" a " +
                   "inner join \"Users\" b on a.\"UserId\" = b.\"UserId\" " +
                   "inner join \"Status\" c on a.\"StatusId\" = c.\"Id\" " +
                   "where a.\"TaskId\" = @TaskId ", new { TaskId = taskId });
            }
        }

        public IEnumerable<TaskExecuteTimeLife> FindForTimLife(IEnumerable<int> listTaskId)
        {
            var ids = "";
            var taskId = listTaskId as int[] ?? listTaskId.ToArray();
            if (taskId.Any())
            {
                ids = taskId.Aggregate(ids, (current, id) => current + (id + ","));
            }

            var idList = ids.Length > 0 ? ids.Remove(ids.Length - 1, 1) : "";

            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<TaskExecuteTimeLife>("select a3.\"EmployeeId\", a3.\"EmployeeName\", a.\"Progress\", " +
                       "a.\"Description\", a2.\"StatusName\", a3.\"Mobile\", a3.\"Phone\", a3.\"Mobile\", a3.\"Email\", " +
                       "to_char(a.\"CreateDate\", 'DD/MM/YYYY') as \"CreateDate\"," +
                       "to_char(a.\"FinishDate\", 'DD/MM/YYYY') as \"FinishDate\" " +
                       "from \"TaskExecutes\" a " +
                       "inner join \"Users\" a1 on a.\"UserId\" = a1.\"UserId\" " +
                       "inner join \"Status\" a2 on a.\"StatusId\" = a2.\"Id\" " +
                       "inner join \"Employees\" a3 on a1.\"UserName\" = a3.\"EmployeeId\" " +
                       "where a.\"TaskId\" in(" + idList + ")");
            }
        }

        public IEnumerable<TaskExecuteResponse> FindByIssueId(int issueId, string languageId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@IssueId", issueId);
                parameters.Add("@LanguageId", languageId);
                return dbConnection.Query<TaskExecuteResponse>("select a.\"Id\", a4.\"EmployeeId\", a4.\"EmployeeName\", a4.\"Mobile\", a4.\"Phone\", " +
                        "a4.\"Email\", a.\"Description\", a1.\"StartDate\", a1.\"EndDate\", a.\"FinishDate\", a.\"Progress\", " +
                        "case when @LanguageId = 'ru-RU' then a5.\"DepartmentRussianName\" ELSE a5.\"DepartmentName\" end as \"DepartmentName\"  " +
                        "from \"TaskExecutes\" a " +
                        "inner join \"Tasks\" a1 on a.\"TaskId\" = a1.\"Id\" " +
                        "inner join \"Issues\" a2 on a1.\"IssueId\" = a2.\"Id\" " +
                        "inner join \"Users\" a3 on a.\"UserId\" = a3.\"UserId\" " +
                        "inner join \"Employees\" a4 on a3.\"UserName\" = a4.\"EmployeeId\" " +
                        "inner join \"DepartmentViews\" a5 on a4.\"DepartmentId\" = a5.\"DepartmentId\" " +
                        "where a1.\"IssueId\" = @IssueId ", parameters);
            }
        }

        public IEnumerable<TaskExecuteResponse> FindByIssueId(int issueId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@IssueId", issueId);
                return dbConnection.Query<TaskExecuteResponse>("" +
                    "select a.\"Id\", a4.\"EmployeeId\", a4.\"EmployeeName\", a4.\"Mobile\", a4.\"Phone\", " +
                    "a4.\"Email\", a.\"Description\", a.\"FinishDate\", a.\"Progress\" " +
                    "from \"TaskExecutes\" a" +
                    "" +
                    "" +
                    "" +
                    "" +
                    "where a1.\"IssueId\" = @IssueId ", parameters);
            }
        }

        public IEnumerable<TaskExecuteResponse> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskExecuteResponse> FindAllByFunction()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskExecuteResponse> FindById(int id)
        {
            throw new NotImplementedException();
        }
    }
}