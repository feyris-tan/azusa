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
    public interface IActivityEntryApiApi
    {
        /// <summary>
        /// Gets a list of recent activity entries. Entries are always returned sorted from newest to oldest.              Activity for deleted entries is not returned.
        /// </summary>
        /// <param name="before">Filter to return activity entries only before this date. Optional, by default no filter.</param>
        /// <param name="since">Filter to return activity entries only after this date. Optional, by default no filter.</param>
        /// <param name="userId">Filter by user Id. Optional, by default no filter.</param>
        /// <param name="editEvent">Filter by entry edit event (either Created or Updated). Optional, by default no filter.</param>
        /// <param name="maxResults">Maximum number of results to return. Default 50. Maximum value 500.</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="fields">Optional fields.</param>
        /// <param name="entryFields">Optional fields for entries.</param>
        /// <param name="lang">Content language preference.</param>
        /// <returns>PartialFindResultActivityEntryForApiContract</returns>
        PartialFindResultActivityEntryForApiContract ActivityEntryApiGetList (DateTime? before, DateTime? since, int? userId, string editEvent, int? maxResults, bool? getTotalCount, string fields, string entryFields, string lang);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ActivityEntryApiApi : IActivityEntryApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityEntryApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ActivityEntryApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityEntryApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ActivityEntryApiApi(String basePath)
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
        /// Gets a list of recent activity entries. Entries are always returned sorted from newest to oldest.              Activity for deleted entries is not returned.
        /// </summary>
        /// <param name="before">Filter to return activity entries only before this date. Optional, by default no filter.</param> 
        /// <param name="since">Filter to return activity entries only after this date. Optional, by default no filter.</param> 
        /// <param name="userId">Filter by user Id. Optional, by default no filter.</param> 
        /// <param name="editEvent">Filter by entry edit event (either Created or Updated). Optional, by default no filter.</param> 
        /// <param name="maxResults">Maximum number of results to return. Default 50. Maximum value 500.</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="fields">Optional fields.</param> 
        /// <param name="entryFields">Optional fields for entries.</param> 
        /// <param name="lang">Content language preference.</param> 
        /// <returns>PartialFindResultActivityEntryForApiContract</returns>            
        public PartialFindResultActivityEntryForApiContract ActivityEntryApiGetList (DateTime? before, DateTime? since, int? userId, string editEvent, int? maxResults, bool? getTotalCount, string fields, string entryFields, string lang)
        {
            
    
            var path = "/api/activityEntries";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (before != null) queryParams.Add("before", ApiClient.ParameterToString(before)); // query parameter
 if (since != null) queryParams.Add("since", ApiClient.ParameterToString(since)); // query parameter
 if (userId != null) queryParams.Add("userId", ApiClient.ParameterToString(userId)); // query parameter
 if (editEvent != null) queryParams.Add("editEvent", ApiClient.ParameterToString(editEvent)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (entryFields != null) queryParams.Add("entryFields", ApiClient.ParameterToString(entryFields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ActivityEntryApiGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ActivityEntryApiGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultActivityEntryForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultActivityEntryForApiContract), response.Headers);
        }
    
    }
}
