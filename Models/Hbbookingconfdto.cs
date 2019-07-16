using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace HotelDevMTWebapi.Models
{
    public class Hbbookingconfdto
    {
    }

    [XmlRoot(ElementName = "auditData", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class AuditData_bcf
    {
        [XmlAttribute(AttributeName = "processTime")]
        public string ProcessTime { get; set; }
        [XmlAttribute(AttributeName = "timestamp")]
        public string Timestamp { get; set; }
        [XmlAttribute(AttributeName = "requestHost")]
        public string RequestHost { get; set; }
        [XmlAttribute(AttributeName = "serverId")]
        public string ServerId { get; set; }
        [XmlAttribute(AttributeName = "environment")]
        public string Environment { get; set; }
        [XmlAttribute(AttributeName = "release")]
        public string Release { get; set; }
        [XmlAttribute(AttributeName = "token")]
        public string Token { get; set; }
        [XmlAttribute(AttributeName = "internal")]
        public string Internal { get; set; }
    }

    [XmlRoot(ElementName = "modificationPolicies", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class ModificationPolicies
    {
        [XmlAttribute(AttributeName = "cancellation")]
        public string Cancellation { get; set; }
        [XmlAttribute(AttributeName = "modification")]
        public string Modification { get; set; }
    }

    [XmlRoot(ElementName = "holder", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Holder
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "surname")]
        public string Surname { get; set; }
    }

    [XmlRoot(ElementName = "pax", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Pax
    {
        [XmlAttribute(AttributeName = "roomId")]
        public string RoomId { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
        [XmlAttribute(AttributeName = "age")]
        public string Age { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "surname")]
        public string Surname { get; set; }
    }

    [XmlRoot(ElementName = "paxes", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Paxes
    {
        [XmlElement(ElementName = "pax", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public List<Pax> Pax { get; set; }
    }

    [XmlRoot(ElementName = "cancellationPolicy", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class CancellationPolicy_bcf
    {
        [XmlAttribute(AttributeName = "amount")]
        public string Amount { get; set; }
        [XmlAttribute(AttributeName = "from")]
        public string From { get; set; }
    }

    [XmlRoot(ElementName = "cancellationPolicies", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class CancellationPolicies_bcfg
    {
        [XmlElement(ElementName = "cancellationPolicy", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public CancellationPolicy_bcf CancellationPolicy { get; set; }
    }

    [XmlRoot(ElementName = "rate", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Rate_bcf
    {
        [XmlElement(ElementName = "cancellationPolicies", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public CancellationPolicies_bcfg CancellationPolicies { get; set; }
        [XmlAttribute(AttributeName = "rateClass")]
        public string RateClass { get; set; }
        [XmlAttribute(AttributeName = "net")]
        public string Net { get; set; }
        [XmlAttribute(AttributeName = "rateComments")]
        public string RateComments { get; set; }
        [XmlAttribute(AttributeName = "paymentType")]
        public string PaymentType { get; set; }
        [XmlAttribute(AttributeName = "packaging")]
        public string Packaging { get; set; }
        [XmlAttribute(AttributeName = "boardCode")]
        public string BoardCode { get; set; }
        [XmlAttribute(AttributeName = "boardName")]
        public string BoardName { get; set; }
        [XmlAttribute(AttributeName = "rooms")]
        public string Rooms { get; set; }
        [XmlAttribute(AttributeName = "adults")]
        public string Adults { get; set; }
        [XmlAttribute(AttributeName = "children")]
        public string Children { get; set; }
    }

    [XmlRoot(ElementName = "rates", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Rates_bcf
    {
        [XmlElement(ElementName = "rate", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Rate_bcf Rate { get; set; }
    }

    [XmlRoot(ElementName = "room", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Room_bcf
    {
        [XmlElement(ElementName = "paxes", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Paxes Paxes { get; set; }
        [XmlElement(ElementName = "rates", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Rates_bcf Rates { get; set; }
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "rooms", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Rooms_bcf
    {
        [XmlElement(ElementName = "room", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public List<Room_bcf> Room { get; set; }
    }

    [XmlRoot(ElementName = "supplier", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Supplier
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "vatNumber")]
        public string VatNumber { get; set; }
    }

    [XmlRoot(ElementName = "hotel", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Hotel_bcf
    {
        [XmlElement(ElementName = "rooms", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Rooms_bcf Rooms { get; set; }
        [XmlElement(ElementName = "supplier", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Supplier Supplier { get; set; }
        [XmlAttribute(AttributeName = "checkOut")]
        public string CheckOut { get; set; }
        [XmlAttribute(AttributeName = "checkIn")]
        public string CheckIn { get; set; }
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "categoryCode")]
        public string CategoryCode { get; set; }
        [XmlAttribute(AttributeName = "categoryName")]
        public string CategoryName { get; set; }
        [XmlAttribute(AttributeName = "destinationCode")]
        public string DestinationCode { get; set; }
        [XmlAttribute(AttributeName = "destinationName")]
        public string DestinationName { get; set; }
        [XmlAttribute(AttributeName = "zoneCode")]
        public string ZoneCode { get; set; }
        [XmlAttribute(AttributeName = "zoneName")]
        public string ZoneName { get; set; }
        [XmlAttribute(AttributeName = "latitude")]
        public string Latitude { get; set; }
        [XmlAttribute(AttributeName = "longitude")]
        public string Longitude { get; set; }
        [XmlAttribute(AttributeName = "totalNet")]
        public string TotalNet { get; set; }
        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
    }

    [XmlRoot(ElementName = "invoiceCompany", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class InvoiceCompany
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "registrationNumber")]
        public string RegistrationNumber { get; set; }
    }

    [XmlRoot(ElementName = "booking", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Booking
    {
        [XmlElement(ElementName = "modificationPolicies", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public ModificationPolicies ModificationPolicies { get; set; }
        [XmlElement(ElementName = "holder", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Holder Holder { get; set; }
        [XmlElement(ElementName = "hotel", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Hotel_bcf Hotel { get; set; }
        [XmlElement(ElementName = "remark", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string Remark { get; set; }
        [XmlElement(ElementName = "invoiceCompany", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public InvoiceCompany InvoiceCompany { get; set; }
        [XmlAttribute(AttributeName = "reference")]
        public string Reference { get; set; }
        [XmlAttribute(AttributeName = "clientReference")]
        public string ClientReference { get; set; }
        [XmlAttribute(AttributeName = "creationDate")]
        public string CreationDate { get; set; }
        [XmlAttribute(AttributeName = "status")]
        public string Status { get; set; }
        [XmlAttribute(AttributeName = "creationUser")]
        public string CreationUser { get; set; }
        [XmlAttribute(AttributeName = "totalNet")]
        public string TotalNet { get; set; }
        [XmlAttribute(AttributeName = "pendingAmount")]
        public string PendingAmount { get; set; }
        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
    }

    [XmlRoot(ElementName = "bookingRS", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class BookingRS
    {
        [XmlElement(ElementName = "auditData", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public AuditData_bcf AuditData { get; set; }
        [XmlElement(ElementName = "booking", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Booking Booking { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation")]
        public string SchemaLocation { get; set; }
    }

}