using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using HotelDevMTWebapi.Models;
using System.Web.Configuration;

namespace TripxolHotelsWebapi.Controllers
{
    public class HotelMediaImageController : ApiController
    {
        public string Get(string searchid, string hcode, string viewid, string b2c_idn)
        {
            string rvalue = "";

            //   ManageHDetAj mhd = new ManageHDetAj();
            // HotelPropertyAj hpr = mhd.GetData(searchid, hcode,"USD");

            HotelMedia hm = new HotelMedia(hcode, "", searchid, viewid, b2c_idn);
            rvalue = GetSlideImages(hcode);
            return rvalue;
        }
        //private string GetSlideImages(string hcode)
        //{
        //    string images = "";
        //    foreach (DataRow drh in HImages.Rows)
        //    {
        //        images += "<div class='gallery__img-block'><img src='" + drh["URL"].ToString() + "' thumb-url='" + drh["URL"].ToString() + "' class='' height=400px></div>";
        //    }
        //    return images;
        //}

        public static string checkimg(string slnk)
        {
            HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create("http://photos.hotelbeds.com/giata/bigger/" + slnk);
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
                result = " not found";
            }
            //finally
            //{
            //    // Close the response.
            //    httpRes.Close();
            //}
           
            return result;
        }


        public static string GetSlideImages(string hcode)
        {
            string images = string.Empty;
            int timg = 0;
            int i = 0;

            HdbImagepath obj = new HdbImagepath();
            DataTable dtffbookingfb = new DataTable();

            try
            {

                string cmdflbkfb = "select Path ,HotelCode from HotelImage where HotelCode='" + hcode + "' ";
                dtffbookingfb = manage_data.GetDataTable(cmdflbkfb, manage_data.flip_conhb);
                DataRow[] dr = dtffbookingfb.Select("HotelCode= '" + hcode + "'");
                timg = dtffbookingfb.Rows.Count;
               //slide old

                foreach (DataRow drh in dr)
                {
                    string res = checkimg(drh["Path"].ToString());
                    if (res == "found")
                    {

                        images += "<div class='gallery__img-block'><img height= '557px' src='http://photos.hotelbeds.com/giata/" + drh["Path"].ToString() + "' thumb-url='http://photos.hotelbeds.com/giata/" + drh["Path"].ToString() + "' class=''></div>";
                    }
                }
               
                return images;



            }
            catch
            {

            }


            return images;
        
        }



        //private string GetImages(DataTable HImages)
        //{
        //    string images = "";
        //    int timg = HImages.Rows.Count;
        //    int i = 0;
        //    foreach (DataRow drh in HImages.Rows)
        //    {
        //        i++;
        //        images += "<div class='mySlides fade'> <div class='numbertext'>" + i + "/" + timg + "</div>  <img src='" + drh["URL"].ToString() + "' width= '600px' height= '225px'>  <div class='text'>Caption Text</div></div>";
        //    }
        //    return images;
        //}



        public static string GetImages(string hotelcode)
        {
            string images = string.Empty;
            int timg = 0;
            int i = 0;

            HdbImagepath obj = new HdbImagepath();
            DataTable dtffbookingfb=new DataTable();
            
            try
            {

                string cmdflbkfb = "select Path from HotelImage where HotelCode='" + hotelcode + "' ";
                 dtffbookingfb = manage_data.GetDataTable(cmdflbkfb, manage_data.flip_conhb);
                 DataRow[] dr = dtffbookingfb.Select("HotelCode= '" + hotelcode + "'");
                 timg = dtffbookingfb.Rows.Count;
                 //foreach (DataRow drh in dr)
                 //{
                 //    i++;
                 //    imagepath += "<div class='mySlides fade'> <div class='numbertext'>" + i + "/" + timg + "</div>  <img src='http://photos.hotelbeds.com/giata/" + drh["Path"].ToString() + "' width= '600px' height= '225px'>  <div class='text'>Caption Text</div></div>";
                 //}




                 foreach (DataRow drh in dr)
                 {
                     i++;
                     images += "<div class='mySlides fade'> <div class='numbertext'>" + i + "/" + timg + "</div>  <img  src='http://photos.hotelbeds.com/giata/" + drh["Path"].ToString() + "' width= '600px' height= '225px'>  <div class='text'>Caption Text</div></div>";
                 }
                 
               
            }
            catch
            {

            }


            return images;
        }



    }
}