﻿using Sarthi.Core.Models;

namespace Sarthi.Core.Interfaces
{
    public interface IRequestRepository
    {
        Task<IEnumerable<ResponseRequestModel>> SaveServiceRequests(RequestVendorModel objRequestVendorModel);
        Task<bool> SaveRequestQuotations(RequestVendorModel objRequestVendorModel);
        Task<int> AcceptQuotationByCustomer(int customerId, int QuoationDetailedId);
        Task<IEnumerable<CustomerRequestModel>> GetCustomerServiceRequest(int customerId);
        Task<IEnumerable<CustomerRequestServiceModel>> GetActiveCustomerServiceRequest(int customerId);
        Task<bool> CheckActiveRequestByCustomer(int customerId);
        Task<CustomerRequestStatusModel> GetCurrentStatusByCustomer(int customerId);
        Task<int> RejectServiceRequestByCustomer(int customerId, int requestId);
    }
}
