using HotelDevMTWebapi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelDevMTWebapi.Controllers
{
    public class RatecommentsController : Controller
    {
        [HttpGet]
        public string Get(string searchid, string Ratecommentdt)
        {


            string rvalue = "test";


            RatecommentsAj RatecommentsAjdata = new RatecommentsAj(searchid, Ratecommentdt);
            string Rdescription = RatecommentsAjdata.Ratecommentdescription;
            if (Rdescription != "")
            {
                rvalue = Rdescription;
            }
            else
            {
                rvalue = "";
            }
            return rvalue;
        }
    }
}
