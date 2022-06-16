using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.Status;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public StatusRepository()
        {
        }

        public IEnumerable<StatusResponse> FindAll()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();

                return dbConnection.Query<StatusResponse>("select * from \"Status\" where \"Id\" <> 4 order by \"Id\"", parameters);
            }
        }

        public IEnumerable<StatusResponse> FindById(int id)
        {
            throw new NotImplementedException();
        }
    }
}