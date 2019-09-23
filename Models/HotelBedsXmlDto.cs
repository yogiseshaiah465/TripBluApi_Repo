using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace HotelDevMTWebapi.Models
{
    [XmlRoot(ElementName = "auditData", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class AuditData
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

    [XmlRoot(ElementName = "cancellationPolicy", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class CancellationPolicy
    {
        [XmlAttribute(AttributeName = "amount")]
        public string Amount { get; set; }
        [XmlAttribute(AttributeName = "from")]
        public string From { get; set; }
    }

    [XmlRoot(ElementName = "cancellationPolicies", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class CancellationPolicies
    {
        [XmlElement(ElementName = "cancellationPolicy", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public CancellationPolicy CancellationPolicy { get; set; }
    }

    [XmlRoot(ElementName = "tax", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Tax
    {
        [XmlAttribute(AttributeName = "included")]
        public string Included { get; set; }
        [XmlAttribute(AttributeName = "amount")]
        public string Amount { get; set; }
        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
        [XmlAttribute(AttributeName = "clientAmount")]
        public string ClientAmount { get; set; }
        [XmlAttribute(AttributeName = "clientCurrency")]
        public string ClientCurrency { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string type { get; set; }
    }

    [XmlRoot(ElementName = "taxes", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Taxes
    {
        [XmlElement(ElementName = "tax", Namespace = "http://www.hotelbeds.com/schemas/messages")]
       // public List<Tax> Tax { get; set; }
        public Tax Tax { get; set; }
        [XmlAttribute(AttributeName = "allIncluded")]
        public string AllIncluded { get; set; }
    }

    [XmlRoot(ElementName = "rate", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Rate
    {
        [XmlElement(ElementName = "cancellationPolicies", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public CancellationPolicies CancellationPolicies { get; set; }
        [XmlElement(ElementName = "taxes", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Taxes Taxes { get; set; }
        [XmlAttribute(AttributeName = "rateKey")]
        public string RateKey { get; set; }
        [XmlAttribute(AttributeName = "rateClass")]
        public string RateClass { get; set; }
        [XmlAttribute(AttributeName = "rateType")]
        public string RateType { get; set; }
        [XmlAttribute(AttributeName = "net")]
        public string Net { get; set; }
        [XmlAttribute(AttributeName = "allotment")]
        public string Allotment { get; set; }
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
        [XmlAttribute(AttributeName = "childrenAges")]
        public string ChildrenAges { get; set; }
        [XmlAttribute(AttributeName = "rateCommentsId")]
        public string RateCommentsId { get; set; }
        [XmlAttribute(AttributeName = "sellingRate")]
        public string SellingRate { get; set; }
        [XmlAttribute(AttributeName = "hotelMandatory")]
        public string HotelMandatory { get; set; }
        [XmlElement(ElementName = "offers", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Offers Offers { get; set; }
    }

    [XmlRoot(ElementName = "rates", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Rates
    {
        [XmlElement(ElementName = "rate", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public List<Rate> Rate { get; set; }
    }

    [XmlRoot(ElementName = "room", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Room
    {
        [XmlElement(ElementName = "rates", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Rates Rates { get; set; }
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
    }

    [XmlRoot(ElementName = "rooms", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Rooms
    {
        [XmlElement(ElementName = "room", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public List<Room> Room { get; set; }
    }
    [XmlRoot(ElementName = "upsellings", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class upselling
    {
        [XmlElement(ElementName = "room", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public List<Room> Room { get; set; }
    }

    [XmlRoot(ElementName = "hotel", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Hotel
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "description", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string Description { get; set; }
        [XmlElement(ElementName = "zoneCode", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string ZoneCode { get; set; }
        [XmlElement(ElementName = "coordinates", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Coordinates Coordinates { get; set; }
        [XmlElement(ElementName = "boardCodes", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public BoardCodes BoardCodes { get; set; }
        [XmlAttribute(AttributeName = "checkOut")]
        public string CheckOut { get; set; }
        [XmlAttribute(AttributeName = "checkIn")]
        public string CheckIn { get; set; }
        [XmlElement(ElementName = "segmentCodes", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public SegmentCodes SegmentCodes { get; set; }
        [XmlElement(ElementName = "address", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string Address { get; set; }
        [XmlElement(ElementName = "postalCode", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "city", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string City { get; set; }
        [XmlElement(ElementName = "email", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string Email { get; set; }
        [XmlElement(ElementName = "license", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string License { get; set; }
        [XmlElement(ElementName = "phones", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Phones Phones { get; set; }
        [XmlElement(ElementName = "reviews", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Reviews Reviews { get; set; }
        [XmlElement(ElementName = "rooms", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Rooms Rooms { get; set; }
        [XmlElement(ElementName = "upsellings", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public upselling upsellings { get; set; }

        [XmlElement(ElementName = "facilities", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Facilities Facilities { get; set; }
        [XmlElement(ElementName = "terminals", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Terminals Terminals { get; set; }
        [XmlElement(ElementName = "interestPoints", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public InterestPoints InterestPoints { get; set; }
        [XmlElement(ElementName = "images", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Images Images { get; set; }
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "countryCode")]
        public string CountryCode { get; set; }
        [XmlAttribute(AttributeName = "categoryCode")]
        public string CategoryCode { get; set; }
        [XmlAttribute(AttributeName = "categoryName")]
        public string CategoryName { get; set; }
        [XmlAttribute(AttributeName = "destinationCode")]
        public string DestinationCode { get; set; }
        [XmlAttribute(AttributeName = "destinationName")]
        public string DestinationName { get; set; }
        [XmlAttribute(AttributeName = "zoneName")]
        public string ZoneName { get; set; }
        [XmlAttribute(AttributeName = "latitude")]
        public string Latitude { get; set; }
        [XmlAttribute(AttributeName = "longitude")]
        public string Longitude { get; set; }
        [XmlAttribute(AttributeName = "minRate")]
        public string MinRate { get; set; }
        [XmlAttribute(AttributeName = "maxRate")]
        public string MaxRate { get; set; }
        [XmlAttribute(AttributeName = "currency")]
        public string Currency { get; set; }
        [XmlAttribute(AttributeName = "categoryGroupCode")]
        public string CategoryGroupCode { get; set; }
        [XmlAttribute(AttributeName = "chainCode")]
        public string ChainCode { get; set; }
        [XmlAttribute(AttributeName = "accommodationTypeCode")]
        public string AccommodationTypeCode { get; set; }
        [XmlAttribute(AttributeName = "web")]
        public string Web { get; set; }
        [XmlAttribute(AttributeName = "S2C")]
        public string S2C { get; set; }
    }

    [XmlRoot(ElementName = "offer", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Offer
    {
        [XmlAttribute(AttributeName = "code")]
        public string Code { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlAttribute(AttributeName = "amount")]
        public string Amount { get; set; }
    }

    [XmlRoot(ElementName = "offers", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Offers
    {
        [XmlElement(ElementName = "offer", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Offer Offer { get; set; }
    }

    [XmlRoot(ElementName = "hotels", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Hotels
    {
        [XmlElement(ElementName = "hotel", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public List<Hotel> Hotel { get; set; }
        [XmlAttribute(AttributeName = "checkIn")]
        public string CheckIn { get; set; }
        [XmlAttribute(AttributeName = "total")]
        public string Total { get; set; }
        [XmlAttribute(AttributeName = "checkOut")]
        public string CheckOut { get; set; }
    }

    [XmlRoot(ElementName = "availabilityRS", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class AvailabilityRS
    {
        [XmlElement(ElementName = "auditData", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public AuditData AuditData { get; set; }
        [XmlElement(ElementName = "hotels", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Hotels Hotels { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
        [XmlAttribute(AttributeName = "schemaLocation")]
        public string SchemaLocation { get; set; }
    }
    [XmlRoot(ElementName = "hotelsRS", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class HotelsRS
    {
        [XmlElement(ElementName = "from", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string From { get; set; }
        [XmlElement(ElementName = "to", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string To { get; set; }
        [XmlElement(ElementName = "total", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string Total { get; set; }
        [XmlElement(ElementName = "auditData", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public AuditData AuditData { get; set; }
        [XmlElement(ElementName = "hotels", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Hotels Hotels { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Xsi { get; set; }
    }

    [XmlRoot(ElementName = "review", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Review
    {
        [XmlAttribute(AttributeName = "rate")]
        public string Rate { get; set; }
        [XmlAttribute(AttributeName = "reviewCount")]
        public string ReviewCount { get; set; }
        [XmlAttribute(AttributeName = "type")]
        public string Type { get; set; }
    }

    [XmlRoot(ElementName = "reviews", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Reviews
    {
        [XmlElement(ElementName = "review", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Review Review { get; set; }
    }
    [XmlRoot(ElementName = "facility", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Facility
    {
        [XmlAttribute(AttributeName = "facilityCode")]
        public string FacilityCode { get; set; }
        [XmlAttribute(AttributeName = "facilityGroupCode")]
        public string FacilityGroupCode { get; set; }
        [XmlAttribute(AttributeName = "order")]
        public string Order { get; set; }
        [XmlAttribute(AttributeName = "indLogic")]
        public string IndLogic { get; set; }
        [XmlAttribute(AttributeName = "indFee")]
        public string IndFee { get; set; }
        [XmlAttribute(AttributeName = "indYesOrNo")]
        public string IndYesOrNo { get; set; }
        [XmlAttribute(AttributeName = "number")]
        public string Number { get; set; }
        [XmlAttribute(AttributeName = "distance")]
        public string Distance { get; set; }
    }

    [XmlRoot(ElementName = "facilities", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Facilities
    {
        [XmlElement(ElementName = "facility", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Facility Facility { get; set; }
    }
    [XmlRoot(ElementName = "terminal", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Terminal
    {
        [XmlAttribute(AttributeName = "terminalCode")]
        public string TerminalCode { get; set; }
        [XmlAttribute(AttributeName = "distance")]
        public string Distance { get; set; }
    }
    [XmlRoot(ElementName = "terminals", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Terminals
    {
        [XmlElement(ElementName = "terminal", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Terminal Terminal { get; set; }
    }

    [XmlRoot(ElementName = "interestPoint", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class InterestPoint
    {
        [XmlAttribute(AttributeName = "facilityCode")]
        public string FacilityCode { get; set; }
        [XmlAttribute(AttributeName = "facilityGroupCode")]
        public string FacilityGroupCode { get; set; }
        [XmlAttribute(AttributeName = "order")]
        public string Order { get; set; }
        [XmlAttribute(AttributeName = "poiName")]
        public string PoiName { get; set; }
        [XmlAttribute(AttributeName = "distance")]
        public string Distance { get; set; }
    }
    [XmlRoot(ElementName = "interestPoints", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class InterestPoints
    {
        [XmlElement(ElementName = "interestPoint", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public InterestPoint InterestPoint { get; set; }
    }

    [XmlRoot(ElementName = "image", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Image
    {
        [XmlAttribute(AttributeName = "imageTypeCode")]
        public string ImageTypeCode { get; set; }
        [XmlAttribute(AttributeName = "path")]
        public string Path { get; set; }
        [XmlAttribute(AttributeName = "roomCode")]
        public string RoomCode { get; set; }
        [XmlAttribute(AttributeName = "characteristicCode")]
        public string CharacteristicCode { get; set; }
        [XmlAttribute(AttributeName = "order")]
        public string Order { get; set; }
    }

    [XmlRoot(ElementName = "images", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Images
    {
        [XmlElement(ElementName = "image", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Image Image { get; set; }
    }
    [XmlRoot(ElementName = "segmentCodes", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class SegmentCodes
    {
        [XmlElement(ElementName = "segmentCode", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string SegmentCode { get; set; }
    }
    [XmlRoot(ElementName = "coordinates", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Coordinates
    {
        [XmlAttribute(AttributeName = "longitude")]
        public string Longitude { get; set; }
        [XmlAttribute(AttributeName = "latitude")]
        public string Latitude { get; set; }
    }

    [XmlRoot(ElementName = "phones", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Phones
    {
        [XmlElement(ElementName = "phone", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public Phone Phone { get; set; }
    }
    [XmlRoot(ElementName = "phone", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class Phone
    {
        [XmlElement(ElementName = "phoneNumber", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string PhoneNumber { get; set; }
        [XmlElement(ElementName = "phoneType", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string PhoneType { get; set; }
    }
    [XmlRoot(ElementName = "boardCodes", Namespace = "http://www.hotelbeds.com/schemas/messages")]
    public class BoardCodes
    {
        [XmlElement(ElementName = "boardCode", Namespace = "http://www.hotelbeds.com/schemas/messages")]
        public string BoardCode { get; set; }
    }
}