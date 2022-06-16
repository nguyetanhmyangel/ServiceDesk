using Dapper;
using Dapper.Transaction;
using Npgsql;
using ServiceDesk.Data.Features.RolePermission;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class RolePermissionRepository : IRolePermissionRepository//<RolePermissions>
    {
        public RolePermissionRepository()
        {
            //Config.DbInfo = configuration.GetValue<string>("DbInfo:ConnectionString");
        }

        public bool Add(RolePermissionCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "INSERT INTO \"RolePermissions\" (\"RoleId\", \"MenuId\", \"RolePermission\")" +
                                        "Values (@RoleId, @MenuId, @RolePermission)";
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", model.RoleId);
                parameters.Add("@MenuId", model.MenuId);
                parameters.Add("@RolePermission", model.RolePermission);
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

        public bool Update(RolePermissionCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "UPDATE \"RolePermissions\"  SET \"RoleId\"  = @RoleId, " +
                "\"MenuId\"  = @MenuId, \"RolePermission\"  = @RolePermission " +
                "WHERE \"Id\" = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@RoleId", model.RoleId);
                parameters.Add("@MenuId", model.MenuId);
                parameters.Add("@RolePermission", model.RolePermission);
                parameters.Add("@Id", model.Id);
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

        public IEnumerable<RolePermissionResponse> FindAll()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<RolePermissionResponse>("SELECT * FROM \"RolePermissions\"");
            }
        }

        public IEnumerable<RolePermissionResponse> FindByFunction()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<RolePermissionResponse> FindById(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<RolePermissionResponse>($"SELECT * FROM \"RolePermissions\" WHERE \"Id\" = @Id", new { Id = id });
            }
        }

        //public IEnumerable<RolePermissionResponse> FindByRoleId(int id)
        //{
        //    using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
        //    {
        //        if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
        //        return dbConnection.Query<RolePermissionResponse>("select a.\"MenuId\", b.\"MenuName\", a.\"ParentId\", " +
        //            "case when a.\"MenuId\" in (select \"MenuId\" from \"RolePermissions\" " +
        //            "where \"RoleId\" = @RoleId) then CAST('1' as int) else  CAST('0' as int) " +
        //            "end as \"MenuActive\" from \"Menus\" a inner join \"MenuTranslations\" b on a.\"MenuId\" = b.\"MenuId\" " +
        //            "and b.\"LanguageId\" = 'vi-VN' order by a.\"MenuId\"", new { RoleId = id });
        //    }
        //}

        public IEnumerable<RolePermissionResponse> FindByRoleId(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<RolePermissionResponse>("select a.\"MenuId\", b.\"MenuName\", a.\"ParentId\", " +
                      "COALESCE((select e.\"RolePermission\" from \"RolePermissions\" e " +
                      "where e.\"RoleId\" = @RoleId and e.\"MenuId\" = a.\"MenuId\"),0)  as \"RolePermission\" from \"Menus\" a " +
                      "inner join \"MenuTranslations\" b on a.\"MenuId\" = b.\"MenuId\" " +
                      "and b.\"LanguageId\" = 'vi-VN' order by a.\"MenuId\" ", new { RoleId = id });
            }
        }

        public bool Remove(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                try
                {
                    dbConnection.Execute($"DELETE FROM \"RolePermissions\" WHERE \"Id\" = @Id", new { Id = id });
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        public bool ChangePermission(RolePermissionCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@RoleId", model.RoleId);
                        parameters.Add("@MenuId", model.MenuId);
                        parameters.Add("@RolePermission", model.RolePermission);

                        transaction.Execute("INSERT INTO \"RolePermissions\" (\"RoleId\", \"MenuId\", \"RolePermission\") " +
                                            "Values (@RoleId, @MenuId, @RolePermission) ON CONFLICT (\"RoleId\", \"MenuId\") DO NOTHING", parameters);

                        transaction.Execute("Update \"RolePermissions\" set \"RolePermission\" = @RolePermission " +
                                            "where \"RoleId\" = @RoleId and \"MenuId\" = @MenuId", parameters);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    return true;
                }
            }
        }
    }
}