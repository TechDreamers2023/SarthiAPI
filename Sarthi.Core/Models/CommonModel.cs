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


}
