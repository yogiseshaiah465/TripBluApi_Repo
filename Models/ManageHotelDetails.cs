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

/// <summary>
/// Summary description for ManageHotelDetails
/// </summary>
public class ManageHotelDetails
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
    public ManageHotelDetails()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    //public string GetContextResult()
    //{
    //    string rvalue = "";
    //    pcc = "VL5H";
    //    ipcc = "7A7H"; ;
    //    username = "373541";
    //    password = "WS110542";
    //    //getting the contexchangeresult
    //    string filePathContext = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelAvail/" + searchid + "_Context.xml");
    //    if (File.Exists(filePathContext))
    //    {
    //        ContextResult = File.ReadAllText(filePathContext);
    //    }
    //    else
    //    {
    //        ContextResult = XMLRead.ContextChange(pcc, ipcc, username, password, searchid);
    //    }
    //    return ContextResult;
    //}
    public int SaveRoom(HotelProperty hpr, string srph, string searchid, string viewid, string itinid, string firstname, string lastname, string email, string phone)
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
    //public  HotelProperty  GetData(string SearchID,string contextresult,string hotelcode )
    public HotelProperty GetData(string SearchID,string hotelcode)
    {
        HotelProperty hpr=null;
        string filePathContext = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + SearchID + "_ContextChange-RS.xml");
        if (File.Exists(filePathContext))
        {
            ContextResult = File.ReadAllText(filePathContext);
        }
        else
        {
            ContextResult = XMLRead.ContextChange(SearchID);
        }
        try
        {
            searchid = SearchID;
            objfavicons = new ClsFevicons();
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo = cultureInfo.TextInfo;
           // string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelProperty/" + searchid + "_" + hotelcode + "_PropertyData.xml");
            string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_propertydesc_" + hotelcode + "-RS.xml");
            if (File.Exists(filePathRQ))
            {
                string result = File.ReadAllText(filePathRQ);
                hpr = new HotelProperty(result, hotelcode, searchid);
            }
            else
            {
                DataTable dssearch = HotelDBLayer.GetSearch(searchid);
                HPDCondition Hapc = GetCondition(hotelcode);
                hpr = new HotelProperty(Hapc, ContextResult, searchid);
                if (!File.Exists(filePathRQ))
                {
                    File.WriteAllText(filePathRQ, hpr.PropertyXmlResult);
                }

            }
        }
        catch { }
        return hpr;
    }
    private HPDCondition GetCondition(string hotelcode)
    {
        HPDCondition Hapc = new HPDCondition();
        DataTable dssearch = HotelDBLayer.GetSearch(searchid);
        int guestcount = Convert.ToInt16(dssearch.Rows[0]["Adults"].ToString()) + Convert.ToInt16(dssearch.Rows[0]["Children"].ToString());
        try { Hapc.HotelCode = hotelcode; }
        catch { Hapc.HotelCode = ""; }
        try { Hapc.checkin = Convert.ToDateTime(dssearch.Rows[0]["CheckInDt"]).ToString("MM-dd"); }
        catch { Hapc.checkin = ""; }
        try { Hapc.checkout = Convert.ToDateTime(dssearch.Rows[0]["CheckOutDt"]).ToString("MM-dd"); }
        catch { Hapc.checkout = ""; }
        try { Hapc.guestcount = guestcount.ToString(); }
        catch { Hapc.guestcount = ""; }
        return Hapc;
    }
}