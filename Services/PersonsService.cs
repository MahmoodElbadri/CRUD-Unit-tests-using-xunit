using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Services.Helpers;
using ServiceContracts.Enums;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        private readonly List<Person> _persons;
        private readonly ICountriesService _countries;
        public PersonsService()
        {
            _countries = new CountriesService();
            _persons = new List<Person>();
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse? personResponse = person.ToPersonResponse();
            personResponse.CountryName = _countries.GetCountryByCountryID(personResponse.CountryID)?.CountryName;
            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));
            ValidationHelper.ModelValidation(personAddRequest);
            Person person = personAddRequest.ToPerson();
            person.PersonID = Guid.NewGuid();
            _persons.Add(person);
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetPersonList()
        {
            return _persons.Select(tmp => tmp.ToPersonResponse()).ToList();
        }

        public PersonResponse? GetPersonByPersonID(Guid? personID)
        {
            if (personID == null) return null;
            Person? person = _persons.FirstOrDefault(tmp => tmp.PersonID == personID);
            if (person == null) return null;
            else return person.ToPersonResponse();
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? serachString)
        {
            List<PersonResponse> allPersons = GetPersonList();
            var matchingPersons = allPersons;
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(serachString)) return allPersons;
            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(tmp =>
                    (!string.IsNullOrEmpty(tmp.PersonName) ?
                    tmp.PersonName.Contains(serachString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(tmp =>
                    (!string.IsNullOrEmpty(tmp.Email) ?
                    tmp.Email.Contains(serachString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(tmp =>
                    (tmp.DateOfBirth != null) ?
                    tmp.DateOfBirth.Value.ToString("dd MMMM yyyy")
                    .Contains(serachString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(tmp =>
                    (!string.IsNullOrEmpty(tmp.Gender) ?
                    tmp.Gender.Contains(serachString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.CountryID):
                    matchingPersons = allPersons.Where(tmp =>
                    (!string.IsNullOrEmpty(tmp.CountryName) ?
                    tmp.CountryName.Contains(serachString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Address):
                    matchingPersons = allPersons.Where(tmp =>
                    (!string.IsNullOrEmpty(tmp.Address) ?
                    tmp.Address.Contains(serachString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default:
                    matchingPersons = allPersons;
                    break;
            }
            return matchingPersons;
        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy)) return allPersons;
            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
                switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC)
                => allPersons.OrderBy(tmp => tmp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(tmp => tmp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC)
                => allPersons.OrderBy(tmp => tmp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(tmp => tmp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC)
                => allPersons.OrderBy(tmp => tmp.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(tmp => tmp.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC)
                => allPersons.OrderBy(tmp => tmp.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(tmp => tmp.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC)
                => allPersons.OrderBy(tmp => tmp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(tmp => tmp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrderOptions.ASC)
                => allPersons.OrderBy(tmp => tmp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.CountryName), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(tmp => tmp.CountryName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC)
                => allPersons.OrderBy(tmp => tmp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(tmp => tmp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.ASC)
                => allPersons.OrderBy(tmp => tmp.ReceiveNewsLetter).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetter), SortOrderOptions.DESC)
                => allPersons.OrderByDescending(tmp => tmp.ReceiveNewsLetter).ToList(),

                _ => allPersons
            };
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null) throw new ArgumentNullException(nameof(personUpdateRequest));
            ValidationHelper.ModelValidation(personUpdateRequest);
            Person? matchingPerson = _persons.FirstOrDefault(tmp => tmp.PersonID == personUpdateRequest.PersonID);
            if (matchingPerson == null) throw new ArgumentException("Given person id is not correct");
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.ReceiveNewsLetter = personUpdateRequest.ReceiveNewsLetter;
            return matchingPerson.ToPersonResponse();
        }

        public bool DeletePerson(Guid? personID)
        {
            if (personID == null) throw new ArgumentNullException(nameof(personID));
            Person? person = _persons.FirstOrDefault(tmp=>tmp.PersonID == personID);
            if (person == null) return false;
            _persons.RemoveAll(tmp=>tmp.PersonID==personID);
            return true;
        }
    }
}
