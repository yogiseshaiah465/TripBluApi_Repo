using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.IO;
using System.Web;

namespace TripxolHotelsWebapi.Controllers
{
    public class ImageUrlCollController : ApiController
    {
        string ContextResult = "";
        public List<HotelImageJsonCode> Get(string searchid, string HotelCode, string curcode, string b2c_idn)
        {
            List<HotelImageJsonCode> HotelImages = new List<HotelImageJsonCode>();
            try
            {
                if (searchid != "" && searchid != null && HotelCode != "" && HotelCode != null)
                {
                    string[] HotelCodes = HotelCode.Trim('*').Split('*');
                    foreach (string hcode in HotelCodes)
                    {
                        HotelImageJsonCode hijc = new HotelImageJsonCode();
                        DataTable dtHotelUrl = HotelDBLayer.GetHotelImageUrl(hcode);
                        if (dtHotelUrl.Rows.Count > 0)
                        {
                            hijc.Image = dtHotelUrl.Rows[0]["ImageUrl"].ToString();
                            hijc.Logo = dtHotelUrl.Rows[0]["Logourl"].ToString();
                            hijc.Hotelcode = hcode;
                            HotelImages.Add(hijc);
                        }
                        else
                        {
                            string filePathContext = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_" + curcode + "_ImageContextChange-RS.xml");
                            if (File.Exists(filePathContext))
                            {
                                ContextResult = File.ReadAllText(filePathContext);
                            }
                            else
                            {
                                ContextResult = XMLRead.ContextChange(searchid + "Image");
                                try
                                {
                                    File.WriteAllText(filePathContext, ContextResult);
                                }
                                catch
                                {
                                }
                            }
                            HotelImageAj hotelimage = new HotelImageAj(HotelCode, "", searchid, ContextResult);
                            string image = hotelimage.Image;
                            string logo = hotelimage.logo;
                            if (image != "N")
                            {
                                HotelDBLayer.SaveHotelImageUrl(hcode, image, logo);
                            }

                            hijc.Hotelcode = hcode;
                            if (image != "N")
                            {
                                hijc.Image = image;
                            }
                            else
                            {
                                hijc.Image = "../images/No Image found.png";
                            }
                            if (logo != "N")
                            {
                                hijc.Logo = logo;
                            }
                            else
                            {
                                hijc.Logo = "../images/No Image found.png";
                            }
                            HotelImages.Add(hijc);
                        }
                    }

                    //closing the session
                    if (ContextResult.ToString() != "")
                    {
                        DataSet ds = new DataSet();
                        DataSet dsSession = new DataSet();
                        StringReader se_stream = new StringReader(ContextResult);
                        dsSession.ReadXml(se_stream);
                        string Rq = "";

                        if (dsSession.Tables["BinarySecurityToken"] != null)
                        {
                            DataTable dtBinarySecurityToken = dsSession.Tables["BinarySecurityToken"];
                            DataTable dtMessageData = dsSession.Tables["MessageData"];
                            DataTable dtMessageHeader = dsSession.Tables["MessageHeader"];
                            string timestamp = DateTime.UtcNow.ToString();
                            string cresult = XMLRead.closession(dtMessageData.Rows[0]["MessageId"].ToString(), timestamp, dtBinarySecurityToken.Rows[0]["BinarySecurityToken_Text"].ToString(), searchid + "Image", XMLRead.pcc, XMLRead.ipcc);
                        }
                    }
                }
            }
            catch
            {
            }
            return HotelImages;

        }
    }
}