using Sarthi.Core.Interfaces;
using Sarthi.Core.Models;
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

    }
}
