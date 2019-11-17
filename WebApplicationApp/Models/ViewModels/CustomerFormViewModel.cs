using System.Collections.Generic;

namespace WebApplicationApp.Models.ViewModels
{
    public class CustomerFormViewModel
    {
        public Customers Customers { get; set; }
        public List<MembershipType> MembershipTypes { get; set; }
    }
}