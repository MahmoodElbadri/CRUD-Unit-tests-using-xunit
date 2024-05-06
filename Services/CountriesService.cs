using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        private readonly List<Country> _countries;
        public CountriesService()
        {
            _countries = new List<Country>();
        }
        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            Country? country = countryAddRequest?.ToCountry();
            if (country == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if (country.CountryName == null)
            {
                throw new ArgumentException(nameof(country.CountryName));
            }
            if (_countries.Where(tmp => tmp.CountryName == country.CountryName).Count() > 0)
            {
                throw new ArgumentException($"{nameof(country.CountryName)} is already exist");
            }
            country.CountryID = Guid.NewGuid();
            _countries.Add(country);
            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            if (countryID == null) return null;
            Country? countryResponseFromGet = _countries.FirstOrDefault(_countries => _countries.CountryID == countryID);
            if (countryResponseFromGet == null) return null;
            return countryResponseFromGet.ToCountryResponse();
        }
    }
}
