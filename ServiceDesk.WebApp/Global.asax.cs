//using Microsoft.AspNet.WebFormsDependencyInjection.Unity;
//using HouseofCat.DependencyInjection.WebForms.Unity;
using Microsoft.Extensions.DependencyInjection;
using Unity;

using ServiceDesk.Data.Interfaces;
using ServiceDesk.Data.Repositories;
using ServiceDesk.Data.Services;
using System;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;

namespace ServiceDesk.WebApp
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            //var container = this.AddUnity();

            var collection = new ServiceCollection();
            collection.AddScoped<IAuthorityRepository, AuthorityRepository>();
            collection.AddScoped<IMenuRepository, MenuRepository>();
            collection.AddScoped<IMenuIconRepository, MenuIconRepository>();
            collection.AddScoped<IRoleRepository, RoleRepository>();
            collection.AddScoped<IDivisionRepository, DivisionRepository>();
            collection.AddScoped<IDepartmentRepository, DepartmentRepository>();
            collection.AddScoped<ITaskRepository, TaskRepository>();
            collection.AddScoped<IUserRepository, UserRepository>();
            collection.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            collection.AddScoped<IUserPermissionRepository, UserPermissionRepository>();
            collection.AddScoped<IPositionRepository, PositionRepository>();
            collection.AddScoped<IStatusRepository, StatusRepository>();
            collection.AddScoped<IIssuesRepository, IssuesRepository>();
            collection.AddScoped<ITaskExecuteRepository, TaskExecuteRepository>();
            collection.AddScoped<IPriorityRepository, PriorityRepository>();
            collection.AddScoped<ITagRepository, TagRepository>();
            collection.AddScoped<IEmployeeRepository, EmployeeRepository>();
            collection.AddScoped<ITransferTaskRepository, TransferTaskRepository>();
            collection.AddScoped<ISendMailService, SendMailService>();


            var provider = new ServiceProvider(collection.BuildServiceProvider());
            HttpRuntime.WebObjectActivator = provider;

            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}