using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.Employee;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public EmployeeRepository()
        {
        }

        public IEnumerable<EmployeeResponse> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<EmployeeResponse> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public int FindByEmployeeId(string employeeId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<int>("SELECT \"DepartmentId\" FROM \"Employees\" WHERE \"EmployeeId\" = @EmployeeId  and \"Quit\" = 'f' LIMIT 1", new { EmployeeId = employeeId });
            }
        }

        public IEnumerable<EmployeeResponse> FindInformation(string employeeId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<EmployeeResponse>("SELECT \"DepartmentId\",\"Email\",\"Phone\",\"Mobile\" FROM \"Employees\" " +
                  "WHERE \"EmployeeId\" = @EmployeeId and \"Quit\" = 'f' LIMIT 1", new { EmployeeId = employeeId });
            }
        }

        public int ExistEmployeeId(string employeeId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<int>("SELECT 1 FROM \"Employees\" WHERE \"EmployeeId\" = @EmployeeId and \"Quit\" = 'f'", new { EmployeeId = employeeId });
            }
        }

        public IEnumerable<EmployeeResponse> FindByEmployeeExecuting(int departmentId, int roleId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@DepartmentId", departmentId);
                parameters.Add("@RoleId", roleId);
                var showAll = roleId == -1 || roleId == 16 || roleId == 32;

                var query = showAll
                    ? "select  a.\"Id\", a.\"EmployeeId\", a1.\"UserId\", a.\"DepartmentId\", a2.\"DepartmentName\", a.\"EmployeeName\", " +
                      "COALESCE (a3.\"NumExecute\",0) as \"NumExecute\" from \"Employees\" a " +
                      "inner join \"Users\" a1 on a.\"EmployeeId\" = a1.\"UserName\" " +
                      "inner join \"DepartmentViews\" a2 on a.\"DepartmentId\" = a2.\"DepartmentId\" " +
                      "left join (select b.\"UserId\", count(b.\"Id\") as \"NumExecute\" from \"TaskExecutes\" b " +
                      "inner join \"Tasks\" b1 on b.\"TaskId\" = b1.\"Id\" " +
                      "where b.\"Progress\" < " + Config.CompleteProgress + 
                      " and b.\"StatusId\" in (" + Config.Processing + "," + Config.Waiting + ") and" +
                      "b1.\"StatusId\" not in (" + Config.Cancel + "," + Config.Complete + ") " +
                      "group by \"UserId\") a3 on  a3.\"UserId\" = a1.\"UserId\" " +
                      "where (a.\"DepartmentId\" = @DepartmentId or @DepartmentId = - 1) and a.\"Quit\" = 'f'"

                    : "select  a.\"Id\", a.\"EmployeeId\", a1.\"UserId\", a.\"DepartmentId\", a2.\"DepartmentName\", a.\"EmployeeName\", " +
                      "COALESCE (a3.\"NumExecute\",0) as \"NumExecute\" from \"Employees\" a " +
                      "inner join \"Users\" a1 on a.\"EmployeeId\" = a1.\"UserName\" " +
                      "inner join \"DepartmentViews\" a2 on a.\"DepartmentId\" = a2.\"DepartmentId\" " +
                      "left join (select b.\"UserId\", count(b.\"Id\") as \"NumExecute\" from \"TaskExecutes\" b " +
                      "inner join \"Tasks\" b1 on b.\"TaskId\" = b.\"Id\" " +
                      "where b.\"Progress\" < " + Config.CompleteProgress +
                      " and b.\"StatusId\" in (" + Config.Processing + "," + Config.Waiting + ") and" +
                      "b1.\"StatusId\" not in (" + Config.Cancel + "," + Config.Complete + ") " +
                      "group by \"UserId\") a3 on  a3.\"UserId\" = a1.\"UserId\" " +
                      "where a.\"DepartmentId\" = @DepartmentId and a.\"Quit\" = 'f'";

                return dbConnection.Query<EmployeeResponse>(query, parameters);
            }
        }

        public IEnumerable<ExecutingResponse> FindByDetail(int userId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                return dbConnection.Query<ExecutingResponse>("select b.\"Id\", b.\"TaskId\", b.\"UserId\", " +
                     "b3.\"EmployeeName\" as \"UserName\", b1.\"StartDate\", " +
                     "b1.\"EndDate\", b.\"Progress\", b.\"Description\" from \"TaskExecutes\" b " +
                     "inner join \"Tasks\" b1 on b.\"TaskId\" = b1.\"Id\" " +
                     "inner join \"Users\" b2 on b2.\"UserId\" = b.\"UserId\" " +
                     "inner join \"Employees\" b3 on b2.\"UserName\" = b3.\"EmployeeId\" " +
                     "where  b.\"Progress\" < 100 and b.\"StatusId\" = 6 and b.\"UserId\" = @UserId ", parameters);
            }
        }

        public int FindByLeaderDepartment(string employeeId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.QueryFirstOrDefault<int>("SELECT \"DepartmentId\" FROM \"Employees\" " +
                       "WHERE \"EmployeeId\" = @EmployeeId  and \"Quit\" = 'f' LIMIT 1", 
                    new { EmployeeId = employeeId });
            }
        }
    }
}