using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.Position;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public PositionRepository()
        {
        }

        public IEnumerable<PositionResponse> FindAll()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<PositionResponse>("SELECT * FROM \"Positions\"");
            }
        }
    }
}