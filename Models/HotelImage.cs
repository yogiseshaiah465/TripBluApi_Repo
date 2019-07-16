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
/// Summary description for HotelImage
/// </summary>
public class HotelImage
{
	 public string logo;
        public string Image;
        public static string pcc = "";
        public static string ipcc = "";
        public static string username = "";
        public static string password = "";
       // public string Hotelcode="";
        XmlDataDocument xmldoc = new XmlDataDocument();
        public HotelImage(string Hotelcode, string eximagefilename, string searchid, string ContextResult)
        {
            string resultxml = GetImageXMLContext(Hotelcode, searchid, ContextResult);
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
           // logo = "images/No Image found.png";
          //  Image = "images/No Image found.png";

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
        private string GetXMLRQ(string contextresult, string Hotelcode)
        {
            string rq="";
            if (contextresult.ToString() != "")
            {
                DataSet ds = new DataSet();
                DataSet dsSession = new DataSet();
                StringReader se_stream = new StringReader(contextresult);
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
                    //rq += " <eb:Service eb:type='OTA'>Hotel Shopping Service</eb:Service>";
                    rq += "<eb:Service />";
                    rq += "<eb:Action>GetHotelImageRQ</eb:Action>";
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
                    rq += "<GetHotelImageRQ xmlns='http://services.sabre.com/hotel/image/v1' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' version='1.0.0' xsi:schemaLocation='http://services.sabre.com/hotel/image/v1 GetHotelImageRQ.xsd'>";
                    rq += "<HotelRefs>";
                    rq += " <HotelRef HotelCode='" + Hotelcode + "' CodeContext='Sabre'/>";
                    rq += "</HotelRefs>";
                    rq += "<ImageRef Type='SMALL'  LanguageCode='EN'/>";
                   
                    rq += "</GetHotelImageRQ>";
                    rq += "</SOAP-ENV:Body>";
                    rq += "</SOAP-ENV:Envelope>";
                }
        }
            return rq;
        }
        public string GetImageXML(string Hotelcode, string searchid)
        {
            //pcc = "VL5H";
            //ipcc = "7A7H"; ;
            //username = "373541";
            //password = "WS110542";
            //XMLRead xmlHotelImage = new XMLRead();
            string result = "";
           // result = XMLRead.ContextChange(pcc, ipcc, username, password, searchid);
            result = XMLRead.ContextChange(searchid);
            string rq = "";
            rq = GetXMLRQ(result,Hotelcode);
            result = XMLRead.SendQuery(rq);
            XMLRead.SaveXMLFile(rq, result, searchid + "_hotelsImage" + "_" + Hotelcode);
            return result;
        }
        public string GetImageXMLContext(string Hotelcode, string searchid, string ContextResult)
        {
            XMLRead xmlHotelImage = new XMLRead();
            string rq = "";
            string result = "";
            rq = GetXMLRQ(ContextResult, Hotelcode);
            result = XMLRead.SendQuery (rq);
            XMLRead.SaveXMLFile(rq, result, searchid + "_hotelsImage" + "_" + Hotelcode);
            return result;
        }

    }
