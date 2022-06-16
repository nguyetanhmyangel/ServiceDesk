using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.Priority;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class PriorityRepository : IPriorityRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public PriorityRepository()
        {
        }

        public IEnumerable<PriorityResponse> FindAll()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<PriorityResponse>("SELECT * FROM \"Priorities\"");
            }
        }
    }
}