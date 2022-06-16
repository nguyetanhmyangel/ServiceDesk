using Dapper;
using Dapper.Transaction;
using Npgsql;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ServiceDesk.Data.Features.Task;

namespace ServiceDesk.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public TaskRepository()
        {
        }

        public bool Add(TaskCommand model)
        {
            throw new NotImplementedException();
        }

        public bool Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(string multiDepartmentIds, int issueId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                // use Isolation Level in Transaction
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        // insert new department to Task if not exist
                        var updateTaskQuery = "update \"Tasks\" set \"IsSendMail\" = 't'" +
                            " where \"DepartmentId\" in ( " + multiDepartmentIds + ") and \"IssueId\" = " + issueId;
                        transaction.Execute(updateTaskQuery);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        //ex.Message.ToString();
                        transaction.Rollback();
                    }
                }
            }
        }

        public void Update(string multiDepartmentIds, string multiUserIds, int issueId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                // use Isolation Level in Transaction
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        if (multiDepartmentIds.Length > 0)
                        {
                            var updateTaskQuery = "update \"Tasks\" set \"IsSendMail\" = 't'" +
                            " where \"DepartmentId\" in ( " + multiDepartmentIds + ") and \"IssueId\" = " + issueId + " " +
                            "and  EXISTS (select 1 from \"Tasks\" where \"IssueId\" = " + issueId + ")";
                            transaction.Execute(updateTaskQuery);
                        }


                        if (multiUserIds.Length > 0)
                        {
                            var updateTaskExcuteQuery = "update \"TaskExecutes\" a set \"IsSendMail\" = 't' " +
                                "from \"Tasks\" a1 inner join \"Issues\" a2 on a2.\"Id\" = a1.\"IssueId\" " +
                                "where a.\"TaskId\" = a1.\"Id\" and a.\"UserId\" in ( " + multiUserIds + ") and a2.\"Id\" = " + issueId + " and " +
                                "EXISTS(select 1 from \"TaskExecutes\" b " +
                                "inner join \"Tasks\" b1 on b.\"TaskId\" = b1.\"Id\" " +
                                "inner join \"Issues\" b2 on b2.\"Id\" = b1.\"IssueId\")";
                            transaction.Execute(updateTaskExcuteQuery);
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        //ex.Message.ToString();
                        transaction.Rollback();
                    }
                }
            }
        }

        //dang test
        public bool Update(TaskCommand model, IEnumerable<object> multiDepartments)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                // use Isolation Level in Transaction
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        var statusId = model.StatusId == Config.Waiting ? Config.Processing : model.StatusId;
                        var newDepartments = multiDepartments.ToList();
                        if (newDepartments.Any())
                        {
                            foreach (var newDepartmentId in newDepartments.Select(Helper.ConvertToInt))
                            {
                                // insert new department to Task if not exist
                                transaction.Execute("insert into \"Tasks\" (\"DepartmentId\", " +
                                    "\"IssueId\", \"StatusId\",\"PriorityId\", \"Description\", \"CreateUser\") " +
                                    "Values (@DepartmentId,  @IssueId, @StatusId, @PriorityId, @Description, @CreateUser) " +
                                    "ON CONFLICT (\"IssueId\", \"DepartmentId\") DO NOTHING",
                                    new
                                    {
                                        DepartmentId = newDepartmentId,
                                        IssueId = model.IssueId,
                                        StatusId = Config.Waiting,
                                        PriorityId = model.PriorityId,
                                        Description = model.Description,
                                        CreateUser = model.UpdateUser
                                    });

                            }
                        }

                        // Update Tasks with current department and current Issue have status is Cancel
                        if (model.StatusId == Config.Complete || model.StatusId == Config.Cancel)
                        {
                            transaction.Execute("Update \"Issues\" set \"StatusId\" = @StatusId " +
                            "where \"Id\" = @IssueId",
                            new
                            {
                                StatusId = statusId,
                                IssueId = model.IssueId
                            });

                            transaction.Execute("Update \"Tasks\" set \"StatusId\" = @StatusId " +
                            "where \"IssueId\" = @IssueId EXISTS(select 1 from \"Tasks\" where \"IssueId\" = @IssueId)",
                            new
                            {
                                StatusId = statusId,
                                IssueId = model.IssueId
                            });

                            transaction.Execute("update \"TaskExecutes\" a set \"StatusId\" = @StatusId " +
                                "from \"Tasks\" a1 inner join \"Issues\" a2 on a2.\"Id\" = a1.\"IssueId\" " +
                                "where a.\"TaskId\" = a1.\"Id\" and a1.\"IssueId\" = @IssueId and " +
                                "EXISTS(select 1 from \"TaskExecutes\" b " +
                                "inner join \"Tasks\" b1 on b.\"TaskId\" = b1.\"Id\" " +
                                "inner join \"Issues\" b2 on b2.\"Id\" = b1.\"IssueId\"  where b2.\"Id\" = @IssueId)", new
                                {
                                    StatusId = statusId,
                                    IssueId = model.IssueId
                                });

                        }
                        else
                        {
                            transaction.Execute("Update \"Issues\" set \"StatusId\" = @StatusId ," +
                            "\"Description\" = @Description, \"PriorityId\" = @PriorityId where \"Id\" = @IssueId",
                            new
                            {
                                StatusId = statusId,
                                Description = model.Description,
                                PriorityId = model.PriorityId,
                                IssueId = model.IssueId
                            });
                        }
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

        private bool CheckExist()
        {

            return false;
        }

        public bool Update(TaskCommand model, IEnumerable<object> multiDepartments, IEnumerable<object> multiUsers)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                // use Isolation Level in Transaction
                using (var transaction = dbConnection.BeginTransaction(IsolationLevel.Serializable))
                {
                    try
                    {
                        //var currentStatusId = 0;
                        //if (model.StatusId == Config.Cancel || model.StatusId == Config.Complete)
                        //    currentStatusId = 

                        //var exist = transaction.QueryFirstOrDefault<bool>("select case when exists (SELECT 1 FROM \"Tasks\" a " +
                        //    "WHERE a.\"IssueId\" = @IssueId and a.\"DepartmentId\" = @DepartmentId  " +
                        //    "then CAST('1' as bool) else CAST('0' as bool) end", 
                        //    new { IssueId = model.IssueId, DepartmentId = model.DepartmentId });

                        var newTaskId = model.TaskId;

                        if (model.TaskId > 0)
                        {
                            transaction.Execute("Update \"Tasks\" set \"StartDate\" = @StartDate, " +
                            "\"EndDate\" = @EndDate, \"StatusId\" = @StatusId , \"PriorityId\" = @PriorityId ," +
                            "\"PrivateDescription\" = @PrivateDescription , \"UpdateUser\" = @UpdateUser " +
                            "where \"Id\" = @TaskId",
                            new
                            {
                                StartDate = model.StartDate,
                                EndDate = model.EndDate,
                                StatusId = model.StatusId,
                                PriorityId = model.PriorityId,
                                PrivateDescription = model.PrivateDescription,
                                UpdateUser = model.UpdateUser,
                                TaskId = model.TaskId ///---
                            });
                        }
                        else
                        {
                            newTaskId = transaction.ExecuteScalar<int>("insert into \"Tasks\" (\"DepartmentId\", " +
                                    "\"IssueId\", \"StatusId\",\"PriorityId\", \"Description\", \"CreateUser\") " +
                                    "Values (@DepartmentId,  @IssueId, @StatusId, @PriorityId, @Description, @CreateUser) " +
                                    "ON CONFLICT (\"IssueId\", \"DepartmentId\") DO NOTHING",
                                    new
                                    {
                                        DepartmentId = model.DepartmentId,
                                        IssueId = model.IssueId,
                                        StatusId = Config.Processing,
                                        PriorityId = model.PriorityId,
                                        Description = model.Description,
                                        CreateUser = model.UpdateUser
                                    });
                        }

                        // add users
                        var users = multiUsers.ToList();
                        if (users.Any())
                        {
                            // set query
                            var updateTaskExecutes = "Update \"TaskExecutes\" a  set \"StatusId\" = " + Config.Cancel +
                                "from \"Users\" b " +
                                "where a.\"UserId\" = b.\"UserId\" and a.\"UserId\" not in (";

                            foreach (var userId in users)
                            {
                                transaction.Execute("insert into \"TaskExecutes\" (\"TaskId\", \"UserId\"," +
                                                    "\"CreateUser\") Values (@TaskId, @UserId, @CreateUser) " +
                                                    "ON CONFLICT (\"TaskId\", \"UserId\") DO NOTHING",
                                    new
                                    {
                                        TaskId = newTaskId,
                                        UserId = userId,
                                        StatusId = Config.Waiting,
                                        CreateUser = model.UpdateUser ///---
                                    });

                                updateTaskExecutes += userId + ",";
                            }

                            // set status TaskExecute is Cancel user not in list

                            // update TaskExecutes of Task with current User
                            if (model.StatusId == Config.Cancel || model.StatusId == Config.Complete)
                            {
                                transaction.Execute("Update \"TaskExecutes\" a set \"StatusId\" = @StatusId " +
                                    "from \"Users\" b where a.\"UserId\" = b.\"UserId\" " +
                                    "and a.\"TaskId\" = @TaskId and b.\"DepartmentId\" = @DepartmentId and " +
                                    "EXISTS(select 1 from \"TaskExecutes\" b " +
                                    "inner join \"Tasks\" b1 on b.\"TaskId\" = b1.\"Id\" " +
                                    "where b1.\"Id\" = @TaskId)",
                                new
                                {
                                    StatusId = model.StatusId,
                                    TaskId = newTaskId,
                                    DepartmentId = model.DepartmentId
                                });
                            }
                            else
                            {
                                updateTaskExecutes = updateTaskExecutes.Remove(updateTaskExecutes.Length - 1, 1) +
                                    ") and a.\"TaskId\" = " + newTaskId +
                                    " and b.\"DepartmentId\" = " + model.DepartmentId + " and " +
                                    "EXISTS(select 1 from \"TaskExecutes\" b " +
                                    "inner join \"Tasks\" b1 on b.\"TaskId\" = b1.\"Id\" " +
                                    "where b1.\"Id\" = " + @newTaskId  + ")";

                                transaction.Execute(updateTaskExecutes);
                            }
                        }

                        // add departments
                        var newDepartments = multiDepartments.ToList();
                        if (newDepartments.Any())
                        {
                            foreach (var newDepartmentId in newDepartments.Select(Helper.ConvertToInt))
                            {
                                // insert new department to Task if not exist
                                transaction.Execute("insert into \"Tasks\" (\"DepartmentId\", \"IssueId\", \"StatusId\", \"CreateUser\") " +
                                                    "Values (@DepartmentId,  @IssueId, @StatusId, @CreateUser) " +
                                                    "ON CONFLICT (\"IssueId\", \"DepartmentId\") DO NOTHING",
                                    new
                                    {
                                        DepartmentId = newDepartmentId,
                                        IssueId = model.IssueId,
                                        StatusId = Config.Waiting,
                                        CreateUser = model.UpdateUser
                                    });
                            }
                        }

                        // update status of Issue
                        //transaction.Execute("call \"UpdateIssueStatus\"(" + model.IssueId + ")");

                        transaction.Execute("UPDATE \"Issues\" SET \"StatusId\" = (select case when not exists " +
                            "(SELECT 1 FROM \"Tasks\" a WHERE a.\"IssueId\" = @IssueId and " +
                            "a.\"StatusId\" != 3 ) then " + Config.Cancel + " when not exists " +
                            "(SELECT 1 FROM \"Tasks\" a1 WHERE a1.\"IssueId\" = @IssueId " +
                            "and a1.\"StatusId\" not in (3,5)) then " + Config.Complete +
                            " when \"StatusId\" = 6 then 1  ELSE \"StatusId\" end) where \"Id\" = @IssueId",
                            new { IssueId = model.IssueId });

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


        public IEnumerable<TaskResponse> FindAll(int statusId, DateTime fromDate, DateTime toDate)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@StatusId", statusId);
                parameters.Add("@FromDate", fromDate);
                parameters.Add("@ToDate", toDate);
                return dbConnection.Query<TaskResponse>("select * from \"TaskList\"(@StatusId, @FromDate, @ToDate) ", parameters);
            }
        }

        public IEnumerable<TaskResponse> FindAllByFunction()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TaskResponse> FindById(int issueId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@IssueId", issueId);
                return dbConnection.Query<TaskResponse>("select a.\"Id\",a.\"IssueId\",a.\"DepartmentId\",a1.\"DepartmentName\"," +
                    "a.\"StartDate\",\"EndDate\",a.\"StatusId\",a2.\"StatusName\",a.\"Description\" from \"Tasks\" a " +
                    "INNER JOIN \"DepartmentViews\" a1 on a.\"DepartmentId\" = a1.\"DepartmentId\" " +
                    "INNER JOIN \"Status\" a2 on a2.\"Id\" = a.\"StatusId\" " +
                    "where a.\"IssueId\" = @IssueId", parameters);
            }
        }

        public IEnumerable<TaskResponse> FindByDepartmentId(int departmentId, int statusId, DateTime fromDate, DateTime toDate)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@DepartmentId", departmentId);
                parameters.Add("@StatusId", statusId);
                parameters.Add("@FromDate", fromDate);
                parameters.Add("@ToDate", toDate);
                return dbConnection.Query<TaskResponse>("select * from \"DepartmentTaskList\"(@DepartmentId, @StatusId, @FromDate, @ToDate) ", parameters);
            }
        }

        public IEnumerable<TaskTimeLife> FindForTimLife(int issueId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<TaskTimeLife>("select a.\"Id\", a.\"DepartmentId\", " +
                    "a.\"IssueId\", d.\"DepartmentName\", a.\"StatusId\",e.\"StatusName\", " +
                    "to_char(a.\"StartDate\", 'DD/MM/YYYY') as \"StartDate\", " +
                    "to_char(a.\"EndDate\", 'DD/MM/YYYY') as \"EndDate\", " +
                    "to_char(a.\"CreateDate\", 'DD/MM/YYYY') as \"CreateDate\", " +
                    "a.\"CreateUser\", to_char(a.\"UpdateDate\", 'DD/MM/YYYY') as \"UpdateDate\", " +
                    "a.\"UpdateUser\", a.\"Description\", a.\"TransferDepartmentId\", " +
                    "string_agg(c.\"FullName\", ',') as \"UserHandleList\" " +
                    "from \"Tasks\" a left join \"TaskExecutes\" b on a.\"Id\" = b.\"TaskId\" " +
                    "left join \"Users\" c on b.\"UserId\" = c.\"UserId\" " +
                    "left join \"DepartmentViews\" d on d.\"DepartmentId\" = a.\"DepartmentId\" " +
                    "left join \"Status\" e on e.\"Id\" = a.\"StatusId\" " +
                    "where a.\"IssueId\" = @IssueId " +
                    "group by a.\"Id\", a.\"DepartmentId\", d.\"DepartmentName\", " +
                    "e.\"StatusName\" ", new { IssueId = issueId });
            }
        }

    }
}