using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Data;
using System.IO;

namespace HotelDevMTWebapi.Controllers
{
    public class ModifySearchHotelListController : ApiController
    {
        public string Get(string city, string checkind, string checkoutd, string guestscount, string selroom, string seladults, string adultbyroom, string selchilds, string childrenbyroom, string childages, string curcode, string b2c_idn)
        {

            string ipaddress = "";
            string searchid = "";
            string cust_idn = string.Empty;
            if (Request.Properties.ContainsKey("MS_HttpContext"))
            {
                var ctx = Request.Properties["MS_HttpContext"] as HttpContextWrapper;
                if (ctx != null)
                {
                    var ip = ctx.Request.UserHostAddress;
                    //do stuff with IP
                }
            }
            searchid = HotelDBLayer.SaveSearch(city, checkind, checkoutd, Convert.ToInt16(selroom), Convert.ToInt16(seladults), adultbyroom, Convert.ToInt16(selchilds), childrenbyroom, childages, ipaddress, curcode, b2c_idn, cust_idn);
            HACondition Hac = Utilities.GetSearchConditon(city, checkind, checkoutd, guestscount.ToString(), Convert.ToInt32(selroom), Convert.ToInt32(seladults), adultbyroom, Convert.ToInt32(selchilds), childrenbyroom, childages, curcode);
            HotelSearchAj hs = new HotelSearchAj(Hac, searchid, b2c_idn);
            DataTable dtHList = hs.dtBPIadd;
            DataSet ds = new DataSet();
            ds.Tables.Add(dtHList);
            string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_" + curcode + "6140_USD_hotelsAvail-RS.xml");
            ds.WriteXml(HLFPath);
            string rvalue = searchid;
            return rvalue;
        }
    }
}