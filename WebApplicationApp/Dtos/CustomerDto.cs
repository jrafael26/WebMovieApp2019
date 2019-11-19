using System;
using System.ComponentModel.DataAnnotations;
using WebApplicationApp.Models;

namespace WebApplicationApp.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public bool IsSubscribedToNewsletter { get; set; }

        public byte MembershipTypeId { get; set; }

        public MembershipType MembershipType { get; set; }

        // [Min18YearsIfAMember] esta validacion esta hecha en base a Customer y no CustomerDto
        public DateTime? Birthdate { get; set; }
    }
}