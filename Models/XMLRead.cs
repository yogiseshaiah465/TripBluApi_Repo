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
using System.Security.Cryptography;
using System.Configuration;
using RestSharp;
using Newtonsoft.Json;
using System.Net.Http;

/// <summary>
/// Summary description for XMLRead
/// </summary>
public class XMLRead
{
   // static string apiUri = "https://api.test.hotelbeds.com/hotel-api/1.0/hotels";
    public static string apiKey = ConfigurationManager.AppSettings["HotelPortalApiKey"] != null ? ConfigurationManager.AppSettings["HotelPortalApiKey"].ToString() : string.Empty;//"d7rbjw8rmfg47qphpx3r5bvw";//"ywj295pwmk98egeuvs2taztm";
    public static string signature = string.Empty;
    public static string secretKey = ConfigurationManager.AppSettings["HotelPortalSecret"] != null ? ConfigurationManager.AppSettings["HotelPortalSecret"].ToString() : string.Empty;//"DeUKpzPZbr";//"k5yAZKufPP";
    public static string apiuricontent = ConfigurationManager.AppSettings["HotelPortalUricontent"] != null ? ConfigurationManager.AppSettings["HotelPortalUricontent"].ToString() : string.Empty;
    public XMLRead()
    {
        //apiUri = ConfigurationManager.AppSettings["HotelPortalUri"] != null ? ConfigurationManager.AppSettings["HotelPortalUri"] : string.Empty;
        //apiKey = ConfigurationManager.AppSettings["HotelPortalApiKey"] != null ? ConfigurationManager.AppSettings["HotelPortalApiKey"] : string.Empty;
        //secretKey = ConfigurationManager.AppSettings["HotelPortalSecret"] != null ? ConfigurationManager.AppSettings["HotelPortalSecret"] : string.Empty;

    }

    public string logo;
    public string Image;
    public string b2cidn = "";
    public static string result = "";
    public static string xml_rq = "";
    public static string cc_rs = "";
    public static string pcc = "";
    public static string ipcc = "";
    public static string username = "";
    public static string password = "";
    XmlDataDocument xmldoc = new XmlDataDocument();


    public static void GetPccDetails(string b2cidn)
    {
        DataTable dtpccdet = manage_data.getpccdetails(b2cidn);

        if (dtpccdet.Rows.Count > 0)
        {
            pcc = dtpccdet.Rows[0]["prv_pcc"].ToString();
            ipcc = dtpccdet.Rows[0]["prv_ipcc"].ToString();
            username = dtpccdet.Rows[0]["prv_username"].ToString();
            password = dtpccdet.Rows[0]["prv_pwd"].ToString();
        }
    }
    //   public static string ContextChange(string pcc, string ipcc, string username, string password,string searchid)
    public static string ContextChange(string searchid)
    {
        result = createsession(pcc, ipcc, username, password, searchid);
        string cc_rq = "";
        //DataSet ds = new DataSet();
        //DataSet dsSession = new DataSet();
        //StringReader se_stream = new StringReader(result);
        //dsSession.ReadXml(se_stream);
        //if (dsSession.Tables["BinarySecurityToken"] != null)
        //{
        //    DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
        //    DataTable dtMessageData = dsSession.Tables["MessageData"];
        //    DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
        //    string timestamp = DateTime.UtcNow.ToString();
        //    cc_rq = "<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:SOAP-ENC='http://schemas.xmlsoap.org/soap/encoding/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>";
        //    cc_rq += "<SOAP-ENV:Header>";
        //    cc_rq += "<eb:MessageHeader  xmlns:eb='http://www.ebxml.org/namespaces/messageHeader' SOAP-ENV:mustUnderstand='1' eb:version='2.0'>";
        //    cc_rq += "<eb:From>";
        //    cc_rq += "<eb:PartyId type='urn:x12.org:IO5:01'>WebServiceClient</eb:PartyId>";
        //    cc_rq += " </eb:From>";
        //    cc_rq += "<eb:To>";
        //    cc_rq += " <eb:PartyId type='urn:x12.org:IO5:01'>WebServiceSupplier</eb:PartyId>";
        //    cc_rq += "</eb:To>";
        //    cc_rq += " <eb:CPAId>" + pcc + "</eb:CPAId>";
        //    cc_rq += " <eb:ConversationId>" + dtMessageHeader.Rows[0]["ConversationId"].ToString() + "</eb:ConversationId>";
        //    cc_rq += " <eb:Service eb:type='OTA'>Air Shopping Service</eb:Service>";
        //    cc_rq += "<eb:Action>ContextChangeLLSRQ</eb:Action>";
        //    cc_rq += "<eb:MessageData>";
        //    cc_rq += " <eb:MessageId>" + dtMessageData.Rows[0]["MessageId"].ToString() + "</eb:MessageId>";
        //    cc_rq += "<eb:Timestamp>" + dtMessageData.Rows[0]["Timestamp"].ToString() + "</eb:Timestamp>";
        //    cc_rq += "</eb:MessageData>";
        //    cc_rq += "<eb:RefToMessageId>" + dtMessageData.Rows[0]["ReftoMessageId"].ToString() + "</eb:RefToMessageId> ";
        //    cc_rq += "<eb:DuplicateElimination /> ";
        //    cc_rq += "</eb:MessageHeader>";
        //    cc_rq += "<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext' xmlns:wsu='http://schemas.xmlsoap.org/ws/2002/12/utility'>";
        //    cc_rq += "<wsse:BinarySecurityToken valueType='String' EncodingType='wsse:Base64Binary'>" + dtBinarySecurityToken.Rows[0]["BinarySecurityToken_Text"].ToString() + "</wsse:BinarySecurityToken>";
        //    cc_rq += "</wsse:Security>";
        //    cc_rq += "</SOAP-ENV:Header>";
        //    cc_rq += "<SOAP-ENV:Body>";
        //    cc_rq += "<ContextChangeRQ xmlns='http://webservices.sabre.com/sabreXML/2011/10' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' Version='2.0.3'>";
        //    cc_rq += "<ChangeAAA PseudoCityCode='" + pcc + "' />";
        //    cc_rq += " </ContextChangeRQ>";
        //    cc_rq += "</SOAP-ENV:Body>";
        //    cc_rq += "</SOAP-ENV:Envelope>";
        //    result = SendQuery(cc_rq);
        //    SaveXMLFile(cc_rq, result, searchid + "_ContextChange");
        // }
        return result;
    }
    public static string createsession(string pcc, string ipcc, string username, string password, string searchid)
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
        //XMLRead.SaveXMLFile(xml_rq, result, searchid + "_sessioncreate");
        return result;
    }
    public static string closession(string mid, string timestamp, string token, string sid, string pcc, string ipcc)
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
        //SaveXMLFile(xmlrq, xmlrs, sid + "_sessionclose");
        return "";

    }
    public static string GenerateSignature()
    {
        try
        {
            using (var sha = SHA256.Create())
            {
                long ts = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds / 1000;
                var computedHash = sha.ComputeHash(Encoding.UTF8.GetBytes(apiKey + secretKey + ts));
                signature = BitConverter.ToString(computedHash).Replace("-", "");
            }
        }
        catch (Exception)
        {
            throw;
        }
        return signature;

    }


    public static string GetResponse(string resourceUri)
    {
        string strresultxml = string.Empty;
        try
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.BaseAddress = new Uri(apiuricontent);
            // dev ---https://api.test.hotelbeds.com    pro---https://api.hotelbeds.com
            client.DefaultRequestHeaders.Add("Api-Key", apiKey.Trim());
            client.DefaultRequestHeaders.Add("X-Signature", GenerateSignature().Trim());
        
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            //client.DefaultRequestHeaders.AcceptEncoding
            //var httpContent = new StringContent(xml_rq.ToString(), Encoding.UTF8, "application/xml");
            var respone = client.GetAsync(resourceUri);

            strresultxml = respone.Result.Content.ReadAsStringAsync().Result;
        }
        catch (Exception)
        {

            throw;
        }
        return strresultxml;
    }


    //public static string SendQuerypaymetgat(string xml_rq, string apiUri = "")
    //{
    //    DataSet ds = new DataSet();
    //    GenerateSignature();
    //    string strresultxml = "";
    //    try
    //    {
    //        HttpClient client = new HttpClient();
    //        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
    //        //client.DefaultRequestHeaders.Add("Api-Key", apiKey.Trim());
    //        //client.DefaultRequestHeaders.Add("X-Signature", signature.Trim());
    //        client.DefaultRequestHeaders.Add("Content-Type", "application/xml");
    //        //client.DefaultRequestHeaders.AcceptEncoding
    //        var httpContent = new StringContent(xml_rq.ToString(), Encoding.UTF8, "application/xml");
    //        var respone = client.PostAsync(apiUri, httpContent);



    //        //if (!respone.Result.IsSuccessStatusCode)
    //        //{
    //        //    string message = String.Format("POST failed. Received HTTP {0}", respone.Result.StatusCode);
    //        //    throw new ApplicationException(message);
    //        //}

    //        strresultxml = respone.Result.Content.ReadAsStringAsync().Result;

    //        //if (strresultxml.IndexOf("error") > 0)
    //        //{
    //        //    SendQuery(xml_rq, apiUri);
    //        //}

    //    }
    //    catch (WebException wex)
    //    {
    //        if (wex.Response != null)
    //        {
    //            using (var errorResponse = (HttpWebResponse)wex.Response)
    //            {
    //                using (var reader = new StreamReader(errorResponse.GetResponseStream()))
    //                {
    //                    strresultxml = reader.ReadToEnd();
    //                    //TODO: use JSON.net to parse this string and look at the error message
    //                }
    //            }
    //        }
    //    }
    //    return strresultxml;

    //}

    public static string SendQuerypaymetgat(String xml_rq, string apiUri = "")
    {
        string strresultxml = string.Empty;
        try
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.BaseAddress = new Uri("https://demo.convergepay.com/VirtualMerchantDemo/processxml.do?");

            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            //client.DefaultRequestHeaders.AcceptEncoding
            // var httpContent = new StringContent(xml_rq.ToString(), Encoding.UTF8, "application/xml");
            var respone = client.GetAsync(xml_rq);

            strresultxml = respone.Result.Content.ReadAsStringAsync().Result;
        }
        catch (Exception)
        {

            throw;
        }
        return strresultxml;

    }


    public static string SendQuery(string xml_rq, string apiUri = "")
    {
        DataSet ds = new DataSet();
        GenerateSignature();
        string strresultxml = "";
        try
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.DefaultRequestHeaders.Add("Api-Key", apiKey.Trim());
            client.DefaultRequestHeaders.Add("X-Signature", signature.Trim());
            client.DefaultRequestHeaders.Add("Accept", "application/xml");
            //client.DefaultRequestHeaders.AcceptEncoding
            var httpContent = new StringContent(xml_rq.ToString(), Encoding.UTF8, "application/xml");
            var respone = client.PostAsync(apiUri, httpContent);



            //if (!respone.Result.IsSuccessStatusCode)
            //{
            //    string message = String.Format("POST failed. Received HTTP {0}", respone.Result.StatusCode);
            //    throw new ApplicationException(message);
            //}

            strresultxml = respone.Result.Content.ReadAsStringAsync().Result;

            //if (strresultxml.IndexOf("error") > 0)
            //{
            //    SendQuery(xml_rq, apiUri);
            //}

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
    public static void SaveXMLFilettype(string RQXML, string RSXML, string FileName)
    {
        try
        {
            XmlDocument RQdoc = new XmlDocument();
            RQdoc.LoadXml(RQXML);
            RQdoc.PreserveWhitespace = true;
            string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML_Ttype/" + FileName + "-RQ.xml");
            RQdoc.Save(filePathRQ);


            XmlDocument RSdoc = new XmlDocument();
            RSdoc.LoadXml(RSXML);
            RSdoc.PreserveWhitespace = true;
            string filePathRS = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML_Ttype/" + FileName + "-RS.xml");
            RSdoc.Save(filePathRS);

        }
        catch (Exception ex)
        {

        }

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




    public static void SaveXMLTextFile(string RQXML, string RSXML, string FileName)
    {
        try
        {

            string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + FileName + "-RQ.xml");
            File.WriteAllText(filePathRQ, RQXML);
            string filePathRS = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + FileName + "-RS.xml");
            File.WriteAllText(filePathRS, RSXML);

        }
        catch (Exception ex)
        {

        }

    }


    public static void SaveJSONTextFile(string RQXML, string RSXML, string FileName)
    {
        try
        {

            string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelRRatecomments/" + FileName + "-RQ.json");
            File.WriteAllText(filePathRQ, RQXML);
            string filePathRS = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelRRatecomments/" + FileName + "-RS.json");
            File.WriteAllText(filePathRS, RSXML);

        }
        catch (Exception ex)
        {

        }

    }


}

//// HttpWebRequest httprequest = (HttpWebRequest)HttpWebRequest.Create("https://webservices.havail.sabre.com");//prods
// string result = "";


//pcc = "VL5H";
//ipcc = "7A7H"; ;
//username = "373541";
//password = "WS110542";




//pcc = "K4EF";
//ipcc = "K4EF";
//username = "360194";
//password = "WS682492";