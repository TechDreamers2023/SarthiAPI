using Microsoft.Data.SqlClient;
using Sarthi.Core.Interfaces;
using Sarthi.Core.Models;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Sarthi.Infrastructure.Repositories
{
    public class CommonRepository :  ICommonRepository
    { 
        private readonly IConfiguration _configuration;
        public CommonRepository(IConfiguration configuration)  
        { 
            _configuration = configuration;
        }

        public async Task<IEnumerable<UserAuth>> UserAuthentication(string emailAddress, string password)
        {
            IEnumerable<UserAuth> objUserAuth = null;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                var parameters = new { Email = emailAddress , Password  = password };
                objUserAuth = await connection.QueryAsync<UserAuth>("Sp_UserAuthentication", parameters, commandType: CommandType.StoredProcedure);
            }
            return objUserAuth;
        }

        public async Task<IEnumerable<RequestVendorDetailsModel>> FindAvailableServiceProviders(string pickUpCityName, string dropOffCityName, decimal distanceKM)
        {
            IEnumerable<RequestVendorDetailsModel> objRequestVendorModel = null;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                var parameters = new { PickUpCityName = pickUpCityName, DropOffCityName = dropOffCityName, DistanceKM = distanceKM };
                objRequestVendorModel = await connection.QueryAsync<RequestVendorDetailsModel>("SP_FindAvailableServiceProviders", parameters, commandType: CommandType.StoredProcedure);
            }
            return objRequestVendorModel;
        }
    }
}
