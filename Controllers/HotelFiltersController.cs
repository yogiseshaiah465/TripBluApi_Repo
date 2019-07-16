using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http.Headers;

namespace TripxolHotelsWebapi.Controllers
{
    public class HotelFiltersController : ApiController
    {
        //
        // GET: /HotelFilters/
        public string GetHFilters(string searchid)
        {
            string rvalue = "";
            DataTable dssearch = HotelDBLayer.GetSearch(searchid);
            string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_HotelData.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(HLFPath);
            DataTable Hotellist = ds.Tables[0];
            return rvalue;
        }

    }
}
