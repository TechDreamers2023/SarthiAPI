using Sarthi.Core.Interfaces;
using Sarthi.Core.Models;
using Sarthi.Core.ViewModels;
using Sarthi.Services.Interfaces;

namespace Sarthi.Services
{
    public class VendorService : IVendorService
    {
        public IUnitOfWork _unitOfWork; 
        public VendorService(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
         }

        public async Task<int> RejectQuotationByVendor(int vendorId, int QuoationDetailedId)
        {
            return await _unitOfWork.VendorRepository.RejectQuotationByVendor(vendorId, QuoationDetailedId);
        }
        public async Task<VendorShiftViewModel> UpdateVendorShifts(int vendorId)
        {
            return await _unitOfWork.VendorRepository.UpdateVendorShifts(vendorId);
        }
        public async Task<int> AccpetQuotationByVendor(int vendorId, int QuoationDetailedId)
        {
            return await _unitOfWork.VendorRepository.AccpetQuotationByVendor(vendorId, QuoationDetailedId);
        }
        public async Task<bool> UpdateRequestStatus(int requestId, int userId, int stageId)
        {
            return await _unitOfWork.VendorRepository.UpdateRequestStatus(requestId, userId, stageId);
        }
        public async Task<int> CheckVendorShiftStatus(int vendorId)
        {
            return await _unitOfWork.VendorRepository.CheckVendorShiftStatus(vendorId);
        }
        public async Task<VendorRequestServiceModel> GetVendorActiveRequest(int vendorUserId)
        {
            return await _unitOfWork.VendorRepository.GetVendorActiveRequest(vendorUserId);
        }
        public async Task<bool> SaveVendorLocation(VendorLocationViewModel objVendorLocationViewModel)
        {
            return await _unitOfWork.VendorRepository.SaveVendorLocation(objVendorLocationViewModel);
        }
    }
}
