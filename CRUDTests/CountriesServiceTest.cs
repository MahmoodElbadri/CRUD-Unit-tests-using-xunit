using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _outputHelper;
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
            _outputHelper = new TestOutputHelper();
        }

        #region AddCountry
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;
            //Act
            //Assert
            Assert.Throws<ArgumentNullException>(() => _countriesService.AddCountry(countryAddRequest));
        }
        [Fact]
        public void RemoveCountry_NullCountryName()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = null,
            };
            Assert.Throws<ArgumentException>(() => _countriesService.AddCountry(countryAddRequest));
        }
        [Fact]
        public void RemoveCountry_DuplicateCountryName()
        {
            CountryAddRequest countryAddRequest1 = new CountryAddRequest()
            {
                CountryName = "usa",
            };
            CountryAddRequest countryAddRequest2 = new CountryAddRequest()
            {
                CountryName = "usa",
            };
            Assert.Throws<ArgumentException>(
                () =>
                {
                    _countriesService.AddCountry(countryAddRequest1);
                    _countriesService.AddCountry(countryAddRequest2);
                }
                );
        }

        [Fact]
        public void AddCountry_ProperCountry()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest()
            {
                CountryName = "usa",
            };
            //Act
            //Assert
            CountryResponse countryResponse = _countriesService.AddCountry(countryAddRequest);
            Assert.True(countryResponse.CountryID != Guid.Empty);
            List<CountryResponse> responseList = _countriesService.GetAllCountries();
            Assert.Contains(countryResponse, responseList);
        }
        #endregion

        #region GetAllCountries

        [Fact]
        public void GetAllCountries_EmptyList()
        {
            var actualCountryResposeList = _countriesService.GetAllCountries();
            Assert.Empty(actualCountryResposeList);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            List<CountryAddRequest> countryRequestList = new List<CountryAddRequest>()
            {
                new CountryAddRequest() {CountryName = "USA"},
                new CountryAddRequest() {CountryName="Japan"},
            };
            List<CountryResponse> countryResponseFromAdd = new List<CountryResponse>();
            foreach (var country in countryRequestList)
            {
                countryResponseFromAdd.Add(_countriesService.AddCountry(country));
            }
            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();
            foreach (var country in countryResponseFromAdd)
            {
                Assert.Contains(country, actualCountryResponseList);
            }
        }
        #endregion

        #region GetCountryByCountryID
        [Fact]
        public void GetCountryByCountryID_NULLCountryID()
        {
            Guid? countryID = null;
            CountryResponse? countryResponse = _countriesService.GetCountryByCountryID(countryID);
            Assert.Null(countryResponse);
        }
        [Fact]
        public void GetCountryByCountryID_ValidCountryID()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "USA",
            };
            CountryResponse countryResponseFromAdd =  _countriesService.AddCountry(countryAddRequest);
            CountryResponse? countryResponseFromGet = _countriesService.GetCountryByCountryID(countryResponseFromAdd.CountryID);
            Assert.Equal(countryResponseFromGet, countryResponseFromAdd);
        }
        #endregion
    }
}