using System;
using System.Collections.Generic;
using RestSharp;
using IO.Swagger.Client;
using IO.Swagger.Model;

namespace IO.Swagger.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IEntryApiApi
    {
        /// <summary>
        /// Find entries. 
        /// </summary>
        /// <param name="query">Entry name query (optional).</param>
        /// <param name="tagName">Filter by tag name (optional).</param>
        /// <param name="tagId">Filter by tag Id (optional).</param>
        /// <param name="childTags">Include child tags, if the tags being filtered by have any.</param>
        /// <param name="entryTypes">Included entry types (optional).</param>
        /// <param name="status">Filter by entry status (optional).</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 30).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, AdditionDate.</param>
        /// <param name="nameMatchMode">Match mode for entry name (optional, defaults to Exact).</param>
        /// <param name="fields">List of optional fields (optional). Possible values are Description, MainPicture, Names, Tags, WebLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>PartialFindResultEntryForApiContract</returns>
        PartialFindResultEntryForApiContract EntryApiGetList (string query, List<string> tagName, List<int?> tagId, bool? childTags, string entryTypes, string status, int? start, int? maxResults, bool? getTotalCount, string sort, string nameMatchMode, string fields, string lang);
        /// <summary>
        /// Gets a list of entry names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param>
        /// <param name="nameMatchMode">Name match mode.</param>
        /// <param name="maxResults">Maximum number of results.</param>
        /// <returns>List&lt;string&gt;</returns>
        List<string> EntryApiGetNames (string query, string nameMatchMode, int? maxResults);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class EntryApiApi : IEntryApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public EntryApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="EntryApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public EntryApiApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
        }
    
        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public void SetBasePath(String basePath)
        {
            this.ApiClient.BasePath = basePath;
        }
    
        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public String GetBasePath(String basePath)
        {
            return this.ApiClient.BasePath;
        }
    
        /// <summary>
        /// Gets or sets the API client.
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient {get; set;}
    
        /// <summary>
        /// Find entries. 
        /// </summary>
        /// <param name="query">Entry name query (optional).</param> 
        /// <param name="tagName">Filter by tag name (optional).</param> 
        /// <param name="tagId">Filter by tag Id (optional).</param> 
        /// <param name="childTags">Include child tags, if the tags being filtered by have any.</param> 
        /// <param name="entryTypes">Included entry types (optional).</param> 
        /// <param name="status">Filter by entry status (optional).</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 30).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, AdditionDate.</param> 
        /// <param name="nameMatchMode">Match mode for entry name (optional, defaults to Exact).</param> 
        /// <param name="fields">List of optional fields (optional). Possible values are Description, MainPicture, Names, Tags, WebLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>PartialFindResultEntryForApiContract</returns>            
        public PartialFindResultEntryForApiContract EntryApiGetList (string query, List<string> tagName, List<int?> tagId, bool? childTags, string entryTypes, string status, int? start, int? maxResults, bool? getTotalCount, string sort, string nameMatchMode, string fields, string lang)
        {
            
    
            var path = "/api/entries";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (tagName != null) queryParams.Add("tagName", ApiClient.ParameterToString(tagName)); // query parameter
 if (tagId != null) queryParams.Add("tagId", ApiClient.ParameterToString(tagId)); // query parameter
 if (childTags != null) queryParams.Add("childTags", ApiClient.ParameterToString(childTags)); // query parameter
 if (entryTypes != null) queryParams.Add("entryTypes", ApiClient.ParameterToString(entryTypes)); // query parameter
 if (status != null) queryParams.Add("status", ApiClient.ParameterToString(status)); // query parameter
 if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
 if (sort != null) queryParams.Add("sort", ApiClient.ParameterToString(sort)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling EntryApiGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling EntryApiGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultEntryForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultEntryForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of entry names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param> 
        /// <param name="nameMatchMode">Name match mode.</param> 
        /// <param name="maxResults">Maximum number of results.</param> 
        /// <returns>List&lt;string&gt;</returns>            
        public List<string> EntryApiGetNames (string query, string nameMatchMode, int? maxResults)
        {
            
    
            var path = "/api/entries/names";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling EntryApiGetNames: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling EntryApiGetNames: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
    
    }
}
