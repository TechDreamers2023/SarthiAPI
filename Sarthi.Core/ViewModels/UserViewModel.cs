using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sarthi.Core.ViewModels
{
    public class UserViewModel
    {
        public string emailAddress { get; set; }
        public string password { get; set; }
    }

    public class UserProfileViewModel
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string VehicleNumber { get; set; }
        public string ContactNo { get; set; }
    }

    public class UserRegisterViewModel
    { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string VehicleNumber { get; set; }
        public string ContactNo { get; set; }
    }
}
