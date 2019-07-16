using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotelDevMTWebapi
{
    public class Autocom
    {
        public class Rootobject
        {
            public Response Response { get; set; }
            public Link[] Links { get; set; }
        }

        public class Response
        {
            public Responseheader responseHeader { get; set; }
            public Grouped grouped { get; set; }
        }

        public class Responseheader
        {
            public int status { get; set; }
            public int QTime { get; set; }
        }

        public class Grouped
        {
            public CategoryAIR categoryAIR { get; set; }
            public CategoryCITY categoryCITY { get; set; }
            public CategoryRail categoryRail { get; set; }
            public CategoryPOI categoryPOI { get; set; }
        }

        public class CategoryAIR
        {
            public int matches { get; set; }
            public Doclist doclist { get; set; }
        }

        public class Doclist
        {
            public int numFound { get; set; }
            public int start { get; set; }
            public Doc[] docs { get; set; }
        }

        public class Doc
        {
            public string name { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string countryName { get; set; }
            public string stateName { get; set; }
            public string state { get; set; }
            public string category { get; set; }
            public string id { get; set; }
            public string dataset { get; set; }
            public string datasource { get; set; }
            public string confidenceFactor { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public string iataCityCode { get; set; }
            public int ranking { get; set; }
        }

        public class CategoryCITY
        {
            public int matches { get; set; }
            public Doclist1 doclist { get; set; }
        }

        public class Doclist1
        {
            public int numFound { get; set; }
            public int start { get; set; }
            public Doc1[] docs { get; set; }
        }

        public class Doc1
        {
            public string name { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string countryName { get; set; }
            public string stateName { get; set; }
            public string state { get; set; }
            public string category { get; set; }
            public string id { get; set; }
            public string dataset { get; set; }
            public string datasource { get; set; }
            public string confidenceFactor { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public int ranking { get; set; }
        }

        public class CategoryRail
        {
            public int matches { get; set; }
            public Doclist2 doclist { get; set; }
        }

        public class Doclist2
        {
            public int numFound { get; set; }
            public int start { get; set; }
            public Doc2[] docs { get; set; }
        }

        public class Doc2
        {
            public string name { get; set; }
            public string category { get; set; }
            public string id { get; set; }
            public string dataset { get; set; }
            public string datasource { get; set; }
            public string confidenceFactor { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public int ranking { get; set; }
            public string stateName { get; set; }
            public string state { get; set; }
            public string description { get; set; }
            public string city { get; set; }
        }

        public class CategoryPOI
        {
            public int matches { get; set; }
            public Doclist3 doclist { get; set; }
        }

        public class Doclist3
        {
            public int numFound { get; set; }
            public int start { get; set; }
            public Doc3[] docs { get; set; }
        }

        public class Doc3
        {
            public string name { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string countryName { get; set; }
            public string stateName { get; set; }
            public string state { get; set; }
            public string category { get; set; }
            public string dataset { get; set; }
            public string datasource { get; set; }
            public string confidenceFactor { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
            public int ranking { get; set; }
            public string id { get; set; }
        }

        public class Link
        {
            public string rel { get; set; }
            public string href { get; set; }
        }
    }
}

