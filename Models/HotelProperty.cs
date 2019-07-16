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

/// <summary>
/// Summary description for HotelProperty
/// </summary>
public class HotelProperty
{
	public static string result = "";
        public static string xml_rq = "";
        public static string pcc = "";
        public static string ipcc = "";
        public static string username = "";
        string HotelCode;
        public static string password = "";
        CultureInfo culinfo = Thread.CurrentThread.CurrentCulture;
       
        #region ReadXML
    
        public static string GetPropertyDesc(string rqbody, string ContextResult,string searchid,string HotelCode)
        {
            string rq = "";
            string result = "";
            result = ContextResult;
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
                    rq += rqbody;
                    rq += "</SOAP-ENV:Envelope>";
                    result =XMLRead.SendQuery(rq);
                    XMLRead.SaveXMLFile(rq, result, searchid + "_propertydesc_" + HotelCode);
                }
            }

            return result;
        }
        //public static string GetRateDesc(string Rph,string result)
        //{
        //    string rq = "";

        //    // tripxol
        //    //pcc = "V3MI";
        //    //ipcc = "V7PI"; ;
        //    //username = "622755";
        //    //password = "WS499652";
        //    //string result = "";
        //    //xml_rq = "";
        //    //result = createsession("hotels");
        //    if (result.ToString() != "")
        //    {
        //        DataSet ds = new DataSet();
        //        DataSet dsSession = new DataSet();
        //        StringReader se_stream = new StringReader(result);
        //        dsSession.ReadXml(se_stream);
        //        if (dsSession.Tables["BinarySecurityToken"] != null)
        //        {
        //            DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
        //            DataTable dtMessageData = dsSession.Tables["MessageData"];
        //            DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
        //            string timestamp = DateTime.UtcNow.ToString();

        //            rq = "<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:SOAP-ENC='http://schemas.xmlsoap.org/soap/encoding/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>";
        //            rq += "<SOAP-ENV:Header>";
        //            rq += "<eb:MessageHeader  xmlns:eb='http://www.ebxml.org/namespaces/messageHeader' SOAP-ENV:mustUnderstand='1' eb:version='2.0'>";
        //            rq += "<eb:From>";
        //            rq += "<eb:PartyId type='urn:x12.org:IO5:01'>WebServiceClient</eb:PartyId>";
        //            rq += " </eb:From>";
        //            rq += "<eb:To>";
        //            rq += " <eb:PartyId type='urn:x12.org:IO5:01'>WebServiceSupplier</eb:PartyId>";
        //            rq += "</eb:To>";
        //            rq += " <eb:CPAId>" + pcc + "</eb:CPAId>";
        //            rq += " <eb:ConversationId>" + dtMessageHeader.Rows[0]["ConversationId"].ToString() + "</eb:ConversationId>";
        //            rq += " <eb:Service eb:type='OTA'>Air Shopping Service</eb:Service>";
        //            rq += "<eb:Action>HotelPropertyDescriptionLLSRQ</eb:Action>";
        //            rq += "<eb:MessageData>";
        //            rq += " <eb:MessageId>" + dtMessageData.Rows[0]["MessageId"].ToString() + "</eb:MessageId>";
        //            rq += "<eb:Timestamp>" + dtMessageData.Rows[0]["Timestamp"].ToString() + "</eb:Timestamp>";
        //            rq += "</eb:MessageData>";
        //            rq += "<eb:RefToMessageId>" + dtMessageData.Rows[0]["ReftoMessageId"].ToString() + "</eb:RefToMessageId> ";
        //            rq += "<eb:DuplicateElimination /> ";
        //            rq += "</eb:MessageHeader>";
        //            rq += "<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext' xmlns:wsu='http://schemas.xmlsoap.org/ws/2002/12/utility'>";
        //            rq += "<wsse:BinarySecurityToken valueType='String' EncodingType='wsse:Base64Binary'>" + dtBinarySecurityToken.Rows[0]["BinarySecurityToken_Text"].ToString() + "</wsse:BinarySecurityToken>";
        //            rq += "</wsse:Security>";
        //            rq += "</SOAP-ENV:Header>";
        //            rq += "<SOAP-ENV:Body>";
        //            rq += "<HotelRateDescriptionRQ ReturnHostCommand='true' Version='2.0.0' xmlns='http://webservices.sabre.com/sabreXML/2011/10' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>";
        //            rq += "<AvailRequestSegment>";
        //            rq += "<RatePlanCandidates>";
        //            rq += "<RatePlanCandidate RPH='" + Rph + "'/>";
        //            rq += "</RatePlanCandidates>";
        //            rq += " </AvailRequestSegment>";
        //            rq += "</HotelRateDescriptionRQ>";
        //            rq += "</SOAP-ENV:Body>";
                    
        //            //rq += "<SOAP-ENV:Body>";
        //            //rq += "<HotelPropertyDescriptionRQ xmlns='http://webservices.sabre.com/sabreXML/2011/10' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'  Version='2.3.0'>";
        //            //rq += "<AvailRequestSegment>";
        //            //rq += " <GuestCounts Count='2' />";
        //            //rq += "   <HotelSearchCriteria>";
        //            //rq += "   <Criterion>";
        //            //rq += " <HotelRef HotelCode='0013033'/>";
        //            //rq += " </Criterion>";
        //            //rq += "</HotelSearchCriteria>";
        //            //rq += " <TimeSpan End='04-24' Start='04-22' />";
        //            //rq += "  </AvailRequestSegment>";
        //            //rq += "</HotelPropertyDescriptionRQ>";
        //            //rq += "</SOAP-ENV:Body>";
        //            rq += "</SOAP-ENV:Envelope>";

        //            result = XMLRead.SendQuery(rq);
        //            XMLRead.SaveXMLFile(rq, result, searchid + "_Ratedesc_" + HotelCode);

        //            //result = XMLRead.SendQuery(rq);
        //            //SaveXMLFile(rq, result, "Ratedesc");

                   


        //        }
        //    }

        //    return result;
        //}
        #endregion

      
        public DataTable dtPropDescRS = new DataTable();
        public DataTable dtRoomStay = new DataTable();
        public DataTable dtBasicPropInfo = new DataTable();
        public DataTable dtAddressLines = new DataTable();
        public DataTable dtAwards = new DataTable();
        public DataTable dtDirectConnect = new DataTable();
        public DataTable dtIndexData = new DataTable();
        public DataTable dtPropOptInfo = new DataTable();
        public DataTable dtPropTypeInfo = new DataTable();
        public DataTable dtVendorMsgs = new DataTable();
        public DataTable dtDepositsAccepted = new DataTable();
        public DataTable dtGuaranteesAccepted = new DataTable();
        public DataTable dtGuaranteesText = new DataTable();
        public DataTable dtRateList = new DataTable();
        public DataTable dtRoomRate = new DataTable();
        public DataTable dtAddInfoText = new DataTable();
        public DataTable dtDCAOptions = new DataTable();
        public DataTable dtPaymentCard = new DataTable();
        public DataTable dtHotelRateCode = new DataTable();
        public DataTable dtRate = new DataTable();
        public DataTable dtHotelPricing = new DataTable();
        public DataTable HP_RateRange = new DataTable();
        public DataTable RateRange = new DataTable();
        public DataTable dtRateTypeCode = new DataTable();
        public DataTable dtRoomInfo = new DataTable();
        public string PropertyXmlResult = "";
        public string Properrormsg = "";
        TextInfo txtinfo;
        public HotelProperty()
        {

        }
        public HotelProperty(HPDCondition Hapc, string ContextResult,string searchid)
        {
            try
            {
                string Rqbody = GetRQ(Hapc);
                string result = GetPropertyDesc(Rqbody, ContextResult, searchid, Hapc.HotelCode);
              //  string result = File.ReadAllText("E:\\TripxolHotels_Webapi\\TripxolHotelsWebapi\\HotelProperty\\1677_0017062_PropertyData.xml");
                PropertyXmlResult = result;
                XmlDataDocument xmldoc = new XmlDataDocument();
                xmldoc.LoadXml(result);
                GenerateHotelProperty(xmldoc);
                 }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public HotelProperty(string XmlResult, string HotelCode, string Searchid)
        {
            XmlDataDocument xmldoc = new XmlDataDocument();
            xmldoc.LoadXml(XmlResult);
            GenerateHotelProperty(xmldoc);
        }
        private void GenerateHotelProperty(XmlDataDocument xmldoc)
        {
            CreateTables();
            XmlNode xnod = xmldoc.DocumentElement;
            XmlNode xheader = xnod.ChildNodes[0];
            XmlNode xbody = xnod.ChildNodes[1];
            XmlNode xOTA_HotelAvailRS = xbody.ChildNodes[0];
            txtinfo = culinfo.TextInfo;

            XmlNode xAppResult = xOTA_HotelAvailRS.ChildNodes[0];
            string appresult = GetValue(xAppResult.Attributes["status"]);
            if (appresult.ToLower() == "complete")
            {
                AddXMLPropDescRS(xOTA_HotelAvailRS);
                PrepareRooms();
            }
            else
            {
                Properrormsg = GetChildText(xAppResult.ChildNodes[0].ChildNodes[0], "stl:Message");
            }


        }
        private string GetRQ(HPDCondition hpc)
        {
            string rqvalue="";
            rqvalue += "<SOAP-ENV:Body>";
            rqvalue += "<HotelPropertyDescriptionRQ xmlns='http://webservices.sabre.com/sabreXML/2011/10' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'  Version='2.3.0'>";
            rqvalue += "<AvailRequestSegment>";
            rqvalue += " <GuestCounts Count='" + hpc.guestcount  + "' />";
            rqvalue += "   <HotelSearchCriteria>";
            rqvalue += "   <Criterion>";
            rqvalue += " <HotelRef HotelCode='" + hpc.HotelCode +"'/>";
            rqvalue += " </Criterion>";
            rqvalue += "</HotelSearchCriteria>";
            rqvalue += " <TimeSpan End='" + hpc.checkout  + "' Start='" + hpc.checkin  + "' />";
            rqvalue += "  </AvailRequestSegment>";
            rqvalue += "</HotelPropertyDescriptionRQ>";
            rqvalue += "</SOAP-ENV:Body>";
            return rqvalue;
              
        }
        private void PrepareRooms()
        {
            dtRoomInfo.Columns.Add("RoomRateID");
           // dtRoomInfo.Columns.Add("CurrencyCode");
            dtRoomInfo.Columns.Add("RoomTypedesc");
            dtRoomInfo.Columns.Add("Rate");
            dtRoomInfo.Columns.Add("CanPolicy_Num");
            dtRoomInfo.Columns.Add("CanPolicy_Opt");
            dtRoomInfo.Columns.Add("RoomStayID");
            dtRoomInfo.Columns.Add("CanPolicy_Text");
            dtRoomInfo.Columns.Add("HPTotalAmount");
            dtRoomInfo.Columns.Add("IATACharacter");
            dtRoomInfo.Columns.Add("RPH");
            dtRoomInfo.Columns.Add("RatePernight");
            dtRoomInfo.Columns.Add("Taxes");

            foreach (DataRow dr in dtRoomRate.Rows)
            {
                DataRow[] drrates = dtRate.Select("RoomRateID='" + dr["RoomRateID"].ToString() + "'");
                foreach (DataRow drt in drrates)
                {
                    try
                    {
                        int rno = dtRoomInfo.Rows.Count + 1;
                        DataRow dri = dtRoomInfo.NewRow();
                        dri["RoomRateID"] = dr["RoomRateID"];
                        dri["RoomTypedesc"] = GetAdditionalInfo(dr["RoomRateID"].ToString()).Replace('/', '-');
                        dri["RoomStayID"] = dr["RoomStayID"];
                        dri["CanPolicy_Num"] = dr["CanPolicy_Num"];
                        dri["CanPolicy_Opt"] = dr["CanPolicy_Opt"];
                        dri["CanPolicy_Text"] = dr["CanPolicy_Text"];
                        dri["IATACharacter"] = dr["IATA_ChaIdent"];
                        dri["RPH"] = dr["RPH"];

                        //need to get symbols for currency.
                        dri["Rate"] = GetCurrencySymbol(drt["CurrencyCode"].ToString()) + drt["Amount"].ToString();
                        DataRow[] drHP = dtHotelPricing.Select("RoomRateID='" + dr["RoomRateID"].ToString() + "'");

                        dri["HPTotalAmount"] = GetCurrencySymbol(drt["CurrencyCode"].ToString()) + drHP[0]["HPTotalAmount"].ToString();
                        dri["RatePernight"] = GetRatePerNight(drHP[0]["HPTotalAmount"].ToString());
                        dri["Taxes"] = GetTaxes(drHP[0]);
                        dtRoomInfo.Rows.Add(dri);
                    }
                    catch
                    {
                    }
                }
            }
           

        }
        private string GetRatePerNight(string TotalAmount)
        {
            string rvalue = "";
            double totalamt = Convert.ToDouble(TotalAmount);
            string timestart = dtRoomStay.Rows[0]["TimeSpan_Start"].ToString();
            string timeend = dtRoomStay.Rows[0]["TimeSpan_End"].ToString();

            double dc = 0.0;
            try
            { dc = (Convert.ToDateTime(timeend) - Convert.ToDateTime(timestart)).TotalDays; }
            catch { }
            Double amtpernightt = totalamt / dc;
            rvalue = amtpernightt.ToString("0.##");
            return rvalue;
        }
        private string GetTaxes(DataRow drh)
        {
            string rvalue = "";
            rvalue = drh["TTaxes_Amount"].ToString();
            return rvalue;

        }
        private string GetCurrencySymbol(string CurrencyCode)
        {
            if (CurrencyCode.ToUpper() == "USD")
                return "$";
            else
                return "";
        }
        private string GetAdditionalInfo(string rrid)
        {
            string rvalue="";
            DataRow[] dr = dtAddInfoText.Select("RoomRateID='" + rrid + "'");
            foreach (DataRow dr1 in dr)
            {
                rvalue += dr1["Text"].ToString() + " " ;
            }
           rvalue= txtinfo.ToTitleCase(rvalue.ToLowerInvariant());
             return rvalue;
        }
        private void CreateTables()
        {
            //TravelItinerary 
            dtPropDescRS.Columns.Add("PropertySearchID");
            dtPropDescRS.Columns.Add("SucTimeStamp");
            dtPropDescRS.Columns.Add("HotelCode");

            dtRoomStay.Columns.Add("RoomStayID");
            dtRoomStay.Columns.Add("LgSellTimeIdent");
            dtRoomStay.Columns.Add("TimeSpan_Start");
            dtRoomStay.Columns.Add("TimeSpan_End");
            dtRoomStay.Columns.Add("Duration");
            dtRoomStay.Columns.Add("PropertySearchID");

            #region basicproinfo
            dtBasicPropInfo.Columns.Add("BasicPropInfo_ID");
            dtBasicPropInfo.Columns.Add("HotelCode");
            dtBasicPropInfo.Columns.Add("ChainCode");
            dtBasicPropInfo.Columns.Add("GEO_ConfLevel");
            dtBasicPropInfo.Columns.Add("HotelCityCode");
            dtBasicPropInfo.Columns.Add("HotelName");
            dtBasicPropInfo.Columns.Add("Latitude");
            dtBasicPropInfo.Columns.Add("Longitude");
            dtBasicPropInfo.Columns.Add("NumFloors");
            dtBasicPropInfo.Columns.Add("RPH");
            dtBasicPropInfo.Columns.Add("CountryCode");
            dtBasicPropInfo.Columns.Add("CheckInTime");
            dtBasicPropInfo.Columns.Add("CheckOutTime");
            dtBasicPropInfo.Columns.Add("Phone");
            dtBasicPropInfo.Columns.Add("Fax");
            dtBasicPropInfo.Columns.Add("LongSellTimeIdent");
            dtBasicPropInfo.Columns.Add("LocDescCode");
            dtBasicPropInfo.Columns.Add("LocDescText");
            dtBasicPropInfo.Columns.Add("SpeOffers");
            dtBasicPropInfo.Columns.Add("SpeOffers_Text1");
            dtBasicPropInfo.Columns.Add("SpeOffers_Text2");
            dtBasicPropInfo.Columns.Add("TaxshortText");
            dtBasicPropInfo.Columns.Add("TaxTest1");
            dtBasicPropInfo.Columns.Add("TaxTest2");
            dtBasicPropInfo.Columns.Add("TaxTest3");


            dtBasicPropInfo.Columns.Add("NegotiatedRateCodeMatch");
            dtBasicPropInfo.Columns.Add("PropertyTierLabel");
            dtBasicPropInfo.Columns.Add("RateRange_CurrencyCode");
            dtBasicPropInfo.Columns.Add("RateRange_Max");
            dtBasicPropInfo.Columns.Add("RateRange_Min");
            dtBasicPropInfo.Columns.Add("RoomStayID");
            ////--------------------------
            #endregion

            dtAddressLines.Columns.Add("AddressLines_ID");
            dtAddressLines.Columns.Add("Address");
            dtAddressLines.Columns.Add("BasicPropInfo_ID");


            dtAwards.Columns.Add("Award_ID");
            dtAwards.Columns.Add("AwardProvider");
            dtAwards.Columns.Add("BasicPropInfo_ID");

            dtDirectConnect.Columns.Add("DirConnectID");
            dtDirectConnect.Columns.Add("DCtext");
            dtDirectConnect.Columns.Add("DCValue");
            dtDirectConnect.Columns.Add("BasicPropInfo_ID");


            dtIndexData.Columns.Add("IndexID");
            dtIndexData.Columns.Add("CalcDistance");
            dtIndexData.Columns.Add("CountryState");
            dtIndexData.Columns.Add("DistDirection");
            dtIndexData.Columns.Add("Latitude");
            dtIndexData.Columns.Add("LocationCode");
            dtIndexData.Columns.Add("Longitude");
            dtIndexData.Columns.Add("Point");
            dtIndexData.Columns.Add("TransCode");
            dtIndexData.Columns.Add("UOM");
            dtIndexData.Columns.Add("BasicPropInfo_ID");


            dtPropOptInfo.Columns.Add("PropOptInfoID");
            dtPropOptInfo.Columns.Add("PropOptInfoType");
            dtPropOptInfo.Columns.Add("PropOptInfoInd");
            dtPropOptInfo.Columns.Add("BasicPropInfo_ID");

            dtPropTypeInfo.Columns.Add("PropOptTypeID");
            dtPropTypeInfo.Columns.Add("PropOptTypeType");
            dtPropTypeInfo.Columns.Add("PropOptTypeInd");
            dtPropTypeInfo.Columns.Add("BasicPropInfo_ID");


            dtVendorMsgs.Columns.Add("VendorMsgsID");
            dtVendorMsgs.Columns.Add("MsgType");
            dtVendorMsgs.Columns.Add("MsgText");
            dtVendorMsgs.Columns.Add("BasicPropInfo_ID");



            dtDepositsAccepted.Columns.Add("DepositID");
            dtDepositsAccepted.Columns.Add("PayCardCode");
            dtDepositsAccepted.Columns.Add("PayCardType");
            dtDepositsAccepted.Columns.Add("BasicPropInfo_ID");

            dtGuaranteesAccepted.Columns.Add("GuaranteeID");
            dtGuaranteesAccepted.Columns.Add("PayCardCode");
            dtGuaranteesAccepted.Columns.Add("PayCardType");
            dtGuaranteesAccepted.Columns.Add("BasicPropInfo_ID");


            dtGuaranteesText.Columns.Add("GuaranteeID");
            dtGuaranteesText.Columns.Add("Text");
            dtGuaranteesText.Columns.Add("BasicPropInfo_ID");


            //dtRateList.Columns.Add("RateListId");
            //dtRateList.Columns.Add("RateCode");
            //dtRateList.Columns.Add("RateDesc");
            //dtRateList.Columns.Add("BasicPropInfo_ID");
            #region dtroomrate
            dtRoomRate.Columns.Add("RoomRateID");
            dtRoomRate.Columns.Add("AdvResPeriod");
            dtRoomRate.Columns.Add("ClientID");
            dtRoomRate.Columns.Add("CurrencyCode");
            dtRoomRate.Columns.Add("DirectConnect");
            dtRoomRate.Columns.Add("GuaRateProg");
            dtRoomRate.Columns.Add("GuaSurchrgReq");

            dtRoomRate.Columns.Add("IATA_ChaIdent");
            dtRoomRate.Columns.Add("IATA_ProdIdent");
            dtRoomRate.Columns.Add("LowInvThreshold");
            dtRoomRate.Columns.Add("RateAccessCode");
            dtRoomRate.Columns.Add("RateCategory");
            dtRoomRate.Columns.Add("RateChangeInd");

            dtRoomRate.Columns.Add("RateConversionInd");
            dtRoomRate.Columns.Add("ReturnOfRateInd");
            dtRoomRate.Columns.Add("RateLevelCode");
            dtRoomRate.Columns.Add("RoomLocationCode");
            dtRoomRate.Columns.Add("RPH");
            dtRoomRate.Columns.Add("SpecialOffer");
            dtRoomRate.Columns.Add("XPM_GuaReq");
            dtRoomRate.Columns.Add("CanPolicy_Num");
            dtRoomRate.Columns.Add("CanPolicy_Opt");
            dtRoomRate.Columns.Add("CanPolicy_Text");
            dtRoomRate.Columns.Add("NonCommission");
            dtRoomRate.Columns.Add("Commission_Text");
            dtRoomRate.Columns.Add("Taxes");
            dtRoomRate.Columns.Add("RoomStayID");
            #endregion


            dtAddInfoText.Columns.Add("AddInfoTextID");
            dtAddInfoText.Columns.Add("Text");
            dtAddInfoText.Columns.Add("RoomRateID");



            dtDCAOptions.Columns.Add("DCAOptID");
            dtDCAOptions.Columns.Add("DCAOptType");
            dtDCAOptions.Columns.Add("DCAOptText");
            dtDCAOptions.Columns.Add("RoomRateID");

            DataTable dtRRPayment = new DataTable();
            dtRateTypeCode.Columns.Add("RRPayment_ID");
            dtRateTypeCode.Columns.Add("PayCardCode");
            dtRateTypeCode.Columns.Add("RoomRateID");

            DataTable dtHotelRateCodes = new DataTable();
            dtHotelRateCodes.Columns.Add("HRateCode_ID");
            dtHotelRateCodes.Columns.Add("HRateCode_Text");
            dtHotelRateCodes.Columns.Add("RoomRateID");

            //DataTable dtRoomRates = new DataTable();
            dtRate.Columns.Add("RRate_ID");
            dtRate.Columns.Add("Amount");
            dtRate.Columns.Add("CngIndicator");
            dtRate.Columns.Add("CurrencyCode");
            dtRate.Columns.Add("HRD_ReqforSell");
            dtRate.Columns.Add("PkgIndicator");
            dtRate.Columns.Add("RateConvIndicator");
            dtRate.Columns.Add("RoomOnRequest");
            dtRate.Columns.Add("ReturnOfRateInd"); 
            dtRate.Columns.Add("RoomLocCode");
            dtRate.Columns.Add("RoomTypeCode");
            dtRate.Columns.Add("AddGuestAmt_NumCribs");
            dtRate.Columns.Add("AddGuestAmt_NumChild");
            dtRate.Columns.Add("AddGuestAmt_NumAdults");
            dtRate.Columns.Add("AddGuestAmt_Max");
            dtRate.Columns.Add("AGA_Charges_ExPer");
            dtRate.Columns.Add("AGA_Charges_Crib");
            dtRate.Columns.Add("AGA_Charges_ChildRollAway");
            dtRate.Columns.Add("AGA_Charges_AdultRollAway");
          
            dtRate.Columns.Add("RoomRateID");


            dtHotelPricing.Columns.Add("HotelPricingID");
            dtHotelPricing.Columns.Add("HPTotalAmount");
            dtHotelPricing.Columns.Add("Disclaimer1");
            dtHotelPricing.Columns.Add("Disclaimer2");
            dtHotelPricing.Columns.Add("TTaxes_Amount");
            dtHotelPricing.Columns.Add("TaxAmount");
            dtHotelPricing.Columns.Add("TTaxes_Field1");
            dtHotelPricing.Columns.Add("TTaxes_Field2");
            dtHotelPricing.Columns.Add("TTaxes_Field3");
            dtHotelPricing.Columns.Add("TTText1");
            dtHotelPricing.Columns.Add("TTText2");
            dtHotelPricing.Columns.Add("TTText3");
            dtHotelPricing.Columns.Add("TTText4");
            dtHotelPricing.Columns.Add("TSurcharges_Amount");
            dtHotelPricing.Columns.Add("TSurchargesOne");
            dtHotelPricing.Columns.Add("TSurchargesTwo");
            dtHotelPricing.Columns.Add("TSurchargesThree");
            dtHotelPricing.Columns.Add("TSurchargesFour");
            dtHotelPricing.Columns.Add("TSCText1");
            dtHotelPricing.Columns.Add("TSCText2");
            dtHotelPricing.Columns.Add("TSCText3");
            dtHotelPricing.Columns.Add("TSCText4");
            dtHotelPricing.Columns.Add("RoomRateID");



           HP_RateRange.Columns.Add("HP_RateRangeID");
           HP_RateRange.Columns.Add("Amount");
           HP_RateRange.Columns.Add("Taxes");
           HP_RateRange.Columns.Add("Surcharges");
           HP_RateRange.Columns.Add("ExpireDate");
           HP_RateRange.Columns.Add("EffectiveDate");
           HP_RateRange.Columns.Add("HotelPricingID");

            //dtRateTypeCode.Columns.Add("RateTypeCode_ID");
            //dtRateTypeCode.Columns.Add("RateTypeCode");
            //dtRateTypeCode.Columns.Add("RoomRateID");
        }
        private void AddXMLPropDescRS(XmlNode pxPropDescRS)
        {
            int rno = dtPropDescRS.Rows.Count;
            FilldtPropDescRS(rno, pxPropDescRS);
            foreach (XmlNode xn in pxPropDescRS.ChildNodes)
            {
                if (xn.Name.ToLower() == "roomstay")
                {
                    AddXMLRoomStay(xn, rno);
                }
            }
        }
        private void AddXMLRoomStay(XmlNode pxRoomStay, int ParentRno)
        {
            int rno = dtRoomStay.Rows.Count;
            FilldtRoomstay(rno, pxRoomStay, ParentRno);
            foreach (XmlNode xn in pxRoomStay.ChildNodes)
            {
                if (xn.Name.ToLower() == "basicpropertyinfo")
                {
                    AddXMLBPInfo(xn, ParentRno);
                }

                if (xn.Name.ToLower() == "guarantee")
                {
                    AddXMLGuarantee(xn, ParentRno);
                }

                if (xn.Name.ToLower() == "roomrates")
                {
                    AddXMLRoomRates(xn, ParentRno);
                }

            }
        }
        #region BPINfo
        private void AddXMLBPInfo(XmlNode pxBPInfo, int ParentRno)
        {
            int rno = dtBasicPropInfo.Rows.Count;
            FilldtBasicPropertyInfo(rno, pxBPInfo, ParentRno);
            foreach (XmlNode xn in pxBPInfo.ChildNodes)
            {
                if (xn.Name.ToLower() == "address")
                {
                    AddXMLAddress(xn, rno);
                }
                if (xn.Name.ToLower() == "awards")
                {
                    AddXMLAwards(xn, ParentRno);
                }
                if (xn.Name.ToLower() == "directconnect")
                {
                    AddXMLDirectConnect(xn, rno);
                }
                if (xn.Name.ToLower() == "indexdata")
                {
                    AddXMLIndexData(xn, rno);
                }
                if (xn.Name.ToLower() == "propertyoptioninfo")
                {
                    AddXMLPropOptInfo(xn, rno);
                }

                if (xn.Name.ToLower() == "propertytypeinfo")
                {
                    AddXMLPropTypeInfo(xn, rno);
                }

                if (xn.Name.ToLower() == "vendormessages")
                {
                    AddXMLVendorMsgs(xn, rno);
                }

            }
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
        private void AddXMLAwards(XmlNode PNode, int ParentRno)
        {
            foreach (XmlNode xn in PNode.ChildNodes)
            {
                if (xn.Name.ToLower() == "awardprovider")
                {
                    int rno = dtAwards.Rows.Count;
                    FilldtAwardProvider(rno, xn, ParentRno);
                }
            }
        }
        private void AddXMLDirectConnect(XmlNode PNode, int ParentRno)
        {
            foreach (XmlNode xn in PNode.ChildNodes)
            {
                int rno = dtDirectConnect.Rows.Count;
                FilldtDirectConnect(rno, xn, ParentRno);
            }
        }
        private void AddXMLIndexData(XmlNode PNode, int ParentRno)
        {

            foreach (XmlNode xn in PNode.ChildNodes)
            {
                //if (xn.Name.ToLower() == "text")
                //{
                int rno = dtIndexData.Rows.Count;
                FilldtIndexData(rno, xn, ParentRno);
                //}
            }
        }
        private void AddXMLLocDescText(XmlNode PNode, int ParentRno)
        {

            foreach (XmlNode xn in PNode.ChildNodes)
            {
                if (xn.Name.ToLower() == "text")
                {
                    //  int rno = dtLocDescText.Rows.Count;
                    int rno = 0;
                    FilldtLocDescText(rno, PNode, ParentRno);
                }
            }
        }
        private void AddXMLProperty(XmlNode PNode, int ParentRno)
        {

            foreach (XmlNode xn in PNode.ChildNodes)
            {
                if (xn.Name.ToLower() == "text")
                {
                    //int rno = dtProperty.Rows.Count;
                    //FilldtProperty(rno, PNode, ParentRno);
                }
            }
        }
        private void AddXMLPropOptInfo(XmlNode PNode, int ParentRno)
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
        private void AddXMLPropTypeInfo(XmlNode PNode, int ParentRno)
        {

            foreach (XmlNode xn in PNode.ChildNodes)
            {
                //if (xn.Name.ToLower() == "text")
                //{
                int rno = dtPropTypeInfo.Rows.Count;
                FilldtPropertyTypeInfo(rno, xn, ParentRno);
                //}
            }
        }
        private void AddXMLVendorMsgs(XmlNode PNode, int ParentRno)
        {
            foreach (XmlNode xn1 in PNode.ChildNodes)
            {
                foreach (XmlNode xn2 in xn1.ChildNodes)
                {
                    int rno = dtVendorMsgs.Rows.Count;
                    FilldtVendorMsgs(rno, xn1, xn2, ParentRno);
                }
            }
        }
        #endregion
        #region Guarantee
        private void AddXMLGuarantee(XmlNode PNode, int ParentRno)
        {
            foreach (XmlNode xn in PNode.ChildNodes)
            {
                if (xn.Name.ToLower() == "depositsaccepted")
                {
                    AddXMLDepositsAccepted(xn, ParentRno);
                }
                if (xn.Name.ToLower() == "guaranteesaccepted")
                {
                    AddXMLGuaranteesAccepted(xn, ParentRno);
               }
            }
        }
        private void AddXMLDepositsAccepted(XmlNode PNode, int ParentRno)
        {
            foreach (XmlNode xn in PNode.ChildNodes)
            {
                int rno = dtDepositsAccepted.Rows.Count;
                FilldtDepositsAccepted(rno, xn, ParentRno);
            }
        }
        private void AddXMLGuaranteesAccepted(XmlNode PNode, int ParentRno)
        {
            foreach (XmlNode xn in PNode.ChildNodes)
            {
                if (xn.Name.ToLower() == "paymentcard")
                {
                    int rno = dtGuaranteesAccepted.Rows.Count;
                    FilldtGuaranteesAccepted(rno, xn, ParentRno);
                }
                if (xn.Name.ToLower() == "text")
                {
                    int rno = dtGuaranteesText.Rows.Count;
                    FilldtGuaranteesText(rno, xn, ParentRno);
                }

            }
        }
        //private void AddXMLGuaranteesText(XmlNode PNode, int ParentRno)
        //{
        //    foreach (XmlNode xn in PNode.ChildNodes)
        //    {
        //        int rno = dtDepositsAccepted.Rows.Count;
        //        FilldtGuaranteesAccepted(rno, xn, ParentRno);
        //    }
        //}
        #endregion
        #region Roomrates
        private void AddXMLRoomRates(XmlNode PNode, int ParentRno)
        {
            //int rno = dtRoomRate.Rows.Count;
            //FilldtRoomRate(rno, PNode, ParentRno);
            foreach (XmlNode xn in PNode.ChildNodes)
            {
                if (xn.Name.ToLower() == "roomrate")
                {
                    AddXMLRoomRate(xn, ParentRno);
                }
            }

        }
        private void AddXMLRoomRate(XmlNode PNode, int ParentRno)
        {
            int rno = dtRoomRate.Rows.Count;
            FilldtRoomRate(rno, PNode, ParentRno);
            foreach (XmlNode xn in PNode.ChildNodes)
            {
                if (xn.Name.ToLower() == "additionalinfo")
                {
                    AddXMLAdditionalInfo(xn, rno);
                }
                if (xn.Name.ToLower() == "hotelratecode")
                {
                    AddXMLAdditionalInfo(xn, rno);
                }
                if (xn.Name.ToLower() == "rates")
                {
                    AddXMLrates(xn, rno);
                }
            }
        }
        private void AddXMLrates(XmlNode PNode, int ParentRno)
        {
            foreach (XmlNode xn in PNode.ChildNodes)
            {
                if (xn.Name.ToLower() == "rate")
                {
                    AddXMLrate(xn, ParentRno);
                }
            }
        }
        private void AddXMLrate(XmlNode PNode, int ParentRno)
        {
            int rno = dtRate.Rows.Count;
            FilldtRate(rno, PNode, ParentRno);
            foreach (XmlNode xn in PNode.ChildNodes)
            {
                if (xn.Name.ToLower() == "hoteltotalpricing")
                {
                    AddXMLHotelTotalPricing(xn, ParentRno);
                }
            }
        }
        private void AddXMLHotelTotalPricing(XmlNode PNode, int ParentRno)
        {

            int rno = dtHotelPricing.Rows.Count;
            FilldtHotelPricing(rno, PNode, ParentRno);

            foreach (XmlNode xn in PNode.ChildNodes)
            {
                if (xn.Name.ToLower() == "raterange")
                {
                    AddXMLRateRange(xn, ParentRno);
                }
            }
        }
        private void AddXMLRateRange(XmlNode PNode, int ParentRno)
        {

            int rno = dtHotelPricing.Rows.Count;
            FilldtHPRateRange(rno, PNode, ParentRno);

           
        }
        private void AddXMLAdditionalInfo(XmlNode PNode, int ParentRno)
        {
            foreach (XmlNode xn in PNode.ChildNodes)
            {
                if (xn.Name.ToLower() == "text")
                {
                    AddXMLAdditionalInfoText(xn, ParentRno);
                }
            }

        }
        private void AddXMLAdditionalInfoText(XmlNode PNode, int ParentRno)
        {
            int rno = dtAddInfoText.Rows.Count;
            FilldtAddInfoText(rno, PNode, ParentRno);
        }
        //private void AddXMLRoomRate(XmlNode PNode, int ParentRno)
        //{
        //    int rno = dtRoomRate.Rows.Count;
        //    FilldtRoomRate(rno, PNode, ParentRno);
        //    //foreach (XmlNode xn in PNode.ChildNodes)
        //    //{
        //    //    if (xn.Name.ToLower() == "roomrate")
        //    //    {
        //    //        AddXMLroomrate(xn, rno);
        //    //    }
        //    //}

        //}
        private void AddXMLRateTypeCode(XmlNode PNode, int ParentRno)
        {
            int rno = dtRateTypeCode.Rows.Count;
            FilldtRateTypeCode(rno, PNode, ParentRno);
        }
        #endregion
        private void FilldtHPRateRange(int rno, XmlNode pNode, int ParentNo)
        {
            DataRow dr = HP_RateRange.NewRow();

            dr["HP_RateRangeID"] = rno;
            dr["Amount"] = GetValue(pNode.Attributes["Amount"]);
            dr["Taxes"] = GetValue(pNode.Attributes["Taxes"]);
            dr["Surcharges"] = GetValue(pNode.Attributes["Surcharges"]);
            dr["ExpireDate"] = GetValue(pNode.Attributes["ExpireDate"]);
            dr["EffectiveDate"] = GetValue(pNode.Attributes["EffectiveDate"]);
            dr["HotelPricingID"] = ParentNo;
            HP_RateRange.Rows.Add(dr);




        }
        private void FilldtHotelPricing(int rno, XmlNode pNode, int ParentNo)
        {
            DataRow dr = dtHotelPricing.NewRow();

            dr["HotelPricingID"]=rno;
           //dr["Disclaimer1"]=GetChildTextM(pnode,"Disclaimer");
           //dr["Disclaimer2"]=GetChildTextM(pnode,"Disclaimer");
            dr["HPTotalAmount"] = GetValue(pNode.Attributes["Amount"]);
           dr["TTaxes_Amount"]=GetCNAttrValue("TotalTaxes" ,"Amount",pNode); 
           dr["TaxAmount"]=GetCNCNAttrValue("TotalTaxes","Tax" ,"Amount",pNode);
           dr["TTaxes_Field1"]=GetChildChildText(pNode, "TotalTaxes","TaxFieldOne");
           dr["TTaxes_Field2"]=GetChildChildText(pNode, "TotalTaxes","TaxFieldTwo");
           dr["TTaxes_Field3"]=GetChildChildText(pNode, "TotalTaxes","TaxFieldThree");
           //dr["TTText1"]=GetChildTextM(pnode,"Text");
           //dr["TTText2"]=GetChildTextM(pnode,"Text");
           //dr["TTText3"]=GetChildTextM(pnode,"Text");
           //dr["TTText4"]=GetChildTextM(pnode,"Text");
           dr["TSurcharges_Amount"]=GetCNAttrValue("TotalSurcharges" ,"Amount",pNode); 
           dr["TSurchargesOne"]=GetChildChildText(pNode, "TotalSurcharges","SurchargeOne");
           dr["TSurchargesTwo"]=GetChildChildText(pNode, "TotalSurcharges","SurchargeTwo");
           dr["TSurchargesThree"]=GetChildChildText(pNode, "TotalSurcharges","SurchargeThree");
           dr["TSurchargesFour"]=GetChildChildText(pNode, "TotalSurcharges","SurchargeFour");
           //dr["TSCText1"]=GetChildTextM(pnode,"Text");
           //dr["TSCText2"]=GetChildTextM(pnode,"Text");
           //dr["TSCText3"]=GetChildTextM(pnode,"Text");
           //dr["TSCText4"]=GetChildTextM(pnode,"Text");
           dr["RoomRateID"] = ParentNo;
            dtHotelPricing.Rows.Add(dr);
        }
        private void FilldtDepositsAccepted(int rno, XmlNode pNode, int ParentNo)
        {
            DataRow dr = dtDepositsAccepted.NewRow();

            dr["DepositID"] = rno;
            dr["PayCardCode"] = GetValue(pNode.Attributes["Code"]);
            dr["PayCardType"] = GetValue(pNode.Attributes["Type"]);
            dr["BasicPropInfo_ID"] = ParentNo;
            dtDepositsAccepted.Rows.Add(dr);
        }
        private void FilldtGuaranteesAccepted(int rno, XmlNode pNode, int ParentNo)
        {
            DataRow dr = dtGuaranteesAccepted.NewRow();

            dr["GuaranteeID"] = rno;
            dr["PayCardCode"] = GetValue(pNode.Attributes["Code"]);
            dr["PayCardType"] = GetValue(pNode.Attributes["Type"]);
            dr["BasicPropInfo_ID"] = ParentNo;
            dtGuaranteesAccepted.Rows.Add(dr);
        }
        private void FilldtGuaranteesText(int rno, XmlNode pNode, int ParentNo)
        {
            DataRow dr = dtGuaranteesText.NewRow();
            dr["GuaranteeID"] = rno;
            try { dr["Text"] = pNode.InnerText; }
            catch { }
            dr["BasicPropInfo_ID"] = ParentNo;
            dtGuaranteesText.Rows.Add(dr);
        }
        private void FilldtDirectConnect(int rno, XmlNode pNode, int ParentRno)
        {
            DataRow dr = dtDirectConnect.NewRow();
            dr["DirConnectID"] = rno;
            try
            {
                dr["DCtext"] = pNode.Name;
            }
            catch
            {
            }
            dr["DCValue"] = GetValue(pNode.Attributes[0]); ;
            dr["BasicPropInfo_ID"] = ParentRno;
            dtDirectConnect.Rows.Add(dr);

        }
        private void FilldtIndexData(int rno, XmlNode pNode, int ParentNo)
        {
            DataRow dr = dtIndexData.NewRow();
            dr["IndexID"] = rno;
            dr["CalcDistance"] = GetValue(pNode.Attributes["CalculatedDistance"]);
            dr["CountryState"] = GetValue(pNode.Attributes["CountryState"]);
            dr["DistDirection"] = GetValue(pNode.Attributes["DistanceDirection"]);
            dr["Latitude"] = GetValue(pNode.Attributes["Latitude"]);
            dr["LocationCode"] = GetValue(pNode.Attributes["LocationCode"]);
            dr["Longitude"] = GetValue(pNode.Attributes["TimeSpan_Start"]);
            dr["Point"] = GetValue(pNode.Attributes["Point"]);
            dr["TransCode"] = GetValue(pNode.Attributes["TransportationCode"]);
            dr["UOM"] = GetValue(pNode.Attributes["UnitOfMeasure"]);
            dr["BasicPropInfo_ID"] = ParentNo;
            dtIndexData.Rows.Add(dr);
        }
        private void FilldtRate(int rno, XmlNode pNode, int ParentNo)
        {
           DataRow dr = dtRate.NewRow();
           dr["RRate_ID"] = rno;
           dr["Amount"] = GetValue(pNode.Attributes["Amount"]);
           dr["CngIndicator"] = GetValue(pNode.Attributes["ChangeIndicator"]);
           dr["CurrencyCode"] = GetValue(pNode.Attributes["CurrencyCode"]);
           dr["HRD_ReqforSell"] = GetValue(pNode.Attributes["HRD_RequiredForSell"]);
           dr["PkgIndicator"] = GetValue(pNode.Attributes["PackageIndicator"]);
           dr["RateConvIndicator"] = GetValue(pNode.Attributes["RateConversionInd"]);
           dr["RoomLocCode"] = GetValue(pNode.Attributes["RoomLocationCode"]);
           dr["RoomTypeCode"] = GetValue(pNode.Attributes["RoomTypeCode"]);
           dr["RoomOnRequest"] = GetValue(pNode.Attributes["RoomOnRequest"]);
           dr["ReturnOfRateInd"] = GetValue(pNode.Attributes["ReturnOfRateInd"]);
           dr["AddGuestAmt_NumCribs"] = GetCNCNAttrValue("AdditionalGuestAmounts","AdditionalGuestAmount","NumCribs",pNode);
           dr["AddGuestAmt_NumChild"] = GetCNCNAttrValue("AdditionalGuestAmounts", "AdditionalGuestAmount", "NumChildren", pNode);
           dr["AddGuestAmt_NumAdults"] = GetCNCNAttrValue("AdditionalGuestAmounts", "AdditionalGuestAmount", "NumAdults", pNode);
           dr["AddGuestAmt_Max"] = GetCNCNAttrValue("AdditionalGuestAmounts", "AdditionalGuestAmount", "MaxExtraPersonsAllowed", pNode); ;
           dr["AGA_Charges_ExPer"]=GetValue(pNode.Attributes["ExtraPerson"]);
           //dr["AGA_Charges_Crib"]=GetValue(pNode.Attributes["Crib"]);
           //dr["AGA_Charges_ChildRollAway"] = GetValue(pNode.Attributes["ChildRollAway"]);
           //dr["AGA_Charges_AdultRollAway"] = GetValue(pNode.Attributes["AdultRollAway"]);
           dr["RoomRateID"] = ParentNo;
                dtRate.Rows.Add(dr);
        }
        private void FilldtAddInfoText(int rno, XmlNode pNode, int ParentNo)
        {
            DataRow dr = dtAddInfoText.NewRow();
            dr["AddInfoTextID"] = rno;
            dr["Text"] = pNode.InnerText;
            dr["RoomRateID"] = ParentNo;
            dtAddInfoText.Rows.Add(dr);
        }
        private void FilldtVendorMsgs(int rno, XmlNode pNode, XmlNode node2, int ParentRno)
        {
            DataRow dr = dtVendorMsgs.NewRow();
            dr["VendorMsgsID"] = rno;
            try
            {
                dr["MsgType"] = pNode.Name;
            }
            catch
            {
            }
            try { dr["MsgText"] = node2.InnerText + " " ; }
            catch { };
            dr["BasicPropInfo_ID"] = ParentRno;
            dtVendorMsgs.Rows.Add(dr);
        }
        private void FilldtPropDescRS(int rno, XmlNode pxPropDescRS)
        {
            DataRow dr = dtPropDescRS.NewRow();
            dr["PropertySearchID"] = rno;
            dr["SucTimeStamp"] = GetSucTimeStamp(pxPropDescRS);
            dr["HotelCode"] = HotelCode;
            dtPropDescRS.Rows.Add(dr);
        }
        private void FilldtRoomstay(int rno, XmlNode pNode, int ParentNo)
        {
            DataRow dr = dtRoomStay.NewRow();
            dr["RoomStayID"] = rno;
            dr["LgSellTimeIdent"] = GetChildText(pNode, "LongSellTimeIdentifier");
            dr["TimeSpan_Start"] = GetCNAttrValue("TimeSpan", "Start", pNode);
            dr["TimeSpan_End"] = GetCNAttrValue("TimeSpan", "End", pNode);
            dr["Duration"] = GetValue(pNode.Attributes["Duration"]);
            dr["PropertySearchID"] = ParentNo;
            dtRoomStay.Rows.Add(dr);
        }
        private void FilldtBasicPropertyInfo(int rno, XmlNode pNode, int ParentRno)
        {
            DataRow dr = dtBasicPropInfo.NewRow();
            dr["BasicPropInfo_ID"] = rno;
            dr["HotelCode"] = HotelCode;
            dr["ChainCode"] = GetValue(pNode.Attributes["ChainCode"]);
            dr["GEO_ConfLevel"] = GetValue(pNode.Attributes["GeoConfidenceLevel"]);
            dr["HotelCityCode"] = GetValue(pNode.Attributes["HotelCityCode"]);
            dr["HotelCode"] = GetValue(pNode.Attributes["HotelCode"]);
            dr["HotelName"] = GetValue(pNode.Attributes["HotelName"]);
            dr["Latitude"] = GetValue(pNode.Attributes["Latitude"]);
            dr["Longitude"] = GetValue(pNode.Attributes["Longitude"]);
            dr["NumFloors"] = GetValue(pNode.Attributes["NumFloors"]);
            dr["RPH"] = GetPhoneFax(pNode, "RPH");
            dr["CountryCode"] = GetChildChildText(pNode, "Address", "CountryCode");
            dr["CheckInTime"] = GetChildText(pNode, "CheckInTime");
            dr["CheckOutTime"] = GetChildText(pNode, "CheckOutTime");
            dr["Phone"] = GetPhoneFax(pNode, "Phone");
            dr["Fax"] = GetPhoneFax(pNode, "Fax");
            dr["LongSellTimeIdent"] = GetChildText(pNode, "LongSellTimeIdentifier");
            //dr["Alt_Avail"] = GetCNCNAttrValue("DirectConnect", "Alt_Avail", "Ind", pNode);
            //dr["DC_AvailParticipant"] = GetCNCNAttrValue("DirectConnect", "DC_AvailParticipant", "Ind", pNode);
            //dr["DC_SellParticipant"] = GetCNCNAttrValue("DirectConnect", "DC_SellParticipant", "Ind", pNode);
            //dr["RatesExceedMax"] = GetCNCNAttrValue("DirectConnect", "RatesExceedMax", "Ind", pNode);
            //dr["UnAvail"] = GetCNCNAttrValue("DirectConnect", "UnAvail", "Ind", pNode);
            dr["LocDescCode"] = GetCNAttrValue("LocationDescription", "Code", pNode);
            dr["LocDescText"] = GetLocationText(pNode, "Text");
            dr["SpeOffers"] = GetCNAttrValue("SpecialOffers", "Ind", pNode);
            dr["SpeOffers_Text1"] = GetCNAttrValue("SpecialOffers", "Ind", pNode);
            dr["SpeOffers_Text2"] = GetCNAttrValue("SpecialOffers", "Ind", pNode);
            dr["TaxshortText"] = GetCNAttrValue("Taxes", "shorttext", pNode);
            //dr["Taxes_Text1"] = GetCNAttrValue("Taxes", "Ind", pNode);
            //dr["Taxes_Text2"] = GetCNAttrValue("Taxes", "Ind", pNode);
            //dr["Taxes_Text3"] = GetCNAttrValue("Taxes", "Ind", pNode);

            dr["RoomStayID"] = ParentRno;
            dtBasicPropInfo.Rows.Add(dr);
        }
        private void FilldtAddressLine(int rno, XmlNode pNode, int ParentRno)
        {

            DataRow dr = dtAddressLines.NewRow();
            dr["AddressLines_ID"] = rno;
            dr["Address"] = pNode.InnerText;
            dr["BasicPropInfo_ID"] = ParentRno;
            dtAddressLines.Rows.Add(dr);
        }
        private void FilldtAwardProvider(int rno, XmlNode pNode, int ParentRno)
        {
            DataRow dr = dtAwards.NewRow();
            dr["Award_ID"] = rno;
            dr["AwardProvider"] = pNode.InnerText;
            dr["BasicPropInfo_ID"] = ParentRno;
            dtAwards.Rows.Add(dr);
        }
        private void FilldtLocDescText(int rno, XmlNode pNode, int ParentRno)
        {
            //DataRow dr = dtLocDescText.NewRow();
            //dr["LocDesc_Text_ID"] = rno;
            //dr["Text"] = GetChildText(pNode, "text");
            //dr["BasicPropInfo_ID"] = ParentRno;
            //dtLocDescText.Rows.Add(dr);
        }
        private void FilldtProperty(int rno, XmlNode pNode, int ParentRno)
        {
            //DataRow dr = dtProperty.NewRow();
            //dr["PropertyID"] = rno;
            //dr["Rating"] = GetValue(pNode.Attributes["Rating"]);
            //dr["Text"] = GetChildText(pNode, "text");
            //dr["BasicPropInfo_ID"] = ParentRno;
            //dtProperty.Rows.Add(dr);
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
            if (pNode.Name == "CarRentalCounter")
            {
                dr["PropOptInfoInd"] = pNode.InnerText;
            }
            else if (pNode.Name == "Parking")
            {
                dr["PropOptInfoInd"] = pNode.InnerText;
            }
            else
            {
                dr["PropOptInfoInd"] = GetValue(pNode.Attributes[0]);
            }
            dr["BasicPropInfo_ID"] = ParentRno;
            dtPropOptInfo.Rows.Add(dr);

        }
        private void FilldtPropertyTypeInfo(int rno, XmlNode pNode, int ParentRno)
        {

            DataRow dr = dtPropTypeInfo.NewRow();
            dr["PropOptTypeID"] = rno;
            try
            {
                dr["PropOptTypeType"] = pNode.Name;
            }
            catch
            {
            }
            dr["PropOptTypeInd"] = GetValue(pNode.Attributes[0]);
            dr["BasicPropInfo_ID"] = ParentRno;
            dtPropTypeInfo.Rows.Add(dr);

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
            //  dr["AdvResPeriod"] = GetValue(pNode.Attributes["GuaranteeSurchargeRequired"]);
            dr["ClientID"] = GetValue(pNode.Attributes["ClientID"]);
            // dr["CurrencyCode"] = GetValue(pNode.Attributes["XPM_GuaranteeRequired"]);
            dr["DirectConnect"] = GetValue(pNode.Attributes["DirectConnect"]);
            dr["GuaRateProg"] = GetValue(pNode.Attributes["GuaranteedRateProgram"]);
            dr["GuaSurchrgReq"] = GetValue(pNode.Attributes["GuaranteeSurchargeRequired"]);
            dr["IATA_ChaIdent"] = GetValue(pNode.Attributes["IATA_CharacteristicIdentification"]);
            dr["IATA_ProdIdent"] = GetValue(pNode.Attributes["IATA_ProductIdentification"]);
            dr["LowInvThreshold"] = GetValue(pNode.Attributes["LowInventoryThreshold"]);
            dr["RateAccessCode"] = GetValue(pNode.Attributes["RateAccessCode"]);
            dr["RateCategory"] = GetValue(pNode.Attributes["RateCategory"]);
            dr["RateChangeInd"] = GetValue(pNode.Attributes["RateChangeInd"]);

            dr["RateConversionInd"] = GetValue(pNode.Attributes["RateConversionInd"]);
            dr["ReturnOfRateInd"] = GetValue(pNode.Attributes["RateLevelCode"]);
            dr["RateLevelCode"] = GetValue(pNode.Attributes["XPM_GuaranteeRequired"]);
            // dr["RoomLocationCode"] = GetCNCNAttrValue("AdditionalInfo", "CancelPolicy", "Option", pNode);
            dr["RPH"] = GetValue(pNode.Attributes["RPH"]);
            dr["SpecialOffer"] = GetValue(pNode.Attributes["SpecialOffer"]);
            // dr["XPM_GuaReq"] = GetValue(pNode.Attributes["GuaranteeSurchargeRequired"]);
            dr["CanPolicy_Opt"] = GetCNCNAttrValue("AdditionalInfo", "CancelPolicy", "Option", pNode);
            dr["CanPolicy_Num"] = GetCNCNAttrValue("AdditionalInfo", "CancelPolicy", "Numeric", pNode);
            //dr["CanPolicy_Text"] = GetCNCNAttrValue("AdditionalInfo", "CancelPolicy", "Option", pNode);
            dr["NonCommission"] = GetCNCNAttrValue("AdditionalInfo", "Commission", "NonCommission", pNode);
            dr["Commission_Text"] = GetChildChildText(pNode, "AdditionalInfo", "Commission");
            dr["Commission_Text"] = GetChildChildText(pNode, "AdditionalInfo", "Commission");
            //dr["Taxes"] = GetChildText(pNode, "HotelRateCode");
            dr["RoomStayID"] = ParentRno;
            dtRoomRate.Rows.Add(dr);
        }
        #region supportfunctions
        private string GetLocationText(XmlNode pNode, string item)
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
        private string GetChildChildText(XmlNode pnode, string node1, string node2)
        {
            string rvalue = "";
            foreach (XmlNode xn in pnode.ChildNodes)
            {
                if (xn.Name.ToLower() == node1.ToLower())
                {
                    foreach (XmlNode xn2 in xn.ChildNodes)
                    {
                        if (xn2.Name.ToLower() == node2.ToLower())
                        {
                            try { rvalue = xn2.InnerText; }
                            catch { }
                        }
                    }
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
        private string GetSucTimeStamp(XmlNode pxOTA_HotelAvailRS)
        {
            string rvalue="";
            try
            {
                rvalue = GetValue(pxOTA_HotelAvailRS.ChildNodes[0].ChildNodes[0].Attributes["timeStamp"]);
            }
            catch { }
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
        #endregion
    }
// HttpWebRequest httprequest = (HttpWebRequest)HttpWebRequest.Create("https://webservices.havail.sabre.com");//prods




//public static string createsession(string searchid)
//{
//    string result = "";
//    xml_rq = "";

//    xml_rq += " <SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:eb='http://www.ebxml.org/namespaces/messageHeader' xmlns:xlink='http://www.w3.org/1999/xlink' xmlns:xsd='http://www.w3.org/1999/XMLSchema'>";
//    xml_rq += " <SOAP-ENV:Header><eb:MessageHeader SOAP-ENV:mustUnderstand='1' eb:version='1.0'>";
//    xml_rq += " <eb:ConversationId>Session</eb:ConversationId>";
//    xml_rq += "  <eb:From> ";
//    xml_rq += " <eb:PartyId type='urn:x12.org:IO5:01'>WebServiceClient</eb:PartyId>";
//    xml_rq += "</eb:From> ";
//    xml_rq += "<eb:To>";
//    xml_rq += " <eb:PartyId type='urn:x12.org:IO5:01'>WebServiceSupplier</eb:PartyId> ";
//    xml_rq += "</eb:To><eb:CPAId>" + ipcc + "</eb:CPAId> ";
//    xml_rq += "<eb:Service eb:type='OTA'>SessionCreateRQ</eb:Service>";
//    xml_rq += "<eb:Action>SessionCreateRQ</eb:Action><eb:MessageData>";
//    xml_rq += "  <eb:MessageId>mid:20001209-133003-2333@clientofsabre.com1</eb:MessageId> ";
//    xml_rq += " <eb:Timestamp>" + DateTime.UtcNow.ToString("s") + "Z</eb:Timestamp>";
//    xml_rq += "<eb:TimeToLive>" + DateTime.UtcNow.ToString("s") + "Z</eb:TimeToLive> ";
//    xml_rq += "</eb:MessageData>";
//    xml_rq += "</eb:MessageHeader>";
//    xml_rq += "<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext' xmlns:wsu='http://schemas.xmlsoap.org/ws/2002/12/utility'>  ";
//    xml_rq += "  <wsse:UsernameToken>       ";
//    xml_rq += "<wsse:Username>" + username + "</wsse:Username>  ";
//    xml_rq += "<wsse:Password>" + password + "</wsse:Password> ";
//    xml_rq += "<Organization>" + ipcc + "</Organization>   ";
//    xml_rq += "<Domain>DEFAULT</Domain>  </wsse:UsernameToken>";
//    xml_rq += "</wsse:Security>";
//    xml_rq += " </SOAP-ENV:Header> ";
//    xml_rq += "<SOAP-ENV:Body>  ";
//    xml_rq += "<eb:Manifest SOAP-ENV:mustUnderstand='1' eb:version='1.0'> ";
//    xml_rq += "  <eb:Reference xmlns:xlink='http://www.w3.org/1999/xlink' xlink:href='cid:rootelement' xlink:type='simple'/>";
//    xml_rq += "</eb:Manifest>";
//    xml_rq += "</SOAP-ENV:Body>";
//    xml_rq += "</SOAP-ENV:Envelope>";
//    result = XMLRead.SendQuery(xml_rq);
//    SaveXMLFile(xml_rq, result, "sessioncreatePD_" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
//    return result;
//}
//public static string closession(string mid, string timestamp, string token, string sid)
//{
//    string xmlrq = "";
//    xmlrq = " <SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/'  xmlns:eb='http://www.ebxml.org/namespaces/messageHeader'      xmlns:xlink='http://www.w3.org/1999/xlink'  xmlns:xsd='http://www.w3.org/1999/XMLSchema'>";
//    xmlrq += "<SOAP-ENV:Header>";
//    xmlrq += " <eb:MessageHeader SOAP-ENV:mustUnderstand='1' eb:version='2.0'>";
//    xmlrq += "<eb:ConversationId>Session</eb:ConversationId>";
//    xmlrq += "<eb:From>";
//    xmlrq += "<eb:PartyId type='urn:x12.org:IO5:01'>WebServiceClient</eb:PartyId>";
//    xmlrq += "  </eb:From>";
//    xmlrq += "<eb:To>";
//    xmlrq += "<eb:PartyId type='urn:x12.org:IO5:01'>WebServiceSupplier</eb:PartyId>";
//    xmlrq += "</eb:To>";
//    xmlrq += "<eb:CPAId>" + ipcc + "</eb:CPAId>";
//    xmlrq += "<eb:Service eb:type='sabreXML'>Session</eb:Service> ";
//    xmlrq += "<eb:Action>SessionCloseRQ</eb:Action>";
//    xmlrq += "<eb:MessageData>";
//    xmlrq += "<eb:MessageId>" + mid + "</eb:MessageId>";
//    xmlrq += "<eb:Timestamp>" + timestamp + "Z</eb:Timestamp>";
//    xmlrq += "</eb:MessageData>";
//    xmlrq += "</eb:MessageHeader>";
//    xmlrq += "<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext'>";
//    xmlrq += "<wsse:BinarySecurityToken valueType='String' EncodingType='wsse:Base64Binary'>" + token + "</wsse:BinarySecurityToken>";
//    xmlrq += "</wsse:Security>";
//    xmlrq += "</SOAP-ENV:Header> ";
//    xmlrq += "<SOAP-ENV:Body>";
//    xmlrq += "<eb:Manifest SOAP-ENV:mustUnderstand='1' eb:version='2.0'>";
//    xmlrq += "<eb:Reference xmlns:xlink='http://www.w3.org/1999/xlink' xlink:type='simple'/>";
//    xmlrq += "<SessionCloseRQ>";
//    xmlrq += "<POS>";
//    xmlrq += "<Source PseudoCityCode='" + ipcc + "'/>";
//    xmlrq += "</POS>";
//    xmlrq += "</SessionCloseRQ>";
//    xmlrq += "</eb:Manifest>";
//    xmlrq += "</SOAP-ENV:Body>";
//    xmlrq += "</SOAP-ENV:Envelope>";

//    string xmlrs = SendQuery(xmlrq);
//    SaveXMLFile(xmlrq, xmlrs, "sessionclose" + sid);
//    return xmlrs;

//}
//public static string SendQuery(String xml_rq)
//{
//    string strresultxml = "";
//    try
//    {
//       HttpWebRequest httprequest = (HttpWebRequest)HttpWebRequest.Create("https://sws-crt.cert.havail.sabre.com");//test


//        byte[] byteArray = Encoding.UTF8.GetBytes(xml_rq);

//        httprequest.Method = "POST";

//        httprequest.ContentType = "text/xml; charset=utf-8";

//        httprequest.ContentLength = byteArray.Length;

//        httprequest.ProtocolVersion = HttpVersion.Version11;

//        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

//        Stream httprq_Stream = httprequest.GetRequestStream();

//        httprq_Stream.Write(byteArray, 0, byteArray.Length);

//        HttpWebResponse httpresponse = (HttpWebResponse)httprequest.GetResponse();

//        Stream httprs_stream = httpresponse.GetResponseStream();

//        StreamReader httpsr = new StreamReader(httprs_stream);

//        strresultxml = httpsr.ReadToEnd();

//        httpsr.Close();
//        httprs_stream.Close();

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
//public static void SaveXMLFile(string RQXML, string RSXML, string FileName)
//{
//    try
//    {
//        XmlDocument RQdoc = new XmlDocument();
//        RQdoc.LoadXml(RQXML);
//        RQdoc.PreserveWhitespace = true;
//        string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + FileName + "-RQ.xml");
//        RQdoc.Save(filePathRQ);


//        XmlDocument RSdoc = new XmlDocument();
//        RSdoc.LoadXml(RSXML);
//        RSdoc.PreserveWhitespace = true;
//        string filePathRS = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + FileName + "-RS.xml");
//        RSdoc.Save(filePathRS);

//    }
//    catch (Exception ex)
//    {

//    }

//}
//public static string ContextChange()
//{
//    // string result = "";
//    result = createsession("hotels");
//    string cc_rq = "";
//    DataSet ds = new DataSet();
//    DataSet dsSession = new DataSet();
//    StringReader se_stream = new StringReader(result);
//    dsSession.ReadXml(se_stream);
//    if (dsSession.Tables["BinarySecurityToken"] != null)
//    {
//        DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
//        DataTable dtMessageData = dsSession.Tables["MessageData"];
//        DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
//        string timestamp = DateTime.UtcNow.ToString();
//        cc_rq = "<SOAP-ENV:Envelope xmlns:SOAP-ENV='http://schemas.xmlsoap.org/soap/envelope/' xmlns:SOAP-ENC='http://schemas.xmlsoap.org/soap/encoding/' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>";
//        cc_rq += "<SOAP-ENV:Header>";
//        cc_rq += "<eb:MessageHeader  xmlns:eb='http://www.ebxml.org/namespaces/messageHeader' SOAP-ENV:mustUnderstand='1' eb:version='2.0'>";
//        cc_rq += "<eb:From>";
//        cc_rq += "<eb:PartyId type='urn:x12.org:IO5:01'>WebServiceClient</eb:PartyId>";
//        cc_rq += " </eb:From>";
//        cc_rq += "<eb:To>";
//        cc_rq += " <eb:PartyId type='urn:x12.org:IO5:01'>WebServiceSupplier</eb:PartyId>";
//        cc_rq += "</eb:To>";
//        cc_rq += " <eb:CPAId>" + pcc + "</eb:CPAId>";
//        cc_rq += " <eb:ConversationId>" + dtMessageHeader.Rows[0]["ConversationId"].ToString() + "</eb:ConversationId>";
//        cc_rq += " <eb:Service eb:type='OTA'>Air Shopping Service</eb:Service>";
//        cc_rq += "<eb:Action>ContextChangeLLSRQ</eb:Action>";
//        cc_rq += "<eb:MessageData>";
//        cc_rq += " <eb:MessageId>" + dtMessageData.Rows[0]["MessageId"].ToString() + "</eb:MessageId>";
//        cc_rq += "<eb:Timestamp>" + dtMessageData.Rows[0]["Timestamp"].ToString() + "</eb:Timestamp>";
//        cc_rq += "</eb:MessageData>";
//        cc_rq += "<eb:RefToMessageId>" + dtMessageData.Rows[0]["ReftoMessageId"].ToString() + "</eb:RefToMessageId> ";
//        cc_rq += "<eb:DuplicateElimination /> ";
//        cc_rq += "</eb:MessageHeader>";
//        cc_rq += "<wsse:Security xmlns:wsse='http://schemas.xmlsoap.org/ws/2002/12/secext' xmlns:wsu='http://schemas.xmlsoap.org/ws/2002/12/utility'>";
//        cc_rq += "<wsse:BinarySecurityToken valueType='String' EncodingType='wsse:Base64Binary'>" + dtBinarySecurityToken.Rows[0]["BinarySecurityToken_Text"].ToString() + "</wsse:BinarySecurityToken>";
//        cc_rq += "</wsse:Security>";
//        cc_rq += "</SOAP-ENV:Header>";
//        cc_rq += "<SOAP-ENV:Body>";
//        cc_rq += "<ContextChangeRQ xmlns='http://webservices.sabre.com/sabreXML/2011/10' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' Version='2.0.3'>";
//        cc_rq += "<ChangeAAA PseudoCityCode='" + pcc + "' />";
//        cc_rq += " </ContextChangeRQ>";
//        cc_rq += "</SOAP-ENV:Body>";
//        cc_rq += "</SOAP-ENV:Envelope>";
//        result = SendQuery(cc_rq);
//        SaveXMLFile(cc_rq, result, "ContextChangePD_" + DateTime.Now.Day + DateTime.Now.Month + DateTime.Now.Year + "_" + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);
//    }
//    return result;
//}