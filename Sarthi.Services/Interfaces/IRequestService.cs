using Sarthi.Core.Models;

namespace Sarthi.Services.Interfaces
{
    public interface IRequestService
    {
        Task<IEnumerable<ResponseRequestModel>> SaveServiceRequests(RequestVendorModel objRequestVendorModel);
        Task<bool> SaveRequestQuotations(RequestVendorModel objRequestVendorModel);
        Task<int> AcceptQuotationByCustomer(int customerId, int QuoationDetailedId);
        Task<IEnumerable<CustomerRequestModel>> GetCustomerServiceRequest(int customerId);
        Task<IEnumerable<CustomerRequestServiceModel>> GetActiveCustomerServiceRequest(int customerId);
        Task<int> CheckActiveRequestByCustomer(int customerId);

    }
}