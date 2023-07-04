using Microsoft.Data.SqlClient;
using Sarthi.Core.Interfaces;
using Sarthi.Core.Models;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;
using System.Collections.Generic;
using Sarthi.Core.ViewModels;

namespace Sarthi.Infrastructure.Repositories
{
    public class VendorRepository : IVendorRepository
    { 
        private readonly IConfiguration _configuration;
        public VendorRepository(IConfiguration configuration)  
        { 
            _configuration = configuration;
        }
        public async Task<int> RejectQuotationByVendor(int vendorId, int QuoationDetailedId)
        {
            int rtnStatus = 0;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                        var parameters = new
                        {
                            QuoationDetailId = QuoationDetailedId,
                            VendorId = vendorId
                        };

                       rtnStatus = await connection.QuerySingleAsync<int>("Sp_RejectQuotationByVendor", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    rtnStatus = 0;
                }
            }
            return rtnStatus;
        }
        public async Task<VendorShiftViewModel> UpdateVendorShifts(int vendorId)
        {
            VendorShiftViewModel objVendorShiftViewModel = new VendorShiftViewModel();

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    {
                        VendorUserId = vendorId
                    };

                    objVendorShiftViewModel = await connection.QuerySingleAsync<VendorShiftViewModel>("SP_UpdateShiftByUserId", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    objVendorShiftViewModel.Status = 0;
                    objVendorShiftViewModel.ShiftId = null;
                }
            }
            return objVendorShiftViewModel;
        }
        public async Task<int> AccpetQuotationByVendor(int vendorId, int QuoationDetailedId)
        {
            int rtnStatus = 0;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    {
                        QuoationDetailId = QuoationDetailedId,
                        VendorId = vendorId
                    };

                    rtnStatus = await connection.QuerySingleAsync<int>("Sp_AcceptQuotationByVendor", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    rtnStatus = 0;
                }
            }
            return rtnStatus;
        }
        public async Task<bool> UpdateRequestStatus(int requestId, int userId, int stageId)
        {
            bool rtnStatus = false;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    {
                        RequestId = requestId,
                        UserId = userId,
                        StageId = stageId
                    };

                    rtnStatus = await connection.QuerySingleAsync<bool>("SP_UpdateRequestStatus", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    rtnStatus = false;
                }
            }
            return rtnStatus;
        }
        public async Task<int> CheckVendorShiftStatus(int vendorId)
        {
            int rtnStatus = 0;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    {
                        VendorUserId = vendorId
                    };

                    rtnStatus = await connection.QuerySingleAsync<int>("SP_CheckVendorShiftStatus", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    rtnStatus = 0;
                }
            }
            return rtnStatus;
        }
        public async Task<VendorRequestServiceModel> GetVendorActiveRequest(int vendorUserId)
        {
            VendorRequestServiceModel objVendorRequestServiceModel = new VendorRequestServiceModel();

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    {
                        VendorId = vendorUserId 
                    };
                    objVendorRequestServiceModel = await connection.QuerySingleOrDefaultAsync<VendorRequestServiceModel>("SP_GetActiveRequestByVendor", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return objVendorRequestServiceModel;
        }
        public async Task<bool> SaveVendorLocation(VendorLocationViewModel objVendorLocationViewModel)
        {
            bool rtnStatus = false;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    {
                        VendorId = objVendorLocationViewModel.vendorId,
                        CurrentLatitude = objVendorLocationViewModel.CurrentLatitude,
                        CurrentLongitude = objVendorLocationViewModel.CurrentLongitude,
                        ShiftId = objVendorLocationViewModel.ShiftId,
                        RequestId = objVendorLocationViewModel.RequestId
                    };

                    rtnStatus = await connection.QuerySingleAsync<bool>("SP_SaveVendorLocations", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    rtnStatus = false;
                }
            }
            return rtnStatus;
        }
    }
}
