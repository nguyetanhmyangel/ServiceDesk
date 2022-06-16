using Dapper;
using Npgsql;
using ServiceDesk.Data.Features.Employee;
using ServiceDesk.Data.Features.Login;
using ServiceDesk.Data.Features.User;
using ServiceDesk.Data.Interfaces;
using ServiceDesk.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper.Transaction;

namespace ServiceDesk.Data.Repositories
{
    public class AuthorityRepository : IAuthorityRepository//<UserViewModel>
    {
        //private readonly IConfiguration configuration;
        public AuthorityRepository()
        {
            //Config.DbInfo = configuration.GetValue<string>("DbInfo:ConnectionString");
        }

        //public AuthorityRepository()
        //{
        //    Config.DbInfo = configuration.GetValue<string>("DbInfo:ConnectionString");
        //}

        #region Property

        public int CurrentPrivilege
        {
            get => Claim.Session[Config.Privilege] != null ? Helper.ConvertToInt(Claim.Session[Config.Privilege]) : 0;
            set => Claim.Session[Config.Privilege] = value;
        }

        public string CurrentUserName
        {
            get => Claim.Session[Config.UserName] != null ? Claim.Session[Config.UserName].ToString() : string.Empty;
            set => Claim.Session[Config.UserName] = value;
        }

        public string CurrentUserId
        {
            get => Claim.Session[Config.UserId] != null ? Claim.Session[Config.UserId].ToString() : string.Empty;
            set => Claim.Session[Config.UserId] = value;
        }

        //public string CurrentLanguageId
        //{
        //    get => Claim.Session[Config.LanguageId] != null ? Claim.Session[Config.LanguageId].ToString() : "vi-VN";
        //    set => Claim.Session[Config.LanguageId] = value;
        //}

        public int CurrentDepartmentId
        {
            get => Claim.Session[Config.DepartmentId] != null ? Helper.ConvertToInt(Claim.Session[Config.DepartmentId]) : 0;
            set => Claim.Session[Config.DepartmentId] = value;
        }

        public int CurrentDivisionId
        {
            get => Claim.Session[Config.DivisionId] != null ? Helper.ConvertToInt(Claim.Session[Config.DivisionId]) : 0;
            set => Claim.Session[Config.DivisionId] = value;
        }

        public int CurrentRoleId
        {
            get => Claim.Session[Config.RoleId] != null ? Helper.ConvertToInt(Claim.Session[Config.RoleId]) : 0;
            set => Claim.Session[Config.RoleId] = value;
        }

        public bool CanAdd
        {
            get => (CurrentPrivilege & Config.AllowAdd) == Config.AllowAdd;
            set
            {
                if (value)
                    CurrentPrivilege |= Config.AllowAdd;
                else
                    CurrentPrivilege &= ~Config.AllowAdd;
            }
        }

        public bool CanRemove
        {
            get => (CurrentPrivilege & Config.AllowDelete) == Config.AllowDelete;
            set
            {
                if (value)
                    CurrentPrivilege |= Config.AllowDelete;
                else
                    CurrentPrivilege &= ~Config.AllowDelete;
            }
        }

        public bool CanUpdate
        {
            get => (CurrentPrivilege & Config.AllowEdit) == Config.AllowEdit;
            set
            {
                if (value)
                    CurrentPrivilege |= Config.AllowEdit;
                else
                    CurrentPrivilege &= ~Config.AllowEdit;
            }
        }

        public bool CanView
        {
            get => (CurrentPrivilege & Config.AllowView) == Config.AllowView;
            set
            {
                if (value)
                    CurrentPrivilege |= Config.AllowView;
                else
                    CurrentPrivilege &= ~Config.AllowView;
            }
        }

        public bool CanSaveChange => CanUpdate || CanAdd;

        #endregion Property

        public bool AutoLogin()
        {
            var valid = false;
            if (!Helper.SameString(Claim.Cookie[Config.AutoLogin], Config.AutoLogin) || string.IsNullOrEmpty(Claim.Cookie[Config.UserId]) || string.IsNullOrEmpty(Claim.Cookie[Config.Password]))
            {
                return false;
            }

            var userId = Claim.Cookie["UserId"];
            var passWord = Claim.Cookie["password"];
            var loginResponse = new LoginResponse
            {
                UserName = Helper.TeaDecrypt(userId, Config.RandomKey),
                Password = Helper.TeaDecrypt(passWord, Config.RandomKey)
            };

            var result = UserAuthority(loginResponse);
            if (result) valid = true;
            return valid;
        }

        public bool LoggedIn()
        {
            return !string.IsNullOrEmpty(CurrentUserName);
        }

        public bool IsAdmin()
        {
            return CurrentRoleId == -1;
        }

        public bool UserAuthority(LoginResponse model)
        {
            var result = false;
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                var loginParameters = new DynamicParameters();
                loginParameters.Add("@UserName", model.UserName);
                var user = HrFindByUserName(loginParameters);
                if (user != null)
                {
                    // bắt buộc phải dùng user,pass của HR,ko mã hóa. Chuối vãi! :)
                    var existUser = HrPasswordSignIn(model);
                    if (existUser)
                    {
                        result = true;
                        foreach (var u in user)
                        {
                            CurrentUserName = u.EmployeeName;
                            CurrentUserId = u.EmployeeId;
                            CurrentDepartmentId = u.DepartmentId;
                            CurrentDivisionId = u.DivisionId;
                            //CurrentLanguageId = "vi-VN";
                            CurrentRoleId = GetRole(model.UserName);
                        }
                        if (model.Remember)
                        {
                            Claim.Cookie[Config.AutoLogin] = Config.AutoLogin;
                            Claim.Cookie[Config.UserName] = Helper.TeaEncrypt(model.UserName, Config.RandomKey);
                            Claim.Cookie[Config.Password] = Helper.TeaEncrypt(model.Password, Config.RandomKey);
                        }
                        else
                            Claim.Cookie[Config.AutoLogin] = string.Empty;
                    }
                }
            }

            return result;
        }

        public IEnumerable<EmployeeResponse> EmployeList(string userName)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<EmployeeResponse>("select * from \"Employees\" where \"EmployeeId\" = @EmployeeId and \"Active\" = '1'", new { EmployeeId = userName });
            }
        }

        public IEnumerable<UserResponse> FindByUserName(DynamicParameters parameters)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<UserResponse>("select * from \"FindByUserName\"(@UserName)", parameters);
            }
        }

        public IEnumerable<EmployeeResponse> HrFindByUserName(DynamicParameters parameters)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                return dbConnection.Query<EmployeeResponse>("select * from \"HrFindByUserName\"(@UserName)", parameters);
            }
        }

        public bool PasswordSignIn(LoginResponse model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                //return await dbConnection.QueryFirstOrDefaultAsync<bool>("select * from \"PasswordSign\"(@UserName, @Password)", parameters);

                const string sqlQuery = "select case when exists (SELECT 1 FROM \"Users\" " +
                                        "WHERE \"UserName\" = @UserName and \"Password\" = @Password and \"Active\" = '1') then CAST('1' as bool) else CAST('0' as bool) end";
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", model.UserName);
                parameters.Add("@Password", Helper.Encrypt(model.Password));
                return dbConnection.Query(sqlQuery, parameters).FirstOrDefault();
            }
        }

        public bool HrPasswordSignIn(LoginResponse model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                //return await dbConnection.QueryFirstOrDefaultAsync<bool>("select * from \"PasswordSign\"(@UserName, @Password)", parameters);

                const string sqlQuery = "select case when exists (SELECT 1 FROM \"Users\" WHERE \"UserName\" = @UserName and " +
                      "\"Password\" = @Password and \"Active\" = '1') then CAST('1' as bool) " +
                      "when exists (SELECT 1 FROM \"Employees\" WHERE \"EmployeeId\" = @UserName " +
                      "and (\"Password\" = @Password or \"HrPassword\" = @HrPassword and \"Active\" = '1')) then CAST('1' as bool) " +
                      "else CAST('0' as bool) end";
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", model.UserName);
                parameters.Add("@Password", model.Password);
                parameters.Add("@HrPassword", model.HrPassword);
                return dbConnection.QueryFirstOrDefault<bool>(sqlQuery, parameters);
            }
        }

        public int GetRole(string userName)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                //return await dbConnection.QueryFirstOrDefaultAsync<bool>("select * from \"PasswordSign\"(@UserName, @Password)", parameters);

                //const string sqlQuery = "select \"RoleId\" from \"Users\" where \"UserName\" = @UserName";
                const string sqlQuery = "select case when not exists (select \"RoleId\" from \"Users\" where \"UserName\" = @UserName and \"Active\" = '1') " +
                    "then  CAST(1 as int) ELSE (select \"RoleId\" from \"Users\" where \"UserName\" = @UserName and \"Active\" = '1') END";
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", userName);
                return dbConnection.QueryFirstOrDefault<int>(sqlQuery, parameters);
            }
        }

        public void LoadPrivilege()
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                const string sqlQuery = "select case when a.\"RoleId\" = - 1 then - 1 when " +
                    "b.\"UserPermission\" is not null then b.\"UserPermission\" when " +
                    "b.\"UserPermission\" is null and c.\"RolePermission\" is not null " +
                    "then c.\"RolePermission\" else 0 end as \"Permission\" from \"Users\" a " +
                    "left join \"UserPermissions\" b on a.\"UserId\" = b.\"UserId\" and b.\"MenuId\" = @MenuId " +
                    "left join \"RolePermissions\" c on a.\"RoleId\" = c.\"RoleId\" and c.\"MenuId\" = @MenuId " +
                    "where a.\"UserId\" = @UserId";
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", CurrentUserId);
                parameters.Add("@MenuId", Helper.CurrentPage());
                try
                {
                    CurrentPrivilege = dbConnection.Query<int?>(sqlQuery, parameters).FirstOrDefault() ?? 0;
                }
                catch (Exception)
                {
                    //
                }
            }
        }

        public bool ChangePassword(LoginResponse model)
        {
            using (var dbConnection = new NpgsqlConnection(Config.DbInfo))
            {
                if (dbConnection.State == ConnectionState.Closed) dbConnection.Open();
                using (var transaction = dbConnection.BeginTransaction())
                {
                    var parameters = new DynamicParameters();
                    const string sqlQuery = "UPDATE \"Employees\" SET \"Password\" = @Password " +
                        "WHERE \"EmployeeId\" = @EmployeeId and \"Active\" = '1'";

                    parameters.Add("@Password", model.Password);
                    parameters.Add("@EmployeeId", model.UserName);
                    try
                    {
                        dbConnection.Query(sqlQuery, parameters, transaction);

                        transaction.Execute("UPDATE \"Users\" SET \"Password\" = @Password " +
                                            "WHERE \"UserName\" = @UserName and \"Active\" = '1'",
                            new { UserName = model.UserName, PassWord = model.Password });
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }


                return true;
            }
        }
    }
}