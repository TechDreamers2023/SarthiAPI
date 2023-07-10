using Sarthi.Core.Interfaces;
using Sarthi.Core.Models;
using Sarthi.Core.ViewModels;
using Sarthi.Services.Interfaces;

namespace Sarthi.Services
{
    public class CommonService : ICommonService
    {
        public IUnitOfWork _unitOfWork; 
        public CommonService(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork; 
         }

        public async Task<IEnumerable<UserAuth>> GetUserAuthentication(string emailAddress, string password)
        { 
            return await  _unitOfWork.CommonRepository.UserAuthentication(emailAddress, password);
        }

        public async Task<IEnumerable<RequestVendorDetailsModel>> FindAvailableServiceProviders(string pickUpCityName, string dropOffCityName, decimal distanceKM)
        {
            return await _unitOfWork.CommonRepository.FindAvailableServiceProviders(pickUpCityName, dropOffCityName, distanceKM);
        }

        public async Task<UserProfile> GetUserProfileById(int userId)
        {
            return await _unitOfWork.CommonRepository.GetUserProfileById(userId);
        }

        public async Task<bool> UpdateUserProfile(UserProfileViewModel objUserProfileViewModel)
        {
            return await _unitOfWork.CommonRepository.UpdateUserProfile(objUserProfileViewModel);
        }

        public async Task<bool> UserRegistration(UserRegisterViewModel objUserRegisterViewModel)
        {
            return await _unitOfWork.CommonRepository.UserRegistration(objUserRegisterViewModel);
        }
        public async Task<IEnumerable<RequestHistoryDetails>> GetRequestHistoryById(int userId)
        {
            return await _unitOfWork.CommonRepository.GetRequestHistoryById(userId);
        }
        public async Task<IEnumerable<TrackServiceModel>> GetTrackServiceRequest(int userId)
        {
            return await _unitOfWork.CommonRepository.GetTrackServiceRequest(userId);
        }
        public async Task<PastTrackServiceModel> GetPastHistoyDeatilsCustomer(int userId)
        {
            return await _unitOfWork.CommonRepository.GetPastHistoyDeatilsCustomer(userId);
        }
    }
}
