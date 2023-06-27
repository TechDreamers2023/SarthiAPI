﻿using Microsoft.AspNetCore.Mvc;
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

    }
}
