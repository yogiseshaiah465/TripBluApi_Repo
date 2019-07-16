using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

/// <summary>
/// Summary description for HotelDBLayer
/// </summary>
public class HotelDBLayer
{
		public static string con = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
        public static string con2 = ConfigurationManager.ConnectionStrings["SqlConn2"].ToString();

       // public static string triblue_ht = ConfigurationManager.ConnectionStrings["tripbluhtlscon"].ToString();

        public static string SaveSearch(string city, string checkin, string checkout, string guests, string hostaddress)
        {
            string i = "";
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            string ipaddress = "";
            try
            {
                ipaddress = hostaddress;
            }
            catch { }
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelSearch_Insert";
                cmd.Parameters.AddWithValue("@p_Destination", city);
                cmd.Parameters.AddWithValue("@p_CheckInDt", checkin);
                cmd.Parameters.AddWithValue("@p_CheckOutDt", checkout);
                cmd.Parameters.AddWithValue("@p_Adults", guests);
                cmd.Parameters.AddWithValue("@p_IP", ipaddress);
                sqlcon.Open();
               
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                i = dt.Rows[0][0].ToString();
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                i ="";
            }
            return i;
        }
        public static string SaveSearch(string city, string checkin, string checkout, int rooms, int adults,int children, string hostaddress)
        {
            string i = "";
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            string ipaddress = "";
            try
            {
                ipaddress = hostaddress;
            }
            catch { }
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelSearch_Insert";
                cmd.Parameters.AddWithValue("@p_Destination", city);
                cmd.Parameters.AddWithValue("@p_CheckInDt", checkin);
                cmd.Parameters.AddWithValue("@p_CheckOutDt", checkout);
                cmd.Parameters.AddWithValue("@p_Rooms", rooms);
                cmd.Parameters.AddWithValue("@p_Adults", adults);
                cmd.Parameters.AddWithValue("@p_Children", children);
                cmd.Parameters.AddWithValue("@p_IP", ipaddress);
                sqlcon.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                i = dt.Rows[0][0].ToString();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                i = "";
            }
            return i;
        }
        public static string SaveSearch(string city, string checkin, string checkout, int rooms, int adults, string adultbyroom, int children, string childrenbyroom, string childages, string hostaddress, string Currency, string b2c_idn, string cust_idn)
        {
            string i = "";
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            string ipaddress = "";
            try
            {
                ipaddress = hostaddress;
            }
            catch { }
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelSearch_Insert";
                cmd.Parameters.AddWithValue("@p_Destination", city);
                cmd.Parameters.AddWithValue("@p_CheckInDt", checkin);
                cmd.Parameters.AddWithValue("@p_CheckOutDt", checkout);
                cmd.Parameters.AddWithValue("@p_Rooms", rooms);
                cmd.Parameters.AddWithValue("@p_Adults", adults);
                cmd.Parameters.AddWithValue("@HB_Adultsbyroom", adultbyroom);
                cmd.Parameters.AddWithValue("@p_Children", children);
                cmd.Parameters.AddWithValue("@HB_Childrenbyroom", childrenbyroom);
                cmd.Parameters.AddWithValue("@HB_ChildAge", childages);
                cmd.Parameters.AddWithValue("@p_GDS", "HB");
                cmd.Parameters.AddWithValue("@p_IP", ipaddress);
                cmd.Parameters.AddWithValue("@P_CurrencyCode", Currency);
                cmd.Parameters.AddWithValue("@p_b2c_idn", b2c_idn);
              
                ////below lines modified on 08 may 2018
                if (cust_idn != "")
                {
                    cmd.Parameters.AddWithValue("@cust_idn", cust_idn);
                }




                sqlcon.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                i = dt.Rows[0][0].ToString();
                sqlcon.Close();
            }
            catch (Exception ex)
            {
                i = "";
            }
            return i;
        }
        public static string SaveHotelSelected(string SearchIdn, string HotelCode,string HotelName, string HotelAddress,string chaincode)
        {
            string i = "";
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelViews_Insert";
                cmd.Parameters.AddWithValue("@p_SearchIdn", SearchIdn);
                cmd.Parameters.AddWithValue("@p_PropertyName", HotelName);
                cmd.Parameters.AddWithValue("@p_GDSPropertyId", HotelCode);
                cmd.Parameters.AddWithValue("@p_ChainCode", chaincode);
                cmd.Parameters.AddWithValue("@p_Location", HotelAddress);

                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                i = dt.Rows[0][0].ToString();

            }
            catch (Exception ex)
            {
                i = "";
            }
            return i;
        }
        public static string SaveBooking(string SearchID,string ViewID, string itinid, string firstname, string lastname, string email,string phone)
        {
            string i = "";
            SqlConnection sqlcon = new SqlConnection(con2);//con);
      
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBooking_Ins";
                cmd.Parameters.AddWithValue("@Viewidn", ViewID);
                cmd.Parameters.AddWithValue("@SearchIdn", SearchID);
                cmd.Parameters.AddWithValue("@ItinSelId", itinid);
                cmd.Parameters.AddWithValue("@FirstName", firstname);
                cmd.Parameters.AddWithValue("@LastName", lastname);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);

                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                //i = cmd.ExecuteNonQuery();
                try {
                    i = dt.Rows[0][0].ToString();
                }
                    catch {
                    }
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                i = "";
            }
            return i;
        }
        public static string SaveBookingProperty(string BookingIdn,string PropertyName,string PropertyCode,string PropertyCityCode,string ChainCode,string ChainName,string GDSId,string Checkime,string CheckOutTime,string AddressLine1,string AddressLine2,string Phone,string Fax,string City,string State,string CityCode,string CountryCode,string PostalCode,string Latitude,string Longitude,string Distance,string DistanceUnit,string StarRating,string ReviewRating)
        {
            string i = "";
            SqlConnection sqlcon = new SqlConnection(con2);//;con);

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBookingProperty_Ins";
                cmd.Parameters.AddWithValue("@BookingIdn", BookingIdn);
                cmd.Parameters.AddWithValue("@PropertyName", PropertyName);
                cmd.Parameters.AddWithValue("@PropertyCode", PropertyCode);
                cmd.Parameters.AddWithValue("@PropertyCityCode", PropertyCityCode);
                cmd.Parameters.AddWithValue("@ChainCode", ChainCode);
                //cmd.Parameters.AddWithValue("@ChainName", ChainName);

                cmd.Parameters.AddWithValue("@GDSId", "SB");
                cmd.Parameters.AddWithValue("@CheckInTime", Checkime);
                cmd.Parameters.AddWithValue("@CheckOutTime", CheckOutTime);

                //cmd.Parameters.AddWithValue("@AddressLine", AddressLine1);
                //cmd.Parameters.AddWithValue("@AddressLine", AddressLine2);

                cmd.Parameters.AddWithValue("@Phone", Phone);
                cmd.Parameters.AddWithValue("@Fax", Fax);

                //cmd.Parameters.AddWithValue("@City", City);
                //cmd.Parameters.AddWithValue("@State", State);
                //cmd.Parameters.AddWithValue("@CityCode", CityCode);
                cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
                ////cmd.Parameters.AddWithValue("@PostalCode", PostalCode);
                cmd.Parameters.AddWithValue("@Latitude", Latitude);
                cmd.Parameters.AddWithValue("@Longitude", Longitude);
                //cmd.Parameters.AddWithValue("@Distance", Distance);
                //cmd.Parameters.AddWithValue("@DistanceUnit", DistanceUnit);
                //cmd.Parameters.AddWithValue("@StarRating", StarRating);
                //cmd.Parameters.AddWithValue("@ReviewRating", ReviewRating);
                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                try
                {
                    i = dt.Rows[0][0].ToString();
                }
                catch
                {
                }
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                i = "";
            }
            return i;
        }
        public static string SaveBookingRate(string BookingIdn, string BookingStatus, string BookingConfirmation, string NumAdults, string NumChildren, string Amount, string TotalAmount, string TotalBaseAmount, string TotalTaxAmount, string TotalSurgeAmount, string MarkupAmount, string DiscountAmount, string NumExtraAdults, string NumExtraChildren, string NumExtraCribs, string NumExtraPersonAllowed, string ExtraPersonAmount, string ExtraCribAmount, string ChildRollawayAmount, string AdultRollAwayAmount, string CurrencyCode, string IsSpecialOffer, string IsRateConversion, string IsRateChanges, string RateLevelCode, string ReturnOfRate, string RateCategory, string RateAccessCode, string LowInvThreshold, string ProductIdentif, string Identif, string GTSurgeRequired, string GTRateProgram, string DirectConnect, string AdvResPeriod,string ClientID, string XPM_GTRequired, string RoomLocCode, string RoomTypeCode, string IsRateConverted, string IsPackage, string HRDForSell,string CPVal, string CPText, string CommissionAvl, string RateCode, string RuleIdn,string roomdesc,string rph)
        {
            string i = "";
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {

                if (TotalAmount == "")  {TotalAmount = "0";}
                if (Amount == "") {  Amount = "0";}
                if (TotalBaseAmount == "") {TotalBaseAmount = "0";}
                if (TotalTaxAmount == "") { TotalTaxAmount = "0"; }
                if (TotalSurgeAmount == "") { TotalSurgeAmount = "0"; }
                if (NumAdults == "") { NumAdults = "0"; }
                if (NumChildren == "") { NumChildren = "0"; }

                if (NumExtraAdults == "") { NumExtraAdults = "0"; }
                if (NumExtraChildren == "") { NumExtraChildren = "0"; }
                if (NumExtraCribs == "") { NumExtraCribs = "0"; }
                if (NumExtraPersonAllowed == "") { NumExtraPersonAllowed = "0"; }
                if (ExtraPersonAmount == "") { ExtraPersonAmount = "0"; }
                if (ExtraCribAmount == "") { ExtraCribAmount = "0"; }
                if (ChildRollawayAmount == "") { ChildRollawayAmount = "0"; }
                if (AdultRollAwayAmount == "") { AdultRollAwayAmount = "0"; }

                if (AdvResPeriod == "") { AdvResPeriod = "0"; }
                if (RateCode == "") { RateCode = "0"; }

         
                Amount = Amount.Trim('$');

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBookingRate_Ins";
                cmd.Parameters.AddWithValue("@BookingIdn", BookingIdn);
                cmd.Parameters.AddWithValue("@NumAdults", NumAdults);
                cmd.Parameters.AddWithValue("@NumChildren", NumChildren);
                cmd.Parameters.AddWithValue("@TotalAmount", TotalAmount);
                cmd.Parameters.AddWithValue("@Amount", Amount);
               // cmd.Parameters.AddWithValue("@TotalBaseAmount", TotalBaseAmount);
                cmd.Parameters.AddWithValue("@TotalTaxAmount", TotalTaxAmount);
                cmd.Parameters.AddWithValue("@RoomDesc", roomdesc);
                cmd.Parameters.AddWithValue("@RPH", rph);
                cmd.Parameters.AddWithValue("@TotalSurchargeAmount", TotalSurgeAmount);
                cmd.Parameters.AddWithValue("@NumExtraAdults", NumExtraAdults);
                cmd.Parameters.AddWithValue("@NumExtraChildren", NumExtraChildren);
                cmd.Parameters.AddWithValue("@NumExtraCribs", NumExtraCribs);
                cmd.Parameters.AddWithValue("@NumExtraPersonAllowed", NumExtraPersonAllowed);
                cmd.Parameters.AddWithValue("@ExtraPersonAmount", ExtraPersonAmount);
                cmd.Parameters.AddWithValue("@ExtraCribAmount", ExtraCribAmount);
                cmd.Parameters.AddWithValue("@ChildRollawayAmount", ChildRollawayAmount);
                cmd.Parameters.AddWithValue("@AdultRollAwayAmount", AdultRollAwayAmount);
                cmd.Parameters.AddWithValue("@CurrencyCode", CurrencyCode);
                cmd.Parameters.AddWithValue("@IsSpecialOffer", IsSpecialOffer);
                cmd.Parameters.AddWithValue("@IsRateConversion", IsRateConversion);
                cmd.Parameters.AddWithValue("@IsRateChanges", IsRateChanges);
                cmd.Parameters.AddWithValue("@RateLevelCode", RateLevelCode);
                cmd.Parameters.AddWithValue("@ReturnOfRate", ReturnOfRate);
                cmd.Parameters.AddWithValue("@RateCategory", RateCategory);
                cmd.Parameters.AddWithValue("@RateAccessCode", RateAccessCode);
                cmd.Parameters.AddWithValue("@LowInvThreshold", LowInvThreshold);
                cmd.Parameters.AddWithValue("@ProductIdentif", ProductIdentif);
                cmd.Parameters.AddWithValue("@CharIdentif", Identif);

                cmd.Parameters.AddWithValue("@GTSurchargeRequired", GTSurgeRequired);
                cmd.Parameters.AddWithValue("@GTRateProgram", GTRateProgram);
                cmd.Parameters.AddWithValue("@DirectConnect", DirectConnect);
                cmd.Parameters.AddWithValue("@AdvResPeriod", AdvResPeriod);
                cmd.Parameters.AddWithValue("@ClientID", ClientID);
                cmd.Parameters.AddWithValue("@XPM_GTRequired", XPM_GTRequired);
                cmd.Parameters.AddWithValue("@RoomLocCode", RoomLocCode);
                cmd.Parameters.AddWithValue("@RoomTypeCode", RoomTypeCode);
                cmd.Parameters.AddWithValue("@IsRateConverted", IsRateConverted);
                cmd.Parameters.AddWithValue("@IsPackage", IsPackage);
                cmd.Parameters.AddWithValue("@HRDForSell", HRDForSell);
                cmd.Parameters.AddWithValue("@CPVal1", CPVal);
                cmd.Parameters.AddWithValue("@CPVal2", CPVal);
                cmd.Parameters.AddWithValue("@CPText", CPText);
                cmd.Parameters.AddWithValue("@CommissionAvl", CommissionAvl);
                cmd.Parameters.AddWithValue("@RateCode", RateCode);
                //cmd.Parameters.AddWithValue("@RuleIdn", RuleIdn);


                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                try
                {
                    i = dt.Rows[0][0].ToString();
                }
                catch
                {
                }
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                i = "";
            }
            return i;
        }
        public static string SaveBookingRateRange(string BookingIdn, string RateIdn, string BaseAmount, string TaxAmount, string SurchargeAmount, string ExpireDt, string EffectiveDt)
        {
            string i = "";

            if (BaseAmount.Trim() == "")
            {
                BaseAmount = "0.0";
            }

            if (TaxAmount.Trim() == "")
            {
                TaxAmount = "0.0";
            }

            if (SurchargeAmount.Trim() == "")
            {
                SurchargeAmount = "0.0";
            }

            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBookingRateRange_Ins";
                cmd.Parameters.AddWithValue("@BookingIdn", BookingIdn);
                cmd.Parameters.AddWithValue("@RateIdn", RateIdn);
                cmd.Parameters.AddWithValue("@BaseAmount", BaseAmount);
                cmd.Parameters.AddWithValue("@TaxAmount", TaxAmount);
                cmd.Parameters.AddWithValue("@SurchargeAmount", SurchargeAmount);
                cmd.Parameters.AddWithValue("@ExpireDt", ExpireDt);
               cmd.Parameters.AddWithValue("@EffectiveDt", EffectiveDt);
                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                try
                {
                    i = dt.Rows[0][0].ToString();
                }
                catch
                {
                }
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                i = "";
            }
            return i;
        }
        public static string SaveBookingPax(string BookingId, string Title, string firstname, string MiddleName, string lastname, string email, string phone)
        {
            string i = "";
            SqlConnection sqlcon = new SqlConnection(con2);//con);

            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBookingPax_Ins";
                cmd.Parameters.AddWithValue("@Bookingidn", BookingId);
                cmd.Parameters.AddWithValue("@Title", Title);
                cmd.Parameters.AddWithValue("@FirstName", firstname);
                cmd.Parameters.AddWithValue("@MiddleName", MiddleName);
                cmd.Parameters.AddWithValue("@LastName", lastname);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Phone", phone);
                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                try
                {
                    i = dt.Rows[0][0].ToString();
                }
                catch
                {
                }
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                i = "";
            }
            return i;
        }
        public static string SaveBookingPayment(string Bookingidn, string Title, string FirstName, string MiddleName, string LastName, string Email, string Phone, string AltPhone, string PhoneExtn, string AddressLine1, string AddressLine2, string CityName, string CityCode, string StateName, string StateCode, string CountryName, string CountryCode, string Zip, string CardType, string CardCode, string CardNum, string CardExpirDt, string CardCVV)
        {
            string i = "";
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBookingPayment_Ins";

                cmd.Parameters.AddWithValue("@Bookingidn", Bookingidn);
                cmd.Parameters.AddWithValue("@Title", Title);
                cmd.Parameters.AddWithValue("@FirstName", FirstName);
                cmd.Parameters.AddWithValue("@MiddleName", MiddleName);
                cmd.Parameters.AddWithValue("@LastName", LastName);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Phone", Phone);
                cmd.Parameters.AddWithValue("@AltPhone", AltPhone);
                cmd.Parameters.AddWithValue("@PhoneExtn", PhoneExtn);
                cmd.Parameters.AddWithValue("@AddressLine1", AddressLine1);
                cmd.Parameters.AddWithValue("@AddressLine2", AddressLine2);
                cmd.Parameters.AddWithValue("@CityName", CityName);
                cmd.Parameters.AddWithValue("@CityCode", CityCode);
                cmd.Parameters.AddWithValue("@StateName", StateName);
                cmd.Parameters.AddWithValue("@StateCode", StateCode);
                cmd.Parameters.AddWithValue("@CountryName", CountryName);
                cmd.Parameters.AddWithValue("@CountryCode", CountryCode);
                cmd.Parameters.AddWithValue("@Zip", Zip);
                cmd.Parameters.AddWithValue("@CardType", CardType);
                //cmd.Parameters.AddWithValue("@CardCode", CardCode);
                cmd.Parameters.AddWithValue("@CardNum", CardNum);
                cmd.Parameters.AddWithValue("@CardExpirDt", CardExpirDt);
                cmd.Parameters.AddWithValue("@CardCVV", CardCVV);

                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                try
                {
                    i = dt.Rows[0][0].ToString();
                }
                catch
                {
                }
                sqlcon.Close();

            }
            catch (Exception ex)
            {
                i = "";
            }
            return i;
        }
        public static DataTable GetSearch()
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelSearch_Sel";
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable GetSearch(string SearchId)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelSearch_Sel";
                cmd.Parameters.AddWithValue("@SearchIdn", SearchId);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable GetSearchFromBookingID(string BookingID)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelSearch_Sel_Booking";
                cmd.Parameters.AddWithValue("@BookingIdn", BookingID);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable GetFaicons()
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_CodeFavicons_sel";
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable GetSelectedHotel(string viewid)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "[p_HotelViews_Sel]";
                cmd.Parameters.AddWithValue("@viewidn", viewid);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable GetBookingDetails(string BookingID)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBookingDetails";
                cmd.Parameters.AddWithValue("@BookingIdn", BookingID);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable GetBookingDetails_BOF()
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBooking_Sel_BOF";
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable GetBookingDetails_BOF(int bid)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBKSrh_Sel_BOF";
                cmd.Parameters.AddWithValue("@BookingIdn", bid);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable GetPropertyDet(int bid)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBookingProperty_Sel";
                cmd.Parameters.AddWithValue("@BookingIdn", bid);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable GetPropertyDet_BOF(int bid)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con2);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBookingProperty_Sel";
                cmd.Parameters.AddWithValue("@BookingIdn", bid);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable GetPaymentDet_BOF(int bid)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con2);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBookingPayment_Sel";
                cmd.Parameters.AddWithValue("@BookingIdn", bid);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable GetPaxDet_BOF(int bid)
        {
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con2);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBookingPax_Sel";
                cmd.Parameters.AddWithValue("@BookingIdn", bid);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
        public static DataTable UpdateItinerary(string bid,string ItineraryRefId)
        {
            string rvalue = "";
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBooking_UpdPnr";
                cmd.Parameters.AddWithValue("@BookingIdn", bid);
                cmd.Parameters.AddWithValue("@PNR", ItineraryRefId);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            return dt;
        }


        public static DataTable GetRateidn(int bid)
        {
            string address = string.Empty;
            DataTable dt = new DataTable();
            string i = "";
            // SqlConnection sqlcon = new SqlConnection(con);
            try
            {
                string strQry = "select RateIdn,b2c_idn from HotelBookingRate  where BookingIdn =" + bid;
                
                dt = manage_data.GetDataTable(strQry, con2);
               
            }
            catch (Exception ex)
            {
               
            }


            return dt;
        }

        public static DataTable UpdateHBbookingRef(string bid, string ItineraryRefId, string status, string vatnumber, string Reservationum, string cancellationfrom, string cancellationamount, string Ratecomments, string supplier_name)//,string supliername)
        {
            string rvalue = "";
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBooking_Upd";
                cmd.Parameters.AddWithValue("@bookingidn", bid);
                cmd.Parameters.AddWithValue("@HB_Reference", ItineraryRefId);
                cmd.Parameters.AddWithValue("@HB_VatNumber", vatnumber);
                cmd.Parameters.AddWithValue("@BookingStatus ", status);
                cmd.Parameters.AddWithValue("@HB_RegNumber", Reservationum);
                cmd.Parameters.AddWithValue("@HB_SupplierName", supplier_name);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch (Exception ex)
            {
            }


            string rateid = string.Empty;
            DataTable dtrateidn = GetRateidn(Convert.ToInt32(bid));
            string b2c_idn = string.Empty;

            if (dtrateidn.Rows.Count > 0)
            {
                 rateid = dtrateidn.Rows[0]["RateIdn"].ToString();
                 b2c_idn = dtrateidn.Rows[0]["b2c_idn"].ToString();
            }


            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelBookingRate_Upd";
                cmd.Parameters.AddWithValue("@RateIdn ",Convert.ToInt32(rateid));
                cmd.Parameters.AddWithValue("@BookingIdn ", bid);
                cmd.Parameters.AddWithValue("@b2c_idn ", b2c_idn);
                cmd.Parameters.AddWithValue("@HB_CancellationAmt ", cancellationamount);
                cmd.Parameters.AddWithValue("@HB_CancellationFromDt ", cancellationfrom);
                cmd.Parameters.AddWithValue("@HB_RateComments  ", Ratecomments);
                

                // cmd.Parameters.AddWithValue("@HB_SupplierName", supliername);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch (Exception ex)
            {
            }


            return dt;
        }


        public static DataTable GetHotelImageUrl(string HotelCode)
        {
            string rvalue = "";
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelImageUrl_sel";
                cmd.Parameters.AddWithValue("@HotelCode", HotelCode);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            return dt;
        }
        public static DataTable SaveHotelImageUrl(string HotelCode,string ImageUrl, string LogoUrl)
        {
            string rvalue = "";
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelImageUrl_ui";
                cmd.Parameters.AddWithValue("@HotelCode", HotelCode);
                cmd.Parameters.AddWithValue("@ImageUrl", ImageUrl);
                cmd.Parameters.AddWithValue("@Logourl ", LogoUrl);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch (Exception ex)
            {
            }
            return dt;


        }
        public static string SaveContactUs(string ipaddress, string Name, string Email, string Phone, string Company, string Message)
        {
            string rv = "";
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_contact_us_ins";
                cmd.Parameters.AddWithValue("@IP", ipaddress);
                cmd.Parameters.AddWithValue("@first_name ", Name);
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Phone", Phone);
                cmd.Parameters.AddWithValue("@company", Company);
                cmd.Parameters.AddWithValue("@Note", Message);

                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                sqlcon.Close();
                rv = "Y";
            }
            catch (Exception ex)
            {
                rv = "N";
            }
            return rv;
        }
        public static string SaveSignUp(string ipaddress, string FirstName, string LastName, string Email, string Phone, string Password, string b2c_idn, string b2b_idn)
        {
            string rv = "";
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_customer_ins";
                cmd.Parameters.AddWithValue("@IP", ipaddress);
                cmd.Parameters.AddWithValue("@first_name", FirstName);
                cmd.Parameters.AddWithValue("@last_name", LastName);
                cmd.Parameters.AddWithValue("@email ", Email);
                cmd.Parameters.AddWithValue("@Phone", Phone);
                cmd.Parameters.AddWithValue("@password1", Password);
                cmd.Parameters.AddWithValue("@b2c_idn", b2c_idn);
                cmd.Parameters.AddWithValue("@b2b_idn", b2b_idn);
                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                sqlcon.Close();
                rv = "Y";
            }
            catch (Exception ex)
            {
                rv = "N";
            }
            return rv;
        }
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                //byte[] encData_byte = new byte[password.Length];
                //encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                //string encodedData = Convert.ToBase64String(encData_byte);
                //return encodedData;
               string hashMethod = "";
               string hashedPassword = "";
                hashMethod = "MD5";
                hashedPassword=FormsAuthentication.HashPasswordForStoringInConfigFile(password, hashMethod);
                return hashedPassword;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        } 
        public static string Logincheck(string Email, string Password,string b2c_idn, string b2b_idn)
        {
            string rv = "";
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_hotel_customer";
                cmd.Parameters.AddWithValue("@Email_id", Email);
                cmd.Parameters.AddWithValue("@pwd", Password);
                //cmd.Parameters.AddWithValue("@p_b2c_idn", b2c_idn);
                //cmd.Parameters.AddWithValue("@p_b2b_idn", b2b_idn);
                sqlcon.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                sqlcon.Close();
                if (dt.Rows.Count > 0)
                {
                    rv = "Y";
                }
                else
                {
                    rv = "N";
                }
            }
            catch (Exception ex)
            {
                rv = "N";
            }
            return rv;
        }
        public static DataTable GetLatLong(string City)
        {

          ////  p_HotelSearchLocation @City = 'Bangalore',@State = 'Karnataka',@Country = 'IN'
            string[] cityparts = City.Split(',');
            int c = cityparts.Length;
            string CityP = cityparts[0];
            //string State = cityparts[1];
            string Country = cityparts[1];
            DataTable dt = new DataTable();
            SqlConnection sqlcon = new SqlConnection(con2);//con);
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "p_HotelSearchLocation";
                cmd.Parameters.AddWithValue("@City", CityP.Trim());
                cmd.Parameters.AddWithValue("@State","" );//State.Trim()
                cmd.Parameters.AddWithValue("@Country", Country.Trim());
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch
            {
            }
            return dt;
        }
       
}
