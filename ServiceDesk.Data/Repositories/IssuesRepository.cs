using Dapper;
using Dapper.Transaction;
using Npgsql;
using ServiceDesk.Data.Features.Issue;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;

namespace ServiceDesk.Data.Repositories
{
    public class IssuesRepository : IIssuesRepository
    {
        //private readonly string _connectionString;

        //public MenuIconRepository(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        public IssuesRepository()
        {
        }

        public bool Add(IssuesCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                try
                {
                    dbConnection.QueryFirstOrDefault<int>(
                            "INSERT INTO \"Issues\" (\"Title\",\"Phone\"," +
                            "\"Mobile\",\"Email\",\"CustomerDescription\",\"EmployeeId\",\"DepartmentId\"," +
                            "\"StatusId\",\"DispatchPath\",\"Review\",\"CreateUser\") Values " +
                            "(@Title, @Phone, @Mobile, @Email, @CustomerDescription, @EmployeeId, " +
                            "@DepartmentId,@StatusId,@DispatchPath,@Review,@CreateUser) " +
                            "returning \"Id\"; ",
                            new
                            {
                                Title = model.Title,
                                Phone = model.Phone,
                                Mobile = model.Mobile,
                                Email = model.Email,
                                CustomerDescription = model.CustomerDescription,
                                EmployeeId = model.EmployeeId,
                                DepartmentId = model.DepartmentId,
                                StatusId = model.StatusId,
                                //TagId = model.TagId,
                                CreateUser = model.CreateUser,
                                DispatchPath = model.DispatchPath,
                                Review = model.Review
                            });
                }
                catch (Exception ex)
                {
                    //ex.Message.ToString();
                    return false;
                }
                return true;
                
            }
        }

        public bool Remove(int id)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        dbConnection.Execute("DELETE FROM \"Tasks\" where \"IssueId\" = @IssueId and \"StatusId\" = 6", new { IssueId = id }, transaction: transaction);
                        transaction.Execute($"DELETE FROM \"Issues\" WHERE \"Id\" = @Id and \"StatusId\" = 6", new { Id = id });
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        //ex.Message.ToString();
                        transaction.Rollback();
                        return false;
                    }
                }
                return true;
            }
        }

        public bool CancelIssue(int issueId, string reason)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        dbConnection.Execute("Update \"TaskExecutes\" set \"StatusId\" = @StatusId from \"TaskExecutes\" a " +
                                 "inner join \"Tasks\" b on a.\"TaskId\" = b.\"Id\" where b.\"IssueId\" = @IssueId ",
                            new { IssueId = issueId, StatusId = Config.Cancel }, transaction: transaction);

                        transaction.Execute("Update \"Tasks\" set \"StatusId\" = @StatusId where \"IssueId\" = @IssueId " +
                                            "and EXISTS(select 1 from \"Tasks\" where \"IssueId\" = @IssueId) ",
                            new { IssueId = issueId, StatusId = Config.Cancel });

                        transaction.Execute("Update \"Issues\" set \"StatusId\" = @StatusId, \"Reason\" = @Reason, " +
                                            "\"OwnerCancel\" = 'true' WHERE \"Id\" = @IssueId",
                            new { IssueId = issueId, Reason = reason, StatusId = Config.Cancel });

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        //ex.Message.ToString();
                        transaction.Rollback();
                        return false;
                    }
                }
                return true;
            }
        }

        public bool Update(IssuesCommand model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

                try
                {
                    dbConnection.Execute("Update \"Issues\" set \"Title\" = @Title," +
                                "\"Phone\" = @Phone, \"Mobile\" = @Mobile, \"Email\" = @Email, " +
                                "\"CustomerDescription\" = @CustomerDescription, \"EmployeeId\" = @EmployeeId, " +
                                "\"DepartmentId\" = @DepartmentId, " +
                                "\"DispatchPath\" = @DispatchPath, \"Review\" = @Review, \"UpdateUser\" = @UpdateUser " +
                                "where \"Id\" = @Id and (\"CreateUser\" = @UpdateUser or \"EmployeeId\" = @UpdateUser) and " +
                                "\"StatusId\" = " + Config.Waiting,
                        new
                        {
                            Title = model.Title,
                            Phone = model.Phone,
                            Mobile = model.Mobile,
                            Email = model.Email,
                            CustomerDescription = model.CustomerDescription,
                            EmployeeId = model.EmployeeId,
                            DepartmentId = model.DepartmentId,
                            //TagId = model.TagId,
                            UpdateUser = model.UpdateUser,
                            DispatchPath = model.DispatchPath,
                            Review = model.Review,
                            //StatusId = model.StatusId,
                            //Reason = model.Reason,
                            //OwnerCancel = model.OwnerCancel,
                            Id = model.Id
                        });

                }
                catch (Exception ex)
                {
                    //ex.Message.ToString();
                    return false;
                }
                return true;            
            }
        }

        public bool Update(int id, int ratingValue)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "Update \"Issues\" set \"Rating\" = @Rating where \"Id\" = @Id ";
                var parameters = new DynamicParameters();
                parameters.Add("@Rating", ratingValue);
                parameters.Add("@Id", id);
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

        public bool Update(string userId, int issueId, IEnumerable<object> multiValue)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        dbConnection.Execute("Update \"Issues\" set \"StatusId\" = 6, " +
                            "\"ApproveDate\" = @ApproveDate,\"ApproveUser\" = @ApproveUser  where \"Id\" = @Id",
                            new { ApproveDate = DateTime.Now, Id = issueId, ApproveUser = userId }, transaction: transaction);

                        transaction.Execute("delete from \"Tasks\" where \"IssueId\" = @Id",
                            new { Id = issueId });

                        foreach (var value in multiValue)
                        {
                            //var departmentId = Helper.ConvertToInt(multiValue);
                            var departmentId = Helper.ConvertToInt(value);
                            transaction.Execute("insert into \"Tasks\" (\"DepartmentId\", \"IssueId\" " +
                                "\"CreateUser\", \"CreateDate\") Values (@DepartmentId,  @IssueId, @StatusId, @CreateUser, @CreateDate)",
                            new { DepartmentId = departmentId, IssueId = issueId, CreateUser = userId });
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        //ex.Message.ToString();
                        transaction.Rollback();
                        return false;
                    }
                    return true;
                }
            }
        }

        //public IEnumerable<IssuesResponse> FindByOwner(string employeeId, string languageId, DateTime fromDate, DateTime toDate)
        //{
        //    using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
        //    {
        //        if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
        //        var parameters = new DynamicParameters();
        //        parameters.Add("@EmployeeId", employeeId);
        //        parameters.Add("@LanguageId", languageId);
        //        parameters.Add("@FromDate", fromDate);
        //        parameters.Add("@ToDate", toDate);
        //        return dbConnection.Query<IssuesResponse>("select * from \"IssuesList\"(@EmployeeId, @LanguageId, @FromDate, @ToDate) ", parameters);
        //    }
        //}

        public IEnumerable<IssuesResponse> FindByOwner(string employeeId, string languageId, DateTime fromDate, DateTime toDate, int tagId, int statusId, int roleId, int departmentId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeId", employeeId);
                parameters.Add("@LanguageId", languageId);
                parameters.Add("@FromDate", fromDate);
                parameters.Add("@ToDate", toDate);
                parameters.Add("@TagId", tagId);
                parameters.Add("@StatusId", statusId);
                parameters.Add("@RoleId", roleId);
                parameters.Add("@DepartmentId", departmentId);
                return dbConnection.Query<IssuesResponse>("select * from \"IssuesList\"(@EmployeeId, @LanguageId,  @FromDate, @ToDate, @TagId, @StatusId, @RoleId, @DepartmentId) ", parameters);
            }
        }

        public IEnumerable<IssuesResponse> FindByOwner(string employeeId, string languageId, DateTime fromDate, DateTime toDate)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeId", employeeId);
                parameters.Add("@LanguageId", languageId);
                parameters.Add("@FromDate", fromDate);
                parameters.Add("@ToDate", toDate);
                return dbConnection.Query<IssuesResponse>("select * from \"IssuesList\"(@EmployeeId, @LanguageId,  @FromDate, @ToDate) ", parameters);
            }
        }

        public IEnumerable<IssuesResponse> FindByEmployeeId(string employeeId, string languageId, DateTime fromDate, DateTime toDate)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@EmployeeId", employeeId);
                parameters.Add("@LanguageId", languageId);
                parameters.Add("@FromDate", fromDate);
                parameters.Add("@ToDate", toDate);
                return dbConnection.Query<IssuesResponse>("select * from \"IssuesByEmployeeId\"(@EmployeeId, @LanguageId, @FromDate, @ToDate) ", parameters);
            }
        }

        public IEnumerable<IssuesResponse> FindByDivision(int divisionId, int statusId, string languageId, DateTime fromDate, DateTime toDate)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@DivisionId", divisionId);
                parameters.Add("@StatusId", statusId);
                parameters.Add("@LanguageId", languageId);
                parameters.Add("@FromDate", fromDate);
                parameters.Add("@ToDate", toDate);
                return dbConnection.Query<IssuesResponse>("select * from \"IssuesByDivision\"(@DivisionId, @StatusId, @LanguageId, @FromDate, @ToDate) ", parameters);
            }
        }

        public IEnumerable<IssueTimeLifeResponse> FindForTimeLife(int issueId, string languageId, string userId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@IssueId", issueId);
                return dbConnection.Query<IssueTimeLifeResponse>("select a.\"Id\", a.\"EmployeeId\", a1.\"EmployeeName\", a.\"Title\", a.\"CreateUser\", " +
                    "a.\"Description\", a.\"Mobile\", a.\"Phone\", a.\"Email\"," +
                    "case when @LanguageId = 'ru-RU' then a2.\"DepartmentRussianName\" ELSE a2.\"DepartmentName\" end as \"DepartmentName\"," +
                    "case when @LanguageId = 'ru-RU' then a3.\"DivisionRussianName\" ELSE a3.\"DivisionName\" end as \"DivisionName\", " +
                    "to_char(a.\"CreateDate\", 'DD/MM/YYYY') as \"CreateDate\" " +
                    "from \"Issues\" a " +
                    "inner join \"Employees\" a1 on a1.\"EmployeeId\" = a.\"EmployeeId\" " +
                    "inner join \"DepartmentViews\" a2 on a.\"DepartmentId\" = a2.\"DepartmentId\"" +
                    "inner join \"DivisionViews\" a3 on a2.\"DivisionId\" = a3.\"DivisionId\" " +
                    "where a.\"Id\" = @Id and (a.\"CreateUser\" = @UserName or a.\"EmployeeId\" = @UserName)   ",
                new { Id = issueId, LanguageId = languageId, UserName = userId });
            }
        }

        public int CanCancelIssue(int issueId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();

                var sqlQuery = "select case When exists (SELECT 1 FROM \"Issues\" " +
                                  "where (\"StatusId\" = " + Config.Cancel + " or \"StatusId\" = " + Config.Complete + ") and " +
                                  "\"Id\" = @IssueId) then 0 " +
                                  "Else 1 end";

                var parameters = new DynamicParameters();
                parameters.Add("@IssueId", issueId);
                return dbConnection.QueryFirstOrDefault<int>(sqlQuery, parameters);
            }
        }

        public int ProcessedStatus(int issueId)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                //const string sqlQuery = "select case when not exists (SELECT 1 FROM \"Tasks\" where \"StatusId\" = 1 or " +
                //                        "\"StatusId\" = 6 and \"IssueId\" = @IssueId) then 3 " +
                //                        "when exists (SELECT 1 FROM \"Tasks\" where \"StatusId\" <> 6  and \"IssueId\" = @IssueId) then 2 " +
                //                        "else 1 end";

                const string sqlQuery =
                      //Owner cancel issue
                      "select case When exists (SELECT 1 FROM \"Issues\" " +
                      "where \"OwnerCancel\" = 'TRUE' and \"Id\" = @IssueId) then 1 " +
                      //All task of issue is waiting
                      "When not exists (SELECT 1 FROM \"Tasks\" " +
                      "where \"StatusId\" <> 6 and \"IssueId\" = @IssueId) then 2 " +
                      //All task of issue is complete
                      "When not exists (SELECT 1 FROM \"Tasks\" " +
                      "where \"StatusId\" <> 5 and \"IssueId\" = @IssueId) then 3 " +
                      //All task of issue is cancel
                      "When not exists (SELECT 1 FROM \"Tasks\" " +
                      "where \"StatusId\" <> 3 and \"IssueId\" = @IssueId) then 4 " +
                      //Exist one Task is Waiting,Pausing or Processing
                      "Else 5 end";

                //Exist one Task is Waiting,Pausing or Processing
                //"When exists (SELECT 1 FROM \"Tasks\" " +
                //"where \"StatusId\" in (1,2,6) and \"IssueId\" = @IssueId) then 5 end";

                var parameters = new DynamicParameters();
                parameters.Add("@IssueId", issueId);
                return dbConnection.QueryFirstOrDefault<int>(sqlQuery, parameters);
            }
        }

        public IEnumerable<IssuesResponse> FindAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IssuesResponse> FindAllByFunction()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IssuesResponse> FindById(int id)
        {
            throw new NotImplementedException();
        }
    }
}