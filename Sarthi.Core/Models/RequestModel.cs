using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace Sarthi.Core.Models
{
    public class ServiceRequestModel
    {
        public string RequestNumber { get; set; }
        public decimal DistanceKM { get; set; }
        public string DurationInMins { get; set; }
        public AddressModel CurrentLocation { get; set; }
        public AddressModel PickupLocation { get; set; }
        public AddressModel DropOffLocation { get; set; }
    }

    public class AddressModel
    {
        public string Address { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class DistanceInfoModel
    {
        public List<RowsModel> rows { get; set; }
    }

    public class RowsModel
    {
        public List<ElementsModel> elements { get; set; }
    }

    public class ElementsModel
    {
        public DistanceModel distance { get; set; }
        public DistanceModel duration_in_traffic { get; set; }
    }

    public class DistanceModel
    {
        public string text { get; set; }
        public string value { get; set; }
    }

    public class RequestVendorModel
    {
        public string DurationInMins { get; set; }
        public decimal DistanceKM { get; set; }
        public AddressModel CurrentLocation { get; set; }
        public AddressModel PickupLocation { get; set; }
        public AddressModel DropOffLocation { get; set; }
        public IEnumerable<RequestVendorDetailsModel> VendorDetails { get; set; }
        public int UserId { get; set; }
        public int RequestId { get; set; }
        public string RequestNumber { get; set; }
        public int CurrentStageId { get; set; }
        public DateTime ExpireDateTime { get; set; }
    }

    public class RequestVendorDetailsModel
    {
        public int VendorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public decimal TotalAmount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string DurationInMins { get; set; }
        public decimal DistanceKM { get; set; }
        public bool? IsCustomerAccepted { get; set; }
        public bool? IsRejectedByVendor { get; set; }
        public string VehicleNumber { get; set; }
    }

    public class VendorDistanceModel
    {
        public string DurationInMins { get; set; }
        public decimal DistanceKM { get; set; }
    }

    public class ResponseRequestModel
    {
        public int RequestId { get; set; }
        public string RequestNumber { get; set; }
    }

    public class CustomerRequestModel
    {
        public int RequestId { get; set; }
        public string RequestNumber { get; set; }
        public string CurrentLocation { get; set; }
        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }
        public string PickUpLocation { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public string DropOffLocation { get; set; }
        public double DropOffLatitude { get; set; }
        public double DropOffLongitude { get; set; }
        public string DurationInMins { get; set; }
        public decimal DistanceKM { get; set; }
        public int CurrentStageId { get; set; }
        public DateTime ExpireDateTime { get; set; }
        public bool? IsCustomerAccepted { get; set; }
        public bool? IsRejectedByVendor { get; set; }
        public decimal TotalAmount { get; set; }
        public string VendorContactNo { get; set; }
        public string VendorFirstName { get; set; }
        public string VendorLastName { get; set; }
        public int VendorUserId { get; set; }
        public double VendorLatitude { get; set; }
        public double VendorLongitude { get; set; }

    }

    public class CustomerRequestServiceModel
    {
        public int RequestId { get; set; }
        public string RequestNumber { get; set; }
        public string CurrentLocation { get; set; }
        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }
        public string PickUpLocation { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public string DropOffLocation { get; set; }
        public double DropOffLatitude { get; set; }
        public double DropOffLongitude { get; set; }
        public string DurationInMins { get; set; }
        public decimal DistanceKM { get; set; }
        public int CurrentStageId { get; set; }
        public DateTime ExpireDateTime { get; set; }
        public bool? IsCustomerAccepted { get; set; }
        public bool? IsRejectedByVendor { get; set; }
        public decimal TotalAmount { get; set; }
        public string VendorContactNo { get; set; }
        public string VendorFirstName { get; set; }
        public string VendorLastName { get; set; }
        public int VendorUserId { get; set; }
        public double VendorLatitude { get; set; }
        public double VendorLongitude { get; set; }
        public string VehicleNumber { get; set; }

    }
}
