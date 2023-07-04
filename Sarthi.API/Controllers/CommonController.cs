using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sarthi.Core.Models;
using Sarthi.Core.ViewModels;
using Sarthi.Services.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Xml.Linq;

namespace Sarthi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ILogger<CommonController> _logger;
        public readonly ICommonService _commonService;

        public CommonController(ILogger<CommonController> logger, ICommonService commonService)
        {
            _logger = logger;
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objUserViewModel"></param>
        /// <returns></returns>
        [HttpPost("UserAuthentication")]
        public async Task<IActionResult> UserAuthentication(UserViewModel objUserViewModel)
        {
            var model = new ResultList<UserAuth>();

            try
            {
                if (!string.IsNullOrEmpty(objUserViewModel.emailAddress) || !string.IsNullOrEmpty(objUserViewModel.password))
                {
                    var objUserAuth = await _commonService.GetUserAuthentication(objUserViewModel.emailAddress, objUserViewModel.password);
                    if (objUserAuth.Count() > 0)
                    {
                        model = new ResultList<UserAuth>
                        {
                            Status = 1,
                            Count = objUserAuth.Count(),
                            Message = "User successfully authenticated",
                            Data = objUserAuth
                        };
                        return Ok(model);
                    }
                }

                model = new ResultList<UserAuth>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Invalid Username/Password",
                    Data = null
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

        [HttpGet("GetUserProfileById")]
        public async Task<IActionResult> GetUserProfileById(int userId)
        {
            var model = new Result<UserProfile>();

            try
            {
                if (userId > 0)
                {
                    var objUserAuth = await _commonService.GetUserProfileById(userId);
                    if (objUserAuth != null)
                    {
                        model = new Result<UserProfile>
                        {
                            Status = 1,
                            Count = 1,
                            Message = "User details fecthed successfully.",
                            Data = objUserAuth
                        };
                        return Ok(model);
                    }
                    model = new Result<UserProfile>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "No user found",
                        Data = null
                    };
                }
                else
                {
                    model = new Result<UserProfile>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "Invalid Parameters",
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                model = new Result<UserProfile>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Something went wrong.",
                    Data = null
                };
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

        [HttpPost("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile(UserProfileViewModel objUserViewModel)
        {
            var model = new Result<bool>();

            try
            {
                if (objUserViewModel.UserId > 0)
                {
                    var objUserAuth = await _commonService.UpdateUserProfile(objUserViewModel);
                    if (objUserAuth)
                    {
                        model = new Result<bool>
                        {
                            Status = 1,
                            Count = 1,
                            Message = "User profile successfully updated.",
                            Data = objUserAuth
                        };
                        return Ok(model);
                    }
                }

                model = new Result<bool>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Invalid Username/Password",
                    Data = false
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

        [HttpPost("UserRegistration")]
        public async Task<IActionResult> UserRegistration(UserRegisterViewModel objUserRegisterViewModel)
        {
            var model = new Result<bool>();

            try
            {
                var objUserAuth = await _commonService.UserRegistration(objUserRegisterViewModel);
                if (objUserAuth)
                {
                    model = new Result<bool>
                    {
                        Status = 1,
                        Count = 1,
                        Message = "User successfully registered.",
                        Data = objUserAuth
                    };
                }
                else
                {
                    model = new Result<bool>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "User already exists.",
                        Data = objUserAuth
                    };

                }
                return Ok(model);

            }
            catch (Exception ex)
            {
                model = new Result<bool>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Somthing worng happened.",
                    Data = false
                };
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

        [HttpGet("GetRequestHistoryById")]
        public async Task<IActionResult> GetRequestHistoryById(int userId)
        {
            var model = new ResultList<RequestHistoryDetails>();
            List<RequestHistoryDetails> lstRequestHistory = new List<RequestHistoryDetails>();

            try
            {
                if (userId > 0)
                {
                    var objRequestHistory= await _commonService.GetRequestHistoryById(userId);
                    if (objRequestHistory.Count() > 0)
                    {
                        model = new ResultList<RequestHistoryDetails>
                        {
                            Status = 1,
                            Count = objRequestHistory.Count(),
                            Message = "User details fecthed successfully.",
                            Data = objRequestHistory
                        };
                        return Ok(model);
                    }
                    model = new ResultList<RequestHistoryDetails>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "No Records Found.",
                        Data = null
                    };
                }
                else
                {
                    model = new ResultList<RequestHistoryDetails>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "Invalid Parameters",
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                model = new ResultList<RequestHistoryDetails>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Something went wrong.",
                    Data = null
                };
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

        [HttpGet("GetTrackServiceRequest")]
        public async Task<IActionResult> GetTrackServiceRequest(int userId)
        {
            var model = new ResultList<TrackServiceModel>();
            List<TrackServiceModel> lstTrackServiceModel = new List<TrackServiceModel>();

            try
            {
                if (userId > 0)
                {
                    var objRequestHistory = await _commonService.GetTrackServiceRequest(userId);
                    if (objRequestHistory.Count() > 0)
                    {
                        model = new ResultList<TrackServiceModel>
                        {
                            Status = 1,
                            Count = objRequestHistory.Count(),
                            Message = "Track request successfully.",
                            Data = objRequestHistory
                        };
                        return Ok(model);
                    }
                    model = new ResultList<TrackServiceModel>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "No Records Found.",
                        Data = null
                    };
                }
                else
                {
                    model = new ResultList<TrackServiceModel>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "Invalid Parameters",
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                model = new ResultList<TrackServiceModel>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Something went wrong.",
                    Data = null
                };
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

    }
}
