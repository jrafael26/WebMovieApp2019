using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApplicationApp.Models;
using WebApplicationApp.Models.ViewModels;

namespace WebApplicationMovie.Controllers
{
    [Authorize(Roles = RoleName.CanManageMovies)]
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(_context.Movies.
                Include(m => m.Genre).ToList());
        }

        public ActionResult Create()
        {
            var genres = _context.Genres.ToList();

            var viewModel = new MovieFormViewModel
            {
                Genres = genres,
                Movie = new Movie()
            };
            return View("Create", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var genres = _context.Genres.ToList();

                var viewModel = new MovieFormViewModel
                {
                    Genres = genres,
                    Movie = new Movie()
                };
                return View("Create", viewModel);
            }

            if (model.Movie.Id == 0)
            {
                _context.Movies.Add(model.Movie);
            }
            else
            {
                var _movie = _context.Movies.
                    SingleOrDefault(m => m.Id == model.Movie.Id);

                if (_movie == null) return View();

                _movie.Name = model.Movie.Name;
                _movie.GenreId = model.Movie.GenreId;
                _movie.DateAdded = model.Movie.DateAdded;
                _movie.ReleaseDate = model.Movie.ReleaseDate;
                _movie.NumberInStock = model.Movie.NumberInStock;
                _movie.NumberAvailable = model.Movie.NumberAvailable;


            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var movie = _context.Movies.ToList().
                Find(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            var genres = _context.Genres.ToList();

            var viewModel = new MovieFormViewModel
            {
                Genres = genres,
                Movie = movie
            };

            return View("Edit", viewModel);
        }

        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {

                return HttpNotFound();
            }

            var movie = _context.Movies.Include(m => m.Genre).ToList().
                Find(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var movie = _context.Movies.Include(m => m.Genre).ToList().
                Find(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var movie = _context.Movies.Find(id);
            _context.Movies.Remove(movie);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose(); // libera los recursos
        }
    }
}