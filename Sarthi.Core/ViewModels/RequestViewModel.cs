using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarthi.Core.ViewModels
{
    public class RequestViewModel
    {
        public double currentlat { get; set; }
        public double currentlong { get; set; }
        public double pickuplat { get; set; }
        public double pickuplong { get; set; }
        public double dropOfflat { get; set; }
        public double dropOfflong { get; set; }
        public int userId { get; set; }
    }

    public class AcceptCustomerRequestViewModel
    { 
        public int customerId { get; set; }
        public int quoationDetailedId { get; set; }
    }
}
