
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Threading;
using System.Globalization;
using System.Configuration;
using System.Net;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace TripxolHotelsWebapi.Controllers
{
    public class HotelBackOfficeController : ApiController
    {
        // GET api/<controller>/5
        public HotelPropertyAj Get(string searchid, string hcode,string CurrencyCode)
        {
            HotelPropertyAj hpr = null;
            string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_propertydesc_" + hcode + "_" + CurrencyCode + "-RS.xml");
            if (File.Exists(filePathRQ))
            {
                string result = File.ReadAllText(filePathRQ);
                hpr = new HotelPropertyAj(result, hcode, searchid, CurrencyCode, "");
            }
            return hpr;
        }
       
    }
}