using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.Department;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public DepartmentRepository()
        {
        }

        public IEnumerable<DepartmentResponse> FindAll()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<DepartmentResponse>("SELECT * FROM \"DepartmentViews\"");
            }
        }

        public IEnumerable<DepartmentResponse> FindByUser(int roleId, int departmentId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                string query;
                switch (roleId)
                {
                    case 1:
                    case 2:
                    case 4:
                    case 8:
                        query = "SELECT \"DepartmentId\", \"DepartmentName\" FROM \"DepartmentViews\" where \"DivisionId\" = 10 and \"DepartmentId\" = @DepartmentId";
                        break;

                    case -1:
                    case 32:
                    case 16:
                        query = "SELECT \"DepartmentId\", \"DepartmentName\" FROM \"DepartmentViews\" where \"DivisionId\" = 10";
                        break;

                    default:
                        query = string.Empty;
                        break;
                }
                return dbConnection.Query<DepartmentResponse>(query, new { RoleId = roleId, DepartmentId = departmentId });
            }
        }

        public IEnumerable<DepartmentResponse> FindAllByFunction()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<DepartmentResponse> FindById(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<DepartmentResponse>("SELECT * FROM \"DepartmentViews\" " +
                                                              "WHERE \"DepartmentId\" = @DepartmentId", new { DepartmentId = id });
            }
        }

        public IEnumerable<DepartmentResponse> FindByDivisionId(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<DepartmentResponse>("SELECT * FROM \"DepartmentViews\" WHERE \"DivisionId\" = @DivisionId", new { DivisionId = id });
            }
        }

        public IEnumerable<DepartmentResponse> FindByDivisionId(int divisionId, int currentDepartmentId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<DepartmentResponse>("SELECT * FROM \"DepartmentViews\" " +
                       "WHERE \"DivisionId\" = @DivisionId and \"DepartmentId\" <> @DepartmentId", 
                    new { DivisionId = divisionId, DepartmentId = currentDepartmentId });
            }
        }

        public IEnumerable<DepartmentResponse> FindByDivisionId()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<DepartmentResponse>("SELECT * FROM \"DepartmentViews\" WHERE \"DivisionId\" = @DivisionId", new { DivisionId = 10 });
            }
        }

        public IEnumerable<DepartmentResponse> FindByTaskId(int taskId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@TaskId", taskId);
                return dbConnection.Query<DepartmentResponse>("select a.\"DepartmentId\", a.\"DepartmentName\" from \"DepartmentViews\" a " +
                                                              "inner join \"TransferTasks\" b on a.\"DepartmentId\" = b.\"DepartmentId\" " +
                                                              "where b.\"TaskId\" = @TaskId", parameters);
            }
        }

        public IEnumerable<DepartmentResponse> FindByIssueId(int issueId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "select a.\"DepartmentId\",b.\"DepartmentName\" from \"Tasks\" a " +
                    "inner join \"DepartmentViews\" b on b.\"DepartmentId\" = b.\"DepartmentId\" " +
                    "where a.\"IssueId\" = @IssueId";
                var parameters = new DynamicParameters();
                parameters.Add("@IssueId", issueId);
                return dbConnection.Query<DepartmentResponse>(sqlQuery, parameters);
            }
        }
    }
}