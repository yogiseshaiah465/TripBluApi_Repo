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
using System.Web;

public class PaymentgatewayData
{
    //public HotelEndTrans Hespay;
    public string Ratecommentdescription = "";
    public PaymentgatewayData(string searchid, CustomerInfopaymentgt ci, string rph, string BookinID, string hotelcode, string CurrencyCode, string b2c_idn, string rooms, string Guestdet)
    {
        string ratekey = string.Empty;
        string result = "";
        string cmdtxtrkey = "select HB_RateKey from HotelBooking where Bookingidn=" + BookinID + "";
        DataTable dtrkey = manage_data.GetDataTable(cmdtxtrkey, manage_data.con);
        if (dtrkey.Rows.Count > 0)
        {
            ratekey = dtrkey.Rows[0]["HB_RateKey"].ToString();
        }


        int rphl = rph.ToString().Length;
        if (rphl == 1)
        {
            rph = "0" + rph.ToString();
        }

        //string RateBodyRQ = GetRateDescBodyRQ(rph);
        //string PassengerBodyRQ = GetPassengerBodyRQ(ci, Guestdet);
        string htlpaymentgturi = ConfigurationManager.AppSettings["HotelpaymentgatewayUri"] != null ? ConfigurationManager.AppSettings["HotelpaymentgatewayUri"].ToString() : string.Empty;
        if (!string.IsNullOrEmpty(htlpaymentgturi))
        {
            string HoteResBodyRQ = GetHotelResBodyRQ(searchid, ci, rph, rooms, ratekey, BookinID, htlpaymentgturi);

            string expdate = "";
            string[] expdatepart = ci.CCExpDate.Split('-');
            expdate = expdatepart[1] + "-" + expdatepart[0];

            string dat1 = expdatepart[1];
            string yearend = dat1.Substring(dat1.Length - 2);
            string expdatedd = expdatepart[0] + yearend;
            string rq = string.Empty;
            //string rq = "?xmldata=<txn><ssl_merchant_ID>009005</ssl_merchant_ID><ssl_user_id>devportal</ssl_user_id><ssl_pin>BDDZY5KOUDCNPV4L3821K7PETO4Z7TPYOJB06TYBI1CW771IDHXBVBP51HZ6ZANJ</ssl_pin><ssl_description>Auth for 3.00</ssl_description><ssl_transaction_type>ccsale</ssl_transaction_type><ssl_card_number>" + ci.CCNumber + "</ssl_card_number><ssl_exp_date>" + expdatedd + "</ssl_exp_date><ssl_amount>" + ci.Totelamount + "</ssl_amount><ssl_salestax>" + ci.toteltaxes + "</ssl_salestax><ssl_cvv2cvc2_indicator>1</ssl_cvv2cvc2_indicator><ssl_cvv2cvc2>" + ci.Cvnum + "</ssl_cvv2cvc2><ssl_customer_code>CORP</ssl_customer_code><ssl_invoice_number>" + ci.PNR + "</ssl_invoice_number><ssl_first_name>" + ci.Name + "</ssl_first_name><ssl_last_name>" + ci.SurName + "</ssl_last_name><ssl_avs_address>" + ci.Addressline1 + " Main</ssl_avs_address><ssl_city>" + ci.city + "</ssl_city><ssl_state>" + ci.state + "</ssl_state><ssl_avs_zip>99999</ssl_avs_zip><ssl_country>" + ci.Country + "</ssl_country><ssl_phone>" + ci.Phone + "</ssl_phone><ssl_ship_to_state>" + ci.state + "</ssl_ship_to_state><ssl_ship_to_zip>99999</ssl_ship_to_zip><ssl_ship_to_address1>" + ci.Addressline1 + " Main</ssl_ship_to_address1><ssl_ship_to_company>Ship Company</ssl_ship_to_company><ssl_ship_to_last_name>" + ci.SurName + "</ssl_ship_to_last_name><ssl_ship_to_city>" + ci.city + "</ssl_ship_to_city><ssl_ship_to_first_name>" + ci.Name + "</ssl_ship_to_first_name></txn>";
             rq="?xmldata=<txn><ssl_merchant_ID>009005</ssl_merchant_ID><ssl_user_id>devportal</ssl_user_id><ssl_pin>BDDZY5KOUDCNPV4L3821K7PETO4Z7TPYOJB06TYBI1CW771IDHXBVBP51HZ6ZANJ</ssl_pin><ssl_description>Auth for 3.00</ssl_description><ssl_transaction_type>ccsale</ssl_transaction_type><ssl_card_number>"+ci.CCNumber+"</ssl_card_number><ssl_exp_date>" + expdatedd + "</ssl_exp_date><ssl_amount>"+ci.Totelamount+"</ssl_amount><ssl_salestax>"+ci.toteltaxes+"</ssl_salestax><ssl_cvv2cvc2_indicator>1</ssl_cvv2cvc2_indicator><ssl_cvv2cvc2>"+ci.Cvnum+"</ssl_cvv2cvc2><ssl_customer_code>CORP</ssl_customer_code><ssl_invoice_number>"+ci.PNR+"</ssl_invoice_number><ssl_first_name>"+ci.Name+"</ssl_first_name><ssl_last_name>"+ci.SurName+"</ssl_last_name><ssl_avs_address>"+ci.Addressline1+"</ssl_avs_address><ssl_city>"+ci.city+"</ssl_city><ssl_state>"+ci.state+"</ssl_state><ssl_avs_zip>"+ci.Zipcode+"</ssl_avs_zip><ssl_country>" + ci.Country + "</ssl_country><ssl_phone>" + ci.Phone + "</ssl_phone><ssl_ship_to_state>" + ci.state + "</ssl_ship_to_state><ssl_ship_to_zip>" + ci.Zipcode + "</ssl_ship_to_zip><ssl_ship_to_address1>" + ci.state + "</ssl_ship_to_address1><ssl_ship_to_company>Ship Company</ssl_ship_to_company><ssl_ship_to_last_name>" + ci.SurName + "</ssl_ship_to_last_name><ssl_ship_to_city>"+ci.city+"</ssl_ship_to_city><ssl_ship_to_first_name>"+ci.Name+"</ssl_ship_to_first_name></txn>";


            result = XMLRead.SendQuerypaymetgat(rq, htlpaymentgturi);


            XMLRead.SaveXMLTextFile(rq, result, searchid + "_" + BookinID + "_PgatewayHotelResDet");



            //searchid = "7430";
            //BookinID = "1918";


            string filePathRS = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_" + BookinID + "_PgatewayHotelResDet" + "-RS.xml");
            result = File.ReadAllText(filePathRS);


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

            Ratecommentdescription = filePathRS;


           // Hespay = new HotelEndTrans(EndTransXML, BookinID, filePathRS);

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
    private string GetHotelResBodyRQ(string searchid, CustomerInfopaymentgt ci, string rph, string rooms, string ratekey, string bookingid, string htlpaymentgturi)
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
        string expdatedd = expdatepart[1] + expdatepart[0];
        string rq = "";

        rq += "?xmldata=";
        rq += "<txn>";
        rq += "<ssl_merchant_ID>009005</ssl_merchant_ID>";
        rq += "<ssl_user_id>devportal</ssl_user_id>";
        rq += "<ssl_pin>BDDZY5KOUDCNPV4L3821K7PETO4Z7TPYOJB06TYBI1CW771IDHXBVBP51HZ6ZANJ</ssl_pin>";
        rq += "<ssl_description>Auth for 3.00</ssl_description>";
        rq += "<ssl_transaction_type>ccsale</ssl_transaction_type>";
        rq += "<ssl_card_number>" + ci.CCNumber + "</ssl_card_number>";
        rq += "<ssl_exp_date>" + expdatedd + "</ssl_exp_date>";
        rq += "<ssl_amount>" + ci.Totelamount + "</ssl_amount>";
        rq += "<ssl_salestax>" + ci.toteltaxes + "</ssl_salestax>";
        rq += "<ssl_cvv2cvc2_indicator>1</ssl_cvv2cvc2_indicator>";
        rq += "<ssl_cvv2cvc2>" + ci.Cvnum + "</ssl_cvv2cvc2>";
        rq += "<ssl_customer_code>CORP</ssl_customer_code>";
        rq += "<ssl_invoice_number>" + ci.PNR + "</ssl_invoice_number>";
        rq += "<ssl_first_name>" + ci.Name + "</ssl_first_name>";
        rq += "<ssl_last_name>" + ci.SurName + "</ssl_last_name>";
        rq += " <ssl_avs_address>" + ci.Addressline1 + " Main</ssl_avs_address>";
        rq += "<ssl_city>" + ci.city + "</ssl_city>";
        rq += "<ssl_state>" + ci.state + "</ssl_state>";
        rq += "<ssl_avs_zip>99999</ssl_avs_zip>";
        rq += " <ssl_country>" + ci.Country + "</ssl_country>";
        rq += "<ssl_phone>" + ci.Phone + "</ssl_phone>";
        rq += "<ssl_ship_to_state>" + ci.state + "</ssl_ship_to_state>";
        rq += "<ssl_ship_to_zip>99999</ssl_ship_to_zip>";
        rq += "<ssl_ship_to_address1>" + ci.Addressline1 + " Main</ssl_ship_to_address1>";
        rq += "<ssl_ship_to_company>Ship Company</ssl_ship_to_company>";
        rq += "<ssl_ship_to_last_name>" + ci.SurName + "</ssl_ship_to_last_name>";
        rq += "<ssl_ship_to_city>" + ci.city + "</ssl_ship_to_city>";
        rq += "<ssl_ship_to_first_name>" + ci.Name + "</ssl_ship_to_first_name>";
        rq += " </txn>";




        return rq;
    }
}