using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HotelDevMTWebapi.Models;
using System.Globalization;

namespace HotelDevMTWebapi.Controllers
{
    public class TestingController : ApiController
    {
        public static string pcc = "";
        public static string ipcc = "";
        public static string username = "";
        public static string password = "";
        public static string result = "";
        public string ContextResult = "";
        TextInfo TextInfo;
        public AvailabilityRS Get(string searchid, string hcode, string checkin, string checkout, string gc, string CurrencyCode, string b2c_idn)
        {
            ManageHDetAj mhd = new ManageHDetAj();
            AvailabilityRS hpr = mhd.GetData(searchid, hcode, CurrencyCode, b2c_idn);
            return hpr;
        }
    }
}
