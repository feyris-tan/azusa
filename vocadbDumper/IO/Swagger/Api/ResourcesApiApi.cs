using System;
using System.Collections.Generic;
using RestSharp;
using IO.Swagger.Client;

namespace IO.Swagger.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IResourcesApiApi
    {
        /// <summary>
        /// Gets a number of resource sets for a specific culture. 
        /// </summary>
        /// <param name="cultureCode">Culture code, for example \&quot;en-US\&quot; or \&quot;fi-FI\&quot;.</param>
        /// <param name="setNames">Names of resource sets to be returned. More than one value can be specified. For example \&quot;artistTypeNames\&quot;</param>
        /// <returns>Dictionary&lt;string, Dictionary&lt;string, string&gt;&gt;</returns>
        Dictionary<string, Dictionary<string, string>> ResourcesApiGetList (string cultureCode, List<string> setNames);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ResourcesApiApi : IResourcesApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ResourcesApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourcesApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ResourcesApiApi(String basePath)
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
        /// Gets a number of resource sets for a specific culture. 
        /// </summary>
        /// <param name="cultureCode">Culture code, for example \&quot;en-US\&quot; or \&quot;fi-FI\&quot;.</param> 
        /// <param name="setNames">Names of resource sets to be returned. More than one value can be specified. For example \&quot;artistTypeNames\&quot;</param> 
        /// <returns>Dictionary&lt;string, Dictionary&lt;string, string&gt;&gt;</returns>            
        public Dictionary<string, Dictionary<string, string>> ResourcesApiGetList (string cultureCode, List<string> setNames)
        {
            
            // verify the required parameter 'cultureCode' is set
            if (cultureCode == null) throw new ApiException(400, "Missing required parameter 'cultureCode' when calling ResourcesApiGetList");
            
            // verify the required parameter 'setNames' is set
            if (setNames == null) throw new ApiException(400, "Missing required parameter 'setNames' when calling ResourcesApiGetList");
            
    
            var path = "/api/resources/{cultureCode}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "cultureCode" + "}", ApiClient.ParameterToString(cultureCode));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (setNames != null) queryParams.Add("setNames", ApiClient.ParameterToString(setNames)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ResourcesApiGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ResourcesApiGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (Dictionary<string, Dictionary<string, string>>) ApiClient.Deserialize(response.Content, typeof(Dictionary<string, Dictionary<string, string>>), response.Headers);
        }
    
    }
}
