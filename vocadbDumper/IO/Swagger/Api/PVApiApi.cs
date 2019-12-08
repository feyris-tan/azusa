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
    public interface IPVApiApi
    {
        /// <summary>
        /// Gets a list of PVs for songs. 
        /// </summary>
        /// <param name="author">Uploader name (optional).</param>
        /// <param name="maxResults">Maximum number of results.</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>PartialFindResultPVForSongContract</returns>
        PartialFindResultPVForSongContract PVApiGetList (string author, int? maxResults, bool? getTotalCount, string lang);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class PVApiApi : IPVApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PVApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public PVApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="PVApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public PVApiApi(String basePath)
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
        /// Gets a list of PVs for songs. 
        /// </summary>
        /// <param name="author">Uploader name (optional).</param> 
        /// <param name="maxResults">Maximum number of results.</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>PartialFindResultPVForSongContract</returns>            
        public PartialFindResultPVForSongContract PVApiGetList (string author, int? maxResults, bool? getTotalCount, string lang)
        {
            
    
            var path = "/api/pvs/for-songs";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (author != null) queryParams.Add("author", ApiClient.ParameterToString(author)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling PVApiGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling PVApiGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultPVForSongContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultPVForSongContract), response.Headers);
        }
    
    }
}
