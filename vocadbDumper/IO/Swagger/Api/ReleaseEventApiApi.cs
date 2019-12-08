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
    public interface IReleaseEventApiApi
    {
        /// <summary>
        /// Deletes an event. 
        /// </summary>
        /// <param name="id">ID of the event to be deleted.</param>
        /// <param name="notes">Notes.</param>
        /// <param name="hardDelete">If true, the entry is hard deleted. Hard deleted entries cannot be restored normally, but they will be moved to trash.              If false, the entry is soft deleted, meaning it can still be restored.</param>
        /// <returns></returns>
        void ReleaseEventApiDelete (int? id, string notes, bool? hardDelete);
        /// <summary>
        /// Gets a list of albums for a specific event. 
        /// </summary>
        /// <param name="eventId">Release event ID.</param>
        /// <param name="fields">List of optional album fields.</param>
        /// <param name="lang">Content language preference.</param>
        /// <returns>List&lt;AlbumForApiContract&gt;</returns>
        List<AlbumForApiContract> ReleaseEventApiGetAlbums (int? eventId, string fields, string lang);
        /// <summary>
        /// Gets a page of events. 
        /// </summary>
        /// <param name="query">Event name query (optional).</param>
        /// <param name="nameMatchMode">Match mode for event name (optional, defaults to Auto).</param>
        /// <param name="seriesId">Filter by series Id.</param>
        /// <param name="afterDate">Filter by events after this date (inclusive).</param>
        /// <param name="beforeDate">Filter by events before this date (exclusive).</param>
        /// <param name="category">Filter by event category.</param>
        /// <param name="userCollectionId">Filter to include only events in user&#39;s events (interested or attending).</param>
        /// <param name="tagId">Filter by one or more tag Ids (optional).</param>
        /// <param name="childTags">Include child tags, if the tags being filtered by have any.</param>
        /// <param name="artistId">Filter by artist Id.</param>
        /// <param name="childVoicebanks">Include child voicebanks, if the artist being filtered by has any.</param>
        /// <param name="includeMembers">Include members of groups. This applies if {artistId} is a group.</param>
        /// <param name="status">Filter by entry status.</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="sort">Sort rule (optional, defaults to Name).               Possible values are None, Name, Date, SeriesName.</param>
        /// <param name="fields">Optional fields (optional). Possible values are Description, Series.</param>
        /// <param name="lang">Content language preference.</param>
        /// <returns>PartialFindResultReleaseEventForApiContract</returns>
        PartialFindResultReleaseEventForApiContract ReleaseEventApiGetList (string query, string nameMatchMode, int? seriesId, DateTime? afterDate, DateTime? beforeDate, string category, int? userCollectionId, List<int?> tagId, bool? childTags, List<int?> artistId, bool? childVoicebanks, bool? includeMembers, string status, int? start, int? maxResults, bool? getTotalCount, string sort, string fields, string lang);
        /// <summary>
        /// Find event names by a part of name.                            Matching is done anywhere from the name. 
        /// </summary>
        /// <param name="query">Event name query, for example \&quot;Voc@loid\&quot;.</param>
        /// <param name="maxResults">Maximum number of search results.</param>
        /// <returns>List&lt;string&gt;</returns>
        List<string> ReleaseEventApiGetNames (string query, int? maxResults);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fields"></param>
        /// <param name="lang"></param>
        /// <returns>ReleaseEventForApiContract</returns>
        ReleaseEventForApiContract ReleaseEventApiGetOne (int? id, string fields, string lang);
        /// <summary>
        /// Gets a list of songs for a specific event. 
        /// </summary>
        /// <param name="eventId">Event ID.</param>
        /// <param name="fields">List of optional song fields.</param>
        /// <param name="lang">Content language preference.</param>
        /// <returns>List&lt;SongForApiContract&gt;</returns>
        List<SongForApiContract> ReleaseEventApiGetPublishedSongs (int? eventId, string fields, string lang);
        /// <summary>
        /// Creates a new report. 
        /// </summary>
        /// <param name="eventId">Event to be reported.</param>
        /// <param name="reportType">Report type.</param>
        /// <param name="notes">Notes. Optional.</param>
        /// <param name="versionNumber">Version to be reported. Optional.</param>
        /// <returns></returns>
        void ReleaseEventApiPostReport (int? eventId, string reportType, string notes, int? versionNumber);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ReleaseEventApiApi : IReleaseEventApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseEventApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ReleaseEventApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ReleaseEventApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ReleaseEventApiApi(String basePath)
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
        /// Deletes an event. 
        /// </summary>
        /// <param name="id">ID of the event to be deleted.</param> 
        /// <param name="notes">Notes.</param> 
        /// <param name="hardDelete">If true, the entry is hard deleted. Hard deleted entries cannot be restored normally, but they will be moved to trash.              If false, the entry is soft deleted, meaning it can still be restored.</param> 
        /// <returns></returns>            
        public void ReleaseEventApiDelete (int? id, string notes, bool? hardDelete)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ReleaseEventApiDelete");
            
    
            var path = "/api/releaseEvents/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Gets a list of albums for a specific event. 
        /// </summary>
        /// <param name="eventId">Release event ID.</param> 
        /// <param name="fields">List of optional album fields.</param> 
        /// <param name="lang">Content language preference.</param> 
        /// <returns>List&lt;AlbumForApiContract&gt;</returns>            
        public List<AlbumForApiContract> ReleaseEventApiGetAlbums (int? eventId, string fields, string lang)
        {
            
            // verify the required parameter 'eventId' is set
            if (eventId == null) throw new ApiException(400, "Missing required parameter 'eventId' when calling ReleaseEventApiGetAlbums");
            
    
            var path = "/api/releaseEvents/{eventId}/albums";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "eventId" + "}", ApiClient.ParameterToString(eventId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiGetAlbums: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiGetAlbums: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<AlbumForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<AlbumForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Gets a page of events. 
        /// </summary>
        /// <param name="query">Event name query (optional).</param> 
        /// <param name="nameMatchMode">Match mode for event name (optional, defaults to Auto).</param> 
        /// <param name="seriesId">Filter by series Id.</param> 
        /// <param name="afterDate">Filter by events after this date (inclusive).</param> 
        /// <param name="beforeDate">Filter by events before this date (exclusive).</param> 
        /// <param name="category">Filter by event category.</param> 
        /// <param name="userCollectionId">Filter to include only events in user&#39;s events (interested or attending).</param> 
        /// <param name="tagId">Filter by one or more tag Ids (optional).</param> 
        /// <param name="childTags">Include child tags, if the tags being filtered by have any.</param> 
        /// <param name="artistId">Filter by artist Id.</param> 
        /// <param name="childVoicebanks">Include child voicebanks, if the artist being filtered by has any.</param> 
        /// <param name="includeMembers">Include members of groups. This applies if {artistId} is a group.</param> 
        /// <param name="status">Filter by entry status.</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="sort">Sort rule (optional, defaults to Name).               Possible values are None, Name, Date, SeriesName.</param> 
        /// <param name="fields">Optional fields (optional). Possible values are Description, Series.</param> 
        /// <param name="lang">Content language preference.</param> 
        /// <returns>PartialFindResultReleaseEventForApiContract</returns>            
        public PartialFindResultReleaseEventForApiContract ReleaseEventApiGetList (string query, string nameMatchMode, int? seriesId, DateTime? afterDate, DateTime? beforeDate, string category, int? userCollectionId, List<int?> tagId, bool? childTags, List<int?> artistId, bool? childVoicebanks, bool? includeMembers, string status, int? start, int? maxResults, bool? getTotalCount, string sort, string fields, string lang)
        {
            
    
            var path = "/api/releaseEvents";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (seriesId != null) queryParams.Add("seriesId", ApiClient.ParameterToString(seriesId)); // query parameter
 if (afterDate != null) queryParams.Add("afterDate", ApiClient.ParameterToString(afterDate)); // query parameter
 if (beforeDate != null) queryParams.Add("beforeDate", ApiClient.ParameterToString(beforeDate)); // query parameter
 if (category != null) queryParams.Add("category", ApiClient.ParameterToString(category)); // query parameter
 if (userCollectionId != null) queryParams.Add("userCollectionId", ApiClient.ParameterToString(userCollectionId)); // query parameter
 if (tagId != null) queryParams.Add("tagId", ApiClient.ParameterToString(tagId)); // query parameter
 if (childTags != null) queryParams.Add("childTags", ApiClient.ParameterToString(childTags)); // query parameter
 if (artistId != null) queryParams.Add("artistId", ApiClient.ParameterToString(artistId)); // query parameter
 if (childVoicebanks != null) queryParams.Add("childVoicebanks", ApiClient.ParameterToString(childVoicebanks)); // query parameter
 if (includeMembers != null) queryParams.Add("includeMembers", ApiClient.ParameterToString(includeMembers)); // query parameter
 if (status != null) queryParams.Add("status", ApiClient.ParameterToString(status)); // query parameter
 if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
 if (sort != null) queryParams.Add("sort", ApiClient.ParameterToString(sort)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultReleaseEventForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultReleaseEventForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Find event names by a part of name.                            Matching is done anywhere from the name. 
        /// </summary>
        /// <param name="query">Event name query, for example \&quot;Voc@loid\&quot;.</param> 
        /// <param name="maxResults">Maximum number of search results.</param> 
        /// <returns>List&lt;string&gt;</returns>            
        public List<string> ReleaseEventApiGetNames (string query, int? maxResults)
        {
            
    
            var path = "/api/releaseEvents/names";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiGetNames: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiGetNames: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="fields"></param> 
        /// <param name="lang"></param> 
        /// <returns>ReleaseEventForApiContract</returns>            
        public ReleaseEventForApiContract ReleaseEventApiGetOne (int? id, string fields, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ReleaseEventApiGetOne");
            
    
            var path = "/api/releaseEvents/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiGetOne: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiGetOne: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ReleaseEventForApiContract) ApiClient.Deserialize(response.Content, typeof(ReleaseEventForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of songs for a specific event. 
        /// </summary>
        /// <param name="eventId">Event ID.</param> 
        /// <param name="fields">List of optional song fields.</param> 
        /// <param name="lang">Content language preference.</param> 
        /// <returns>List&lt;SongForApiContract&gt;</returns>            
        public List<SongForApiContract> ReleaseEventApiGetPublishedSongs (int? eventId, string fields, string lang)
        {
            
            // verify the required parameter 'eventId' is set
            if (eventId == null) throw new ApiException(400, "Missing required parameter 'eventId' when calling ReleaseEventApiGetPublishedSongs");
            
    
            var path = "/api/releaseEvents/{eventId}/published-songs";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "eventId" + "}", ApiClient.ParameterToString(eventId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiGetPublishedSongs: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiGetPublishedSongs: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<SongForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<SongForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Creates a new report. 
        /// </summary>
        /// <param name="eventId">Event to be reported.</param> 
        /// <param name="reportType">Report type.</param> 
        /// <param name="notes">Notes. Optional.</param> 
        /// <param name="versionNumber">Version to be reported. Optional.</param> 
        /// <returns></returns>            
        public void ReleaseEventApiPostReport (int? eventId, string reportType, string notes, int? versionNumber)
        {
            
            // verify the required parameter 'eventId' is set
            if (eventId == null) throw new ApiException(400, "Missing required parameter 'eventId' when calling ReleaseEventApiPostReport");
            
            // verify the required parameter 'reportType' is set
            if (reportType == null) throw new ApiException(400, "Missing required parameter 'reportType' when calling ReleaseEventApiPostReport");
            
            // verify the required parameter 'notes' is set
            if (notes == null) throw new ApiException(400, "Missing required parameter 'notes' when calling ReleaseEventApiPostReport");
            
            // verify the required parameter 'versionNumber' is set
            if (versionNumber == null) throw new ApiException(400, "Missing required parameter 'versionNumber' when calling ReleaseEventApiPostReport");
            
    
            var path = "/api/releaseEvents/{eventId}/reports";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "eventId" + "}", ApiClient.ParameterToString(eventId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (reportType != null) queryParams.Add("reportType", ApiClient.ParameterToString(reportType)); // query parameter
 if (notes != null) queryParams.Add("notes", ApiClient.ParameterToString(notes)); // query parameter
 if (versionNumber != null) queryParams.Add("versionNumber", ApiClient.ParameterToString(versionNumber)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiPostReport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ReleaseEventApiPostReport: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}
