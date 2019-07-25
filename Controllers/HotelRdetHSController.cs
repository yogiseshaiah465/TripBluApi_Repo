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
using HotelDevMTWebapi.Models;
using System.Xml;
using System.Xml.Serialization;
using System.Web.Configuration;

namespace TripxolHotelsWebapi.Controllers
{
    public class HotelPostData
    {
        public string HotelCode { get; set; }
        public string Searchid { get; set; }
    }
    public class HotelRdetHSController : ApiController
    {
        public static string pcc = "";
        public static string ipcc = "";
        public static string username = "";
        public static string password = "";
        public static string result = "";
        public string ContextResult = "";
        TextInfo TextInfo;


        public static string conhb = ConfigurationManager.ConnectionStrings["SqlConnhb"].ToString();
        public static string con = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
        public string Get(string searchid, string hcode, string curcode, string award, string b2c_idn)
        {
            string rvalue = "";


            ManageHDetAj mhd = new ManageHDetAj();
            AvailabilityRS hpr = mhd.GetData(searchid, hcode, curcode, b2c_idn);
            //DataTable tblroominfo = hpr.dtRoomInfo.Clone();

            //DataRow[] drfilter = hpr.dtRoomInfo.Select("CurrencyCode='" + curcode + "'");
            foreach (var ritem in hpr.Hotels.Hotel.Select(k => k.Rooms))
            {
                object[] row = ritem.Room.ToArray();


                //tblroominfo.Rows.Add(row);
            }

            string stravroomdet = "";
            string stravroomdetmore = "";
            string lbav = "lnkviewavrooms" + hcode;
            string lessid = "divavailroomLess" + hcode;
            string moreid = "divavailroomMore" + hcode;
            stravroomdet = "<div class='availble-rm-blk' id='" + lessid + "'>";
            stravroomdet += "<div class='row avail-rm-inn'>";
            stravroomdet += "<div class='avail-rm-header'> <h3 class='avail-rm-hd'>available rooms</h3><a  style='cursor:pointer' onclick=\"CloseAvlRoom('" + lessid + "','" + lbav + "')\"><i class='fas fa-times-circle'></i>close</a></div>";
            stravroomdet += GetRoomdet(hpr, hcode, 3, searchid, curcode, award,b2c_idn);
            stravroomdet += "</div>";


            //if (tblroominfo.Rows.Count > 3)
            //{
            //    stravroomdet += " <div class='view-mre'><a style='cursor:pointer' onclick='showavroomsMore(\"" + lessid + "\",\"" + moreid + "\")'>view more rooms</a></div>";
            //}
            stravroomdet += "</div>";
            stravroomdetmore = "<div class='availble-rm-blk' style='display:none' id='" + moreid + "'>";
            stravroomdetmore += "<div class='row avail-rm-inn'>";
            stravroomdetmore += "<div class='avail-rm-header'> <h3 class='avail-rm-hd'>available rooms</h3><a  style='cursor:pointer' onclick=\"CloseAvlRoom('" + moreid + "','" + lbav + "')\"><i class='fas fa-times-circle'></i>close</a></div>";
            stravroomdetmore += GetRoomdet(hpr, hcode, 3, searchid, curcode, award,b2c_idn);
            stravroomdetmore += "</div>";
            stravroomdetmore += " <div class='view-mre'><a style='cursor:pointer' onclick='showavroomsLess(\"" + lessid + "\",\"" + moreid + "\")'>view less rooms</a></div>";
            stravroomdetmore += "</div>";
            if (hpr.Hotels.Hotel.Count > 0)
            {
                rvalue = stravroomdet + stravroomdetmore;
            }
            else
            {
                string strnoroom = "";
                strnoroom = "<div class='availble-rm-blk' id='" + lessid + "'>";
                strnoroom += "<div class='row avail-rm-inn'>";
                strnoroom += "<div class='avail-rm-header'> <h3 class='avail-rm-hd'>No Rooms Available</h3><a  style='cursor:pointer' onclick=\"CloseAvlRoom('" + lessid + "','" + lbav + "')\"><i class='fas fa-times-circle'></i>close</a></div>";
                stravroomdet += "</div>";
                rvalue = strnoroom;
            }
            return rvalue;

        }
        #region supporing functions
        // private string GetRoomdet(DataTable dtrows, HotelProperty hpr, string hcode, string hdVARsessionHPR,int count,string searchid)
        private string GetRoomdet(AvailabilityRS hpr, string hcode, int count, string searchid, string curcode, string award,string b2c_idn)
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








            int guestcount = 1;


            DataTable dssearch = HotelDBLayer.GetSearch(searchid);

            int rooms = Convert.ToInt32(dssearch.Rows[0]["Rooms"]);
            int adults = Convert.ToInt32(dssearch.Rows[0]["Adults"]);
            int children = Convert.ToInt32(dssearch.Rows[0]["Children"]);
            var childAges1 = dssearch.Rows[0]["HB_ChildAge"].ToString().Split(',');
            string childAges = string.Empty;
            foreach (var item in childAges1)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    if (string.IsNullOrEmpty(childAges))
                        childAges = (item.Split('_')[1]);//+ ','
                    else

                        childAges = childAges + "," + item.Split('_')[1];
                }

            }
           
            guestcount = Convert.ToInt16(dssearch.Rows[0]["Adults"].ToString()) + Convert.ToInt16(dssearch.Rows[0]["Children"].ToString());
            string stravroomdet = "";
            string hotelName = string.Empty;
            int i = 0;
            double rrate;
            string roomdesc;
            List<Rooms> lstRooms = new List<Rooms>();
            Room objRoom = new Room();








            // DataView dtFilteredv = dtrows.DefaultView;
            // dtFilteredv.Sort = "RatePernight"; 
            //DataTable sortedHotels = dtFilteredv.ToTable();

            //for (int m = 0; m < hpr.Hotels.Hotel[].Code.Count(); m++)
            //{
            //hpr.Hotels.Hotel = hpr.Hotels.Hotel.Where(k => k.Code == hcode).ToList();
            //var rooms = hpr.Hotels.Hotel.Where(k => k.Code == hcode).Select(k => k.Rooms).ToList();
            lstRooms = hpr.Hotels.Hotel.Where(k => k.Code == hcode).Select(k => k.Rooms).ToList();
            hotelName = hpr.Hotels.Hotel.Where(k => k.Code == hcode).Select(k => k.Name).FirstOrDefault();
            string checkind = hpr.Hotels.CheckIn.ToString();
            string checkoutd = hpr.Hotels.CheckOut.ToString();


            double dc = 0.0;
            try
            { dc = (Convert.ToDateTime(checkoutd.ToString()) - Convert.ToDateTime(checkind.ToString())).TotalDays; }
            catch { }

            // var getrooms = hpr.Hotels.Hotel[];

            if (lstRooms.Count > 0)
            {
                foreach (Rooms objrooms in lstRooms)
                {

                    List<Room> lstRoom = objrooms.Room;
                    int chkcount = 1;
                    foreach (Room dr in lstRoom)
                    {
                        string roomname = dr.Name.ToString();
                        string rph = lstRooms.Select(k => k.Room.Select(m => m.Code).FirstOrDefault()).ToString();
                        List<Rate> lstRate = new List<Rate>();

                        if (!string.IsNullOrEmpty(dssearch.Rows[0]["Children"].ToString()) && !string.IsNullOrEmpty(childAges))
                        {
                            try
                            {
                                string boardCodes = string.Join(",", dr.Rates.Rate.Select(k => k.BoardCode).Distinct());
                                foreach (string boardCode in boardCodes.Split(','))
                                {
                                    string ch = dr.Rates.Rate[0].Children.ToString();
                                    if (!string.IsNullOrEmpty(ch) && ch != "0")
                                    {
                                        lstRate.Add(dr.Rates.Rate.Where(n => n.BoardCode == boardCode && (n.ChildrenAges.Contains(',') ? n.ChildrenAges.Split(',')[0].ToString() : n.ChildrenAges) == (childAges.Contains(',') ? childAges.Split(',')[0].ToString() : childAges)).FirstOrDefault());
                                    }
                                    else
                                    {
                                        childAges = null; ;
                                        lstRate.Add(dr.Rates.Rate.Where(n => n.BoardCode == boardCode && (n.ChildrenAges) == childAges).FirstOrDefault());
                                    }
                                }

                            }
                            catch(Exception ex)
                            {


                            }
                        }
                        else
                        {
                            string boardCodes = string.Join(",", dr.Rates.Rate.Select(k => k.BoardCode).Distinct());

                            //lstRate = dr.Rates.Rate.ToList();

                            foreach (string boardCode in boardCodes.Split(','))
                            {


                                lstRate.Add(dr.Rates.Rate.Where(n => n.BoardCode == boardCode).FirstOrDefault());

                            }

                        }
                        foreach (Rate drrt in lstRate)
                        {
                            //var roomamt = rooms[i].Room[i].Rates.Rate[n].Net;
                            string ratetype = drrt.RateType;
                            string boardcode = drrt.BoardCode.ToString();
                            string ratekey = drrt.RateKey.ToString();
                            string ratecommentid = string.Empty;
                            try
                            {
                                ratecommentid = drrt.RateCommentsId;
                            }
                            catch
                            {
                            }

                            rph = dr.Code;
                            rrate = Convert.ToDouble(Convert.ToDouble(Convert.ToDouble(drrt.Net) / dc).ToString("0.00"));

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

                                finalroommarkup = adroommarkup + clroommarkup;
                                finalroomdiscount = adroomdiscount + clroomdiscount;
                                roombaseamount = roombaseamount + (finalroommarkup - finalroomdiscount);
                                roomamountwithouttax = (Convert.ToDouble(roombaseamount));
                                // roombaseamount = (Convert.ToDouble(roomamountwithouttax) + Convert.ToDouble(tax));

                            }

                            string cancleplFrom="";
                            string cancleplamount = "";
                            try
                            {
                                cancleplFrom = drrt.CancellationPolicies.CancellationPolicy.From.ToString();
                            }
                            catch 
                            { 
                            
                            }
                            try
                            {
                                cancleplamount = drrt.CancellationPolicies.CancellationPolicy.Amount.ToString();
                            }
                            catch
                            {
                                //cancleplamount = "NO Cancellation amount";
                            }
                             
                            //roomdesc = getrooms.Rooms.Room[0].Name.ToString();
                            // roomdesc = objRoom.Name;
                            stravroomdet += "<div class='available-rooms avl-wht' id='roommblock_" + dr.Code + "_" + boardcode + "'>";
                            string roomdesc1 = "";
                            //try
                            //{
                            //    roomdesc1 = roomdesc.Substring(0, roomdesc.IndexOf(','));
                            //}
                            //catch
                            //{
                            //    roomdesc1 = roomdesc;
                            //}
                            string test = "<h4 class='modal-title'><span>Room Type:</span>" + roomname + " " + drrt.BoardName.ToString() + "</h4>";
                            string roomdetpopup = HttpUtility.HtmlEncode(test);
                            string RDroominfopopup = HttpUtility.HtmlEncode(Getrdroominfopopup(Convert.ToInt32(hcode), dr.Code, drrt.BoardCode, drrt.BoardName.ToString(), searchid, cancleplFrom, cancleplamount,ratecommentid));
                            string ratetable = HttpUtility.HtmlEncode(GetRateTable(dr.Code, hpr, hcode, searchid, curcode, adroommarkup, clroommarkup, adroomdiscount, clroomdiscount));
                            //string ratetable = "";

                            //stravroomdet += "<div class='avl-room-dtls'><p>" + drrt.BoardName.ToString() + "</p><a href='#' data-toggle='modal' data-target='#room-details-pop' onclick='showroomdet(\"" + roomdetpopup + "\",\"" + RDroominfopopup + "\")'>room details</a></div>";
                            stravroomdet += "<div class='avl-room-dtls'><p>" + test + "</p><a href='#' data-toggle='modal' data-target='#room-details-pop' onclick='showroomdet(\"" + roomdetpopup + "\",\"" + ratetable + "\",\"" + RDroominfopopup + "\")'>room details</a></div>";


                            //stravroomdet += "<div class='avl-room-dtls'><p>" + roomdesc1 + "</p><a href='#' data-toggle='modal' data-target='#room-details-pop' onclick='showroomdet(\"" + roomdetpopup + "\",\"" + RDroominfopopup + "\")'>room details</a></div></div>";
                            stravroomdet += "<div class='avl-price-dtls'>";
                            stravroomdet += "<div class='avl-price-inn'><h2>" + Utilities.GetRatewithSymbol(curcode) + roombaseamount.ToString("0.00") + "</h2> <p>per night</p></div>";
                            if (ratetype == "RECHECK")
                            {
                                stravroomdet += "<div class='avl-bknw-btn' id='divbooking_" + chkcount +"_"+hcode+ "'>";
                                stravroomdet += "<a style='cursor:pointer;' id='idRecheck_" + chkcount +"_"+hcode+ "'  class='rmrates-booknw'   onclick='checkrate(\"" + searchid + "\",\"" + hcode + "\",\"" + ratekey + "\",\"" + b2c_idn + "\",\"" + rph + "\",\"" + boardcode + "\",\"" + chkcount + "\")'>Check Rate</a>";//dr["HPTotalAmount"]
                              
                                stravroomdet += "</div>";
                                chkcount++;
                            }
                            else 
                            {
                                stravroomdet += "<div class='avl-bknw-btn'>";
                                //stravroomdet += "<a target='_blank' href='HotelPayment.aspx?Searchid=" + searchid + "&HotelCode=" + hcode + "&RPH=" + dr.Code + "&Boardcode=" + drrt.BoardCode + "&Currency=" + curcode + "&award=" + award + "&checkin=" + checkind + "&checkout=" + checkoutd + "&gc=" + adults +"&children="+children+"'>Book now</a>";

                                //stravroomdet += "<a target='_blank' href='#' onclick='selroom(" + dr.Code + "," + rrate + "," + rrate.ToString().Trim('$') + ")'>Book now</a>";
                                stravroomdet += "<a href='#'   data-toggle='modal'  onclick='selroom(\"" + rph + "\",\"" + boardcode + "\",\"" + roombaseamount + "\",\"" + roombaseamount.ToString().Trim('$') + "\",\"" + hcode.ToString() + "\",\"" + curcode.ToString() + "\")'>Book Now</a>";
                                // stravroomdet += "<div class='rr-pc-rht'><a href='#'  class='rmrates-booknw' data-toggle='modal' data-target='#myContactModal' onclick='selroom(" + dr.Code + "," + rrate + "," + rrate.ToString().Trim('$') + ")'>Book Now</a>";//dr["HPTotalAmount"]

                                stravroomdet += "</div>";
                            }
                        
                            stravroomdet += "</div> ";
                            stravroomdet += "</div>";
                        }
                    }
                }
            }


            //}

            return stravroomdet;
        }

        private string Getroomdetpopup(string RPH, string roomName)
        {
            string rvalue = "";
            string roomtype = "";
            rvalue += "<h4 class='modal-title'><span>Room Type:</span>" + roomtype + "</h4>";
            return rvalue;
        }
        //yogi_commented
        private string Getrdroominfopopup(int hotelCode, string roomCode, string boardcode, string BoardName, string searchid, string cancleplFrom, string cancleplamount, string ratecommentid)
        {
            string rvalue = "";
            //string canpolicy = "Please note you are within cancellation penalty of 1 night/s fee No-show is subjected to 1 night/s fee";//GetVendorMessage("Cancellation", hpr);
           //string canpolicy = GetVendorMessage(cancleplFrom, cancleplamount);

            string canpolicy = string.Empty;
            if (!string.IsNullOrEmpty(cancleplFrom))
            {
                canpolicy = GetCanclecationMessage(cancleplFrom, cancleplamount);//GetVendorMessage("Cancellation", hpr);
            }
            else
            {
                canpolicy = "No cancellation";
            }

            string facilities = GetAllAmenitiestop(hotelCode.ToString(),"", roomCode); //GetVendorMessage("Facilities", hpr).TrimEnd(',');//"break fast";
            string Ratecomments = string.Empty;
            string coment = string.Empty;
            string ratecommentmessage = string.Empty;
            if (!string.IsNullOrEmpty(ratecommentid))
            {
                Ratecomments = Getratecomments(searchid,ratecommentid);
                coment = Ratecomments.Replace(@"\", string.Empty).Replace(@"\", string.Empty);
                ratecommentmessage = coment.Replace("\"", string.Empty).Trim();
            }
            else
            {
                Ratecomments = "No Ratecomments";
            }

            //string bkftdiningpol = GetVendorMessage("Dining", hpr);
            //string bkftdiningpol = "off property: ihop american whataburger american bugers";
            //string Miscpolicies = GetVendorMessage("Policies", hpr);
            // string Miscpolicies = "checkin-1500 checkout -1100 *family policy the property is suitable for children *pet policy - *group policy -10 or more rooms is considered a group see gm </br> for best available rates *commission policy -commission percent - 10 -a 10 percent commission is allowed to agents with an industry valid iata number.</br> exceptions may apply - please see hp. -hotel taxes and service charges -taxes and service charges may apply -please check hrd rate rules display";
            // string Safety = "accessible baths accessible elevators braille elevator hold bar in bathroom";
            //string services = GetVendorMessage("Services", hpr).TrimEnd(',');
            // string services = "guest laundry-continental breakfast-maid service-24 hour front desk-free parking";
            //string roomtype = room.Name;

            rvalue += "<div class='room-infm-blk'><h2>Room Details</h2><p>" + BoardName + "</p></div>";
            rvalue += "<div class='room-infm-blk'><h2>Cancellation Policy</h2><p>" + canpolicy + "</p> </div>";
            if (facilities != "")
            {
                rvalue += "<div class='room-infm-blk'><h2>Facilities</h2><p>" + facilities + "</p></div>";
            }
            rvalue += "<div class='room-infm-blk rmdtls-imp-infrm'>";
            rvalue += "</div>";
            //rvalue += "<div class='room-infm-blk rmdtls-imp-infrm'><h2>Important Information</h2><p>" + Ratecomments + "</p></div>";
            //rvalue += "<div class='imp-infrm-inn'> <h3>Breakfast/Dining Policy</h3><p>" + bkftdiningpol + "</p></div>";
            //rvalue += "<div class='imp-infrm-inn'><h3>Safety</h3> <p>" + Safety + "</p> </div>";
            //rvalue += "<div class='imp-infrm-inn'><h3>Services</h3> <p>" + services + "</p> </div>";
            //rvalue += "<div class='imp-infrm-inn'><h3>Other Policies</h3> <p>" + Miscpolicies + "</p> </div>";
            ////rvalue += "</div>";
            return rvalue;
        }


        private string GetCanclecationMessage(string cancleplFrom, string cancleplamount)//string MsgType, AvailabilityRS hpr in side
        {
            string rvalue = "";
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo = cultureInfo.TextInfo;
            string cancleplFrom1 = cancleplFrom.Replace("T", " To ");

            //DataRow[] drf = hpr.dtVendorMsgs.Select("MsgType='" + MsgType + "'");
            //foreach (DataRow dr in drf)
            //{
            string cancledet = "";

            cancledet += "Free cancellation  before " + cancleplFrom1 + "</br>";
            cancledet += "cancellations & changes made up to 3 Day(s) prior to Check-in time , will be charged for $" + cancleplamount + " in case of no show will be charged for $" + cancleplamount + " ";
            rvalue += cancledet;//TextInfo.ToTitleCase(cancledet.ToString().ToLowerInvariant()) + "";
            //}

            return rvalue.TrimStart().TrimStart('-'); ;

        }

        private string Getratecomments(string searchid,string ratecommentid)//string MsgType, AvailabilityRS hpr in side
        {
            string rvalue = "";
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo = cultureInfo.TextInfo;

            //string ratecommentdt = "";
            //if (!string.IsNullOrEmpty(ratecommentid))
            //{
            //    string[] ratecommentidlst = ratecommentid.Split(' ');

            //    DataTable dsrate = HotelDBLayer.Getratecommentmsg(hfserverid.Value, b2c_idn, b2b_idn);
            //    ratecommentdt = dsrate.Rows[0]["Destination"].ToString();

            //}

            string BaseUrl = WebConfigurationManager.AppSettings["apiurl"].ToString();
            string ratecommentmsg = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(BaseUrl);
            string fullpath = "/api/RatecommentsHB/?searchid=" + searchid + "&Ratecommentdt=" + ratecommentid;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response;
            response = client.GetAsync(fullpath).Result;
            string hrresponse = "";
            if (response.IsSuccessStatusCode)
            {
                ratecommentmsg = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                hrresponse = response.ReasonPhrase;
            }

            rvalue = TextInfo.ToTitleCase(ratecommentmsg.ToString().ToLowerInvariant()) + "";


            return rvalue.TrimStart().TrimStart('-'); ;


        }


        private string GetRateTable(string RPH, AvailabilityRS hpr, string hcode, string searchid, string curcode, double admarkup, double clmarkup, double addiscount, double cldiscount)
        {
            List<Rooms> lstRooms = new List<Rooms>();
            Room objRoom = new Room();
            string rvalue = "";
            DataTable dts = HotelDBLayer.GetSearch(searchid);
            DateTime checkindt = Convert.ToDateTime(dts.Rows[0]["CheckInDt"]);
            DateTime checkoutdt = Convert.ToDateTime(dts.Rows[0]["CheckOutDt"]);

            double dc = 0.0;
            try
            { dc = (Convert.ToDateTime(checkoutdt.ToString()) - Convert.ToDateTime(checkindt.ToString())).TotalDays; }
            catch { }



            DateTime startweekdate = checkindt.AddDays(-(int)checkindt.DayOfWeek);
            DateTime endweekdate = checkoutdt.AddDays(-(int)checkoutdt.DayOfWeek).AddDays(6); ;

            double noofdays = (endweekdate - startweekdate).TotalDays;
            decimal noofweeks = Decimal.Ceiling(Convert.ToDecimal(noofdays) / 7);
            string strtable = "<table width='100%' border='0' cellspacing='0' cellpadding='0' class='week-table-main'>";
            strtable += "<tbody><tr><td>";
            strtable += "<table width='100%' border='0' cellspacing='0' cellpadding='0' class='week-days'>";
            strtable += "<tbody><tr><td>&nbsp;</td><td>Sun</td><td>mon</td><td>tue</td><td>wed</td><td>thu</td><td>fri</td> <td>sat</td> </tr></tbody></table>";
            strtable += "</td></tr><tr><td> <table width='100%' border='0' cellspacing='0' cellpadding='0' class='price-details'><tbody>";
            lstRooms = hpr.Hotels.Hotel.Where(k => k.Currency == curcode && k.Code == hcode).Select(k => k.Rooms).ToList();//ddlCurrency.Value
            objRoom = lstRooms.Select(k => k.Room.Where(j => j.Code == RPH).FirstOrDefault()).FirstOrDefault();

            //DataRow[] roomdescrow = hpr.dtRoomInfo.Select("RPH='" + RPH + "' and CurrencyCode='" + curcode + "'");
            //DataRow[] roomRaterow = hpr.dtRoomRate.Select("RPH='" + RPH + "'");
            //DataRow[] Raterow = hpr.dtRate.Select("RoomRateID='" + roomRaterow[0]["RoomRateID"] + "'");
            //DataRow[] hotelpricingrow = hpr.dtHotelPricing.Select("RRate_ID='" + Raterow[0]["RRate_ID"].ToString() + "'");

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
            int hprrrowcount = 0;
            if (hpraterangerow != null)
            {
                hprrrowcount = hpraterangerow.Count();
            }

            if (hprrrowcount > 0)
            {
                foreach (DataRow drr in hpraterangerow)
                {
                    DataRow drp = dtr.NewRow();
                    drp[0] = drr[0];
                    drp[1] = drr[1];
                    drp[2] = drr[2];
                    drp[3] = drr[3];
                    drp[4] = "2019-" + drr[4].ToString();
                    drp[5] = "2019-" + drr[5].ToString();
                    drp[6] = drr[6];
                    dtr.Rows.Add(drp);
                }
            }
            else
            {
                double tax = 0.00;
                if (objRoom.Rates.Rate[0].Taxes != null && objRoom.Rates.Rate[0].Taxes.Tax.Amount != null)
                {
                    tax = Convert.ToDouble(objRoom.Rates.Rate[0].Taxes.Tax.Amount.ToString());
                }
                string pernightamount = Convert.ToDouble(Convert.ToDouble(objRoom.Rates.Rate[0].Net.ToString()) / dc).ToString("0.00");
                double pernightamtwithmarkup = (Convert.ToDouble(pernightamount) + ((admarkup + clmarkup) - (addiscount + cldiscount)));
                DataRow drp = dtr.NewRow();
                drp[0] = 0;
                drp[1] = pernightamtwithmarkup.ToString("0.00"); //roomdescrow[0]["RatePernight"];
                drp[2] = 0;
                drp[3] = 0;
                drp[4] = "2019-" + checkoutdt.ToString("MM-dd");
                drp[5] = "2019-" + checkindt.ToString("MM-dd");
                drp[6] = 1;
                dtr.Rows.Add(drp);
            }

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
                        strtable += "<td>"+"$" + GetDayRate(dtr, chkdate.ToString("MM-dd-yyyy")) + "</td>";
                    }
                }
                strtable += "</tr>";

            }
            strtable += "</tbody> </table> </td> </tr> </tbody></table>";

            return strtable;
        }

       


        private string GetAllAmenitiestop(string hotelcode, string type = "", string roomCode = "")
        {
            int i = 0;
            string rvalue = "<tr>";
            DataTable dtffbookingfb = new DataTable();
            string cmdflbkfb = string.Empty;
            roomCode = "";
            try
            {
                if (string.IsNullOrEmpty(roomCode))
                {
                    //cmdflbkfb = "select F.facilityDesc from HotelFacility HF " +
                    //                   "inner join Facility F on HF.FacilityCode = F.FacilityCode and HF.FacilityGroupCode = F.FacilityGroupCode" + " " + "Where HF.hotelCode = " + hotelcode;


                    //cmdflbkfb = "select STUFF ( ( SELECT ', '+F.facilityDesc from HotelFacility HF inner join Facility F on HF.FacilityCode = F.FacilityCode and HF.FacilityGroupCode = F.FacilityGroupCode Where HF.hotelCode = "+hotelcode+" FOR XML PATH(''),TYPE).value('.','VARCHAR(MAX)') , 1,1,SPACE(0)) facilityDesc";


                    cmdflbkfb = "select distinct stuff((select distinct ', ' + F.facilityDesc from HotelFacility HF inner join Facility F on HF.FacilityCode = F.FacilityCode and HF.FacilityGroupCode = F.FacilityGroupCode Where HF.hotelCode = " + hotelcode + " for xml path('')),1,1,'') as facilityDesc from HotelFacility";

                }
                else
                {


                    cmdflbkfb = "select FacilityDesc from Facility F" +
                                "inner join HotelRoomFacility HRF on F.FacilityCode = HRF.RoomFacilityCode" +
                                 "and F.facilityGroupCode = HRF.RoomFacilityGroupCode" + " " + " Where HRF.RoomCode = " + roomCode + " and HRF.HotelCode=" + hotelcode;
                }

                DataTable dt = new DataTable();
                dt = manage_data.GetDataTable(cmdflbkfb, conhb);
                if (string.IsNullOrEmpty(type))//&& string.IsNullOrEmpty(roomCode))
                {
                    rvalue = "<tr>";
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (i % 4 == 0)
                                rvalue += "</tr><tr>";
                            rvalue += "<td width='25%'>" + dr["facilityDesc"].ToString() + "</td>";
                            i++;
                        }
                    }
                    rvalue += "</tr>";
                }
                else if (!string.IsNullOrEmpty(type))
                {
                    rvalue = dt.Rows.Count.ToString();
                }
                else if (!string.IsNullOrEmpty(roomCode))
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            if (i == 5)
                            {
                                rvalue = rvalue + "," + item["FacilityDesc"].ToString() + "</br>";
                                i = 0;
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(rvalue))
                                    rvalue = item["FacilityDesc"].ToString();
                                else
                                    rvalue = rvalue + "," + item["FacilityDesc"].ToString();
                            }
                            i++;
                        }
                    }
                }

            }
            catch
            {

            }
            return rvalue;
        }




        private string GetDayRate(DataTable dt, string rdate)
        {
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
                    amount = Convert.ToDecimal(dr[0]["amount"].ToString());
                }
                if (dr[0]["taxes"].ToString() != "")
                {
                    taxes = Convert.ToDecimal(dr[0]["taxes"].ToString());
                }
                Decimal rate = amount + taxes;
                rvalue =  Convert.ToDouble(rate.ToString()).ToString("0.00");
            }
            return rvalue;
        }

        #endregion
    }
}