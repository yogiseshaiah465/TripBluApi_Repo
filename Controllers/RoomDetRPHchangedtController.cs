using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Text;
using System.Globalization;
using System.Web;
using HotelDevMTWebapi.Models;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;
using System.Data;

namespace HotelDevMTWebapi.Controllers
{
    public class RoomDetRPHchangedtController : ApiController
    {
        public static string pcc = "";
        public static string ipcc = "";
        public static string username = "";
        public static string password = "";
        public static string result = "";
        public static string ContextResult = "";
        public static TextInfo TextInfo;

        [HttpGet]
        public AvailabilityRS Get(string searchid, string hcode,string rooms, string checkin, string checkout,string adult,string children,string childrenage, string gc, string CurrencyCode, string b2c_idn)
        {
            ManageHDetAj mhd = new ManageHDetAj();
            //string hpr = GetDataroom(searchid, hcode, CurrencyCode, b2c_idn);
            AvailabilityRS hpr = mhd.GetDataChangedate(searchid, hcode,rooms,checkin,checkout,adult,children,childrenage,gc, CurrencyCode, b2c_idn);
            return hpr;
        }
    }
}