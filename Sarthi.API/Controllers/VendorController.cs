using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sarthi.Core.Models;
using Sarthi.Core.ViewModels;
using Sarthi.Services;
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
        public async Task<IActionResult> RejectQuotationByVendor(VendorRequestViewModel objVendorRequestViewModel)
        {
            var model = new Result<bool>();
            try
            {
                if (objVendorRequestViewModel.vendorId > 0 || objVendorRequestViewModel.quoationDetailedId > 0)
                {
                    int status = _vendorService.RejectQuotationByVendor(objVendorRequestViewModel.vendorId,
                        objVendorRequestViewModel.quoationDetailedId).Result;

                    if (status == 0)
                    {
                        model = new Result<bool>
                        {
                            Status = 0,
                            Count = 0,
                            Message = "Something wrong happened.",
                            Data = false
                        };
                    }
                    else if (status == 1)
                    {
                        model = new Result<bool>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Quatations Request Has Been Rejected Successfully.",
                            Data = true
                        };
                    }
                    else if (status == 2)
                    {
                        model = new Result<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Quatations Request Has Been not found.",
                            Data = false
                        };
                    }
                }
                else
                {
                    model = new Result<bool>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "Please provide the all required inputs.",
                        Data = false
                    };
                }
            }   
            catch (Exception ex) {
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

        [HttpGet("ManageVendorShifts")]
        public async Task<IActionResult> ManageVendorShifts(int vendorId)
        {
            var model = new Result<int?>();
            try
            {
                if (vendorId > 0)
                {
                    VendorShiftViewModel objVendorShiftViewModel = await _vendorService.UpdateVendorShifts(vendorId);
                    if (objVendorShiftViewModel == null)
                    {
                        model = new Result<int?>
                        {
                            Status = 0,
                            Count = 0,
                            Message = "Something wrong happened.",
                            Data = null
                        };
                    }
                    else if (objVendorShiftViewModel.Status == 1)
                    {
                        model = new Result<int?>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Shift has been started successfully.",
                            Data = objVendorShiftViewModel.ShiftId
                        };
                    }
                    else if (objVendorShiftViewModel.Status == 2)
                    {
                        model = new Result<int?>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Shift has been ended successfully.",
                            Data = objVendorShiftViewModel.ShiftId
                        };
                    }
                    else if (objVendorShiftViewModel.Status == 3)
                    {
                        model = new Result<int?>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Active request is in process. Hence can't end the shift.",
                            Data = objVendorShiftViewModel.ShiftId
                        };
                    }
                }
                else
                {
                    model = new Result<int?>
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
        public async Task<IActionResult> AccpetQuotationByVendor(VendorRequestViewModel objVendorRequestViewModel)
        {
            var model = new Result<bool>();
            try
            {
                if (objVendorRequestViewModel.vendorId > 0 || objVendorRequestViewModel.quoationDetailedId > 0)
                {
                    int status = _vendorService.AccpetQuotationByVendor(objVendorRequestViewModel.vendorId, objVendorRequestViewModel.quoationDetailedId).Result;
                    if (status == 0)
                    {
                        model = new Result<bool>
                        {
                            Status = 0,
                            Count = 0,
                            Message = "Something wrong happened.",
                            Data = false
                        };
                    }
                    else if (status == 1)
                    {
                        model = new Result<bool>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Quotations Request Has Been Accepted Successfully.",
                            Data = true
                        };
                    }
                    return Ok(model);
                }
            }
            catch (Exception ex)
            {
                model = new Result<bool>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Something wrong happened.",
                    Data = false
                };
                _logger.LogError(ex, "Transaction failed");
            }

            model = new Result<bool>
            {
                Status = 2,
                Count = 0,
                Message = "Invalid Request",
                Data = false
            };

            return Ok(model);
        }

        [HttpPost("UpdateRequestStatus")]
        public async Task<IActionResult> UpdateRequestStatus(VendorUpdateRequestViewModel objVendorUpdateRequestViewModel)
        {
            var model = new Result<bool>();
            try
            {
                if (objVendorUpdateRequestViewModel.requestId > 0 || objVendorUpdateRequestViewModel.userId > 0 || 
                    objVendorUpdateRequestViewModel.stageId > 0)
                {
                    bool IsScuccess = _vendorService.UpdateRequestStatus(objVendorUpdateRequestViewModel.requestId, objVendorUpdateRequestViewModel.userId,
                        objVendorUpdateRequestViewModel.stageId).Result;

                    if (!IsScuccess)
                    {
                        model = new Result<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Invalid service request.",
                            Data = false
                        };
                    }
                    else if (IsScuccess)
                    {
                        model = new Result<bool>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Request status has been updated successfully.",
                            Data = true
                        };
                    }
                 }
            }
            catch (Exception ex)
            {
                model = new Result<bool>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Something wrong happened.",
                    Data = false
                };
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

        [HttpGet("CheckVendorShiftStatus")]
        public async Task<IActionResult> CheckVendorShiftStatus(int vendorId)
        {
            var model = new Result<int?>();
            try
            {
                if (vendorId > 0)
                {
                    int status = await _vendorService.CheckVendorShiftStatus(vendorId);
                    if (status > 0)
                    {
                        model = new Result<int?>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Shift is active.",
                            Data = status
                        };
                    }
                    else
                    {
                        model = new Result<int?>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Shift is closed.",
                            Data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                model = new Result<int?>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Error occured, Please try again later",
                    Data = null
                };
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

        [HttpGet("GetVendorActiveRequest")]
        public async Task<IActionResult> GetVendorActiveRequest(int vendorUserId)
        {
            VendorRequestServiceModel objRequestVendorModel = new VendorRequestServiceModel();
            var model = new Result<VendorRequestServiceModel>();

            try
            {
                if (vendorUserId > 0)
                {
                    objRequestVendorModel = await _vendorService.GetVendorActiveRequest(vendorUserId);

                    if (objRequestVendorModel == null)
                    {
                        model = new Result<VendorRequestServiceModel>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Active request not found.",
                            Data = null
                        };
                    }
                    else if (objRequestVendorModel != null)
                    {
                        model = new Result<VendorRequestServiceModel>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Active request found.",
                            Data = objRequestVendorModel
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                model = new Result<VendorRequestServiceModel>
                {
                    Status = 0,
                    Count = 0,
                    Message = ex.Message,
                    Data = null
                };
            }

            return Ok(model);
        }

        [HttpPost("SaveVendorLocations")]
        public async Task<IActionResult> SaveVendorLocations(VendorLocationViewModel objVendorLocationViewModel)
        {
            var model = new Result<bool>();
            try
            {
                if (objVendorLocationViewModel.vendorId > 0 && objVendorLocationViewModel.ShiftId > 0)
                {
                    bool status = await _vendorService.SaveVendorLocation(objVendorLocationViewModel);
                    if (!status)
                    {
                        model = new Result<bool>
                        {
                            Status = 0,
                            Count = 0,
                            Message = "Error occured, Please try again later.",
                            Data = false
                        };
                    }
                    else if (status)
                    {
                        model = new Result<bool>
                        {
                            Status = 1,
                            Count = 0,
                            Message = "Location Added Successfully.",
                            Data = true
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                model = new Result<bool>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Error occured, Please try again later",
                    Data = false
                };
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }
    }
}
