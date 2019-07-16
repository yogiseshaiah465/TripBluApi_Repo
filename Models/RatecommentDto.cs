using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelDevMTWebapi.Models
{
    public class AuditData_Ratecom
    {
        public string processTime { get; set; }
        public string timestamp { get; set; }
        public string requestHost { get; set; }
        public string serverId { get; set; }
        public string environment { get; set; }
        public string release { get; set; }
    }

    public class RateComment
    {
        public string dateEnd { get; set; }
        public string dateStart { get; set; }
        public string description { get; set; }
    }

    public class RateCommentdto
    {
        public AuditData_Ratecom auditData { get; set; }
        public string date { get; set; }
        public int incoming { get; set; }
        public int hotel { get; set; }
        public List<RateComment> rateComments { get; set; }
        public string code { get; set; }
    }
}