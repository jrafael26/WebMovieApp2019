using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplicationApp.Dtos;
using WebApplicationApp.Models;
using System.Data.Entity;

namespace WebApplicationApp.Controllers.api
{
    public class NewRentalsController : ApiController
    {
        private readonly ApplicationDbContext _context;

        public NewRentalsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            var customer = _context.Customers.Single(
                c => c.Id == newRental.CustomerId);

            var movies = _context.Movies.Where(
                m => newRental.MovieIds.Contains(m.Id)).ToList();

            foreach (var movie in movies)
            {
                if (movie.NumberAvailable == 0)
                    return BadRequest("Movie is not available.");

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

            return Ok();
        }

        [HttpPost]
        public IHttpActionResult UnRent(Rental newRental)
        {
            var viewRental = _context.Rentals
                .Include(c => c.Customer)
                .Include(m => m.Movie)
                .ToList();

            Rental rent = viewRental.Find(r => r.Id == newRental.Id);
            //var rental = _context.Rentals.Find(model.Id);
            if (rent == null || rent.DateRented ==null)
            {
                return BadRequest("Invalid Operetions.");
            }
            //Verificar que lo encuentra
            var customer = _context.Customers.Single(
                c => c.Id == rent.Customer.Id);

            var movies = _context.Movies.Find(rent.Movie.Id);

            if (movies == null)
            {
                return BadRequest("Movie is not available.");
            }

            if (movies.NumberAvailable > movies.NumberInStock)
            {
                return BadRequest("Invalid Operetions.");
            }

            movies.NumberAvailable++;

            _context.Entry(movies).State = EntityState.Modified;
            _context.SaveChanges();

            return Ok();
        }
    }
}
