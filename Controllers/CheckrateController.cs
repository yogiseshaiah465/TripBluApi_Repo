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
using System.Linq;
using System.Collections;

namespace HotelDevMTWebapi.Controllers
{
    public class checkresult
    {
        public string amount { get; set; }
        public string Tax { get; set; }
        public string allroomstamount { get; set; }
        public string cancellationamt { get; set; }
        public string cancellationfrom { get; set; }
        public string roomdetailstable { get; set; }

        public string adroommarkup { get; set; }
        public string adroomdiscount { get; set; }
        public string clmarkup { get; set; }
        public string clroomdiscount { get; set; }

        public string Message { get; set; }

    }


    public class CheckrateController : ApiController
    {
        public static string con = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();

        [HttpGet]
        [AllowAnonymous]
        public checkresult Get(string searchid, string hcode, string ratekey, string b2c_idn,string entity_idn)
        {
            double admarkup = 0.00;
            double adroommarkup = 0.00;

            double adpercmarkup = 0.00;
            double clpercmarkup = 0.00;
            double adperdiscount = 0.00;
            double clperdiscount = 0.00;
            double clmarkup = 0.00;
            double finalmarkup = 0.00;
            double finaldiscount = 0.00;
            double addiscount = 0.00;
            double cldiscount = 0.00;
            double baseamount = 0.00;

            double roombaseamount = 0.00;
            double roomamountwithouttax = 0.00;
            double roomamountwithtax = 0.00;

            double adroompercmarkup = 0.00;
            double clroompercmarkup = 0.00;
            double adroomperdiscount = 0.00;
            double clroomperdiscount = 0.00;
            double clroommarkup = 0.00;
            double finalroommarkup = 0.00;
            double finalroomdiscount = 0.00;
            double adroomdiscount = 0.00;
            double clroomdiscount = 0.00;

            double agnpercmarkup = 0.00;
            double agnmarkup = 0.00;
            double agnperdiscount = 0.00;
            double agndiscount = 0.00;

            double rrate;
            double roomtaxprice = 0.00;
            double eachroomavgtaxprice = 0.00;
            double roomcancellationprice = 0.00;
            double troomspricepernightwithmarkup = 0.00;
            double eachroomspernihgtpricewmrk = 0.00;
            string ratetable = string.Empty;
            double allroomsprice = 0.00;
            string filePathRQ = string.Empty;
            ManageHDetAj mhd = new ManageHDetAj();
            string Result = string.Empty;
            AvailabilityRS objAvailabilityRS = new AvailabilityRS();
            CheckRateRS objCheckRateRS = new CheckRateRS();
            List<upselling> lstupselling = new List<upselling>();
            Rooms objRooms = new Rooms();
            Room objupsellingRoom = new Room();
            checkresult objchkres = new checkresult();

            DataTable dssearch = HotelDBLayer.GetSearch(searchid);
            int rooms = Convert.ToInt32(dssearch.Rows[0]["Rooms"]);

            checkrates(searchid, hcode, ratekey, b2c_idn);
            string[] ratekeys = ratekey.Split(',');
            string ratekey_split = ratekeys[0].Substring(ratekeys[0].Length - 4);
            try
            {
                ClsFevicons objfavicons;
                objfavicons = new ClsFevicons();
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                // TextInfo = cultureInfo.TextInfo;
                //string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_propertydesc_" + hotelcode + "_" + CurrencyCode + "-RS.xml");
                filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "Checkrates_tbluexml/" + searchid + "_" + ratekey_split + "_checkrate-RS.xml");

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

                    if (objCheckRateRS.Error == null)
                    {
                        double dc = 0.0;
                        try
                        { dc = (Convert.ToDateTime(objCheckRateRS.Hotel.CheckOut.ToString()) - Convert.ToDateTime(objCheckRateRS.Hotel.CheckIn.ToString())).TotalDays; }
                        catch { }
                        string cancleplFrom = "";
                        string cancleplamount = "";
                        foreach (Room objrooms in objCheckRateRS.Hotel.Rooms.Room)
                        {
                            Rates objrates = new Rates();
                            objrates = objrooms.Rates;
                            List<Rate> lstRate = objrates.Rate;

                            if (lstRate != null && lstRate.Count > 0)
                            {
                                for (int r = 0; r < lstRate.Count; r++)
                                {
                                    // eachroomsprice = Convert.ToDouble(troomsprice / norooms);
                                    rrate = Convert.ToDouble(Convert.ToDouble(Convert.ToDouble(lstRate[r].Net) / dc).ToString("0.00"));

                                    string avgpnignt = Convert.ToDouble(rrate).ToString();

                                    //string avgroompernight = (Convert.ToDouble(avgpnignt) / dc).ToString("0.00");

                                    roombaseamount = Convert.ToDouble(avgpnignt);

                                    DataTable dtwt = new DataTable();
                                    SqlConnection sqlconwt = new SqlConnection(con);
                                    try
                                    {
                                        SqlCommand cmd = new SqlCommand();
                                        cmd.Connection = sqlconwt;
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.CommandText = "p_SreTS_HDR";
                                        cmd.Parameters.AddWithValue("@B2C_IDN", b2c_idn);
                                        cmd.Parameters.AddWithValue("@Hotelcode", hcode);
                                        cmd.Parameters.AddWithValue("@GDS", "HB");
                                        cmd.Parameters.AddWithValue("@User_Entity_Idn", entity_idn);
                                        cmd.Parameters.AddWithValue("@IsLoginCust", "Y");
                                        SqlDataAdapter sa = new SqlDataAdapter(cmd);
                                        sa.Fill(dtwt);
                                    }
                                    catch
                                    {
                                    }

                                    if (dtwt.Rows.Count > 0)
                                    {
                                        string Ts_mode = string.Empty;
                                        Ts_mode = dtwt.Rows[0]["TS_Mode"].ToString();
                                        if (Ts_mode == "Fixed")
                                        {
                                            adroommarkup = Convert.ToDouble(dtwt.Rows[0]["TS_Markup"].ToString());
                                            adroomdiscount = Convert.ToDouble(dtwt.Rows[0]["TS_Discount"].ToString());

                                        }
                                        else if (Ts_mode == "Percentage")
                                        {
                                            adroompercmarkup = Convert.ToDouble(dtwt.Rows[0]["TS_Markup"].ToString());
                                            adroommarkup = ((roombaseamount / 100.00) * adroompercmarkup);

                                            adroomperdiscount = Convert.ToDouble(dtwt.Rows[0]["TS_Discount"].ToString());
                                            adroomdiscount = (((roombaseamount) / 100.00) * adroomperdiscount);
                                        }
                                        else
                                        {
                                            adroommarkup = 0.00;
                                            adroomdiscount = 0.00;

                                        }


                                        string Cl_Mode = string.Empty;
                                        Cl_Mode = dtwt.Rows[0]["Cl_Mode"].ToString();
                                        if (Cl_Mode == "Fixed")
                                        {
                                            clroommarkup = Convert.ToDouble(dtwt.Rows[0]["Cl_Markup"].ToString());
                                            cldiscount = Convert.ToDouble(dtwt.Rows[0]["Cl_Discount"].ToString());

                                        }
                                        else if (Cl_Mode == "Percentage")
                                        {
                                            clroompercmarkup = Convert.ToDouble(dtwt.Rows[0]["Cl_Markup"].ToString());
                                            clroommarkup = (((roombaseamount+ adroommarkup) / (100)) * clroompercmarkup);

                                            clroomperdiscount = Convert.ToDouble(dtwt.Rows[0]["Cl_Discount"].ToString());
                                            clroomdiscount = (((roombaseamount+ adroomdiscount) / 100.00) * clroomperdiscount);
                                        }
                                        else
                                        {
                                            clroommarkup = 0.00;
                                            clroomdiscount = 0.00;
                                        }

                                        string agl_Mode = string.Empty;
                                        agl_Mode = dtwt.Rows[0]["Ag_Mode"].ToString();
                                        if (agl_Mode == "Fixed")
                                        {
                                            agnmarkup = Convert.ToDouble(dtwt.Rows[0]["Ag_Markup"].ToString());
                                            agndiscount = Convert.ToDouble(dtwt.Rows[0]["Ag_Discount"].ToString());
                                        }
                                        else if (agl_Mode == "Percentage")
                                        {
                                            agnpercmarkup = Convert.ToDouble(dtwt.Rows[0]["Ag_Markup"].ToString());
                                            agnperdiscount = Convert.ToDouble(dtwt.Rows[0]["Ag_Discount"].ToString());
                                            agnmarkup = (((baseamount + adroommarkup + clroommarkup) / 100.00) * agnpercmarkup);
                                            agndiscount = (((baseamount + adroomdiscount + clroomdiscount) / 100.00) * agnperdiscount);

                                        }
                                        else
                                        {
                                            agnmarkup = 0.00;
                                            agndiscount = 0.00;
                                        }


                                        finalroommarkup = adroommarkup + clroommarkup+ agnmarkup;
                                        finalroomdiscount = adroomdiscount + clroomdiscount+ agndiscount;
                                        roombaseamount = roombaseamount + (finalroommarkup - finalroomdiscount);
                                        roomamountwithouttax = (Convert.ToDouble(roombaseamount));
                                        // roombaseamount = (Convert.ToDouble(roomamountwithouttax) + Convert.ToDouble(tax));
                                        troomspricepernightwithmarkup = troomspricepernightwithmarkup + (Convert.ToDouble(roomamountwithouttax));
                                        allroomsprice = allroomsprice + Convert.ToDouble(lstRate[r].Net);
                                    }



                                    if (lstRate[r].Taxes != null && lstRate[r].Taxes.Tax.Amount != null)
                                    {

                                        roomtaxprice = roomtaxprice + Convert.ToDouble(lstRate[r].Taxes.Tax.Amount.ToString());

                                    }

                                    try
                                    {
                                        if (lstRate[r].CancellationPolicies != null && lstRate[r].CancellationPolicies.CancellationPolicy.Amount != null)
                                        {
                                            roomcancellationprice = roomcancellationprice + Convert.ToDouble(lstRate[r].CancellationPolicies.CancellationPolicy.Amount.ToString());
                                        }
                                    }
                                    catch
                                    {
                                        //cancleplamount = "NO Cancellation amount";
                                    }


                                }
                                eachroomspernihgtpricewmrk = (troomspricepernightwithmarkup / rooms);
                                eachroomavgtaxprice = Convert.ToDouble(roomtaxprice / rooms);
                                //lstRate.Add(dr.Rates.Rate.Where(n => n.BoardCode == boardCode && (n.ChildrenAges.Contains(',') ? n.ChildrenAges.Split(',')[0].ToString() : n.ChildrenAges) == (childAges.Contains(',') ? childAges.Split(',')[0].ToString() : childAges)).FirstOrDefault());


                                //foreach (Rate drrt in lstRate)
                                //{
                                //var roomamt = rooms[i].Room[i].Rates.Rate[n].Net;

                                string ratetype = lstRate[0].RateType;


                                string ratecommentid = string.Empty;
                                try
                                {
                                    ratecommentid = lstRate[0].RateCommentsId;
                                }
                                catch
                                {
                                }


                                try
                                {
                                    cancleplFrom = lstRate[0].CancellationPolicies.CancellationPolicy.From.ToString();
                                }
                                catch
                                {

                                }
                                try
                                {
                                    cancleplamount = roomcancellationprice.ToString("0.00");//roomcancellationprice.ToString();
                                }
                                catch
                                {
                                    //cancleplamount = "NO Cancellation amount";
                                }
                                ratetable = GetRateTable(hcode, objCheckRateRS, lstRate, hcode, searchid, "USD", adroommarkup, clroommarkup, adroomdiscount, clroomdiscount, roomtaxprice, b2c_idn, rooms,  agnmarkup,agndiscount,entity_idn);//HttpUtility.HtmlEncode(


                            }

                        }

                        //objchkres.amount = Convert.ToDouble(Convert.ToDouble(objCheckRateRS.Hotel.Rooms.Room[0].Rates.Rate[0].Net) / dc).ToString("0.00");

                        objchkres.amount = eachroomspernihgtpricewmrk.ToString("0.00");
                        objchkres.allroomstamount = allroomsprice.ToString("0.00");
                        objchkres.cancellationamt = cancleplamount;
                        objchkres.cancellationfrom = cancleplFrom;
                        objchkres.roomdetailstable = ratetable;
                        objchkres.adroommarkup = adroommarkup.ToString();
                        objchkres.adroomdiscount = adroomdiscount.ToString();
                        objchkres.clmarkup = clroommarkup.ToString();
                        objchkres.clroomdiscount = clroomdiscount.ToString();

                        try
                        {
                            //List<Tax> lsttax = new List<Tax>();
                            //foreach (var lttax in objCheckRateRS.Hotel.Rooms.Room[0].Rates.Rate[0].Taxes.Tax)
                            //{
                            if (objCheckRateRS.Hotel.Rooms.Room[0].Rates.Rate[0].Taxes.Tax.Amount != null)
                            {

                                objchkres.Tax = eachroomavgtaxprice.ToString();//Convert.ToDouble(Convert.ToDouble(objCheckRateRS.Hotel.Rooms.Room[0].Rates.Rate[0].Taxes.Tax.Amount)).ToString("0.00");
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

            string[] ratekeys = ratekey.Split(',');
            //string ratekey_split = ratekey.Substring(ratekey.Length - 4);
            string ratekey_split = ratekeys[0].Substring(ratekeys[0].Length - 4);
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
                int adults = 0;
                string adultsbyroom = string.Empty;
                string ChildrenByRoom = string.Empty;
                int childs = 0;
                int Roomsdb = 0;
                string childages = string.Empty;

                string cmdtxtrkey = "select * from HotelSearch where Searchidn=" + searchid + "";
                DataTable dtsres = manage_data.GetDataTable(cmdtxtrkey, manage_data.con);
                if (dtsres.Rows.Count > 0)
                {
                    Roomsdb = Convert.ToInt32(dtsres.Rows[0]["Rooms"].ToString());
                    adults = Convert.ToInt32(dtsres.Rows[0]["Adults"].ToString());
                    adultsbyroom = dtsres.Rows[0]["HB_AdultsByRoom"].ToString();
                    childs = Convert.ToInt32(dtsres.Rows[0]["Children"].ToString());
                    ChildrenByRoom = dtsres.Rows[0]["HB_ChildrenByRoom"].ToString();
                    childages = dtsres.Rows[0]["HB_ChildAge"].ToString();
                    //room1-child1-childAge1_4,room1-child2-childAge2_6,room2-child1-childAge1_7

                    //ratekey = "20190322|20190323|W|235|425919|DBL.ST|FIT1|RO||1~1~2|4~6|N@440CE2A9A5234A11546798860704AAUK0000031001500020924da2";
                    //20190222|20190223|W|235|168947|DBL.2Q-NM|ID_B2B_26|RO|SF2|1~2~1|5~8|N@665957DF25404AE1546686230830AAUK000000100010001052601b6
                }

                ClsFevicons objfavicons;
                objfavicons = new ClsFevicons();
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                //filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "checkratexml/" + searchid + "_" + CurrencyCode + "_hotelschkrate-RS.xml");

                rq += "<checkRateRQ xmlns='http://www.hotelbeds.com/schemas/messages' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' upselling='False' language='ENG'>";

                //int rc = 0;
                //int cco = 0;
                //int adco = 0;
                //string[] ratekey_split = ratekeys.Split('|');
                //string[] chags_split = childages.Split(',');
                //string[] childbyroom_split = ChildrenByRoom.Split('_');
                //string[] adtbyroom_split = adultsbyroom.Split('_');
                //int chagc = 0;


                rq += "<rooms>";
                //for (int i = 0; i < Roomsdb; i++)
                //{
                //int adts = Convert.ToInt32(adtbyroom_split[i+1].Split('-')[1]);
                //int chds = Convert.ToInt32(childbyroom_split[i + 1].Split('-')[1]);
                //if (i == 0)
                //{
                //    rq += "<room rateKey='" + ratekey + "'/>";
                //    chagc = chds;
                //}
                //else
                //{
                //    string ratekey_new = "";
                //    //string[] roominfo = null;

                //    //if (chds > 0)
                //    //{

                //    //    roominfo = chags_split[i].Split('_');
                //    //}

                //    string roomcont = "1~" + adts + "~" + chds;
                //    string agscont = "";
                //    for (int a = 0; a < chds; a++)
                //    {

                //        agscont += chags_split[chagc].Split('_')[1] + "~";
                //        chagc++;
                //    }
                //    if (chds > 0)
                //    {
                //        ratekey_split[10] = agscont.Remove(agscont.Length - 1);
                //        ratekey_split[9] = roomcont;
                //        for (int r = 0; r <= 11; r++)
                //        {
                //            ratekey_new += ratekey_split[r] + "|";
                //        }

                //        rq += "<room rateKey='" + ratekey_new.Remove(ratekey_new.Length - 1) + "'/>";
                //    }
                //    else
                //    {
                //        ratekey_split[10] = "";
                //        ratekey_split[9] = roomcont;
                //        for (int r = 0; r <= 11; r++)
                //        {
                //            ratekey_new += ratekey_split[r] + "|";
                //        }

                //        rq += "<room rateKey='" + ratekey_new.Remove(ratekey_new.Length - 1) + "'/>";
                //    }
                //}
                foreach (string ratek in ratekey.Split(','))
                {
                    if (ratek != null && ratek != "")
                    {
                        rq += "<room rateKey='" + ratek + "'/>";
                    }
                }

                // }
                rq += "</rooms>";
                rq += "</checkRateRQ>";

            }
            catch
            {


            }
            return rq;

        }
        private string GetDayRate(DataTable dt, int room, string rdate)
        {
            int rm = room - 1;
            string rvalue = "&nbsp";
            DataRow[] dr = dt.Select("expdate >=#" + rdate + "# and  effedate <=#" + rdate + "#");
            int i = 0;
            if (dr.Count() > 1)
            {
                i = dr.Count() - 1;
            }
            if (dr.Count() > 0)
            {
                decimal amount = 0.0M;
                decimal taxes = 0.0M;

                if (dr[0]["amount"].ToString() != "")
                {
                    amount = Convert.ToDecimal(dr[rm]["amount"].ToString());
                }
                if (dr[0]["taxes"].ToString() != "")
                {
                    taxes = Convert.ToDecimal(dr[rm]["taxes"].ToString());
                }
                //Decimal rate = amount + taxes;
                Decimal rate = amount;
                rvalue = Convert.ToDouble(rate.ToString()).ToString("0.00");
            }
            return rvalue;
        }


        private string GetRateTable(string RPH, CheckRateRS hpr, List<Rate> lstRate, string hcode, string searchid, string curcode, double admarkup, double clmarkup, double addiscount, double cldiscount, double roomtaxprice, string b2c_idn, int norooms,double agnmarkup, double agndiscount, string entity_idn)// DataRow dr,
        {

            string rvalue = "";
            List<Rooms> lstRooms = new List<Rooms>();
            Room objRoom = new Room();
            double rrate = 0.00;
            double adroommarkup = 0.00;
            double roombaseamount = 0.00;
            double roomamountwithouttax = 0.00;
            double adroompercmarkup = 0.00;
            double clroompercmarkup = 0.00;
            double adroomperdiscount = 0.00;
            double clroomperdiscount = 0.00;
            double clroommarkup = 0.00;
            double finalroommarkup = 0.00;
            double finalroomdiscount = 0.00;
            double adroomdiscount = 0.00;
            double clroomdiscount = 0.00;
            double agnpercmarkup = 0.00;
            double agnperdiscount = 0.00;


            DataTable dts = HotelDBLayer.GetSearch(searchid);
            DateTime checkindt = Convert.ToDateTime(hpr.Hotel.CheckIn);
            DateTime checkoutdt = Convert.ToDateTime(hpr.Hotel.CheckOut);
            norooms = Convert.ToInt32(dts.Rows[0]["Rooms"].ToString());

            double dc = 0.0;
            try
            { dc = (Convert.ToDateTime(checkoutdt.ToString()) - Convert.ToDateTime(checkindt.ToString())).TotalDays; }
            catch { }

            DateTime startweekdate = checkindt.AddDays(-(int)checkindt.DayOfWeek);
            DateTime endweekdate = checkoutdt.AddDays(-(int)checkoutdt.DayOfWeek).AddDays(6);

            double noofdays = (endweekdate - startweekdate).TotalDays;
            decimal noofweeks = Decimal.Ceiling(Convert.ToDecimal(noofdays) / 7);

            string strtable = "<table width='100%' border='0' cellspacing='0' cellpadding='0' class='week-table-main'>";
            strtable += "<tbody><tr><td>";
            strtable += "<table width='100%' border='0' cellspacing='0' cellpadding='0' class='week-days'>";
            strtable += "<tbody><tr><td>&nbsp;</td><td>Sun</td><td>mon</td><td>tue</td><td>wed</td><td>thu</td><td>fri</td> <td>sat</td> </tr></tbody></table>";
            strtable += "</td></tr><tr><td> <table width='100%' border='0' cellspacing='0' cellpadding='0' class='price-details'><tbody>";

            DataRow[] hpraterangerow = null;
            try
            {

                // hpraterangerow = hpr.HP_RateRange.Select("HotelPricingID='" + hotelpricingrow[0]["HotelPricingID"].ToString() + "'");
            }
            catch
            {

            }

            DataTable dtr = new DataTable();
            dtr.Columns.Add("id");
            dtr.Columns.Add("amount");
            dtr.Columns.Add("taxes");
            dtr.Columns.Add("surcharges");
            dtr.Columns.Add("expdate", typeof(DateTime));
            dtr.Columns.Add("effedate", typeof(DateTime));
            dtr.Columns.Add("hid");


            int r = 1;
            foreach (var rate in lstRate)
            {
                double troomspricepernightwithmarkup = 0.00;



                // eachroomsprice = Convert.ToDouble(troomsprice / norooms);
                rrate = Convert.ToDouble(Convert.ToDouble(Convert.ToDouble(lstRate[r - 1].Net) / dc).ToString("0.00"));

                string avgpnignt = Convert.ToDouble(rrate).ToString();

                //string avgroompernight = (Convert.ToDouble(avgpnignt) / dc).ToString("0.00");

                roombaseamount = Convert.ToDouble(avgpnignt);

                DataTable dtwt = new DataTable();
                SqlConnection sqlconwt = new SqlConnection(con);
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlconwt;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "p_SreTS_HDR";
                    cmd.Parameters.AddWithValue("@B2C_IDN", b2c_idn);
                    cmd.Parameters.AddWithValue("@Hotelcode", hcode);
                    cmd.Parameters.AddWithValue("@GDS", "HB");
                    cmd.Parameters.AddWithValue("@User_Entity_Idn", entity_idn);
                    cmd.Parameters.AddWithValue("@IsLoginCust", "Y");
                    SqlDataAdapter sa = new SqlDataAdapter(cmd);
                    sa.Fill(dtwt);
                }
                catch
                {
                }

                if (dtwt.Rows.Count > 0)
                {

                    string Ts_mode = string.Empty;
                    Ts_mode = dtwt.Rows[0]["TS_Mode"].ToString();
                    if (Ts_mode == "Fixed")
                    {
                        adroommarkup = Convert.ToDouble(dtwt.Rows[0]["TS_Markup"].ToString());
                        adroomdiscount = Convert.ToDouble(dtwt.Rows[0]["TS_Discount"].ToString());

                    }
                    else if (Ts_mode == "Percentage")
                    {
                        adroompercmarkup = Convert.ToDouble(dtwt.Rows[0]["TS_Markup"].ToString());
                        adroommarkup = ((roombaseamount / 100.00) * adroompercmarkup);

                        adroomperdiscount = Convert.ToDouble(dtwt.Rows[0]["TS_Discount"].ToString());
                        adroomdiscount = (((roombaseamount) / 100.00) * adroomperdiscount);
                    }
                    else
                    {
                        adroommarkup = 0.00;
                        adroomdiscount = 0.00;

                    }

                    string Cl_Mode = string.Empty;
                    Cl_Mode = dtwt.Rows[0]["Cl_Mode"].ToString();
                    if (Cl_Mode == "Fixed")
                    {
                        clmarkup = Convert.ToDouble(dtwt.Rows[0]["Cl_Markup"].ToString());
                        cldiscount = Convert.ToDouble(dtwt.Rows[0]["Cl_Discount"].ToString());

                    }
                    else if (Cl_Mode == "Percentage")
                    {
                        clroompercmarkup = Convert.ToDouble(dtwt.Rows[0]["Cl_Markup"].ToString());
                        clroommarkup = (((roombaseamount) / (100)) * clroompercmarkup);

                        clroomperdiscount = Convert.ToDouble(dtwt.Rows[0]["Cl_Discount"].ToString());
                        clroomdiscount = ((roombaseamount / 100.00) * clroomperdiscount);
                    }
                    else
                    {
                        clmarkup = 0.00;

                    }
                    string agl_Mode = string.Empty;
                    agl_Mode = dtwt.Rows[0]["Ag_Mode"].ToString();
                    if (agl_Mode == "Fixed")
                    {
                        agnmarkup = Convert.ToDouble(dtwt.Rows[0]["Ag_Markup"].ToString());
                        agndiscount = Convert.ToDouble(dtwt.Rows[0]["Ag_Discount"].ToString());
                    }
                    else if (agl_Mode == "Percentage")
                    {
                        agnpercmarkup = Convert.ToDouble(dtwt.Rows[0]["Ag_Markup"].ToString());
                        agnperdiscount = Convert.ToDouble(dtwt.Rows[0]["Ag_Discount"].ToString());
                        agnmarkup = (((roombaseamount + adroommarkup + clroommarkup) / 100.00) * agnpercmarkup);
                        agndiscount = (((roombaseamount + adroomdiscount + clroomdiscount) / 100.00) * agnperdiscount);

                    }
                    else
                    {
                        agnmarkup = 0.00;
                        agndiscount = 0.00;
                    }
                    finalroommarkup = adroommarkup + clroommarkup;
                    finalroomdiscount = adroomdiscount + clroomdiscount;
                    roombaseamount = roombaseamount + (finalroommarkup - finalroomdiscount);
                    roomamountwithouttax = (Convert.ToDouble(roombaseamount));
                    // roombaseamount = (Convert.ToDouble(roomamountwithouttax) + Convert.ToDouble(tax));
                    troomspricepernightwithmarkup = troomspricepernightwithmarkup + (Convert.ToDouble(roomamountwithouttax));

                }
                DataRow drp = dtr.NewRow();
                drp[0] = 0;
                drp[1] = troomspricepernightwithmarkup.ToString("0.00"); //roomdescrow[0]["RatePernight"];
                drp[2] = 0;
                drp[3] = 0;
                drp[4] = "2019-" + checkoutdt.ToString("MM-dd");
                drp[5] = "2019-" + checkindt.ToString("MM-dd");
                drp[6] = 1;
                dtr.Rows.Add(drp);
                strtable += "<tr><td>Room" + (r) + "</td>";
                for (int i = 0; i < noofweeks; i++)
                {

                    strtable += "<tr><td>Week" + (i + 1) + "</td>";
                    for (int j = 0; j < 7; j++)
                    {

                        DateTime chkdate = startweekdate.AddDays((7 * i) + j);
                        if (checkoutdt == chkdate)
                        {
                            strtable += "<td>&nbsp;</td>";
                        }
                        else
                        {
                            var perDays = rate.Net.ToString();
                            var perDay = Convert.ToDouble(perDays) / dc;


                            strtable += "<td>" + "$" + GetDayRate(dtr, r, chkdate.ToString("MM-dd-yyyy")) + "</td>";

                        }


                    }
                    strtable += "</tr>";

                }
                r++;
            }
            strtable += "</tbody> </table> </td> </tr> </tbody></table>";

            return strtable;
        }
    }

}



