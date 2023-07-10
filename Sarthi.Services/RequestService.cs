using Sarthi.Core.Interfaces;
using Sarthi.Core.Models;
using Sarthi.Services.Interfaces;

namespace Sarthi.Services
{
    public class RequestService : IRequestService
    {
        public IUnitOfWork _unitOfWork; 
        public RequestService(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
         }

        public async Task<IEnumerable<ResponseRequestModel>> SaveServiceRequests(RequestVendorModel objRequestVendorModel)
        { 
            return await  _unitOfWork.RequestRepository.SaveServiceRequests(objRequestVendorModel);
        }
        public async Task<bool> SaveRequestQuotations(RequestVendorModel objRequestVendorModel)
        {
            return await _unitOfWork.RequestRepository.SaveRequestQuotations(objRequestVendorModel);
        }
        public async Task<int> AcceptQuotationByCustomer(int customerId, int QuoationDetailedId)
        {
            return await _unitOfWork.RequestRepository.AcceptQuotationByCustomer(customerId, QuoationDetailedId);
        }
        public async Task<IEnumerable<CustomerRequestModel>> GetCustomerServiceRequest(int customerId)
        {
            return await _unitOfWork.RequestRepository.GetCustomerServiceRequest(customerId);
        }
        public async Task<IEnumerable<CustomerRequestServiceModel>> GetActiveCustomerServiceRequest(int customerId)
        {
            return await _unitOfWork.RequestRepository.GetActiveCustomerServiceRequest(customerId);
        }
        public async Task<bool> CheckActiveRequestByCustomer(int customerId)
        {
            return await _unitOfWork.RequestRepository.CheckActiveRequestByCustomer(customerId);
        }
        public async Task<CustomerRequestStatusModel> GetCurrentStatusByCustomer(int customerId)
        {
            return await _unitOfWork.RequestRepository.GetCurrentStatusByCustomer(customerId);
        }
        public async Task<int> RejectServiceRequestByCustomer(int customerId, int requestId)
        {
            return await _unitOfWork.RequestRepository.RejectServiceRequestByCustomer(customerId, requestId);
        }
    }
}
