using Sarthi.Core.Models;

namespace Sarthi.Core.Interfaces
{
    public interface ICommonRepository
    {
        Task<IEnumerable<UserAuth>> UserAuthentication(string emailAddress, string password);
        Task<IEnumerable<RequestVendorDetailsModel>> FindAvailableServiceProviders(string pickUpCityName, string dropOffCityName, decimal distanceKM);
    }
}
