using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HotelDevMTWebapi.Controllers
{
    public class TestHotelController : ApiController
    {
        // GET api/testhotel
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/testhotel/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/testhotel
        public void Post([FromBody]string value)
        {
        }

        // PUT api/testhotel/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/testhotel/5
        public void Delete(int id)
        {
        }
    }
}
