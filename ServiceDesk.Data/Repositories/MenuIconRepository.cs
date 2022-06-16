using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.MenuIcon;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class MenuIconRepository : IMenuIconRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public MenuIconRepository()
        {
        }

        public bool Add(MenuIconCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "INSERT INTO \"MenuIcons\" (\"IconName\") VALUES (@IconName)";
                var parameters = new DynamicParameters();
                parameters.Add("@IconName", model.IconName);
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

        public bool Update(MenuIconCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "UPDATE \"MenuIcons\"  SET \"IconName\"  = @Name   WHERE \"Id\" = @Id";
                var parameters = new DynamicParameters();
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

        // SELECT \"Id\", \"Email\", \"EmailConfirmed\" FROM \"AspNetUsers\

        public IEnumerable<MenuIconResponse> FindAll()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<MenuIconResponse>("SELECT \"Id\", \"IconName\" FROM \"MenuIcons\" ");
            }
        }

        public IEnumerable<MenuIconResponse> FindAllByFunction()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<MenuIconResponse> FindById(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                //return await dbConnection.QueryFirstOrDefaultAsync<MenuIcons>($"SELECT * FROM \"MenuIcons\" WHERE \"Id\" = {id}");
                return dbConnection.Query<MenuIconResponse>("SELECT * FROM \"MenuIcons\" WHERE \"Id\" = @Id", new { Id = id });
            }
        }

        public bool Remove(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                try
                {
                    dbConnection.ExecuteAsync($"DELETE FROM \"MenuIcons\" WHERE \"Id\" = @Id", new { Id = id });
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