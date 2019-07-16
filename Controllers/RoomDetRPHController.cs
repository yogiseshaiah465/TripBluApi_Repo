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
namespace TripxolHotelsWebapi.Controllers
{
    public class RoomDetRPHController : ApiController
    {
        public static string pcc = "";
        public static string ipcc = "";
        public static string username = "";
        public static string password = "";
        public static string result = "";
        public static string ContextResult = "";
        TextInfo TextInfo;
        [HttpGet]
        public  string Get(string searchid, string hcode, string checkin, string checkout, string gc, string CurrencyCode, string b2c_idn,string Totelguest,string childs)
        {
            ManageHDetAj mhd = new ManageHDetAj();
            string hpr = GetDataroom(searchid, hcode, CurrencyCode, b2c_idn);
            return hpr;
        }


        public static string GetDataroom(string SearchID, string hotelcode, string CurrencyCode, string b2c_idn)
        {
            
            string filePathRQ = string.Empty;
            AvailabilityRS objAvailabilityRS = new AvailabilityRS();
            ManageHDetAj objManageHDetAj = new ManageHDetAj();
            string result = string.Empty;
           

            try
            {
                string searchid = SearchID;
                ClsFevicons objfavicons;
                objfavicons = new ClsFevicons();
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                // TextInfo = cultureInfo.TextInfo;
                //string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_propertydesc_" + hotelcode + "_" + CurrencyCode + "-RS.xml");
                filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_" + CurrencyCode + "_hotelsAvail-RS.xml");
                XmlDataDocument xmldoc = new XmlDataDocument();
                if (File.Exists(filePathRQ))
                {
                    FileStream fs = new FileStream(filePathRQ, FileMode.Open, FileAccess.Read);
                    xmldoc.Load(fs);
                    fs.Close();
                    XmlNode xnod = xmldoc.DocumentElement;
                    XmlSerializer deserializer = new XmlSerializer(typeof(AvailabilityRS));
                    StreamReader reader = new StreamReader(filePathRQ);
                    result = File.ReadAllText(filePathRQ);
                    objAvailabilityRS = (AvailabilityRS)deserializer.Deserialize(reader);
                }
                else
                {
                    DataTable dssearch = HotelDBLayer.GetSearch(searchid);
                    HPDCondition Hapc = objManageHDetAj.GetCondition(searchid,hotelcode, CurrencyCode);
                    result = objManageHDetAj.gethdata(Hapc, ContextResult, searchid, CurrencyCode, b2c_idn);
                    filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_propertydesc_" + hotelcode + "_" + CurrencyCode + "_hotelsAvail-RS.xml");
                    FileStream fs = new FileStream(filePathRQ, FileMode.Open, FileAccess.Read);
                    xmldoc.Load(fs);
                    fs.Close();
                    XmlNode xnod = xmldoc.DocumentElement;
                    XmlSerializer deserializer = new XmlSerializer(typeof(AvailabilityRS));
                    StreamReader reader = new StreamReader(filePathRQ);
                    objAvailabilityRS = (AvailabilityRS)deserializer.Deserialize(reader);

                    result = File.ReadAllText(filePathRQ);
                    if (!File.Exists(filePathRQ))
                    {
                        File.WriteAllText(filePathRQ, objAvailabilityRS.Xmlns);
                    }
                }


                
            }
            catch { }
            return result;
        }





    }
}