using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HotelDevMTWebapi.Controllers
{
    public class citiesapiController : ApiController
    {
        public string Get()
        {
            string text = System.IO.File.ReadAllText(@"E:\geojson.json");
            return text;
        }
    }
}