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
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Xml.Linq;
using System.Xml;
using HotelDevMTWebapi.Models;
using System.Xml.Serialization;
using System.Security.Cryptography;

namespace TripxolHotelsWebapi.Controllers
{
    public class HotelListSortController : ApiController
    {
        public DataTable dtBPIadd = new DataTable();
        string vcity = "";
        int vpageno = 1;
        int vtotpages = 0;
        string vcurcode = "";
        public static string con = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
        public string Get(string searchid, string city, string checkind, string checkoutd, string guestscount, string selroom, string selaudults, string selchilds, string sortby, string pageno, string starfilter, string amenities, string Hotelname, string Hotelchain, string lprice, string hprice, string curcode, string b2c_idn,string entity_idn)
        {
            AvailabilityRS objAvailabilityRS = new AvailabilityRS();
            vcity = city;
            vpageno = Convert.ToInt16(pageno);
            vcurcode = curcode;
            string rvalue = "";
            // string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelAvail/" + searchid + "_" + vcurcode + "_HotelList.xml");
            // string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelAvail/" + b2c_idn + "_" + searchid + "_" + curcode + "_HotelList.xml");
            string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_" + curcode + "_hotelsAvail-RS.xml"); //HotelXML/" + searchid + "_" + curcode + "_hotelsAvail-RS.xml
            DataSet dsHL = new DataSet();

            if (File.Exists(HLFPath))
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                FileStream fs = new FileStream(HLFPath, FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                fs.Close();
                XmlNode xnod = xmldoc.DocumentElement;
                //HotelListGenerate.CreateTables(dtBPIadd);
                //HotelListGenerate.FillHStable(xnod, dtBPIadd);
                XmlSerializer deserializer = new XmlSerializer(typeof(AvailabilityRS));
                StreamReader reader = new StreamReader(HLFPath);
                objAvailabilityRS = (AvailabilityRS)deserializer.Deserialize(reader);
                string pricerangecond = HotelListGenerate.GetPricrangecondzero();
                //AvailabilityRS dtPricingzero = HotelListGenerate.FilterTable(objAvailabilityRS, pricerangecond);
                int totalhotels = 0;
                if (objAvailabilityRS.Hotels.Hotel.Count >= 350)
                {
                    totalhotels = 350;

                }
                else
                {
                    totalhotels = objAvailabilityRS.Hotels.Hotel.Count;
                
                }
                
                
                    vtotpages = Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(totalhotels) / 20));
               
                AvailabilityRS dtFilterHotels = HotelListGenerate.GetFilteredDataList(objAvailabilityRS, Hotelname, Hotelchain, sortby, starfilter, amenities, lprice, hprice);
                //DataTable dtSortedHotels = HotelListGenerate.GetSortedData(dtFilterHotels, sortby);
                rvalue += GetHotellistPG(dtFilterHotels, searchid, checkind, checkoutd, guestscount, selaudults, selchilds, sortby, totalhotels, curcode, b2c_idn,entity_idn);
            }
            else
            {

            }
            return rvalue;
        }
        private string GetHotellistPG(AvailabilityRS dtHList, string searchid, string checkind, string checkoutd, string guestcount, string selaudults, string selchilds, string sortby, int totalhotels, string currencycode, string b2c_idn,string entity_idn)
        {
            string genstring = "";
            string cityname = "";
            try
            {
                cityname = vcity.Substring(0, vcity.IndexOf(','));
            }
            catch
            {
                cityname = vcity;
            }


            int htcount = 0;
            if (dtHList.Hotels.Hotel.Count() >= 350)
            {
                htcount = 350;
            }
            else
            {
                htcount = dtHList.Hotels.Hotel.Count();
            }



            string hcount = htcount.ToString();

            genstring += "<div id='divhotellist'>";

            //genstring += "<div class='bk-selct-bx'><select id='ddlCurrency' onchange='CCchange()'> ";

            //if (currencycode == "USD")
            //{
            //    genstring += "<option value='USD' selected='selected'>(USD) US dollar</option>";
            //}
            //else
            //{
            //    genstring += "<option value='USD'>(USD) US dollar</option>";
            //}
            //if (currencycode == "CAD")
            //{
            //    genstring += "<option value='CAD' selected='selected'>(CAD) Canadian dollar</option>";
            //}
            //else
            //{
            //    genstring += "<option value='CAD'>(CAD) Canadian dollar</option>";
            //}
            //if (currencycode == "EUR")
            //{
            //    genstring += "<option value='EUR' selected='selected'>(EUR) Euro</option>";
            //}
            //else
            //{
            //    genstring += "<option value='EUR'>(EUR) Euro</option>";
            //}
            //if (currencycode == "GBP")
            //{
            //    genstring += "<option value='GBP' selected='selected'>(GBP) British pound</option>";
            //}
            //else
            //{
            //    genstring += "<option value='GBP'>(GBP) British pound</option>";
            //}

            //genstring += " </select></div>";
            genstring += " <div class='inner-col-right col-md-9'>";

            genstring += " <div class='row'>";
            genstring += "<div class='inn-rht-menu'><div style='display:none' class='rht-menu-map'><i class='fa fa-map-marker' aria-hidden='true'></i><a onclick='ShowAllinMap()'> Map</a> </div>";
            genstring += "<div class='rht-menu-selctbx'><select id='ddlCurrency' onchange='CCchange()' disabled='disabled'> ";

            if (currencycode == "USD")
            {
                genstring += "<option value='USD' selected='selected'>(USD) US dollar</option>";
            }
            else
            {
                genstring += "<option value='USD'>(USD) US dollar</option>";
            }
            //if (currencycode == "CAD")
            //{
            //    genstring += "<option value='CAD' selected='selected'>(CAD) Canadian dollar</option>";
            //}
            //else
            //{
            //    genstring += "<option value='CAD'>(CAD) Canadian dollar</option>";
            //}
            //if (currencycode == "EUR")
            //{
            //    genstring += "<option value='EUR' selected='selected'>(EUR) Euro</option>";
            //}
            //else
            //{
            //    genstring += "<option value='EUR'>(EUR) Euro</option>";
            //}
            //if (currencycode == "GBP")
            //{
            //    genstring += "<option value='GBP' selected='selected'>(GBP) British pound</option>";
            //}
            //else
            //{
            //    genstring += "<option value='GBP'>(GBP) British pound</option>";
            //}
            genstring += " </select></div></div>";
            genstring += "<div class='inn-right-header'> <h2>" + cityname + " - <sapn class='htlcount'>" + hcount + " Hotels Selected of " + totalhotels + " Hotels Found</span>" + "</h2></div>";
            if (hcount == "0")
            {
                genstring += "<div class='resultsp-msg'><span>Please Clear the filters or change filters to get hotels list </span></div>";
            }
            genstring += " <div class='sortpagn'>";
            genstring += HotelListGenerate.GetSortBydd(sortby, hcount);
            genstring += HotelListGenerate.GetPagingHtmlTop(Convert.ToInt32(hcount), vpageno);
            genstring += GetHotellist(dtHList, searchid, checkind, checkoutd, guestcount, selaudults, selchilds, currencycode, b2c_idn,entity_idn);
            genstring += HotelListGenerate.GetPagingHtmlBottom(Convert.ToInt32(hcount), vpageno);
            genstring += "</div>";
            genstring += "<div><div>";
            genstring += "<div><div>";
            genstring += "<div>";
            return genstring;
        }
        public static string Encrypt(string mid)
        {
            string EncryptionKey = "tripxolproj_clients";
            byte[] clearBytes = Encoding.Unicode.GetBytes(mid);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    mid = Convert.ToBase64String(ms.ToArray());
                }
            }
            return mid;
        }
        private string GetHotellist(AvailabilityRS dtHList, string searchid, string checkind, string checkoutd, string guestcount, string selaudults, string selchilds, string currencycode, string b2c_idn,string entity_idn)
        {

                double admarkup = 0.00;
                double adpercmarkup = 0.00;
                double clpercmarkup = 0.00;
                double clmarkup = 0.00;

                double agnpercmarkup = 0.00;
                double agnmarkup = 0.00;
                double agnperdiscount = 0.00;
                double agndiscount = 0.00;

            double finalmarkup = 0.00;
                double finaldiscount = 0.00;
                double adperdiscount = 0.00;
                double clperdiscount = 0.00;
                double addiscount = 0.00;
                double cldiscount = 0.00;
                double baseamount = 0.00;
            string reviewrate = string.Empty;
            string rvalue = "";
            string imagecode = "GEN";
            int order = 2;
            string childage = string.Empty;
            // foreach (DataRow drh in dtHList.Rows)
            //{


            string b2c_idnencript = Encrypt(b2c_idn);


            double dc = 0.0;
            try
            { dc = (Convert.ToDateTime(checkoutd.ToString()) - Convert.ToDateTime(checkind.ToString())).TotalDays; }
            catch { }
            DataTable dssearch = HotelDBLayer.GetSearch(searchid);
            if (dssearch.Rows.Count > 0)
            {
                childage = dssearch.Rows[0]["HB_ChildAge"].ToString();
            }
            HotelDevMTWebapi.Models.Hotel drh = new HotelDevMTWebapi.Models.Hotel();
            string agency_name = "";
            DataTable dtpccdet = manage_data.getpccdetails(b2c_idn);

            if (dtpccdet.Rows.Count > 0)
            {
                agency_name = dtpccdet.Rows[0]["agency_name"].ToString();
            }



            string cmdfacility = "";

            cmdfacility = "select HB_HotelCodes from hotelsearch where searchidn=" + searchid;
            DataTable dthbhotelcds = manage_data.GetDataTable(cmdfacility, manage_data.con);

            DataTable dtffbookingfb = null;
            DataRow[] drimgpath = null;
            if (System.Web.HttpContext.Current.Cache["dtffbookingfb"+searchid] == null)
            {
                string cmdflbkfb = "select HotelCode,Path,ImageTypeCode from HotelImage where HotelCode in (" + dthbhotelcds.Rows[0]["HB_HotelCodes"].ToString() + ") order by HotelCode,Path asc";
                dtffbookingfb = manage_data.GetDataTable(cmdflbkfb, manage_data.flip_conhb);
                System.Web.HttpContext.Current.Cache["dtffbookingfb" + searchid] = dtffbookingfb;
            }
            else
            {
                dtffbookingfb = (DataTable)System.Web.HttpContext.Current.Cache["dtffbookingfb"+searchid];
            }



            for (int i = (vpageno - 1) * 20; i < (vpageno * 20); i++)
            {
                if (i >= dtHList.Hotels.Hotel.Count||i>=350)
                {
                    break;
                }
                drh = dtHList.Hotels.Hotel[i];
                HotelMaincintent objhtl = new HotelMaincintent();
                List<Hdbfacilities> lstHdbfacilities = new List<Hdbfacilities>();
                objhtl = HotelListGenerate.GetHotelContent(vpageno, drh.Code);
                if (objhtl.Name != null)
                {
                   

                    lstHdbfacilities = HotelListGenerate.Gethotelfacilities(drh.Code);
                    //string lengt = objfacility.FacilityCode;
                    string facilitydescr = string.Empty;
                    facilitydescr = HotelListGenerate.GetFacilityList(lstHdbfacilities);
                    //foreach (Hdbfacilities obj in objfacility)
                    //{
                    //    string decrptnlist = "";
                    //    if (!string.IsNullOrEmpty(obj.FacilityCode))
                    //    {
                    //        decrptnlist = HotelListGenerate.getfacidisc(obj.FacilityCode.ToString(), obj.FacilityGroupCode.ToString());
                    //        facilitydescr += "<li>" + decrptnlist + "</li>";
                    //    }
                    //}
                    drimgpath = dtffbookingfb.Select("HotelCode='" + dtHList.Hotels.Hotel[i].Code + "' and ImageTypeCode='" + imagecode + "'");


                    rvalue += "<div class='col-md-12 hotel-details'><div class='row'>";
                    try
                    {
                        rvalue += " <div class='htl-img-blk col-md-4'> <img src='http://photos.hotelbeds.com/giata/" + drimgpath[0]["Path"].ToString() + "' onerror='this.src=&quot;../images/No%20Image%20found.png&quot;' class='img-responsive'  id='HotelImage_" + dtHList.Hotels.Hotel[i].Code + "' /></div>";
                    }
                    catch
                    {
                        rvalue += " <div class='htl-img-blk col-md-4'> <img src='../images/No Image found.png' onerror='this.src=&quot;../images/No%20Image%20found.png&quot;' class='img-responsive'  id='HotelImage_" + dtHList.Hotels.Hotel[i].Code + "' /></div>";
                    }
                    rvalue += "<div class='col-md-8 col-sm-8 col-xs-8 htl-content'> <div class='col-md-8 col-sm-8 col-xs-8 htl-cont-left'>";
                    rvalue += "<div class='htl-header'> <div class='htl-name'>";
                    double rating = 0.0;
                    int starRating = 0;
                    try
                    {
                        //rating = Convert.ToDouble("3.0");
                        string ratingv = drh.CategoryName.ToString();
                       
                        string[] rativ_split = ratingv.Split(' ');
                        if (rativ_split[0].ToString() != "1" && rativ_split[0].ToString() != "2" && rativ_split[0].ToString() != "3" && rativ_split[0].ToString() != "4" && rativ_split[0].ToString() != "5")
                        {
                            //if (ratingv == "WITHOUT OFFICIAL CATEGORY")
                        //{
                            rating = 0.0;
                        }
                        else
                        { 
                             //rativ_split = ratingv.Split(' ');
                            rating = Convert.ToDouble(rativ_split[0].ToString());
                        }

                        starRating = Convert.ToInt32(rating);
                        //if (drh.Reviews != null)
                        //{
                        //    rating = Convert.ToDouble(drh.Reviews.Review.Rate.ToString());
                        //    starRating = Convert.ToInt32(rating);
                        //}
                    }
                    catch
                    {



                        rating = Convert.ToDouble("0.0");
                        starRating = Convert.ToInt32(rating);
                    
                    
                    
                    
                    }
                    rvalue += "<a target='_blank' href='HotelDetails_load.aspx?id=" + searchid + "&HotelCode=" + drh.Code + "&chkin=" + checkind + "&chkout=" + checkoutd + "&gc=" + selaudults + "&award=" + rating.ToString() + "&childAges=" + childage + "&Totalguest=" + guestcount + "&childrens=" + selchilds + "&clid=" + b2c_idnencript + "'>";
                    //if(drh["contractnegcode"].ToString()!="")
                    //{ rvalue += "<h4>" + drh["HotelName"] + "*<span class='star-blk'><img src='../images/" + drh["RatingValue"] + "stars.png' class='img-responsive' /> </span></h4></a>"; }
                    //else
                    //{

                    //}
                    try
                    {
                        if (starRating>0)
                        {
                            rvalue += "<h4>" + drh.Name + "<span class='star-blk'><img src='../images/" + starRating + "stars.png' class='img-responsive' /> </span></h4>";
                        }
                        else
                        {
                            rvalue += "<h4>" + drh.Name + "</h4>";
                        }
                    }
                    catch
                    { 
                    }
                    rvalue += "</a>";

                    rvalue += "</div></div>";
                    rvalue += "<div class='htl-adress'> <p><span class='addressrpblck'><i class='fa fa-map-marker' aria-hidden='true'></i>" + HotelListGenerate.Getaddress(drh.Code) + "</span> |<a href='#' id='btnShow1' data-toggle='modal' data-target='#DivMap' onclick=\"showmap(" + drh.Latitude + "," + drh.Longitude + ",'" + drh.Name + "')\"> Show Map </a></span></p> </div>";

                    rvalue += "<div class='facilities'><ul class='list-inline'>";
                    rvalue += facilitydescr;
                    rvalue += "</ul></div>";
                    //rvalue += " <div class='facilities'>" + facilitydescr + " </div> ";
                    try
                    {
                        if (drh.Reviews != null)
                        {
                            rvalue += " <div class='reviws-blk'> <ul class='list-inline'>";

                            rvalue += "<li class='review-rate'>" + drh.Reviews.Review.Rate + "</li> <li class='reviw-scre'><span>Excellent</span> (" + drh.Reviews.Review.ReviewCount + " reviews)</li> </ul></div>";
                        }
                        //else
                        //{
                        //    rvalue += " <div class='reviws-blk'> <ul class='list-inline'>";

                        //    rvalue += "<li class='review-rate'>3.0</li> <li class='reviw-scre'><span>Excellent</span> (15 reviews)</li> </ul></div>";
                        //}
                    }
                    catch
                    { 
                    
                    }
                    rvalue += "</div>";
                    rvalue += "<div class='col-md-4 col-sm-4 col-xs-4 htl-cont-right'>";
                    baseamount = (Convert.ToDouble(drh.MinRate)/Convert.ToDouble(dc));
                    if (drh.Code.ToString() != "")
                    {

                        DataTable dt = new DataTable();
                        SqlConnection sqlcon = new SqlConnection(con);
                        try
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.Connection = sqlcon;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandText = "p_SreTS_HDR";
                            cmd.Parameters.AddWithValue("@B2C_IDN", b2c_idn);
                            cmd.Parameters.AddWithValue("@Hotelcode", drh.Code.ToString());
                            cmd.Parameters.AddWithValue("@GDS", "HB");
                            cmd.Parameters.AddWithValue("@User_Entity_Idn", entity_idn);
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
                                clmarkup = (((baseamount+ admarkup) / 100.00) * clpercmarkup);
                                cldiscount = (((baseamount+ addiscount) / 100.00) * clperdiscount);

                            }
                            else
                            {
                                clmarkup = 0.00;
                                cldiscount = 0.00;
                            }



                            string agl_Mode = string.Empty;
                            agl_Mode = dt.Rows[0]["Ag_Mode"].ToString();
                            if (agl_Mode == "Fixed")
                            {
                                agnmarkup = Convert.ToDouble(dt.Rows[0]["Ag_Markup"].ToString());
                                agndiscount = Convert.ToDouble(dt.Rows[0]["Ag_Discount"].ToString());
                            }
                            else if (agl_Mode == "Percentage")
                            {
                                agnpercmarkup = Convert.ToDouble(dt.Rows[0]["Ag_Markup"].ToString());
                                agnperdiscount = Convert.ToDouble(dt.Rows[0]["Ag_Discount"].ToString());
                                agnmarkup = (((baseamount + admarkup+clmarkup) / 100.00) * agnpercmarkup);
                                agndiscount = (((baseamount+ addiscount+ cldiscount) / 100.00) * agnperdiscount);

                            }
                            else
                            {
                                agnmarkup = 0.00;
                                agndiscount = 0.00;
                            }




                            finalmarkup = admarkup + clmarkup+ agnmarkup;
                            finaldiscount = addiscount + cldiscount+ agndiscount;
                            baseamount = baseamount + (finalmarkup-finaldiscount);
                         

                        }

                        rvalue += " <h2 class='price-cnt'>" + Utilities.GetRatewithSymbol(currencycode) + Convert.ToDouble(Convert.ToDouble(baseamount)).ToString("0.00") + "*</h2>";//drh.MinRate) / (dc)
                    }
                    else
                    {
                        rvalue += " <h2 class='price-cnt'>" + Utilities.GetRatewithSymbol(currencycode) + Convert.ToDouble(Convert.ToDouble(baseamount)).ToString("0.00") + "</h2>";//drh.MinRate) / (dc)
                    }
                    rvalue += " <p class='srch-pr-nyt'>Per Night</p>";
                    //if (drh.Reviews != null)
                    //{
                    //    rating = Convert.ToDouble(drh.Reviews.Review.Rate.ToString());
                    //}
                    rvalue += "<a target='_blank' href='HotelDetails_load.aspx?id=" + searchid + "&HotelCode=" + drh.Code + "&chkin=" + checkind + "&chkout=" + checkoutd + "&gc=" + guestcount + "&award=" + rating.ToString() + "&childAges=" + childage + "&Currency=" + currencycode +"&clid="+b2c_idnencript+"' class='chocse-rm'>Choose Room</a>";
                    rvalue += " <p class='avail-rms'>";
                    rvalue += "<a class='lnkviewavrooms' style='cursor:pointer;'  onclick=\"RoomDetails('" + drh.Code + "',this,' " + rating.ToString() + "')\">View Available Rooms</a>";
                    rvalue += " </p></div>";
                    rvalue += " <div class='discount col-md-12 col-sm-12 col-xs-12 col-sm-12 col-xs-12'>";
                    rvalue += " <div class='benft'>  <p> <span class='hotl-logo'>";
                    rvalue += " <img src='http://photos.hotelbeds.com/giata/" + HotelListGenerate.getimpagePath(imagecode, drh.Code) + "' onerror='this.src=&quot;../images/No%20Image%20found.png&quot;' id='HotelLogo_" + drh.Code + "'/></span>*Special Discounted Rates  </p> </div>";
                    rvalue += "</div></div> </div>";
                    rvalue += " <div class='availble-rm-blk' id='divavailroom" + drh.Code + "'>";
                    rvalue += " <div class='loaing-bg_img' style='display:none ;width: 100%; margin-top: 20px; text-align: center;'><img src='../images/loader.gif' /><br /> please wait..</div>";
                    rvalue += "</div></div>";
                }
            }
            return rvalue;
        }
    }
}
//private string GetRatewithSymbol(String CurrencyCode)
//{
//    string rvalue = "";
//    int i = 0;
//    CultureInfo culture = new CultureInfo("en-US");
//    if (CurrencyCode == "USD")
//    {
//        culture = CultureInfo.CreateSpecificCulture("en-US");
//    }
//    else if (CurrencyCode == "CAD")
//    {
//        culture = CultureInfo.CreateSpecificCulture("en-CA");
//    }
//    else if (CurrencyCode == "EUR")
//    {
//        culture = CultureInfo.CreateSpecificCulture("fr-FR");
//    }
//    else if (CurrencyCode == "GBP")
//    {
//        culture = CultureInfo.CreateSpecificCulture("en-GB");
//    }
//    // €
//    rvalue = Convert.ToDecimal(i).ToString("C", culture);
//    rvalue = rvalue.Replace("0", "").Replace(",", "").Replace(".", "");
//    return rvalue;

//}