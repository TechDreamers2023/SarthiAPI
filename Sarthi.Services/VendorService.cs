using Sarthi.Core.Interfaces;
using Sarthi.Core.Models;
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
        public async Task<int> UpdateVendorShifts(int vendorId)
        {
            return await _unitOfWork.VendorRepository.UpdateVendorShifts(vendorId);
        }
        public async Task<int> AccpetQuotationByVendor(int vendorId, int QuoationDetailedId)
        {
            return await _unitOfWork.VendorRepository.AccpetQuotationByVendor(vendorId, QuoationDetailedId);
        }
        public async Task<int> UpdateRequestStatus(int requestId, int userId, int stageId)
        {
            return await _unitOfWork.VendorRepository.UpdateRequestStatus(requestId, userId, stageId);
        }
    }
}
