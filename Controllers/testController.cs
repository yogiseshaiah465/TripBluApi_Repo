using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TripxolHotelsWebapi.Controllers
{
    public class testController : ApiController
    {
        public string GetRoomDetails(string searchid, string hcode)
        {
            return hcode;
        }
    }
}