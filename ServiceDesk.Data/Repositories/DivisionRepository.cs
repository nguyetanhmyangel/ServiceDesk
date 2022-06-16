using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.Division;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class DivisionRepository : IDivisionRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public DivisionRepository()
        {
        }

        public IEnumerable<DivisionResponse> FindAll()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<DivisionResponse>("SELECT * FROM \"DivisionViews\" ");
            }
        }

        public IEnumerable<DivisionResponse> FindAllByFunction()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<DivisionResponse> FindById(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<DivisionResponse>("SELECT * FROM \"DivisionViews\" WHERE \"DivisionId\" = @DivisionId", new { DivisionId = id });
            }
        }
    }
}