using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person Name can't be null")]
        public string? PersonName { get; set; }
        [Required(ErrorMessage = "Email Can't be null")]
        [EmailAddress(ErrorMessage = "Email address should be in a valid format")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetter { get; set; }

        public Person ToPerson()
        {
            return new Person()
            {
                PersonName = PersonName,
                Address = Address,
                Email = Email,
                CountryID = CountryID,
                Gender = Gender.ToString(),
                DateOfBirth = DateOfBirth,
                ReceiveNewsLetter = ReceiveNewsLetter
            };
        }

    }
}
