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

public class HotelMedia
{
    public string logo;
    public string Image;
    // public static string result = "";
    public static string xml_rq = "";
    public static string cc_rs = "";
    public static string pcc = "";
    public static string ipcc = "";
    public static string username = "";
    public static string password = "";
    XmlDataDocument xmldoc = new XmlDataDocument();
    DataTable dtHotelMediaRs = new DataTable();
    DataTable dtHotelMediaInfo = new DataTable();
    DataTable dtHotelInfo = new DataTable();
    DataTable dtImageItem = new DataTable();
    public DataTable dtImage = new DataTable();

    private string GetValue(XmlAttribute x)
    {
        string rvalue;
        try { rvalue = x.Value; }
        catch { rvalue = ""; }
        return rvalue;
    }
    public HotelMedia(string Hotelcode, string contextresult, string searchid, string viewid, string b2c_idn)

    {
        DataTable dtpcc = manage_data.getpccdetails(b2c_idn);

        if (dtpcc.Rows.Count > 0)
        {
            pcc = dtpcc.Rows[0]["prv_pcc"].ToString();
            ipcc = dtpcc.Rows[0]["prv_ipcc"].ToString();
            username = dtpcc.Rows[0]["prv_username"].ToString();
            password = dtpcc.Rows[0]["prv_pwd"].ToString();
        }

        try
        {
            string ContextResult = "";
            // ContextResult = contextresult;
            //string filePathContext = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_ContextChange-RS.xml");
            //if (File.Exists(filePathContext))
            //{
            //    ContextResult = File.ReadAllText(filePathContext);
            //}
            //else
            //{
            //    ContextResult = XMLRead.ContextChange(searchid);
            //}
            ContextResult = XMLRead.ContextChange(searchid + "_Media");
            string resultxml = GetMediaXMLContext(Hotelcode, searchid, ContextResult, b2c_idn);

            if (resultxml.ToString() != "")
            {
                DataSet ds = new DataSet();
                DataSet dsSession = new DataSet();
                StringReader se_stream = new StringReader(resultxml);
                dsSession.ReadXml(se_stream);
                string Rq = "";

                if (dsSession.Tables["BinarySecurityToken"] != null)
                {
                    DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
                    DataTable dtMessageData = dsSession.Tables["MessageData"];
                    DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
                    string timestamp = DateTime.UtcNow.ToString();
                    string cresult = XMLRead.closession(dtMessageData.Rows[0]["MessageId"].ToString(), timestamp, dtBinarySecurityToken.Rows[0]["BinarySecurityToken_Text"].ToString(), searchid + "_Media", XMLRead.pcc, XMLRead.ipcc);
                }
            }

            xmldoc.LoadXml(resultxml);
            XmlNode xnod = xmldoc.DocumentElement;
            XmlNode xheader = xnod.ChildNodes[0];
            XmlNode xbody = xnod.ChildNodes[1];
            XmlNode xGetImageRS = xbody.ChildNodes[0];
            XmlNode xMediaInfos = xGetImageRS.ChildNodes[1];



            CreateTables();
            foreach (XmlNode xn in xMediaInfos)
            {
                int rno = dtHotelMediaInfo.Rows.Count + 1;
                FillHotelMediaInfo(rno, xn);
                foreach (XmlNode xn1 in xn)
                {
                    if (xn1.Name.ToLower() == "ns23:hotelinfo")
                    {
                        AddXMLHotelInfo(xn1, rno);
                    }
                    if (xn1.Name.ToLower() == "ns23:imageitems")
                    {
                        AddXMLImageItems(xn1, rno);
                    }
                }
            }
        }
        catch
        {
        }
    }
    public string GetMediaXMLContext(string Hotelcode, string searchid, string ContextResult, string b2c_idn)
    {
        //XMLRead xmlHotelImage = new XMLRead();
        XMLRead.GetPccDetails(b2c_idn);
        string rq = "";
        string result = "";
        ////rq = GetMediaXMLRQ(ContextResult, Hotelcode);
        ////result = XMLRead.SendQuery(rq);
        //XMLRead.SaveXMLFile(rq, result, searchid + "_Media" + "_" + Hotelcode);
        return result;
    }
    private string GetMediaXMLRQ(string contextresult, string Hotelcode)
    {
        string rq = "";
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
                rq += "<eb:Action>GetHotelMediaRQ</eb:Action>";
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
                //rq += "<GetHotelMediaRQ xmlns='http://services.sabre.com/hotel/media/v1' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' version='1.0.0' xsi:schemaLocation='http://services.sabre.com/hotel/image/v1 GetHotelMediaRQ.xsd'>";
                rq += "<GetHotelMediaRQ xmlns='http://services.sabre.com/hotel/media/v1' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' version='1.0.0' xsi:schemaLocation='http://services.sabre.com/hotel/media/v1 GetHotelMediaRQ.xsd'>";
                // rq += "<GetHotelMediaRQ xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:stlp='http://services.sabre.com/STL_Payload/v02_02' xmlns:jaxb='http://java.sun.com/xml/ns/jaxb' xmlns:xjc='http://java.sun.com/xml/ns/jaxb/xjc' xmlns='http://services.sabre.com/hotel/media/v1' jaxb:version='2.0' jaxb:extensionBindingPrefixes='xjc' targetNamespace='http://services.sabre.com/hotel/media/v1' elementFormDefault='qualified' attributeFormDefault='unqualified'>";
                rq += "<HotelRefs>";
                rq += " <HotelRef HotelCode='" + Hotelcode + "' CodeContext='Sabre'>";
                rq += "<ImageRef MaxImages='25'><Images><Image Type='ORIGINAL'/> ";
                rq += " </Images><Categories> ";
                rq += "<Category Code='2' /> <Category Code='3' /><Category Code='4' />";
                rq += "</Categories>";
                rq += "<AdditionalInfo><Info Type='CAPTION'>true</Info></AdditionalInfo><Languages><Language Code='EN'/></Languages></ImageRef>";
                rq += "</HotelRef></HotelRefs>";
                rq += "</GetHotelMediaRQ>";

                rq += "</SOAP-ENV:Body>";
                rq += "</SOAP-ENV:Envelope>";
            }
        }
        return rq;
    }
    private void FilldtHotelMediaRs(int rno, XmlNode HotelMediaRS)
    {

    }
    private void FillHotelMediaInfo(int rno, XmlNode pNode)
    {
        DataRow dr = dtHotelMediaInfo.NewRow();
        dr["MInfoID"] = rno;
        dtHotelMediaInfo.Rows.Add(dr);
    }
    private void FillHotelInfo(int rno, XmlNode pNode, int ParentRno)
    {
        DataRow dr = dtHotelInfo.NewRow();
        dr["HinfoID"] = rno;
        dr["HotelCode"] = GetValue(pNode.Attributes["HotelCode"]);
        dr["Marketer"] = GetValue(pNode.Attributes["Marketer"]);
        dr["Logo"] = GetValue(pNode.Attributes["Logo"]);
        dr["ChainCode"] = GetValue(pNode.Attributes["ChainCode"]);
        dr["CodeContext"] = GetValue(pNode.Attributes["CodeContext"]);
        dr["MinfoID"] = ParentRno;
        dtHotelInfo.Rows.Add(dr);

    }
    private void FillImageItem(int rno, XmlNode pNode, int ParentRno)
    {
        DataRow dr = dtImageItem.NewRow();
        dr["ImageItemID"] = rno;
        dr["Ordinal"] = GetValue(pNode.Attributes["Ordinal"]);
        dr["LastModified"] = GetValue(pNode.Attributes["LastModified"]);
        dr["ID"] = GetValue(pNode.Attributes["ID"]);
        dr["Format"] = GetValue(pNode.Attributes["Format"]);
        dr["MinfoID"] = ParentRno;
        dtImageItem.Rows.Add(dr);
    }
    private void FillImage(int rno, XmlNode pNode, int ParentRno)
    {
        DataRow dr = dtImage.NewRow();
        dr["ImageID"] = rno;
        dr["width"] = GetValue(pNode.Attributes["Width"]);
        dr["height"] = GetValue(pNode.Attributes["Height"]);
        dr["type"] = GetValue(pNode.Attributes["Type"]);
        dr["URL"] = GetValue(pNode.Attributes["Url"]);
        dr["ImageItemID"] = ParentRno;
        dtImage.Rows.Add(dr);
    }
    private void CreateTables()
    {
        dtHotelMediaRs.Columns.Add("HMRSID");
        dtHotelMediaRs.Columns.Add("SucTimeStamp");
        dtHotelMediaRs.Columns.Add("CPAID");
        dtHotelMediaRs.Columns.Add("MessageID");
        dtHotelMediaRs.Columns.Add("ReftoMessageID");

        dtHotelMediaInfo.Columns.Add("MInfoID");

        dtHotelInfo.Columns.Add("HinfoID");
        dtHotelInfo.Columns.Add("HotelCode");
        dtHotelInfo.Columns.Add("Marketer");
        dtHotelInfo.Columns.Add("Logo");
        dtHotelInfo.Columns.Add("ChainCode");
        dtHotelInfo.Columns.Add("CodeContext");
        dtHotelInfo.Columns.Add("MinfoID");

        dtImageItem.Columns.Add("ImageItemID");
        dtImageItem.Columns.Add("Ordinal");
        dtImageItem.Columns.Add("LastModified");
        dtImageItem.Columns.Add("ID");
        dtImageItem.Columns.Add("Format");
        dtImageItem.Columns.Add("MinfoID");


        dtImage.Columns.Add("ImageID");
        dtImage.Columns.Add("width");
        dtImage.Columns.Add("height");
        dtImage.Columns.Add("type");
        dtImage.Columns.Add("URL");
        dtImage.Columns.Add("ImageItemID");


        // dtHotelMediaInfo.Columns.Add("SucTimeStamp");

    }
    private void AddXMLHotelInfo(XmlNode PNode, int ParentRno)
    {
        int rno = dtHotelInfo.Rows.Count + 1;
        FillHotelInfo(rno, PNode, ParentRno);
    }
    private void AddXMLImageItems(XmlNode PNode, int ParentRno)
    {
        foreach (XmlNode xn in PNode.ChildNodes)
        {
            int rno = dtImageItem.Rows.Count + 1;
            FillImageItem(rno, xn, ParentRno);

            foreach (XmlNode xn1 in xn.ChildNodes)
            {
                if (xn1.Name.ToLower() == "ns23:images")
                {
                    AddXMLImages(xn1, ParentRno);
                }
                if (xn1.Name.ToLower() == "ns23:category")
                {
                    AddXMLCategory(xn1, ParentRno);
                }
                if (xn1.Name.ToLower() == "ns23:additonalinfo")
                {
                    AddXMLAdditonalInfo(xn1, ParentRno);
                }
            }
        }
    }
    private void AddXMLImages(XmlNode PNode, int ParentRno)
    {
        foreach (XmlNode xn in PNode.ChildNodes)
        {
            int rno = dtImage.Rows.Count + 1;
            FillImage(rno, xn, ParentRno);
        }

    }
    private void AddXMLCategory(XmlNode PNode, int ParentRno)
    {
    }
    private void AddXMLAdditonalInfo(XmlNode PNode, int ParentRno)
    {
    }
}
