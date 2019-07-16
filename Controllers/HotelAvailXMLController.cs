using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.IO;
using System.Web;

namespace TripxolHotelsWebapi.Controllers
{
    public class HotelAvailXMLController : ApiController
    {
        public string Get(string searchid, string curcode, string b2c_idn)
        {
            string rvalue = "N";
            try
            {
                string checkind = "";
                string checkoutd = "";
                string selroom = "";
                string seladults = "";
                string selchildren = "";
                string seladultsbyroom= "";
                string selchildrenbyroom= "";

                string city = "";
                int guestcount = 1;
                string childrenages = "";

                DataTable dssearch = HotelDBLayer.GetSearch(searchid);
                city = dssearch.Rows[0]["Destination"].ToString();
                checkind = Convert.ToDateTime(dssearch.Rows[0]["CheckInDt"]).ToString("yyyy-MM-dd");
                checkoutd = Convert.ToDateTime(dssearch.Rows[0]["CheckOutDt"]).ToString("yyyy-MM-dd");
                selroom = dssearch.Rows[0]["Rooms"].ToString();
                seladults = dssearch.Rows[0]["Adults"].ToString();
                seladultsbyroom = dssearch.Rows[0]["HB_AdultsByRoom"].ToString();

                selchildren = dssearch.Rows[0]["Children"].ToString();
                selchildrenbyroom = dssearch.Rows[0]["HB_ChildrenByRoom"].ToString();
                guestcount = Convert.ToInt16(dssearch.Rows[0]["Adults"].ToString()) + Convert.ToInt16(dssearch.Rows[0]["Children"].ToString());
                childrenages = dssearch.Rows[0]["HB_ChildAge"].ToString();
                HACondition Hac = GetSearchConditon(city, checkind, checkoutd, guestcount.ToString(), Convert.ToInt32(selroom), Convert.ToInt32(seladults), seladultsbyroom, Convert.ToInt32(selchildren), selchildrenbyroom, childrenages, curcode.Trim());
                HotelSearchAj hs = new HotelSearchAj(Hac, searchid, b2c_idn);
                DataTable dtHList = hs.dtBPIadd;
                DataSet ds = new DataSet();
                ds.Tables.Add(dtHList);
                // string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelAvail/" + searchid + "_" + curcode + "_HotelList.xml");//yogi
                //string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_" + curcode + "_hotelsAvail-RS");
                //ds.WriteXml(HLFPath);


                rvalue = "Y";
            }
            catch (Exception ex)
            {
                rvalue = "N";
            }
            return rvalue;
        }
        private HACondition GetSearchConditon(string city, string checkin, string checkout, string guestcount, int rooms, int adults,string adultsbyroom,int children, string childbyroom,string childrenages, string currencycode)
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
                        DataTable dtll = HotelDBLayer.GetLatLong(city);
                        string[] cityparts = city.Split(',');
                        string CityP = cityparts[0];
                        //string State = cityparts[1];
                        string Country = cityparts[1];

                        Hac.CityName = CityP;
                        Hac.fullCity = city;
                        Hac.Latitude = dtll.Rows[0]["Latitude"].ToString();
                        Hac.Longitude = dtll.Rows[0]["Longitude"].ToString();
                        if (dtll.Rows[0]["CityCode"] == null || dtll.Rows[0]["CityCode"].ToString().Trim() == "" && CityP == "" || CityP==null)
                        {
                            
                            Hac.CityType = "LL"; 
                        }
                        else
                        {
                            Hac.CityType = "CN";
                        }

                    }
                }
                Hac.fullCity = city.ToString();
                Hac.GuestCount = (adults + children).ToString();
                if (!string.IsNullOrEmpty(checkin)) { Hac.CheckIn = Convert.ToDateTime(checkin).ToString("yyyy-MM-dd"); }
                if (!string.IsNullOrEmpty(checkout)) { Hac.CheckOut = Convert.ToDateTime(checkout).ToString("yyyy-MM-dd"); }
                Hac.Rooms = rooms;
                Hac.Adults = adults;
                Hac.Adultsbyroom = adultsbyroom;
                Hac.Children = children;
                Hac.Childrenbyroom = childbyroom;
                Hac.SortBy = "low_price";
                Hac.CurrencyCode = currencycode;
                Hac.childrenage = childrenages;
               
                return Hac;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

}