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
using System.Data;
using System.Data.SqlClient;


namespace HotelDevMTWebapi.Controllers
{
    public class checkresult
    {
        public string amount { get; set; }
        public string Tax { get; set; }
        public string cancellationamt { get; set; }
        public string cancellationfrom { get; set; }
        public string Message { get; set; }

    }

    public class CheckrateController : ApiController
    {
        public static string con = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();

        [HttpGet]
        public checkresult Get(string searchid, string hcode, string ratekey, string b2c_idn)
        {
            double admarkup = 0.00;
            double adpercmarkup = 0.00;
            double clpercmarkup = 0.00;
            double clmarkup = 0.00;
            double finalmarkup = 0.00;
            double finaldiscount = 0.00;
            double adperdiscount = 0.00;
            double clperdiscount = 0.00;
            double addiscount = 0.00;
            double cldiscount = 0.00;
            double baseamount = 0.00;


            string filePathRQ = string.Empty;
            ManageHDetAj mhd = new ManageHDetAj();
            string Result = string.Empty;
            AvailabilityRS objAvailabilityRS = new AvailabilityRS();
            CheckRateRS objCheckRateRS = new CheckRateRS();
            List<upselling> lstupselling = new List<upselling>();
            Room objupsellingRoom = new Room();
            checkresult objchkres = new checkresult();
            checkrates(searchid, hcode, ratekey, b2c_idn);


            string ratekey_split = ratekey.Substring(ratekey.Length - 4);


            try
            {
                ClsFevicons objfavicons;
                objfavicons = new ClsFevicons();
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                // TextInfo = cultureInfo.TextInfo;
                //string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_propertydesc_" + hotelcode + "_" + CurrencyCode + "-RS.xml");
                filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "Checkrates_tbluexml/" + searchid + "_"+ratekey_split+ "_checkrate-RS.xml");

                if (File.Exists(filePathRQ))
                {
                    XmlDataDocument xmldoc = new XmlDataDocument();
                    FileStream fs = new FileStream(filePathRQ, FileMode.Open, FileAccess.Read);
                    xmldoc.Load(fs);
                    fs.Close();
                    XmlNode xnod = xmldoc.DocumentElement;
                    XmlSerializer deserializer = new XmlSerializer(typeof(CheckRateRS));
                    StreamReader reader = new StreamReader(filePathRQ);
                    objCheckRateRS = (CheckRateRS)deserializer.Deserialize(reader);

                    if (objCheckRateRS.Error== null)
                    {

                        string checkin = objCheckRateRS.Hotel.CheckIn;
                        string CheckOut = objCheckRateRS.Hotel.CheckOut;
                        double dc = 0.0;
                        try
                        { dc = (Convert.ToDateTime(CheckOut.ToString()) - Convert.ToDateTime(checkin.ToString())).TotalDays; }
                        catch { }
                        objchkres.amount = Convert.ToDouble(Convert.ToDouble(objCheckRateRS.Hotel.Rooms.Room[0].Rates.Rate[0].Net) / dc).ToString("0.00");
                        try
                        {
                            //List<Tax> lsttax = new List<Tax>();
                            //foreach (var lttax in objCheckRateRS.Hotel.Rooms.Room[0].Rates.Rate[0].Taxes.Tax)
                            //{
                                if (objCheckRateRS.Hotel.Rooms.Room[0].Rates.Rate[0].Taxes.Tax.Amount != null)
                                {
                                    
                                    objchkres.Tax = Convert.ToDouble(Convert.ToDouble(objCheckRateRS.Hotel.Rooms.Room[0].Rates.Rate[0].Taxes.Tax.Amount)).ToString("0.00");
                                }
                                else
                                {
                                    objchkres.Tax = "0.00";
                                }
                            //}

                           
                        }
                        catch
                        {
                            objchkres.Tax = "0.00";
                        }

                        baseamount = Convert.ToDouble(objchkres.amount);


                        DataTable dt = new DataTable();
                        SqlConnection sqlcon = new SqlConnection(con);
                        try
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = sqlcon;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "p_SreTS_HDR";
                            cmd.Parameters.AddWithValue("@B2C_IDN", b2c_idn);
                            cmd.Parameters.AddWithValue("@Hotelcode", hcode.ToString());
                            cmd.Parameters.AddWithValue("@GDS", "HB");
                            cmd.Parameters.AddWithValue("@IsLoginCust", "Y");
                            SqlDataAdapter sa = new SqlDataAdapter(cmd);
                            sa.Fill(dt);
                        }
                        catch
                        {
                        }


                        if (dt.Rows.Count > 0)
                        {
                            string Ts_mode = string.Empty;
                            Ts_mode = dt.Rows[0]["TS_Mode"].ToString();
                            if (Ts_mode == "Fixed")
                            {
                                admarkup = Convert.ToDouble(dt.Rows[0]["TS_Markup"].ToString());
                                addiscount = Convert.ToDouble(dt.Rows[0]["TS_Discount"].ToString());
                            }
                            else if (Ts_mode == "Percentage")
                            {
                                adpercmarkup = Convert.ToDouble(dt.Rows[0]["TS_Markup"].ToString());
                                adperdiscount = Convert.ToDouble(dt.Rows[0]["TS_Discount"].ToString());
                                admarkup = (((baseamount) / 100.00) * adpercmarkup);
                                addiscount = (((baseamount) / 100.00) * adperdiscount);

                            }
                            else
                            {
                                admarkup = 0.00;
                                addiscount = 0.00;
                            }


                            string Cl_Mode = string.Empty;
                            Cl_Mode = dt.Rows[0]["Cl_Mode"].ToString();
                            if (Cl_Mode == "Fixed")
                            {
                                clmarkup = Convert.ToDouble(dt.Rows[0]["Cl_Markup"].ToString());
                                cldiscount = Convert.ToDouble(dt.Rows[0]["Cl_Discount"].ToString());
                            }
                            else if (Cl_Mode == "Percentage")
                            {
                                clpercmarkup = Convert.ToDouble(dt.Rows[0]["Cl_Markup"].ToString());
                                clperdiscount = Convert.ToDouble(dt.Rows[0]["Cl_Discount"].ToString());
                                clmarkup = ((baseamount / 100.00) * clpercmarkup);
                                cldiscount = ((baseamount / 100.00) * clperdiscount);

                            }
                            else
                            {
                                clmarkup = 0.00;
                                cldiscount = 0.00;
                            }

                            finalmarkup = admarkup + clmarkup;
                            finaldiscount = addiscount + cldiscount;
                            baseamount = baseamount + (finalmarkup - finaldiscount);


                        }


                        objchkres.amount = (baseamount).ToString("0.00");


                        //HotelListGenerate.CreateTables(dtBPIadd);
                        //HotelListGenerate.FillHStable(xnod, dtBPIadd);//yogi
                    }
                    else
                    {
                        objchkres.Message = "Fare Not Available";

                        
                    }
                }

            }
            catch
            {
                objchkres.Message = "Fare Not Available";
            }
            return objchkres;

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
            string ratekey_split = ratekey.Substring(ratekey.Length - 4);
            //   ManageHDetAj mhd = new ManageHDetAj();
            // HotelPropertyAj hpr = mhd.GetData(searchid, hcode,"USD");
            string htlchkuri = ConfigurationManager.AppSettings["HotelPortalBookingUri"] != null ? ConfigurationManager.AppSettings["HotelPortalCheckRateUri"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(htlchkuri))
            {
                result = XMLRead.SendQuery(sc, htlchkuri);
                SaveXMLFilec(sc, result, SearchID + "_" + ratekey_split + "_checkrate");
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
                string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "Checkrates_tbluexml/" + FileName + "-RQ.xml");
                RQdoc.Save(filePathRQ);


                XmlDocument RSdoc = new XmlDocument();
                RSdoc.LoadXml(RSXML);
                RSdoc.PreserveWhitespace = true;
                string filePathRS = Path.Combine(HttpRuntime.AppDomainAppPath, "Checkrates_tbluexml/" + FileName + "-RS.xml");
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
