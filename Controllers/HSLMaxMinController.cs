using HotelDevMTWebapi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
namespace TripxolHotelsWebapi.Controllers
{
    public class HSLMaxMinController : ApiController
    {
        public static string con = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();

        HotelRateMaxMin hrmm = new HotelRateMaxMin();
        public DataTable dtBPIadd = new DataTable();
        public HotelRateMaxMin Get(string searchid, string curcode,string b2c_idn)
        {
            hrmm.Max = "0";
            hrmm.Min = "0";
            double minrate = 0.00;
            double maxrate = 0.00;
            string minratepernight = string.Empty;
            string maxratepernight = string.Empty;
            string checkin = string.Empty;
            string checkout = string.Empty;
            double dc = 0.0;



            double admarkup = 0.00;
            double adpercmarkup = 0.00;
            double clpercmarkup = 0.00;
            double clmarkup = 0.00;
            double finalmarkup = 0.00;
            double finaldiscount = 0.00;
            double addiscount = 0.00;
            double adperdiscount = 0.00;
            double cldiscount = 0.00;
            double clperdiscount = 0.00;
            double minbaseamount = 0.00;
            double maxbaseamount = 0.00;


            double maxadmarkup = 0.00;
            double maxadpercmarkup = 0.00;
            double maxclpercmarkup = 0.00;
            double maxclmarkup = 0.00;
            double maxfinalmarkup = 0.00;
            double maxfinaldiscount = 0.00;
            double maxaddiscount = 0.00;
            double maxadperdiscount = 0.00;
            double maxcldiscount = 0.00;
            double maxclperdiscount = 0.00;



            AvailabilityRS objAvailabilityRS = new AvailabilityRS();
            string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_" + curcode + "_hotelsAvail-RS.xml");
            if (File.Exists(HLFPath))
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                FileStream fs = new FileStream(HLFPath, FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                fs.Close();
                XmlNode xnod = xmldoc.DocumentElement;
                XmlSerializer deserializer = new XmlSerializer(typeof(AvailabilityRS));
                StreamReader reader = new StreamReader(HLFPath);
                objAvailabilityRS = (AvailabilityRS)deserializer.Deserialize(reader);
                checkin = objAvailabilityRS.Hotels.CheckIn.ToString();
                checkout = objAvailabilityRS.Hotels.CheckOut.ToString();
                dc = 0.0;
                try
                { dc = (Convert.ToDateTime(checkout) - Convert.ToDateTime(checkin)).TotalDays; }
                catch { }

                minrate = Convert.ToDouble(objAvailabilityRS.Hotels.Hotel.Min(k => Convert.ToDouble(k.MinRate)).ToString());
                maxrate = Convert.ToDouble(objAvailabilityRS.Hotels.Hotel.Max(k => Convert.ToDouble(k.MinRate)).ToString());
                //maxrate = Convert.ToDouble(objAvailabilityRS.Hotels.Hotel.Max(k => Convert.ToDouble(k.MaxRate)).ToString());
                maxratepernight = Convert.ToDouble((maxrate) / (dc)).ToString("0.00"); //objAvailabilityRS.Hotels.Hotel.Max(k => Convert.ToDouble(k.MaxRate)).ToString();
                minratepernight = Convert.ToDouble((minrate) / (dc)).ToString("0.00"); //objAvailabilityRS.Hotels.Hotel.Min(k => Convert.ToDouble(k.MinRate)).ToString();



                minbaseamount = (Convert.ToDouble(minratepernight));
                maxbaseamount = (Convert.ToDouble(maxratepernight));
                DataTable dt = new DataTable();
                SqlConnection sqlcon = new SqlConnection(con);
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlcon;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "p_SreTS_HDR";
                    cmd.Parameters.AddWithValue("@B2C_IDN", b2c_idn);
                    cmd.Parameters.AddWithValue("@Hotelcode", 0);
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


                        maxadmarkup = Convert.ToDouble(dt.Rows[0]["TS_Markup"].ToString());
                        maxaddiscount = Convert.ToDouble(dt.Rows[0]["TS_Discount"].ToString());
                    }
                    else if (Ts_mode == "Percentage")
                    {
                        adpercmarkup = Convert.ToDouble(dt.Rows[0]["TS_Markup"].ToString());
                        adperdiscount = Convert.ToDouble(dt.Rows[0]["TS_Discount"].ToString());
                        admarkup = (((minbaseamount) / 100.00) * adpercmarkup);
                        addiscount = (((minbaseamount) / 100.00) * adperdiscount);





                        maxadpercmarkup = Convert.ToDouble(dt.Rows[0]["TS_Markup"].ToString());
                        maxadperdiscount = Convert.ToDouble(dt.Rows[0]["TS_Discount"].ToString());
                        maxadmarkup = (((maxbaseamount) / 100.00) * maxadpercmarkup);
                        maxaddiscount = (((maxbaseamount) / 100.00) * maxadperdiscount);


                    }
                    else
                    {
                        maxadmarkup = 0.00;
                        maxaddiscount = 0.00;
                    }


                    string Cl_Mode = string.Empty;
                    Cl_Mode = dt.Rows[0]["Cl_Mode"].ToString();
                    if (Cl_Mode == "Fixed")
                    {
                        clmarkup = Convert.ToDouble(dt.Rows[0]["Cl_Markup"].ToString());
                        cldiscount = Convert.ToDouble(dt.Rows[0]["Cl_Discount"].ToString());


                        maxclmarkup = Convert.ToDouble(dt.Rows[0]["Cl_Markup"].ToString());
                        maxcldiscount = Convert.ToDouble(dt.Rows[0]["Cl_Discount"].ToString());
                    }
                    else if (Cl_Mode == "Percentage")
                    {
                        clpercmarkup = Convert.ToDouble(dt.Rows[0]["Cl_Markup"].ToString());
                        clperdiscount = Convert.ToDouble(dt.Rows[0]["Cl_Discount"].ToString());
                        clmarkup = ((minbaseamount / 100.00) * clpercmarkup);
                        cldiscount = ((minbaseamount / 100.00) * clperdiscount);





                        maxclpercmarkup = Convert.ToDouble(dt.Rows[0]["Cl_Markup"].ToString());
                        maxclperdiscount = Convert.ToDouble(dt.Rows[0]["Cl_Discount"].ToString());
                        maxclmarkup = ((maxbaseamount / 100.00) * maxclpercmarkup);
                        maxcldiscount = ((maxbaseamount / 100.00) * maxclperdiscount);

                    }
                    else
                    {
                        maxclmarkup = 0.00;
                        maxcldiscount = 0.00;
                    }

                    finalmarkup = admarkup + clmarkup;
                    finaldiscount = addiscount + cldiscount;
                    minbaseamount = minbaseamount + (finalmarkup - finaldiscount);
                    //baseamount = baseamount - finaldiscount;


                    maxfinalmarkup = maxadmarkup + maxclmarkup;
                    maxfinaldiscount = maxaddiscount + maxcldiscount;
                    maxbaseamount = maxbaseamount + (maxfinalmarkup - maxfinaldiscount);


                    hrmm.Min = minbaseamount.ToString();
                    hrmm.Max = maxbaseamount.ToString();
                }




                


















                //CreateTables();
                //FillHStable(xnod);
                //string pricerangecond = GetPricrangecondzero();
                //DataTable dtPricingzero = FilterTable(dtBPIadd, pricerangecond);
                //DataRow[] drmax = dtPricingzero.Select("RateRange_Maxrate = MAX(RateRange_Maxrate)");
                //if (drmax.Count() > 0)
                //{
                //    hrmm.Max = drmax[0]["RateRange_Maxrate"].ToString();
                //}
                //DataRow[] drmin = dtPricingzero.Select("RateRange_Maxrate = MIN(RateRange_Maxrate)");
                //if (drmin.Count() > 0)
                //{
                //    hrmm.Min = drmin[0]["RateRange_Maxrate"].ToString();
                //}

                hrmm.Cursymbol = Utilities.GetRatewithSymbol(curcode);
            }

            return hrmm;

        }
        private void CreateTables()
        {
            dtBPIadd.Columns.Add("MainImage");
            dtBPIadd.Columns.Add("Logo");
            dtBPIadd.Columns.Add("BasicPropertyInfo_ID");
            dtBPIadd.Columns.Add("AreaID");
            dtBPIadd.Columns.Add("ChainCode");
            dtBPIadd.Columns.Add("GEO_ConfidenceLevel");
            dtBPIadd.Columns.Add("HotelCode");
            dtBPIadd.Columns.Add("HotelCityCode");
            dtBPIadd.Columns.Add("HotelName");
            dtBPIadd.Columns.Add("Latitude");
            dtBPIadd.Columns.Add("Longitude");
            dtBPIadd.Columns.Add("Phone");
            dtBPIadd.Columns.Add("Fax");
            dtBPIadd.Columns.Add("Address");
            dtBPIadd.Columns.Add("Rating");
            dtBPIadd.Columns.Add("RatingValue", typeof(int));
            dtBPIadd.Columns.Add("RateRange_Max");
            dtBPIadd.Columns.Add("RateRange_Maxrate", typeof(decimal));
            dtBPIadd.Columns.Add("CurrencyCode");
            dtBPIadd.Columns.Add("FetAmenities");
            dtBPIadd.Columns.Add("logo");
            dtBPIadd.Columns.Add("image");
            dtBPIadd.Columns.Add("ChainName");
            dtBPIadd.Columns.Add("Fitness");
            dtBPIadd.Columns.Add("Hottub");
            dtBPIadd.Columns.Add("Indpool");
            dtBPIadd.Columns.Add("Internet");
            dtBPIadd.Columns.Add("Breakfast");
            dtBPIadd.Columns.Add("Kitchen");
            dtBPIadd.Columns.Add("Freeparking");
            dtBPIadd.Columns.Add("Nonsmoking");
            dtBPIadd.Columns.Add("accessible");
            dtBPIadd.Columns.Add("pets");
            dtBPIadd.Columns.Add("airport");
            dtBPIadd.Columns.Add("Business");
            dtBPIadd.Columns.Add("Outpool");
            dtBPIadd.Columns.Add("Kids");
        }
        private void FillHStable(XmlNode xnod)
        {
            #region FillHotelListTable
            foreach (XmlNode xn in xnod.ChildNodes)
            {
                DataRow dtr = dtBPIadd.NewRow();
                dtr["MainImage"] = GetChildText(xn, "MainImage");
                dtr["Logo"] = GetChildText(xn, "Logo");
                dtr["BasicPropertyInfo_ID"] = GetChildText(xn, "BasicPropertyInfo_ID");
                dtr["AreaID"] = GetChildText(xn, "AreaID");
                dtr["ChainCode"] = GetChildText(xn, "ChainCode");
                dtr["GEO_ConfidenceLevel"] = GetChildText(xn, "GEO_ConfidenceLevel");
                dtr["HotelCode"] = GetChildText(xn, "HotelCode");
                dtr["HotelCityCode"] = GetChildText(xn, "HotelCityCode");
                dtr["HotelName"] = GetChildText(xn, "HotelName");
                dtr["Latitude"] = GetChildText(xn, "Latitude");
                dtr["Longitude"] = GetChildText(xn, "Longitude");
                dtr["Phone"] = GetChildText(xn, "Phone");
                dtr["Fax"] = GetChildText(xn, "Fax");
                dtr["Address"] = GetChildText(xn, "Address");
                dtr["Rating"] = GetChildText(xn, "Rating");
                dtr["RatingValue"] = GetChildText(xn, "RatingValue");
                dtr["RateRange_Max"] = GetChildText(xn, "RateRange_Max");
                dtr["RateRange_Maxrate"] = GetChildRRMText(xn, "RateRange_Maxrate");
                dtr["CurrencyCode"] = GetChildText(xn, "CurrencyCode");
                dtr["FetAmenities"] = GetChildText(xn, "FetAmenities");
                dtr["logo"] = GetChildText(xn, "logo");
                dtr["image"] = GetChildText(xn, "image");
                dtr["ChainName"] = GetChildText(xn, "ChainName");
                dtr["Fitness"] = GetChildText(xn, "Fitness");
                dtr["Hottub"] = GetChildText(xn, "Hottub");
                dtr["Indpool"] = GetChildText(xn, "Indpool");
                dtr["Internet"] = GetChildText(xn, "Internet");
                dtr["Breakfast"] = GetChildText(xn, "Breakfast");
                dtr["Kitchen"] = GetChildText(xn, "Kitchen");
                dtr["Freeparking"] = GetChildText(xn, "Freeparking");
                dtr["Nonsmoking"] = GetChildText(xn, "Nonsmoking");
                dtr["accessible"] = GetChildText(xn, "accessible");
                dtr["pets"] = GetChildText(xn, "pets");
                dtr["airport"] = GetChildText(xn, "airport");
                dtr["Business"] = GetChildText(xn, "Business");
                dtr["Outpool"] = GetChildText(xn, "Outpool");
                dtr["Kids"] = GetChildText(xn, "Kids");
                dtBPIadd.Rows.Add(dtr);
            }
            #endregion
        }
        private string GetChildText(XmlNode pnode, string node)
        {
            string rvalue = "";
            try
            {

                foreach (XmlNode xn in pnode.ChildNodes)
                {
                    if (xn.Name.ToLower() == node.ToLower())
                    {
                        rvalue = xn.InnerText;
                    }
                }
            }
            catch
            {
            }
            return rvalue;
        }
        private string GetChildRRMText(XmlNode pnode, string node)
        {
            string rvalue = "0";
            try
            {

                foreach (XmlNode xn in pnode.ChildNodes)
                {
                    if (xn.Name.ToLower() == node.ToLower())
                    {
                        rvalue = xn.InnerText;
                    }
                }
            }
            catch
            {
            }
            if (rvalue == "")
            {
                rvalue = "0";
            }
            return rvalue;
        }
        private string GetPricrangecondzero()
        {
            string rvalue = "";

            try
            {
                rvalue = "(RateRange_Maxrate >0)";
            }
            catch { }
            return rvalue;
        }
        private DataTable FilterTable(DataTable dtmain, String Conditon)
        {
            DataTable dtfilter = dtmain.Clone();
            DataRow[] drfilter = dtmain.Select(Conditon);

            foreach (DataRow dr in drfilter)
            {
                object[] row = dr.ItemArray;
                dtfilter.Rows.Add(row);
            }
            return dtfilter;
        }

    }
}