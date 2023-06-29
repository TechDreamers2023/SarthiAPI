using Sarthi.Core.Models;
using Sarthi.Core.ViewModels;

namespace Sarthi.Core.Interfaces
{
    public interface IVendorRepository
    {
        Task<int> RejectQuotationByVendor(int vendorId, int QuoationDetailedId);
        Task<VendorShiftViewModel> UpdateVendorShifts(int vendorId);
        Task<int> AccpetQuotationByVendor(int vendorId, int QuoationDetailedId);
        Task<int> UpdateRequestStatus(int requestId, int userId, int stageId);
        Task<int> CheckVendorShiftStatus(int vendorId);
        Task<VendorRequestServiceModel> GetVendorActiveRequest(int vendorUserId);
        Task<bool> SaveVendorLocation(VendorLocationViewModel objVendorLocationViewModel);
    }
}
