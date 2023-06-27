using Sarthi.Core.Models;

namespace Sarthi.Services.Interfaces
{
    public interface IVendorService
    { 
        Task<int> RejectQuotationByVendor(int vendorId, int QuoationDetailedId);
        Task<int> UpdateVendorShifts(int vendorId);
        Task<int> AccpetQuotationByVendor(int vendorId, int QuoationDetailedId);
        Task<int> UpdateRequestStatus(int requestId, int userId, int stageId);
    }
}