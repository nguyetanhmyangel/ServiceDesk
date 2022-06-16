using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.Role;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class RoleRepository : IRoleRepository//<Roles>
    {
        public RoleRepository()
        {
            //Config.DbInfo = configuration.GetValue<string>("DbInfo:ConnectionString");
        }

        public bool Add(RoleCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "INSERT INTO \"Roles\" (\"RoleId\",\"RoleName\")" +
                                        "Values (@RoleId, @RoleName)";
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", model.RoleId);
                parameters.Add("@RoleName", model.RoleName);
                try
                {
                    dbConnection.Execute(sqlQuery, parameters);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        public bool Update(RoleCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "UPDATE \"Roles\" SET \"RoleName\"  = @RoleName WHERE \"RoleId\" = @RoleId";
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", model.RoleId);
                parameters.Add("@RoleName", model.RoleName);
                try
                {
                    dbConnection.Query(sqlQuery, parameters);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        // SELECT \"Id\", \"Email\", \"EmailConfirmed\" FROM \"AspNetUsers\
        public IEnumerable<RoleResponse> FindAll()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<RoleResponse>("SELECT * FROM \"Roles\"");
            }
        }

        public IEnumerable<RoleResponse> FindByFunction()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<RoleResponse> FindById(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<RoleResponse>("SELECT * FROM \"Roles\" WHERE \"RoleId\" = @Id", new { Id = id });
            }
        }

        public IEnumerable<RoleResponse> FindByName(string roleName)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "select * from \"Roles\" where \"RoleName\" ilike '%' || @RoleName || '%' order by \"RoleId\"";
                var parameters = new DynamicParameters();
                parameters.Add("@RoleName", roleName);
                return dbConnection.Query<RoleResponse>(sqlQuery, parameters);
            }
        }

        public bool Remove(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                try
                {
                    dbConnection.Execute($"DELETE FROM \"Roles\" WHERE \"RoleId\" = @Id", new { Id = id });
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }
    }
}