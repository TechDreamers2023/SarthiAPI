using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;

namespace Sarthi.Core.Models
{
    public class Result<T>
    {
        public int Status { get; set; }
        public int Count { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class ResultList<T>
    {
        public int Status { get; set; }
        public int Count { get; set; }
        public string Message { get; set; }
        public IEnumerable<T> Data { get; set; }
    }

    public class UserAuth
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UsertypeId { get; set; }
    }

    public class UserLocationDetails
    {
        [JsonExtensionData]
        public List<AddressInfo> result { get; set; }
    }

    public class AddressInfo
    {
        public string formatted_address { get; set; }
        public List<AddressComponent> address_components { get; set; }
    }

    public class AddressComponent
    {
        public string long_name { get; set; }
        public List<string> types { get; set;}
    }

    public class UserProfile
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string VehicleNumber { get; set; }
        public string ContactNo { get; set; } 
    }

    public class RequestHistoryDetails
    {
        public int RequestId { get; set; }
        public string RequestNumber { get; set; }
        public string PickUpLocation { get; set; }
        public string DropOffLocation { get; set; }
        public string DistanceKM { get; set; }
        public string DurationInMins { get; set; }
        public int CurrentStageId { get; set; }
        public decimal TotalAmount { get; set; }
        public string VendorContactNo { get; set; }
        public string VendorFirstName { get; set; }
        public string VendorLastName { get; set; }
        public int VendorUserId { get; set; }
        public string VendorVehicleNumber { get; set; }
        public string CustomerContactNo { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerVehicleNumber { get; set; }
        public int QuoationDetailId { get; set; }
        public string StageName { get; set; }
        public DateTime? PaymentDateTime { get; set; }
    }

    public class TrackServiceModel
    {
        public int RequestId { get; set; }
        public string StageId { get; set; }
        public string StageName { get; set; }
        public string DateString { get; set; }
        public string timeString { get; set; } 
    }


}
