using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Entities;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _outputHelper;
        private readonly List<PersonAddRequest> _personAddRequests;
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personAddRequests = new List<PersonAddRequest>();
            _countriesService = new CountriesService();
            _personsService = new PersonsService();
            _outputHelper = testOutputHelper;
        }
        private List<PersonAddRequest> addPersonsAddRequestsToList()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
            CountryResponse countryResponse2 = _countriesService.AddCountry(countryAddRequest2);
            PersonAddRequest personAddRequest1 = new PersonAddRequest()
            {
                CountryID = countryResponse1.CountryID,
                Address = "Some Address",
                DateOfBirth = DateTime.Parse("1997-1-1"),
                Email = "jack@email.com",
                Gender = GenderOptions.FEMALE,
                PersonName = "Mahmoud",
                ReceiveNewsLetter = true,
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest()
            {
                CountryID = countryResponse2.CountryID,
                Address = "Some Address",
                DateOfBirth = DateTime.Parse("1997-1-1"),
                Email = "jack@email.com",
                Gender = GenderOptions.FEMALE,
                PersonName = "Hassan",
                ReceiveNewsLetter = true,
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest()
            {
                CountryID = countryResponse2.CountryID,
                Address = "Some Address",
                DateOfBirth = DateTime.Parse("1997-1-1"),
                Email = "jack@email.com",
                Gender = GenderOptions.FEMALE,
                PersonName = "Aboud",
                ReceiveNewsLetter = true,
            };
            List<PersonAddRequest> personAddRequests = new List<PersonAddRequest>() { personAddRequest1, personAddRequest2,
                personAddRequest3 };
            return personAddRequests;
        }
        #region Add Person
        [Fact]
        public void AddPerosn_NullPerson()
        {
            PersonAddRequest? personAddRequest = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.AddPerson(personAddRequest);
            });
        }
        [Fact]
        public void AddPerosn_PersonNameIsNull()
        {
            PersonAddRequest? personAddRequest = new PersonAddRequest
            {
                PersonName = null,
            };
            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.AddPerson(personAddRequest);
            });
        }

        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "john",
                Address = "USA",
                CountryID = Guid.NewGuid(),
                DateOfBirth = DateTime.Parse("1990-01-01"),
                Email = "nQ5pU@example.com",
                Gender = GenderOptions.MALE,
                ReceiveNewsLetter = true,
            };
            PersonResponse personResponseFromAdd = _personsService.AddPerson(personAddRequest);
            List<PersonResponse> personResponseFromGet = _personsService.GetPersonList();
            Assert.True(personResponseFromAdd.PersonID != Guid.Empty);
            Assert.Contains(personResponseFromAdd, personResponseFromGet);
        }
        #endregion

        #region GetPersonByPersonID
        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            Guid? personID = null;
            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);
            Assert.Null(personResponse);
        }
        [Fact]
        public void GetPersonByPersonID_ValidPersonID()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Canada" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                CountryID = countryResponse.CountryID,
                Address = "Some Address",
                DateOfBirth = DateTime.Parse("1990-1-1"),
                Email = "email@sample.com",
                Gender = GenderOptions.MALE,
                PersonName = "Jack",
                ReceiveNewsLetter = true
            };
            PersonResponse personResponseFromAdd = _personsService.AddPerson(personAddRequest);
            PersonResponse? personResponseFromGet = _personsService.GetPersonByPersonID(personResponseFromAdd.PersonID);
            Assert.Equal(personResponseFromAdd, personResponseFromGet);
        }
        #endregion

        #region GetAllPersons
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            var personResponseFromGet = _personsService.GetPersonList();
            Assert.Empty(personResponseFromGet);
        }

        [Fact]
        public void GetAllPersons_NonEmptyList()
        {
            List<PersonAddRequest> personAddRequests = addPersonsAddRequestsToList();
            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
                personResponsesFromAdd.Add(personResponse);
            }
            _outputHelper.WriteLine("Expected");
            foreach (PersonResponse person in personResponsesFromAdd)
            {
                _outputHelper.WriteLine(person.ToString());
            }
            List<PersonResponse> personResponsesFromGet = _personsService.GetPersonList();
            foreach (PersonResponse response in personResponsesFromAdd)
            {
                Assert.Contains(response, personResponsesFromGet);
            }
            _outputHelper.WriteLine("Actual");
            foreach (PersonResponse person in personResponsesFromGet)
            {
                _outputHelper.WriteLine(person.ToString());
            }
        }
        #endregion

        #region GetFilteredPerson
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {

            List<PersonAddRequest> personAddRequests = addPersonsAddRequestsToList();
            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
                personResponsesFromAdd.Add(personResponse);
            }
            _outputHelper.WriteLine("Expected");
            foreach (PersonResponse person in personResponsesFromAdd)
            {
                _outputHelper.WriteLine(person.ToString());
            }
            List<PersonResponse> personResponsesFromGet = _personsService.GetFilteredPersons(nameof(Person.PersonName), "");
            foreach (PersonResponse response in personResponsesFromAdd)
            {
                Assert.Contains(response, personResponsesFromGet);
            }
            _outputHelper.WriteLine("Actual");
            foreach (PersonResponse person in personResponsesFromGet)
            {
                _outputHelper.WriteLine(person.ToString());
            }
        }

        [Fact]
        public void GetFilteredPersons_NonEmptyString()
        {

            List<PersonAddRequest> personAddRequests = addPersonsAddRequestsToList();
            List<PersonResponse> personResponsesFromAdd = new List<PersonResponse>();
            foreach (PersonAddRequest personAddRequest in personAddRequests)
            {
                PersonResponse personResponse = _personsService.AddPerson(personAddRequest);
                personResponsesFromAdd.Add(personResponse);
            }
            _outputHelper.WriteLine("Expected");
            foreach (PersonResponse person in personResponsesFromAdd)
            {
                _outputHelper.WriteLine(person.ToString());
            }
            List<PersonResponse> personResponsesFromGet = _personsService.GetFilteredPersons(nameof(Person.PersonName), "ou");
            foreach (PersonResponse response in personResponsesFromAdd)
            {
                if (response.PersonName != null)
                {
                    if (response.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(response, personResponsesFromGet);
                    }
                }
            }
            _outputHelper.WriteLine("Actual");
            foreach (PersonResponse person in personResponsesFromGet)
            {
                _outputHelper.WriteLine(person.ToString());
            }
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public void GetSortedPersons()
        {
            List<PersonAddRequest> person_requests = addPersonsAddRequestsToList();

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personsService.AddPerson(person_request);
                person_response_list_from_add.Add(person_response);
            }

            //print person_response_list_from_add
            _outputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _outputHelper.WriteLine(person_response_from_add.ToString());
            }
            List<PersonResponse> allPersons = _personsService.GetPersonList();

            //Act
            List<PersonResponse> persons_list_from_sort = _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //print persons_list_from_get
            _outputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_response_from_get in persons_list_from_sort)
            {
                _outputHelper.WriteLine(person_response_from_get.ToString());
            }
            person_response_list_from_add = person_response_list_from_add.OrderByDescending(temp => temp.PersonName).ToList();

            //Assert
            for (int i = 0; i < person_response_list_from_add.Count; i++)
            {
                Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            }
        }

        #endregion

        #region UpdateRequest
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            PersonUpdateRequest? personUpdateRequest = null;
            Assert.Throws<ArgumentNullException>(() =>
            {
                _personsService.UpdatePerson(personUpdateRequest);
            });
        }
        [Fact]
        public void UpdatePerson_InvalidPersonID()
        {
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest() { PersonID = Guid.NewGuid() };
            Assert.Throws<ArgumentException>(() =>
            {
                _personsService.UpdatePerson(personUpdateRequest);
            });
        }
        [Fact]
        public void UpdatePerson_NullPersonName()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse countryResponseFromAdd = _countriesService.AddCountry(countryAddRequest);

            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                PersonName = "John",
                CountryID = countryResponseFromAdd.CountryID,
                Email = "abc@example.com",
                ReceiveNewsLetter = true,
                Gender = GenderOptions.FEMALE,
            };

            PersonResponse personResponseFromAdd = _personsService.AddPerson(personAddRequest);

            PersonUpdateRequest personUpdateRequest = personResponseFromAdd.ToPersonUpdateRequest();

            personUpdateRequest.PersonName = null;
            Assert.Throws<ArgumentException>(() => _personsService.UpdatePerson(personUpdateRequest));
        }
        #endregion

        #region DeletePerson

        [Fact]
        public void DeletePerson_InvalidPersonID()
        {
            bool isDeleted = _personsService.DeletePerson(Guid.NewGuid());
            Assert.False(isDeleted);
        }
        [Fact]
        public void DeletePerson_ValidPersonID()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "m3'a3'a"
            };
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            PersonAddRequest personAddRequest = new PersonAddRequest()
            {
                CountryID = countryResponse.CountryID,
                PersonName = "jack",
                Email = "jackgrealish@mancity.com",
                Gender = GenderOptions.MALE,
            };
            PersonResponse personResponseFromAdd = _personsService.AddPerson(personAddRequest);
            bool isDeleted = _personsService.DeletePerson(personResponseFromAdd.PersonID);
            Assert.True(isDeleted);
        }
        #endregion
    }
}
