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
    public interface IReleaseEventSeriesApiApi
    {
        /// <summary>
        /// Deletes an event series. 
        /// </summary>
        /// <param name="id">ID of the series to be deleted.</param>
        /// <param name="notes">Notes.</param>
        /// <param name="hardDelete">If true, the entry is hard deleted. Hard deleted entries cannot be restored normally, but they will be moved to trash.              If false, the entry is soft deleted, meaning it can still be restored.</param>
        /// <returns></returns>
        void ReleaseEventSeriesApiDelete (int? id, string notes, bool? hardDelete);
        /// <summary>
        /// Gets a page of event series. 
        /// </summary>
        /// <param name="query">Text query.</param>
        /// <param name="start">First item to be retrieved (optional).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional).</param>
        /// <param name="nameMatchMode">Match mode for event name (optional).</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>PartialFindResultReleaseEventSeriesContract</returns>
        PartialFindResultReleaseEventSeriesContract ReleaseEventSeriesApiGetList (string query, int? start, int? maxResults, bool? getTotalCount, string nameMatchMode, string lang);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ReleaseEventSeriesApiApi : IReleaseEventSeriesApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseEventSeriesApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ReleaseEventSeriesApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseEventSeriesApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ReleaseEventSeriesApiApi(String basePath)
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
        /// Deletes an event series. 
        /// </summary>
        /// <param name="id">ID of the series to be deleted.</param> 
        /// <param name="notes">Notes.</param> 
        /// <param name="hardDelete">If true, the entry is hard deleted. Hard deleted entries cannot be restored normally, but they will be moved to trash.              If false, the entry is soft deleted, meaning it can still be restored.</param> 
        /// <returns></returns>            
        public void ReleaseEventSeriesApiDelete (int? id, string notes, bool? hardDelete)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ReleaseEventSeriesApiDelete");
            
    
            var path = "/api/releaseEventSeries/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (notes != null) queryParams.Add("notes", ApiClient.ParameterToString(notes)); // query parameter
 if (hardDelete != null) queryParams.Add("hardDelete", ApiClient.ParameterToString(hardDelete)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventSeriesApiDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventSeriesApiDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Gets a page of event series. 
        /// </summary>
        /// <param name="query">Text query.</param> 
        /// <param name="start">First item to be retrieved (optional).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional).</param> 
        /// <param name="nameMatchMode">Match mode for event name (optional).</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>PartialFindResultReleaseEventSeriesContract</returns>            
        public PartialFindResultReleaseEventSeriesContract ReleaseEventSeriesApiGetList (string query, int? start, int? maxResults, bool? getTotalCount, string nameMatchMode, string lang)
        {
            
    
            var path = "/api/releaseEventSeries";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventSeriesApiGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventSeriesApiGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultReleaseEventSeriesContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultReleaseEventSeriesContract), response.Headers);
        }
    
    }
}
