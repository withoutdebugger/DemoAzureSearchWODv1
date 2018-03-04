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
    [Produces("application/json")]
    [Route("api/Search")]
    public class SearchController : Controller
    {

        private IConfiguration configuration { get; set; }        
        SearchIndexClient indexClient;
        string searchServiceName;
        string queryApiKey;

        public SearchController(IConfiguration _configuration)
        {
            searchServiceName = _configuration["AzureSearch:SearchServiceName"];            
            queryApiKey = _configuration["AzureSearch:SearchServiceQueryApiKey"];            
            indexClient = new SearchIndexClient(searchServiceName, "realestate-us-sample", new SearchCredentials(queryApiKey));
        }

        [HttpGet]
        public DocumentSearchResult<Hotel> Get(string searchtext = "", string cityFacet = "", string typeFacet = "", string tags = "",
                                                    string orderby = "", string orderbydirection = "desc", int actualPage = 1)
        {
            //Filter
            string filter = CreateFilter(cityFacet, typeFacet);
            //OrderBy
            IList<string> orderBy = CreateOrderBy(orderby, orderbydirection);
            //Filter Tags
            IList<ScoringParameter> filterTags = CreatedFilterTags(tags);
            //Parameters
            SearchParameters sp = CreateParameter(10, filter, orderBy, filterTags, actualPage);
            //Search
            var result = indexClient.Documents.Search<Hotel>(searchtext, sp);
            
            return result;
        }

        #region privates

        private string CreateFilter(string cityFacet, string typeFacet) {

            string filter = "";
 
            if (cityFacet != null)
            {
                filter += filter != string.Empty ? " and " : "";
                filter += "city eq '" + cityFacet + "'";
            }

            if (typeFacet != null)
            {
                filter += filter != string.Empty ? " and " : "";
                filter += "type eq '" + typeFacet + "'";
            }

            return filter;
        }

        private IList<string> CreateOrderBy(string orderby = "", string orderbydirection = "desc") {

            IList<string> _orderBy = new List<string>();

            if (orderby != string.Empty)
            {
                _orderBy.Add(orderby + " " + orderbydirection + " ");
            }

            return _orderBy;

        }

        private IList<ScoringParameter> CreatedFilterTags(string tags) {

            IList<ScoringParameter> _parameterTags = new List<ScoringParameter>();

            if (tags == null) { return _parameterTags; }

            string[] _listTags = tags.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
          
            if (_listTags.Count() > 0)
            {
                _parameterTags.Add(new ScoringParameter("tags", _listTags));
            }

            return _parameterTags;

        }

        private SearchParameters CreateParameter(int top, string filter, IList<string> orderby, IList<ScoringParameter> filterTag, int currentPage)
        {

            SearchParameters searchParameters =
                new SearchParameters() {
                    SearchMode = SearchMode.Any,
                    Top = 10,
                    Filter = filter,
                    Skip = currentPage - 1,
                    Select = new List<String>() { "listingId", "description", "description_es", "number", "street", "city", "countryCode", "postCode", "thumbnail", "status", "tags" },
                    // Add count
                    IncludeTotalResultCount = true,                    
                    //// Add facets
                    Facets = new List<String>() { "city", "type", "status", "tags" },
                };

            searchParameters.OrderBy = orderby;
            searchParameters.ScoringParameters = filterTag;

            return searchParameters;

        }

        #endregion
    }
}
