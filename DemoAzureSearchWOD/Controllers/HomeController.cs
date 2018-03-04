using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoAzureSearchWOD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Configuration;

namespace DemoAzureSearchWOD.Controllers
{
    public class HomeController : Controller
    {

        private IConfiguration configuration { get; set; }
        //SearchServiceClient serviceClient;
        SearchIndexClient indexClient;
        string searchServiceName;
        string queryApiKey;

        public HomeController(IConfiguration _configuration )
        {
            searchServiceName = _configuration["AzureSearch:SearchServiceName"];
            /*string adminApiKey = _configuration["AzureSearch:SearchServiceAdminApiKey"]*/;
            queryApiKey = _configuration["AzureSearch:SearchServiceQueryApiKey"];

            //serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
            indexClient = new SearchIndexClient(searchServiceName, "realestate-us-sample", new SearchCredentials(queryApiKey));

        }
        public IActionResult Index()
        {
            
            SearchParameters sp = CreateParameter(10, "", 1);
            var result = indexClient.Documents.Search<Hotel>("house", sp);
           
            return View(result);
        }

        [HttpPost]
        public ActionResult Index(IFormCollection _form)
        {

            SearchParameters sp = CreateParameter(10, _form["txtSearch"], 2);
            var result = indexClient.Documents.Search<Hotel>("house", sp);

            return View(result);
        }

        public SearchParameters CreateParameter(int top, string filter, int currentPage) {

            return new SearchParameters()
            {
                SearchMode = SearchMode.Any,
                Top = 10,
                Filter = filter,
                Skip = currentPage - 1,
                // Limit results
                Select = new List<String>() { "listingId", "description", "description_es", "number", "street", "city", "countryCode", "postCode", "thumbnail", "status", "tags" },
                // Add count
                IncludeTotalResultCount = true,
                //// Add search highlights
                //HighlightFields = new List<String>() { "job_description" },
                //HighlightPreTag = "<b>",
                //HighlightPostTag = "</b>",
                //// Add facets
                Facets = new List<String>() { "city", "type", "status", "tags" },
            };

        }

        #region Createors
        //private static SearchServiceClient CreateSearchServiceClient(IConfiguration configuration)
        //{
        //    searchServiceName = configuration["AzureSearch:SearchServiceName"];
        //    adminApiKey = configuration["AzureSearch:SearchServiceAdminApiKey"];

        //    SearchServiceClient serviceClient = new SearchServiceClient(searchServiceName, new SearchCredentials(adminApiKey));
        //    return serviceClient;
        //}

        //private static SearchIndexClient CreateSearchIndexClient(IConfiguration configuration)
        //{
        //    searchServiceName = configuration["AzureSearch:SearchServiceName"];
        //    queryApiKey = configuration["AzureSearch:SearchServiceQueryApiKey"];

        //    SearchIndexClient indexClient = new SearchIndexClient(searchServiceName, "realestate-us-sample", new SearchCredentials(queryApiKey));
        //    return indexClient;
        //}


        private static void DeleteHotelsIndexIfExists(SearchServiceClient serviceClient)
        {
            if (serviceClient.Indexes.Exists("hotels"))
            {
                serviceClient.Indexes.Delete("hotels");
            }
        }

        private static void CreateHotelsIndex(SearchServiceClient serviceClient)
        {
            var definition = new Index()
            {
                Name = "hotels",
                Fields = FieldBuilder.BuildForType<Hotel>()
            };

            serviceClient.Indexes.Create(definition);
        }

        #endregion

        private static void RunQueries(ISearchIndexClient indexClient)
        {
            SearchParameters parameters;
            DocumentSearchResult<Hotel> results;

            Console.WriteLine("Search the entire index for the term 'budget' and return only the hotelName field:\n");

            parameters =
                new SearchParameters()
                {
                    Select = new[] { "beds" }
                };

            results = indexClient.Documents.Search<Hotel>("3", parameters);

            WriteDocuments(results);

            //Console.Write("Apply a filter to the index to find hotels cheaper than $150 per night, ");
            //Console.WriteLine("and return the hotelId and description:\n");

            //parameters =
            //    new SearchParameters()
            //    {
            //        Filter = "baseRate lt 150",
            //        Select = new[] { "hotelId", "description" }
            //    };

            //results = indexClient.Documents.Search<Hotel>("*", parameters);

            //WriteDocuments(results);

            //Console.Write("Search the entire index, order by a specific field (lastRenovationDate) ");
            //Console.Write("in descending order, take the top two results, and show only hotelName and ");
            //Console.WriteLine("lastRenovationDate:\n");

            //parameters =
            //    new SearchParameters()
            //    {
            //        OrderBy = new[] { "lastRenovationDate desc" },
            //        Select = new[] { "hotelName", "lastRenovationDate" },
            //        Top = 2
            //    };

            //results = indexClient.Documents.Search<Hotel>("*", parameters);

            //WriteDocuments(results);

            //Console.WriteLine("Search the entire index for the term 'motel':\n");

            //parameters = new SearchParameters();
            //results = indexClient.Documents.Search<Hotel>("motel", parameters);

            //WriteDocuments(results);
        }

        private static void WriteDocuments(DocumentSearchResult<Hotel> searchResults)
        {
            foreach (SearchResult<Hotel> result in searchResults.Results)
            {
                Console.WriteLine(result.Document);
            }

            Console.WriteLine();
        }
    }
}
