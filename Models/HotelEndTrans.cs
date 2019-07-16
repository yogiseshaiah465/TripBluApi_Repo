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
using HotelDevMTWebapi.Models;


public class HotelEndTrans
{
    XmlDataDocument xmldoc = new XmlDataDocument();
    public string errormsg = "";
    public string ItineraryRefID = "";
    public string transtext = "";
    public HotelEndTrans(string resultxml, string bookingid, string filePathRQ)
    {
        BookingRS objBookingRS = new BookingRS();
        string HBReference = string.Empty;
        string status = string.Empty;
        string vatnumber = string.Empty;
        string Reservationum = string.Empty;
        string Recordlocator = string.Empty;
        string cancellationfrom = string.Empty;
        string cancellationamount = string.Empty;
        string ratecommentsdt = string.Empty;
        string Sup_Name = string.Empty;
        if (File.Exists(filePathRQ))
        {
            XmlDataDocument xmldoc = new XmlDataDocument();
            FileStream fs = new FileStream(filePathRQ, FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);
            fs.Close();
            XmlNode xnod = xmldoc.DocumentElement;
            //HotelListGenerate.CreateTables(dtBPIadd);
            //HotelListGenerate.FillHStable(xnod, dtBPIadd);
            XmlSerializer deserializer = new XmlSerializer(typeof(BookingRS));
            StreamReader reader = new StreamReader(filePathRQ);
            objBookingRS = (BookingRS)deserializer.Deserialize(reader);
            HBReference = objBookingRS.Booking.Reference;
            status = objBookingRS.Booking.Hotel.Rooms.Room.Select(k => k.Status).FirstOrDefault();
            vatnumber = objBookingRS.Booking.Hotel.Supplier.VatNumber;
            Reservationum = objBookingRS.Booking.InvoiceCompany.RegistrationNumber.ToString();
            cancellationfrom = objBookingRS.Booking.Hotel.Rooms.Room[0].Rates.Rate.CancellationPolicies.CancellationPolicy.From;
            cancellationamount = objBookingRS.Booking.Hotel.Rooms.Room[0].Rates.Rate.CancellationPolicies.CancellationPolicy.Amount;

            ratecommentsdt = objBookingRS.Booking.Hotel.Rooms.Room[0].Rates.Rate.RateComments;
            Sup_Name = objBookingRS.Booking.Hotel.Supplier.Name.ToString();

            HotelDBLayer.UpdateHBbookingRef(bookingid, HBReference, status, vatnumber, Reservationum, cancellationfrom, cancellationamount, ratecommentsdt, Sup_Name);


        }
        else
        {

        }
        AddXMLxEndTransRs(HBReference, status);

        //XmlNode xnod = xmldoc.DocumentElement;
        //XmlNode xheader = xnod.ChildNodes[0];
        //XmlNode xbody = xnod.ChildNodes[1];
        //XmlNode xEndTransRs = xbody.ChildNodes[0];
        //XmlNode xAppResult = xEndTransRs.ChildNodes[0];
        //string appresult = Utilities.GetValue(xAppResult.Attributes["status"]);




    }
    private void AddXMLxEndTransRs(string HBReference, string status)
    {

        if (status == "CONFIRMED")
        {
            ItineraryRefID = HBReference;
        }


    }
}