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
/// <summary>
/// Summary description for Hotel
/// </summary>
public class Hotel
{
    public string BasicPropertyInfo_ID { get; set; }
    public string AreaID { get; set; }
    public string ChainCode { get; set; }
    public string GEO_ConfidenceLevel { get; set; }
    public string HotelCode { get; set; }
    public string HotelCityCode { get; set; }
    public string HotelName { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }
    public string Phone { get; set; }
    public string Fax { get; set; }
    public string Address { get; set; }
    public string Rating { get; set; }
    public string FetAmenities { get; set; }
    public string RateRange_Max { get; set; }
    public string RateRange_Min { get; set; }
    public string ChainName { get; set; }
}
public class HACondition
{
    public string City { get; set; }
    public string fullCity { get; set; }
    public string CityName { get; set; }
    public string GuestCount { get; set; }
    public string CheckIn { get; set; }
    public string CheckOut { get; set; }
    public int Rooms { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
    public string Adultsbyroom { get; set; }
    public string Childrenbyroom { get; set; }
    public string childrenage { get; set;}
    public string Rating { get; set; }
    public string HotelName { get; set; }
    public string ChainCode { get; set; }
    public string Address { get; set; }
    public string AwardRating { get; set; }
    public string HotelAmenities { get; set; }
    public string RoomAmenities { get; set; }
    public string PropertyTypes { get; set; }
    public string Packages { get; set; }
    public string RateMax { get; set; }
    public string RateMin { get; set; }
    public string SortBy { get; set; }
    public string CityType { get; set; }
    public string CurrencyCode { get; set; }
    public string Latitude { get; set; }
    public string Longitude { get; set; }

    public HACondition()
    {
        City = "";
        fullCity = "";
        CityName = "";
        GuestCount = "";
        CheckIn = "";
        CheckOut = "";
        Rooms = 0;
        Adults = 0;
        Children = 0;
        Rating = "";
        HotelName = "";
        ChainCode = "";
        Address = "";
        AwardRating = "";
        HotelAmenities = "";
        RoomAmenities = "";
        PropertyTypes = "";
        Packages = "";
        RateMax = "";
        RateMin = "";
        SortBy = "";
        CityType = "CC";

    }
}
public class HPDCondition
{
    public string HotelCode { get; set; }
    public string checkin { get; set; }
    public string checkout { get; set; }
    public string guestcount { get; set; }
    public string Adult { get; set; }
    public int children { get; set; }
    public string childrenage { get; set; }
    public string rooms { get; set; }
    public string CurrencyCode { get; set; }
}
public class CustomerInfo
{
    public string title { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public string MiddleName { get; set; }
    public string Email { get; set; }
    public string Addressline1 { get; set; }
    public string Addressline2 { get; set; }
    public string Phone { get; set; }
    public string PayMethod { get; set; }
    public string CCNumber { get; set; }
    public string CCHName { get; set; }
    public string CCExpDate { get; set; }
    public string Cvnum { get; set; }
    public string Country { get; set; }
    public string Zipcode { get; set; }
}

public class CustomerInfopaymentgt
{
    public string title { get; set; }
    public string Name { get; set; }
    public string SurName { get; set; }
    public string MiddleName { get; set; }
    public string Email { get; set; }
    public string Addressline1 { get; set; }
    public string Addressline2 { get; set; }
    public string Phone { get; set; }
    public string PayMethod { get; set; }
    public string CCNumber { get; set; }
    public string CCHName { get; set; }
    public string CCExpDate { get; set; }
    public string Cvnum { get; set; }
    public string Country { get; set; }
    public string Zipcode { get; set; }
    public string PNR { get; set; }
    public string Totelamount { get; set; }
    public string toteltaxes { get; set; }
    public string city { get; set; }
    public string state { get; set; }
}


public class HotelImages
{
    public Dictionary<string, string> himages = new System.Collections.Generic.Dictionary<string, string>();
    public HotelImages()
    {
        FillHImages();
    }
    private void FillHImages()
    {
        himages.Add("H", "images/htl-img-1.jpg");

    }
}
public class ClsFevicons
{
    public Dictionary<string, string> favicons = new System.Collections.Generic.Dictionary<string, string>();
    public ClsFevicons()
    {
        FillFaiconDictionary();
    }

    private void FillFaiconDictionary()
    {
        DataTable Dtfaicons = HotelDBLayer.GetFaicons();
        foreach (DataRow dr in Dtfaicons.Rows)
        {
            favicons.Add(dr["FaviconDesc"].ToString(), dr["ImageIcon"].ToString());
        }
    }

    //private void FillFaiconDictionary()
    //{
    //    favicons.Add("ADA_Accessible", "fa fa-wheelchair-alt");
    //    favicons.Add("AdultsOnly", "fa fa-fw");
    //    favicons.Add("BeachFront", "fa fa-beach");
    //    favicons.Add("Breakfast", "fa fa-breakfast");
    //    favicons.Add("BusinessCenter", "fa fa-fw");
    //    favicons.Add("BusinessReady", "fa fa-fw");
    //    favicons.Add("Conventions", "fa fa-fw");
    //    favicons.Add("Dataport", "fa fa-fw");
    //    favicons.Add("Dining", "fa fa-fw");
    //    favicons.Add("DryClean", "fa fa-fw");
    //    favicons.Add("EcoCertified", "fa fa-fw");
    //    favicons.Add("ExecutiveFloors", "fa fa-fw");
    //    favicons.Add("FitnessCenter", "fa fa-fw");
    //    favicons.Add("FreeLocalCalls", "fa fa-phone");
    //    favicons.Add("FreeParking", "fa fa-car");
    //    favicons.Add("FreeShuttle", "fa fa-bus");
    //    favicons.Add("FreeWifiInMeetingRooms", "fa fa-wifi");
    //    favicons.Add("FreeWifiInPublicSpaces", "fa fa-wifi");
    //    favicons.Add("FreeWifiInRooms", "fa fa-wifi");
    //    favicons.Add("FullServiceSpa", "fa fa-fw");
    //    favicons.Add("GameFacilities", "fa fa-sports");
    //    favicons.Add("Golf", "fa fa-golf");
    //    favicons.Add("HighSpeedInternet", "fa fa-wifi");
    //    favicons.Add("HypoallergenicRooms", "fa fa-fw");
    //    favicons.Add("IndoorPool", "fa fa-fw");
    //    favicons.Add("InteriorDoorways", "fa fa-fw");
    //    favicons.Add("InRoomCoffeeTea ", "fa fa-fw");
    //    favicons.Add("InRoomMiniBar", "fa fa-fw");
    //    favicons.Add("InRoomRefrigerator", "fa fa-fw");
    //    favicons.Add("InRoomSafe", "fa fa-fw");
    //    favicons.Add("Jacuzzi", "fa fa-fw");
    //    favicons.Add("KidsFacilities", "fa fa-fw");
    //    favicons.Add("KitchenFacilities", "fa fa-fw");
    //    favicons.Add("MealService", "fa fa-fw");
    //    favicons.Add("MeetingFacilities", "fa fa-fw");
    //    favicons.Add("NoAdultTV", "fa fa-fw");
    //    favicons.Add("NonSmoking", "fa fa-fw");
    //    favicons.Add("OutdoorPool", "fa fa-fw");
    //    favicons.Add("Pets", "fa fa-dog");
    //    favicons.Add("Pool", "fa fa-fw");
    //    favicons.Add("PublicTransportationAdjacent", "fa fa-fw");
    //    favicons.Add("RateAssured", "fa fa-fw");
    //    favicons.Add("RestrictedRoomAccess", "fa fa-fw");
    //    favicons.Add("Recreation", "fa fa-fw");
    //    favicons.Add("RoomService", "fa fa-fw");
    //    favicons.Add("RoomService24Hours", "fa fa-fw");
    //    favicons.Add("RoomsWithBalcony", "fa fa-fw");
    //    favicons.Add("SkiInOutProperty", "fa fa-fw");
    //    favicons.Add("SmokeFree", "fa fa-no-smoking");
    //    favicons.Add("SmokingRoomsAvail", "fa fa-smoking");
    //    favicons.Add("Tennis", "fa fa-racket");
    //    favicons.Add("WaterPurificationSystem", "fa fa-fw");
    //    favicons.Add("Wheelchair", "fa fa-wheelchair-alt");
    //}
}
public class HotelImage1
{
    public string logo;
    public string Image;
    public static string result = "";
    public static string xml_rq = "";
    public static string cc_rs = "";
    public static string pcc = "";
    public static string ipcc = "";
    public static string username = "";
    public static string password = "";
    XmlDataDocument xmldoc = new XmlDataDocument();
    public HotelImage1(string Hotelcode)
    {
        string resultxml = GetImageXML(Hotelcode);
        //FileStream fs = new FileStream(@"E:\aravind\BookHotel\BookHotel\BookHotel\HotelXML\hotelsImage_712018_1844-RS.xml", FileMode.Open, FileAccess.Read);
        //xmldoc.Load(fs);
        //fs.Close();
        xmldoc.LoadXml(resultxml);
        XmlNode xnod = xmldoc.DocumentElement;
        XmlNode xheader = xnod.ChildNodes[0];
        XmlNode xbody = xnod.ChildNodes[1];
        XmlNode xGetImageRS = xbody.ChildNodes[0];
        try
        {
            XmlNode xhotellogo = xGetImageRS.ChildNodes[1].ChildNodes[0].ChildNodes[0];
            logo = GetValue(xhotellogo.Attributes["Logo"]);
        }
        catch
        {
            logo = "images/No Image found.png";
        }
        try
        {
            XmlNode xhotelImage = xGetImageRS.ChildNodes[1].ChildNodes[0].ChildNodes[1].ChildNodes[0];
            Image = GetValue(xhotelImage.Attributes["Url"]);
        }
        catch
        {
            Image = "images/No Image found.png";
        }
        //logo = "images/No Image found.png";
        //Image = "images/No Image found.png";

    }

    private string GetValue(XmlAttribute x)
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
    public string GetImageXML(string Hotelcode)
    {
        pcc = "VL5H";
        ipcc = "7A7H"; ;
        username = "373541";
        password = "WS110542";

        string result = "";
        string rq = "";
        result = ContextChange();
        if (result.ToString() != "")
        {
            DataSet ds = new DataSet();
            DataSet dsSession = new DataSet();
            StringReader se_stream = new StringReader(result);
            dsSession.ReadXml(se_stream);
            if (dsSession.Tables["BinarySecurityToken"] != null)
            {
                //DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
                //DataTable dtMessageData = dsSession.Tables["MessageData"];
                //DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
                //string timestamp = DateTime.UtcNow.ToString();

                //rq = "<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:SOAP-ENC='http://schemas.xmlsoap.org/soap/encoding/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>";
                //rq += "<SOAP-ENV:Header>";
                //rq += "<eb:MessageHeader  xmlns:eb='http://www.ebxml.org/namespaces/messageHeader' SOAP-ENV:mustUnderstand='1' eb:version='2.0'>";
                //rq += "<eb:From>";
                //rq += "<eb:PartyId type='urn:x12.org:IO5:01'>WebServiceClient</eb:PartyId>";
                //rq += " </eb:From>";
                //rq += "<eb:To>";
                //rq += " <eb:PartyId type='urn:x12.org:IO5:01'>WebServiceSupplier</eb:PartyId>";
                //rq += "</eb:To>";
                //rq += " <eb:CPAId>" + pcc + "</eb:CPAId>";
                //rq += " <eb:ConversationId>" + dtMessageHeader.Rows[0]["ConversationId"].ToString() + "</eb:ConversationId>";
                ////rq += " <eb:Service eb:type='OTA'>Hotel Shopping Service</eb:Service>";
                //rq += "<eb:Service />";
                //rq += "<eb:Action>GetHotelImageRQ</eb:Action>";
                //rq += "<eb:MessageData>";
                //rq += " <eb:MessageId>" + dtMessageData.Rows[0]["MessageId"].ToString() + "</eb:MessageId>";
                //rq += "<eb:Timestamp>" + dtMessageData.Rows[0]["Timestamp"].ToString() + "</eb:Timestamp>";
                //rq += "</eb:MessageData>";
                //rq += "<eb:RefToMessageId>" + dtMessageData.Rows[0]["ReftoMessageId"].ToString() + "</eb:RefToMessageId> ";
                //rq += "<eb:DuplicateElimination /> ";
                //rq += "</eb:MessageHeader>";
                //rq += "<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext' xmlns:wsu='http://schemas.xmlsoap.org/ws/2002/12/utility'>";
                //rq += "<wsse:BinarySecurityToken valueType='String' EncodingType='wsse:Base64Binary'>" + dtBinarySecurityToken.Rows[0]["BinarySecurityToken_Text"].ToString() + "</wsse:BinarySecurityToken>";
                //rq += "</wsse:Security>";
                //rq += "</SOAP-ENV:Header>";
                //rq += "<SOAP-ENV:Body>";
                //rq += "<GetHotelImageRQ xmlns='http://services.sabre.com/hotel/image/v1' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' version='1.0.0' xsi:schemaLocation='http://services.sabre.com/hotel/image/v1 GetHotelImageRQ.xsd'>";
                //rq += "<HotelRefs>";
                //rq += " <HotelRef HotelCode='" + Hotelcode + "' CodeContext='Sabre'/>";
                //rq += "</HotelRefs>";
                //rq += "<ImageRef Type='MEDIUM'  LanguageCode='EN'/>";
                //// rq += "<ImageRef Type='LARGE'  LanguageCode='EN'/>";
                //rq += "</GetHotelImageRQ>";
                //rq += "</SOAP-ENV:Body>";
                //rq += "</SOAP-ENV:Envelope>";
                //result = SendQuery(rq);
                //SaveXMLFile(rq, result, "hotelsImage_" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
            }

        }
        return result;
    }
    public static string createsession(string searchid)
    {
        string result = "";
        string xml_rq = "";
        xml_rq = "";

        //xml_rq += " <SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:eb='http://www.ebxml.org/namespaces/messageHeader' xmlns:xlink='http://www.w3.org/1999/xlink' xmlns:xsd='http://www.w3.org/1999/XMLSchema'>";
        //xml_rq += " <SOAP-ENV:Header><eb:MessageHeader SOAP-ENV:mustUnderstand='1' eb:version='1.0'>";
        //xml_rq += " <eb:ConversationId>Session</eb:ConversationId>";
        //xml_rq += "  <eb:From> ";
        //xml_rq += " <eb:PartyId type='urn:x12.org:IO5:01'>WebServiceClient</eb:PartyId>";
        //xml_rq += "</eb:From> ";
        //xml_rq += "<eb:To>";
        //xml_rq += " <eb:PartyId type='urn:x12.org:IO5:01'>WebServiceSupplier</eb:PartyId> ";
        //xml_rq += "</eb:To><eb:CPAId>" + ipcc + "</eb:CPAId> ";
        //xml_rq += "<eb:Service eb:type='OTA'>SessionCreateRQ</eb:Service>";
        //xml_rq += "<eb:Action>SessionCreateRQ</eb:Action><eb:MessageData>";
        //xml_rq += "  <eb:MessageId>mid:20001209-133003-2333@clientofsabre.com1</eb:MessageId> ";
        //xml_rq += " <eb:Timestamp>" + DateTime.UtcNow.ToString("s") + "Z</eb:Timestamp>";
        //xml_rq += "<eb:TimeToLive>" + DateTime.UtcNow.ToString("s") + "Z</eb:TimeToLive> ";
        //xml_rq += "</eb:MessageData>";
        //xml_rq += "</eb:MessageHeader>";
        //xml_rq += "<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext' xmlns:wsu='http://schemas.xmlsoap.org/ws/2002/12/utility'>  ";
        //xml_rq += "  <wsse:UsernameToken>       ";
        //xml_rq += "<wsse:Username>" + username + "</wsse:Username>  ";
        //xml_rq += "<wsse:Password>" + password + "</wsse:Password> ";
        //xml_rq += "<Organization>" + ipcc + "</Organization>   ";
        //xml_rq += "<Domain>DEFAULT</Domain>  </wsse:UsernameToken>";
        //xml_rq += "</wsse:Security>";
        //xml_rq += " </SOAP-ENV:Header> ";
        //xml_rq += "<SOAP-ENV:Body>  ";
        //xml_rq += "<eb:Manifest SOAP-ENV:mustUnderstand='1' eb:version='1.0'> ";
        //xml_rq += "  <eb:Reference xmlns:xlink='http://www.w3.org/1999/xlink' xlink:href='cid:rootelement' xlink:type='simple'/>";
        //xml_rq += "</eb:Manifest>";
        //xml_rq += "</SOAP-ENV:Body>";
        //xml_rq += "</SOAP-ENV:Envelope>";
        //result = SendQuery(xml_rq);
        //SaveXMLFile(xml_rq, result, "sessioncreate_" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
        return result;
    }
    public static string ContextChange()
    {
        // string result = "";
        result = createsession("hotels");
        string cc_rq = "";
        DataSet ds = new DataSet();
        DataSet dsSession = new DataSet();
        StringReader se_stream = new StringReader(result);
        dsSession.ReadXml(se_stream);
        if (dsSession.Tables["BinarySecurityToken"] != null)
        {
            //DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
            //DataTable dtMessageData = dsSession.Tables["MessageData"];
            //DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
            //string timestamp = DateTime.UtcNow.ToString();
            //cc_rq = "<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:SOAP-ENC='http://schemas.xmlsoap.org/soap/encoding/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>";
            //cc_rq += "<SOAP-ENV:Header>";
            //cc_rq += "<eb:MessageHeader  xmlns:eb='http://www.ebxml.org/namespaces/messageHeader' SOAP-ENV:mustUnderstand='1' eb:version='2.0'>";
            //cc_rq += "<eb:From>";
            //cc_rq += "<eb:PartyId type='urn:x12.org:IO5:01'>WebServiceClient</eb:PartyId>";
            //cc_rq += " </eb:From>";
            //cc_rq += "<eb:To>";
            //cc_rq += " <eb:PartyId type='urn:x12.org:IO5:01'>WebServiceSupplier</eb:PartyId>";
            //cc_rq += "</eb:To>";
            //cc_rq += " <eb:CPAId>" + pcc + "</eb:CPAId>";
            //cc_rq += " <eb:ConversationId>" + dtMessageHeader.Rows[0]["ConversationId"].ToString() + "</eb:ConversationId>";
            //cc_rq += " <eb:Service eb:type='OTA'>Air Shopping Service</eb:Service>";
            //cc_rq += "<eb:Action>ContextChangeLLSRQ</eb:Action>";
            //cc_rq += "<eb:MessageData>";
            //cc_rq += " <eb:MessageId>" + dtMessageData.Rows[0]["MessageId"].ToString() + "</eb:MessageId>";
            //cc_rq += "<eb:Timestamp>" + dtMessageData.Rows[0]["Timestamp"].ToString() + "</eb:Timestamp>";
            //cc_rq += "</eb:MessageData>";
            //cc_rq += "<eb:RefToMessageId>" + dtMessageData.Rows[0]["ReftoMessageId"].ToString() + "</eb:RefToMessageId> ";
            //cc_rq += "<eb:DuplicateElimination /> ";
            //cc_rq += "</eb:MessageHeader>";
            //cc_rq += "<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext' xmlns:wsu='http://schemas.xmlsoap.org/ws/2002/12/utility'>";
            //cc_rq += "<wsse:BinarySecurityToken valueType='String' EncodingType='wsse:Base64Binary'>" + dtBinarySecurityToken.Rows[0]["BinarySecurityToken_Text"].ToString() + "</wsse:BinarySecurityToken>";
            //cc_rq += "</wsse:Security>";
            //cc_rq += "</SOAP-ENV:Header>";
            //cc_rq += "<SOAP-ENV:Body>";
            //cc_rq += "<ContextChangeRQ xmlns='http://webservices.sabre.com/sabreXML/2011/10' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' Version='2.0.3'>";
            //cc_rq += "<ChangeAAA PseudoCityCode='" + pcc + "' />";
            //cc_rq += " </ContextChangeRQ>";
            //cc_rq += "</SOAP-ENV:Body>";
            //cc_rq += "</SOAP-ENV:Envelope>";
            //result = SendQuery(cc_rq);
            //SaveXMLFile(cc_rq, result, "ContextChange_" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
        }
        return result;
    }
    public static string closession(string mid, string timestamp, string token, string sid)
    {
        string xmlrq = "";
        //xmlrq = " <SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/'  xmlns:eb='http://www.ebxml.org/namespaces/messageHeader'      xmlns:xlink='http://www.w3.org/1999/xlink'  xmlns:xsd='http://www.w3.org/1999/XMLSchema'>";
        //xmlrq += "<SOAP-ENV:Header>";
        //xmlrq += " <eb:MessageHeader SOAP-ENV:mustUnderstand='1' eb:version='2.0'>";
        //xmlrq += "<eb:ConversationId>Session</eb:ConversationId>";
        //xmlrq += "<eb:From>";
        //xmlrq += "<eb:PartyId type='urn:x12.org:IO5:01'>WebServiceClient</eb:PartyId>";
        //xmlrq += "  </eb:From>";
        //xmlrq += "<eb:To>";
        //xmlrq += "<eb:PartyId type='urn:x12.org:IO5:01'>WebServiceSupplier</eb:PartyId>";
        //xmlrq += "</eb:To>";
        //xmlrq += "<eb:CPAId>" + ipcc + "</eb:CPAId>";
        //xmlrq += "<eb:Service eb:type='sabreXML'>Session</eb:Service> ";
        //xmlrq += "<eb:Action>SessionCloseRQ</eb:Action>";
        //xmlrq += "<eb:MessageData>";
        //xmlrq += "<eb:MessageId>" + mid + "</eb:MessageId>";
        //xmlrq += "<eb:Timestamp>" + timestamp + "Z</eb:Timestamp>";
        //xmlrq += "</eb:MessageData>";
        //xmlrq += "</eb:MessageHeader>";
        //xmlrq += "<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext'>";
        //xmlrq += "<wsse:BinarySecurityToken valueType='String' EncodingType='wsse:Base64Binary'>" + token + "</wsse:BinarySecurityToken>";
        //xmlrq += "</wsse:Security>";
        //xmlrq += "</SOAP-ENV:Header> ";
        //xmlrq += "<SOAP-ENV:Body>";
        //xmlrq += "<eb:Manifest SOAP-ENV:mustUnderstand='1' eb:version='2.0'>";
        //xmlrq += "<eb:Reference xmlns:xlink='http://www.w3.org/1999/xlink' xlink:type='simple'/>";
        //xmlrq += "<SessionCloseRQ>";
        //xmlrq += "<POS>";
        //xmlrq += "<Source PseudoCityCode='" + ipcc + "'/>";
        //xmlrq += "</POS>";
        //xmlrq += "</SessionCloseRQ>";
        //xmlrq += "</eb:Manifest>";
        //xmlrq += "</SOAP-ENV:Body>";
        //xmlrq += "</SOAP-ENV:Envelope>";

        //string xmlrs = SendQuery(xmlrq);
        //// SaveXMLFile(xmlrq, xmlrs, "sessionclose" + sid);
        return "";

    }
    public static string SendQuery(String xml_rq)
    {
        string strresultxml = "";
        try
        {
            HttpWebRequest httprequest = (HttpWebRequest)HttpWebRequest.Create("https://sws-crt.cert.havail.sabre.com");//test

            byte[] byteArray = Encoding.UTF8.GetBytes(xml_rq);

            httprequest.Method = "POST";

            httprequest.ContentType = "text/xml; charset=utf-8";

            httprequest.ContentLength = byteArray.Length;

            httprequest.ProtocolVersion = HttpVersion.Version11;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Stream httprq_Stream = httprequest.GetRequestStream();

            httprq_Stream.Write(byteArray, 0, byteArray.Length);

            HttpWebResponse httpresponse = (HttpWebResponse)httprequest.GetResponse();

            Stream httprs_stream = httpresponse.GetResponseStream();

            StreamReader httpsr = new StreamReader(httprs_stream);

            strresultxml = httpsr.ReadToEnd();

            httpsr.Close();
            httprs_stream.Close();

        }
        catch (WebException wex)
        {
            if (wex.Response != null)
            {
                using (var errorResponse = (HttpWebResponse)wex.Response)
                {
                    using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                    {
                        strresultxml = reader.ReadToEnd();
                        //TODO: use JSON.net to parse this string and look at the error message
                    }
                }
            }
        }
        return strresultxml;

    }
    public static void SaveXMLFile(string RQXML, string RSXML, string FileName)
    {
        try
        {
            XmlDocument RQdoc = new XmlDocument();
            RQdoc.LoadXml(RQXML);
            RQdoc.PreserveWhitespace = true;
            string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + FileName + "-RQ.xml");
            RQdoc.Save(filePathRQ);


            XmlDocument RSdoc = new XmlDocument();
            RSdoc.LoadXml(RSXML);
            RSdoc.PreserveWhitespace = true;
            string filePathRS = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + FileName + "-RS.xml");
            RSdoc.Save(filePathRS);

        }
        catch (Exception ex)
        {

        }

    }
}
public class HotelImageJson
{
    public string Logo;
    public string Image;
}
public class HotelImageJsonCode
{
    public string Logo;
    public string Image;
    public string Hotelcode;
}
public class HotelRateMaxMin
{
    public string Max;
    public string Min;
    public string Cursymbol;
}
