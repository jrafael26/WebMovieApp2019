using System.Linq;
using System.Web.Mvc;
using WebApplicationApp.Models;

namespace WebApplicationApp.Controllers
{
    [Authorize(Roles = RoleName.CanManageMovies)]
    public class GenresController : Controller
    {
        private ApplicationDbContext _context;

        // GET: Genres

        public GenresController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            return View(_context.Genres.ToList());
        }

        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Genre model)
        {
            if (!ModelState.IsValid) return View(model);

            var genre = _context.Genres.Find(model.Id);
            if (genre == null)
            {
                _context.Genres.Add(model);
            }
            else
            {
                genre.Name = model.Name;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var genre = _context.Genres.ToList().Find(g => g.Id == id);

            if (genre == null)
            {
                return HttpNotFound();
            }

            return View(genre);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var genre = _context.Genres.ToList().Find(g => g.Id == id);

            if (genre == null)
            {
                return HttpNotFound();
            }

            return View(genre);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var genre = _context.Genres.ToList().
                Find(g => g.Id == id);

            if (genre == null)
            {
                return HttpNotFound();
            }

            return View(genre);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var genre = _context.Genres.Find(id);
            _context.Genres.Remove(genre);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
    }
}