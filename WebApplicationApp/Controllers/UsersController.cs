using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationApp.Models;
using WebApplicationApp.Models.ViewModels;

namespace WebApplicationApp.Controllers
{
    [Authorize(Roles = RoleName.Admin)]
    public class UsersController : Controller
    {
        // GET: Users

        private ApplicationDbContext _context;
        public UsersController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            //Necesito el UserManager para trabajar con los usuarios
            var userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(_context));

            //Traigo todos los usuario a la lista
            var users = userManager.Users.ToList();

            //Creo una lista vacia
            var usersView = new List<UserView>();

            //Recorro la lista con todos los usuarios
            foreach (var user in users)
            {
                var userView = new UserView
                {
                    Email = user.Email,
                    Name = user.UserName,
                    UserID = user.Id
                };
                //Asigno lo recogido a mi lista vacia
                usersView.Add(userView);
            }

            //Devuelvo la lista preparada con los usuarios
            return View(usersView);
        }

        public ActionResult Roles(string userID)
        {
            if (userID == null) return HttpNotFound();
            //Necesito el UserManager para trabajar con los usuarios
            var userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(_context));

            var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(_context));

            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.ToList();
            //Busco el usuario que me envio la vista
            var user = users.Find(u => u.Id == userID);

            //Lista que guardara los roles del mi usuario
            var rolesView = new List<RoleView>();

            //Recorro la lista de roles del usuario
            foreach (var item in user.Roles)
            {
                //Busco el nombre del rol en la lista de roles de arriba
                var role = roles.Find(r => r.Id == item.RoleId);

                //Armamos la lista de role con su nombre
                var roleView = new RoleView
                {
                    Name = role.Name,
                    RoleID = role.Id
                };

                //Asignamos la lista con los roles a la vacia de arriba
                rolesView.Add(roleView);
            }

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                //No se cuales roles tiene el usuario, tenemos que pasarle una lista vacia y luego llenarla
                Roles = rolesView
            };

            return View(userView);
        }

        public ActionResult AddRole(string userID)
        {

            if (userID == null)
            {
                return HttpNotFound();
            }

            var userManager = new UserManager<ApplicationUser>
                (new UserStore<ApplicationUser>(_context));

            var roleManager = new RoleManager<IdentityRole>
                (new RoleStore<IdentityRole>(_context));

            var users = userManager.Users.ToList();

            //Busco el usuario que me envio la vista
            var user = users.Find(u => u.Id == userID);

            if (user == null)
            {
                return HttpNotFound();
            }

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
            };

            var list = roleManager.Roles.ToList();
            list.Add(new IdentityRole { Id = "", Name = "[Seleccione un rol...]" });
            list.OrderBy(r => r.Name).ToList();
            ViewBag.RoleID = new SelectList(list, "Id", "Name");

            return View(userView);


        }

        [HttpPost]
        public ActionResult AddRole(string userID, FormCollection form)
        {
            var roleID = Request["RoleID"];

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

            var users = userManager.Users.ToList();

            //Busco el usuario que me envio la vista
            var user = users.Find(u => u.Id == userID);

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
            };
            if (string.IsNullOrEmpty(roleID))
            {
                ViewBag.Error = "Debes seleccionar un rol";


                var list = roleManager.Roles.ToList();
                list.Add(new IdentityRole { Id = "", Name = "[Seleccione un rol...]" });
                list.OrderBy(r => r.Name).ToList();
                ViewBag.RoleID = new SelectList(list, "Id", "Name");

                return View(userView);
            }

            var roles = roleManager.Roles.ToList();
            var role = roles.Find(r => r.Id == roleID);

            if (!userManager.IsInRole(userID, role.Name))
            {
                userManager.AddToRole(userID, role.Name);
            }

            var rolesView = new List<RoleView>();

            //Recorro la lista de roles del usuario
            foreach (var item in user.Roles)
            {
                //Busco el nombre del rol en la lista de roles de arriba
                role = roles.Find(r => r.Id == item.RoleId);

                //Armamos la lista de role con su nombre
                var roleView = new RoleView
                {
                    Name = role.Name,
                    RoleID = role.Id
                };

                //Asignamos la lista con los roles a la vacia de arriba
                rolesView.Add(roleView);
            }

            userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                //No se cuales roles tiene el usuario, tenemos que pasarle una lista vacia y luego llenarla
                Roles = rolesView
            };


            return View("Roles", userView);

        }

        public ActionResult Delete(string userID, string roleID)
        {
            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(roleID))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            }

            //Validar que exista el usuario y el rol
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

            var user = userManager.Users.ToList().Find(u => u.Id == userID);
            var role = roleManager.Roles.ToList().Find(r => r.Id == roleID);

            //Buscamos para saber si el usuario pertenece al role
            if (userManager.IsInRole(user.Id, role.Name))
            {
                //Eliminamos al usuario de ese role
                userManager.RemoveFromRole(user.Id, role.Name);
            }

            //Preparamos la vista de roles

            var users = userManager.Users.ToList();
            var roles = roleManager.Roles.ToList();
            

            var rolesView = new List<RoleView>();

            //Recorro la lista de roles del usuario
            foreach (var item in user.Roles)
            {
                //Busco el nombre del rol en la lista de roles de arriba
                role = roles.Find(r => r.Id == item.RoleId);

                //Armamos la lista de role con su nombre
                var roleView = new RoleView
                {
                    Name = role.Name,
                    RoleID = role.Id
                };

                //Asignamos la lista con los roles a la vacia de arriba
                rolesView.Add(roleView);
            }

            var userView = new UserView
            {
                Email = user.Email,
                Name = user.UserName,
                UserID = user.Id,
                //No se cuales roles tiene el usuario, tenemos que pasarle una lista vacia y luego llenarla
                Roles = rolesView
            };


            return View("Roles", userView);


        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}