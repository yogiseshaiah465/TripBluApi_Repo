using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace HotelDevMTWebapi.Models
{
    public class RatecommentsAj
    {

        public string Ratecommentdescription = "";

        public RatecommentsAj(string searchid, string ratecommentid)
        {
            string result = string.Empty;
            string[] ratecommentidlst;
          
           
                string HRoomRatecomBodyRS = GetHotelResBodyRQ(searchid, ratecommentid);
                result = XMLRead.GetResponse(HRoomRatecomBodyRS);
                RateCommentdto objRateCommentdto = new RateCommentdto();
                ratecommentidlst = ratecommentid.Split('|');

                
                XMLRead.SaveJSONTextFile(HRoomRatecomBodyRS, result, searchid + "_" + ratecommentidlst[1] + "_RoomRatecomment");
                string filePathRQ = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelRRatecomments/" + searchid + "_" + ratecommentidlst[1] + "_RoomRatecomment" + "-RS.json");

                using (StreamReader r = new StreamReader(filePathRQ))
                {
                    string json = r.ReadToEnd();
                    objRateCommentdto = JsonConvert.DeserializeObject<RateCommentdto>(json);
                }
                if (objRateCommentdto.rateComments.Count > 0)
                {
                    
                    Ratecommentdescription = objRateCommentdto.rateComments[0].description;
                }
               
        }

        private string GetHotelResBodyRQ(string searchid, string ratecommentid)
        {


            string resourceUri = "";

            string response = string.Empty;
            string date = DateTime.Today.ToString("yyyy-MM-dd");
       
            resourceUri = "/hotel-content-api/1.0/types/ratecommentdetails?code=235|41556|3&fields=all&language=ENG&useSecondaryLanguage=True&date=" + date;

            return resourceUri;
        }

    }
}