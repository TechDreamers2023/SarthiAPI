using Sarthi.Core.Models;
using Sarthi.Core.ViewModels;

namespace Sarthi.Core.Interfaces
{
    public interface ICommonRepository
    {
        Task<IEnumerable<UserAuth>> UserAuthentication(string emailAddress, string password);
        Task<IEnumerable<RequestVendorDetailsModel>> FindAvailableServiceProviders(string pickUpCityName, string dropOffCityName, decimal distanceKM);
        Task<UserProfile> GetUserProfileById(int userId);
        Task<bool> UpdateUserProfile(UserProfileViewModel objUserProfileViewModel);
        Task<bool> UserRegistration(UserRegisterViewModel objUserRegisterViewModel);
        Task<IEnumerable<RequestHistoryDetails>> GetRequestHistoryById(int userId);
        Task<IEnumerable<TrackServiceModel>> GetTrackServiceRequest(int userId);
        Task<PastTrackServiceModel> GetPastHistoyDeatilsCustomer(int userId);
    }
}
