using Dapper;
using Dapper.Transaction;
using Npgsql;
using ServiceDesk.Data.Features.Menu;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        //public MenuRepository(IConfiguration configuration)
        //{
        //    Config.DbInfo = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public MenuRepository()
        {
        }

        public bool Add(MenuCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    var parameters = new DynamicParameters();
                    // insert table Menus
                    const string masterQuery = "INSERT INTO \"Menus\" (\"MenuIconId\", \"Url\", " +
                                    "\"ParentId\", \"Sort\", \"Active\", \"CreateUser\", \"CreateDate\") " +
                                    "Values (@MenuIconId, @Url, @ParentId, @Sort, @Active, @CreateUser, @CreateDate) RETURNING \"MenuId\"";
                    parameters.Add("@MenuIconId", model.MenuIconId);
                    parameters.Add("@Url", model.Url);
                    parameters.Add("@ParentId", model.ParentId);
                    parameters.Add("@Sort", model.Sort);
                    parameters.Add("@Active", model.Active);
                    parameters.Add("@CreateUser", Claim.Session[Config.UserId]);
                    parameters.Add("@CreateDate", DateTime.Now);
                    try
                    {
                        var menuId = dbConnection.ExecuteScalar<int>(masterQuery, parameters, transaction: transaction);

                        // Remove MenuTransactions
                        transaction.Execute("delete from \"MenuTranslations\" where \"MenuId\" = @MenuId", new { MenuId = menuId });

                        transaction.Execute("insert into \"MenuTranslations\"(\"MenuId\", \"MenuName\", \"LanguageId\") values(@MenuId, @MenuName, 'vi-VN') ", new { MenuId = menuId, MenuName = model.MenuName });

                        transaction.Execute("insert into \"MenuTranslations\"(\"MenuId\", \"MenuName\", \"LanguageId\") values(@MenuId, @MenuName, 'ru-RU') ", new { MenuId = menuId, MenuName = model.RussianMenuName });

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

        public bool Update(MenuCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    const string sqlQuery = "UPDATE \"Menus\" SET \"MenuIconId\"  = @MenuIconId, " +
                                            "\"Url\" = @Url, \"ParentId\" = @ParentId, \"Sort\" = @Sort " +
                                            "\"Active\" = @Active, \"UpdateDate\" = @UpdateDate,\"UpdateUser\" = @UpdateUser WHERE \"MenuId\" = @MenuId";
                    var parameters = new DynamicParameters();
                    parameters.Add("@MenuIconId", model.MenuIconId);
                    parameters.Add("@Url", model.Url);
                    parameters.Add("@ParentId", model.ParentId);
                    parameters.Add("@Sort", model.Sort);
                    parameters.Add("@Active", model.Active);
                    parameters.Add("@UpdateDate", Claim.Session[Config.UserId]);
                    parameters.Add("@UpdateUser", DateTime.Now);
                    parameters.Add("@MenuId", model.MenuId);
                    try
                    {
                        dbConnection.Query(sqlQuery, parameters, transaction: transaction);

                        // Update to MenuTransactions
                        transaction.Execute("Update \"MenuTranslations\" set \"MenuName\" = @MenuName where \"LanguageId\" = 'vi-VN' and \"MenuId\" = @MenuId ", new { MenuId = model.MenuId, MenuName = model.MenuName });

                        transaction.Execute("Update \"MenuTranslations\" set \"MenuName\" = @MenuName where \"LanguageId\" = 'ru-RU' and \"MenuId\" = @MenuId ", new { MenuId = model.MenuId, MenuName = model.RussianMenuName });

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

        public IEnumerable<MenuResponse> FindAll()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<MenuResponse>("SELECT a.\"MenuId\", a.\"Url\", a.\"FriendlyUrl\", a.\"ParentId\", " +
                 "a.\"Sort\", a.\"MenuIconId\", a.\"Active\", b.\"MenuName\" FROM \"Menus\" a " +
                 "inner join \"MenuTranslations\" b on a.\"MenuId\" = b.\"MenuId\" " +
                 "where b.\"LanguageId\" = 'vi-VN'");
            }
        }

        public IEnumerable<MenuResponse> FindAllByFunction()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<MenuResponse> FindById(int menuId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<MenuResponse>($"SELECT * FROM \"Menus\" WHERE \"MenuId\" = @MenuId", new { MenuId = menuId });
            }
        }

        public IEnumerable<MenuResponse> FindByName(string menuName)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "select a.\"MenuId\", a.\"Url\", a.\"FriendlyUrl\", " +
                "a.\"Sort\", a.\"ParentId\", a.\"MenuIconId\", a.\"Active\", b.\"IconName\", " +
                "c.\"MenuName\", d.\"MenuName\" as \"RussianMenuName\" from \"Menus\" a " +
                "left join \"MenuIcons\" b on  a.\"MenuIconId\" = b.\"Id\" " +
                "inner join (select * from \"MenuTranslations\" where \"LanguageId\" ='vi-VN') c " +
                "on a.\"MenuId\" = c.\"MenuId\" " +
                "inner join (select * from \"MenuTranslations\" where \"LanguageId\" ='ru-RU') d " +
                "on a.\"MenuId\" = d.\"MenuId\" " +
                "where a.\"Active\" = '1' and  (c.\"MenuName\" ilike '%' || @MenuName || '%' or " +
                "d.\"MenuName\" ilike '%' || @MenuName || '%') " +
                "order by a.\"Sort\", a.\"MenuId\"";
                var parameters = new DynamicParameters();
                parameters.Add("@MenuName", menuName);
                return dbConnection.Query<MenuResponse>(sqlQuery, parameters);
            }
        }

        public bool Remove(int menuId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                try
                {
                    dbConnection.Execute($"DELETE FROM \"Menus\" WHERE \"MenuId\" = @MenuId", new { MenuId = menuId });
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }
        }

        public IEnumerable<SidebarResponse> SidebarList(string languageId, string userId, int roleId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "select * from public.\"SidebarList\"(@LanguageId, @UserId, @RoleId)";

                var parameters = new DynamicParameters();
                parameters.Add("@LanguageId", languageId);
                parameters.Add("@UserId", userId);
                parameters.Add("@RoleId", roleId);
                return dbConnection.Query<SidebarResponse>(sqlQuery, parameters);
            }
        }
    }
}