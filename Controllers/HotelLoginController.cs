using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TripxolHotelsWebapi.Controllers
{
    public class HotelLoginController : ApiController
    {
        public string Get(string Email, string Password, string b2c_idn, string b2b_idn)
        {
            string rvalue = "N";
            rvalue = HotelDBLayer.Logincheck( Email, Password, b2c_idn,  b2b_idn);
            return rvalue;
        }
    }
}