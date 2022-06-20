using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.User;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using Dapper.Transaction;

namespace ServiceDesk.Data.Repositories
{
    public class UserRepository : IUserRepository//<Users>
    {
        public UserRepository()
        {
        }

        public bool Add(UserCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    const string sqlQuery = "INSERT INTO \"Users\" (\"UserName\",\"Password\",\"Email\"," +
                                            "\"RoleId\",\"Active\",\"FullName\",\"DepartmentId\", \"PositionId\") Values " +
                                            "(@UserName, @Password, @Email, @RoleId, @Active, @FullName, @DepartmentId, @PositionId)";
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserName", model.UserName);
                    parameters.Add("@Password", model.Password);
                    parameters.Add("@Email", model.Email);
                    parameters.Add("@RoleId", model.RoleId);
                    parameters.Add("@Active", model.Active);
                    parameters.Add("@FullName", model.FullName);
                    parameters.Add("@DepartmentId", model.DepartmentId);
                    parameters.Add("@PositionId", model.PositionId);
                    try
                    {
                        dbConnection.Execute(sqlQuery, parameters,transaction);

                        transaction.Execute("UPDATE \"Employees\" SET \"Password\" = @Password " +
                                            "WHERE \"EmployeeId\" = @EmployeeId ",
                            new { EmployeeId = model.UserName, Password = model.Password });
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
                    

                return true;
            }
        }

        public bool Update(UserCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    var parameters = new DynamicParameters();
                    const string sqlQuery = "UPDATE \"Users\" SET \"Email\" = @Email, " +
                                            "\"RoleId\" = @RoleId, \"Active\" = @Active, \"Password\" = @Password," +
                                            "\"DepartmentId\" = @DepartmentId, \"PositionId\" = @PositionId " +
                                            "WHERE \"UserId\" = @UserId";
                    parameters.Add("@Email", model.Email);
                    parameters.Add("@RoleId", model.RoleId);
                    parameters.Add("@Active", model.Active);
                    parameters.Add("@DepartmentId", model.DepartmentId);
                    parameters.Add("@PositionId", model.PositionId);
                    parameters.Add("@Password", model.Password);
                    parameters.Add("@UserId", model.UserId);
                    try
                    {
                        dbConnection.Query(sqlQuery, parameters,transaction);

                        transaction.Execute("UPDATE \"Employees\" SET \"Password\" = @Password " +
                                            "WHERE \"EmployeeId\" = @EmployeeId ",
                            new { Password = model.Password, EmployeeId = model.UserName });

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                    

                return true;
            }
        }

        public string FindLeaderEmail(int departmentId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<string>("select string_agg(\"Users\".\"Email\", " +
                    "',') as Email from \"Users\" where \"DepartmentId\" = " +
                    "@DepartmentId and \"Active\" = 'true' and \"PositionId\" in (3,4)", new { DepartmentId = departmentId });
            }
        }

        public string FindLeaderEmail(string departmentList, int issueId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                //var t = "select string_agg(\"Users\".\"Email\",',') as Email from \"Users\" where \"DepartmentId\" in (" + departmentList + ") and \"PositionId\" in (3,4)";
                return dbConnection.QueryFirstOrDefault<string>("select string_agg(\"Users\".\"Email\", " +
                    "',') as Email from \"Users\" " +
                    "where \"DepartmentId\" in (" + departmentList + ") and \"Active\" = 'true' and \"PositionId\" in (3,4) " +
                    "and \"DepartmentId\" not in (select \"DepartmentId\" from \"Tasks\" " +
                    "where \"IssueId\" = " + issueId +  " and \"IsSendMail\" = 't')");               
            }
        }

        public string FindPersonnelEmail(string userList, int departmentId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<string>("select string_agg(a.\"Email\",',') as Email " +
                    "from \"Users\" a where a.\"UserId\" in (" + userList + ") and a.\"Active\" = 't' " +
                    "and \"UserId\" not in (select b1.\"UserId\" from \"TaskExecutes\" b1 " +
                    "inner join \"Tasks\" b2 on b1.\"TaskId\" = b2.\"Id\" " +
                    "where b2.\"IssueId\" = 232 and b1.\"IsSendMail\" = 't')");
            }
        }

        public string FindPersonnelEmail(int taskId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<string>("select string_agg(b.\"Email\",',') as Email " +
                    "from \"TaskExecutes\" a inner join \"Users\" b on a.\"UserId\" = b.\"UserId\" " +
                    "where a.\"TaskId\" = @TaskId and a.\"StatusId\" != 3",
                    new { TaskId = taskId });
            }
        }

        public IEnumerable<UserResponse> FindByName(string userName)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "select a.\"UserId\", a.\"Password\", a.\"Email\", a.\"Active\", " +
                    "a.\"FullName\", a.\"DepartmentId\", a.\"UserName\", a.\"RoleId\", b.\"RoleName\", " +
                    "c.\"DepartmentName\", d.\"DivisionName\",a.\"PositionId\", e.\"PositionName\" from \"Users\" a " +
                    "inner join \"Roles\" b on a.\"RoleId\" = b.\"RoleId\" " +
                    "inner join \"DepartmentViews\" c on a.\"DepartmentId\" = c.\"DepartmentId\" " +
                    "inner join \"DivisionViews\" d on c.\"DivisionId\" = d.\"DivisionId\" " +
                    "inner join \"Positions\" e on a.\"PositionId\" = e.\"Id\" " +
                    "where a.\"Active\" = 'true' and (a.\"UserName\" ilike '%' || @UserName || '%' or " +
                    "a.\"FullName\" ilike '%' || @UserName || '%') and c.\"DivisionId\" = 10 " +
                    "order by a.\"UserId\"";
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", userName);
                return dbConnection.Query<UserResponse>(sqlQuery, parameters);
            }
        }

        public IEnumerable<UserResponse> FindAll()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<UserResponse>("SELECT * FROM \"Users\" where \"Active\" = 'true'");
            }
        }

        public IEnumerable<UserResponse> FindByFunction()
        {
            throw new NotImplementedException();
        }


        public IEnumerable<UserResponse> FindUserByDepartmentId(int departmentId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<UserResponse>("select \"UserId\",\"UserName\",\"FullName\" from \"Users\" " +
                    "where \"Active\" = 'true' and \"DepartmentId\" = @DepartmentId", new { DepartmentId = departmentId });
            }
        }

        public IEnumerable<UserResponse> FindUserByDepartmentId(int departmentId, bool isAdmin)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                if (isAdmin)
                {
                    return dbConnection.Query<UserResponse>("select \"UserId\",\"UserName\",\"FullName\" from \"Users\" " +
                    "where \"Active\" = 'true'");
                }
                else
                {
                    return dbConnection.Query<UserResponse>("select \"UserId\",\"UserName\",\"FullName\" from \"Users\" " +
                    "where \"Active\" = 'true' and \"DepartmentId\" = @DepartmentId", new { DepartmentId = departmentId });
                }
            }
        }

        public IEnumerable<UserResponse> FindAllUser()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<UserResponse>("select \"UserId\",\"UserName\",\"FullName\" from \"Users\" " +
                    "where \"Active\" = 'true'");
            }
        }

        public int FindCountExecuting(int userId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<int>("select COALESCE (c.\"CountExecuting\",0) as \"CountExecuting\" from \"Users\" a " +
                     "inner join \"Employees\" b on b.\"EmployeeId\" = a.\"UserName\" " +
                     "left join (select \"UserId\", count(\"Id\") as \"CountExecuting\" from \"TaskExecutes\" " +
                     "where  \"Progress\" < 100 or \"StatusId\" = @StatusId " +
                     "group by \"UserId\") c on  c.\"UserId\" = a.\"UserId\" " +
                     "where a.\"UserId\" = @UserId order by a.\"UserId\" LIMIT 1", new { UserId = userId, StatusId = Config.Processing });
            }
        }

        public IEnumerable<UserResponse> FindExecutingList(int departmentId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<UserResponse>("select a.\"UserId\" , a.\"DepartmentId\", " +
                    "COALESCE (a2.\"CountExecuting\",0) as \"CountExecuting\" " +
                    "from \"Users\" a " +
                    "left join (select b.\"UserId\", count(b.\"Id\") as \"CountExecuting\" from \"TaskExecutes\" b " +
                    "inner join \"Tasks\" b1 on b.\"TaskId\" = b1.\"Id\" " +
                    "where b.\"Progress\" < " + Config.CompleteProgress +
                    " and b.\"StatusId\" in (" + Config.Processing + "," + Config.Waiting + ") and" +
                    "b1.\"StatusId\" not in (" + Config.Cancel + "," + Config.Complete + ") " +
                    "group by b.\"UserId\") a2 on  a2.\"UserId\" = a.\"UserId\" " +
                    "where a.\"DepartmentId\" = @DepartmentId  order by a.\"UserId\"", new { DepartmentId = departmentId });
            }
        }

        public IEnumerable<UserResponse> FindAllUserByTaskId(int taskId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<UserResponse>("select a.\"UserId\",b.\"UserName\",b.\"FullName\" " +
                    "from \"TaskExecutes\" a inner join \"Users\" b on a.\"UserId\" = b.\"UserId\" " +
                    "where  b.\"Active\" = 'true' and a.\"StatusId\" != 3 and  a.\"TaskId\" = @TaskId", new { TaskId = taskId });
            }
        }

        public IEnumerable<UserResponse> FindUserByTaskId(int taskId,int departmentId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                    return dbConnection.Query<UserResponse>("select a.\"UserId\",b.\"UserName\",b.\"FullName\" " +
                    "from \"TaskExecutes\" a inner join \"Users\" b on a.\"UserId\" = b.\"UserId\" " +
                    "where  b.\"Active\" = 'true' and a.\"StatusId\" != 3 and  " +
                    "a.\"TaskId\" = @TaskId and b.\"DepartmentId\" = @DepartmentId", 
                    new { TaskId = taskId, DepartmentId = departmentId });
            }
        }

        public IEnumerable<UserResponse> FindUserByIssueId(int issueId, int departmentId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<UserResponse>("select a.\"UserId\",b.\"UserName\",b.\"FullName\" " +
                "from \"TaskExecutes\" a inner join \"Users\" b on a.\"UserId\" = b.\"UserId\" " +
                "inner join \"Tasks\" c on c.\"Id\" = a.\"TaskId\" " +
                "where  b.\"Active\" = 'true' and a.\"StatusId\" != 3 and " +
                "c.\"IssueId\" = @IssueId and b.\"DepartmentId\" = @DepartmentId",
                new { IssueId = issueId, DepartmentId = departmentId });
            }
        }

        public IEnumerable<UserResponse> FindById(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<UserResponse>("select a.*, b.\"RoleName\", " +
                    "c.\"DepartmentName\", d.\"DivisionName\" from \"Users\" a " +
                    "inner join \"Roles\" b on a.\"RoleId\" = b.\"RoleId\" " +
                    "inner join \"DepartmentViews\" c on a.\"DepartmentId\" = c.\"DepartmentId\" " +
                    "inner join \"DivisionViews\" d on c.\"DivisionId\" = d.\"DivisionId\" WHERE a.\"UserId\" = @Id", new { Id = id });
            }
        }

        public bool Remove(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                try
                {
                    dbConnection.ExecuteAsync($"DELETE FROM \"Users\" WHERE \"UserId\" = @Id", new { Id = id });
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        public bool DepartmentView(string userId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<bool>("select case when exists (SELECT 1 FROM \"Users\" " +
                    "WHERE \"UserName\" = @UserId and  (\"RoleId\" = -1 or \"PositionId\" in (2,5))) " +
                    "then CAST('1' as bool) else CAST('0' as bool) end", new { UserId = userId });
            }
        }

        public IEnumerable<UserResponse> FindByEmployeeExecuting(int departmentId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@DepartmentId", departmentId);

                return dbConnection.Query<UserResponse>("select  a.\"Id\",a.\"EmployeeId\",a.\"EmployeeName\" , " +
                    "COALESCE (c.\"NumExecute\",0) as \"NumExecute\"  " +
                    "from \"Employees\" a inner join \"Users\" b on a.\"EmployeeId\" = b.\"UserName\" " +
                    "left join (select \"UserId\", count(\"Id\") as \"NumExecute\" from \"TaskExecutes\" " +
                    "where  \"Progress\" < 100 or \"StatusId\" = 2 " +
                    "group by \"UserId\") c on  c.\"UserId\" = b.\"UserId\" where (a.\"DepartmentId\" = @DepartmentId and @DepartmentId != - 1) or @DepartmentId != -1", parameters);
            }
        }

        public bool ExistUser(string userId)
        {
            throw new NotImplementedException();
        }
    }
}