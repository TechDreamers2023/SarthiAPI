using Sarthi.Core.Models;

namespace Sarthi.Services.Interfaces
{
    public interface ICommonService
    {
        Task<IEnumerable<UserAuth>> GetUserAuthentication(string emailAddress, string password);

        Task<IEnumerable<RequestVendorDetailsModel>> FindAvailableServiceProviders(string pickUpCityName, string dropOffCityName,decimal distanceKM);
    }
}