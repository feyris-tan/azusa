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
    public interface ISongListApiApi
    {
        /// <summary>
        /// Deletes a song list. 
        /// </summary>
        /// <param name="id">ID of the list to be deleted.</param>
        /// <param name="notes">Notes.</param>
        /// <param name="hardDelete">If true, the entry is hard deleted. Hard deleted entries cannot be restored normally, but they will be moved to trash.              If false, the entry is soft deleted, meaning it can still be restored.</param>
        /// <returns></returns>
        void SongListApiDelete (int? id, string notes, bool? hardDelete);
        /// <summary>
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param>
        /// <returns></returns>
        void SongListApiDeleteComment (int? commentId);
        /// <summary>
        /// Gets a list of comments for a song list. 
        /// </summary>
        /// <param name="listId">ID of the list whose comments to load.</param>
        /// <returns>PartialFindResultCommentForApiContract</returns>
        PartialFindResultCommentForApiContract SongListApiGetComments (int? listId);
        /// <summary>
        /// Gets a list of featuedd list names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param>
        /// <param name="nameMatchMode">Name match mode. Words is treated the same as Partial.</param>
        /// <param name="featuredCategory">Filter by a specific featured category. If empty, all categories are returned.</param>
        /// <param name="maxResults">Maximum number of results.</param>
        /// <returns>List&lt;string&gt;</returns>
        List<string> SongListApiGetFeaturedListNames (string query, string nameMatchMode, string featuredCategory, int? maxResults);
        /// <summary>
        /// Gets a list of featured song lists. 
        /// </summary>
        /// <param name="query">Song list name query (optional).</param>
        /// <param name="nameMatchMode">Match mode for list name (optional, defaults to Auto).</param>
        /// <param name="featuredCategory">Filter by a specific featured category. If empty, all categories are returned.</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="sort">List sort rule. Possible values are Nothing, Date, CreateDate, Name.</param>
        /// <returns>PartialFindResultSongListForApiContract</returns>
        PartialFindResultSongListForApiContract SongListApiGetFeaturedLists (string query, string nameMatchMode, string featuredCategory, int? start, int? maxResults, bool? getTotalCount, string sort);
        /// <summary>
        /// Gets a list of songs in a song list. 
        /// </summary>
        /// <param name="listId">ID of the song list.</param>
        /// <param name="query">Song name query (optional).</param>
        /// <param name="songTypes">Filtered song types (optional).</param>
        /// <param name="pvServices">Filter by one or more PV services (separated by commas). The song will pass the filter if it has a PV for any of the matched services.</param>
        /// <param name="tagId">Filter by one or more tag Ids (optional).</param>
        /// <param name="artistId">Filter by artist Id.</param>
        /// <param name="childVoicebanks">Include child voicebanks, if the artist being filtered by has any.</param>
        /// <param name="advancedFilters">List of advanced filters (optional).</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="sort">Song sort rule (optional, by default songs are sorted by song list order).</param>
        /// <param name="nameMatchMode">Match mode for song name (optional, defaults to Auto).</param>
        /// <param name="fields">List of optional fields (optional). Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>PartialFindResultSongInListForApiContract</returns>
        PartialFindResultSongInListForApiContract SongListApiGetSongs (int? listId, string query, string songTypes, string pvServices, List<int?> tagId, List<int?> artistId, bool? childVoicebanks, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, string nameMatchMode, string fields, string lang);
        /// <summary>
        /// Creates a song list. 
        /// </summary>
        /// <param name="list">Song list properties.</param>
        /// <returns>int?</returns>
        int? SongListApiPost (SongListForEditContract list);
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param>
        /// <param name="contract">New comment data. Only message can be edited.</param>
        /// <returns></returns>
        void SongListApiPostEditComment (int? commentId, CommentForApiContract contract);
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="listId">ID of the song list for which to create the comment.</param>
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param>
        /// <returns>CommentForApiContract</returns>
        CommentForApiContract SongListApiPostNewComment (int? listId, CommentForApiContract contract);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class SongListApiApi : ISongListApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongListApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public SongListApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="SongListApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public SongListApiApi(String basePath)
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
        /// Deletes a song list. 
        /// </summary>
        /// <param name="id">ID of the list to be deleted.</param> 
        /// <param name="notes">Notes.</param> 
        /// <param name="hardDelete">If true, the entry is hard deleted. Hard deleted entries cannot be restored normally, but they will be moved to trash.              If false, the entry is soft deleted, meaning it can still be restored.</param> 
        /// <returns></returns>            
        public void SongListApiDelete (int? id, string notes, bool? hardDelete)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SongListApiDelete");
            
    
            var path = "/api/songLists/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param> 
        /// <returns></returns>            
        public void SongListApiDeleteComment (int? commentId)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling SongListApiDeleteComment");
            
    
            var path = "/api/songLists/comments/{commentId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "commentId" + "}", ApiClient.ParameterToString(commentId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiDeleteComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiDeleteComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Gets a list of comments for a song list. 
        /// </summary>
        /// <param name="listId">ID of the list whose comments to load.</param> 
        /// <returns>PartialFindResultCommentForApiContract</returns>            
        public PartialFindResultCommentForApiContract SongListApiGetComments (int? listId)
        {
            
            // verify the required parameter 'listId' is set
            if (listId == null) throw new ApiException(400, "Missing required parameter 'listId' when calling SongListApiGetComments");
            
    
            var path = "/api/songLists/{listId}/comments";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "listId" + "}", ApiClient.ParameterToString(listId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiGetComments: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiGetComments: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultCommentForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultCommentForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of featuedd list names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param> 
        /// <param name="nameMatchMode">Name match mode. Words is treated the same as Partial.</param> 
        /// <param name="featuredCategory">Filter by a specific featured category. If empty, all categories are returned.</param> 
        /// <param name="maxResults">Maximum number of results.</param> 
        /// <returns>List&lt;string&gt;</returns>            
        public List<string> SongListApiGetFeaturedListNames (string query, string nameMatchMode, string featuredCategory, int? maxResults)
        {
            
    
            var path = "/api/songLists/featured/names";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (featuredCategory != null) queryParams.Add("featuredCategory", ApiClient.ParameterToString(featuredCategory)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiGetFeaturedListNames: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiGetFeaturedListNames: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of featured song lists. 
        /// </summary>
        /// <param name="query">Song list name query (optional).</param> 
        /// <param name="nameMatchMode">Match mode for list name (optional, defaults to Auto).</param> 
        /// <param name="featuredCategory">Filter by a specific featured category. If empty, all categories are returned.</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="sort">List sort rule. Possible values are Nothing, Date, CreateDate, Name.</param> 
        /// <returns>PartialFindResultSongListForApiContract</returns>            
        public PartialFindResultSongListForApiContract SongListApiGetFeaturedLists (string query, string nameMatchMode, string featuredCategory, int? start, int? maxResults, bool? getTotalCount, string sort)
        {
            
    
            var path = "/api/songLists/featured";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (featuredCategory != null) queryParams.Add("featuredCategory", ApiClient.ParameterToString(featuredCategory)); // query parameter
 if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
 if (sort != null) queryParams.Add("sort", ApiClient.ParameterToString(sort)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiGetFeaturedLists: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiGetFeaturedLists: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultSongListForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultSongListForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of songs in a song list. 
        /// </summary>
        /// <param name="listId">ID of the song list.</param> 
        /// <param name="query">Song name query (optional).</param> 
        /// <param name="songTypes">Filtered song types (optional).</param> 
        /// <param name="pvServices">Filter by one or more PV services (separated by commas). The song will pass the filter if it has a PV for any of the matched services.</param> 
        /// <param name="tagId">Filter by one or more tag Ids (optional).</param> 
        /// <param name="artistId">Filter by artist Id.</param> 
        /// <param name="childVoicebanks">Include child voicebanks, if the artist being filtered by has any.</param> 
        /// <param name="advancedFilters">List of advanced filters (optional).</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="sort">Song sort rule (optional, by default songs are sorted by song list order).</param> 
        /// <param name="nameMatchMode">Match mode for song name (optional, defaults to Auto).</param> 
        /// <param name="fields">List of optional fields (optional). Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>PartialFindResultSongInListForApiContract</returns>            
        public PartialFindResultSongInListForApiContract SongListApiGetSongs (int? listId, string query, string songTypes, string pvServices, List<int?> tagId, List<int?> artistId, bool? childVoicebanks, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, string nameMatchMode, string fields, string lang)
        {
            
            // verify the required parameter 'listId' is set
            if (listId == null) throw new ApiException(400, "Missing required parameter 'listId' when calling SongListApiGetSongs");
            
    
            var path = "/api/songLists/{listId}/songs";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "listId" + "}", ApiClient.ParameterToString(listId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (songTypes != null) queryParams.Add("songTypes", ApiClient.ParameterToString(songTypes)); // query parameter
 if (pvServices != null) queryParams.Add("pvServices", ApiClient.ParameterToString(pvServices)); // query parameter
 if (tagId != null) queryParams.Add("tagId", ApiClient.ParameterToString(tagId)); // query parameter
 if (artistId != null) queryParams.Add("artistId", ApiClient.ParameterToString(artistId)); // query parameter
 if (childVoicebanks != null) queryParams.Add("childVoicebanks", ApiClient.ParameterToString(childVoicebanks)); // query parameter
 if (advancedFilters != null) queryParams.Add("advancedFilters", ApiClient.ParameterToString(advancedFilters)); // query parameter
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiGetSongs: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiGetSongs: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultSongInListForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultSongInListForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Creates a song list. 
        /// </summary>
        /// <param name="list">Song list properties.</param> 
        /// <returns>int?</returns>            
        public int? SongListApiPost (SongListForEditContract list)
        {
            
            // verify the required parameter 'list' is set
            if (list == null) throw new ApiException(400, "Missing required parameter 'list' when calling SongListApiPost");
            
    
            var path = "/api/songLists";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(list); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiPost: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiPost: " + response.ErrorMessage, response.ErrorMessage);
    
            return (int?) ApiClient.Deserialize(response.Content, typeof(int?), response.Headers);
        }
    
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param> 
        /// <param name="contract">New comment data. Only message can be edited.</param> 
        /// <returns></returns>            
        public void SongListApiPostEditComment (int? commentId, CommentForApiContract contract)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling SongListApiPostEditComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling SongListApiPostEditComment");
            
    
            var path = "/api/songLists/comments/{commentId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "commentId" + "}", ApiClient.ParameterToString(commentId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(contract); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiPostEditComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiPostEditComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="listId">ID of the song list for which to create the comment.</param> 
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param> 
        /// <returns>CommentForApiContract</returns>            
        public CommentForApiContract SongListApiPostNewComment (int? listId, CommentForApiContract contract)
        {
            
            // verify the required parameter 'listId' is set
            if (listId == null) throw new ApiException(400, "Missing required parameter 'listId' when calling SongListApiPostNewComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling SongListApiPostNewComment");
            
    
            var path = "/api/songLists/{listId}/comments";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "listId" + "}", ApiClient.ParameterToString(listId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(contract); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiPostNewComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongListApiPostNewComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CommentForApiContract) ApiClient.Deserialize(response.Content, typeof(CommentForApiContract), response.Headers);
        }
    
    }
}
