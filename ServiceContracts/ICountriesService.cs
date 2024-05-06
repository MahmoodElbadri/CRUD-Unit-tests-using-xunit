using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic
    /// </summary>
    public interface ICountriesService
    {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        /// <summary>
        /// it returns all countries
        /// </summary>
        /// <returns>All Countries from the list as a list of country response</returns>
        List<CountryResponse> GetAllCountries();
        /// <summary>
        /// Get a specific country based on country ID
        /// </summary>
        /// <param name="countryID">country id U want to search</param>
        /// <returns>The country with country id U provided</returns>
        CountryResponse? GetCountryByCountryID(Guid? countryID);
    }
}
