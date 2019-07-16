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
using System.Threading;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
//using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
//using System.Web.Script.Serialization;
using Newtonsoft.Json;
public class PNRDataAj
{
    public HotelEndTrans Hes;

    string pcc = "";
    string ipcc = "";
    string username = "";
    string password = "";
    string cnphone = "";
    string cident = "";
    public PNRDataAj(string searchid, CustomerInfo ci, string rph, string BookinID, string hotelcode, string CurrencyCode, string b2c_idn, string rooms, string Guestdet)
    {
        string ratekey = string.Empty;

        string cmdtxtrkey = "select HB_RateKey from HotelBooking where Bookingidn=" + BookinID + "";
        DataTable dtrkey = manage_data.GetDataTable(cmdtxtrkey, manage_data.con);
        if (dtrkey.Rows.Count > 0)
        {
            ratekey = dtrkey.Rows[0]["HB_RateKey"].ToString();
        }
        //// temporarily commented 
        //string filePathContext = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_ContextChange-RS.xml");
        //if (File.Exists(filePathContext))
        //{
        //    ContextResult = File.ReadAllText(filePathContext);
        //}
        //else
        //{
        //    ContextResult = XMLRead.ContextChange(searchid);
        //}

        DataTable dtpcc = manage_data.getpccdetails(b2c_idn);

        if (dtpcc.Rows.Count > 0)
        {
            cnphone = dtpcc.Rows[0]["cust_identifier"].ToString().Substring(0, 3) + "-" + dtpcc.Rows[0]["cust_identifier"].ToString().Substring(3, 3) + "-" + dtpcc.Rows[0]["cust_identifier"].ToString().Substring(6, 4);
            cident = dtpcc.Rows[0]["cust_identifier"].ToString();
            pcc = dtpcc.Rows[0]["prv_pcc"].ToString();
            ipcc = dtpcc.Rows[0]["prv_ipcc"].ToString();
            username = dtpcc.Rows[0]["prv_username"].ToString();
            password = dtpcc.Rows[0]["prv_pwd"].ToString();
        }

        XMLRead.GetPccDetails(b2c_idn);
        string result = "";
        result = XMLRead.ContextChange(searchid + "_" + hotelcode + "_PNR");

        int rphl = rph.ToString().Length;
        if (rphl == 1)
        {
            rph = "0" + rph.ToString();
        }

        //string RateBodyRQ = GetRateDescBodyRQ(rph);
        //string PassengerBodyRQ = GetPassengerBodyRQ(ci, Guestdet);
        string htlbkuri = ConfigurationManager.AppSettings["HotelPortalBookingUri"] != null ? ConfigurationManager.AppSettings["HotelPortalBookingUri"].ToString() : string.Empty;
        if (!string.IsNullOrEmpty(htlbkuri))
        {
            string HoteResBodyRQ = GetHotelResBodyRQ(searchid, ci, rph, rooms, ratekey,BookinID);


            result = XMLRead.SendQuery(HoteResBodyRQ, htlbkuri);


             XMLRead.SaveXMLTextFile(HoteResBodyRQ, result, searchid + "_" + BookinID + "_PNRHotelResDet");



            //searchid = "7430";
            //BookinID = "1918";


            string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_" + BookinID + "_PNRHotelResDet" + "-RS.xml");
            result = File.ReadAllText(filePathRQ);


            //string RateRQ = GetRateDescRQ(result, RateBodyRQ);

            //result = XMLRead.SendQuery(RateRQ);
            //XMLRead.SaveXMLTextFile(RateRQ, result, searchid + "_" + BookinID + "_PNRRateDesec");

            //string PassengerRQ = GetPassengerXMLRQ(result, PassengerBodyRQ);
            //result = XMLRead.SendQuery(PassengerRQ);
            //XMLRead.SaveXMLTextFile(PassengerRQ, result, searchid + "_" + BookinID + "_PNRPassengerDet");

            //string HotelResRq = GetHotelResXMLRQ(result, HoteResBodyRQ);
            //result = XMLRead.SendQuery(HotelResRq);
            //XMLRead.SaveXMLTextFile(HotelResRq, result, searchid + "_" + BookinID + "_PNRHotelResDet");

            //string EndTransRQ = GetEndTransRQ(result);
            //result = XMLRead.SendQuery(EndTransRQ);
            //XMLRead.SaveXMLTextFile(EndTransRQ, result, searchid + "_" + BookinID + "_PNREndTrans");


            string EndTransXML = result;
            Hes = new HotelEndTrans(EndTransXML, BookinID, filePathRQ);

            ////closing the session 

            //if (result.ToString() != "")
            //{
            //    DataSet ds = new DataSet();
            //    DataSet dsSession = new DataSet();
            //    StringReader se_stream = new StringReader(result);
            //    dsSession.ReadXml(se_stream);
            //    string Rq = "";

            //    if (dsSession.Tables["BinarySecurityToken"] != null)
            //    {
            //        DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
            //        DataTable dtMessageData = dsSession.Tables["MessageData"];
            //        DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
            //        string timestamp = DateTime.UtcNow.ToString();
            //        string cresult = XMLRead.closession(dtMessageData.Rows[0]["MessageId"].ToString(), timestamp, dtBinarySecurityToken.Rows[0]["BinarySecurityToken_Text"].ToString(), searchid + "_" + BookinID + "_PNR", XMLRead.pcc, XMLRead.ipcc);
            //    }
            //}
        }




    }
    private string GetRQ(HPDCondition hpc)
    {
        string rqvalue = "";
        rqvalue += "<SOAP-ENV:Body>";
        rqvalue += "<HotelPropertyDescriptionRQ xmlns='http://webservices.sabre.com/sabreXML/2011/10' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'  Version='2.3.0'>";
        rqvalue += "<AvailRequestSegment>";
        rqvalue += " <GuestCounts Count='" + hpc.guestcount + "' />";
        rqvalue += " <HotelSearchCriteria>";
        rqvalue += " <Criterion>";
        rqvalue += " <HotelRef HotelCode='" + hpc.HotelCode + "'/>";
        rqvalue += " </Criterion>";
        rqvalue += "</HotelSearchCriteria>";
        rqvalue += "<RatePlanCandidates><RateRange CurrencyCode='" + hpc.CurrencyCode + "'/></RatePlanCandidates>";
        rqvalue += " <TimeSpan End='" + hpc.checkout + "' Start='" + hpc.checkin + "' />";
        rqvalue += "  </AvailRequestSegment>";
        rqvalue += "</HotelPropertyDescriptionRQ>";
        rqvalue += "</SOAP-ENV:Body>";
        return rqvalue;
    }
    private string GetPropDescBodyRQ(HPDCondition hpc)
    {
        string rq = "";
        rq += "<SOAP-ENV:Body>";
        rq += "<HotelPropertyDescriptionRQ ReturnHostCommand='true' Version='2.1.0' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns='http://webservices.sabre.com/sabreXML/2011/10'>";
        rq += "<AvailRequestSegment>";
        rq += " <GuestCounts Count='" + hpc.guestcount + "'/>";
        rq += " <HotelSearchCriteria>";
        rq += " <Criterion>";
        rq += " <HotelRef HotelCode='" + hpc.HotelCode + "'/>";
        rq += " </Criterion>";
        rq += "</HotelSearchCriteria>";
        //rq += " <TimeSpan Start='" + Convert.ToDateTime(Hapc.checkin).ToString("yyyy-MM-dd") + "' End='" + Convert.ToDateTime(Hapc.checkout).ToString("yyyy-MM-dd") + "'/>";
        rq += "  </AvailRequestSegment>";
        rq += "</HotelPropertyDescriptionRQ>";
        rq += "</SOAP-ENV:Body>";
        return rq;
    }
    private string GetRateDescBodyRQ(string Rph)
    {
        int rphl = Rph.ToString().Length;
        if (rphl == 1)
        {
            Rph = "0" + Rph.ToString();
        }
        string rq = "";
        rq += "<SOAP-ENV:Body>";
        rq += "<HotelRateDescriptionRQ ReturnHostCommand='true' Version='2.0.0' xmlns='http://webservices.sabre.com/sabreXML/2011/10' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>";
        rq += "<AvailRequestSegment>";
        rq += "<RatePlanCandidates>";
        rq += "<RatePlanCandidate RPH='" + Rph + "'/>";
        rq += "</RatePlanCandidates>";
        rq += " </AvailRequestSegment>";
        rq += "</HotelRateDescriptionRQ>";
        rq += "</SOAP-ENV:Body>";
        return rq;
    }
    private string GetPassengerBodyRQ(CustomerInfo ci, string Guestdet)
    {
        string rq = "";
        rq += "<SOAP-ENV:Body>";
        rq += "<PassengerDetailsRQ xmlns='http://services.sabre.com/sp/pd/v3_3' version='3.3.0' HaltOnError='true'>";
        //rq += " <PostProcessing IgnoreAfter='true' RedisplayReservation='true'>";
        //rq += " <EndTransactionRQ>";
        //rq += " <EndTransaction Ind='true'>";
        //rq += " </EndTransaction>";
        //rq += " <Source ReceivedFrom='TRIPXOL SOLUTIONS'/>";
        //rq += " </EndTransactionRQ>";
        //rq += " </PostProcessing>";
        rq += " <SpecialReqDetails>";
        rq += " <AddRemarkRQ>";
        rq += "  <RemarkInfo>";
        rq += "  <Remark Type='Historical'>";
        rq += "  <Text>ASD/TP/PNR/</Text>";
        rq += " </Remark>";
        rq += "</RemarkInfo>";
        rq += " </AddRemarkRQ>";
        rq += "  </SpecialReqDetails>";
        rq += "<TravelItineraryAddInfoRQ>";
        rq += "<AgencyInfo>";
        //rq += "<Address><AddressLine>SABRE TRAVEL</AddressLine><CityName>SOUTHLAKE</CityName><CountryCode>US</CountryCode><PostalCode>76092</PostalCode><StateCountyProv StateCode='TX'/> <StreetNmbr>3150 SABRE DRIVE</StreetNmbr></Address>";
        //  rq += " <Ticketing TicketType='7TAW' />";
        rq += "</AgencyInfo>";
        rq += "<CustomerInfo>";
        rq += "<ContactNumbers>";
        rq += "<ContactNumber LocationCode='FSG' NameNumber='1.1' Phone='" + cnphone + "' PhoneUseType='H'/>";
        rq += "</ContactNumbers>";
        rq += "<CustomerIdentifier>" + cident + "</CustomerIdentifier>";
        rq += " <Email Address='" + ci.Email + "' NameNumber='1.1' Type='TO' />";

        if (Guestdet != "")
        {
            string[] GDsplit = Guestdet.Split('*');
            int i = 0;
            foreach (string gds in GDsplit)
            {
                string[] gnames = gds.Split(',');

                i++;
                rq += "<PersonName PassengerType='ADT' NameNumber='" + i + ".1" + "'>";
                rq += "<GivenName>" + gnames[1] + "</GivenName>";
                rq += "<Surname>" + gnames[2] + "</Surname>";
                rq += "</PersonName>";
            }
        }



        rq += "</CustomerInfo>";
        rq += "</TravelItineraryAddInfoRQ>";
        rq += "</PassengerDetailsRQ>";
        rq += "</SOAP-ENV:Body>";
        return rq;
    }
    private string GetHotelResBodyRQ(string searchid, CustomerInfo ci, string rph, string rooms, string ratekey,string bookingid)
    {
        int rphl = rph.ToString().Length;
        if (rphl == 1)
        {
            rph = "00" + rph.ToString();
        }
        if (rphl == 2)
        {
            rph = "0" + rph.ToString();
        }

        string expdate = "";
        string[] expdatepart = ci.CCExpDate.Split('-');
        expdate = expdatepart[1] + "-" + expdatepart[0];

        string rq = "";
        //rq += "<SOAP-ENV:Body>";
        //rq += "<OTA_HotelResRQ xmlns='http://webservices.sabre.com/sabreXML/2011/10' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' ReturnHostCommand='false' TimeStamp='2013-11-22T17:15:00-06:00' Version='2.2.0'>";
        //rq += "<Hotel>";
        //rq += "<BasicPropertyInfo RPH='" + rph + "' />";
        //rq += "<Guarantee Type='G'>";
        //rq += "<CC_Info>";
        //rq += "<PaymentCard Code='" + ci.PayMethod + "' ExpireDate='" + expdate + "' Number='" + ci.CCNumber + "'/>";
        //rq += "<PersonName>";
        //rq += "<Surname>" + ci.CCHName + "</Surname>";
        //rq += "</PersonName>";
        //rq += "</CC_Info>";
        //rq += "</Guarantee>";
        //rq += "<RoomType NumberOfUnits='" + rooms  + "'/>";
        //rq += "</Hotel>";
        //rq += "</OTA_HotelResRQ>";
        //rq += "</SOAP-ENV:Body>";

        int adults = 0;
        string adultsbyroom = string.Empty;
        string ChildrenByRoom = string.Empty;
        int childs = 0;
        int Roomsdb = 0;
        string childages = string.Empty;

        string cmdtxtrkey = "select * from HotelSearch where Searchidn=" + searchid + "";
        DataTable dtsres = manage_data.GetDataTable(cmdtxtrkey, manage_data.con);
        int bid = Convert.ToInt32(bookingid);
        DataTable dssearch = HotelDBLayer.GetPaxDet_BOF(bid);
        string Name = string.Empty;
        //if (dssearch.Rows.Count > 0)
        //{
        //    foreach (DataTable guestdet in dssearch.Rows)
        //    {
        //        Name += guestdet.Rows[0]["FirstName"].ToString() + " " + guestdet.Rows[0]["LastName"].ToString()+",";
        //    }
        
        
        //}
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

        


        rq += "<bookingRQ xmlns='http://www.hotelbeds.com/schemas/messages' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>";
        rq += "<holder name='" + ci.Name + "' surname='" + ci.SurName + "'/>";
        rq += "<clientReference>IntegrationAgency</clientReference>";
        rq += "<rooms>";
        int rc = 0;
        int cco = 0;
        int adco = 0;
        string[] ratekey_split = ratekey.Split('|');
        string[] chags_split = childages.Split(',');
        string[] childbyroom_split = ChildrenByRoom.Split('_');
         string[] adtbyroom_split = adultsbyroom.Split('_');
         int chagc = 0;
        for (int i = 0; i < Roomsdb; i++)
        {
            int adts = Convert.ToInt32(adtbyroom_split[i].Split('-')[1]);
            int chds = Convert.ToInt32(childbyroom_split[i].Split('-')[1]);
            if (i == 0)
            {
                rq += "<room rateKey='" + ratekey + "'>";
                chagc = chds;
            }
            else
            {
                string ratekey_new = "";
                //string[] roominfo = null;

                //if (chds > 0)
                //{

                //    roominfo = chags_split[i].Split('_');
                //}

                string roomcont = "1~" + adts + "~" + chds;
                string agscont = "";
                for (int a = 0; a < chds; a++)
                {
                    
                    agscont += chags_split[chagc].Split('_')[1] + "~";
                    chagc++;
                }
                if (chds > 0)
                {
                    ratekey_split[10] = agscont.Remove(agscont.Length - 1);
                    ratekey_split[9] = roomcont;
                    for (int r = 0; r <= 11; r++)
                    {
                        ratekey_new += ratekey_split[r] + "|";
                    }

                    rq += "<room rateKey='" + ratekey_new.Remove(ratekey_new.Length - 1) + "'>";
                }
                else
                {
                    ratekey_split[10] = "";
                    ratekey_split[9] = roomcont;
                    for(int r=0;r<=11;r++)
                    {
                        ratekey_new += ratekey_split[r] + "|";
                    }

                    rq += "<room rateKey='" + ratekey_new.Remove(ratekey_new.Length - 1) + "'>";
                }
            }
            //rq += "<paxes>";

            //rate key splitting

            //rq += "<pax roomId='1' type='AD' name='" + ci.Name + "' surname='" + ci.CCHName + "'/>";
            //rq += "</paxes>";
            int adlbyr = adultsbyroom.Split('_').Count();

            //if (Convert.ToInt32(adultsbyroom.Split('_')[(i)].Split('-')[1]) > 0)
            //{
            rq += "<paxes>";
            string stradlts = string.Empty;
            stradlts=adults.ToString();
            
                //for (int j = 0; j < Convert.ToInt32(adultsbyroom.Split('_')[(i)].Split('-')[1]); j++)
                //{
                   
                        rq += "	<pax roomId='1' type='AD' name='" + dssearch.Rows[i]["FirstName"].ToString() + "' surname='" + dssearch.Rows[i]["LastName"] + "'/>";
                        adco++;
                   
                //}
            
             

            if (Convert.ToInt32(ChildrenByRoom.Split('_')[(i)].Split('-')[1]) > 0)
            {
                //string rcage = "room" + i + "-" + "child" + i + "-" + "childAge"+i;
                
                for (int k = 0; k < Convert.ToInt32(ChildrenByRoom.Split('_')[(i)].Split('-')[1]); k++)
                {

                    rq += "	<pax roomId='1' type='CH' age='" + childages.Split(',')[cco].Split('_')[1].ToString() + "'/>";
                        cco++;
                }

            }
            rq += "	</paxes>";
            //}

            rq += "</room>";
        }
        rq += "</rooms>";
        rq += "<remark>Booking remarks are to be written here.</remark>";
        rq += "</bookingRQ>";


        return rq;
    }
    private string GetPropDescRQ(string result, string PropDescRQ)
    {
        string rq = "";
        if (result.ToString() != "")
        {
            DataSet ds = new DataSet();
            DataSet dsSession = new DataSet();
            StringReader se_stream = new StringReader(result);
            dsSession.ReadXml(se_stream);
            if (dsSession.Tables["BinarySecurityToken"] != null)
            {
                DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
                DataTable dtMessageData = dsSession.Tables["MessageData"];
                DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
                string timestamp = DateTime.UtcNow.ToString();

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
                rq += "<eb:Action>HotelPropertyDescriptionLLSRQ</eb:Action>";
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
                rq += PropDescRQ;
                rq += "</SOAP-ENV:Envelope>";
                result = XMLRead.SendQuery(rq);
            }
        }
        return rq;
    }
    private string GetRateDescRQ(string propertyresult, string RateBodyRQ)
    {
        string rq = "";
        if (propertyresult.ToString() != "")
        {
            DataSet ds = new DataSet();
            DataSet dsSession = new DataSet();
            StringReader se_stream = new StringReader(propertyresult);
            dsSession.ReadXml(se_stream);
            if (dsSession.Tables["BinarySecurityToken"] != null)
            {
                DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
                DataTable dtMessageData = dsSession.Tables["MessageData"];
                DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
                string timestamp = DateTime.UtcNow.ToString();

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
                rq += "<eb:Action>HotelRateDescriptionLLSRQ</eb:Action>";
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
                rq += RateBodyRQ;
                rq += "</SOAP-ENV:Envelope>";
            }
        }
        return rq;
    }
    private string GetPassengerXMLRQ(string propertyresult, string bodyrq)
    {
        string rq = "";
        if (propertyresult.ToString() != "")
        {
            // string bodyrq = GetPassengerBodyRQ(ci);
            DataSet ds = new DataSet();
            DataSet dsSession = new DataSet();
            StringReader se_stream = new StringReader(propertyresult);
            dsSession.ReadXml(se_stream);
            if (dsSession.Tables["BinarySecurityToken"] != null)
            {
                DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
                DataTable dtMessageData = dsSession.Tables["MessageData"];
                DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
                string timestamp = DateTime.UtcNow.ToString();
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
                rq += "<eb:Action>PassengerDetailsRQ</eb:Action>";
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
                rq += bodyrq;
                rq += "</SOAP-ENV:Envelope>";
                //  result = SendQuery(rq);
                // SaveXMLFile(rq, result, "passngerdet" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
            }
        }
        return rq;
    }
    private string GetHotelResXMLRQ(string Passengerresult, string bodyrq)
    {
        string rq = "";
        if (Passengerresult.ToString() != "")
        {
            DataSet ds = new DataSet();
            DataSet dsSession = new DataSet();
            StringReader se_stream = new StringReader(Passengerresult);
            dsSession.ReadXml(se_stream);
            if (dsSession.Tables["BinarySecurityToken"] != null)
            {
                DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
                DataTable dtMessageData = dsSession.Tables["MessageData"];
                DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
                string timestamp = DateTime.UtcNow.ToString();
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
                rq += "<eb:Action>OTA_HotelResLLSRQ</eb:Action>";
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
                rq += bodyrq;
                rq += "</SOAP-ENV:Envelope>";
            }
            return rq;
        }
        return rq;
    }
    private string GetEndTransRQ(string resresult)
    {
        string rq = "";
        if (resresult.ToString() != "")
        {
            DataSet ds = new DataSet();
            DataSet dsSession = new DataSet();
            StringReader se_stream = new StringReader(resresult);
            dsSession.ReadXml(se_stream);
            if (dsSession.Tables["BinarySecurityToken"] != null)
            {
                DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
                DataTable dtMessageData = dsSession.Tables["MessageData"];
                DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
                string timestamp = DateTime.UtcNow.ToString();

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
                rq += "<eb:Action>EndTransactionLLSRQ</eb:Action>";
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
                rq += "<SOAP-ENV:Body>";
                rq += "<EndTransactionRQ Version='2.0.8' xmlns='http://webservices.sabre.com/sabreXML/2011/10' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>";
                rq += "<EndTransaction Ind='true'/>";
                rq += "<Source ReceivedFrom='TRIPXOL SOLUTIONS' />";
                rq += "</EndTransactionRQ>";
                rq += "</SOAP-ENV:Body>";
                rq += "</SOAP-ENV:Envelope>";
            }
        }
        return rq;
    }
    private HPDCondition GetCondition(string hotelcode, string CurrencyCode, string BookinID)
    {
        HPDCondition Hapc = new HPDCondition();
        DataTable dssearch = HotelDBLayer.GetSearchFromBookingID(BookinID);
        int guestcount = Convert.ToInt16(dssearch.Rows[0]["Adults"].ToString()) + Convert.ToInt16(dssearch.Rows[0]["Children"].ToString());
        try { Hapc.HotelCode = hotelcode; }
        catch { Hapc.HotelCode = ""; }
        try { Hapc.checkin = Convert.ToDateTime(dssearch.Rows[0]["CheckInDt"]).ToString("MM-dd"); }
        catch { Hapc.checkin = ""; }
        try { Hapc.checkout = Convert.ToDateTime(dssearch.Rows[0]["CheckOutDt"]).ToString("MM-dd"); }
        catch { Hapc.checkout = ""; }
        try { Hapc.guestcount = guestcount.ToString(); }
        catch { Hapc.guestcount = ""; }
        if (CurrencyCode == "")
        {
            Hapc.CurrencyCode = "USD";
        }
        else
        {
            Hapc.CurrencyCode = CurrencyCode;
        }
        return Hapc;
    }


}
