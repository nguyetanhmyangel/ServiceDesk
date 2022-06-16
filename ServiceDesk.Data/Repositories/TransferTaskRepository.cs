using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.TransferTasks;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class TransferTaskRepository : ITransferTaskRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public TransferTaskRepository()
        {
        }

        //public IEnumerable<StatusResponse> FindAll()
        //{
        //    using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
        //    {
        //        if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
        //        var parameters = new DynamicParameters();

        //        return dbConnection.Query<StatusResponse>("select * from \"Status\" where \"Id\" <> 4 order by \"Id\"", parameters);
        //    }
        //}

        public IEnumerable<TransferTaskResponse> FindById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TransferTaskResponse> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TransferTaskResponse> FindByTaskId(int taskId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@TaskId", taskId);
                return dbConnection.Query<TransferTaskResponse>("select a.\"DepartmentId\", a.\"DepartmentName\", c.\"Description\" from \"DepartmentViews\" a " +
                        "inner join \"TransferTasks\" b on a.\"DepartmentId\" = b.\"DepartmentId\" " +
                        "inner join \"Tasks\" c on c.\"Id\" = b.\"TaskId\" " +
                        "where b.\"TaskId\" = @TaskId and b.\"Active\" = 'true'", parameters);
            }
        }
    }
}