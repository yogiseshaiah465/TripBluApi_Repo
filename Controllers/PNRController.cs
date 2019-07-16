using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TripxolHotelsWebapi.Controllers
{
    public class PNRController : ApiController
    {
        public string Get(string searchid, string srph, string BookingID, string hotelcode, string Addressline1, string Addressline2, string CCExpDate, string CCHName, string CCNumber, string Country, string Cvnum, string Email, string MiddleName, string Name, string PayMethod, string Phone, string SurName, string title, string Zipcode, string CurrencyCode, string b2c_idn, string rooms, string Guestdet)
        {


            string rvalue = "test";
            CustomerInfo ci = new CustomerInfo();
            ci.Addressline1 = Addressline1;
            ci.Addressline2 = Addressline2;
            ci.CCExpDate = CCExpDate;
            ci.CCHName = CCHName;
            ci.CCNumber = CCNumber;
            ci.Country = Country;
            ci.Cvnum = Cvnum;
            ci.Email = Email;
            ci.MiddleName = MiddleName;
            ci.Name = Name;
            ci.PayMethod = PayMethod;
            ci.Phone = Phone;
            ci.SurName = SurName;
            ci.title = title;
            ci.Zipcode = Zipcode;
            PNRDataAj pnrdata = new PNRDataAj(searchid, ci, srph, BookingID, hotelcode, CurrencyCode, b2c_idn, rooms, Guestdet);
            string ItineraryRefId = pnrdata.Hes.ItineraryRefID;
            if (ItineraryRefId != "")
            {
                rvalue = ItineraryRefId;
            }
            else
            {
                rvalue = "";
            }
            return rvalue;
        }
    }
}