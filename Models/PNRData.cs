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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
  
    public class PNRData
    {
        public HotelEndTrans Hes;
        string pcc = "VL5H";
        string ipcc = "7A7H";
        string username = "373541";
        string password = "WS110542";
        public PNRData(string searchid, CustomerInfo ci, string rph, string ContextResult, string BookinID)
        {
            //string filePathContext = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_ContextChange-RS.xml");
            //if (File.Exists(filePathContext))
            //{
            //    ContextResult = File.ReadAllText(filePathContext);
            //}
            //else
            //{
            //    ContextResult = XMLRead.ContextChange(searchid);
            //}

            string result = "";
            int rphl = rph.ToString().Length;
            if (rphl == 1)
            {
                rph = "0" + rph.ToString();
            }
            string RateBodyRQ = GetRateDescBodyRQ(rph);
            string PassengerBodyRQ = GetPassengerBodyRQ(ci);
            string HoteResBodyRQ = GetHotelResBodyRQ(ci, rph);
            string RateRQ = GetRateDescRQ(ContextResult, RateBodyRQ);
            result = XMLRead.SendQuery(RateRQ);
            XMLRead.SaveXMLFile(RateRQ, result, BookinID + "_RateDesec");
            string PassengerRQ = GetPassengerXMLRQ(result, PassengerBodyRQ);
            result = XMLRead.SendQuery(PassengerRQ);
            XMLRead.SaveXMLFile(RateRQ, result, BookinID + "_PassengerDet");
            string HotelResRq = GetHotelResXMLRQ(result, HoteResBodyRQ);
            result = XMLRead.SendQuery(HotelResRq);
            XMLRead.SaveXMLFile(RateRQ, result, BookinID + "_HotelResDet");
            string EndTransRQ = GetEndTransRQ(result);
            result = XMLRead.SendQuery(EndTransRQ);
            XMLRead.SaveXMLFile(RateRQ, result, BookinID + "_EndTrans");
            string EndTransXML = result;
            Hes = new HotelEndTrans(EndTransXML);

            if (result.ToString() != "")
            {
                DataSet ds = new DataSet();
                DataSet dsSession = new DataSet();
                StringReader se_stream = new StringReader(result);
                dsSession.ReadXml(se_stream);
                string Rq = "";

                if (dsSession.Tables["BinarySecurityToken"] != null)
                {
                    DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
                    DataTable dtMessageData = dsSession.Tables["MessageData"];
                    DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
                    string timestamp = DateTime.UtcNow.ToString();
                    string cresult = XMLRead.closession(dtMessageData.Rows[0]["MessageId"].ToString(), timestamp, dtBinarySecurityToken.Rows[0]["BinarySecurityToken_Text"].ToString(), searchid + "_PNREndTrans", XMLRead.pcc, XMLRead.ipcc);
                }
            }

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
    private string GetPassengerBodyRQ(CustomerInfo ci)
    {
        string rq = "";
        rq += "<SOAP-ENV:Body>";
        rq += "<PassengerDetailsRQ xmlns='http://services.sabre.com/sp/pd/v3_3' version='3.3.0' HaltOnError='true'>";
        rq += "<TravelItineraryAddInfoRQ>";
        rq += "<AgencyInfo>";
        rq += "<Address><AddressLine>SABRE TRAVEL</AddressLine><CityName>SOUTHLAKE</CityName><CountryCode>US</CountryCode><PostalCode>76092</PostalCode><StateCountyProv StateCode='TX'/> <StreetNmbr>3150 SABRE DRIVE</StreetNmbr></Address>";
        rq += "</AgencyInfo>";
        rq += "<CustomerInfo>";
        rq += "<ContactNumbers>";
        rq += "<ContactNumber LocationCode='FSG' NameNumber='1.1' Phone='214-566-9567' PhoneUseType='H'/>";
        rq += "</ContactNumbers>";
        rq += "<CustomerIdentifier>2146312458</CustomerIdentifier>";
        rq += " <Email Address='" + ci.Email + "' NameNumber='1.1' Type='TO' />";
        rq += "<PersonName PassengerType='ADT' NameNumber='1.1'>";
        rq += "<GivenName>" + ci.Name + "</GivenName>";
        rq += "<Surname>" + ci.SurName + "</Surname>";
        rq += "</PersonName>";
        rq += "</CustomerInfo>";
        rq += "</TravelItineraryAddInfoRQ>";
        rq += "</PassengerDetailsRQ>";
        rq += "</SOAP-ENV:Body>";
        return rq;
    }
    private string GetHotelResBodyRQ(CustomerInfo ci, string rph)
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
        expdate = "20" + expdatepart[1] + "-" + expdatepart[0];

        string rq = "";
        rq += "<SOAP-ENV:Body>";
        rq += "<OTA_HotelResRQ xmlns='http://webservices.sabre.com/sabreXML/2011/10' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' ReturnHostCommand='false' TimeStamp='2013-11-22T17:15:00-06:00' Version='2.2.0'>";
        rq += "<Hotel>";
        rq += "<BasicPropertyInfo RPH='" + rph + "' />";
        rq += "<Guarantee Type='G'>";
        rq += "<CC_Info>";
        rq += "<PaymentCard Code='" + ci.PayMethod + "' ExpireDate='" + expdate + "' Number='" + ci.CCNumber + "'/>";
        rq += "<PersonName>";
        rq += "<Surname>" + ci.CCHName + "</Surname>";
        rq += "</PersonName>";
        rq += "</CC_Info>";
        rq += "</Guarantee>";
        rq += "<RoomType NumberOfUnits='1'/>";
        rq += "</Hotel>";
        rq += "</OTA_HotelResRQ>";
        rq += "</SOAP-ENV:Body>";
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
                rq += "<Source ReceivedFrom='SWS TEST' />";
                rq += "</EndTransactionRQ>";
                rq += "</SOAP-ENV:Body>";
                rq += "</SOAP-ENV:Envelope>";
            }
        }
        return rq;
    }
    }

