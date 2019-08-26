using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;
using HotelDevMTWebapi.Models;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;

public class HotelListGenerate
{

    public static string con = ConfigurationManager.ConnectionStrings["SqlConn"].ToString();
    //public static string con_trbl = ConfigurationManager.ConnectionStrings["tripbluhtlscon"].ToString();
    public static string GetStarrating(string chkname, int stars, AvailabilityRS maintable)
    {
        string rvalue = "";
        rvalue += "<div class='filter-label'><label class='pad-Rten'>";
        rvalue += "<input name='" + chkname + "' type='checkbox' id='" + chkname + "' value='" + stars + "'  onclick='FilterChange()' >";
        rvalue += "<span class='lbl padding-8'> <span class='filter-rhtblk'>";
        for (int i = 0; i < stars; i++)
        {
            rvalue += " <i class='fa fa-star' aria-hidden='true'></i>";
        }
        rvalue += "</span></span><span class='filter-rhtblk'> " + "(" + GetStartCount(stars.ToString(), maintable) + ") </span></label></div>";
        return rvalue;
    }
    public static string GetAmenities(string Amtchkname, string amtvalue, string amtname, string amtcondition, string lstHotelCodes, List<HotelDevMTWebapi.Models.Hotel> lstHotels)//DataTable maintable)
    {
        int hotelCount = 0;
        List<Hdbfacilities> lstfacility = Gethotelfacilities("", lstHotelCodes, amtcondition);
        if (lstfacility != null && lstfacility.Count > 0 && lstfacility[0].lstHotelCodes.Count > 0)
        {
            lstfacility[0].lstHotelCodes.ForEach(j =>
            {
                if (lstHotels.Where(k => k.Code == j) != null)
                {
                    if (hotelCount == 300)
                    {
                        hotelCount = 300;
                    }
                    else 
                    {
                        hotelCount++;
                    }
                }
            });
        }

        string rvalue = "";
        rvalue += "<div class='filter-label'> <label class='pad-Rten'>";
        rvalue += "<input name='" + Amtchkname + "' type='checkbox' id='" + Amtchkname + "' value='" + amtvalue + "' onclick='FilterChange()'>";
        rvalue += " <span class='lbl padding-8'>" + amtname + "</span>";
        // rvalue += "<span class='filter-rhtblk'>" + "(" + GetPropinfocount("PropOptInfoType ='" + amtcondition + "' and PropOptInfoInd='true'", dtpropinfo, maintable).ToString() + ") </span> </label> </div>";
        if (hotelCount == 0)
        {
            rvalue += "<span class='filter-rhtblk'>( 0) </span> </label> </div>";
        }
        else
        {
            rvalue += "<span class='filter-rhtblk'>" + "(" + hotelCount + ") </span> </label> </div>";
        }


        return rvalue;
    }
    public static string GetSortBydd(string sortby, string hcount)
    {
        string genstring = "";
        if (Convert.ToInt32(hcount) > 0)
        {
            genstring += " <div class='sortbyblck' id='divsortbyblck' > <label>Sort By</label>";
        }
        else
        {
            genstring += " <div class='sortbyblck' style='display:none'  id='divsortbyblck' > <label>Sort By</label>";
        }
        genstring += "<select name='sort_order' id='sort_order'  class='form-control' onchange='sortbychange()'>";
        if (sortby == "most_popular")
        {
            genstring += "<option value='most_popular' selected='selected'>Most popular</option>";
        }
        else
        {
            genstring += "<option value='most_popular'>Most popular</option>";
        }
        //if (sortby == "distance")
        //{
        //    genstring += "<option value='distance' selected='selected'>Distance</option>";
        //}
        //else
        //{
        //    genstring += "<option value='distance' >Distance</option>";
        //}
        if (sortby == "low_price")
        {
            genstring += "<option value='low_price' selected='selected'>Lowest price</option>";
        }
        else
        {
            genstring += "<option value='low_price' >Lowest price</option>";
        }
        if (sortby == "high_price")
        {
            genstring += "<option value='high_price' selected='selected'>Highest price</option>";
        }
        else
        {
            genstring += "<option value='high_price' >Highest price</option>";
        }
        //if (sortby == "star_rating")
        //{
        //    genstring += "<option value='star_rating' selected='selected'>Star rating</option>";
        //}
        //else
        //{
        //    genstring += "<option value='star_rating' >Star rating</option>";
        //}


        //if (sortby == "guest_score")
        //{
        //    genstring += "<option value='guest_score' selected='selected'>Guest score</option>";
        //}
        //else
        //{
        //    genstring += "<option value='guest_score' >Guest score</option>";
        //}
        if (sortby == "name")
        {
            genstring += " <option value='name' selected='selected'>Hotel name</option>";
        }
        else
        {
            genstring += " <option value='name' >Hotel name</option>";
        }
        //if (sortby == "promos_first")
        //{
        //    genstring += " <option value='promos_first' selected='selected'>Promos first</option>";
        //}
        //else
        //{
        //    genstring += " <option value='promos_first' >Promos first</option>";
        //}
        genstring += "</select></div>";
        return genstring;
    }


    public static Hashtable GetPropinfocount(AvailabilityRS maintable)
    {
        int rv = 0;
        Hashtable hsAmentities = new Hashtable();
        try
        {
            //string condition = cond + "='true'";
            //DataRow[] dta = maintable.Select(condition);
            string[] arr = new string[] { "Restaurant", "Breakfast", "Fitness", "Car park", "Pets", "Smoking Room", "Smoking rooms", "Wi-fi", "Gym", "Outdoor swimming pool", "Indoor swimming pool", "Wired Internet", "Bathroom", "Bar", "DryClean", "supermarket", "alarm clock", "snacks", "pub", "auditorium", "BeachFront", "dining", "EcoCertified", "ExecutiveFloors", "FamilyPlan", "FreeLocalCalls", "FreeShuttle", "Room service", "table tennis", "Tennis", "vacation resort", "volleyball" };

            HotelMaincintent objhtl = new HotelMaincintent();
            List<Hdbfacilities> lstfacility = new List<Hdbfacilities>();
            //objhotel = objhtladress.Hotels.Hotel.Where(k => k.Code == objAvailabilityRS.Hotels.Hotel[i].Code).ToList();

            //objhtl = GetHotelContent(vpageno, objAvailabilityRS.Hotels.Hotel[i].Code);
            //foreach (string amentity in arr)
            //{

            foreach (var item in maintable.Hotels.Hotel)
            {
                lstfacility = Gethotelfacilities(item.Code);
                // if(lstfacility.Where(k=>k.FacilityDesc.Contains(con)).fir==true)
                foreach (string amentity in arr)
                {
                    rv = 0;
                    rv += (lstfacility.Count > 0 && lstfacility.Where(k => k.FacilityDesc.Contains(amentity)) != null) ? 1 : 0;
                    // int j = rv;
                    if (!hsAmentities.ContainsKey(amentity))
                        hsAmentities.Add(amentity, rv);
                    else
                    {
                        int i = rv;
                        i += Convert.ToInt32(hsAmentities[amentity].ToString());
                        hsAmentities[amentity] = i;
                    }
                }

                //foreach (var facility in lstfacility)
                //{
                //    if (facility.FacilityDesc.Contains(cond))
                //        rv++;
                //}
                // lstHdbfacilities=GetFacilityList(item.fa)
                // maintable.Hotels.Hotel.Where(k=>k.)
            }

            //if (!hsAmentities.ContainsKey(amentity))
            //    hsAmentities.Add(amentity, rv);
            //else
            //    hsAmentities[amentity] = rv;

            //rv = dta.Count();
            //}
        }
        catch
        {
        }
        return hsAmentities;
    }
    private static int GetStartCount(string cond, AvailabilityRS maintable)
    {
        
        int rv = 0;
        if (!string.IsNullOrEmpty(cond))
        {
            int rating = Convert.ToInt32(cond);
            rv = maintable.Hotels.Hotel.Where(k => Convert.ToInt32(Convert.ToDouble(k.Reviews.Review.Rate)) == rating).Count();

            //if (rating == 5)
            //    rv = maintable.Hotels.Hotel.Where(k => Convert.ToDouble(k.Reviews.Review.Rate) > 4).Count();
            //else if (rating == 4)
            //    rv = maintable.Hotels.Hotel.Where(k => Convert.ToDouble(k.Reviews.Review.Rate) > 3 && Convert.ToDouble(k.Reviews.Review.Rate) <= 4).Count();
            //else if (rating == 3)
            //    rv = maintable.Hotels.Hotel.Where(k => Convert.ToDouble(k.Reviews.Review.Rate) > 2 && Convert.ToDouble(k.Reviews.Review.Rate) <= 3).Count();
            //else if (rating == 2)
            //    rv = maintable.Hotels.Hotel.Where(k => Convert.ToDouble(k.Reviews.Review.Rate) > 1 && Convert.ToDouble(k.Reviews.Review.Rate) <= 2).Count();
            //else if (rating == 1)
            //    rv = maintable.Hotels.Hotel.Where(k => Convert.ToDouble(k.Reviews.Review.Rate) > 0 && Convert.ToDouble(k.Reviews.Review.Rate) <= 1).Count();
        }
        else
        {
            rv = 0;
        }
        return rv;
    }
    private static string GetPropertyInfoIds(string cond, DataTable dtpropinfo)
    {
        string propinfoid = "";
        DataRow[] drfilter = dtpropinfo.Select(cond);

        foreach (DataRow dr in drfilter)
        {
            propinfoid = propinfoid + "," + dr["BasicPropInfo_ID"];
        }
        propinfoid = propinfoid.Trim(',');

        return propinfoid;
    }


    public static string Encrypt(string mid)
    {
        string EncryptionKey = "tripxolproj_clients";
        byte[] clearBytes = Encoding.Unicode.GetBytes(mid);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                mid = Convert.ToBase64String(ms.ToArray());
            }
        }
        return mid;
    }

    public static string GetHotellist(AvailabilityRS objAvailabilityRS, string searchid, string checkind, string checkoutd, string guestcount, string CurrencyCode, int vpageno, string b2c_idn, string seladults, string selchilds)
    {
        string rvalue = "";
        string facilitydescr = "";
        string agency_name = "";
        string childage = string.Empty;
        double admarkup = 0.00;
        double adpercmarkup = 0.00;
        double clpercmarkup = 0.00;
        double clmarkup = 0.00;
        double finalmarkup = 0.00;
        double finaldiscount = 0.00;
        double addiscount = 0.00;
        double adperdiscount = 0.00;
        double cldiscount = 0.00;
        double clperdiscount = 0.00;
        double baseamount = 0.00;
        string reviewrate = string.Empty;

        DataTable dssearch = HotelDBLayer.GetSearch(searchid);
        if (dssearch.Rows.Count > 0)
        {
            childage = dssearch.Rows[0]["HB_ChildAge"].ToString();
        }
        DataTable dtpccdet = manage_data.getpccdetails(b2c_idn);

        if (dtpccdet.Rows.Count > 0)
        {
            agency_name = dtpccdet.Rows[0]["agency_name"].ToString();

        }



        string b2c_idnencript = Encrypt(b2c_idn);



        double dc = 0.0;
        try
        { dc = (Convert.ToDateTime(checkoutd.ToString()) - Convert.ToDateTime(checkind.ToString())).TotalDays; }
        catch { }

        HotelsRS objhtladress = new HotelsRS();
        //objhtladress = GetHotelContent(vpageno, CurrencyCode, objAvailabilityRS.Hotels.Hotel[0].Code.ToString());

        if (objAvailabilityRS.Hotels.Hotel.Count() > 0)
        {
            //foreach (DataRow drh in dtHList.Rows) {
            //DataRow drh;
            //foreach(Hotel obj in Hotels)
            //{ }
            // string str = "";

            string cmdfacility = "";

            cmdfacility = "select HB_HotelCodes from hotelsearch where searchidn=" + searchid;
            DataTable dthbhotelcds = manage_data.GetDataTable(cmdfacility, manage_data.con);

            DataTable dtffbookingfb = null;
            DataRow[] drimgpath = null;
            if (System.Web.HttpContext.Current.Cache["dtffbookingfb" + searchid] == null)
            {
                string cmdflbkfb = "select HotelCode,Path,ImageTypeCode from HotelImage where HotelCode in (" + dthbhotelcds.Rows[0]["HB_HotelCodes"].ToString() + ") order by HotelCode,Path asc";
                dtffbookingfb = manage_data.GetDataTable(cmdflbkfb, manage_data.flip_conhb);
                System.Web.HttpContext.Current.Cache["dtffbookingfb" + searchid] = dtffbookingfb;
            }
            else
            {
                dtffbookingfb = (DataTable)System.Web.HttpContext.Current.Cache["dtffbookingfb" + searchid];
            }


            for (int i = (vpageno - 1) * 20; i < (vpageno * 20); i++)
            {



                //Hotel objHotel = new Hotel();
                //if (i >= objAvailabilityRS.Hotels.Hotel.Count||i>250)
                //{
                //    break;
                //}


                if (i >= objAvailabilityRS.Hotels.Hotel.Count || i >= 350)
                {
                    break;
                }








                List<HotelDevMTWebapi.Models.Hotel> objhotel = new List<HotelDevMTWebapi.Models.Hotel>();
                //List<HotelDevMTWebapi.Models.HotelsRS> objhotelcontent = new List<HotelDevMTWebapi.Models.HotelsRS>();
                HotelMaincintent objhtl = new HotelMaincintent();
                List<Hdbfacilities> lstfacility = new List<Hdbfacilities>();
                //objhotel = objhtladress.Hotels.Hotel.Where(k => k.Code == objAvailabilityRS.Hotels.Hotel[i].Code).ToList();

                objhtl = GetHotelContent(vpageno, objAvailabilityRS.Hotels.Hotel[i].Code);
                if (objhtl.Name != null)
                {
                    if (objAvailabilityRS.Hotels.Hotel[i].Code == "364262")
                    {
                        string hcd = objAvailabilityRS.Hotels.Hotel[i].Code;
                    }

                    lstfacility = Gethotelfacilities(objAvailabilityRS.Hotels.Hotel[i].Code);
                    //string lengt = objfacility.FacilityCode;
                    facilitydescr = GetFacilityList(lstfacility);

                    #region not using facility code
                    //foreach (Hdbfacilities obj in objfacility)
                    //{
                    //    string decrptnlist = "";
                    //    if (!string.IsNullOrEmpty(obj.FacilityCode))
                    //    {
                    //        decrptnlist = getfacidisc(obj.FacilityCode.ToString(), obj.FacilityGroupCode.ToString());
                    //        facilitydescr += "<li>" + decrptnlist + "</li>";
                    //    }

                    //}

                    //for (int z = 0; z < lengt.Length; z++)
                    //{
                    //    string decrptnlist = "";
                    //    if (objfacility.FacilityCode != null && objfacility.FacilityCode != "")
                    //    {
                    //        decrptnlist = getfacidisc(objfacility.FacilityCode[z].ToString(), objfacility.FacilityGroupCode[z].ToString());
                    //    }
                    //    facilitydescr += "<li>" + decrptnlist + "</li>";
                    //}



                    //string impagePath = objhotel[0].Images.Image.Path;
                    //objhtladress.
                    //objAvailabilityRS.Hotels.Hotel[i].Images.Image.ImageTypeCode = getimpagePath(objAvailabilityRS.Hotels.Hotel[i].Code);
                    #endregion

                    string imagecode = "GEN";
                    string imagecode1 = "HAB";
                    int order = 2;
                    //<img src='http://photos.hotelbeds.com/giata/'" + impagePath + "' 
                    // str += objAvailabilityRS.Hotels.Hotel[i];
                    rvalue += "<div class='col-md-12 hotel-details'><div class='row'>";
                    #region start
                    double star = 0.00;
                    int starval = 0;
                    if (objAvailabilityRS.Hotels.Hotel[i].CategoryName != null || objAvailabilityRS.Hotels.Hotel[i].CategoryName!="")
                    {
                        string ratingval = objAvailabilityRS.Hotels.Hotel[i].CategoryName.ToString();
                        string[] ratingval_split = ratingval.Split(' ');
                        //if (ratingval == "WITHOUT OFFICIAL CATEGORY")
                        //{
                        if ((ratingval_split[0].ToString() != "1" && ratingval_split[0].ToString() != "2" && ratingval_split[0].ToString() != "3" && ratingval_split[0].ToString() != "4" && ratingval_split[0].ToString() != "5"))
                        {
                            star = 0.0;
                            starval = Convert.ToInt32(star);
                        }
                        else
                        {
                             //ratingval_split = ratingval.Split(' ');
                            star = Convert.ToDouble(ratingval_split[0].ToString());
                            starval = Convert.ToInt32(star);
                        }
                    }
                    else
                    {
                        star = 0.0;
                        starval = Convert.ToInt32(star);
                    }

                    drimgpath = dtffbookingfb.Select("HotelCode='" + objAvailabilityRS.Hotels.Hotel[i].Code + "' and ImageTypeCode='" + imagecode + "'");
                    //try
                    //{
                    //    if (drimgpath[0]["Path"].ToString().Length == 0)
                    //    {
                    //        drimgpath = dtffbookingfb.Select("HotelCode='" + objAvailabilityRS.Hotels.Hotel[i].Code + "' and ImageTypeCode='" + imagecode1 + "'");
                    //    }
                    //}
                    //catch
                    //{

                    //}
                    
                    string imgresult = "not found";
                    try
                    {
                        imgresult = checkimg(drimgpath[0]["Path"].ToString());
                    }
                    catch
                    {
                        imgresult = "not found";
                    }
                    if (imgresult == "found")
                    {
                        try
                        {
                            rvalue += " <div class='htl-img-blk col-md-4'> <img src='http://photos.hotelbeds.com/giata/" + drimgpath[0]["Path"].ToString() + "' onerror='this.src=&quot;../images/No%20Image%20found.png&quot;' class='img-responsive' id='HotelImage_" + objAvailabilityRS.Hotels.Hotel[i].Code + "'/></div>";
                        }
                        catch
                        {
                            rvalue += " <div class='htl-img-blk col-md-4'> <img src='../images/No Image found.png' onerror='this.src=&quot;../images/No%20Image%20found.png&quot;' class='img-responsive' id='HotelImage_" + objAvailabilityRS.Hotels.Hotel[i].Code + "'/></div>";

                        }
                    }
                    else
                    {
                        try
                        {
                            rvalue += " <div class='htl-img-blk col-md-4'> <img src='http://photos.hotelbeds.com/giata/" + drimgpath[1]["Path"].ToString() + "' onerror='this.src=&quot;../images/No%20Image%20found.png&quot;' class='img-responsive' id='HotelImage_" + objAvailabilityRS.Hotels.Hotel[i].Code + "'/></div>";
                        }
                        catch
                        {
                            rvalue += " <div class='htl-img-blk col-md-4'> <img src='../images/No Image found.png' onerror='this.src=&quot;../images/No%20Image%20found.png&quot;' class='img-responsive' id='HotelImage_" + objAvailabilityRS.Hotels.Hotel[i].Code + "'/></div>";

                        }
                    }
                    rvalue += "<div class='col-md-8 col-sm-8 col-xs-8 htl-content'> <div class='col-md-8 col-sm-8 col-xs-8 htl-cont-left'>";
                    rvalue += "<div class='htl-header'> <div class='htl-name'>";
                    rvalue += "<a target='_blank' href='HotelDetails_load.aspx?id=" + searchid + "&HotelCode=" + objAvailabilityRS.Hotels.Hotel[i].Code + "&chkin=" + checkind + "&chkout=" + checkoutd + "&gc=" + seladults + "&award=" + star.ToString() + "&childAges=" + childage + "&Toteladults=" + guestcount + "&childs=" + selchilds + "&clid=" + b2c_idnencript + "'>";
                    // rvalue += "<h4>" + objAvailabilityRS.Hotels.Hotel[i].Name + "<span class='star-blk'>";
                    if (starval != 0)
                    {
                        rvalue += "<h4>" + objAvailabilityRS.Hotels.Hotel[i].Name + "<span class='star-blk'><img src='../images/" + starval + "stars.png' class='img-responsive' /> </span></h4>";
                    }
                    else
                    {
                        rvalue += "<h4>" + objAvailabilityRS.Hotels.Hotel[i].Name + "</h4>";
                    }
                    rvalue += "</a>";

                    //if (starval == 1)
                    //{
                    //    rvalue += "<img src='E:/hotelsapi_prod_updt06252018/Reviewimages/1stars.png' class='img-responsive' />";
                    //}
                    //else if (starval == 2)
                    //{
                    //    rvalue += "<img src='E:/hotelsapi_prod_updt06252018/Reviewimages/2stars.png' class='img-responsive' />";
                    //}
                    //else if (starval == 3)
                    //{
                    //    rvalue += "<img src='E:/hotelsapi_prod_updt06252018/Reviewimages/3stars.png' class='img-responsive' />";
                    //}
                    //else if (starval == 4)
                    //{
                    //    rvalue += "<img src='E:/hotelsapi_prod_updt06252018/Reviewimages/4stars.png' class='img-responsive' />";
                    //}
                    //else if (starval == 5)
                    //{
                    //    rvalue += "<img src='E:/hotelsapi_prod_updt06252018/Reviewimages/5stars.png' class='img-responsive' />";
                    //}
                    //else
                    //{
                    //    rvalue += "<img src='E:/hotelsapi_prod_updt06252018/Reviewimages/1stars.png' class='img-responsive' />";
                    //}
                    //rvalue += "</span></h4></a>";
                    rvalue += "</div></div>";
                    rvalue += "<div class='htl-adress'> <p><span class='addressrpblck'><i class='fa fa-map-marker' aria-hidden='true'></i>" + Getaddress(objAvailabilityRS.Hotels.Hotel[i].Code) +
                        "</span> |<a href='#' id='btnShow1' data-toggle='modal' data-target='#DivMap' onclick=\"showmap(" + objAvailabilityRS.Hotels.Hotel[i].Latitude + "," + objAvailabilityRS.Hotels.Hotel[i].Longitude + ",'" + objAvailabilityRS.Hotels.Hotel[i].Name + "')\"> Show Map </a></span></p> </div>";
                    rvalue += "<div class='facilities'><ul class='list-inline'>";
                    rvalue += facilitydescr;
                    rvalue += "</ul></div>";

                    if (objAvailabilityRS.Hotels.Hotel[i].Reviews != null)
                    {
                        reviewrate = objAvailabilityRS.Hotels.Hotel[i].Reviews.Review.Rate;
                        rvalue += " <div class='reviws-blk'> <ul class='list-inline'>";
                        rvalue += "<li class='review-rate'>" + objAvailabilityRS.Hotels.Hotel[i].Reviews.Review.Rate + "</li> <li class='reviw-scre'><span>Excellent</span> (" + objAvailabilityRS.Hotels.Hotel[i].Reviews.Review.ReviewCount + " reviews)</li> </ul></div>";
                    }
                    //else
                    //{
                    //    reviewrate = "3.0";
                    //    rvalue += " <div class='reviws-blk'> <ul class='list-inline'>";
                    //    rvalue += "<li class='review-rate'>" + reviewrate + "</li> <li class='reviw-scre'><span>Excellent</span> (15 reviews)</li> </ul></div>";
                    //}


                    //rvalue += " <div class='reviws-blk'> <ul class='list-inline'>";
                    //rvalue += "<li class='review-rate'>8.5</li> <li class='reviw-scre'><span>Excellent</span> (3261 reviews)</li> </ul></div>";
                    rvalue += "</div>";
                    rvalue += "<div class='col-md-4 col-sm-4 col-xs-4 htl-cont-right'>";



                    baseamount = (Convert.ToDouble(objAvailabilityRS.Hotels.Hotel[i].MinRate) / Convert.ToDouble((dc)));
                    DataTable dt = new DataTable();
                    SqlConnection sqlcon = new SqlConnection(con);
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        cmd.Connection = sqlcon;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "p_SreTS_HDR";
                        cmd.Parameters.AddWithValue("@B2C_IDN", b2c_idn);
                        cmd.Parameters.AddWithValue("@Hotelcode", objAvailabilityRS.Hotels.Hotel[i].Code);
                        cmd.Parameters.AddWithValue("@GDS", "HB");
                        cmd.Parameters.AddWithValue("@IsLoginCust", "Y");
                        SqlDataAdapter sa = new SqlDataAdapter(cmd);
                        sa.Fill(dt);
                    }
                    catch
                    {
                    }


                    if (dt.Rows.Count > 0)
                    {
                        string Ts_mode = string.Empty;
                        Ts_mode = dt.Rows[0]["TS_Mode"].ToString();
                        if (Ts_mode == "Fixed")
                        {
                            admarkup = Convert.ToDouble(dt.Rows[0]["TS_Markup"].ToString());
                            addiscount = Convert.ToDouble(dt.Rows[0]["TS_Discount"].ToString());
                        }
                        else if (Ts_mode == "Percentage")
                        {
                            adpercmarkup = Convert.ToDouble(dt.Rows[0]["TS_Markup"].ToString());
                            adperdiscount = Convert.ToDouble(dt.Rows[0]["TS_Discount"].ToString());
                            admarkup = (((baseamount) / 100.00) * adpercmarkup);
                            addiscount = (((baseamount) / 100.00) * adperdiscount);

                        }
                        else
                        {
                            admarkup = 0.00;
                        }


                        string Cl_Mode = string.Empty;
                        Cl_Mode = dt.Rows[0]["Cl_Mode"].ToString();
                        if (Cl_Mode == "Fixed")
                        {
                            clmarkup = Convert.ToDouble(dt.Rows[0]["Cl_Markup"].ToString());
                            cldiscount = Convert.ToDouble(dt.Rows[0]["Cl_Discount"].ToString());
                        }
                        else if (Cl_Mode == "Percentage")
                        {
                            clpercmarkup = Convert.ToDouble(dt.Rows[0]["Cl_Markup"].ToString());
                            clperdiscount = Convert.ToDouble(dt.Rows[0]["Cl_Discount"].ToString());
                            clmarkup = ((baseamount / 100.00) * clpercmarkup);
                            cldiscount = ((baseamount / 100.00) * clperdiscount);

                        }
                        else
                        {
                            clmarkup = 0.00;
                        }

                        finalmarkup = admarkup + clmarkup;
                        finaldiscount = addiscount + cldiscount;
                        baseamount = baseamount + (finalmarkup - finaldiscount);
                        //baseamount = baseamount - finaldiscount;

                    }


                    rvalue += " <h2 class='price-cnt'>" + Utilities.GetRatewithSymbol(CurrencyCode) + Convert.ToDouble((baseamount)).ToString("0.00") + "</h2>";
                    rvalue += " <p class='srch-pr-nyt'>Per Night</p>";
                    if (objAvailabilityRS.Hotels.Hotel[i].Reviews != null)
                    {
                        reviewrate = objAvailabilityRS.Hotels.Hotel[i].Reviews.Review.Rate;
                    }

                    rvalue += "<a target='_blank' onclick='checksessout()' href='HotelDetails_load.aspx?id=" + searchid + "&HotelCode=" + objAvailabilityRS.Hotels.Hotel[i].Code + "&chkin=" + checkind + "&chkout=" + checkoutd + "&gc=" + seladults + "&award=" + reviewrate + "&childAges=" + childage + "&Currency=" + CurrencyCode + "&Totalguest=" + guestcount + "&childs=" + selchilds + "&clid=" + b2c_idnencript + "' class='chocse-rm'>Choose Room</a>";//objAvailabilityRS.Hotels.Hotel[i].Reviews.Review.Rate
                    rvalue += " <p class='avail-rms'>";

                    if (objAvailabilityRS.Hotels.Hotel[i].Reviews != null)
                    {
                        reviewrate = objAvailabilityRS.Hotels.Hotel[i].Reviews.Review.Rate;
                    }
                    rvalue += "<a class='lnkviewavrooms' style='cursor:pointer;'  onclick=\"RoomDetails('" + objAvailabilityRS.Hotels.Hotel[i].Code + "',this,' " + reviewrate + "')\">View Available Rooms</a>";//searchid
                    rvalue += " </p></div>";
                    rvalue += " <div class='discount col-md-12 col-sm-12 col-xs-12 col-sm-12 col-xs-12'>";
                    rvalue += " <div class='benft'>  <p> <span class='hotl-logo'>";
                    // <img src='" + objAvailabilityRS.Hotels.Hotel[i].Code + "'
                    try
                    {
                        rvalue += " <img src='http://photos.hotelbeds.com/giata/" + drimgpath[2]["Path"].ToString() + "' onerror='this.src=&quot;../images/No%20Image%20found.png&quot;' id='HotelLogo_" + objAvailabilityRS.Hotels.Hotel[i].Code + "'/></span>*Special Discounted Rates </p> </div>";
                    }
                    catch
                    {
                        rvalue += " <img src='../images/No Image found.png' onerror='this.src=&quot;../images/No%20Image%20found.png&quot;' id='HotelLogo_" + objAvailabilityRS.Hotels.Hotel[i].Code + "'/></span>*Special Discounted Rates </p> </div>";
                    }
                    rvalue += "</div></div> </div>";
                    rvalue += " <div class='availble-rm-blk' id='divavailroom" + objAvailabilityRS.Hotels.Hotel[i].Code + "'>";
                    rvalue += " <div class='loaing-bg_img' style='display:none ;width: 100%; margin-top: 20px; text-align: center;'><img src='../images/loader.gif' /><br /> please wait..</div>";
                    #endregion end
                    rvalue += "</div></div>";
                }
            }
        }
        return rvalue;
    }
    public static AvailabilityRS GetFilteredData(AvailabilityRS objAvailabilityRS, string Hotelname, string HotelChain, string sortby, string stars, string amenities, string vlprice = "", string vhprice = "")
    {
        DataTable Hotels = new DataTable();
        AvailabilityRS _obj = new AvailabilityRS();

        _obj = CloneUtil.CloneJson(objAvailabilityRS);

        _obj.Hotels.Hotel = new List<HotelDevMTWebapi.Models.Hotel>();
        List<HotelDevMTWebapi.Models.Hotel> lstHotels = new List<HotelDevMTWebapi.Models.Hotel>();

        // string hotelnamecond = "";
        if (!string.IsNullOrEmpty(Hotelname))
        {
            _obj.Hotels.Hotel = objAvailabilityRS.Hotels.Hotel.Where(k => k.Name.ToLower().Contains(Hotelname.ToLower())).ToList();
        }
        //string Chaincond = "";
        if (!string.IsNullOrEmpty(HotelChain))
        {
            _obj.Hotels.Hotel = objAvailabilityRS.Hotels.Hotel.Where(k => k.Code == HotelChain).ToList();
        }

        if (!string.IsNullOrEmpty(stars) && stars != "undefined")
        {
            if (_obj.Hotels.Hotel != null && _obj.Hotels.Hotel.Count > 0)
            {
                _obj.Hotels.Hotel = GetStarFilter(stars, _obj.Hotels.Hotel);
            }
            else
            {
                _obj.Hotels.Hotel = GetStarFilter(stars, objAvailabilityRS.Hotels.Hotel);
            }
        }
        //yogi
        if (!string.IsNullOrEmpty(amenities))
        {
            List<Hdbfacilities> lstfacility = new List<Hdbfacilities>();
            lstHotels = new List<HotelDevMTWebapi.Models.Hotel>();

            string lstHotelCodes = string.Join(",", objAvailabilityRS.Hotels.Hotel.Select(k => k.Code));

            lstfacility = HotelListGenerate.Gethotelfacilities("", lstHotelCodes, amenities);

            if (lstfacility != null && lstfacility.Count > 0)
            {
                List<string> lstCodes = lstfacility.Select(j => j.lstHotelCodes).FirstOrDefault();

                if (_obj.Hotels.Hotel != null && _obj.Hotels.Hotel.Count > 0)
                {
                    foreach (var item in lstCodes)
                    {
                        lstHotels.Add(_obj.Hotels.Hotel.Where(k => k.Code == item).FirstOrDefault());
                    }
                }
                else
                {
                    foreach (var item in lstCodes)
                    {
                        lstHotels.Add(objAvailabilityRS.Hotels.Hotel.Where(k => k.Code == item).FirstOrDefault());
                    }
                }
            }

            if (lstHotels != null && lstHotels.Count > 0)
            {
                _obj.Hotels.Hotel = lstHotels;
            }
        }

        if (!string.IsNullOrEmpty(vlprice) && !string.IsNullOrEmpty(vhprice))
        {
            if (_obj.Hotels.Hotel != null && _obj.Hotels.Hotel.Count > 0)
            {
                _obj.Hotels.Hotel = _obj.Hotels.Hotel.Where(k => Convert.ToDouble(k.MinRate) >= Convert.ToDouble(vlprice)
                        && Convert.ToDouble(k.MaxRate) <= Convert.ToDouble(vhprice)).ToList();
            }
            else
            {
                if (string.IsNullOrEmpty(Hotelname) && string.IsNullOrEmpty(HotelChain) && string.IsNullOrEmpty(stars) && string.IsNullOrEmpty(amenities))
                    _obj.Hotels.Hotel = objAvailabilityRS.Hotels.Hotel.Where(k => Convert.ToDouble(k.MinRate) >= Convert.ToDouble(vlprice)
                        && Convert.ToDouble(k.MaxRate) <= Convert.ToDouble(vhprice)).ToList();
            }
        }

        if (stars == "undefined")
        {
            stars = "";
        }
        if (_obj.Hotels.Hotel.Count == 0 && string.IsNullOrEmpty(Hotelname) && string.IsNullOrEmpty(amenities) && string.IsNullOrEmpty(stars) && string.IsNullOrEmpty(HotelChain))//_obj.Hotels.Hotel == null && 
        {
            _obj.Hotels.Hotel = objAvailabilityRS.Hotels.Hotel;
        }

        _obj = sortingHotels(_obj, sortby);
        return _obj;
        ////   return Hotels;

    }
    //from kranthi c_by me
    //public static string GetPricrangecondzero(AvailabilityRS objAvailabilityRS)
    //{
    //    string rvalue = "";

    //    try
    //    {
    //        rvalue = objAvailabilityRS.Hotels.Hotel[0].MaxRate.ToString();
    //    }
    //    catch { }
    //    return rvalue;
    //}


    public static string GetPricrangecondzero()
    {
        string rvalue = "";

        try
        {
            rvalue = "(RateRange_Maxrate >0)";
        }
        catch { }
        return rvalue;
    }
    public static AvailabilityRS FilterTable(AvailabilityRS objAvailabilityRS, String Conditon)
    {
        //DataTable dtfilter = dtmain.Clone();
        //DataRow[] drfilter = dtmain.Select(Conditon);

        //foreach (DataRow dr in drfilter)
        //{
        //    object[] row = dr.ItemArray;
        //    dtfilter.Rows.Add(row);
        //}
        //return dtfilter;
        AvailabilityRS _availabilityRS = new AvailabilityRS();
        _availabilityRS = objAvailabilityRS;
        _availabilityRS.Hotels.Hotel = objAvailabilityRS.Hotels.Hotel.Where(k => Convert.ToDouble(k.MinRate) > 0).ToList();
        return _availabilityRS;

    }
    private static string Getratingcond(string stars)
    {
        string ratingcond = "";
        if (stars != "" && stars != null)
        {
            string[] starfils = stars.TrimEnd(',').Split(',');

            foreach (string str in starfils)
            {
                if (str == "5")
                {
                    ratingcond += "RatingValue = 5";
                }
                if (str == "4")
                {
                    ratingcond += "or RatingValue = 4";
                }
                if (str == "3")
                {
                    ratingcond += "or RatingValue = 3";
                }
                if (str == "2")
                {
                    ratingcond += "or RatingValue = 2";
                }
                if (str == "1")
                {
                    ratingcond += "or RatingValue = 1";
                }
            }

            ratingcond = ratingcond.TrimStart('o').TrimStart('r');
        }
        return ratingcond;
    }

    private static string Getratingcondst(string stars)
    {
        string ratingcond = "";
        if (stars != "" && stars != null)
        {
            string[] starfils = stars.TrimEnd('*').Split('*');

            foreach (string str in starfils)
            {
                if (str == "st5")
                {
                    ratingcond += "RatingValue = 5";
                }
                if (str == "st4")
                {
                    ratingcond += "or RatingValue = 4";
                }
                if (str == "st3")
                {
                    ratingcond += "or RatingValue = 3";
                }
                if (str == "st2")
                {
                    ratingcond += "or RatingValue = 2";
                }
                if (str == "st1")
                {
                    ratingcond += "or RatingValue = 1";
                }
            }

            ratingcond = ratingcond.TrimStart('o').TrimStart('r');
        }
        return ratingcond;

    }


    private static string GetPropinfoCond(string propselected)
    {
        string rvalue = "";
        string Amenitycond = "";
        if (propselected != "" && propselected != null)
        {
            string[] Amenityfils = propselected.TrimEnd('-').Split('-');
            foreach (string str in Amenityfils)
            {
                if (str == "FITN") { Amenitycond += " or Fitness ='true'"; }
                if (str == "POOL") { Amenitycond += " or Indpool ='true'"; }
                if (str == "HSPD") { Amenitycond += " or Internet ='true'"; }
                if (str == "BKST") { Amenitycond += " or Breakfast ='true'"; }
                if (str == "KT") { Amenitycond += " or Kitchen ='true'"; }
                if (str == "PARK") { Amenitycond += " or Freeparking ='true'"; }
                if (str == "NSMK") { Amenitycond += " or Nonsmoking ='true'"; }
                if (str == "ADAA") { Amenitycond += " or accessible ='true'"; }
                if (str == "PETS") { Amenitycond += " or pets ='true'"; }
                if (str == "SHTL") { Amenitycond += " or airport ='true'"; }
                if (str == "BUSN") { Amenitycond += " or Business ='true'"; }
                if (str == "POOL") { Amenitycond += " or Outpool ='true'"; }
            }
        }

        rvalue = Amenitycond.Trim().TrimStart('o').TrimStart('r');
        return rvalue; ;
    }


    private static string GetPropinfoCondst(string propselected)
    {
        string Amenitycond = "";
        if (propselected != "" && propselected != null)
        {
            string[] Amenityfils = propselected.TrimEnd('*').Split('*');

            foreach (string str in Amenityfils)
            {

                if (str == "FF") { Amenitycond += " Fitness ='true'"; }
                if (str == "JF") { Amenitycond += " or Hottub ='true'"; }
                if (str == "PF") { Amenitycond += " or Indpool ='true'"; }
                if (str == "IT") { Amenitycond += " or Internet ='true'"; }
                if (str == "BF") { Amenitycond += " or Breakfast ='true'"; }
                if (str == "KT") { Amenitycond += " or Kitchen ='true'"; }
                if (str == "FP") { Amenitycond += " or Freeparking ='true'"; }
                if (str == "NS") { Amenitycond += " or Nonsmoking ='true'"; }
                if (str == "AC") { Amenitycond += " or accessible ='true'"; }
                if (str == "PT") { Amenitycond += " or pets ='true'"; }
                if (str == "AS") { Amenitycond += " or airport ='true'"; }
                if (str == "BR") { Amenitycond += " or Business ='true'"; }
                if (str == "OP") { Amenitycond += " or Outpool ='true'"; }
                if (str == "KI") { Amenitycond += " or Kids ='true'"; }

            }
        }
        Amenitycond = Amenitycond.Trim().TrimStart('o').TrimStart('r');
        return Amenitycond;
    }

    public static DataTable GetSortedData(DataTable dtFiltered, string sortby)
    {

        DataTable sortedHotels = dtFiltered;
        try
        {
            DataView dtFilteredv = dtFiltered.DefaultView;

            DataRow[] dtcontr = sortedHotels.Select("contractnegcode not in ('')", "RateRange_Maxrate asc");
            DataRow[] dtnoncontr = sortedHotels.Select("contractnegcode in ('')", "RateRange_Maxrate asc");

            DataTable dt = new DataTable();
            dt.Columns.Add("MainImage");
            dt.Columns.Add("Logo");
            dt.Columns.Add("BasicPropertyInfo_ID");
            dt.Columns.Add("AreaID");
            dt.Columns.Add("ChainCode");
            dt.Columns.Add("GEO_ConfidenceLevel");
            dt.Columns.Add("HotelCode");
            dt.Columns.Add("HotelCityCode");
            dt.Columns.Add("HotelName");
            dt.Columns.Add("Latitude");
            dt.Columns.Add("Longitude");
            dt.Columns.Add("Phone");
            dt.Columns.Add("Fax");
            dt.Columns.Add("Address");
            dt.Columns.Add("Rating");
            dt.Columns.Add("RatingValue", typeof(int));
            dt.Columns.Add("RateRange_Max");
            dt.Columns.Add("RateRange_Maxrate", typeof(decimal));
            dt.Columns.Add("CurrencyCode");
            dt.Columns.Add("FetAmenities");
            dt.Columns.Add("logo");
            dt.Columns.Add("image");
            dt.Columns.Add("ChainName");
            dt.Columns.Add("Fitness");
            dt.Columns.Add("Hottub");
            dt.Columns.Add("Indpool");
            dt.Columns.Add("Internet");
            dt.Columns.Add("Breakfast");
            dt.Columns.Add("Kitchen");
            dt.Columns.Add("Freeparking");
            dt.Columns.Add("Nonsmoking");
            dt.Columns.Add("accessible");
            dt.Columns.Add("pets");
            dt.Columns.Add("airport");
            dt.Columns.Add("Business");
            dt.Columns.Add("Outpool");
            dt.Columns.Add("Kids");
            dt.Columns.Add("contractnegcode");
            foreach (DataRow dr in dtcontr)
            {
                DataRow drnew = dt.NewRow();
                drnew["MainImage"] = dr["MainImage"];
                drnew["Logo"] = dr["Logo"];
                drnew["BasicPropertyInfo_ID"] = dr["BasicPropertyInfo_ID"];
                drnew["AreaID"] = dr["AreaID"];
                drnew["ChainCode"] = dr["ChainCode"];
                drnew["GEO_ConfidenceLevel"] = dr["GEO_ConfidenceLevel"];
                drnew["HotelCode"] = dr["HotelCode"];
                drnew["HotelCityCode"] = dr["HotelCityCode"];
                drnew["HotelName"] = dr["HotelName"];
                drnew["Latitude"] = dr["Latitude"];
                drnew["Longitude"] = dr["Longitude"];
                drnew["Phone"] = dr["Phone"];
                drnew["Fax"] = dr["Fax"];
                drnew["Address"] = dr["Address"];
                drnew["Rating"] = dr["Rating"];
                drnew["RatingValue"] = dr["RatingValue"];
                drnew["RateRange_Max"] = dr["RateRange_Max"];
                drnew["RateRange_Maxrate"] = dr["RateRange_Maxrate"];
                drnew["CurrencyCode"] = dr["CurrencyCode"];
                drnew["FetAmenities"] = dr["FetAmenities"];
                drnew["logo"] = dr["logo"];
                drnew["image"] = dr["image"];
                drnew["ChainName"] = dr["ChainName"];
                drnew["Fitness"] = dr["Fitness"];
                drnew["Hottub"] = dr["Hottub"];
                drnew["Indpool"] = dr["Indpool"];
                drnew["Internet"] = dr["Internet"];
                drnew["Breakfast"] = dr["Breakfast"];
                drnew["Kitchen"] = dr["Kitchen"];
                drnew["Freeparking"] = dr["Freeparking"];
                drnew["Nonsmoking"] = dr["Nonsmoking"];
                drnew["accessible"] = dr["accessible"];
                drnew["pets"] = dr["pets"];
                drnew["airport"] = dr["airport"];
                drnew["Business"] = dr["Business"];
                drnew["Outpool"] = dr["Outpool"];
                drnew["Kids"] = dr["Kids"];
                drnew["contractnegcode"] = dr["contractnegcode"];

                dt.Rows.Add(drnew);

            }
            foreach (DataRow dr in dtnoncontr)
            {
                DataRow drnew = dt.NewRow();
                drnew["MainImage"] = dr["MainImage"];
                drnew["Logo"] = dr["Logo"];
                drnew["BasicPropertyInfo_ID"] = dr["BasicPropertyInfo_ID"];
                drnew["AreaID"] = dr["AreaID"];
                drnew["ChainCode"] = dr["ChainCode"];
                drnew["GEO_ConfidenceLevel"] = dr["GEO_ConfidenceLevel"];
                drnew["HotelCode"] = dr["HotelCode"];
                drnew["HotelCityCode"] = dr["HotelCityCode"];
                drnew["HotelName"] = dr["HotelName"];
                drnew["Latitude"] = dr["Latitude"];
                drnew["Longitude"] = dr["Longitude"];
                drnew["Phone"] = dr["Phone"];
                drnew["Fax"] = dr["Fax"];
                drnew["Address"] = dr["Address"];
                drnew["Rating"] = dr["Rating"];
                drnew["RatingValue"] = dr["RatingValue"];
                drnew["RateRange_Max"] = dr["RateRange_Max"];
                drnew["RateRange_Maxrate"] = dr["RateRange_Maxrate"];
                drnew["CurrencyCode"] = dr["CurrencyCode"];
                drnew["FetAmenities"] = dr["FetAmenities"];
                drnew["logo"] = dr["logo"];
                drnew["image"] = dr["image"];
                drnew["ChainName"] = dr["ChainName"];
                drnew["Fitness"] = dr["Fitness"];
                drnew["Hottub"] = dr["Hottub"];
                drnew["Indpool"] = dr["Indpool"];
                drnew["Internet"] = dr["Internet"];
                drnew["Breakfast"] = dr["Breakfast"];
                drnew["Kitchen"] = dr["Kitchen"];
                drnew["Freeparking"] = dr["Freeparking"];
                drnew["Nonsmoking"] = dr["Nonsmoking"];
                drnew["accessible"] = dr["accessible"];
                drnew["pets"] = dr["pets"];
                drnew["airport"] = dr["airport"];
                drnew["Business"] = dr["Business"];
                drnew["Outpool"] = dr["Outpool"];
                drnew["Kids"] = dr["Kids"];
                drnew["contractnegcode"] = dr["contractnegcode"];

                dt.Rows.Add(drnew);

            }


            if (sortby == "") { dtFilteredv.Sort = "contractnegcode,RateRange_Maxrate"; }
            else if (sortby == "most_popular") { dtFilteredv.Sort = "RateRange_Maxrate"; }
            else if (sortby == "distance") { dtFilteredv.Sort = "RateRange_Maxrate"; }
            else if (sortby == "low_price")
            {
                dtFilteredv = dt.DefaultView;

                //dtFilteredv.Sort = "contractnegcode desc,RateRange_Maxrate";

            }
            else if (sortby == "high_price") { dtFilteredv.Sort = "RateRange_Maxrate desc"; }
            else if (sortby == "star_rating") { dtFilteredv.Sort = "RatingValue desc"; }
            else if (sortby == "guest_score") { dtFilteredv.Sort = "RateRange_Maxrate"; }
            else if (sortby == "name") { dtFilteredv.Sort = "HotelName"; }
            else if (sortby == "promos_first") { dtFilteredv.Sort = "RateRange_Maxrate"; }
            sortedHotels = dtFilteredv.ToTable();
        }
        catch (Exception ex)
        {
        }
        return sortedHotels;

    }
    public static string GetPagingHtmlTop(int Noofrows, int vpageno)
    {
        int np = Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(Noofrows) / 20));
        string rvalue = "";
        rvalue += "<div class='pagnationr pagnationr-top'><div class='showingresults'> <asp:Label ID='lblpagetop' ></asp:Label></div>";
        rvalue += " <nav aria-label='...'><ul class='pagination'>";
        if (np > 1)
        {
            rvalue += "<li class='page-item'><a class='page-link' ID='lbPrevioustop' OnClick='PagePrevClick(" + vpageno + ")'>Previous</a></li>";
            for (int i = 1; i <= np; i++)
            {
                if (i == vpageno)
                { rvalue += "<li class='page-item'><a ID='lbPagingtop" + i + "' OnClick='PageClick(" + i + ")' class='aspNetDisabled active-bgpg'>" + i + "</a> </li>"; }
                else
                { rvalue += "<li class='page-item'><a ID='lbPagingtop" + i + "' OnClick='PageClick(" + i + ")' class='page-link'>" + i + "</a> </li>"; }
            }
            rvalue += "<li class='page-item'><a class='page-link' ID='lbNexttop' OnClick='PageNextClick(" + vpageno + "," + np + ")'>Next</a></li> ";
        }
        rvalue += "</ul></nav></div></div>";
        return rvalue;

    }
    public static string GetPagingHtmlBottom(int Noofrows, int vpageno)
    {
        int np = Convert.ToInt16(Math.Ceiling(Convert.ToDecimal(Noofrows) / 20));
        string rvalue = "";
        rvalue += "<div class='pagnationr pagnationr-top'><div class='showingresults'> <asp:Label ID='lblpage' ></asp:Label></div>";
        rvalue += " <nav aria-label='...'><ul class='pagination'>";
        if (np > 1)
        {
            rvalue += "<li class='page-item'><a class='page-link' ID='lbPrevious' OnClick='PagePrevClick(" + vpageno + ")'>Previous</a></li>";
            for (int i = 1; i <= np; i++)
            {
                if (i == vpageno)
                { rvalue += "<li class='page-item'><a ID='lbPaging" + i + "' OnClick='PageClick(" + i + ")' class='aspNetDisabled active-bgpg'>" + i + "</a> </li>"; }
                else
                { rvalue += "<li class='page-item'><a ID='lbPaging" + i + "' OnClick='PageClick(" + i + ")' class='page-link'>" + i + "</a> </li>"; }
            }
            rvalue += "<li class='page-item'><a class='page-link' ID='lbNext'  OnClick='PageNextClick(" + vpageno + "," + np + ")'>Next</a></li>";
        }
        rvalue += "</ul></nav></div></div>";
        return rvalue;

    }
    public static string FillHotelChains(AvailabilityRS Hotellist)
    {
        string rvalue = "";
        //DataTable dtc = Hotellist.DefaultView.ToTable(true, "ChainCode", "ChainName");
        //DataView dv1 = dtc.DefaultView;
        //dv1.Sort = "ChainName";
        //DataTable dtc2 = dv1.ToTable();
        rvalue += "<select name='ddlhotelchain' id='ddlhotelchain'  class='form-control' onchange='Hotelchainchange()'>";
        rvalue += "<option value=''>select Hotel Name </option>";
        foreach (var dr in Hotellist.Hotels.Hotel)
        {
            if (!string.IsNullOrEmpty(dr.Name))
            {
                rvalue += "<option value='" + dr.Code + "'> " + dr.Name.ToString() + "</option>";
            }
        }
        rvalue += "</select>";
        return rvalue;
    }

    private static string GetChildText(XmlNode pnode, string node)
    {
        string rvalue = "";
        try
        {

            foreach (XmlNode xn in pnode.ChildNodes)
            {
                if (xn.Name.ToLower() == node.ToLower())
                {
                    rvalue = xn.InnerText;
                }
            }
        }
        catch
        {
        }
        return rvalue;
    }
    public static string GetChildRRMText(XmlNode pnode, string node)
    {
        string rvalue = "0";
        try
        {

            foreach (XmlNode xn in pnode.ChildNodes)
            {
                if (xn.Name.ToLower() == node.ToLower())
                {
                    rvalue = xn.InnerText;
                }
            }
        }
        catch
        {
        }
        if (rvalue == "")
        {
            rvalue = "0";
        }
        return rvalue;
    }

    public static void CreateTables(DataTable dtBPIadd)
    {
        dtBPIadd.Columns.Add("MainImage");
        dtBPIadd.Columns.Add("Logo");
        dtBPIadd.Columns.Add("BasicPropertyInfo_ID");
        dtBPIadd.Columns.Add("AreaID");
        dtBPIadd.Columns.Add("ChainCode");
        dtBPIadd.Columns.Add("GEO_ConfidenceLevel");
        dtBPIadd.Columns.Add("HotelCode");
        dtBPIadd.Columns.Add("HotelCityCode");
        dtBPIadd.Columns.Add("HotelName");
        dtBPIadd.Columns.Add("Latitude");
        dtBPIadd.Columns.Add("Longitude");
        dtBPIadd.Columns.Add("Phone");
        dtBPIadd.Columns.Add("Fax");
        dtBPIadd.Columns.Add("Address");
        dtBPIadd.Columns.Add("Rating");
        dtBPIadd.Columns.Add("RatingValue", typeof(int));
        dtBPIadd.Columns.Add("RateRange_Max");
        dtBPIadd.Columns.Add("RateRange_Maxrate", typeof(decimal));
        dtBPIadd.Columns.Add("CurrencyCode");
        dtBPIadd.Columns.Add("FetAmenities");
        dtBPIadd.Columns.Add("logo");
        dtBPIadd.Columns.Add("image");
        dtBPIadd.Columns.Add("ChainName");
        dtBPIadd.Columns.Add("Fitness");
        dtBPIadd.Columns.Add("Hottub");
        dtBPIadd.Columns.Add("Indpool");
        dtBPIadd.Columns.Add("Internet");
        dtBPIadd.Columns.Add("Breakfast");
        dtBPIadd.Columns.Add("Kitchen");
        dtBPIadd.Columns.Add("Freeparking");
        dtBPIadd.Columns.Add("Nonsmoking");
        dtBPIadd.Columns.Add("accessible");
        dtBPIadd.Columns.Add("pets");
        dtBPIadd.Columns.Add("airport");
        dtBPIadd.Columns.Add("Business");
        dtBPIadd.Columns.Add("Outpool");
        dtBPIadd.Columns.Add("Kids");
        dtBPIadd.Columns.Add("contractnegcode");
    }
    public static void FillHStable(XmlNode xnod, DataTable dtBPIadd)
    {
        #region FillHotelListTable
        foreach (XmlNode xn in xnod.ChildNodes)
        {
            DataRow dtr = dtBPIadd.NewRow();
            dtr["MainImage"] = GetChildText(xn, "MainImage");
            dtr["Logo"] = GetChildText(xn, "Logo");
            dtr["BasicPropertyInfo_ID"] = GetChildText(xn, "BasicPropertyInfo_ID");
            dtr["AreaID"] = GetChildText(xn, "AreaID");
            dtr["ChainCode"] = GetChildText(xn, "ChainCode");
            dtr["GEO_ConfidenceLevel"] = GetChildText(xn, "GEO_ConfidenceLevel");
            dtr["HotelCode"] = GetChildText(xn, "HotelCode");
            dtr["HotelCityCode"] = GetChildText(xn, "HotelCityCode");
            dtr["HotelName"] = GetChildText(xn, "HotelName");
            dtr["Latitude"] = GetChildText(xn, "Latitude");
            dtr["Longitude"] = GetChildText(xn, "Longitude");
            dtr["Phone"] = GetChildText(xn, "Phone");
            dtr["Fax"] = GetChildText(xn, "Fax");
            dtr["Address"] = GetChildText(xn, "Address");
            dtr["Rating"] = GetChildText(xn, "Rating");
            dtr["RatingValue"] = GetChildText(xn, "RatingValue");
            dtr["RateRange_Max"] = GetChildText(xn, "RateRange_Max");
            dtr["RateRange_Maxrate"] = GetChildRRMText(xn, "RateRange_Maxrate");
            dtr["CurrencyCode"] = GetChildText(xn, "CurrencyCode");
            dtr["FetAmenities"] = GetChildText(xn, "FetAmenities");
            dtr["logo"] = GetChildText(xn, "logo");
            dtr["image"] = GetChildText(xn, "image");
            dtr["ChainName"] = GetChildText(xn, "ChainName");
            dtr["Fitness"] = GetChildText(xn, "Fitness");
            dtr["Hottub"] = GetChildText(xn, "Hottub");
            dtr["Indpool"] = GetChildText(xn, "Indpool");
            dtr["Internet"] = GetChildText(xn, "Internet");
            dtr["Breakfast"] = GetChildText(xn, "Breakfast");
            dtr["Kitchen"] = GetChildText(xn, "Kitchen");
            dtr["Freeparking"] = GetChildText(xn, "Freeparking");
            dtr["Nonsmoking"] = GetChildText(xn, "Nonsmoking");
            dtr["accessible"] = GetChildText(xn, "accessible");
            dtr["pets"] = GetChildText(xn, "pets");
            dtr["airport"] = GetChildText(xn, "airport");
            dtr["Business"] = GetChildText(xn, "Business");
            dtr["Outpool"] = GetChildText(xn, "Outpool");
            dtr["Kids"] = GetChildText(xn, "Kids");
            dtr["contractnegcode"] = GetChildText(xn, "contractnegcode");
            dtBPIadd.Rows.Add(dtr);
        }
        #endregion
    }

    public static AvailabilityRS GetFilteredDataList(AvailabilityRS objAvailabilityRS, string Hotelname, string HotelChain, string sortby, string stars, string amenities, string vlprice, string vhprice)
    {
        DataTable Hotels = new DataTable();

        string checkin = objAvailabilityRS.Hotels.CheckIn;
        string checkout = objAvailabilityRS.Hotels.CheckOut;

        double dc = 0.0;
        try
        { dc = (Convert.ToDateTime(checkout.ToString()) - Convert.ToDateTime(checkin)).TotalDays; }
        catch { }


        //Hotels = dtHList;
        AvailabilityRS _obj = new AvailabilityRS();

        _obj = CloneUtil.CloneJson(objAvailabilityRS);

        //if(!string.IsNullOrEmpty(Hotelname)||!string.IsNullOrEmpty(HotelChain))
        _obj.Hotels.Hotel = new List<HotelDevMTWebapi.Models.Hotel>();
        List<HotelDevMTWebapi.Models.Hotel> lstHotels = new List<HotelDevMTWebapi.Models.Hotel>();





        // string hotelnamecond = "";
        if (!string.IsNullOrEmpty(Hotelname))
        {
            _obj.Hotels.Hotel = objAvailabilityRS.Hotels.Hotel.Where(k => k.Name.ToLower().Contains(Hotelname.ToLower())).ToList();


        }

        //string Chaincond = "";
        if (!string.IsNullOrEmpty(HotelChain))
        {
            _obj.Hotels.Hotel = objAvailabilityRS.Hotels.Hotel.Where(k => k.Code == HotelChain).ToList();
        }
        //yogi


        if (!string.IsNullOrEmpty(stars))
        {
            if (_obj.Hotels.Hotel != null && _obj.Hotels.Hotel.Count > 0)
                _obj.Hotels.Hotel = GetStarFilter(stars, _obj.Hotels.Hotel);
            else
                _obj.Hotels.Hotel = GetStarFilter(stars, objAvailabilityRS.Hotels.Hotel);
        }



        //string propinfocond = GetPropinfoCondst(amenities);
        //DataTable FilteredTable = FilterTable(dtPricing, propinfocond);


        if (!string.IsNullOrEmpty(amenities))
        {
            List<Hdbfacilities> lstfacility = new List<Hdbfacilities>();
            lstHotels = new List<HotelDevMTWebapi.Models.Hotel>();

            string lstHotelCodes = string.Join(",", objAvailabilityRS.Hotels.Hotel.Select(k => k.Code));

            lstfacility = HotelListGenerate.Gethotelfacilities("", lstHotelCodes, amenities);

            if (lstfacility != null && lstfacility.Count > 0)
            {
                List<string> lstCodes = lstfacility.Select(j => j.lstHotelCodes).FirstOrDefault();

                if (_obj.Hotels.Hotel != null && _obj.Hotels.Hotel.Count > 0)
                {
                    foreach (var item in lstCodes)
                    {
                        lstHotels.Add(_obj.Hotels.Hotel.Where(k => k.Code == item).FirstOrDefault());
                    }
                }
                else
                {
                    foreach (var item in lstCodes)
                    {
                        lstHotels.Add(objAvailabilityRS.Hotels.Hotel.Where(k => k.Code == item).FirstOrDefault());
                    }
                }
            }

            if (lstHotels != null && lstHotels.Count > 0)
                _obj.Hotels.Hotel = lstHotels;

        }

        if (!string.IsNullOrEmpty(vlprice) && !string.IsNullOrEmpty(vhprice))
        {


            if (_obj.Hotels.Hotel != null && _obj.Hotels.Hotel.Count > 0)
            {


                //_obj.Hotels.Hotel = _obj.Hotels.Hotel.Where(k => Convert.ToDouble(Convert.ToDouble(k.MinRate)/(dc)) >= Convert.ToDouble(vlprice)
                //       && Convert.ToDouble(Convert.ToDouble(k.MinRate)/dc) <= Convert.ToDouble(vhprice)).ToList();

                //for sorting by hotel name missing some hotel 08-16-19
                //_obj.Hotels.Hotel = _obj.Hotels.Hotel.Where(k => Convert.ToDouble(k.MinRate) >= Convert.ToDouble(vlprice)
                //       && Convert.ToDouble(k.MaxRate) <= Convert.ToDouble(vhprice)).ToList();


                double maxrate = Convert.ToDouble(_obj.Hotels.Hotel.Max(k => Convert.ToDouble(k.MinRate)).ToString());
                _obj.Hotels.Hotel = _obj.Hotels.Hotel.Where(k => Convert.ToDouble(k.MinRate) >= Convert.ToDouble(vlprice)
                        && Convert.ToDouble(Convert.ToDouble(maxrate) /dc) <= Convert.ToDouble(vhprice)).ToList();
            }
            else
            {
                //if (string.IsNullOrEmpty(Hotelname) && string.IsNullOrEmpty(HotelChain) && string.IsNullOrEmpty(stars) && string.IsNullOrEmpty(amenities))
                //    _obj.Hotels.Hotel = objAvailabilityRS.Hotels.Hotel.Where(k => Convert.ToDouble(Convert.ToDouble(k.MinRate) / (dc)) >= Convert.ToDouble(vlprice)
                //        && Convert.ToDouble(Convert.ToDouble(k.MinRate) / (dc)) <= Convert.ToDouble(vhprice)).ToList();


                if (string.IsNullOrEmpty(Hotelname) && string.IsNullOrEmpty(HotelChain) && string.IsNullOrEmpty(stars) && string.IsNullOrEmpty(amenities)
                    && !string.IsNullOrEmpty(vlprice) && !string.IsNullOrEmpty(vhprice))
                {

                    double minRate = (Convert.ToDouble(vlprice) * dc);
                    double maxRate = (Convert.ToDouble(vhprice) * dc);

                    _obj.Hotels.Hotel = objAvailabilityRS.Hotels.Hotel.Where(k => Convert.ToDouble(k.MinRate) >= minRate
                        && Convert.ToDouble(k.MinRate) <= maxRate).ToList();
                }
            }
        }



        if (_obj.Hotels.Hotel.Count == 0 && string.IsNullOrEmpty(Hotelname) && string.IsNullOrEmpty(HotelChain) && string.IsNullOrEmpty(stars) && string.IsNullOrEmpty(amenities))//_obj.Hotels.Hotel == null
        {
            _obj.Hotels.Hotel = objAvailabilityRS.Hotels.Hotel;
        }

        _obj = sortingHotels(_obj, sortby);
        return _obj;
    }

    private static string GetPricerangecond(string lprice, string hprice)
    {
        string rvalue = "";

        try
        {
            rvalue = "(RateRange_Maxrate >= " + lprice + " and RateRange_Maxrate <=" + hprice + ")";
        }
        catch { }
        return rvalue;
    }

    public static string Getaddress(string hotelcode)
    {
        DataTable result = new DataTable();
        string cmdtxt = "";
        string add = string.Empty;
        try
        {
            //cmdtxt = "select  DestinationCode, Address, City, PostalCode, Longitude, Latitude, ZoneCode, CountryCode, Email, IsActive, CreatedDt, LastUpdDt, CreatedUser, LastUpdUser from HotelAddress where HotelCode='" + hotelcode + "'";

            cmdtxt = "select DestinationCode,case when right(rtrim(ltrim(Address)),1) = ',' then substring(rtrim(Address),1,len(rtrim(Address)) - 1)else rtrim(ltrim(Address)) END as Address, City,StateCode, PostalCode, Longitude, Latitude, ZoneCode, CountryCode, Email, IsActive, CreatedDt, LastUpdDt, CreatedUser, LastUpdUser from HotelAddress where HotelCode ='" + hotelcode + "'";
            result = manage_data.GetDataTable(cmdtxt, manage_data.flip_conhb);
            if (result.Rows.Count > 0)
            {
                string[] arr = result.Rows[0]["Address"].ToString().Split(',');
                add = arr[0].ToString() + "," + result.Rows[0]["DestinationCode"].ToString().Trim(',') + "," + result.Rows[0]["City"].ToString() + "," + result.Rows[0]["StateCode"].ToString() + "," + result.Rows[0]["PostalCode"].ToString() + "," + result.Rows[0]["CountryCode"].ToString() + ",";


            }

        }
        catch
        {
        }


        return add.Trim(',');
    }
    public static string getimpagePath(string imagetcode, string hotelcode)
    {
        string imagepath = string.Empty;

        HdbImagepath obj = new HdbImagepath();
        try
        {

            string cmdflbkfb = "select Path from HotelImage where HotelCode='" + hotelcode + "' and ImageTypeCode='" + imagetcode + "' order by HotelCode,Path asc";
            DataTable dtffbookingfb = manage_data.GetDataTable(cmdflbkfb, manage_data.flip_conhb);
            if (dtffbookingfb.Rows.Count > 0)
            {
                obj.Path = dtffbookingfb.Rows[0]["Path"].ToString();
                imagepath = obj.Path;

            }
            else
            {
                // imagepath = "E:/hotelsapi_prod_updt06252018/Images/No Image found.png";
            }

        }
        catch
        {

        }


        return imagepath;
    }



    public static HotelMaincintent GetHotelContent(int pageNo, string cultureCode)
    {
        HotelMaincintent objHotelsMaincnt = new HotelMaincintent();
        //XMLRead.GenerateSignature();
        string strresultxml = "";

        //if (Convert.ToInt32(pageNo) == 1)
        //   toRange = 1000;


        try
        {
            string cmdflbkfb = "select HotelCode, HotelName,HotelDesc, CategoryCode,CategoryGroupCode,ChainCode,AccomTypeCode,Web,S2C,License,IsActive from HotelMain where HotelCode='" + cultureCode + "' ";
            DataTable dtffbookingfb = manage_data.GetDataTable(cmdflbkfb, manage_data.flip_conhb);
            if (dtffbookingfb.Rows.Count > 0)
            {
                objHotelsMaincnt.Code = dtffbookingfb.Rows[0]["HotelCode"].ToString();
                objHotelsMaincnt.Name = dtffbookingfb.Rows[0]["HotelName"].ToString();
                objHotelsMaincnt.Description = dtffbookingfb.Rows[0]["HotelDesc"].ToString();
                objHotelsMaincnt.CountryCode = dtffbookingfb.Rows[0]["CategoryCode"].ToString();
                objHotelsMaincnt.CategoryGroupCode = dtffbookingfb.Rows[0]["CategoryGroupCode"].ToString();
                objHotelsMaincnt.ChainCode = dtffbookingfb.Rows[0]["ChainCode"].ToString();
                objHotelsMaincnt.AccommodationTypeCode = dtffbookingfb.Rows[0]["AccomTypeCode"].ToString();
                objHotelsMaincnt.Web = dtffbookingfb.Rows[0]["Web"].ToString();

                objHotelsMaincnt.S2C = dtffbookingfb.Rows[0]["S2C"].ToString();
                objHotelsMaincnt.License = dtffbookingfb.Rows[0]["License"].ToString();
            }

        }
        catch
        {

        }



        return objHotelsMaincnt; //strresultxml;
    }
    public static List<Hdbfacilities> Gethotelfacilities(string hotelCode = "", string lstAvlHotelCodes = "", string facility = "")
    {
        List<Hdbfacilities> lsthbfacility = new List<Hdbfacilities>();
        try
        {
            if (string.IsNullOrEmpty(facility))
            {
                string cmdfacility = "SELECT TF.FACILITYCODE,TF.FACILITYGROUPCODE,FACILITYDESC FROM FACILITY TF " +
                                     "INNER JOIN HOTELFACILITY HF ON TF.FACILITYCODE= HF.FACILITYCODE AND TF.FACILITYGROUPCODE = HF.FACILITYGROUPCODE" +
                                     " WHERE HF.HOTELCODE = " + hotelCode + " GROUP BY TF.FACILITYGROUPCODE,TF.FACILITYCODE,FACILITYDESC";
                //"select FacilityCode, FacilityGroupCode, IndYesOrNo from HotelFacility where HotelCode='" + hotelcode + "' ";
                DataTable dtfacility = manage_data.GetDataTable(cmdfacility, manage_data.flip_conhb);
                if (dtfacility.Rows.Count > 0)
                {
                    lsthbfacility = (from DataRow dr in dtfacility.Rows
                                     select new Hdbfacilities()
                                   {
                                       FacilityCode = dr["FACILITYCODE"].ToString(),
                                       FacilityGroupCode = dr["FACILITYGROUPCODE"].ToString(),
                                       FacilityDesc = dr["FACILITYDESC"].ToString(),
                                       //IndYesOrNo = dr["IndYesOrNo"].ToString(),
                                   }).ToList();
                }
            }
            else
            {
                //string[] arrFacility = facility.Split('-');
                //facility = string.Empty;
                //if (arrFacility != null && arrFacility.Count() > 0)
                //{
                //    foreach (var item in arrFacility)
                //    {
                //        if (!string.IsNullOrEmpty(item))
                //        {
                //            if (string.IsNullOrEmpty(facility))
                //                facility = "( FC.FACILITYDESC LIKE '%" + item + "%'";
                //            else
                //                facility = facility + " OR FC.FACILITYDESC LIKE '%" + item + "%'";
                //        }
                //    }

                //    if (!string.IsNullOrEmpty(facility))
                //        facility = facility + " )";

                //}
                //string cmdFacilityName = "SELECT DISTINCT HM.HOTELCODE FROM HOTELMAIN HM " +
                //                            "INNER JOIN HOTELFACILITY HF ON HM.HOTELCODE = HF.HOTELCODE " +
                //                            "INNER JOIN FACILITY FC ON FC.FACILITYCODE = HF.FACILITYCODE AND FC.FACILITYGROUPCODE = HF.FACILITYGROUPCODE " +
                //                            "WHERE " + facility + " AND HM.HOTELCODE IN ( " + lstAvlHotelCodes + " )";
                //DataTable dtfacility = manage_data.GetDataTable(cmdFacilityName, manage_data.flip_conhb);
                DataTable dtfacility = new DataTable();
                SqlConnection sqlcon = new SqlConnection(manage_data.flip_conhb);
                try
                {
                    if (facility == "Restaurant")
                    {
                        string f=facility;
                    }
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = sqlcon;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "p_HotelFacilities";
                    cmd.Parameters.AddWithValue("@facilities", facility.Trim());
                    cmd.Parameters.AddWithValue("@hotecodes", lstAvlHotelCodes);

                    SqlDataAdapter sa = new SqlDataAdapter(cmd);
                    sa.Fill(dtfacility);
                }
                catch
                {
                }


               
                if (dtfacility.Rows.Count > 0)
                {
                    Hdbfacilities objHdbfacilities = new Hdbfacilities();
                    objHdbfacilities.lstHotelCodes = new List<string>();
                    foreach (DataRow dr in dtfacility.Rows)
                    {
                        objHdbfacilities.lstHotelCodes.Add(dr["hotecode"].ToString());
                    }
                    lsthbfacility.Add(objHdbfacilities);
                }

            }
        }
        catch
        {

        }
        return lsthbfacility;
    }
    public static string getfacidisc(string facilitycode, string facilitygrpcode)
    {
        string facdescri = string.Empty;

        try
        {
            string cmdfacdescri = "select FacilityDesc from Facility where FacilityCode='" + facilitycode + "' and FacilityGroupCode='" + facilitygrpcode + "' ";
            DataTable dtcmdfacdescri = manage_data.GetDataTable(cmdfacdescri, manage_data.flip_conhb);
            if (dtcmdfacdescri.Rows.Count > 0)
            {
                facdescri = dtcmdfacdescri.Rows[0]["FacilityDesc"].ToString();
            }
        }
        catch
        {
        }
        return facdescri;
    }


    public static string GetToRangeValue(int pageNo)
    {
        string toRangeCondition = "";
        int fromValue = 0;
        int toValue = 0;
        if (pageNo == 1)
        {
            fromValue = 1; toValue = 1000;
        }
        else
        {
            int vPageno = Convert.ToInt32(pageNo);
            fromValue = ((vPageno * 1000) - 1000) + 1;
            toValue = (vPageno * 1000);
        }
        return toRangeCondition = "from=" + fromValue + "&to=" + toValue + "";
    }

    public static AvailabilityRS sortingHotels(AvailabilityRS obj, string sortBy)
    {
        string checkind = obj.Hotels.CheckIn;
        string checkoutd = obj.Hotels.CheckOut;
        double dc = 0.0;
        try
        { dc = (Convert.ToDateTime(checkoutd.ToString()) - Convert.ToDateTime(checkind.ToString())).TotalDays; }
        catch { }


        if (sortBy == "low_price")
            obj.Hotels.Hotel = obj.Hotels.Hotel.OrderBy(k => (Convert.ToDouble(k.MinRate) / (double)dc)).ToList();
        else if (sortBy == "high_price")
            obj.Hotels.Hotel = obj.Hotels.Hotel.OrderByDescending(k => (Convert.ToDouble(Convert.ToDouble(k.MinRate) / dc))).ToList();
        else if (sortBy == "name")
            obj.Hotels.Hotel = obj.Hotels.Hotel.OrderBy(k => k.Name).ToList();
        else if (sortBy == "star_rating")

            obj.Hotels.Hotel = obj.Hotels.Hotel.OrderByDescending(k => k.Reviews.Review.Rate).ToList();
        else if (sortBy == "most_popular")
        {
            string reviewrate = "0.00";
            obj.Hotels.Hotel = obj.Hotels.Hotel.OrderByDescending(k => Convert.ToDouble((k.Reviews != null) ? (Convert.ToDouble(k.Reviews.Review.Rate).ToString("0.00")) : reviewrate)).ThenBy(s => Convert.ToDouble(Convert.ToDouble(s.MinRate) / (Double)dc)).ToList();
        }
        return obj;
    }

    public static string GetFacilityList(List<Hdbfacilities> lstHdbfacilities)
    {
        string facilitiesList = string.Empty;
        string[] arrFacilities = new string[] { "Restaurant", "Breakfast", "Fitness", "Car park", "Pets", "Smoking Room", "Smoking rooms", "Wi-fi", "Gym", "Outdoor swimming pool", "Indoor swimming pool", "Wired Internet", "Bathroom", "Bar", "DryClean", "supermarket", "alarm clock", "snacks", "pub", "auditorium", "BeachFront", "dining", "EcoCertified", "ExecutiveFloors", "FamilyPlan", "FreeLocalCalls", "FreeShuttle","Room service", "table tennis", "Tennis", "vacation resort", "volleyball" };
                                                
        int i = 1;
        foreach (var item in arrFacilities)
        {
           

            string facilityDesc = lstHdbfacilities.Where(k => k.FacilityDesc.Contains(item)).Count() > 0 ?
                lstHdbfacilities.Where(k => k.FacilityDesc.Contains(item)).Select(J => J.FacilityDesc).FirstOrDefault().ToString() : string.Empty;

            if (!string.IsNullOrEmpty(facilityDesc))
            {

                facilitiesList += "<li>" + "<img src='/tpx2/Images/amenities/" + item + ".svg' class='img-responsive' title='" + facilityDesc + "' /></li>";
                if (i == 7)
                    break;

            }
            i++;

        }
        return facilitiesList;
    }

    public static List<HotelDevMTWebapi.Models.Hotel> GetStarFilter(string stars, List<HotelDevMTWebapi.Models.Hotel> lsthotel)
    {
        List<HotelDevMTWebapi.Models.Hotel> lstFilterList = new List<HotelDevMTWebapi.Models.Hotel>();
        List<Reviews> reviewlist = new List<Reviews>();
        //reviewlist = lsthotel.Select(r => r.Reviews).ToList();
        try
        {
            for (int i = 0; i < reviewlist.Count; i++)
            {
                if (reviewlist[i] != null)
                {
                    string[] arrStars = stars.Split('-');
                    if (arrStars != null)
                    {
                        foreach (var item in arrStars)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                if (lsthotel.Where(k => Convert.ToInt32(k.Reviews.Review.Rate) == Convert.ToInt32(item)) != null)
                                    lstFilterList.AddRange(lsthotel.Where(k => Convert.ToInt32(Convert.ToDouble(k.Reviews.Review.Rate)) == Convert.ToInt32(item)));
                            }

                        }
                    }
                    else
                    {

                    }
                }
            }

        }
        catch (Exception)
        {

            throw;
        }
        return lstFilterList;
    }
    //protected string checkimg(string bofid, string agcyname)
    //{
    //    HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create("http://" + WebConfigurationManager.AppSettings["logoprefix"] + ".tripsolver.net/" + agcyname + "/images/agency/bof_" + bofid + "logo.png");
    //    HttpWebResponse httpRes = null;
    //    string result = "";
    //    try
    //    {
    //        httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
    //        if (httpRes.StatusCode == HttpStatusCode.NotFound)
    //        {

    //        }
    //        else
    //        {

    //            result = "found";
    //        }
    //    }

    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //    finally
    //    {
    //        // Close the response.
    //        httpRes.Close();
    //    }
    //    return result;
    //}

    public static string checkimg(string slnk)
    {
        HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create("http://photos.hotelbeds.com/giata/" + slnk);
        HttpWebResponse httpRes = null;
        string result = "";
        try
        {
            httpRes = (HttpWebResponse)httpReq.GetResponse(); // Error 404 right here,
            if (httpRes.StatusCode == HttpStatusCode.NotFound)
            {

            }
            else
            {

                result = "found";
            }
        }

        catch
        {
            result = "not found";
        }
        //finally
        //{
        //    // Close the response.
        //    httpRes.Close();
        //}

        return result;
    }
}

