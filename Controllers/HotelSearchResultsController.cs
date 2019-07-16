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
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http.Headers;
namespace TripxolHotelsWebapi.Controllers
{
    public class HotelSearchResultsController : ApiController
    {
        // GET api/<controller>
        public HotelSearch  Get(string searchid, string city ,string checkind ,  string checkoutd, string guestscount ,  string selroom , string selaudults, string selchilds)
        {
            HACondition Hac = GetSearchConditon(city ,checkind ,  checkoutd, guestscount ,  Convert.ToInt32(selroom) , Convert.ToInt32(selaudults),Convert.ToInt32(selchilds));
            HotelSearch hs = new HotelSearch(Hac,searchid);
            //string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelAvail/" + searchid + "_Context.xml");
            //File.WriteAllText(filePathRQ, hs.ContextResult);
            //string filePathHA = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelAvail/" + searchid + "_HotelAvail.xml");
            //File.WriteAllText(filePathHA, hs.XMLResult);
            DataTable dtHList = hs.dtBPIadd;
            DataSet ds = new DataSet();
            ds.Tables.Add(dtHList);
            string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelAvail/" + searchid + "_HotelList.xml");
            ds.WriteXml(HLFPath);
            return hs;
        }
        private HACondition GetSearchConditon(string city, string checkin, string checkout, string guestcount, int rooms, int adults, int children)
        {
            try
            {
                HACondition Hac = new HACondition();
                if (city != "") { Hac.City = city.Split('-')[0]; }
                Hac.GuestCount = (adults + children).ToString();
                if (checkin != "") { Hac.CheckIn = Convert.ToDateTime(checkin).ToString("MM-dd"); }
                if (checkout != "") { Hac.CheckOut = Convert.ToDateTime(checkout).ToString("MM-dd"); }
                Hac.Rooms = rooms;
                Hac.Adults = adults;
                Hac.Children = children;
                Hac.SortBy = "low_price";
                return Hac;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}