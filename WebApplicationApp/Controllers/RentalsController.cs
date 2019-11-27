using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationApp.Models;

using System.Data.Entity;
using WebApplicationApp.Dtos;
using System.Net;

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
            return View(_context.Rentals
                .Include(c => c.Customer)
                .Include(m => m.Movie)
                .ToList());
        }

        public ActionResult NewRental()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewRental(NewRentalDto newRental)
        {
            var customer = _context.Customers.Single(
                c => c.Id == newRental.CustomerId);

            var movies = _context.Movies.Where(
                m => newRental.MovieIds.Contains(m.Id)).ToList();

            foreach (var movie in movies)
            {
                if (movie.NumberAvailable == 0)
                    return ViewBag.Error = "Movie is not available.";

                movie.NumberAvailable--;

                var rental = new Rental
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };

                _context.Rentals.Add(rental);
            }

            _context.SaveChanges();
            return RedirectToAction("NewRental");
        }


        public ActionResult UnRent()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UnRent(Rental model, FormCollection form)
        {
            
            var viewRental = _context.Rentals
                .Include(c => c.Customer)
                .Include(m => m.Movie)
                .ToList();
            var f = form["DateReturned"];
            Rental rent = viewRental.Find(r => r.Id == model.Id);
            //var rental = _context.Rentals.Find(model.Id);

            //Verificar que lo encuentra
            if (rent == null || rent.DateReturned == null)
            {
                ViewBag.Error = "Invalid Operetions.";
                return View();
            }

            if (rent.DateReturned !=null)
            {
                ViewBag.Error = "Invalid Operetions.";
                return View();
            }

                var customer = _context.Customers.Single(
                c => c.Id ==rent.Customer.Id);

            var movies = _context.Movies.Find(rent.Movie.Id);

            if (movies == null)
            {
                ViewBag.Error = "Movie is not available.";
                return View();
            }

            if (movies.NumberAvailable> movies.NumberInStock)
            {
                ViewBag.Error = "Invalid Operetions.";
                return View();
            }
            movies.NumberAvailable++;
            rent.DateReturned = DateTime.Parse(f.ToString());
            _context.Entry(movies).State = EntityState.Modified;
            _context.Entry(rent).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var viewRental = _context.Rentals
                .Include(c => c.Customer)
                .Include(m => m.Movie)
                .ToList();
            Rental rent = viewRental.Find(r => r.Id== id);
            if (rent == null)
            {
                return HttpNotFound();
            }
            return View("UnRent",rent);
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}