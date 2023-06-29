using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarthi.Core.ViewModels
{  
    public class VendorRequestViewModel
    { 
        public int vendorId { get; set; }
        public int quoationDetailedId { get; set; }
    }

    public class VendorUpdateRequestViewModel
    {
        public int requestId { get; set; }
        public int userId { get; set; }
        public int stageId { get; set; }
    }

    public class VendorRequestServiceModel
    {
        public int RequestId { get; set; }
        public string RequestNumber { get; set; } 
        public string PickUpLocation { get; set; }
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public string DropOffLocation { get; set; }
        public double DropOffLatitude { get; set; }
        public double DropOffLongitude { get; set; }
        public string DurationInMins { get; set; }
        public decimal DistanceKM { get; set; }
        public int CurrentStageId { get; set; }
        public bool? IsCustomerAccepted { get; set; }
        public bool? IsRejectedByVendor { get; set; }
        public decimal TotalAmount { get; set; }
        public string VendorContactNo { get; set; }
        public string VendorFirstName { get; set; }
        public string VendorLastName { get; set; }
        public int CustomerId { get; set; }
        public double VendorLatitude { get; set; }
        public double VendorLongitude { get; set; }
        public string VehicleNumber { get; set; }
        public int QuoationDetailId { get; set; }
        public DateTime RequestExpireTime { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerContactNo { get; set; }
    }

    public class VendorLocationViewModel
    {
        public int vendorId { get; set; }
        public double CurrentLatitude { get; set; }
        public double CurrentLongitude { get; set; }
        public int ShiftId { get; set; }
        public int? RequestId { get; set; }
    }

    public class VendorShiftViewModel
    {
        public int Status { get; set; }
        public int? ShiftId { get; set; } 
    }
}
