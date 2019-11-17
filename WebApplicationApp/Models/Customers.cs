using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationApp.Models
{
    public class Customers
    {

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Full Name")]
        public string Name { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        [Display(Name = "Tipo de membresía")]
        public byte MembershipTypeId { get; set; }

        [Display(Name = "Fecha de nacimiento")]
        //[Min18YearsIfAMember]
        [DataType(DataType.DateTime)]
        public DateTime? Birthdate { get; set; }

        public MembershipType MembershipType { get; set; }
    }
}