using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Xml;

namespace TripxolHotelsWebapi.Controllers
{
    public class HotelMapMarkersController : ApiController
    {
        public DataTable Get(string searchid, string curcode)
        {
            //string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelAvail/" + searchid + "_HotelList.xml");
            string HLFPath = Path.Combine(HttpRuntime.AppDomainAppPath, "HotelXML/" + searchid + "_" + curcode+"_hotelsAvail-RS.xml");//_HotelList.xml
            DataTable dtmarker = new DataTable();
            if (File.Exists(HLFPath))
            {
                // string HLxml = File.ReadAllText(HLFPath);
                XmlDataDocument xmldoc = new XmlDataDocument();
                FileStream fs = new FileStream(HLFPath, FileMode.Open, FileAccess.Read);
                xmldoc.Load(fs);
                fs.Close();
                XmlNode xnod = xmldoc.DocumentElement;
                dtmarker = FillHStable(xnod);
            }
            return dtmarker;
        }
        private DataTable FillHStable(XmlNode xnod)
        {
            DataTable dtmarker1 = new DataTable();
            dtmarker1.Columns.Add("Latitude");
            dtmarker1.Columns.Add("Longitude");
            dtmarker1.Columns.Add("HotelName");
            foreach (XmlNode xn in xnod.ChildNodes)
            {
                DataRow dtr = dtmarker1.NewRow();
                dtr["Latitude"] = GetChildText(xn, "hotel","latitude");
                dtr["Longitude"] = GetChildText(xn, "hotel","longitude");
                dtr["HotelName"] = GetChildText(xn, "hotel","name");
                dtmarker1.Rows.Add(dtr);
            }
            return dtmarker1;
        }
        private string GetChildText(XmlNode pnode,string Hnode,string node)
        {
            string rvalue = "";
            try
            {

                foreach (XmlNode xn in pnode.ChildNodes)
                {
                    if (xn.Name.ToLower() == Hnode.ToLower())
                    {
                        if (xn.HasChildNodes)
                        {
                            foreach (XmlNode item in pnode.ChildNodes)
                            {
                               
                                    rvalue = xn.Attributes[node].Value;
                                   // rvalue = item.InnerText;
                                
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return rvalue;
        }
    }
}