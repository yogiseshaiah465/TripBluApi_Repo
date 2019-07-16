using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelDevMTWebapi.Models
{
    public class HotelMaincintent
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string ZoneCode { get; set; }

        public Coordinates Coordinates { get; set; }

        public BoardCodes BoardCodes { get; set; }

        public SegmentCodes SegmentCodes { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Email { get; set; }

        public string License { get; set; }

        public Phones Phones { get; set; }

        public Reviews Reviews { get; set; }

        public Rooms Rooms { get; set; }

        public Facilities Facilities { get; set; }

        public Terminals Terminals { get; set; }

        public InterestPoints InterestPoints { get; set; }

        public Images Images { get; set; }

        public string Code { get; set; }

        public string CountryCode { get; set; }

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public string DestinationCode { get; set; }

        public string DestinationName { get; set; }

        public string ZoneName { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string MinRate { get; set; }

        public string MaxRate { get; set; }

        public string Currency { get; set; }

        public string CategoryGroupCode { get; set; }

        public string ChainCode { get; set; }

        public string AccommodationTypeCode { get; set; }

        public string Web { get; set; }

        public string S2C { get; set; }

    }
    public class Hdbfacilities
    {
        public List<string> lstHotelCodes { get; set; }

        public string FacilityCode { get; set; }

        public string FacilityGroupCode { get; set; }

        public string IndYesOrNo { get; set; }

        public string FacilityDesc { get; set; }


    }

    public class HdbImagepath
    {
        public string Path { get; set; }



    }
}