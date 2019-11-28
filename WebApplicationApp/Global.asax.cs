using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplicationApp.App_Start;
using WebApplicationApp.Models;

namespace WebApplicationApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            CreateRoles(db);
            CreateSuperuser(db);
            AddPermisionsToSuperUser(db);
            db.Dispose();
            Mapper.Initialize(c => c.AddProfile<MappingProfile>());
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void AddPermisionsToSuperUser(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));

            var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(db));

            var user = userManager.FindByName("admin@webmovieapp.com");

            if (!userManager.IsInRole(user.Id, RoleName.CanManageCustomers))
            {
                userManager.AddToRole(user.Id, RoleName.CanManageCustomers);
            }

            if (!userManager.IsInRole(user.Id, RoleName.CanManageMovies))
            {
                userManager.AddToRole(user.Id, RoleName.CanManageMovies);
            }

            if (!userManager.IsInRole(user.Id, RoleName.CanManageRentals))
            {
                userManager.AddToRole(user.Id, RoleName.CanManageRentals);
            }

            if (!userManager.IsInRole(user.Id, "Admin"))
            {
                userManager.AddToRole(user.Id, "Admin");
            }
        }

        private void CreateSuperuser(ApplicationDbContext db)
        {
            var userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(db));

            var user = userManager.FindByName("admin@webmovieapp.com");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "admin@webmovieapp.com",
                    Email = "admin@webmovieapp.com"
                };
                userManager.Create(user, "Admin123.");
            }
        }

        private void CreateRoles(ApplicationDbContext db)
        {
            var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(db));

            if (!roleManager.RoleExists("CanManageMovies"))
            {
                roleManager.Create(new IdentityRole("CanManageMovies"));
            }

            if (!roleManager.RoleExists("CanManageCustomers"))
            {
                roleManager.Create(new IdentityRole("CanManageCustomers"));
            }

            if (!roleManager.RoleExists("CanManageRentals"))
            {
                roleManager.Create(new IdentityRole("CanManageRentals"));
            }

            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }
        }
    }
}
