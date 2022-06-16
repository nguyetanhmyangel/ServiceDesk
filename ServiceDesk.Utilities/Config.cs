using System.Configuration;

namespace ServiceDesk.Utilities
{
    public class Config 
    {
        // Debug flag
        public const bool DebugEnabled = false; // Changed to true to debug  DbConnection

        //public static string DbConnectionString = ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

        public static string DbInfo = "User ID=postgres;Password=admin123;Host=localhost;Port=5432;Database=IssuesManagement;Pooling=true;";
        //public static string DbInfo = "User ID=fep;Password=Fvl123456;Host=172.19.9.40;Port=27500;Database=IssuesManagement;Pooling=true;";
        public const string ProjectName = "ServiceDesk";
        public const string Privilege = "Privilege";
        public const string RoleId = "RoleId";
        public const string UserId = "UserId";
        public const string UserName = "UserName";
        public const string Password = "Password";
        public const string GroupId = "GroupId";
        public const string DepartmentId = "DepartmentId";
        public const string DivisionId = "DivisionId";
        public const string TenancyId = "TenancyId";
        public const string LanguageId = "LanguageId";
        public const string RusLanguageId = "ru-Ru";// 
        public const string AutoLogin = "AutoLogin";
        public const string RandomKey = "P9lbw1KUI@#yz&Tr";
        public const short Default = 0; 
        public const short AllowList = 1; // List
        public const short AllowView = 2; // View
        public const short AllowAdd = 4; // Add
        public const short AllowDelete = 8; // Delete
        public const short AllowEdit = 16; // Edit
        public const short Admin = -1; // List
        public const short StockKeeper = 1; // View
        public const short Accounting = 8; // Add
        public const short Mod = 16; // Add
        public const string LoginUrl = "~/Account/Login.aspx";
        public const string AuthorityUrl = "~/Account/Authority.aspx";//
        public const string DefaultUrl = "~/Default.aspx";// 
        public const string LastUrl = "lasturl";// 
        public const short PasswordFailed = 2;
        public const short UserNotExist = 1;
        public const string FailedAttempts = "FailedAttempts";
        public const short Waiting = 6;
        public const short Processing = 1;
        public const short Complete = 5;
        public const short Pausing = 2;
        public const short Cancel = 3;
        public const short Transfer = 4;
        public const int SoftwareDepartment = 273;
        public const int NetworkDepartment = 272;
        public const int RepairDepartment = 275;
        public const int InfoTechDivision = 10;// 

        /*
         *
         *
         *
         */
    }
}