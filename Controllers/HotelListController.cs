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
using System.Web.UI.WebControls;
namespace TripxolHotelsWebapi.Controllers
{
    public class HotelListController : ApiController
    {
        readonly PagedDataSource _pgsource = new PagedDataSource();
      //  public DataTable  Get(string searchid,string checkin,string checkout,string rooms,string rating)
        public string GetHL(string searchid)
        {
            string rvalue = "";
            
            string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_HotelData.xml");
            DataSet ds = new DataSet();
            ds.ReadXml(HLFPath);
            DataTable Hotellist =ds.Tables[0];
            rvalue = GenerateHL(Hotellist, searchid, "12-23", "12-24", 2, 1);
            return rvalue;
        }
        private string GenerateHL(DataTable dtHL,string searchid,string checkin,string checkout,int selroom,int rating)
        {
            string rvalue="";
            foreach(DataRow dr in dtHL.Rows) {
                rvalue +="<div class='col-md-12 hotel-details'><div class='row'>";
                rvalue += "<div class='htl-img-blk col-md-4'><img src='" + dr["MainImage"].ToString() + "' class='img-responsive' /></div>";
                rvalue += "<div class='col-md-8 col-sm-8 col-xs-8 htl-content'> <div class='col-md-8 col-sm-8 col-xs-8 htl-cont-left'>";
                rvalue +="<div class='htl-header'><div class='htl-name'>";
                rvalue +="<a target='_blank' href='HotelDetails.aspx?id=" + searchid + "&HotelCode=" + dr["HotelCode"].ToString() + "&chkin=" + checkin + "&chkout=" +checkout +" &gc=" + selroom + "&award=" + dr["Rating"] + "'>";
                rvalue +="<h4>" + dr["HotelName"].ToString() + "<span class='star-blk'><img src='" + dr["Rating"]+ "' class='img-responsive' /></span></h4> </a> </div> </div>";
                rvalue +="<div class='htl-adress'> <p><span class='addressrpblck'><i class='fa fa-map-marker' aria-hidden='true'></i>" + dr["Address"].ToString()+ "</span> |<a href='#' id='btnShow1' data-toggle='modal' data-target='#DivMap' onclick='showmap(" + dr["Latitude"].ToString() + "," + dr["Longitude"].ToString() + ")'> Show Map </a></span></p></div>";
                rvalue +="<div class='facilities'>" + dr["FetAmenities"].ToString() + "</div>";
                rvalue +="<div class='reviws-blk'><ul class='list-inline'><li class='review-rate'>8.5</li><li class='reviw-scre'><span>Excellent</span> (3261 reviews)</li> </ul> </div>  </div>";
                rvalue +="<div class='col-md-4 col-sm-4 col-xs-4 htl-cont-right'><h2 class='price-cnt'>" + dr["RateRange_Max"].ToString() + "</h2>";
                rvalue +="<p class='srch-pr-nyt'>Per Night</p>";
                rvalue +="<a target='_blank' href='HotelDetails.aspx?id=" + searchid + "&HotelCode=" + dr["HotelCode"].ToString() + "&chkin=" + checkin + "&chkout=" + checkout +"&gc=" + selroom + "&award=" + dr["RatingValue"].ToString() + "' class='chocse-rm'>Choose Room</a>";
                rvalue +="<p class='avail-rms'><i class='fa fa-angle-down' aria-hidden='true'></i>";
                rvalue +=" <a class='lnkviewavrooms' style='cursor:pointer'  onclick=\"RoomDetails('" + dr["HotelCode"].ToString() + "',this,'" + searchid + "')\">View Available Rooms</a></p></div>";
                rvalue +="<div class='discount col-md-12 col-sm-12 col-xs-12 col-sm-12 col-xs-12'> <div class='benft'> <p><span class='hotl-logo'>";
                rvalue+="<img src='" + dr["logo"] + "' /></span> <asp:Literal ID='promotext' runat='server'></asp:Literal></p> </div> </div></div></div>";
                rvalue+="<div class='availble-rm-blk' id='divavailroom" + dr["HotelCode"].ToString()+ "'>";
                rvalue += "<div id='divloader" + dr["HotelCode"].ToString() + " class='loaing-bg_img' style='display:none;width: 100%; margin-top: 20px; text-align: center;'><img src='images/loader.gif' /><br /> please wait..</div></div></div>";
            }
            return rvalue;
        }
    }
}
