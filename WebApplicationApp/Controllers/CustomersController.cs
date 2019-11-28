using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApplicationApp.Models;
using WebApplicationApp.Models.ViewModels;

namespace WebApplicationApp.Controllers
{
    [Authorize(Roles = RoleName.CanManageCustomers)]
    [Authorize(Roles = RoleName.CanManageRentals)]
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;
        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        private IEnumerable<Customers> GetCustomers()
        {
            return _context.Customers.Include(c => c.MembershipType).ToList();
        }

        // GET: Customers
        public ActionResult Index()
        {
            //return View("Partial/_ListPartial", _context.Customers.ToList());
            return View(_context.Customers.
                Include(c => c.MembershipType).ToList());
        }

        public ActionResult Create()
        {
            var membershipType = _context.Memberships.ToList();

            var customerFormViewModel = new CustomerFormViewModel()
            {
                Customers = new Customers(),
                MembershipTypes = membershipType
            };
            return View("Create", customerFormViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(CustomerFormViewModel model)
        {

            if (!ModelState.IsValid)
            {
                var membershipType = _context.Memberships.ToList();

                var customerFormViewModel = new CustomerFormViewModel()
                {
                    Customers = new Customers(),
                    MembershipTypes = membershipType
                };
                return View("Create", customerFormViewModel);
            }

            if (model.Customers.Id == 0)
            {
                _context.Customers.Add(model.Customers);
            }
            else
            {
                var _customer = _context.Customers.
                    SingleOrDefault(c => c.Id == model.Customers.Id);
                if (_customer == null) return View();

                _customer.Name = model.Customers.Name;
                _customer.IsSubscribedToNewsletter = model.Customers.IsSubscribedToNewsletter;
                _customer.Birthdate = model.Customers.Birthdate;
                _customer.MembershipTypeId = model.Customers.MembershipTypeId;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Customers");
        }
        //private List<Customers> GetCustomers()
        //{
        //    return new List<Customers>()
        //    {
        //        new Customers() {Id=1,Name="Juan", Apellido="Cabrera", Edad=25},
        //        new Customers() {Id=2,Name="Carlos", Apellido="Sosa", Edad=29},
        //        new Customers() {Id=3,Name="Maria", Apellido="Ventura", Edad=20},
        //        new Customers() {Id=4,Name="Mario", Apellido="Bross", Edad=35},
        //        new Customers() {Id=4,Name="Andres", Apellido="Cepeda", Edad=32},
        //        new Customers() {Id=6,Name="Jose", Apellido="Marte", Edad=39}
        //    };
        //}

        public ActionResult Edit(int? id)
        {
            var membershipType = _context.Memberships.ToList();
            var customer = _context.Customers.ToList().
                Find(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            var customerFormViewModel = new CustomerFormViewModel()
            {
                Customers = customer,
                MembershipTypes = membershipType
            };
            return View("Edit", customerFormViewModel);

        }

        [HttpPost]
        public ActionResult Edit(Customers customers)
        {
            return View(_context.Customers.ToList().
                Find(c => c.Id == customers.Id));
        }

        public ActionResult Details(int? id)
        {
            var customer = _context.Customers.
                Include(c => c.MembershipType).ToList().
                Find(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);

        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var customer = _context.Customers.
                Include(c => c.MembershipType).ToList().
                Find(c => c.Id == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var customer = _context.Customers.Find(id);
            _context.Customers.Remove(customer);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose(); // libera los recursos
        }
    }
}