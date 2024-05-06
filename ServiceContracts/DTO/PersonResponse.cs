using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonResponse
    {
        public Guid PersonID { get; set; }
        public string? PersonName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? CountryName { get; set; }
        public bool ReceiveNewsLetter { get; set; }
        public double? Age { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;
            PersonResponse other = (PersonResponse)obj;
            return PersonID == other.PersonID &&
                this.PersonName == other.PersonName &&
                this.Email == other.Email &&
                this.Address == other.Address &&
                this.Age == other.Age &&
                this.ReceiveNewsLetter == other.ReceiveNewsLetter &&
                this.CountryName == other.CountryName &&
                this.CountryID == other.CountryID;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
        public override string ToString()
        {
            return $"Person Id = {this.PersonID}, Person Name = {this.PersonName}, Email = {this.Email}, Date of birth = {this.DateOfBirth}," +
                $"Address = {Address}, Gender = {Gender}, Country Id = {CountryID}, Country Name = {CountryName} Age = {Age}";
        }
        public PersonUpdateRequest ToPersonUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonID,
                PersonName = PersonName,
                Email = Email,
                Address = Address,
                CountryID = CountryID,
                DateOfBirth = DateOfBirth,
                Gender = (GenderOptions)Enum.Parse(typeof(GenderOptions), Gender, true),
                ReceiveNewsLetter = ReceiveNewsLetter,
            };
        }
    }
    public static class PersonExtensions
    {
        public static PersonResponse ToPersonResponse(this Person person)
        {
            return new PersonResponse()
            {
                PersonID = person.PersonID,
                CountryID = person.CountryID,
                Email = person.Email,
                Address = person.Address,
                Age = (person.DateOfBirth != null) ? (int)(Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365)) : 0,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                PersonName = person.PersonName,
                ReceiveNewsLetter = person.ReceiveNewsLetter
            };
        }
    }
}
