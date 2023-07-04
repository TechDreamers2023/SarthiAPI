using Microsoft.Data.SqlClient;
using Sarthi.Core.Interfaces;
using Sarthi.Core.Models;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Sarthi.Core.ViewModels;

namespace Sarthi.Infrastructure.Repositories
{
    public class CommonRepository : ICommonRepository
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
                var parameters = new { Email = emailAddress, Password = password };
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

        public async Task<UserProfile> GetUserProfileById(int userId)
        {
            UserProfile objUserProfile = new UserProfile();

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                var parameters = new { UserId = userId };
                objUserProfile = await connection.QuerySingleOrDefaultAsync<UserProfile>("Sp_GetUserProfileByID", parameters, commandType: CommandType.StoredProcedure);
            }
            return objUserProfile;
        }

        public async Task<bool> UpdateUserProfile(UserProfileViewModel objUserProfileViewModel)
        {
            bool isSuccess = false;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                var parameters = new
                {
                    UserId = objUserProfileViewModel.UserId,
                    FirstName = objUserProfileViewModel.FirstName,
                    LastName = objUserProfileViewModel.LastName,
                    Email = objUserProfileViewModel.Email,
                    Password = objUserProfileViewModel.Password,
                    VehicleNumber = objUserProfileViewModel.VehicleNumber,
                    ContactNo = objUserProfileViewModel.ContactNo
                };
                isSuccess = await connection.QuerySingleAsync<bool>("Sp_GetUpdateUserProfile", parameters, commandType: CommandType.StoredProcedure);
            }
            return isSuccess;
        }

        public async Task<bool> UserRegistration(UserRegisterViewModel objUserRegisterViewModel)
        {
            bool isSuccess = false;

            using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
            {
                var parameters = new
                {
                    FirstName = objUserRegisterViewModel.FirstName,
                    LastName = objUserRegisterViewModel.LastName,
                    Email = objUserRegisterViewModel.Email,
                    Password = objUserRegisterViewModel.Password,
                    VehicleNumber = objUserRegisterViewModel.VehicleNumber,
                    ContactNo = objUserRegisterViewModel.ContactNo
                };
                isSuccess = await connection.QuerySingleAsync<bool>("Sp_UserRegistrationProfile", parameters, commandType: CommandType.StoredProcedure);
            }
            return isSuccess;
        }

        public async Task<IEnumerable<RequestHistoryDetails>> GetRequestHistoryById(int userId)
        {
            IEnumerable<RequestHistoryDetails> lstRequestHistory = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { UserId = userId };
                    lstRequestHistory = await connection.QueryAsync<RequestHistoryDetails>("SP_GetPastServiceRequests", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstRequestHistory;
        }

        public async Task<IEnumerable<TrackServiceModel>> GetTrackServiceRequest(int userId)
        {
            IEnumerable<TrackServiceModel> lstTrackServiceModel = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(this._configuration.GetConnectionString("DefaultConnection")))
                {
                    var parameters = new { UserId = userId };
                    lstTrackServiceModel = await connection.QueryAsync<TrackServiceModel>("SP_TrackServiceRequest", parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lstTrackServiceModel;
        }


    }
}
