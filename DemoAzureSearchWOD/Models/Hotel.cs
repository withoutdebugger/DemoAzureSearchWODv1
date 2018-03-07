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
}

