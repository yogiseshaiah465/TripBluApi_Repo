using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.IO;
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
using System.Xml.Linq;
using System.Xml;

namespace TripxolHotelsWebapi.Controllers
{
    public class ImageUrlController : ApiController
    {
        HotelImageJson hij = new HotelImageJson();
        public HotelImageJson Get(string searchid, string HotelCode, string curcode)
        {
            string rvalue = "";
            string ContextResult = "";

            DataTable  dtHotelUrl = HotelDBLayer.GetHotelImageUrl(HotelCode);
            if (dtHotelUrl.Rows.Count > 0)
            {
                hij.Image = dtHotelUrl.Rows[0]["ImageUrl"].ToString();
                hij.Logo = dtHotelUrl.Rows[0]["Logourl"].ToString();
                return hij;
            }
            else
            {
                string filePathContext = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_" + curcode + "_ContextChange-RS.xml");
                if (File.Exists(filePathContext))
                {
                    ContextResult = File.ReadAllText(filePathContext);
                }
                else
                {
                    ContextResult = XMLRead.ContextChange(searchid);
                }
                HotelImageAj hotelimage = new HotelImageAj(HotelCode, "", searchid, ContextResult);
                string image = hotelimage.Image;
                string logo = hotelimage.logo;
                HotelDBLayer.SaveHotelImageUrl(HotelCode,image,logo);
                hij.Image = image;
                hij.Logo = logo;
                return hij;
            }
        }
    }
}