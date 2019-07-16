using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Web;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http.Headers;
namespace TripxolHotelsWebapi.Controllers
{
    //public class HotelPostDataAj
    //{
    //    public string HotelCode { get; set; }
    //    public string Searchid { get; set; }
    //  //  public string ContextResult { get; set; }
    //}
    public class RoomDetailsHSController : ApiController
    {
        public static string pcc = "";
        public static string ipcc = "";
        public static string username = "";
        public static string password = "";
        public static string result = "";
        public string ContextResult = "";
        TextInfo TextInfo;
     public string GetRoomDetails(string searchid, string hcode)
        {
            string rvalue = "";
            //pcc = "VL5H";
            //ipcc = "7A7H"; ;
            //username = "373541";
            //password = "WS110542";
           //getting the contexchangeresult
            //string filePathContext = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelAvail/" + searchid + "_Context.xml");
            //if (File.Exists(filePathContext))
            //{
            //    ContextResult = File.ReadAllText(filePathContext);
            //}
            //else
            //{
            //   // ContextResult = XMLRead.ContextChange(pcc, ipcc, username, password, searchid);
            //}

           //ManageHotelDetails mhd = new ManageHotelDetails();
           //HotelProperty hpr = mhd.GetData(searchid, hcode);

         // HotelProperty hpr = mhd.GetData(searchid, ContextResult, hcode);

            //string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelProperty/" + searchid + "_" + hcode + "_PropertyData.xml");
           //if (!File.Exists(filePathRQ))
           //{
           //    File.WriteAllText(filePathRQ, hpr.PropertyXmlResult);
           //}


            ManageHotelDetails mhd = new ManageHotelDetails();
            HotelProperty hpr = mhd.GetData(searchid, hcode);

           string stravroomdet = "";
           string stravroomdetmore = "";
           string lbav = "lnkviewavrooms" + hcode;
           string lessid = "divavailroomLess" + hcode;
           string moreid = "divavailroomMore" + hcode;
           stravroomdet = "<div class='availble-rm-blk' id='" + lessid + "'>";
           stravroomdet += "<div class='row avail-rm-inn'>";
           stravroomdet += "<div class='avail-rm-header'> <h3 class='avail-rm-hd'>available rooms</h3><a style='cursor:pointer' onclick=\"CloseAvlRoom('" + lessid + "','" + lbav + "')\"><i class='fas fa-times-circle'></i>close</a></div>";
           stravroomdet += GetRoomdet(hpr.dtRoomInfo, hpr, hcode,3, searchid);
           stravroomdet += "</div>";
           if (hpr.dtRoomInfo.Rows.Count > 3)
           {
               stravroomdet += " <div class='view-mre'><a style='cursor:pointer' onclick='showavroomsMore(\"" + lessid + "\",\"" + moreid + "\")'>view more rooms</a></div>";
           }
           stravroomdet += "</div>";
           stravroomdetmore = "<div class='availble-rm-blk' style='display:none' id='" + moreid + "'>";
           stravroomdetmore += "<div class='row avail-rm-inn'>";
           stravroomdetmore += "<div class='avail-rm-header'> <h3 class='avail-rm-hd'>available rooms</h3><a style='cursor:pointer' onclick=\"CloseAvlRoom('" + moreid + "','" + lbav + "')\"><i class='fas fa-times-circle'></i>close</a></div>";
           stravroomdetmore += GetRoomdet(hpr.dtRoomInfo, hpr, hcode, hpr.dtRoomInfo.Rows.Count, searchid);
           stravroomdetmore += "</div>";
           stravroomdetmore += " <div class='view-mre'><a style='cursor:pointer' onclick='showavroomsLess(\"" + lessid + "\",\"" + moreid + "\")'>view less rooms</a></div>";
           stravroomdetmore += "</div>";
           if (hpr.dtRoomInfo.Rows.Count > 0)
           {
               rvalue = stravroomdet + stravroomdetmore;
           }
           else
           {
               rvalue = " No Rooms Available";
           }
           return rvalue;

        }
     #region supporing functions 
    // private string GetRoomdet(DataTable dtrows, HotelProperty hpr, string hcode, string hdVARsessionHPR,int count,string searchid)
     private string GetRoomdet(DataTable dtrows, HotelProperty hpr, string hcode, int count, string searchid)
      {
          string stravroomdet = "";
            int i = 0;
            string rrate;
          string roomdesc;
         
          foreach (DataRow dr in dtrows.Rows)
          {
              #region room info
              i++;
              if (i <= count)
              {
                  rrate = dr["RatePernight"].ToString();
                  roomdesc = dr["RoomTypedesc"].ToString();
                  stravroomdet += "<div class='available-rooms avl-wht'>";
                  string roomdesc1 = "";
                  try
                  {
                      roomdesc1 = roomdesc.Substring(0, roomdesc.IndexOf(','));
                  }
                  catch
                  {
                      roomdesc1 = roomdesc;
                  }
                  string roomdetpopup = HttpUtility.HtmlEncode(Getroomdetpopup(dr["RPH"].ToString(), hpr, dr));
                  string RDroominfopopup = HttpUtility.HtmlEncode(Getrdroominfopopup(dr["RPH"].ToString(), hpr, dr,searchid ));
                  string ratetable =  HttpUtility.HtmlEncode(GetRateTable(dr["RPH"].ToString(), hpr, dr, searchid));
                  stravroomdet += "<div class='avl-room-dtls'><p>" + roomdesc1 + "</p><a href='#' data-toggle='modal' data-target='#room-details-pop' onclick='showroomdet(\"" + roomdetpopup + "\",\"" + ratetable + "\",\"" + RDroominfopopup + "\")'>room details</a></div>";
                  stravroomdet += "<div class='avl-price-dtls'>";
                  stravroomdet += "<div class='avl-price-inn'><h2>$" + rrate + "</h2> <p>per night</p></div>";
                  stravroomdet += "<div class='avl-bknw-btn'>";
                 // stravroomdet += "<a target='_blank' href='HotelDetails.aspx?id=" + searchid + "&HotelCode=" + hcode + "&RPH=" + dr["RPH"] + "&hdvarhpr=" + hdVARsessionHPR + "'>book now</a>";
                  stravroomdet += "<a target='_blank' href='HotelDetails.aspx?id=" + searchid + "&HotelCode=" + hcode + "&RPH=" + dr["RPH"] + "'>book now</a>";
                  stravroomdet += "</div>";
                  stravroomdet += "</div> ";
                  stravroomdet += "</div>";
              }
              #endregion
          }
                  return stravroomdet;
      }
     private string Getroomdetpopup(string RPH,HotelProperty hpr,DataRow dr){
        string rvalue="";
        string roomtype="";
          try
          {
             roomtype=dr["RoomTypedesc"].ToString().Substring(0,dr["RoomTypedesc"].ToString().IndexOf(','));
          }
          catch {
                roomtype=dr["RoomTypedesc"].ToString().Trim(',');
          }
          rvalue += "<h4 class='modal-title'><span>Room Type:</span>" + roomtype + "</h4>";
        return rvalue;
    }
     private string Getrdroominfopopup(string RPH,HotelProperty hpr,DataRow dr,string searchid){
       string rvalue="";
       string canpolicy = GetVendorMessage("Cancellation",hpr);
       string facilities = GetVendorMessage("Facilities",hpr).TrimEnd(',');
       string bkftdiningpol = GetVendorMessage("Dining", hpr);
       string Miscpolicies = GetVendorMessage("Policies",hpr);
       string Safety = GetVendorMessage("Safety",hpr).TrimEnd(',');
       string services = GetVendorMessage("Services",hpr).TrimEnd(',');

       string roomtype = dr["RoomTypedesc"].ToString().Trim().Trim(',');
       
       rvalue += "<div class='room-infm-blk'><h2>Room Details</h2><p>" + roomtype + "</p></div>";
         rvalue+= "<div class='room-infm-blk'><h2>Cancellation Policy</h2><p>" + canpolicy + "</p> </div>";
         rvalue+="<div class='room-infm-blk'><h2>Room Facilities</h2><p>" +  facilities + "</p></div>";
         rvalue += "<div class='room-infm-blk rmdtls-imp-infrm'><h2>Important Information</h2>";
         rvalue += "<div class='imp-infrm-inn'> <h3>Breakfast/Dining Policy</h3><p>" + bkftdiningpol + "</p></div>";
         rvalue += "<div class='imp-infrm-inn'><h3>Safety</h3> <p>" + Safety + "</p> </div>";
         rvalue += "<div class='imp-infrm-inn'><h3>Services</h3> <p>" + services + "</p> </div>";
         rvalue += "<div class='imp-infrm-inn'><h3>Other Policies</h3> <p>" + Miscpolicies + "</p> </div>";
         rvalue+="</div>";
        return rvalue;
    }
    private string GetRateTable(string RPH,HotelProperty hpr,DataRow dr,string searchid) {
    string rvalue="";
    DataTable dts = HotelDBLayer.GetSearch(searchid);
    DateTime checkindt = Convert.ToDateTime(dts.Rows[0]["CheckInDt"]);
    DateTime checkoutdt = Convert.ToDateTime(dts.Rows[0]["CheckOutDt"]);

    DateTime startweekdate = checkindt.AddDays(-(int)checkindt.DayOfWeek);
    DateTime endweekdate = checkoutdt.AddDays(-(int)checkoutdt.DayOfWeek).AddDays(6); ;

    double noofdays = (endweekdate - startweekdate).TotalDays;
    decimal noofweeks = Decimal.Ceiling(Convert.ToDecimal(noofdays) / 7);
    string strtable = "<table width='100%' border='0' cellspacing='0' cellpadding='0' class='week-table-main'>";
    strtable +="<tbody><tr><td>";
    strtable += "<table width='100%' border='0' cellspacing='0' cellpadding='0' class='week-days'>";
    strtable += "<tbody><tr><td>&nbsp;</td><td>Sun</td><td>mon</td><td>tue</td><td>wed</td><td>thu</td><td>fri</td> <td>sat</td> </tr></tbody></table>";
    strtable += "</td></tr><tr><td> <table width='100%' border='0' cellspacing='0' cellpadding='0' class='price-details'><tbody>";
    DataRow[] roomdescrow = hpr.dtRoomInfo.Select("RPH='" + RPH + "'");
    DataRow[] roomRaterow = hpr.dtRoomRate.Select("RPH='" + RPH + "'");
    DataRow[] hotelpricingrow = hpr.dtHotelPricing.Select("RoomRateID='" + roomRaterow[0]["RoomRateID"].ToString() + "'");
    DataRow[] hpraterangerow = hpr.HP_RateRange.Select("HotelPricingID='" + hotelpricingrow[0]["HotelPricingID"].ToString() + "'");

    DataTable dtr = new DataTable();
    dtr.Columns.Add("id");
    dtr.Columns.Add("amount");
    dtr.Columns.Add("taxes");
    dtr.Columns.Add("surcharges");
    dtr.Columns.Add("expdate", typeof(DateTime));
    dtr.Columns.Add("effedate", typeof(DateTime));
    dtr.Columns.Add("hid");

    if (hpraterangerow.Count() > 0)
    {
        foreach (DataRow drr in hpraterangerow)
        {
            DataRow drp = dtr.NewRow();
            drp[0] = drr[0];
            drp[1] = drr[1];
            drp[2] = drr[2];
            drp[3] = drr[3];
            drp[4] = "2018-" + drr[4].ToString();
            drp[5] = "2018-" + drr[5].ToString();
            drp[6] = drr[6];
            dtr.Rows.Add(drp);
        }
    }
    else
    {
        DataRow drp = dtr.NewRow();
        drp[0] = 0;
        drp[1] = roomdescrow[0]["RatePernight"];
        drp[2] = 0;
        drp[3] = 0;
        drp[4] = "2018-" + checkoutdt.ToString("MM-dd");
        drp[5] = "2018-" +  checkindt.ToString("MM-dd");
        drp[6] = 1;
        dtr.Rows.Add(drp);
    }

    for (int i = 0; i < noofweeks; i++)
    {
        strtable += "<tr><td>Week" + (i+1) + "</td>";
        for (int j = 0; j < 7; j++)
        {
            strtable += "<td>" + GetDayRate(dtr, startweekdate.AddDays((7 * i) + j).ToString("MM-dd-yyyy")) + "</td>";
        }
        strtable += "</tr>";

    }
    strtable += "</tbody> </table> </td> </tr> </tbody></table>";

    return strtable;
}

     private string GetVendorMessage(string MsgType, HotelProperty hpr)
     {
         string rvalue = "";
         CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
         TextInfo = cultureInfo.TextInfo;
         DataRow[] drf = hpr.dtVendorMsgs.Select("MsgType='" + MsgType + "'");
         foreach (DataRow dr in drf)
         {
             rvalue += TextInfo.ToTitleCase(dr[2].ToString().ToLowerInvariant()) + "";
         }
         return rvalue;

     }

     private string GetDayRate(DataTable dt, string rdate)
     {
         string rvalue = "&nbsp";
         DataRow[] dr = dt.Select("expdate >=#" + rdate + "# and  effedate <=#" + rdate + "#");
         if (dr.Count() > 0)
         {
             decimal amount = 0.0M;
             decimal taxes = 0.0M;

             if (dr[0]["amount"].ToString() != "")
             {
                 amount = Convert.ToDecimal(dr[0]["amount"].ToString());
             }
             if (dr[0]["taxes"].ToString() != "")
             {
                 taxes = Convert.ToDecimal(dr[0]["amount"].ToString());
             }
             Decimal rate = amount + taxes;
             rvalue = rate.ToString();
         }
         return rvalue;
     }

     #endregion
    }
}