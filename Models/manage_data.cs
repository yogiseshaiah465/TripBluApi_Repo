using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using System.Text;

/// <summary>
/// Summary description for manage_data
/// </summary>
public class manage_data
{
    public static string flip_con = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
    public static string flip_conhb = ConfigurationManager.ConnectionStrings["SqlConnhb"].ToString();
    public static string con = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
    public static string flip_conbof = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
    /*To Retrieve the data from Database*/
    public static DataTable GetDataTable(string cmdtxt, string connstrng)
    {
        DataTable dt = new DataTable();
        try
        {
            SqlConnection sqlcon = new SqlConnection(connstrng);
            SqlDataAdapter da = new SqlDataAdapter(cmdtxt, sqlcon);
            DataSet ds = new DataSet();
            da.Fill(ds, "datatable");
            dt = ds.Tables["datatable"];

        }
        catch (Exception ex)
        {

        }
        return dt;

    }

   
    public static DataTable GetDatable_SP(string SPName, string connstrng)
    {
        int i = 0;
        DataTable dt = new DataTable();
        using (SqlConnection sqlcon = new SqlConnection(connstrng))
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = sqlcon;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SPName;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {

            }
        }
        return dt;
    }
    /*To Insert and Delete the data from database*/
    public static int ManipulateData(string cmdtxt, string connstrng)
    {
        int i = 0;
        try
        {
            SqlConnection sqlcon = new SqlConnection(connstrng);
            sqlcon.Open();
            SqlCommand cmd = new SqlCommand(cmdtxt, sqlcon);
            i = cmd.ExecuteNonQuery();

            sqlcon.Close();
        }
        catch (Exception ex)
        {
            //  send_email.send_emails("dsrini@winsteer.com", ""+ex.ToString()+"", "","exception in query "+cmdtxt+"", "donot-reply@flipfares.com");
        }
        return i;
    }
    public static string SendQuery_BV(string url, string bin)
    {
        string strresultxml = "";
        try
        {

            HttpWebRequest httprequest = (HttpWebRequest)HttpWebRequest.Create(url);//test


            string data = "BinNumber=" + bin + "&LicenseKey=WS45-DSP2-NRF4"; //replace <value>
            byte[] dataStream = Encoding.UTF8.GetBytes(data);

            httprequest.Method = "POST";

            //httprequest.Headers.Add("BinNumber","414472");
            //httprequest.Headers.Add("LicenseKey","WS45-DSP2-NRF4");

            httprequest.ContentType = "application/x-www-form-urlencoded";
            httprequest.Accept = "application/xml";

            httprequest.ContentLength = dataStream.Length;

            httprequest.ProtocolVersion = HttpVersion.Version11;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Stream httprq_Stream = httprequest.GetRequestStream();

            httprq_Stream.Write(dataStream, 0, dataStream.Length);

            HttpWebResponse httpresponse = (HttpWebResponse)httprequest.GetResponse();

            Stream httprs_stream = httpresponse.GetResponseStream();

            StreamReader httpsr = new StreamReader(httprs_stream);

            strresultxml = httpsr.ReadToEnd();



            httpsr.Close();
            httprs_stream.Close();

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
    public static string SendQuery_OV(string url, string BIN, string NumberOfItems, string TransactionTotal, string MCC, string Email, string IPAddress, string TimeOfDay, string BillingCompanyName, string BillingFirstName, string BillingLastName, string BillingAddress, string BillingAddress2, string BillingCity, string BillingState, string BillingPostalCode, string BillingCountry, string BillingPhone, string BillingAddressIsResidential, string LicenseKey)
    {
        //string url = "";
        string rq = "";

        url = "https://trial.serviceobjects.com/OV/api.svc/OrderValidate";

        //rq += " <?xml version='1.0' encoding='utf-8'?>";
        rq += "<OrderValidate xmlns='http://www.serviceobjects.com'>";
        rq += " <BIN>" + BIN + "</BIN>";
        rq += " <NumberOfItems>" + NumberOfItems + "</NumberOfItems>";
        rq += " <TransactionTotal>" + TransactionTotal + "</TransactionTotal>";
        rq += " <MCC></MCC>";
        rq += "  <Email>" + Email + "</Email>";
        rq += "  <IPAddress>" + IPAddress + "</IPAddress>";
        rq += "  <TimeOfDay></TimeOfDay>";
        rq += " <BillingCompanyName></BillingCompanyName>";
        rq += " <BillingFirstName>" + BillingFirstName + "</BillingFirstName>";
        rq += "  <BillingLastName>" + BillingLastName + "</BillingLastName>";
        rq += " <BillingAddress>" + BillingAddress + "</BillingAddress>";
        rq += " <BillingAddress2>" + BillingAddress2 + "</BillingAddress2>";
        rq += " <BillingCity>" + BillingCity + "</BillingCity>";
        rq += " <BillingState>" + BillingState + "</BillingState>";
        rq += "<BillingPostalCode>" + BillingPostalCode + "</BillingPostalCode>";
        rq += "<BillingCountry>" + BillingCountry + "</BillingCountry>";
        rq += " <BillingPhone>" + BillingPhone + "</BillingPhone>";
        rq += "<BillingAddressIsResidential>true</BillingAddressIsResidential>";
        rq += "<TestType>";
        rq += " BusinessToConsumerNoShipping";
        rq += "</TestType>";
        rq += "<LicenseKey>" + LicenseKey + "</LicenseKey>";
        rq += "</OrderValidate>";

        string strresultxml = "";
        try
        {

            HttpWebRequest httprequest = (HttpWebRequest)HttpWebRequest.Create(url);

            string data = rq;
            byte[] dataStream = Encoding.UTF8.GetBytes(data);

            httprequest.Method = "POST";

            httprequest.ContentType = "application/xml";
            httprequest.Accept = "application/xml";

            // httprequest.ContentLength = dataStream.Length;

            httprequest.ProtocolVersion = HttpVersion.Version11;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            Stream httprq_Stream = httprequest.GetRequestStream();

            httprq_Stream.Write(dataStream, 0, dataStream.Length);

            HttpWebResponse httpresponse = (HttpWebResponse)httprequest.GetResponse();

            Stream httprs_stream = httpresponse.GetResponseStream();

            StreamReader httpsr = new StreamReader(httprs_stream);

            strresultxml = httpsr.ReadToEnd();



            httpsr.Close();
            httprs_stream.Close();

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
    public static DataTable getpccdetails(string b2cidn)
    {
        string connstrng = manage_data.con;
        string cmd = "select * from v_agency_provider where b2c_idn =" +  b2cidn + " and is_active = 'Y'";
        ////string cmd = "select * from p_agnecy_info where agency_wp_url = 'signaturetraveldemo.tripxol.com'";
        DataTable dt = new DataTable();
        string id = "";
        try
        {
            SqlConnection sqlcon = new SqlConnection(connstrng);
            SqlDataAdapter da = new SqlDataAdapter(cmd, sqlcon);
            DataSet ds = new DataSet();
            da.Fill(ds, "datatable");
            dt = ds.Tables["datatable"];
        }
        catch (Exception ex)
        {

        }
        return dt;
    }

}