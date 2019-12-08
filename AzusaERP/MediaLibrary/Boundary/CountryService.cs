using System.Collections.Generic;
using moe.yo3explorer.azusa.MediaLibrary.Entity;

namespace moe.yo3explorer.azusa.MediaLibrary.Boundary
{
    static class CountryService
    {
        public static IEnumerable<Country> GetCountries()
        {
            return AzusaContext.GetInstance().DatabaseDriver.GetAllCountries();
        }
    }
}
