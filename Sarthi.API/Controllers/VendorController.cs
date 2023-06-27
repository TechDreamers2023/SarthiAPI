using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sarthi.Core.Models;
using Sarthi.Services.Interfaces;
using System.Net;
using System.Net.Mail;
using System.Xml.Linq;

namespace Sarthi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly ILogger<CommonController> _logger;
        public readonly ICommonService _commonService;
        public readonly IVendorService _vendorService;

        public VendorController(ILogger<CommonController> logger, ICommonService commonService, IVendorService vendorService)
        {
            _logger = logger;
            _commonService = commonService;
            _vendorService = vendorService;
        }
          
        [HttpPost("RejectQuotationByVendor")]
        public async Task<IActionResult> RejectQuotationByVendor(int vendorId,int quoationDetailedId)
        {
            var model = new ResultList<bool>();
            try
            {
                if (vendorId > 0 || quoationDetailedId > 0)
                {
                    int status = _vendorService.RejectQuotationByVendor(vendorId, quoationDetailedId).Result;
                    if (status == 0)
                    {
                        model = new ResultList<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Something wrong happened.",
                            Data = null
                        };
                    }
                    else if (status == 1)
                    {
                        model = new ResultList<bool>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Quatations Request Has Been Rejected Successfully.",
                            Data = null
                        };
                    }
                    else if (status == 2)
                    {
                        model = new ResultList<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Invalid Request.",
                            Data = null
                        };
                    }
                }
                else
                {
                    model = new ResultList<bool>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "Invalid Request",
                        Data = null
                    };
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

        [HttpPost("ManageVendorShifts")]
        public async Task<IActionResult> ManageVendorShifts(int vendorId)
        {
            var model = new ResultList<bool>();
            try
            {
                if (vendorId > 0)
                {
                    int status = _vendorService.UpdateVendorShifts(vendorId).Result;
                    if (status == 0)
                    {
                        model = new ResultList<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Something wrong happened.",
                            Data = null
                        };
                    }
                    else if (status == 1)
                    {
                        model = new ResultList<bool>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Shift has been started successfully.",
                            Data = null
                        };
                    }
                    else if (status == 2)
                    {
                        model = new ResultList<bool>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Shift has been ended successfully.",
                            Data = null
                        };
                    }
                    else if (status == 3)
                    {
                        model = new ResultList<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Active request is in process. Hence can't end the shift.",
                            Data = null
                        };
                    }
                }
                else
                {
                    model = new ResultList<bool>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "Invalid Request",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

        [HttpPost("AccpetQuotationByVendor")]
        public async Task<IActionResult> AccpetQuotationByVendor(int vendorId, int quoationDetailedId)
        {
            var model = new ResultList<bool>();
            try
            {
                if (vendorId > 0 || quoationDetailedId > 0)
                {
                    int status = _vendorService.AccpetQuotationByVendor(vendorId, quoationDetailedId).Result;
                    if (status == 0)
                    {
                        model = new ResultList<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Something wrong happened.",
                            Data = null
                        };
                    }
                    else if (status == 1)
                    {
                        model = new ResultList<bool>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Quotations Request Has Been Accepted Successfully.",
                            Data = null
                        };
                    }
                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction failed");
            }

            model = new ResultList<bool>
            {
                Status = 2,
                Count = 0,
                Message = "Invalid Request",
                Data = null
            };

            return Ok(model);
        }

        [HttpPost("UpdateRequestStatus")]
        public async Task<IActionResult> UpdateRequestStatus(int requestId, int userId, int stageId)
        {
            var model = new ResultList<bool>();
            try
            {
                if (requestId > 0 || userId > 0 || stageId > 0)
                {
                    int status = _vendorService.UpdateRequestStatus(requestId, userId, stageId).Result;
                    if (status == 0)
                    {
                        model = new ResultList<bool>
                        {
                            Status = 0,
                            Count = 0,
                            Message = "Something wrong happened.",
                            Data = null
                        };
                    }
                    else if (status == 1)
                    {
                        model = new ResultList<bool>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Request status has been updated successfully.",
                            Data = null
                        };
                    }
                 }
            }
            catch (Exception ex)
            {
                model = new ResultList<bool>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Something wrong happened.",
                    Data = null
                };
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

    }
}
