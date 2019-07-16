using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TripxolHotelsWebapi.Controllers
{
    public class ContactUsController : ApiController
    {
        public string Get(string ipaddress, string Name, string Email, string Phone, string Company, string Message, string b2c_idn)
        {
            string rvalue = "N";
            rvalue = HotelDBLayer.SaveContactUs(ipaddress, Name, Email, Phone, Company, Message);
            return rvalue ;
        }
    }
}