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

                       rtnStatus = await connection.ExecuteAsync("Sp_RejectQuotationByVendor", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    rtnStatus = 0;
                }
            }
            return rtnStatus;
        }
        public async Task<int> UpdateVendorShifts(int vendorId)
        {
            int rtnStatus = 0;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                try
                {
                    var parameters = new
                    { 
                        VendorId = vendorId
                    };

                    rtnStatus = await connection.ExecuteAsync("SP_UpdateShiftByUserId", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    rtnStatus = 0;
                }
            }
            return rtnStatus;
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

                    rtnStatus = await connection.ExecuteAsync("Sp_AcceptQuotationByVendor", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    rtnStatus = 0;
                }
            }
            return rtnStatus;
        }
        public async Task<int> UpdateRequestStatus(int requestId, int userId, int stageId)
        {
            int rtnStatus = 0;

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

                    rtnStatus = await connection.ExecuteAsync("SP_UpdateRequestStatus", parameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception ex)
                {
                    rtnStatus = 0;
                }
            }
            return rtnStatus;
        }
    }
}
