using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace TripxolHotelsWebapi.Controllers
{
    public class HotelRdetHSPaymentController : ApiController
    {
        public HotelPropertyAj Get(string searchid, string hotelcode, string CurrencyCode, string b2c_idn)
        {
            HotelPropertyAj hpr = null;
            string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_propertydesc_" + hotelcode + "_" + CurrencyCode + "-RS.xml");

            if (File.Exists(filePathRQ))
            {
                string result = File.ReadAllText(filePathRQ);
                hpr = new HotelPropertyAj(result, hotelcode, searchid, CurrencyCode, b2c_idn);
            }
            return hpr;
        }
    }
}