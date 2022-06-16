using Dapper;
using Dapper.Transaction;
using Npgsql;
using ServiceDesk.Data.Features.UserPermission;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class UserPermissionRepository : IUserPermissionRepository//<RolePermissions>
    {
        public UserPermissionRepository()
        {
            //Config.DbInfo = configuration.GetValue<string>("DbInfo:ConnectionString");
        }

        public bool Add(UserPermissionCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "INSERT INTO \"UserPermissions\" (\"UserId\", \"MenuId\", \"UserPermission\")" +
                                        "Values (@UserId, @MenuId, @UserPermission)";
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", model.UserId);
                parameters.Add("@MenuId", model.MenuId);
                parameters.Add("@UserPermission", model.UserPermission);
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

        public bool Update(UserPermissionCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "UPDATE \"UserPermissions\"  SET \"UserId\"  = @UserId, " +
                "\"MenuId\"  = @MenuId, \"UserPermission\"  = @UserPermission " +
                "WHERE \"Id\" = @Id";
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", model.UserId);
                parameters.Add("@MenuId", model.MenuId);
                parameters.Add("@UserPermission", model.UserPermission);
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

        public IEnumerable<UserPermissionResponse> FindAll()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<UserPermissionResponse>("SELECT * FROM \"UserPermissions\"");
            }
        }

        public IEnumerable<UserPermissionResponse> FindByFunction()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<UserPermissionResponse> FindById(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<UserPermissionResponse>($"SELECT * FROM \"UserPermissions\" WHERE \"Id\" = @Id", new { Id = id });
            }
        }

        public IEnumerable<UserPermissionResponse> FindByRoleId(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<UserPermissionResponse>("select a.\"MenuId\", b.\"MenuName\", a.\"ParentId\",  " + 
                  "COALESCE((select e.\"UserPermission\" from \"UserPermissions\" e " +
                  "where e.\"UserId\" = @UserId and e.\"MenuId\" = a.\"MenuId\"),0)  as \"UserPermission\" from \"Menus\" a " +
                  "inner join \"MenuTranslations\" b on a.\"MenuId\" = b.\"MenuId\"  " +
                  "and b.\"LanguageId\" = 'vi-VN' order by a.\"MenuId\" ", new { UserId = id });
            }
        }

        public bool Remove(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                try
                {
                    dbConnection.Execute($"DELETE FROM \"UserPermissions\" WHERE \"Id\" = @Id", new { Id = id });
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        public bool ChangePermission(UserPermissionCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("@UserId", model.UserId);
                        parameters.Add("@MenuId", model.MenuId);
                        parameters.Add("@UserPermission", model.UserPermission);

                        //dbConnection.Execute("delete from \"UserPermissions\" where EXISTS(select 1 from " +
                        //    "\"UserPermissions\" where \"MenuId\" = @MenuId and \"UserId\" = @UserId)",
                        //    parameters, transaction: transaction);

                        //if (model.UserPermission > 0)
                        //{
                        //    transaction.Execute("INSERT INTO \"UserPermissions\" (\"UserId\", \"MenuId\", \"UserPermission\") " +
                        //                        "Values (@UserId, @MenuId, @UserPermission)", parameters);
                        //}


                        transaction.Execute("INSERT INTO \"UserPermissions\" (\"UserId\", \"MenuId\", \"RolePermission\") " +
                                            "Values (@UserId, @MenuId, @RolePermission) ON CONFLICT (\"UserId\", \"MenuId\") DO NOTHING", parameters);

                        transaction.Execute("Update \"UserPermissions\" set \"RolePermission\" = @RolePermission " +
                                            "where \"UserId\" = @UserId and \"MenuId\" = @MenuId", parameters);

                        transaction.Commit();
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
}