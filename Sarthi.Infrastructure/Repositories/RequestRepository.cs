using Microsoft.Data.SqlClient;
using Sarthi.Core.Interfaces;
using Sarthi.Core.Models;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Collections.Generic;

namespace Sarthi.Infrastructure.Repositories
{
    public class RequestRepository : IRequestRepository
    { 
        private readonly IConfiguration _configuration;

        public RequestRepository(IConfiguration configuration)  
        { 
            _configuration = configuration;
        }

        public async Task<IEnumerable<ResponseRequestModel>> SaveServiceRequests(RequestVendorModel objRequestVendorModel)
        {
            IEnumerable<ResponseRequestModel> objResponseRequestModel = null;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            { 
                try
                {
                    var parameters = new {
                        CurrentLocation = objRequestVendorModel.CurrentLocation.Address,
                        CurrentLatitude = objRequestVendorModel.CurrentLocation.Latitude,
                        CurrentLongitude  = objRequestVendorModel.CurrentLocation.Longitude,
                        CurrentCity = objRequestVendorModel.CurrentLocation.City,
                        PickUpLocation = objRequestVendorModel.PickupLocation.Address,
                        PickupLatitude = objRequestVendorModel.PickupLocation.Latitude,
                        PickupLongitude = objRequestVendorModel.PickupLocation.Longitude,
                        PickupCity = objRequestVendorModel.PickupLocation.City,
                        DropOffLocation = objRequestVendorModel.DropOffLocation.Address,
                        DropOffLatitude = objRequestVendorModel.DropOffLocation.Latitude,
                        DropOffLongitude = objRequestVendorModel.DropOffLocation.Longitude,
                        DropOffCity = objRequestVendorModel.DropOffLocation.City,
                        TotalDistanceKM = objRequestVendorModel.DistanceKM,
                        DurationInMins = objRequestVendorModel.DurationInMins,
                        CustomerId = objRequestVendorModel.UserId
                    };

                    objResponseRequestModel =  await connection.QueryAsync<ResponseRequestModel>("Sp_SaveRequestService", parameters, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                { 
                    throw ex;
                }
            }
            return objResponseRequestModel;
        }
        public async Task<bool> SaveRequestQuotations(RequestVendorModel objRequestVendorModel)
        {
            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                { 
                    foreach (var item in objRequestVendorModel.VendorDetails)
                    { 
                        var parameters = new
                        {
                            RequestId = objRequestVendorModel.RequestId,
                            Amount = item.TotalAmount,
                            ServiceTax = 10, 
                            VendorUserId = item.VendorId,
                            ActionBy = objRequestVendorModel.UserId
                        };

                        var rtnStatus = await connection.ExecuteAsync("Sp_SaveRequestServiceQuotations", parameters, commandType: CommandType.StoredProcedure);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    
                }
            }
            return false;
        }
        public async Task<int> AcceptQuotationByCustomer(int customerId, int QuoationDetailedId)
        {
            int rtnStatus = 0;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                        var parameters = new
                        {
                            QuoationDetailId = QuoationDetailedId, 
                            UserId = customerId
                        };

                       rtnStatus = await connection.ExecuteAsync("Sp_AcceptQuotationByCustomer", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    rtnStatus = 0;
                }
            }
            return rtnStatus;
        }
        public async Task<IEnumerable<CustomerRequestModel>> GetCustomerServiceRequest(int customerId)
        {
            IEnumerable<CustomerRequestModel> objResponseRequestModel = null;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    { 
                        CustomerId = customerId
                    };

                    objResponseRequestModel = await connection.QueryAsync<CustomerRequestModel>("SP_GetAllQuotationsCustomer", parameters, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return objResponseRequestModel;
        }
        public async Task<IEnumerable<CustomerRequestServiceModel>> GetActiveCustomerServiceRequest(int customerId)
        {
            IEnumerable<CustomerRequestServiceModel> objResponseRequestModel = null;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    {
                        CustomerId = customerId
                    };

                    objResponseRequestModel = await connection.QueryAsync<CustomerRequestServiceModel>("SP_GetActiveCustomerRequest", parameters, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return objResponseRequestModel;
        }
        public async Task<int> CheckActiveRequestByCustomer(int customerId)
        {
            int rtnStatus = 0;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    { 
                        CustomerId = customerId
                    };

                    rtnStatus = await connection.ExecuteAsync("Sp_CheckActiveRequestByCustomer", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    rtnStatus = 0;
                }
            }
            return rtnStatus;
        }
        public async Task<IEnumerable<CustomerRequestStatusModel>> GetCurrentStatusByCustomer(int customerId)
        {
            IEnumerable<CustomerRequestStatusModel> objResponseRequestModel = null;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    {
                        CustomerId = customerId
                    };

                    objResponseRequestModel = await connection.QueryAsync<CustomerRequestStatusModel>("Sp_GetCurrentStatusByCustomer", parameters, commandType: CommandType.StoredProcedure);

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return objResponseRequestModel;
        }
    }
}
