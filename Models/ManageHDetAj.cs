using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Threading;
using System.Globalization;
using System.Configuration;
using System.Net;
using System.IO;
using HotelDevMTWebapi.Models;
using System.Xml;
using System.Xml.Serialization;
    public class ManageHDetAj
    {
        public string BookingId = "0";
        public string Propertyid = "";
        public string RateID = "";
        string RaterangeID = "";
        public static string pcc = "";
        public static string ipcc = "";
        public static string username = "";
        public static string password = "";
        public static string result = "";
        public string ContextResult = "";
        TextInfo TextInfo;

        ClsFevicons objfavicons;
        string imageurl = "";
        string awardimage = "";
        string searchid = "";
        Dictionary<string, string> faicons = new System.Collections.Generic.Dictionary<string, string>();
        DataTable Dtfaicons;
        private void FillFaiconDictionary()
        {
            Dtfaicons = HotelDBLayer.GetFaicons();
            foreach (DataRow dr in Dtfaicons.Rows)
            {
                faicons.Add(dr["FaviconDesc"].ToString(), dr["ImageIcon"].ToString());
            }
        }
        public ManageHDetAj()
	{
		//
		// TODO: Add constructor logic here
		//
	}
        public int SaveRoom(HotelPropertyAj hpr, string srph, string searchid, string viewid, string itinid, string firstname, string lastname, string email, string phone)
        {
            int rvalue = 0;
            #region collectinginfosaving
            try
            {
                DataTable dt = hpr.dtBasicPropInfo;
                DataTable dtpricing = hpr.dtHotelPricing;
                DataTable dtRateRange = hpr.HP_RateRange;
                string rphcond = "";
                rphcond = srph;
                if (srph.Length == 1)
                {
                    rphcond = "00" + srph;
                }
                if (srph.Length == 2)
                {
                    rphcond = "0" + srph;
                }

                DataRow[] roomdescrow = hpr.dtRoomInfo.Select("RPH='" + rphcond + "'");
                DataRow[] roomRaterow = hpr.dtRoomRate.Select("RPH='" + rphcond + "'");
                DataRow[] hotelpricingrow = hpr.dtHotelPricing.Select("RoomRateID='" + roomRaterow[0]["RoomRateID"].ToString() + "'");
                DataRow[] hpraterangerow = hpr.HP_RateRange.Select("HotelPricingID='" + hotelpricingrow[0]["HotelPricingID"].ToString() + "'");
                DataRow[] propaddress = hpr.dtAddressLines.Select("BasicPropInfo_ID='" + dt.Rows[0]["BasicPropInfo_ID"].ToString() + "'");
                DataRow[] raterow = hpr.dtRate.Select("RoomRateID='" + roomRaterow[0]["RoomRateID"].ToString() + "'");
                ////property details starts -----------------
                string PropertyName = dt.Rows[0]["HotelName"].ToString();
                string PropertyCode = dt.Rows[0]["HotelCode"].ToString();
                string PropertyCityCode = dt.Rows[0]["HotelCityCode"].ToString();
                string ChainCode = dt.Rows[0]["ChainCode"].ToString();
                string ChainName = "";
                string GDSId = "SB";
                string CheckinTime = dt.Rows[0]["CheckInTime"].ToString();
                string CheckOutTime = dt.Rows[0]["CheckOutTime"].ToString();
                string AddressLine1 = propaddress[0]["Address"].ToString();
                string AddressLine2 = propaddress[1]["Address"].ToString();
                string Phone = dt.Rows[0]["Phone"].ToString();
                string Fax = dt.Rows[0]["Fax"].ToString();

                string City = "";
                string State = "";

                string CountryCode = dt.Rows[0]["CountryCode"].ToString(); ;
                string PostalCode = "";
                string Latitude = dt.Rows[0]["Latitude"].ToString();
                string Longitude = dt.Rows[0]["Longitude"].ToString();

                string Distance = "";
                string DistanceUnit = "";
                string StarRating = "";
                string ReviewRating = "";

                ////property details ends -----------------

                ////Rate details starts ---------------------

                string BookingStatus = "";
                string BookingConfirmation = "";
                string NumAdults = "";
                string NumChildren = "";
                string Amount = roomdescrow[0]["Rate"].ToString();
                string TotalAmount = hotelpricingrow[0]["HPTotalAmount"].ToString();
                string TotalBaseAmount = "0";

                string TotalTaxAmount = hotelpricingrow[0]["TTaxes_Amount"].ToString();
                string TotalSurgeAmount = hotelpricingrow[0]["TSurcharges_Amount"].ToString();
                string taxpercent = dt.Rows[0]["TaxshortText"].ToString();
                string MarkupAmount = "";
                string DiscountAmount = "";
                string NumExtraAdults = raterow[0]["AddGuestAmt_NumAdults"].ToString();
                string NumExtraChildren = raterow[0]["AddGuestAmt_NumChild"].ToString();
                string NumExtraCribs = raterow[0]["AddGuestAmt_NumCribs"].ToString();
                string NumExtraPersonAllowed = raterow[0]["AddGuestAmt_Max"].ToString();

                string ExtraPersonAmount = raterow[0]["AGA_Charges_ExPer"].ToString();
                string ExtraCribAmount = raterow[0]["AGA_Charges_Crib"].ToString();
                string ChildRollawayAmount = raterow[0]["AGA_Charges_ChildRollAway"].ToString();
                string AdultRollAwayAmount = raterow[0]["AGA_Charges_AdultRollAway"].ToString();


                string CurrencyCode = roomRaterow[0]["CurrencyCode"].ToString();
                string IsSpecialOffer = roomRaterow[0]["SpecialOffer"].ToString();
                string IsRateConversion = roomRaterow[0]["RateConversionInd"].ToString();
                string IsRateChanges = roomRaterow[0]["RateChangeInd"].ToString();
                string RateLevelCode = roomRaterow[0]["RateLevelCode"].ToString();
                string ReturnOfRate = roomRaterow[0]["ReturnOfRateInd"].ToString();
                string RateCategory = roomRaterow[0]["RateCategory"].ToString();
                string RateAccessCode = roomRaterow[0]["RateCategory"].ToString();
                string LowInvThreshold = roomRaterow[0]["LowInvThreshold"].ToString();
                string ProductIdentif = roomRaterow[0]["IATA_ProdIdent"].ToString();
                string Identif = roomRaterow[0]["IATA_ChaIdent"].ToString();
                string GTSurgeRequired = roomRaterow[0]["GuaSurchrgReq"].ToString();
                string GTRateProgram = roomRaterow[0]["DirectConnect"].ToString();
                string DirectConnect = roomRaterow[0]["GuaRateProg"].ToString();
                string AdvResPeriod = roomRaterow[0]["AdvResPeriod"].ToString();
                string ClientID = roomRaterow[0]["ClientID"].ToString();
                string XPM_GTRequired = roomRaterow[0]["XPM_GuaReq"].ToString();
                string RoomLocCode = roomRaterow[0]["RoomLocationCode"].ToString();
                string RoomTypeCode = raterow[0]["RoomTypeCode"].ToString();
                string IsRateConverted = raterow[0]["RateConvIndicator"].ToString();
                string IsPackage = raterow[0]["PkgIndicator"].ToString();
                string HRDForSell = raterow[0]["HRD_ReqforSell"].ToString();
                string CPVal = "";
                string CPText = "";
                string CommissionAvl = "";
                string RateCode = "";
                string RuleIdn = "";
                string roomdesc = "";

                //// rate details ends ------------------------------

                //// for rate range starts  
                string BaseAmount = "0";
                string TaxAmount = "0";
                string SurchargeAmount = "0";
                string ExpireDt = "0";
                string EffectiveDt = "0";
                //// for rate range starts  ends

                try
                {
                    roomdesc = roomdescrow[0]["RoomTypedesc"].ToString();
                }
                catch
                {
                    roomdesc = "";
                }
                try
                {
                    BookingId = HotelDBLayer.SaveBooking(searchid, viewid, "", firstname, lastname, email, phone);
                    Propertyid = HotelDBLayer.SaveBookingProperty(BookingId, PropertyName, PropertyCode, PropertyCityCode, ChainCode, ChainName, GDSId, CheckinTime, CheckOutTime, AddressLine1, AddressLine2, Phone, Fax, City, State, PropertyCityCode, CountryCode, PostalCode, Latitude, Longitude, Distance, DistanceUnit, StarRating, ReviewRating);
                    RateID = HotelDBLayer.SaveBookingRate(BookingId, BookingStatus, BookingConfirmation, NumAdults, NumChildren, Amount, TotalAmount, TotalBaseAmount, TotalTaxAmount, TotalSurgeAmount, MarkupAmount, DiscountAmount, NumExtraAdults, NumExtraChildren, NumExtraCribs, NumExtraPersonAllowed, ExtraPersonAmount, ExtraCribAmount, ChildRollawayAmount, AdultRollAwayAmount, CurrencyCode, IsSpecialOffer, IsRateConversion, IsRateChanges, RateLevelCode, ReturnOfRate, RateCategory, RateAccessCode, LowInvThreshold, ProductIdentif, Identif, GTSurgeRequired, GTRateProgram, DirectConnect, AdvResPeriod, ClientID, XPM_GTRequired, RoomLocCode, RoomTypeCode, IsRateConverted, IsPackage, HRDForSell, CPVal, CPText, CommissionAvl, RateCode, RuleIdn, roomdesc, srph);
                    foreach (DataRow drrr in hpraterangerow)
                    {
                        BaseAmount = drrr["Amount"].ToString();
                        TaxAmount = drrr["Taxes"].ToString(); ;
                        SurchargeAmount = drrr["Surcharges"].ToString(); ;
                        ExpireDt = drrr["ExpireDate"].ToString(); ;
                        EffectiveDt = drrr["EffectiveDate"].ToString();
                        RaterangeID = HotelDBLayer.SaveBookingRateRange(BookingId, RateID, BaseAmount, TaxAmount, SurchargeAmount, ExpireDt, EffectiveDt);
                    }
                    rvalue = 1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion
            return rvalue;
        }
        public AvailabilityRS GetData(string SearchID, string hotelcode, string CurrencyCode, string b2c_idn)
        {
            //HotelPropertyAj hpr = null;

            AvailabilityRS objAvailabilityRS = new AvailabilityRS();
            //string filePathContext = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + SearchID + "_ContextChange-RS.xml");
            //if (File.Exists(filePathContext))
            //{
            //    ContextResult = File.ReadAllText(filePathContext);
            //}
            //else
            //{
            //    ContextResult = XMLRead.ContextChange(searchid);
            //}


            try
            {
                searchid = SearchID;
                objfavicons = new ClsFevicons();
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo = cultureInfo.TextInfo;
                //string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_propertydesc_" + hotelcode + "_" + CurrencyCode + "-RS.xml");
                string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_" + CurrencyCode + "_hotelsAvail-RS.xml");
                XmlDataDocument xmldoc = new XmlDataDocument();
                FileStream fs = new FileStream(filePathRQ, FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                fs.Close();
                XmlNode xnod = xmldoc.DocumentElement;
                XmlSerializer deserializer = new XmlSerializer(typeof(AvailabilityRS));
                StreamReader reader = new StreamReader(filePathRQ);
                objAvailabilityRS = (AvailabilityRS)deserializer.Deserialize(reader);
                
                
                //if (File.Exists(filePathRQ))
                //{
                //    string result = File.ReadAllText(filePathRQ);
                //    objAvailabilityRS = (AvailabilityRS)deserializer.Deserialize(result);
                //}
                //else
                //{
                //    DataTable dssearch = HotelDBLayer.GetSearch(searchid);
                //    HPDCondition Hapc = GetCondition(hotelcode,CurrencyCode);
                //    objAvailabilityRS = new AvailabilityRS(Hapc, ContextResult, searchid, b2c_idn);
                //    if (!File.Exists(filePathRQ))
                //    {
                //        File.WriteAllText(filePathRQ, hpr.PropertyXmlResult);
                //    }

                //}
            }
            catch { }
            return objAvailabilityRS;
        }

        public AvailabilityRS GetDataChangedate(string SearchID, string hotelcode,string rooms,string checkin,string checkout,string adult,string children,string childrenage,string gc, string CurrencyCode, string b2c_idn)
        {
            //HotelPropertyAj hpr = null;

            AvailabilityRS objAvailabilityRS = new AvailabilityRS();
            //string filePathContext = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + SearchID + "_ContextChange-RS.xml");
            //if (File.Exists(filePathContext))
            //{
            //    ContextResult = File.ReadAllText(filePathContext);
            //}
            //else
            //{
            //    ContextResult = XMLRead.ContextChange(searchid);
            //}


            try
            {
                searchid = SearchID;
                objfavicons = new ClsFevicons();
                CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
                TextInfo = cultureInfo.TextInfo;
                //string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_propertydesc_" + hotelcode + "_" + CurrencyCode + "-RS.xml");
                string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_propertydesc_" + hotelcode + "_" + CurrencyCode + "_hotelsAvail-RS.xml");
                XmlDataDocument xmldoc = new XmlDataDocument();
                if (File.Exists(filePathRQ))
                {
                    FileStream fs = new FileStream(filePathRQ, FileMode.Open, FileAccess.Read);
                    xmldoc.Load(fs);
                    fs.Close();
                    XmlNode xnod = xmldoc.DocumentElement;
                    XmlSerializer deserializer = new XmlSerializer(typeof(AvailabilityRS));
                    StreamReader reader = new StreamReader(filePathRQ);
                    objAvailabilityRS = (AvailabilityRS)deserializer.Deserialize(reader);
                }
                else
                {
                    DataTable dssearch = HotelDBLayer.GetSearch(searchid);
                    HPDCondition Hapc = GetCondition(searchid,hotelcode, CurrencyCode);
                    result = gethdata(Hapc, ContextResult, searchid, CurrencyCode, b2c_idn);
                    filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_propertydesc_" + hotelcode + "_" + CurrencyCode + "_hotelsAvail-RS.xml");
                    FileStream fs = new FileStream(filePathRQ, FileMode.Open, FileAccess.Read);
                    xmldoc.Load(fs);
                    fs.Close();
                    XmlNode xnod = xmldoc.DocumentElement;
                    XmlSerializer deserializer = new XmlSerializer(typeof(AvailabilityRS));
                    StreamReader reader = new StreamReader(filePathRQ);
                    objAvailabilityRS = (AvailabilityRS)deserializer.Deserialize(reader);
                    if (!File.Exists(filePathRQ))
                    {
                        File.WriteAllText(filePathRQ, objAvailabilityRS.Xmlns);
                    }
                }

                
            }
            catch { }
            return objAvailabilityRS;
        }

        public HPDCondition GetCondition(string searchid,string hotelcode,string CurrencyCode)
        {
            HPDCondition Hapc = new HPDCondition();
            DataTable dssearch = HotelDBLayer.GetSearch(searchid);
            int guestcount = Convert.ToInt16(dssearch.Rows[0]["Adults"].ToString()) + Convert.ToInt16(dssearch.Rows[0]["Children"].ToString());
            try { Hapc.HotelCode = hotelcode; }
            catch { Hapc.HotelCode = ""; }
            try { Hapc.rooms = Convert.ToInt32(dssearch.Rows[0]["Rooms"]).ToString(); }
            catch { Hapc.checkin = ""; }
            try { Hapc.checkin = Convert.ToDateTime(dssearch.Rows[0]["CheckInDt"]).ToString("yyyy-MM-dd"); }
            catch { Hapc.checkin = ""; }
            try { Hapc.checkout = Convert.ToDateTime(dssearch.Rows[0]["CheckOutDt"]).ToString("yyyy-MM-dd"); }
            catch { Hapc.checkout = ""; }
            try { Hapc.guestcount = guestcount.ToString(); }
            catch { Hapc.guestcount = ""; }
            try { Hapc.Adult = dssearch.Rows[0]["Adults"].ToString(); }
            catch { }
            try { Hapc.children = Convert.ToInt32(dssearch.Rows[0]["Children"].ToString()); }
            catch { }
            try { Hapc.childrenage = dssearch.Rows[0]["HB_ChildAge"].ToString(); }
            catch { }
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
        public string gethdata(HPDCondition Hapc, string ContextResult, string searchid, string curcode, string b2c_idn)
        {
            string result = "";
            try
            {
                string Rqbody = GetHCRQ(Hapc);


                result = GetHotel(Rqbody, searchid, Hapc.HotelCode, curcode, b2c_idn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        private string GetHCRQ(HPDCondition hpc)
        {
            string rqvalue = "";

            try
            {

                rqvalue += "<availabilityRQ xmlns='http://www.hotelbeds.com/schemas/messages' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>";
                rqvalue += "<stay checkIn='" + hpc.checkin + "' checkOut='" + hpc.checkout + "'/>";
                rqvalue += "<occupancies><occupancy rooms='" + hpc.rooms + "' adults='" + hpc.Adult + "' children='" + hpc.children + "'>";
                if (hpc.children > 0)
                {
                    string childages = string.Empty;

                    foreach (string item in hpc.childrenage.Split(','))
                    {
                        rqvalue += "	<paxes>	<pax type='CH' age='" + item + "'/>";
                        rqvalue += "	</paxes>";
                    }
                }

                rqvalue += "</occupancy>";
                rqvalue += "  </occupancies>";

                rqvalue += "<reviews>	<review type='HOTELBEDS' maxRate='5' minRate= '1' minReviewCount='3' /></reviews>";

                rqvalue += " <hotels>";

                //rq += "<hotel>454281</hotel>";



                if (hpc.HotelCode != null && hpc.HotelCode != string.Empty)
                {

                    rqvalue += "<hotel>" + hpc.HotelCode + "</hotel>";



                }


                rqvalue += "</hotels>";

                rqvalue += "</availabilityRQ>";


            }
            catch (Exception ex)
            {

            }



            return rqvalue;
        }
        private string GetHotel(string rqbody, string searchid, string hotelcode, string CurrencyCode, string b2c_idn)
        {
            string rq = "";

            XMLRead.GetPccDetails(b2c_idn);

            string htlAvuri = ConfigurationManager.AppSettings["HotelPortalUri"] != null ? ConfigurationManager.AppSettings["HotelPortalUri"].ToString() : string.Empty;
            if (!string.IsNullOrEmpty(htlAvuri))
            {
                result = XMLRead.SendQuery(rqbody, htlAvuri);
                XMLRead.SaveXMLFile(rqbody, result, searchid + "_propertydesc_" + hotelcode + "_" + CurrencyCode + "_hotelsAvail");


            }

            return result;
        }
    }
