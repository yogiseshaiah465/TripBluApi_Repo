using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TripxolHotelsWebapi.Controllers
{
    public class HotelSignUpController : ApiController
    {
        public string Get(string ipaddress, string FirstName, string LastName, string Email, string Phone, string Password, string b2c_idn, string b2b_idn)
        {
            string rvalue = "N";
            rvalue = HotelDBLayer.SaveSignUp(ipaddress, FirstName, LastName, Email, Phone, Password,  b2c_idn,  b2b_idn);
            return rvalue;
        }
    }
}