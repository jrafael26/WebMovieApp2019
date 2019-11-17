using System.Collections.Generic;

namespace WebApplicationApp.Models.ViewModels
{
    public class MovieFormViewModel
    {
        public Movie Movie { get; set; }
        public List<Genre> Genres { get; set; }
    }
}