using System;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Spatial;
using Newtonsoft.Json;

namespace DemoAzureSearchWOD.Models
{
    [SerializePropertyNamesAsCamelCase]
    public partial class Hotel
    {


        [System.ComponentModel.DataAnnotations.Key]
        public string listingId { get; set; }
        public string description { get; set; }
        public string description_es { get; set; }
        public string number { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string countryCode { get; set; }
        public string postCode { get; set; }
        public string thumbnail { get; set; }
        public string status { get; set; }
        public string[] tags { get; set; }
        
    }

        //[SerializePropertyNamesAsCamelCase]
        //public partial class Hotel
        //{
        //    [System.ComponentModel.DataAnnotations.Key]
        //    [IsFilterable]
        //    public string HotelId { get; set; }

        //    [IsFilterable, IsSortable, IsFacetable]
        //    public double? BaseRate { get; set; }

        //    [IsSearchable]
        //    public string Description { get; set; }

        //    //[IsSearchable]
        //    //[Analyzer(AnalyzerName.AsString.FrLucene)]
        //    //[JsonProperty("description_fr")]
        //    //public string DescriptionFr { get; set; }

        //    [IsSearchable]
        //    [Analyzer(AnalyzerName.AsString.FrLucene)]
        //    [JsonProperty("description_es")]
        //    public string DescriptionEs { get; set; }


        //    [IsSearchable, IsFilterable, IsSortable]
        //    public string HotelName { get; set; }

        //    [IsSearchable, IsFilterable, IsSortable, IsFacetable]
        //    public string Category { get; set; }

        //    [IsSearchable, IsFilterable, IsFacetable]
        //    public string[] Tags { get; set; }

        //    [IsFilterable, IsFacetable]
        //    public bool? ParkingIncluded { get; set; }

        //    [IsFilterable, IsFacetable]
        //    public bool? SmokingAllowed { get; set; }

        //    [IsFilterable, IsSortable, IsFacetable]
        //    public DateTimeOffset? LastRenovationDate { get; set; }

        //    [IsFilterable, IsSortable, IsFacetable]
        //    public int? Rating { get; set; }

        //    [IsFilterable, IsSortable]
        //    public GeographyPoint Location { get; set; }
        //}
    }

