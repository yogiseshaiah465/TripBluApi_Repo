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
using System;
using HotelDevMTWebapi.Models;
using System.Collections.Generic;
using System.Configuration;


namespace HotelDevMTWebapi.Controllers
{
    

    public class RatecommentsHBController : ApiController
    {

        [HttpGet]
        public string  Get(string searchid, string Ratecommentdt)
        {
            //string rvalue = "test";

            //rateresult objrateresult = new rateresult();
            RatecommentsAj RatecommentsAjdata = new RatecommentsAj(searchid, Ratecommentdt);
            string Rdescription = RatecommentsAjdata.Ratecommentdescription;

            //if (Rdescription != "")
            //{
            //    objrateresult.desc = Rdescription;
            //}
            //else
            //{
            //    objrateresult = null;
            //}

            return Rdescription;
           
        }




        private string checkrates(string SearchID, string hotelcode, string ratekey, string b2c_idn)
        {
            string sc = GetRqcond(SearchID, hotelcode, ratekey, b2c_idn);
            string result = "";
            result = Getrates(sc, SearchID, ratekey, b2c_idn);
            return result;

        }

        public string Getrates(string sc, string SearchID, string ratekey, string b2c_idn)
        {
            string result = "";

            //   ManageHDetAj mhd = new ManageHDetAj();
            // HotelPropertyAj hpr = mhd.GetData(searchid, hcode,"USD");
            string htlchkuri = ConfigurationManager.AppSettings["HotelPortalBookingUri"] != null ? ConfigurationManager.AppSettings["HotelPortalCheckRateUri"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(htlchkuri))
            {
                result = XMLRead.SendQuery(sc, htlchkuri);
                SaveXMLFilec(sc, result, SearchID + "_" + "_checkrate");
            }
            return result;
        }



        public static void SaveXMLFilec(string RQXML, string RSXML, string FileName)
        {
            try
            {
                XmlDocument RQdoc = new XmlDocument();
                RQdoc.LoadXml(RQXML);
                RQdoc.PreserveWhitespace = true;
                string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "Checkrates/" + FileName + "-RQ.xml");
                RQdoc.Save(filePathRQ);


                XmlDocument RSdoc = new XmlDocument();
                RSdoc.LoadXml(RSXML);
                RSdoc.PreserveWhitespace = true;
                string filePathRS = Path.Combine(HttpRuntime.AppDomainAppPath, "Checkrates/" + FileName + "-RS.xml");
                RSdoc.Save(filePathRS);

            }
            catch (Exception ex)
            {

            }

        }

        public static string GetRqcond(string SearchID, string hotelcode, string ratekey, string b2c_idn)
        {
            string rq = string.Empty;
          string filePathRQ = string.Empty;
            AvailabilityRS objAvailabilityRS = new AvailabilityRS();
            try
            {
            string searchid = SearchID;
                ClsFevicons objfavicons;
                objfavicons = new ClsFevicons();
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                //filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "checkratexml/" + searchid + "_" + CurrencyCode + "_hotelschkrate-RS.xml");

                rq += "<checkRateRQ xmlns='http://www.hotelbeds.com/schemas/messages' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' upselling='False' language='ENG'>";

                rq += "<rooms><room rateKey='" + ratekey + "'/></rooms>";
               
                rq += "</checkRateRQ>";

            }
            catch
            {
            
            
            }
            return rq;
        
        }
    }
}
