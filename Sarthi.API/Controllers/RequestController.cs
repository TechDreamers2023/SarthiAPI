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
    public class RequestController : ControllerBase
    {
        private readonly ILogger<CommonController> _logger;
        public readonly ICommonService _commonService;
        public readonly IRequestService _requestService;

        public RequestController(ILogger<CommonController> logger, ICommonService commonService, IRequestService requestService)
        {
            _logger = logger;
            _commonService = commonService;
            _requestService = requestService;
        }

        [HttpPost("GenerateServiceRequest")]
        public async Task<IActionResult> GenerateServiceRequest(RequestViewModel objRequestViewModel)
        {

            ServiceRequestModel objServiceRequestModel = new ServiceRequestModel();
            var model = new Result<bool>();
            try
            {
                if (objRequestViewModel.currentlat > 0 && objRequestViewModel.currentlong > 0 && objRequestViewModel.pickuplat > 0 
                    && objRequestViewModel.pickuplong > 0 && objRequestViewModel.dropOfflat > 0 && 
                  objRequestViewModel.dropOfflong > 0 && objRequestViewModel.userId > 0)
                {

                    //Check if the request is active or not.
                    bool status = await _requestService.CheckActiveRequestByCustomer(objRequestViewModel.userId);
                    if (status)
                    {
                        model = new Result<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "You have already one active request.",
                            Data = false
                        };
                        return Ok(model);
                    }

                    if (objRequestViewModel.pickuplat == objRequestViewModel.dropOfflat 
                        && objRequestViewModel.pickuplong == objRequestViewModel.dropOfflong)
                    {
                        model = new Result<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Please select different pickup & dropoff locations.",
                            Data = false
                        };
                        return Ok(model);
                    }

                    objServiceRequestModel.CurrentLocation = GetAddressInfo(objRequestViewModel.currentlat, objRequestViewModel.currentlong);
                    objServiceRequestModel.PickupLocation = GetAddressInfo(objRequestViewModel.pickuplat, objRequestViewModel.pickuplong);
                    objServiceRequestModel.DropOffLocation = GetAddressInfo(objRequestViewModel.dropOfflat, objRequestViewModel.dropOfflong);

                    if (objServiceRequestModel.PickupLocation != null
                        && objServiceRequestModel.DropOffLocation != null)
                    {
                        if ((objServiceRequestModel.PickupLocation.City != objServiceRequestModel.DropOffLocation.City) || 
                            (string.IsNullOrEmpty(objServiceRequestModel.PickupLocation.City) ||
                            string.IsNullOrEmpty(objServiceRequestModel.DropOffLocation.City)))
                        {
                            model = new Result<bool>
                            {
                                Status = 2,
                                Count = 0,
                                Message = "Service not available in entered cities.",
                                Data = false
                            };
                            return Ok(model);
                        }
                    }

                    if (objServiceRequestModel.PickupLocation.Latitude != objServiceRequestModel.DropOffLocation.Latitude &&
                        objServiceRequestModel.PickupLocation.Longitude != objServiceRequestModel.DropOffLocation.Longitude)
                    {
                        GetDistanceByLatLong(ref objServiceRequestModel);
                    }
                    else
                    {
                        objServiceRequestModel.DistanceKM = 0;
                        objServiceRequestModel.DurationInMins = "0 min";
                    }

                    var objVendorDetails = await _commonService.FindAvailableServiceProviders(objServiceRequestModel.PickupLocation.City,
                        objServiceRequestModel.DropOffLocation.City, objServiceRequestModel.DistanceKM);

                    if (objVendorDetails.Count() > 0)
                    {
                        RequestVendorModel objRequestVendorModel = new RequestVendorModel();

                        objRequestVendorModel.DurationInMins = objServiceRequestModel.DurationInMins;
                        objRequestVendorModel.DistanceKM = objServiceRequestModel.DistanceKM;
                        objRequestVendorModel.CurrentLocation = objServiceRequestModel.CurrentLocation;
                        objRequestVendorModel.PickupLocation = objServiceRequestModel.PickupLocation;
                        objRequestVendorModel.DropOffLocation = objServiceRequestModel.DropOffLocation;
                        objRequestVendorModel.UserId = objRequestViewModel.userId;
                         
                        //save the records
                        var objServiceInfo = await _requestService.SaveServiceRequests(objRequestVendorModel);

                        if (objServiceInfo.Count() > 0)
                        {

                            //Save Quoatations
                            objRequestVendorModel.RequestId = objServiceInfo.SingleOrDefault().RequestId;
                            objRequestVendorModel.RequestNumber = objServiceInfo.SingleOrDefault().RequestNumber;
                            objRequestVendorModel.VendorDetails = objVendorDetails;
                            var IsSuccess = await _requestService.SaveRequestQuotations(objRequestVendorModel);

                            model = new Result<bool>
                            {
                                Status = 1,
                                Count = objVendorDetails.Count(),
                                Message = "Service Request has been created successfully.",
                                Data = true
                            };
                        }
                        else
                        {
                            model = new Result<bool>
                            {
                                Status = 0,
                                Count = objVendorDetails.Count(),
                                Message = "Something wrong happned, Please try again !",
                                Data = false
                            };
                        }
                        return Ok(model);
                    }
                    else
                    {
                        model = new Result<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "No Vendor Service Available.",
                            Data = false
                        };
                    }
                    return Ok(model);
                }
                else
                {
                    model = new Result<bool>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "Please enter all required inputs.",
                        Data = false
                    };
                }
            }
            catch (Exception ex)
            {
                model = new Result<bool>
                {
                    Status = 0,
                    Count = 0,
                    Message = ex.Message,
                    Data = false
                };
            }
            return Ok(model);
        }

        [HttpPost("AcceptQuotationByCustomer")]
        public async Task<IActionResult> AcceptQuotationByCustomer(AcceptCustomerRequestViewModel objAcceptCustomerRequestViewModel)
        {
            var model = new Result<bool>();

            try
            {
                if (objAcceptCustomerRequestViewModel.customerId > 0 || objAcceptCustomerRequestViewModel.quoationDetailedId > 0)
                {
                    int status = _requestService.AcceptQuotationByCustomer(objAcceptCustomerRequestViewModel.customerId,
                        objAcceptCustomerRequestViewModel.quoationDetailedId).Result;

                    if (status == 0)
                    {
                        model = new Result<bool>
                        {
                            Status = 2,
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
                            Message = "Quatations Request Has Been Sent Successfully.",
                            Data = true
                        };

                    }
                    else if (status == 2)
                    {
                        model = new Result<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Request is already rejected by vendor",
                            Data = false
                        };

                    }
                    else if (status == 3)
                    {
                        model = new Result<bool>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "Request Already in Process, Please wait.",
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
                        Message = "Invalid Request",
                        Data = false
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public AddressModel GetAddressInfo(double latitude, double longitude)
        {
            AddressModel objAddressModel = new AddressModel();

            string requestUri = string.Format("https://api.distancematrix.ai/maps/api/geocode/json?latlng={0},{1}&key=xXEaFwNghC7buXPval97zxtUAXAqY", latitude, longitude);

            WebRequest request = WebRequest.Create(requestUri);

            using (var twitpicResponse = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                {
                    var objText = reader.ReadToEnd();
                    var addressDetails = JsonConvert.DeserializeObject<UserLocationDetails>(objText);

                    if (addressDetails != null)
                    {
                        objAddressModel.Address = addressDetails.result[0].formatted_address;
                        if (addressDetails.result[0] != null)
                        {
                            if (addressDetails.result[0].address_components.Exists(x => x.types.Contains("locality")) == true)
                            {
                                objAddressModel.City = Convert.ToString(addressDetails.result[0].address_components.Where(x => x.types.Contains("locality")).SingleOrDefault().long_name);
                            }
                            else
                            {
                                objAddressModel.City = string.Empty;
                            }
                            objAddressModel.Latitude = latitude;
                            objAddressModel.Longitude = longitude;
                        }
                    }
                }
            }
            return objAddressModel;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public ServiceRequestModel GetDistanceByLatLong(ref ServiceRequestModel objServiceRequestModel)
        {
            string requestUri = string.Format("https://api.distancematrix.ai/maps/api/distancematrix/json?origins={0},{1}&destinations={2},{3}&departure_time=now&key=xXEaFwNghC7buXPval97zxtUAXAqY",
              objServiceRequestModel.PickupLocation.Latitude,
              objServiceRequestModel.PickupLocation.Longitude,
              objServiceRequestModel.DropOffLocation.Latitude,
              objServiceRequestModel.DropOffLocation.Longitude);

            WebRequest request = WebRequest.Create(requestUri);

            using (var twitpicResponse = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                {
                    var objText = reader.ReadToEnd();
                    var distanceDetails = JsonConvert.DeserializeObject<DistanceInfoModel>(objText);

                    if (distanceDetails != null)
                    {
                        string distanceString = distanceDetails.rows[0].elements[0].distance.text.Replace("km", "");
                        objServiceRequestModel.DistanceKM = Convert.ToDecimal(distanceString);
                        objServiceRequestModel.DurationInMins = distanceDetails.rows[0].elements[0].duration_in_traffic.text;
                    }
                }
            }

            return objServiceRequestModel;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public VendorDistanceModel GetVendorDistanceByLatLong(RequestVendorModel objServiceRequestModel, double latitude, double longitude)
        {
            string requestUri = string.Format("https://api.distancematrix.ai/maps/api/distancematrix/json?origins={0},{1}&destinations={2},{3}&departure_time=now&key=xXEaFwNghC7buXPval97zxtUAXAqY",
             latitude, longitude, objServiceRequestModel.PickupLocation.Latitude,
              objServiceRequestModel.PickupLocation.Longitude);

            VendorDistanceModel objVendorDistanceModel = new VendorDistanceModel();
            WebRequest request = WebRequest.Create(requestUri);

            using (var twitpicResponse = (HttpWebResponse)request.GetResponse())
            {
                using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                {
                    var objText = reader.ReadToEnd();
                    var distanceDetails = JsonConvert.DeserializeObject<DistanceInfoModel>(objText);

                    if (distanceDetails.rows != null)
                    {
                        string distanceString = distanceDetails.rows[0].elements[0].distance.text.Replace("km", "");
                        objVendorDistanceModel.DistanceKM = Convert.ToDecimal(distanceString);
                        objVendorDistanceModel.DurationInMins = distanceDetails.rows[0].elements[0].duration_in_traffic.text;
                    }
                }
            }
            return objVendorDistanceModel;
        }

        [HttpGet("GetAllQuotationRequest")]
        public async Task<IActionResult> GetAllQuotationRequest(int customerId)
        {
            RequestVendorModel objRequestVendorModel = new RequestVendorModel();
            var model = new Result<RequestVendorModel>();
            try
            {
                if (customerId > 0)
                {
                    var objVendorDetails = await _requestService.GetCustomerServiceRequest(customerId);

                    if (objVendorDetails.Count() > 0)
                    {
                        CustomerRequestModel objCustomerRequestModel = new CustomerRequestModel();
                        objCustomerRequestModel = objVendorDetails.ElementAt(0);

                        //Current Locations
                        objRequestVendorModel.CurrentLocation = new AddressModel();
                        objRequestVendorModel.CurrentLocation.Address = objCustomerRequestModel.CurrentLocation;
                        objRequestVendorModel.CurrentLocation.Latitude = objCustomerRequestModel.CurrentLatitude;
                        objRequestVendorModel.CurrentLocation.Longitude = objCustomerRequestModel.CurrentLongitude;
                        //PickUp Off Locations
                        objRequestVendorModel.PickupLocation = new AddressModel();
                        objRequestVendorModel.PickupLocation.Address = objCustomerRequestModel.PickUpLocation;
                        objRequestVendorModel.PickupLocation.Latitude = objCustomerRequestModel.PickupLatitude;
                        objRequestVendorModel.PickupLocation.Longitude = objCustomerRequestModel.PickupLongitude;
                        //DropOff Locations
                        objRequestVendorModel.DropOffLocation = new AddressModel();
                        objRequestVendorModel.DropOffLocation.Address = objCustomerRequestModel.DropOffLocation;
                        objRequestVendorModel.DropOffLocation.Latitude = objCustomerRequestModel.DropOffLatitude;
                        objRequestVendorModel.DropOffLocation.Longitude = objCustomerRequestModel.DropOffLongitude;
                        //Other Details
                        objRequestVendorModel.DistanceKM = objCustomerRequestModel.DistanceKM;
                        objRequestVendorModel.DurationInMins = objCustomerRequestModel.DurationInMins;
                        objRequestVendorModel.RequestNumber = objCustomerRequestModel.RequestNumber;
                        objRequestVendorModel.RequestId = objCustomerRequestModel.RequestId;
                        objRequestVendorModel.CurrentStageId = objCustomerRequestModel.CurrentStageId;
                        objRequestVendorModel.ExpireDateTime = objCustomerRequestModel.ExpireDateTime;
                        objRequestVendorModel.UserId = customerId;

                        List<RequestVendorDetailsModel> ObjVendorInfoDetails = new List<RequestVendorDetailsModel>();

                        foreach (var currentVendor in objVendorDetails)
                        {
                            VendorDistanceModel objVendorDistanceModel = GetVendorDistanceByLatLong(objRequestVendorModel,
                                currentVendor.VendorLatitude, currentVendor.VendorLongitude);
                            if (objVendorDistanceModel != null)
                            {
                                RequestVendorDetailsModel objRequestVendorDetailsModel = new RequestVendorDetailsModel();
                                objRequestVendorDetailsModel.VendorId = currentVendor.VendorUserId;
                                objRequestVendorDetailsModel.DistanceKM = objVendorDistanceModel.DistanceKM;
                                objRequestVendorDetailsModel.DurationInMins = objVendorDistanceModel.DurationInMins;
                                objRequestVendorDetailsModel.Latitude = currentVendor.VendorLatitude;
                                objRequestVendorDetailsModel.Longitude = currentVendor.VendorLongitude;
                                objRequestVendorDetailsModel.ContactNo = currentVendor.VendorContactNo;
                                objRequestVendorDetailsModel.FirstName = currentVendor.VendorFirstName;
                                objRequestVendorDetailsModel.LastName = currentVendor.VendorLastName;
                                objRequestVendorDetailsModel.TotalAmount = currentVendor.TotalAmount;
                                objRequestVendorDetailsModel.IsCustomerAccepted= currentVendor.IsCustomerAccepted;
                                objRequestVendorDetailsModel.IsRejectedByVendor = currentVendor.IsRejectedByVendor;
                                objRequestVendorDetailsModel.QuoationDetailId = currentVendor.QuoationDetailId;
                                ObjVendorInfoDetails.Add(objRequestVendorDetailsModel);
                            }
                        }

                        objRequestVendorModel.VendorDetails = ObjVendorInfoDetails;
                        model = new Result<RequestVendorModel>
                        {
                            Status = 1,
                            Count = objVendorDetails.Count(),
                            Message = "Request Found Successfully",
                            Data = objRequestVendorModel
                        };
                    }
                    else
                    {
                        model = new Result<RequestVendorModel>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "No Active Request Found",
                            Data = null
                        };
                    }
                }
                else
                {
                    model = new Result<RequestVendorModel>
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
                model = new Result<RequestVendorModel>
                {
                    Status = 0,
                    Count = 0,
                    Message = ex.Message,
                    Data = null
                };
            }
            return Ok(model);
        }

        [HttpGet("GetCustomerServiceRequest")]
        public async Task<IActionResult> GetCustomerServiceRequest(int customerId)
        {
            RequestVendorModel objRequestVendorModel = new RequestVendorModel();
            var model = new Result<RequestVendorModel>();
            try
            {
                if (customerId > 0)
                {
                    var objVendorDetails = await _requestService.GetActiveCustomerServiceRequest(customerId);

                    if (objVendorDetails.Count() > 0)
                    {
                        CustomerRequestServiceModel objCustomerRequestModel = new CustomerRequestServiceModel();
                        objCustomerRequestModel = objVendorDetails.ElementAt(0);
                         
                        //PickUp Off Locations
                        objRequestVendorModel.PickupLocation = new AddressModel();
                        objRequestVendorModel.PickupLocation.Address = objCustomerRequestModel.PickUpLocation;
                        objRequestVendorModel.PickupLocation.Latitude = objCustomerRequestModel.PickupLatitude;
                        objRequestVendorModel.PickupLocation.Longitude = objCustomerRequestModel.PickupLongitude;
                        //DropOff Locations
                        objRequestVendorModel.DropOffLocation = new AddressModel();
                        objRequestVendorModel.DropOffLocation.Address = objCustomerRequestModel.DropOffLocation;
                        objRequestVendorModel.DropOffLocation.Latitude = objCustomerRequestModel.DropOffLatitude;
                        objRequestVendorModel.DropOffLocation.Longitude = objCustomerRequestModel.DropOffLongitude;
                        //Other Details
                        objRequestVendorModel.DistanceKM = objCustomerRequestModel.DistanceKM;
                        objRequestVendorModel.DurationInMins = objCustomerRequestModel.DurationInMins;
                        objRequestVendorModel.RequestNumber = objCustomerRequestModel.RequestNumber;
                        objRequestVendorModel.RequestId = objCustomerRequestModel.RequestId;
                        objRequestVendorModel.CurrentStageId = objCustomerRequestModel.CurrentStageId;
                        objRequestVendorModel.UserId = customerId;

                        List<RequestVendorDetailsModel> ObjVendorInfoDetails = new List<RequestVendorDetailsModel>();

                        foreach (var currentVendor in objVendorDetails)
                        {
                            VendorDistanceModel objVendorDistanceModel = GetVendorDistanceByLatLong(objRequestVendorModel,
                                currentVendor.VendorLatitude, currentVendor.VendorLongitude);
                            if (objVendorDistanceModel != null)
                            {
                                RequestVendorDetailsModel objRequestVendorDetailsModel = new RequestVendorDetailsModel();
                                objRequestVendorDetailsModel.VendorId = currentVendor.VendorUserId;
                                objRequestVendorDetailsModel.DistanceKM = objVendorDistanceModel.DistanceKM;
                                objRequestVendorDetailsModel.DurationInMins = objVendorDistanceModel.DurationInMins;
                                objRequestVendorDetailsModel.Latitude = currentVendor.VendorLatitude;
                                objRequestVendorDetailsModel.Longitude = currentVendor.VendorLongitude;
                                objRequestVendorDetailsModel.ContactNo = currentVendor.VendorContactNo;
                                objRequestVendorDetailsModel.FirstName = currentVendor.VendorFirstName;
                                objRequestVendorDetailsModel.LastName = currentVendor.VendorLastName;
                                objRequestVendorDetailsModel.TotalAmount = currentVendor.TotalAmount;
                                objRequestVendorDetailsModel.IsCustomerAccepted = currentVendor.IsCustomerAccepted;
                                objRequestVendorDetailsModel.IsRejectedByVendor = currentVendor.IsRejectedByVendor;
                                objRequestVendorDetailsModel.VehicleNumber = currentVendor.VehicleNumber;
                                ObjVendorInfoDetails.Add(objRequestVendorDetailsModel);
                            }
                        }
                        objRequestVendorModel.VendorDetails = ObjVendorInfoDetails;
                        model = new Result<RequestVendorModel>
                        {
                            Status = 1,
                            Count = objVendorDetails.Count(),
                            Message = "Request Found Successfully",
                            Data = objRequestVendorModel
                        };
                    }
                    else
                    {
                        model = new Result<RequestVendorModel>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "No Active Request Found",
                            Data = null
                        };
                    }
                }
                else
                {
                    model = new Result<RequestVendorModel>
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
                model = new Result<RequestVendorModel>
                {
                    Status = 0,
                    Count = 0,
                    Message = ex.Message,
                    Data = null
                };
            }
            return Ok(model);
        }

        [HttpGet("GetCurrentStatusByCustomer")]
        public async Task<IActionResult> GetCurrentStatusByCustomer(int customerId)
        {
            RequestVendorModel objRequestVendorModel = new RequestVendorModel();
            var model = new Result<CustomerRequestStatusModel>();

            try
            {
                if (customerId > 0)
                {
                    CustomerRequestStatusModel status = await _requestService.GetCurrentStatusByCustomer(customerId);
                    if (status.RequestId > 0)
                    {
                        model = new Result<CustomerRequestStatusModel>
                        {
                            Status = 1,
                            Count = 1,
                            Message = "Active Request Found.",
                            Data = status
                        };
                    }
                    else
                    {
                        model = new Result<CustomerRequestStatusModel>
                        {
                            Status = 2,
                            Count = 0,
                            Message = "No Active Request Found",
                            Data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                model = new Result<CustomerRequestStatusModel>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Invalid Request",
                    Data = null
                };
                _logger.LogError(ex, "Transaction failed");
                return Ok(model);
            }
             
            return Ok(model);
        }

        [HttpPost("RejectServiceRequestByCustomer")]
        public async Task<IActionResult> RejectServiceRequestByCustomer(RejectCustomerRequestViewModel objRejectCustomerModel)
        {
            var model = new Result<bool>();

            try
            {
                if (objRejectCustomerModel.customerId > 0 || objRejectCustomerModel.requestId > 0)
                {
                    int status = _requestService.RejectServiceRequestByCustomer(objRejectCustomerModel.customerId,
                        objRejectCustomerModel.requestId).Result;

                    if (status == 0)
                    {
                        model = new Result<bool>
                        {
                            Status = 2,
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
                            Message = "Request has been cancelled successfully.",
                            Data = true
                        };
                    }
                }
                else
                {
                    model = new Result<bool>
                    {
                        Status = 2,
                        Count = 0,
                        Message = "Invalid Request",
                        Data = false
                    };
                }
            }
            catch (Exception ex)
            {
                model = new Result<bool>
                {
                    Status = 0,
                    Count = 0,
                    Message = "Something wrong happened",
                    Data = false
                };
                _logger.LogError(ex, "Transaction failed");
            }
            return Ok(model);
        }
    }
}
