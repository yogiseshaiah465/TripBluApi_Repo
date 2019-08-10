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
using System.Configuration;

public class HotelSearchAj
{
    public HotelSearchAj(string XMLResult, string searchid, string b2c_idn)
    {
        GenerateHS(XMLResult, searchid, b2c_idn);
    }

    public HotelSearchAj(HACondition hac, string searchid, string b2c_idn)
    {
        FillFaiconDictionary();
        dtChainCode = manage_data.GetDatable_SP("p_CodeHotelChain", manage_data.flip_con);
        string triptype = string.Empty;
        string sc = GetRqcond(hac, b2c_idn, searchid, triptype = "");

        string sc1 = GetRqcond(hac, b2c_idn, searchid, triptype = "HOTELBEDS");
        string result = "";
        result = GetHotels(sc, sc1, searchid, hac.CurrencyCode, b2c_idn);
        XMLResult = result;
        GenerateHS(XMLResult, searchid, b2c_idn);
    }
    public static string result = "";
    public static string xml_rq = "";
    public static string cc_rs = "";
    public static string pcc = "";
    public static string ipcc = "";
    public static string username = "";
    public static string password = "";

    Dictionary<string, string> faicons = new System.Collections.Generic.Dictionary<string, string>();
    Dictionary<string, string> Himages = new System.Collections.Generic.Dictionary<string, string>();
    Dictionary<string, string> HLogos = new System.Collections.Generic.Dictionary<string, string>();
    public DataTable dtChainCode = new DataTable();
    #region ReadXML
    private string GetHotels(string sc, string sc1, string searchid, string CurrencyCode, string b2c_idn)
    {
        string result1 = string.Empty;
        string result2 = string.Empty;
        string rq = "";

        XMLRead.GetPccDetails(b2c_idn);

        string htlAvuri = ConfigurationManager.AppSettings["HotelPortalUri"] != null ? ConfigurationManager.AppSettings["HotelPortalUri"].ToString() : string.Empty;
        if (!string.IsNullOrEmpty(htlAvuri))
        {
            result1 = XMLRead.SendQuery(sc, htlAvuri);
            result2 = XMLRead.SendQuery(sc1, htlAvuri);
            XMLRead.SaveXMLFilettype(sc, result1, searchid + "_" + CurrencyCode.Trim() + "_hotelsAvail_Nottype");
            XMLRead.SaveXMLFilettype(sc1, result2, searchid + "_" + CurrencyCode.Trim() + "_hotelsAvail_HBttype");



            string HLFPathNottype = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML_Ttype/" + searchid + "_" + CurrencyCode.Trim() + "_hotelsAvail_Nottype-RS.xml");
            string HLFPathHbttype = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML_Ttype/" + searchid + "_" + CurrencyCode.Trim() + "_hotelsAvail_HBttype-RS.xml");

            AvailabilityRS objAvailabilityRS1 = new AvailabilityRS();

            AvailabilityRS objAvailabilityRS2 = new AvailabilityRS();

            if (File.Exists(HLFPathHbttype))
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                //xmldoc.LoadXml(HLFPath);
                FileStream fs = new FileStream(HLFPathHbttype, FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                fs.Close();
                XmlNode xnod = xmldoc.DocumentElement;




                XmlDocument doc = new XmlDocument();
                XmlSerializer deserializer = new XmlSerializer(typeof(AvailabilityRS));
                StreamReader reader = new StreamReader(HLFPathHbttype);
                objAvailabilityRS1 = (AvailabilityRS)deserializer.Deserialize(reader);


            }

            if (File.Exists(HLFPathNottype))
            {
                XmlDataDocument xmldoc = new XmlDataDocument();
                //xmldoc.LoadXml(HLFPath);
                FileStream fs = new FileStream(HLFPathNottype, FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                fs.Close();
                XmlNode xnod = xmldoc.DocumentElement;


                XmlDocument doc = new XmlDocument();
                XmlSerializer deserializer = new XmlSerializer(typeof(AvailabilityRS));
                StreamReader reader = new StreamReader(HLFPathNottype);
                objAvailabilityRS2 = (AvailabilityRS)deserializer.Deserialize(reader);

            }

            result = createxml(objAvailabilityRS1, objAvailabilityRS2);

            XMLRead.SaveXMLFile(sc, result, searchid + "_" + CurrencyCode.Trim() + "_hotelsAvail");


        }

        return result;
    }
    #endregion


    #region create New Hotelbeds XML
    public static string createxml(AvailabilityRS objAvailabilityRS1, AvailabilityRS objAvailabilityRS2)
    {
        double minrate = 0.00;
        double maxrate = 0.00;
        string result = string.Empty;

        string xml = string.Empty;

        List<Hotel> lstHotel = new List<Hotel>();

        minrate = Convert.ToDouble(objAvailabilityRS2.Hotels.Hotel.Min(k => Convert.ToDouble(k.MinRate)).ToString());
        maxrate = Convert.ToDouble(objAvailabilityRS2.Hotels.Hotel.Max(k => Convert.ToDouble(k.MinRate)).ToString());
        string vlprice = minrate.ToString();
        string vhprice = maxrate.ToString();
        if (!string.IsNullOrEmpty(vlprice) && !string.IsNullOrEmpty(vhprice))
        {

            if (objAvailabilityRS2.Hotels.Hotel != null && objAvailabilityRS2.Hotels.Hotel.Count > 0)
            {


                //_obj.Hotels.Hotel = _obj.Hotels.Hotel.Where(k => Convert.ToDouble(Convert.ToDouble(k.MinRate)/(dc)) >= Convert.ToDouble(vlprice)
                //       && Convert.ToDouble(Convert.ToDouble(k.MinRate)/dc) <= Convert.ToDouble(vhprice)).ToList();


                objAvailabilityRS2.Hotels.Hotel = objAvailabilityRS2.Hotels.Hotel.Where(k => Convert.ToDouble(k.MinRate) >= Convert.ToDouble(vlprice)
                        && Convert.ToDouble(k.MaxRate) <= Convert.ToDouble(vhprice)).ToList();
            }
        }


        try
        {
            string ttlhotels = objAvailabilityRS2.Hotels.Total;
            int hotellist = Convert.ToInt32(ttlhotels);

            xml += "<?xml version='1.0' encoding='UTF-8'?>";
            xml += "<availabilityRS xmlns='http://www.hotelbeds.com/schemas/messages' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' schemaLocation='http://www.hotelbeds.com/schemas/messages'>";
            xml += "<auditData processTime='" + objAvailabilityRS2.AuditData.ProcessTime + "' timestamp='" + objAvailabilityRS2.AuditData.Timestamp + "' requestHost='" + objAvailabilityRS2.AuditData.RequestHost + "' serverId='" + objAvailabilityRS2.AuditData.ServerId + "' environment='" + objAvailabilityRS2.AuditData.Environment + "' release='" + objAvailabilityRS2.AuditData.Release + "' token='" + objAvailabilityRS2.AuditData.Token + "' internal='" + objAvailabilityRS2.AuditData.Internal + "'/>";
            if (hotellist > 300)
            {
                xml += "<hotels checkIn='" + objAvailabilityRS2.Hotels.CheckIn + "' total='300' checkOut='" + objAvailabilityRS2.Hotels.CheckOut + "'>";

                int j = 0;
                foreach (var item in objAvailabilityRS2.Hotels.Hotel)
                {
                    try
                    {
                        xml += "<hotel code='" + item.Code + "' name='" + item.Name.Replace("&", "").Replace("'", "") + "' categoryCode='" + item.CategoryCode.Replace("'", "") + "' categoryName='" + item.CategoryName.Replace("'", "") + "' destinationCode='" + item.DestinationCode.Replace("'", "") + "' destinationName='" + item.DestinationName.Replace("'", "") + "' zoneCode='" + item.ZoneCode + "' zoneName='" + item.ZoneName.Replace("'", "") + "' latitude='" + item.Latitude + "' longitude='" + item.Longitude + "' minRate='" + item.MinRate + "' maxRate='" + item.MaxRate + "' currency='" + item.Currency + "'>";
                        foreach (var itemrev in objAvailabilityRS1.Hotels.Hotel)
                        {
                            if (itemrev.Code == item.Code)
                            {
                                xml += "<reviews>";
                                xml += "<review rate='" + itemrev.Reviews.Review.Rate + "' reviewCount='" + itemrev.Reviews.Review.ReviewCount + "' type='" + itemrev.Reviews.Review.Type + "'/>";
                                xml += "</reviews>";
                            }
                        }
                        xml += "<rooms>";
                        foreach (var itemrooms in item.Rooms.Room)
                        {
                            xml += "<room code='" + itemrooms.Code + "' name='" + itemrooms.Name.Replace("'", "").Replace("&", "") + "'>";
                            xml += "<rates>";
                            foreach (var itemrates in itemrooms.Rates.Rate)
                            {

                                if (itemrates.ChildrenAges != null)
                                {

                                    xml += "<rate rateKey='" + itemrates.RateKey + "' rateClass='" + itemrates.RateClass + "' rateType='" + itemrates.RateType + "' net='" + itemrates.Net + "' allotment='" + itemrates.Allotment + "' paymentType='" + itemrates.PaymentType + "' packaging='" + itemrates.Packaging + "' boardCode='" + itemrates.BoardCode + "' boardName='" + itemrates.BoardName + "' rooms='" + itemrates.Rooms + "' adults='" + itemrates.Adults + "' children='" + itemrates.Children + "' childrenAges='" + itemrates.ChildrenAges + "'>";
                                }
                                else
                                {
                                    xml += "<rate rateKey='" + itemrates.RateKey + "' rateClass='" + itemrates.RateClass + "' rateType='" + itemrates.RateType + "' net='" + itemrates.Net + "' allotment='" + itemrates.Allotment + "' paymentType='" + itemrates.PaymentType + "' packaging='" + itemrates.Packaging + "' boardCode='" + itemrates.BoardCode + "' boardName='" + itemrates.BoardName + "' rooms='" + itemrates.Rooms + "' adults='" + itemrates.Adults + "' children='" + itemrates.Children + "'>";
                                }
                                if (itemrates.CancellationPolicies != null)
                                {
                                    xml += "<cancellationPolicies>";
                                    xml += "<cancellationPolicy amount='" + itemrates.CancellationPolicies.CancellationPolicy.Amount + "' from='" + itemrates.CancellationPolicies.CancellationPolicy.From + "' />";
                                    xml += "</cancellationPolicies>";
                                }
                                if (itemrates.Taxes != null)
                                {
                                    //List<Tax> lsttax = new List<Tax>();
                                    xml += "<taxes allIncluded='" + itemrates.Taxes.AllIncluded + "'>";
                                    //foreach (var lttax in itemrates.Taxes.Tax)
                                    //{

                                        xml += "<tax included='" + itemrates.Taxes.Tax.Included + "' amount='" + itemrates.Taxes.Tax.Amount + "' currency='" + itemrates.Taxes.Tax.Currency + "' type='" + itemrates.Taxes.Tax.type + "' />";
                                    //}
                                    xml += "</taxes>";
                                }
                                xml += "</rate>";

                            }
                            xml += "</rates>";
                            xml += "</room> ";
                        }
                        xml += "</rooms>";
                        xml += "</hotel>";
                        j++;
                        if (j == 300)
                            break;
                    }
                    catch (Exception ex)
                    {

                    }
                }


                xml += "</hotels>";
            }
            else
            {
                xml += "<hotels checkIn='" + objAvailabilityRS2.Hotels.CheckIn + "' total='" + hotellist + "' checkOut='" + objAvailabilityRS2.Hotels.CheckOut + "'>";
                foreach (var item in objAvailabilityRS2.Hotels.Hotel)
                {
                    xml += "<hotel code='" + item.Code + "' name='" + item.Name.Replace("&", "").Replace("'", "") + "' categoryCode='" + item.CategoryCode.Replace("'", "") + "' categoryName='" + item.CategoryName.Replace("'", "") + "' destinationCode='" + item.DestinationCode.Replace("'", "") + "' destinationName='" + item.DestinationName.Replace("'", "") + "' zoneCode='" + item.ZoneCode + "' zoneName='" + item.ZoneName.Replace("'", "") + "' latitude='" + item.Latitude + "' longitude='" + item.Longitude + "' minRate='" + item.MinRate + "' maxRate='" + item.MaxRate + "' currency='" + item.Currency + "'>";
                    foreach (var itemrev in objAvailabilityRS1.Hotels.Hotel)
                    {
                        if (itemrev.Code == item.Code)
                        {
                            xml += "<reviews>";
                            xml += "<review rate='" + itemrev.Reviews.Review.Rate + "' reviewCount='" + itemrev.Reviews.Review.ReviewCount + "' type='" + itemrev.Reviews.Review.Type + "'/>";
                            xml += "</reviews>";
                        }
                    }
                    xml += "<rooms>";
                    foreach (var itemrooms in item.Rooms.Room)
                    {
                        xml += "<room code='" + itemrooms.Code + "' name='" + itemrooms.Name.Replace("'", "").Replace("&", "") + "'>";
                        xml += "<rates>";
                        foreach (var itemrates in itemrooms.Rates.Rate)
                        {
                            if (itemrates.ChildrenAges != null)
                            {
                               
                                xml += "<rate rateKey='" + itemrates.RateKey + "' rateClass='" + itemrates.RateClass + "' rateType='" + itemrates.RateType + "' net='" + itemrates.Net + "' allotment='" + itemrates.Allotment + "' paymentType='" + itemrates.PaymentType + "' packaging='" + itemrates.Packaging + "' boardCode='" + itemrates.BoardCode + "' boardName='" + itemrates.BoardName + "' rooms='" + itemrates.Rooms + "' adults='" + itemrates.Adults + "' children='" + itemrates.Children + "' childrenAges='" + itemrates.ChildrenAges + "'>";
                            }
                            else
                            {
                                xml += "<rate rateKey='" + itemrates.RateKey + "' rateClass='" + itemrates.RateClass + "' rateType='" + itemrates.RateType + "' net='" + itemrates.Net + "' allotment='" + itemrates.Allotment + "' paymentType='" + itemrates.PaymentType + "' packaging='" + itemrates.Packaging + "' boardCode='" + itemrates.BoardCode + "' boardName='" + itemrates.BoardName + "' rooms='" + itemrates.Rooms + "' adults='" + itemrates.Adults + "' children='" + itemrates.Children + "'>";
                            }
                            if (itemrates.CancellationPolicies != null)
                            {
                                xml += "<cancellationPolicies>";
                                xml += "<cancellationPolicy amount='" + itemrates.CancellationPolicies.CancellationPolicy.Amount + "' from='" + itemrates.CancellationPolicies.CancellationPolicy.From + "' />";
                                xml += "</cancellationPolicies>";
                            }
                            if (itemrates.Taxes != null)
                            {

                                //List<Tax> lsttax = new List<Tax>();
                                //itemrates.Taxes.Tax= itemrates.Taxes.Tax.Where(t => t.Included == "true").ToList();
                                xml += "<taxes allIncluded='" + itemrates.Taxes.AllIncluded + "'>";                               
                                //foreach (var lttax in itemrates.Taxes.Tax)
                                //{
                                    
                                        xml += "<tax included='" + itemrates.Taxes.Tax.Included + "' amount='" + itemrates.Taxes.Tax.Amount + "' currency='" + itemrates.Taxes.Tax.Currency + "' type='" + itemrates.Taxes.Tax.type + "' />";
                                   
                                //}
                                xml += "</taxes>";

                            }
                            xml += "</rate>";
                        }
                        xml += "</rates>";
                        xml += "</room> ";
                    }
                    xml += "</rooms>";
                    xml += "</hotel>";
                }

                xml += "</hotels>";
            }


            xml += "</availabilityRS>";


        }
        catch (Exception ex)
        {

        }





        return xml;

    }
    #endregion end create Hotelbeds XML

    public DataTable dtOTA_HotelAvail = new DataTable();
    public DataTable dtCityList = new DataTable();
    public DataTable dtBasicPropInfo = new DataTable();
    public DataTable dtAddressLines = new DataTable();
    public DataTable dtAvaNegClientID = new DataTable();
    public DataTable dtAvaContClientID = new DataTable();
    public DataTable dtProperty = new DataTable();
    public DataTable dtPropOptInfo = new DataTable();
    public DataTable dtLocDescText = new DataTable();
    public DataTable dtRoomRate = new DataTable();
    public DataTable dtRateTypeCode = new DataTable();
    public DataTable dtBPIadd = new DataTable();
    public DataTable dtHotelBed = new DataTable();
    public string ContextResult = "";
    public string XMLResult = "";


    public string errormsg = "";
    public List<Hotel> hotels;
    DataTable Dtfaicons;
    private static string GetRq(DataTable dtBinarySecurityToken, DataTable dtMessageData, DataTable dtMessageHeader, string sc)
    {
        #region rq
        string rq = "";
        rq = "<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:SOAP-ENC='http://schemas.xmlsoap.org/soap/encoding/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>";
        rq += "<SOAP-ENV:Header>";
        rq += "<eb:MessageHeader  xmlns:eb='http://www.ebxml.org/namespaces/messageHeader' SOAP-ENV:mustUnderstand='1' eb:version='2.0'>";
        rq += "<eb:From>";
        rq += "<eb:PartyId type='urn:x12.org:IO5:01'>WebServiceClient</eb:PartyId>";
        rq += " </eb:From>";
        rq += "<eb:To>";
        rq += " <eb:PartyId type='urn:x12.org:IO5:01'>WebServiceSupplier</eb:PartyId>";
        rq += "</eb:To>";
        rq += " <eb:CPAId>" + pcc + "</eb:CPAId>";
        rq += " <eb:ConversationId>" + dtMessageHeader.Rows[0]["ConversationId"].ToString() + "</eb:ConversationId>";
        rq += " <eb:Service eb:type='OTA'>Air Shopping Service</eb:Service>";
        rq += "<eb:Action>OTA_HotelAvailLLSRQ</eb:Action>";
        rq += "<eb:MessageData>";
        rq += " <eb:MessageId>" + dtMessageData.Rows[0]["MessageId"].ToString() + "</eb:MessageId>";
        rq += "<eb:Timestamp>" + dtMessageData.Rows[0]["Timestamp"].ToString() + "</eb:Timestamp>";
        rq += "</eb:MessageData>";
        rq += "<eb:RefToMessageId>" + dtMessageData.Rows[0]["ReftoMessageId"].ToString() + "</eb:RefToMessageId> ";
        rq += "<eb:DuplicateElimination /> ";
        rq += "</eb:MessageHeader>";
        rq += "<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext' xmlns:wsu='http://schemas.xmlsoap.org/ws/2002/12/utility'>";
        rq += "<wsse:BinarySecurityToken valueType='String' EncodingType='wsse:Base64Binary'>" + dtBinarySecurityToken.Rows[0]["BinarySecurityToken_Text"].ToString() + "</wsse:BinarySecurityToken>";
        rq += "</wsse:Security>";
        rq += "</SOAP-ENV:Header>";
        rq += sc;
        rq += "</SOAP-ENV:Envelope>";

        return rq;
        #endregion
    }

    private void FillHImages()
    {
        Himages.Add("0026602", "images/embassysuites.jpg");
        Himages.Add("0008977", "images/holidayinnexp.jpg");
        Himages.Add("0009415", "images/doubletree.jpg");
        Himages.Add("0028536", "images/No Image found.png");
        Himages.Add("0040824", "images/haytthouse.jpg");
        Himages.Add("0052205", "images/hawthornsuites.jpg");
        Himages.Add("0030522", "images/highland.jpg");
        Himages.Add("0013033", "images/comfortsuites.jpg");

        Himages.Add("H", "images/htl-img-1.jpg");

        HLogos.Add("EMBASSY STES DFW INTL ARPT", "images/logo_empty.png");
        HLogos.Add("HOLIDAY INN EXP SUITES FRISCO", "images/logo_empty.png");
        HLogos.Add("DOUBLETREE DALLAS RICHARDSON", "images/logo_dou.png");
        HLogos.Add("HOTEL LUMEN - A KIMPTON HOTEL", "images/logo_empty.png");
        HLogos.Add("HYATT HOUSE RICHARDSON", "images/logo_hyh.png");
        HLogos.Add("HAWTHORN SUITES IRVING DFW", "images/logo_empty.png");
        HLogos.Add("THE HIGHLAND DALLAS A CURIO", "images/logo_cur.png");
        HLogos.Add("COMFORT SUITES DFW N/GRAPEVINE", "images/logo_empty.png");
        HLogos.Add("H", "images/logo_empty.png");

    }
    private void FillFaiconDictionary()
    {
        Dtfaicons = HotelDBLayer.GetFaicons();
        foreach (DataRow dr in Dtfaicons.Rows)
        {
            faicons.Add(dr["FaviconDesc"].ToString(), dr["ImageIcon"].ToString());
        }
    }
    private static string GetRqcond(HACondition hac, string b2c_idn, string searchid, string reviewtype)
    {
        string city = hac.City;

        string citytype = hac.CityType;
        string CityName = hac.CityName;
        string citynamedestcd = string.Empty;
        if (city.Contains(',') && CityName == "")
        {
            string[] cnm = city.Split(',');
            CityName = cnm[0].ToString();
            city = "";

        }
        string fullcity = string.Empty;
        string query_city = "";

        string counry = hac.fullCity.Substring(hac.fullCity.Length - 2);
        if (hac.fullCity.Contains("-"))
        {
            try
            {
                fullcity = hac.fullCity.ToString();

                try
                {
                    string[] citynamedestcd_split = fullcity.Split(',');
                    citynamedestcd = citynamedestcd_split[1].ToString();
                }
                catch
                {

                }

                string[] cityname_split = fullcity.Split('-');

                //string hhh=cityname_split[1].Replace("'","");

                if (!cityname_split[0].Contains(','))
                {
                    query_city = "'" + cityname_split[0] + "'";

                    string[] fullcitytwo_split = cityname_split[1].Replace("'", "").Split(',');


                    for (int i = 0; i < fullcitytwo_split.Length; i++)
                    {
                        query_city += ",'" + fullcitytwo_split[i].Trim() + "'";
                    }
                }


            }
            catch
            {
                query_city = city;
            }
        }
        string guestcount = hac.GuestCount;
        string checkin = hac.CheckIn;
        string checkout = hac.CheckOut;
        string adulbyr = string.Empty;
        string childbyr = string.Empty;
        //string checkin = "2018-08-15";
        //string checkout = "2018-08-16";

        string rooms = hac.Rooms.ToString();
        string Awardrating = hac.AwardRating;
        string HotelName = hac.HotelName;
        string chaincode = hac.ChainCode;
        string RateMax = hac.RateMax;
        string RateMin = hac.RateMin;
        string CurrencyCode = hac.CurrencyCode;
        string ad = string.Empty;
        int adults = 0;
        int child = 0;
        int childage = 0;
        string cd = string.Empty;
        string cdag = string.Empty;
        string rq = "";
        int adco = 0;
        int cdco = 0;
        int cdagco = 0;

        #region makerq
        string CustomerCriteria = "";
        string GuestCountCriteria = "";
        string HotelSearchCriteria = "";
        //string AddressCriteria = "";
        string AwardCriteria = "";
        string HotelAmenityCriteria = "";
        string TimeSpanCriteria = "";
        // string HotelFeatureCriteria = "";
        string HotelRefCriteria = "";
        string PropertyTypeCriteria = "";
        string RoomAmenityCriteria = "";
        string RatePlanCandiCriteria = "";
        string RateRangeCriteria = "";
        string Longitude = "";
        string Latitude = "";
        int count = 1;
        try
        {
            Latitude = Convert.ToDecimal(hac.Latitude).ToString("#.0000");
        }
        catch
        {
        }
        try
        {
            Longitude = Convert.ToDecimal(hac.Longitude).ToString("#.0000");
        }
        catch
        {
        }
        //string addressCriteria = "";

        if (Awardrating == "5") { AwardCriteria += "<Award Provider='NTM' Rating='5'/>"; }
        if (Awardrating == "4") { AwardCriteria += "<Award Provider='NTM' Rating='4'/>"; }
        if (Awardrating == "3") { AwardCriteria += "<Award Provider='NTM' Rating='3'/>"; }
        if (Awardrating == "2") { AwardCriteria += "<Award Provider='NTM' Rating='2'/>"; }
        if (Awardrating == "1") { AwardCriteria += "<Award Provider='NTM' Rating='1'/>"; }

        if (!string.IsNullOrEmpty(hac.PropertyTypes))
        {
            string ptstrim = hac.PropertyTypes.Trim(',');
            string[] pts = ptstrim.Split(',');
            foreach (string pt in pts)
            {
                PropertyTypeCriteria += "<PropertyType>" + pt + "</PropertyType>";
            }
        }

        if (!string.IsNullOrEmpty(guestcount))
        {
            GuestCountCriteria = "<GuestCounts Count='" + guestcount + "' />";
        }
        //if (corporateID != "")
        //{
        //    Customercriteria += "</Customer><Corporate><ID> " + corporateID + "</ID></Corporate></Customer>";
        //}
        if (!string.IsNullOrEmpty(checkin))
        {
            TimeSpanCriteria = "<TimeSpan End='" + checkout + "' Start='" + checkin + "' />";
        }

        //if (city != "" )
        //{
        if (citytype == "CC")
        {
            HotelRefCriteria += "<HotelRef HotelCityCode='" + city + "'/>";
        }
        if (citytype == "CN")
        {
            HotelRefCriteria += "<HotelRef HotelCityCode='" + CityName + "'/>";
        }
        if (citytype == "LL")
        {
            HotelRefCriteria += "<HotelRef Latitude='" + Latitude.ToString().Trim() + "' Longitude='" + Longitude.ToString().Trim() + "'/>";

        }
        //}
        //if (HotelName != "")
        //{
        //    HotelRefCriteria += " HotelName ='" + HotelName.ToUpper() + "'";
        //}

        //if (HotelRefCriteria != "")
        //{
        //    HotelRefCriteria = "<HotelRef" + HotelRefCriteria + "/>";
        //}
        //string chaincriteria = "";
        //if (hac.ChainCode != "")
        //{
        //    string ctstrim = hac.ChainCode.Trim(',');
        //    string[] pts = ctstrim.Split(',');
        //    foreach (string pt in pts)
        //    {
        //        chaincriteria += "<HotelRef ChainCode='" + pt + "'/>";
        //    }
        //}
        //if (hac.HotelAmenities != "")
        //{
        //    string ctstrim = hac.HotelAmenities.Trim(',');
        //    string[] pts = ctstrim.Split(',');
        //    foreach (string pt in pts)
        //    {
        //        HotelAmenityCriteria += "<HotelAmenity>" + pt + "</HotelAmenity>";
        //    }
        //}
        //if (hac.RoomAmenities != "")
        //{
        //    string ctstrim = hac.RoomAmenities.Trim(',');
        //    string[] pts = ctstrim.Split(',');
        //    foreach (string pt in pts)
        //    {
        //        RoomAmenityCriteria += "<RoomAmenity>" + pt + "</RoomAmenity>";
        //    }
        //}
        //// HotelSearchCriteria = "<HotelSearchCriteria NumProperties='200'><Criterion>" + AwardCriteria + HotelAmenityCriteria + HotelRefCriteria + chaincriteria + PropertyTypeCriteria + RoomAmenityCriteria;
        HotelSearchCriteria = "<HotelSearchCriteria NumProperties='200'><Criterion>" + HotelRefCriteria;
        HotelSearchCriteria += "</Criterion></HotelSearchCriteria>";

        //if (RateMax != "" || RateMin != "")
        //{
        RateRangeCriteria = "<RateRange CurrencyCode='" + CurrencyCode + "'";
        if (RateMax != "")
        {
            RateRangeCriteria += " Max=" + RateMax;
        }
        if (RateMin != "")
        {
            RateRangeCriteria += " Min=" + RateMin;
        }
        RateRangeCriteria += "></RateRange>";
        //}
        if (RateRangeCriteria != "")
        {
            //RatePlanCandiCriteria = "<RatePlanCandidates><ContractNegotiatedRateCode>TVY</ContractNegotiatedRateCode>" + RateRangeCriteria + "</RatePlanCandidates>";

            RatePlanCandiCriteria = "<RatePlanCandidates>";
            if (citytype == "CC")
            {
                string cmdtxt = "EXEC p_SreAgencyCNC @B2C_IDN = " + b2c_idn + ",@CityCode = '" + city + "'";
                DataTable dtcnc = manage_data.GetDataTable(cmdtxt, manage_data.con);
                int i = 1;
                foreach (DataRow dr in dtcnc.Rows)
                {
                    if (dr["ContractNegotiatedCode"].ToString().TrimEnd() != "S1F")
                    {
                        RatePlanCandiCriteria += "<ContractNegotiatedRateCode>" + dr["ContractNegotiatedCode"].ToString().TrimEnd() + "</ContractNegotiatedRateCode>";
                    }
                    //i++;
                    //if(i==2)
                    //{ break; }
                }
            }

            RatePlanCandiCriteria += "" + RateRangeCriteria + "</RatePlanCandidates>";


        }
        DataTable dthcode = new DataTable();
        #endregion
        #region rq
        try
        {
            if (city != null && city != string.Empty)
            {
                //string cmdcodetxt = "SELECT HM.HotelIdn,HM.HotelCode,HM.HotelName,HM.CategoryCode,HM.CategoryGroupCode,HM.ChainCode,HM.AccomTypeCode,HM.IsActive,HM.CreatedDt FROM HotelMain HM JOIN HotelAddress HA ON  HM.HotelCode = HA.HotelCode WHERE HA.DestinationCode  = '" + city + "' or HA.City in(" + query_city + ")";

                string cmdcodetxt = "EXEC p_HotelCode @DestinationCode = '" + city + "', @City = '" + citynamedestcd + "', @County = '" + counry + "'";


                dthcode = manage_data.GetDataTable(cmdcodetxt, manage_data.flip_conhb);

            }
            else if (CityName != null && CityName != string.Empty)
            {


                //string cmdcodetxt = "SELECT HM.HotelIdn,HM.HotelCode,HM.HotelName,HM.CategoryCode,HM.CategoryGroupCode,HM.ChainCode,HM.AccomTypeCode,HM.IsActive,HM.CreatedDt FROM HotelMain HM JOIN HotelAddress HA ON  HM.HotelCode = HA.HotelCode WHERE HA.City  = '" + CityName + "'";
                string cmdcodetxt = "EXEC p_HotelCode @DestinationCode = '', @City = '" + CityName + "', @County = '" + counry + "'";
                dthcode = manage_data.GetDataTable(cmdcodetxt, manage_data.flip_conhb);

            }


            rq += "<availabilityRQ xmlns='http://www.hotelbeds.com/schemas/messages' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>";
            rq += "<stay checkIn='" + checkin + "' checkOut='" + checkout + "'/>";
            int rc = 0;
            if (hac.Rooms > 0)
            {
                for (int i = 0; i < hac.Rooms; i++)
                {
                    adulbyr = hac.Adultsbyroom;
                    childbyr = hac.Childrenbyroom;

                    rq += "<occupancies><occupancy rooms='1' adults='" + Convert.ToInt32(adulbyr.Split('_')[(i)].Split('-')[1]) + "' children='" + Convert.ToInt32(childbyr.Split('_')[(i)].Split('-')[1]) + "'>";
                    if (Convert.ToInt32(childbyr.Split('_')[(i)].Split('-')[1]) > 0)
                    {
                        string childages = string.Empty;

                        //foreach(string item in hac.childrenage.Split(','))
                        //{
                        //if (item != "")
                        //{
                        for (int c = 0; c < Convert.ToInt32(childbyr.Split('_')[(i)].Split('-')[1]); c++)
                        {
                            rq += "	<paxes>	<pax type='CH' age='" + hac.childrenage.Split(',')[(rc)].Split('_')[1] + "'/>";
                            rq += "	</paxes>";
                            //}
                            rc++;
                        }
                        //}
                    }

                    rq += "</occupancy>";
                    rq += "  </occupancies>";
                }



            }


            //rq += "<reviews>	<review type='HOTELBEDS' maxRate='5' minRate= '1' minReviewCount='3' /></reviews>";
            if (!string.IsNullOrEmpty(reviewtype))
            {
                rq += "<reviews>	<review type='HOTELBEDS' maxRate='5' minRate= '1' minReviewCount='3' /></reviews>";
            }
            rq += " <hotels>";

            //rq += "<hotel>454281</hotel>";

            int htc = 0;
            string htcodes = "";

            if (dthcode.Rows.Count > 0)
            {
                foreach (DataRow drcd in dthcode.Rows)
                {
                    rq += "<hotel>" + drcd["HotelCode"] + "</hotel>";
                    if (htc == 0)
                    {
                        htcodes += drcd["HotelCode"].ToString();
                    }
                    else
                    {
                        htcodes += "," + drcd["HotelCode"].ToString();
                    }

                    htc++;
                }

            }


            rq += "</hotels>";

            rq += "</availabilityRQ>";

            string cmdtxt = "update hotelsearch with(rowlock) set HB_HotelCodes='" + htcodes + "' where searchidn=" + searchid + "";
            manage_data.ManipulateData(cmdtxt, manage_data.con);


        }
        catch (Exception ex)
        {

        }
        return rq;


        #endregion
    }
    private void GenerateHS(string XMLResult, string searchid, string b2c_idn)
    {
        XmlDataDocument xmldoc = new XmlDataDocument();


        xmldoc.LoadXml(XMLResult);
        XmlNode xnod = xmldoc.DocumentElement;
        XmlNode xheader = xnod.ChildNodes[0];
        XmlNode xbody = xnod.ChildNodes[1];
        XmlNode xOTA_HotelAvailRS = xbody.ChildNodes[0];
        XmlNode xAppResult = xOTA_HotelAvailRS.ChildNodes[0];
        string appresult = GetValue(xAppResult.Attributes["status"]);

        if (appresult.ToLower() == "complete")
        {
            CreateTables();
            string dt1 = DateTime.Now.ToLongTimeString();
            AddXMLOTA_HotelAvailRS(xOTA_HotelAvailRS);
            string dt2 = DateTime.Now.ToLongTimeString();
            SetBPAddOns(searchid);
            string dt3 = DateTime.Now.ToLongTimeString();
        }
        else
        {
            errormsg = GetChildText(xAppResult.ChildNodes[0].ChildNodes[0], "stl:Message");
        }
        string endtime = DateTime.Now.ToLongTimeString();

    }
    ////  private void SetBPAddOns(string eximagefilename, string searchid)
    private void SetBPAddOns(string searchid)
    {
        string eximagefilename = "";
        hotels = new List<Hotel>();
        foreach (DataRow dr in dtBasicPropInfo.Rows)
        {
            try
            {
                DataRow drbpa = dtBPIadd.NewRow();
                //HotelImage hotelimage = new HotelImage(dr["HotelCode"].ToString(), eximagefilename, searchid, ContextResult);
                string image = "images/No Image found.png";
                string logo = "images/No Image found.png";
                drbpa["MainImage"] = image;
                drbpa["Logo"] = logo;
                drbpa["BasicPropertyInfo_ID"] = dr["BasicPropertyInfo_ID"];
                drbpa["AreaID"] = dr["AreaID"];
                drbpa["ChainCode"] = dr["ChainCode"];
                drbpa["GEO_ConfidenceLevel"] = dr["GEO_ConfidenceLevel"];
                drbpa["HotelCode"] = dr["HotelCode"];
                drbpa["HotelCityCode"] = dr["HotelCityCode"];
                drbpa["HotelName"] = dr["HotelName"];
                drbpa["Latitude"] = dr["Latitude"];
                drbpa["Longitude"] = dr["Longitude"];
                drbpa["Phone"] = dr["Phone"];
                drbpa["Fax"] = dr["Fax"];
                try
                { drbpa["contractnegcode"] = dr["contractnegcode"]; }
                catch (Exception ex)
                {
                    drbpa["contractnegcode"] = "";
                }

                drbpa["Address"] = GetAddress(dr["BasicPropertyInfo_ID"].ToString());
                drbpa["Rating"] = dr["Rating"];
                // drbpa["RateRange_Max"] = GetCurrencySymbol(dr["RateRange_CurrencyCode"].ToString()) + "" + dr["RateRange_Min"];
                drbpa["RateRange_Max"] = dr["RateRange_Min"];
                if (dr["RateRange_Max"] != "")
                {
                    drbpa["RateRange_Maxrate"] = Convert.ToDecimal(dr["RateRange_Min"]);
                }
                drbpa["Rating"] = GetRating(dr["BasicPropertyInfo_ID"].ToString());
                drbpa["RatingValue"] = GetRatingValue(dr["BasicPropertyInfo_ID"].ToString());
                drbpa["FetAmenities"] = GetFetAmenities(dr["BasicPropertyInfo_ID"].ToString());
                //drbpa["image"] = image;
                //drbpa["logo"] = logo;
                drbpa["ChainCode"] = dr["ChainCode"]; ;
                drbpa["ChainName"] = GetChainName(dr["ChainCode"].ToString());
                drbpa["Fitness"] = dr["Fitness"];
                drbpa["Hottub"] = dr["Hottub"];
                drbpa["Indpool"] = dr["Indpool"];
                drbpa["Internet"] = dr["Internet"];
                drbpa["Breakfast"] = dr["Breakfast"];
                drbpa["Kitchen"] = dr["Kitchen"];
                drbpa["Freeparking"] = dr["Freeparking"];
                drbpa["Nonsmoking"] = dr["Nonsmoking"];
                drbpa["accessible"] = dr["accessible"];
                drbpa["pets"] = dr["pets"];
                drbpa["airport"] = dr["airport"];
                drbpa["Business"] = dr["Business"];
                drbpa["Outpool"] = dr["Outpool"];
                drbpa["Kids"] = dr["Kids"];
                dtBPIadd.Rows.Add(drbpa);
            }
            catch (Exception ex)
            {

            }
        }
    }
    private string GetPropertyInfoValue(string PropInfo, string BasicPropertyInfo_ID)
    {
        string rvalue = "false";
        try
        {
            DataRow[] dtp = dtPropOptInfo.Select("PropOptInfoType='" + PropInfo + "' and BasicPropInfo_ID=" + BasicPropertyInfo_ID);
            if (dtp.Count() > 0)
            {
                rvalue = dtp[0]["PropOptInfoInd"].ToString();
            }
        }
        catch
        {
        }
        return rvalue;
    }
    private string GetChainName(string ChainCode)
    {
        string rvalue = "";
        try
        {
            DataRow[] dr = dtChainCode.Select("ChainCode='" + ChainCode + "'");
            rvalue = dr[0]["ChainName"].ToString();
        }
        catch
        {
        }
        return rvalue;
    }
    private string GetCurrencySymbol(string CurrencyCode)
    {
        if (CurrencyCode.ToUpper() == "USD")
            return "$";
        else
            return "";
    }
    private void CreateTables()
    {
        //TravelItinerary 
        dtOTA_HotelAvail.Columns.Add("AvailSearchID");
        dtOTA_HotelAvail.Columns.Add("SucTimeStamp");
        dtOTA_HotelAvail.Columns.Add("HostCommand_LNIATA");
        dtOTA_HotelAvail.Columns.Add("HostCommand_Text");
        dtOTA_HotelAvail.Columns.Add("AddAvail");

        dtCityList.Columns.Add("CityListID");
        dtCityList.Columns.Add("Line_RPH");
        dtCityList.Columns.Add("Line_CntyStCode");
        dtCityList.Columns.Add("Line_Latitude");
        dtCityList.Columns.Add("Line_LocationName");
        dtCityList.Columns.Add("Line_Longitude");
        dtCityList.Columns.Add("AvailSearchID");

        dtBasicPropInfo.Columns.Add("BasicPropertyInfo_ID");
        dtBasicPropInfo.Columns.Add("AreaID");
        dtBasicPropInfo.Columns.Add("ChainCode");
        dtBasicPropInfo.Columns.Add("GEO_ConfidenceLevel");
        dtBasicPropInfo.Columns.Add("HotelCode");
        dtBasicPropInfo.Columns.Add("HotelCityCode");
        dtBasicPropInfo.Columns.Add("HotelName");
        dtBasicPropInfo.Columns.Add("Latitude");
        dtBasicPropInfo.Columns.Add("Longitude");
        dtBasicPropInfo.Columns.Add("Phone");
        dtBasicPropInfo.Columns.Add("Fax");
        dtBasicPropInfo.Columns.Add("ContRateCodeMatch");
        dtBasicPropInfo.Columns.Add("Alt_Avail");
        dtBasicPropInfo.Columns.Add("DC_AvailParticipant");
        dtBasicPropInfo.Columns.Add("DC_SellParticipant");
        dtBasicPropInfo.Columns.Add("RatesExceedMax");
        dtBasicPropInfo.Columns.Add("UnAvail");
        dtBasicPropInfo.Columns.Add("LocDescCode");
        dtBasicPropInfo.Columns.Add("NegotiatedRateCodeMatch");
        dtBasicPropInfo.Columns.Add("PropertyTierLabel");
        dtBasicPropInfo.Columns.Add("RateRange_CurrencyCode");
        dtBasicPropInfo.Columns.Add("RateRange_Max");
        dtBasicPropInfo.Columns.Add("RateRange_Min");
        dtBasicPropInfo.Columns.Add("SpecialOffers");
        dtBasicPropInfo.Columns.Add("SpecialOffers_Text");
        dtBasicPropInfo.Columns.Add("AvailSearchID");
        dtBasicPropInfo.Columns.Add("Address");
        dtBasicPropInfo.Columns.Add("Rating");
        dtBasicPropInfo.Columns.Add("Fitness");
        dtBasicPropInfo.Columns.Add("Hottub");
        dtBasicPropInfo.Columns.Add("Indpool");
        dtBasicPropInfo.Columns.Add("Internet");
        dtBasicPropInfo.Columns.Add("Breakfast");
        dtBasicPropInfo.Columns.Add("Kitchen");
        dtBasicPropInfo.Columns.Add("Freeparking");
        dtBasicPropInfo.Columns.Add("Nonsmoking");
        dtBasicPropInfo.Columns.Add("accessible");
        dtBasicPropInfo.Columns.Add("pets");
        dtBasicPropInfo.Columns.Add("airport");
        dtBasicPropInfo.Columns.Add("Business");
        dtBasicPropInfo.Columns.Add("Outpool");
        dtBasicPropInfo.Columns.Add("Kids");
        dtBasicPropInfo.Columns.Add("contractnegcode");




        dtAddressLines.Columns.Add("AddressLines_ID");
        dtAddressLines.Columns.Add("Address");
        dtAddressLines.Columns.Add("BasicPropInfo_ID");

        dtLocDescText.Columns.Add("LocDesc_Text_ID");
        dtLocDescText.Columns.Add("Text");
        dtLocDescText.Columns.Add("BasicPropInfo_ID");

        dtProperty.Columns.Add("PropertyID");
        dtProperty.Columns.Add("Rating");
        dtProperty.Columns.Add("Text");
        dtProperty.Columns.Add("BasicPropInfo_ID");

        dtPropOptInfo.Columns.Add("PropOptInfoID");
        dtPropOptInfo.Columns.Add("PropOptInfoType");
        dtPropOptInfo.Columns.Add("PropOptInfoInd");
        dtPropOptInfo.Columns.Add("BasicPropInfo_ID");

        dtRoomRate.Columns.Add("RoomRateID");
        dtRoomRate.Columns.Add("GuatSurchargeReq");
        dtRoomRate.Columns.Add("RateLevelCode");
        dtRoomRate.Columns.Add("XPM_GuartReq");
        dtRoomRate.Columns.Add("CancelPolicy_Option");
        dtRoomRate.Columns.Add("CancelPolicy_Numeric");
        dtRoomRate.Columns.Add("HotelRateCode");
        dtRoomRate.Columns.Add("BasicPropInfo_ID");


        dtRateTypeCode.Columns.Add("RateTypeCode_ID");
        dtRateTypeCode.Columns.Add("RateTypeCode");
        dtRateTypeCode.Columns.Add("RoomRateID");

        dtAvaNegClientID.Columns.Add("AvailNegClientID_ID");
        dtAvaNegClientID.Columns.Add("AvailNegClientID");
        dtAvaNegClientID.Columns.Add("BasicPropInfo_ID");

        dtAvaContClientID.Columns.Add("AvailContClientID_ID");
        dtAvaContClientID.Columns.Add("AvailContClientID");
        dtAvaContClientID.Columns.Add("BasicPropInfo_ID");


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
        dtBPIadd.Columns.Add("contractnegcode");


        dtHotelBed.Columns.Add("");



    }
    #region FillTables
    private void AddXMLOTA_HotelAvailRS(XmlNode pxOTA_HotelAvailRS)
    {
        int rno = dtOTA_HotelAvail.Rows.Count;
        FilldtOTA_HotelAvailRS(rno, pxOTA_HotelAvailRS);



        foreach (XmlNode xn in pxOTA_HotelAvailRS.ChildNodes)
        {
            if (xn.Name.ToLower() == "availabilityoptions")
            {
                string dt1 = DateTime.Now.ToLongTimeString();
                AddXMLAvailabilityOptions(xn, rno);
            }
            else if (xn.Name.ToLower() == "citylist")
            {
                // AddXMLCityList(xn, rno);
            }
        }
    }
    private void FilldtOTA_HotelAvailRS(int rno, XmlNode pxOTA_HotelAvailRS)
    {

        DataRow dr = dtOTA_HotelAvail.NewRow();
        dr["AvailSearchID"] = rno;
        dr["SucTimeStamp"] = GetSucTimeStamp(pxOTA_HotelAvailRS);
        dr["AddAvail"] = GetCNAttrValue("AdditionalAvail", "Ind", pxOTA_HotelAvailRS);
        dtOTA_HotelAvail.Rows.Add(dr);
    }
    private void AddXMLAvailabilityOptions(XmlNode ParentNode, int ParentRno)
    {
        foreach (XmlNode xn in ParentNode.ChildNodes)
        {
            if (xn.Name.ToLower() == "availabilityoption")
            {

                AddXMLAvailabilityOption(xn, ParentRno);
                string dt2 = DateTime.Now.ToLongTimeString();
            }
        }
    }
    private void AddXMLAvailabilityOption(XmlNode ParentNode, int ParentRno)
    {
        foreach (XmlNode xn in ParentNode.ChildNodes)
        {
            if (xn.Name.ToLower() == "basicpropertyinfo")
            {

                AddXMLBasicPropertyInfo(xn, ParentRno);

            }
        }
    }
    private void AddXMLBasicPropertyInfo(XmlNode pNode, int ParentRno)
    {
        int rno = dtBasicPropInfo.Rows.Count;
        FilldtBasicPropertyInfo(rno, pNode, ParentRno);
        foreach (XmlNode xn in pNode.ChildNodes)
        {
            if (xn.Name.ToLower() == "address")
            {
                AddXMLAddress(xn, rno);
            }
            else if (xn.Name.ToLower() == "locationdescription")
            {
                // AddXMLLocDescText(xn, rno);
            }
            else if (xn.Name.ToLower() == "property")
            {
                AddXMLProperty(xn, rno);
            }
            else if (xn.Name.ToLower() == "propertyoptioninfo")
            {
                AddXMLPropertyOptionInfo(xn, rno);
            }
            else if (xn.Name.ToLower() == "roomrate")
            {
                // AddXMLRoomRate(xn, rno);
            }
        }
    }
    private void FilldtBasicPropertyInfo(int rno, XmlNode pNode, int ParentRno)
    {
        DataRow dr = dtBasicPropInfo.NewRow();
        dr["BasicPropertyInfo_ID"] = rno;
        dr["AreaID"] = GetValue(pNode.Attributes["AreaID"]);
        dr["ChainCode"] = GetValue(pNode.Attributes["ChainCode"]);
        dr["GEO_ConfidenceLevel"] = GetValue(pNode.Attributes["GEO_ConfidenceLeve"]);
        dr["HotelCode"] = GetValue(pNode.Attributes["HotelCode"]);
        dr["HotelCityCode"] = GetValue(pNode.Attributes["HotelCityCode"]);
        dr["HotelName"] = GetValue(pNode.Attributes["HotelName"]);
        dr["Latitude"] = GetValue(pNode.Attributes["Latitude"]);
        dr["Longitude"] = GetValue(pNode.Attributes["Longitude"]);
        dr["Phone"] = GetPhoneFax(pNode, "Phone");
        dr["Fax"] = GetPhoneFax(pNode, "Fax"); ;
        dr["contractnegcode"] = Getcontract(pNode, "");
        dr["ContRateCodeMatch"] = GetChildText(pNode, "contractualratecodematch");

        dr["Alt_Avail"] = GetCNCNAttrValue("DirectConnect", "Alt_Avail", "Ind", pNode);
        dr["DC_AvailParticipant"] = GetCNCNAttrValue("DirectConnect", "DC_AvailParticipant", "Ind", pNode);
        dr["DC_SellParticipant"] = GetCNCNAttrValue("DirectConnect", "DC_SellParticipant", "Ind", pNode);
        dr["RatesExceedMax"] = GetCNCNAttrValue("DirectConnect", "RatesExceedMax", "Ind", pNode);
        dr["UnAvail"] = GetCNCNAttrValue("DirectConnect", "UnAvail", "Ind", pNode);
        dr["LocDescCode"] = GetCNAttrValue("LocationDescription", "Code", pNode);
        dr["NegotiatedRateCodeMatch"] = GetChildText(pNode, "NegotiatedRateCodeMatch");


        //dr["ADA_Accessible"] = GetCNCNAttrValue("PropertyOptionInfo", "ADA_Accessible", "Ind", pNode);

        dr["PropertyTierLabel"] = GetChildText(pNode, "PropertyTierLabel");
        dr["RateRange_CurrencyCode"] = GetCNAttrValue("RateRange", "CurrencyCode", pNode);
        dr["RateRange_Max"] = GetCNAttrValue("RateRange", "Max", pNode);
        dr["RateRange_Min"] = GetCNAttrValue("RateRange", "Min", pNode);
        dr["SpecialOffers"] = GetCNAttrValue("SpecialOffers", "Ind", pNode);
        dr["SpecialOffers_Text"] = GetChildText(pNode, "SpecialOffers");
        dr["AvailSearchID"] = ParentRno;


        dr["Fitness"] = GetCNCNAttrValue("PropertyOptionInfo", "FitnessCenter", "Ind", pNode);
        dr["Hottub"] = GetCNCNAttrValue("PropertyOptionInfo", "Jacuzzi", "Ind", pNode);
        dr["Indpool"] = GetCNCNAttrValue("PropertyOptionInfo", "IndoorPool", "Ind", pNode);
        dr["Internet"] = GetCNCNAttrValue("PropertyOptionInfo", "HighSpeedInternet", "Ind", pNode);
        dr["Breakfast"] = GetCNCNAttrValue("PropertyOptionInfo", "Breakfast", "Ind", pNode);
        dr["Freeparking"] = GetCNCNAttrValue("PropertyOptionInfo", "FreeParking", "Ind", pNode);
        dr["Nonsmoking"] = GetCNCNAttrValue("PropertyOptionInfo", "NonSmoking", "Ind", pNode);
        dr["accessible"] = GetCNCNAttrValue("PropertyOptionInfo", "ADA_Accessible", "Ind", pNode);
        dr["pets"] = GetCNCNAttrValue("PropertyOptionInfo", "Pets", "Ind", pNode);
        dr["airport"] = GetCNCNAttrValue("PropertyOptionInfo", "AirportShuttle", "Ind", pNode);
        dr["Business"] = GetCNCNAttrValue("PropertyOptionInfo", "BusinessCenter", "Ind", pNode);
        dr["Outpool"] = GetCNCNAttrValue("PropertyOptionInfo", "OutdoorPool", "Ind", pNode);
        dr["Kids"] = GetCNCNAttrValue("PropertyOptionInfo", "KidsFacilities", "Ind", pNode);



        dtBasicPropInfo.Rows.Add(dr);
    }
    private void AddXMLAddress(XmlNode ParentNode, int ParentRno)
    {
        foreach (XmlNode xn in ParentNode.ChildNodes)
        {
            if (xn.Name.ToLower() == "addressline")
            {
                AddXMLAddressLine(xn, ParentRno);
            }
        }
    }
    private void AddXMLAddressLine(XmlNode PNode, int ParentRno)
    {
        int rno = dtAddressLines.Rows.Count;
        FilldtAddressLine(rno, PNode, ParentRno);
    }
    private void FilldtAddressLine(int rno, XmlNode pNode, int ParentRno)
    {

        DataRow dr = dtAddressLines.NewRow();
        dr["AddressLines_ID"] = rno;
        dr["Address"] = pNode.InnerText;
        dr["BasicPropInfo_ID"] = ParentRno;
        dtAddressLines.Rows.Add(dr);
    }
    private void AddXMLProperty(XmlNode PNode, int ParentRno)
    {

        foreach (XmlNode xn in PNode.ChildNodes)
        {
            if (xn.Name.ToLower() == "text")
            {
                int rno = dtProperty.Rows.Count;
                FilldtProperty(rno, PNode, ParentRno);
            }
        }
    }
    private void FilldtProperty(int rno, XmlNode pNode, int ParentRno)
    {
        DataRow dr = dtProperty.NewRow();
        dr["PropertyID"] = rno;
        dr["Rating"] = GetValue(pNode.Attributes["Rating"]);
        dr["Text"] = GetChildText(pNode, "text");
        dr["BasicPropInfo_ID"] = ParentRno;
        dtProperty.Rows.Add(dr);
    }
    private void AddXMLPropertyOptionInfo(XmlNode PNode, int ParentRno)
    {

        foreach (XmlNode xn in PNode.ChildNodes)
        {
            //if (xn.Name.ToLower() == "text")
            //{
            int rno = dtPropOptInfo.Rows.Count;
            FilldtPropertyOptionInfo(rno, xn, ParentRno);
            //}
        }
    }
    private void FilldtPropertyOptionInfo(int rno, XmlNode pNode, int ParentRno)
    {
        DataRow dr = dtPropOptInfo.NewRow();
        dr["PropOptInfoID"] = rno;
        try
        {
            dr["PropOptInfoType"] = pNode.Name;
        }
        catch
        {
        }
        dr["PropOptInfoInd"] = GetValue(pNode.Attributes[0]); ;
        dr["BasicPropInfo_ID"] = ParentRno;
        dtPropOptInfo.Rows.Add(dr);
    }

    private void AddXMLRoomRate(XmlNode PNode, int ParentRno)
    {
        int rno = dtRoomRate.Rows.Count;
        FilldtRoomRate(rno, PNode, ParentRno);
        foreach (XmlNode xn in PNode.ChildNodes)
        {
            if (xn.Name.ToLower() == "RateTypeCode")
            {
                //AddXMLRateTypeCode(xn, rno);
            }
        }

    }
    private void AddXMLRateTypeCode(XmlNode PNode, int ParentRno)
    {
        int rno = dtRateTypeCode.Rows.Count;
        FilldtRateTypeCode(rno, PNode, ParentRno);
    }
    private void AddXMLLocDescText(XmlNode PNode, int ParentRno)
    {

        foreach (XmlNode xn in PNode.ChildNodes)
        {
            if (xn.Name.ToLower() == "text")
            {
                int rno = dtLocDescText.Rows.Count;
                FilldtLocDescText(rno, PNode, ParentRno);
            }
        }
    }
    private void FilldtCityList(int rno, XmlNode pNode, int ParentRno)
    {
        foreach (XmlNode xn in pNode.ChildNodes)
        {
            if (xn.Name.ToLower() == "line")
            {
                DataRow dr = dtCityList.NewRow();
                dr["CityListID"] = rno;
                dr["CountryStateCode"] = GetValue(pNode.Attributes["CountryStateCode"]);
                dr["Line_RPH"] = GetValue(pNode.Attributes["RPH"]);
                dr["Line_Latitude"] = GetValue(pNode.Attributes["Latitude"]);
                dr["Line_LocationName"] = GetValue(pNode.Attributes["LocationName"]);
                dr["Line_Longitude"] = GetValue(pNode.Attributes["Longitude"]);
                dr["AvailSearchID"] = ParentRno;
                dtCityList.Rows.Add(dr);
            }
        }
    }
    private void FilldtLocDescText(int rno, XmlNode pNode, int ParentRno)
    {
        DataRow dr = dtLocDescText.NewRow();
        dr["LocDesc_Text_ID"] = rno;
        dr["Text"] = GetChildText(pNode, "text");
        dr["BasicPropInfo_ID"] = ParentRno;
        dtLocDescText.Rows.Add(dr);
    }
    private void FilldtRateTypeCode(int rno, XmlNode pNode, int ParentRno)
    {
        DataRow dr = dtRateTypeCode.NewRow();
        dr["RateTypeCode_ID"] = rno;
        dr["RateTypeCode"] = pNode.InnerText;
        dr["RoomRateID"] = ParentRno;
        dtRateTypeCode.Rows.Add(dr);
    }
    private void FilldtRoomRate(int rno, XmlNode pNode, int ParentRno)
    {
        DataRow dr = dtRoomRate.NewRow();
        dr["RoomRateID"] = rno;
        dr["GuatSurchargeReq"] = GetValue(pNode.Attributes["GuaranteeSurchargeRequired"]);
        dr["RateLevelCode"] = GetValue(pNode.Attributes["RateLevelCode"]);
        dr["XPM_GuartReq"] = GetValue(pNode.Attributes["XPM_GuaranteeRequired"]);
        dr["CancelPolicy_Option"] = GetCNCNAttrValue("AdditionalInfo", "CancelPolicy", "Option", pNode);
        dr["CancelPolicy_Numeric"] = GetCNCNAttrValue("AdditionalInfo", "CancelPolicy", "Numeric", pNode);
        dr["HotelRateCode"] = GetChildText(pNode, "HotelRateCode");
        dr["BasicPropInfo_ID"] = ParentRno;
        dtRoomRate.Rows.Add(dr);
    }
    #endregion
    private string GetPhoneFax(XmlNode pNode, string item)
    {
        string rvalue = "";
        foreach (XmlNode xn in pNode.ChildNodes)
        {
            if (xn.Name.ToLower() == "contactnumbers")
            {
                foreach (XmlNode xnc in xn.ChildNodes)
                {
                    if (xnc.Name.ToLower() == "contactnumber")
                    {
                        rvalue = GetValue(xnc.Attributes[item]);
                    }
                }
            }
        }
        return rvalue;
    }
    private string Getcontract(XmlNode pNode, string item)
    {
        string rvalue = "";
        foreach (XmlNode xn in pNode.ChildNodes)
        {
            if (xn.Name == "AvailableNegotiatedClientID")
            {

                rvalue = xn.InnerText;

            }
        }
        return rvalue;
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
    private string GetSucTimeStamp(XmlNode pxOTA_HotelAvailRS)
    {
        string rvalue = "";
        try
        {
            rvalue = GetValue(pxOTA_HotelAvailRS.ChildNodes[0].ChildNodes[0].Attributes["timeStamp"]);
        }
        catch
        {
        }
        return rvalue;
    }
    private string GetCNAttrValue(string chnode, string AttName, XmlNode xParentNode)
    {
        string rvalue = "";
        foreach (XmlNode xcn in xParentNode.ChildNodes)
        {
            if (xcn.Name.ToLower() == chnode.ToLower())
            {
                rvalue = GetValue(xcn.Attributes[AttName]);
                break;
            }
        }
        return rvalue;
    }
    private string GetCNCNAttrValue(string ch1node, string ch2node, string AttName, XmlNode xParentNode)
    {
        string rvalue = "";
        foreach (XmlNode xcn1 in xParentNode.ChildNodes)
        {
            if (xcn1.Name.ToLower() == ch1node.ToLower())
            {
                foreach (XmlNode xcn2 in xcn1.ChildNodes)
                {
                    if (xcn2.Name.ToLower() == ch2node.ToLower())
                    {
                        rvalue = GetValue(xcn2.Attributes[AttName]);
                        break;
                    }
                }
            }
        }
        return rvalue;
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
    private string GetAddress(string BPID)
    {
        string rvalue = "";
        DataRow[] dr = dtAddressLines.Select("BasicPropInfo_ID='" + BPID + "'");
        foreach (DataRow dra in dr)
        {
            rvalue += dra["Address"].ToString() + " ";
        }
        return rvalue;
    }
    private string GetRating(string BPID)
    {
        string rvalue = "";
        try
        {
            DataRow[] dr = dtProperty.Select("BasicPropInfo_ID='" + BPID + "'");
            string RatingURL = "";
            foreach (DataRow dra in dr)
            {
                string i = dra["Text"].ToString().Substring(0, dra["Text"].ToString().LastIndexOf(" "));
                string RImgs = GetRatingImages(i);
                rvalue += RImgs;
            }
        }
        catch
        {
        }
        return rvalue;
    }
    private string GetRatingValue(string BPID)
    {
        string rvalue = "1";
        try
        {

            DataRow[] dr = dtProperty.Select("BasicPropInfo_ID='" + BPID + "'");
            rvalue = dr[0]["Text"].ToString().Substring(0, dr[0]["Text"].ToString().LastIndexOf(" "));

        }
        catch
        {
        }
        return rvalue;
    }
    private string GetRatingImages(string i)
    {
        //return i + "Stars";
        return "images/" + i + "stars.png";
    }
    private string GetFetAmenities(string BPID)
    {
        string faicon = "";
        string rvalue = "";
        string cond = "PropOptInfoInd='true' and BasicPropInfo_ID='" + BPID + "'";
        cond += " and PropOptInfoType in('FitnessCenter','IndoorPool','HighSpeedInternet','Breakfast','FreeParking','NonSmoking','ADA_Accessible','Pets','AirportShuttle','BusinessCenter','OutdoorPool','KidsFacilities')";
        DataRow[] drpt = dtPropOptInfo.Select(cond);
        int i = 0;
        foreach (DataRow dra in drpt)
        {
            string desc = "";
            i++;
            if (i < 4)
            {
                faicon = "";
                try
                {
                    // faicon = faicons[dra["PropOptInfoType"].ToString()];
                    DataRow[] drfaicon = Dtfaicons.Select("FaviconDesc='" + dra["PropOptInfoType"].ToString() + "'");
                    if (drfaicon.Count() > 0)
                    {
                        faicon = drfaicon[0]["ImageIcon"].ToString();
                        desc = drfaicon[0]["FaviconDesc_2"].ToString();
                    }
                    else
                    {
                        faicon = "fa fa-fw";
                    }

                }
                catch
                {
                    faicon = "fa fa-fw";
                }
                if (desc == "")
                {
                    desc = dra["PropOptInfoType"].ToString();
                }
                rvalue += "<sapn class='eminites-blcksr'><img  src='" + faicon + "'/> " + desc + "</sapn>";

            }

        }
        return rvalue;
    }
    private string GetFetAmenitiesImagesonly(string BPID)
    {
        string faicon = "";
        string rvalue = "";
        string cond = "PropOptInfoInd='true' and BasicPropInfo_ID='" + BPID + "'";
        DataRow[] drpt = dtPropOptInfo.Select(cond);
        int i = 0;
        foreach (DataRow dra in drpt)
        {
            i++;
            if (i < 5)
            {
                faicon = "";
                try
                {
                    faicon = faicons[dra["PropOptInfoType"].ToString()];
                }
                catch
                {
                    faicon = "fa fa-fw";
                }
                rvalue += "<img  src='" + faicon + "'/> ";
            }

        }
        return rvalue;
    }
    private void AddXMLCityList(XmlNode PNode, int ParentRno)
    {
        int rno = dtCityList.Rows.Count;
        FilldtCityList(rno, PNode, ParentRno);
    }
}


//<img src="http://vcmp-hotels.sabre.com/image/upload/f_auto,q_auto:best,t_vcmp_logo/hotel/l/yx/YX.png" id="HotelLogo_0061370">