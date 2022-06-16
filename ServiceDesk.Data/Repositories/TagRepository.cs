using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.Tag;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class TagRepository : ITagRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public TagRepository()
        {
        }

        //public IEnumerable<TagResponse> FindAll()
        //{
        //    using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
        //    {
        //        if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
        //        return dbConnection.Query<TagResponse>("SELECT * FROM \"Tags\" order by \"Id\"");
        //    }
        //}

        public IEnumerable<TagResponse> FindAll(string languageId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "SELECT a.\"Id\" , b.\"TagName\" from \"Tags\" a " +
                        "inner join \"TagTranslations\" b on a.\"Id\" = b.\"TagId\" " +
                        "inner join \"Languages\" c on b.\"LanguageId\" = c.\"Id\" " +
                        "where b.\"LanguageId\" = @LanguageId order by a.\"Id\"";
                var parameters = new DynamicParameters();
                parameters.Add("@LanguageId", languageId);
                return dbConnection.Query<TagResponse>(sqlQuery, parameters);
            }
        }
    }
}