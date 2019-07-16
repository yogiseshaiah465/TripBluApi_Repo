using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace HotelDevMTWebapi.Models
{
    //[XmlRoot(ElementName = "auditData", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //public class AuditData
    //{
    //    [XmlAttribute(AttributeName = "processTime")]
    //    public string ProcessTime { get; set; }
    //    [XmlAttribute(AttributeName = "timestamp")]
    //    public string Timestamp { get; set; }
    //    [XmlAttribute(AttributeName = "requestHost")]
    //    public string RequestHost { get; set; }
    //    [XmlAttribute(AttributeName = "serverId")]
    //    public string ServerId { get; set; }
    //    [XmlAttribute(AttributeName = "environment")]
    //    public string Environment { get; set; }
    //    [XmlAttribute(AttributeName = "release")]
    //    public string Release { get; set; }
    //    [XmlAttribute(AttributeName = "token")]
    //    public string Token { get; set; }
    //    [XmlAttribute(AttributeName = "internal")]
    //    public string Internal { get; set; }
    //}

    //[XmlRoot(ElementName = "cancellationPolicy", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //public class CancellationPolicy
    //{
    //    [XmlAttribute(AttributeName = "amount")]
    //    public string Amount { get; set; }
    //    [XmlAttribute(AttributeName = "from")]
    //    public string From { get; set; }
    //}

    //[XmlRoot(ElementName = "cancellationPolicies", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //public class CancellationPolicies
    //{
    //    [XmlElement(ElementName = "cancellationPolicy", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //    public CancellationPolicy CancellationPolicy { get; set; }
    //}

    //[XmlRoot(ElementName = "tax", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //public class Tax
    //{
    //    [XmlAttribute(AttributeName = "included")]
    //    public string Included { get; set; }
    //    [XmlAttribute(AttributeName = "amount")]
    //    public string Amount { get; set; }
    //    [XmlAttribute(AttributeName = "currency")]
    //    public string Currency { get; set; }
    //    [XmlAttribute(AttributeName = "clientAmount")]
    //    public string ClientAmount { get; set; }
    //    [XmlAttribute(AttributeName = "clientCurrency")]
    //    public string ClientCurrency { get; set; }
    //}

    //[XmlRoot(ElementName = "taxes", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //public class Taxes
    //{
    //    [XmlElement(ElementName = "tax", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //    public Tax Tax { get; set; }
    //    [XmlAttribute(AttributeName = "allIncluded")]
    //    public string AllIncluded { get; set; }
    //}

    //[XmlRoot(ElementName = "rate", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //public class Rate
    //{
    //    [XmlElement(ElementName = "cancellationPolicies", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //    public CancellationPolicies CancellationPolicies { get; set; }
    //    [XmlElement(ElementName = "taxes", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //    public Taxes Taxes { get; set; }
    //    [XmlAttribute(AttributeName = "rateKey")]
    //    public string RateKey { get; set; }
    //    [XmlAttribute(AttributeName = "rateClass")]
    //    public string RateClass { get; set; }
    //    [XmlAttribute(AttributeName = "rateType")]
    //    public string RateType { get; set; }
    //    [XmlAttribute(AttributeName = "net")]
    //    public string Net { get; set; }
    //    [XmlAttribute(AttributeName = "rateComments")]
    //    public string RateComments { get; set; }
    //    [XmlAttribute(AttributeName = "paymentType")]
    //    public string PaymentType { get; set; }
    //    [XmlAttribute(AttributeName = "packaging")]
    //    public string Packaging { get; set; }
    //    [XmlAttribute(AttributeName = "boardCode")]
    //    public string BoardCode { get; set; }
    //    [XmlAttribute(AttributeName = "boardName")]
    //    public string BoardName { get; set; }
    //    [XmlAttribute(AttributeName = "rooms")]
    //    public string Rooms { get; set; }
    //    [XmlAttribute(AttributeName = "adults")]
    //    public string Adults { get; set; }
    //    [XmlAttribute(AttributeName = "children")]
    //    public string Children { get; set; }
    //    [XmlAttribute(AttributeName = "allotment")]
    //    public string Allotment { get; set; }
    //    [XmlAttribute(AttributeName = "rateCommentsId")]
    //    public string RateCommentsId { get; set; }
    //    [XmlAttribute(AttributeName = "rateup")]
    //    public string Rateup { get; set; }
    //}

    //[XmlRoot(ElementName = "rates", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //public class Rates
    //{
    //    [XmlElement(ElementName = "rate", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //    public List<Rate> Rate { get; set; }
    //}

    //[XmlRoot(ElementName = "room", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //public class Room
    //{
    //    [XmlElement(ElementName = "rates", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //    public Rates Rates { get; set; }
    //    [XmlAttribute(AttributeName = "code")]
    //    public string Code { get; set; }
    //    [XmlAttribute(AttributeName = "name")]
    //    public string Name { get; set; }
    //}

    //[XmlRoot(ElementName = "rooms", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //public class Rooms
    //{
    //    [XmlElement(ElementName = "room", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //    public Room Room { get; set; }
    //}

    //[XmlRoot(ElementName = "upselling", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //public class Upselling
    //{
    //    [XmlElement(ElementName = "rooms", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //    public Rooms Rooms { get; set; }
    //}

    //[XmlRoot(ElementName = "hotel", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //public class Hotel
    //{
    //    [XmlElement(ElementName = "rooms", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //    public Rooms Rooms { get; set; }
    //    [XmlElement(ElementName = "upselling", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    //    public Upselling Upselling { get; set; }
    //    [XmlAttribute(AttributeName = "checkOut")]
    //    public string CheckOut { get; set; }
    //    [XmlAttribute(AttributeName = "checkIn")]
    //    public string CheckIn { get; set; }
    //    [XmlAttribute(AttributeName = "code")]
    //    public string Code { get; set; }
    //    [XmlAttribute(AttributeName = "name")]
    //    public string Name { get; set; }
    //    [XmlAttribute(AttributeName = "categoryCode")]
    //    public string CategoryCode { get; set; }
    //    [XmlAttribute(AttributeName = "categoryName")]
    //    public string CategoryName { get; set; }
    //    [XmlAttribute(AttributeName = "destinationCode")]
    //    public string DestinationCode { get; set; }
    //    [XmlAttribute(AttributeName = "destinationName")]
    //    public string DestinationName { get; set; }
    //    [XmlAttribute(AttributeName = "zoneCode")]
    //    public string ZoneCode { get; set; }
    //    [XmlAttribute(AttributeName = "zoneName")]
    //    public string ZoneName { get; set; }
    //    [XmlAttribute(AttributeName = "latitude")]
    //    public string Latitude { get; set; }
    //    [XmlAttribute(AttributeName = "longitude")]
    //    public string Longitude { get; set; }
    //    [XmlAttribute(AttributeName = "totalNet")]
    //    public string TotalNet { get; set; }
    //    [XmlAttribute(AttributeName = "currency")]
    //    public string Currency { get; set; }
    //}
    [XmlRoot(ElementName = "error", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Error
    {
        [XmlElement(ElementName = "code", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string Code { get; set; }
        [XmlElement(ElementName = "message", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string Message { get; set; }
    }
    [XmlRoot(ElementName = "checkRateRS", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class CheckRateRS
    {
        [XmlElement(ElementName = "auditData", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public AuditData AuditData { get; set; }
        [XmlElement(ElementName = "hotel", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Hotel Hotel { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation")]
        public string SchemaLocation { get; set; }
        [XmlElement(ElementName = "error", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Error Error { get; set; }
    }
}