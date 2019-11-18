using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationApp.Models;

namespace WebApplicationApp.Controllers
{
    public class RentalsController : Controller
    {
        private ApplicationDbContext _context;

        public RentalsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Rentals
        public ActionResult Index()
        {
            return View(_context.Rentals.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}