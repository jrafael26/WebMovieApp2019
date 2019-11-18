using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicationApp.Models.ViewModels
{
    public class RentalFormViewModel
    {
        public int CustomerID { get; set; }

        public List<int> MovieID { get; set; }
    }
}