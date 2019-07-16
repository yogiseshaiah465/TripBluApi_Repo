using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Globalization;


    public class Utilities
    {
        public static string GetValue(XmlAttribute x)
        {
            string rvalue;
            try
            {
                rvalue = x.Value;
            }
            catch
            {
                rvalue = "";
            }
            return rvalue;

        }
        public static string GetChildText(XmlNode pnode, string node)
        {
            string rvalue = "";
            try
            {
                foreach (XmlNode xn in pnode.ChildNodes)
                {
                    if (xn.Name.ToLower() == node.ToLower())
                    {
                        try
                        {
                            rvalue = xn.InnerText;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
            }
            return rvalue;
        }
        public static string GetRatewithSymbol(String CurrencyCode)
        {
            string rvalue = "";
            //int i = 0;
            //CultureInfo culture = new CultureInfo("en-US");
            if (CurrencyCode == "USD")
            {
               // culture = CultureInfo.CreateSpecificCulture("en-US");
                rvalue = "$";
            }
            else if (CurrencyCode == "CAD")
            {
                //culture = CultureInfo.CreateSpecificCulture("en-CA");
                rvalue = "C$";
            }
            else if (CurrencyCode == "EUR")
            {
               // culture = CultureInfo.CreateSpecificCulture("fr-FR");
                rvalue = "€";
            }
            else if (CurrencyCode == "GBP")
            {
                //culture = CultureInfo.CreateSpecificCulture("en-GB");
                //rvalue = Convert.ToDecimal(i).ToString("C", culture);
                //rvalue = rvalue.Replace("0", "").Replace(",", "").Replace(".", "");

                rvalue = "£";
            }
            // €
            return rvalue;

        }
        public static HACondition GetSearchConditon(string city, string checkin, string checkout, string guestcount, int rooms, int adults,string adultbyroom, int children,string childrenbyroom,string childages, string currencycode)
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
                       // Hac.City = city.Split(',')[0];
                        Hac.CityType = "CN";
                    }
                }
                Hac.fullCity = city.ToString();
                Hac.GuestCount = (adults + children).ToString();
                if (checkin != "") { Hac.CheckIn = Convert.ToDateTime(checkin).ToString("yyyy-MM-dd"); }
                if (checkout != "") { Hac.CheckOut = Convert.ToDateTime(checkout).ToString("yyyy-MM-dd"); }
                Hac.Rooms = rooms;
                Hac.Adults = adults;
                Hac.Adultsbyroom = adultbyroom;
                Hac.Children = children;
                Hac.Childrenbyroom = childrenbyroom;
                Hac.childrenage = childages;
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
