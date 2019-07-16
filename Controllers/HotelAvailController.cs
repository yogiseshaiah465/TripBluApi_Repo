using System;
using System.Collections.Generic;
using System.Collections;
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
using System.Xml.Serialization;
using HotelDevMTWebapi.Models;

namespace TripxolHotelsWebapi.Controllers
{
    public class HotelAvailController : ApiController
    {
        string vcity = "";
        int vpageno = 1;
        int vtotpages = 0;
        public DataTable dtBPIadd = new DataTable();
        public static Hashtable hsAmentitiesCount = null;

        public string Get(string searchid, string city, string checkind, string checkoutd, string guestscount, string selroom, string selaudults, string selchilds, string sortby, string pageno, string amenities, string stars, string hotelname, string curcode, string b2c_idn)
        {
            AvailabilityRS objAvailabilityRS = new AvailabilityRS();
            vcity = city;

            vpageno = Convert.ToInt16(pageno);
            //  string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelAvail/" + searchid + "_" + curcode + "_HotelList.xml");
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

                //HotelListGenerate.CreateTables(dtBPIadd);
                //HotelListGenerate.FillHStable(xnod, dtBPIadd);//yogi
            }
            else
            {
                HACondition Hac = GetSearchConditon(city, checkind, checkoutd, guestscount, Convert.ToInt32(selroom), Convert.ToInt32(selaudults), Convert.ToInt32(selchilds), curcode);
                HotelSearchAj hs = new HotelSearchAj(Hac, searchid, b2c_idn);
                dtBPIadd = hs.dtBPIadd;
                DataSet ds = new DataSet();
                ds.Tables.Add(dtBPIadd);
                ds.WriteXml(HLFPath);
            }

            string pricerangecond = HotelListGenerate.GetPricrangecondzero();
            AvailabilityRS dtPricingzero = HotelListGenerate.FilterTable(objAvailabilityRS, pricerangecond);
            objAvailabilityRS.Hotels.Hotel = objAvailabilityRS.Hotels.Hotel.OrderBy(k => Convert.ToDouble(k.MinRate)).ToList();
            int totalhotels = 0;
            if (objAvailabilityRS.Hotels.Hotel.Count() >= 350)
            {
                totalhotels = 350;//objAvailabilityRS.Hotels.Hotel.Count();
            }
            else
            {
                totalhotels = objAvailabilityRS.Hotels.Hotel.Count();
            }
            //int totalhotels = dtPricingzero.Rows.Count;

            string rvalue = "";
            if (totalhotels <= 0)
            {
                rvalue = "N";
            }
            else
            {

                vtotpages = Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(totalhotels) / 20));



                AvailabilityRS dtFilterHotels = HotelListGenerate.GetFilteredData(objAvailabilityRS, hotelname, "", sortby, stars, amenities);
                //AvailabilityRS dtSortedHotels = HotelListGenerate.GetSortedData(dtFilterHotels, sortby);
                rvalue = Generatehtml(dtFilterHotels, dtPricingzero, searchid, checkind, checkoutd, guestscount, hotelname, sortby, curcode, b2c_idn, selaudults, selchilds);
            }
            return rvalue;
        }





        private string Generatehtml(AvailabilityRS objAvailabilityRS, AvailabilityRS MainTable, string searchid, string checkind, string checkoutd, string guestscount, string Hotelname, string sortby, string currencycode, string b2c_idn, string seladults, string selchilds)
        {

            hsAmentitiesCount = HotelListGenerate.GetPropinfocount(objAvailabilityRS);
            string lstHotelCodes = string.Join(",", MainTable.Hotels.Hotel.Select(k => k.Code));
            string genstring = "";
            genstring = "<div class='inner-col-left col-md-3' id='divleftfilters'>";
            genstring += "<div class='inner-col-left-inn'>";

            genstring += "<div class='filter-block' id='divfilterblk'>";
            genstring += "<div class='filter-title'><h3>filters</h3><a style='cursor: pointer;display:none' id='lnkclearfils' onclick='ClearFilters()'>clear all</a> </div>";
            genstring += "<div class='all-fltr-sec' id='divltselfils' style='display:none'   ><asp:Literal  ID='ltselfilters'></asp:Literal></div>";
            genstring += " <div class='shw-mre' id='divshowmorefils'><a style='cursor: pointer;display:none'  id='lnkshowmorefils' onclick='showmorefil()' style='display:none'>show more</a></div>";
            genstring += "</div>";

            genstring += "<div class='filter-block'>";
            genstring += "<div class='filter-title'><h4>Price range:</h4> </div>";
            genstring += "<div class='filter-label'><label class='pad-Rten'><span class='lbl padding-8'><input type='text' id='amount' class='inputtextbox' readonly onchange='pricerangechange()'></span>";


            genstring += "<div id='slider-range'></div></label></div> </div>";

            genstring += "<div class='filter-block'>";
            genstring += "<div class='filter-title' onclick=\"show_promos('divhotelfil')\">";
            genstring += "<h4>Hotel Filters</h4><p><i class='fa fa-angle-down' aria-hidden='true' id='adhotelfil'></i></p> </div>";
            genstring += "<div id='divhotelfil'> <div class='hotel-name-blk'>";
            if (!string.IsNullOrEmpty(Hotelname))
            {
                genstring += "<input name='txtHotelname' type='text' id='txthotelname' placeholder='Enter Hotel  Name' value='" + Hotelname + "' class='form-control' >";
            }
            else
            {
                genstring += "<input name='txtHotelname' type='text' id='txthotelname' placeholder='Enter Hotel  Name'  class='form-control' >";
            }

            genstring += "<a  id='srhotelname' style='cursor:pointer' onclick='srhotelnameClick()'>go</a></div>";
            genstring += "<div class='hotel-chain-blk'>";

            genstring += HotelListGenerate.FillHotelChains(objAvailabilityRS);
            genstring += "</div></div></div>";
            genstring += "<div style='display:none' class='filter-block'><div class='filter-title' onclick=\"show_promos('divstar')\"> <h4>Star rating</h4>";
            genstring += "<p><i class='fa fa-angle-down' aria-hidden='true'></i></p></div><div id='divstar'>";
            List<Reviews> lstreviews = new List<Reviews>();
            lstreviews = objAvailabilityRS.Hotels.Hotel.Select(k => k.Reviews).ToList();
            //yogi comented for review checking 07/01/19
           
            //for (int i = 0; i < lstreviews.Count; i++)
            //{
            //    if (lstreviews[i] != null)
            //    {
            //        genstring += HotelListGenerate.GetStarrating("chkrt5", 5, MainTable);
            //        genstring += HotelListGenerate.GetStarrating("chkrt4", 4, MainTable);
            //        genstring += HotelListGenerate.GetStarrating("chkrt3", 3, MainTable);
            //        genstring += HotelListGenerate.GetStarrating("chkrt2", 2, MainTable);
            //        genstring += HotelListGenerate.GetStarrating("chkrt1", 1, MainTable);
            //    }

            //}

            genstring += "</div> </div>";
            genstring += "<div class='filter-block'> <div class='filter-title' onclick=\"show_promos('divfc')\">  <h4>Amenities</h4> <p><i class='fa fa-angle-down' aria-hidden='true'></i></p> </div>";
            genstring += "<div id='divfc'>";

            genstring += HotelListGenerate.GetAmenities("chkfitnessfun", "Fitness", "Fitness Center", "Fitness", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chkpoolfun", "Indpool", "Indoor Pool", "Indpool", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chkinternet", "Internet", "Internet", "Internet", lstHotelCodes, MainTable.Hotels.Hotel);
            //genstring += HotelListGenerate.GetAmenities("chkwi-fi", "Wi-fi", "Wi-fi", "Wi-fi", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chkbreakfast", "Breakfast", "Breakfast included", "Breakfast", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chkfreepark", "Park", "Free Parking", "Park", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chknsmk", "Non Smoking", "Non Smoking", "Non Smoking", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chksmk", "Smoking Room", "Smoking Room", "Smoking Room", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chkAccessible", "Accessible", "Accessible", "Accessible", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chkpets", "Pets", "Pets Allowed", "Pets", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chkairport", "Airport", "Airport Access", "Airport", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chkbusineerec", "Business", "Business Travel", "Business", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chkpoolrec", "Outdoor", "The Outdoor Pool", "Outdoor", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += HotelListGenerate.GetAmenities("chkkidsrec", "Kids", "Kid-friendly Travel", "Kids", lstHotelCodes, MainTable.Hotels.Hotel);
            genstring += "</div></div>";
            genstring += "<div class='clear-fltr-btn'><a href='#'  id='clfilter' name='clfilters' onclick='ClearFilters()'>Clear All Filters</a></div>";
            genstring += "<div class='close-filter'><a href='#' class='cf-apply' >Apply</a><a href='#' class='cf-close' onclick='CloseMobFilters()'>Close</a></div>";
            genstring += "</div></div>";

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
            if (objAvailabilityRS.Hotels.Hotel.Count() >= 350)
            {
                htcount = 350;
            }
            else 
            {
                htcount = objAvailabilityRS.Hotels.Hotel.Count();
            }

            string hcount = htcount.ToString();//(objAvailabilityRS.Hotels.Hotel.Count()).ToString();
            string Maincount = htcount.ToString();//MainTable.Hotels.Hotel.Count().ToString();
            genstring += "<div id='divhotellist'>";
            genstring += " <div class='inner-col-right col-md-9'> <div class='row'>";
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


            genstring += "<div class='inn-rht-menu'><div   style='display:none' class='rht-menu-map'><i class='fa fa-map-marker' aria-hidden='true'></i>< a onclick='ShowAllinMap()'> Map</a> </div>";
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

            genstring += "<div class='inn-right-header'> <h2>" + cityname + " - <sapn class='htlcount'>" + hcount + " Hotels Selected of " + Maincount + " Hotels Found </span>" + "</h2></div>";
            if (hcount == "0")
            {
                genstring += "<div class='resultsp-msg'><span>Please Clear the filters or change filters to get hotels list </span></div>";
            }
            genstring += " <div class='sortpagn'>";
            genstring += HotelListGenerate.GetSortBydd(sortby, hcount);
            genstring += HotelListGenerate.GetPagingHtmlTop(Convert.ToInt32(hcount), vpageno);
            genstring += HotelListGenerate.GetHotellist(objAvailabilityRS, searchid, checkind, checkoutd, guestscount, currencycode, vpageno, b2c_idn, seladults, selchilds);

            genstring += HotelListGenerate.GetPagingHtmlBottom(Convert.ToInt32(hcount), vpageno);

            genstring += "</div>";
            genstring += "<div><div>";
            genstring += "<div><div>";
            genstring += "<div>";
            return genstring;
        }
        private HACondition GetSearchConditon(string city, string checkin, string checkout, string guestcount, int rooms, int adults, int children, string currencycode)
        {
            try
            {
                HACondition Hac = new HACondition();
                if (city != "")
                {
                    if (city.IndexOf('-') > 0)
                    {
                        Hac.City = city.Split('-')[0];
                        Hac.CityType = "CC";
                    }
                    else
                    {
                        Hac.City = city.Split(',')[0];
                        Hac.CityType = "CN";
                    }
                }

                Hac.fullCity = city;
                Hac.GuestCount = (adults + children).ToString();
                if (checkin != "") { Hac.CheckIn = Convert.ToDateTime(checkin).ToString("yyyy-MM-dd"); }
                if (checkout != "") { Hac.CheckOut = Convert.ToDateTime(checkout).ToString("yyyy-MM-dd"); }
                Hac.Rooms = rooms;
                Hac.Adults = adults;
                Hac.Children = children;
                Hac.SortBy = "low_price";
                Hac.CurrencyCode = currencycode;
                return Hac;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}

//private HACondition GetSearchConditon(string city, string checkin, string checkout, string guestcount, int rooms, int adults, int children)
//{
//    try
//    {
//        HACondition Hac = new HACondition();
//        if (city != "") { Hac.City = city.Split('-')[0]; }
//        Hac.GuestCount = (adults + children).ToString();
//        if (checkin != "") { Hac.CheckIn = Convert.ToDateTime(checkin).ToString("MM-dd"); }
//        if (checkout != "") { Hac.CheckOut = Convert.ToDateTime(checkout).ToString("MM-dd"); }
//        Hac.Rooms = rooms;
//        Hac.Adults = adults;
//        Hac.Children = children;
//        Hac.SortBy = "low_price";
//        return Hac;
//    }
//    catch (Exception ex)
//    {
//        throw ex;
//    }
//}