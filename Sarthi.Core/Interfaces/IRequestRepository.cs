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
        Task<int> CheckActiveRequestByCustomer(int customerId);
        Task<IEnumerable<CustomerRequestStatusModel>> GetCurrentStatusByCustomer(int customerId);
    }
}
