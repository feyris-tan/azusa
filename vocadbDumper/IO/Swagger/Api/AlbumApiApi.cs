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
    public interface IAlbumApiApi
    {
        /// <summary>
        /// Deletes an album. 
        /// </summary>
        /// <param name="id">ID of the album to be deleted.</param>
        /// <param name="notes">Notes.</param>
        /// <returns></returns>
        void AlbumApiDelete (int? id, string notes);
        /// <summary>
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param>
        /// <returns></returns>
        void AlbumApiDeleteComment (int? commentId);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        void AlbumApiDeleteReview (int? reviewId, string id);
        /// <summary>
        /// Gets a list of comments for an album. Pagination and sorting might be added later.
        /// </summary>
        /// <param name="id">ID of the album whose comments to load.</param>
        /// <returns>List&lt;CommentForApiContract&gt;</returns>
        List<CommentForApiContract> AlbumApiGetComments (int? id);
        /// <summary>
        /// Gets a page of albums. 
        /// </summary>
        /// <param name="query">Album name query (optional).</param>
        /// <param name="discTypes">Disc type. By default nothing. Possible values are Album, Single, EP, SplitAlbum, Compilation, Video, Other. Note: only one type supported for now.</param>
        /// <param name="tagName">Filter by tag name (optional). This filter can be specified multiple times.</param>
        /// <param name="tagId">Filter by tag Id (optional). This filter can be specified multiple times.</param>
        /// <param name="childTags">Include child tags, if the tags being filtered by have any.</param>
        /// <param name="artistId">Filter by artist Id (optional).</param>
        /// <param name="artistParticipationStatus">Filter by artist participation status. Only valid if artistId is specified.              Everything (default): Show all albums by that artist (no filter).              OnlyMainAlbums: Show only main albums by that artist.              OnlyCollaborations: Show only collaborations by that artist.</param>
        /// <param name="childVoicebanks">Include child voicebanks, if the artist being filtered by has any.</param>
        /// <param name="includeMembers">Include members of groups. This applies if {artistId} is a group.</param>
        /// <param name="barcode">Filter by album barcode (optional).</param>
        /// <param name="status">Filter by entry status (optional).</param>
        /// <param name="releaseDateAfter">Filter by albums whose release date is after this date (inclusive).</param>
        /// <param name="releaseDateBefore">Filter by albums whose release date is before this date (exclusive).</param>
        /// <param name="advancedFilters">List of advanced filters (optional).</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="sort">Sort rule (optional, defaults to Name).               Possible values are None, Name, ReleaseDate, ReleaseDateWithNulls, AdditionDate, RatingAverage, RatingTotal, NameThenReleaseDate.</param>
        /// <param name="preferAccurateMatches">Whether the search should prefer accurate matches.               If this is true, entries that match by prefix will be moved first, instead of being sorted alphabetically.              Requires a text query. Does not support pagination.              This is mostly useful for autocomplete boxes.</param>
        /// <param name="deleted">Whether to search for deleted entries.              If this is true, only deleted entries will be returned.              If this is false (default), deleted entries are not returned.</param>
        /// <param name="nameMatchMode">Match mode for artist name (optional, defaults to Exact).</param>
        /// <param name="fields">Optional fields (optional). Possible values are artists, names, pvs, tags, tracks, webLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>PartialFindResultAlbumForApiContract</returns>
        PartialFindResultAlbumForApiContract AlbumApiGetList (string query, string discTypes, List<string> tagName, List<int?> tagId, bool? childTags, List<int?> artistId, string artistParticipationStatus, bool? childVoicebanks, bool? includeMembers, string barcode, string status, DateTime? releaseDateAfter, DateTime? releaseDateBefore, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, bool? preferAccurateMatches, bool? deleted, string nameMatchMode, string fields, string lang);
        /// <summary>
        /// Gets a list of album names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param>
        /// <param name="nameMatchMode">Name match mode.</param>
        /// <param name="maxResults">Maximum number of results.</param>
        /// <returns>List&lt;string&gt;</returns>
        List<string> AlbumApiGetNames (string query, string nameMatchMode, int? maxResults);
        /// <summary>
        /// Gets list of upcoming or recent albums, same as front page. Output is cached for 1 hour.
        /// </summary>
        /// <param name="languagePreference"></param>
        /// <param name="fields"></param>
        /// <returns>List&lt;AlbumForApiContract&gt;</returns>
        List<AlbumForApiContract> AlbumApiGetNewAlbums (string languagePreference, string fields);
        /// <summary>
        /// Gets an album by Id. 
        /// </summary>
        /// <param name="id">Album Id (required).</param>
        /// <param name="fields">Optional fields (optional). Possible values are artists, names, pvs, tags, tracks, webLinks.</param>
        /// <param name="songFields">Optional fields for tracks, if included (optional).</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>AlbumForApiContract</returns>
        AlbumForApiContract AlbumApiGetOne (int? id, string fields, string songFields, string lang);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="languageCode"></param>
        /// <returns>List&lt;AlbumReviewContract&gt;</returns>
        List<AlbumReviewContract> AlbumApiGetReviews (int? id, string languageCode);
        /// <summary>
        /// Gets list of top rated albums, same as front page. Output is cached for 1 hour.
        /// </summary>
        /// <param name="ignoreIds"></param>
        /// <param name="languagePreference"></param>
        /// <param name="fields"></param>
        /// <returns>List&lt;AlbumForApiContract&gt;</returns>
        List<AlbumForApiContract> AlbumApiGetTopAlbums (List<int?> ignoreIds, string languagePreference, string fields);
        /// <summary>
        /// Gets tracks for an album. 
        /// </summary>
        /// <param name="id">Album ID (required).</param>
        /// <param name="fields">List of optional fields (optional). Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>List&lt;SongInAlbumForApiContract&gt;</returns>
        List<SongInAlbumForApiContract> AlbumApiGetTracks (int? id, string fields, string lang);
        /// <summary>
        /// Gets tracks for an album formatted using the CSV format string. 
        /// </summary>
        /// <param name="id">Album ID.</param>
        /// <param name="field">Field to be included, for example \&quot;featvocalists\&quot; or \&quot;url\&quot;. Can be specified multiple times.</param>
        /// <param name="lang">Language preference.</param>
        /// <returns>List&lt;Dictionary&lt;string, string&gt;&gt;</returns>
        List<Dictionary<string, string>> AlbumApiGetTracksFormatted (int? id, List<string> field, string lang);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="languagePreference"></param>
        /// <returns>List&lt;AlbumForUserForApiContract&gt;</returns>
        List<AlbumForUserForApiContract> AlbumApiGetUserCollections (int? id, string languagePreference);
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param>
        /// <param name="contract">New comment data. Only message can be edited.</param>
        /// <returns></returns>
        void AlbumApiPostEditComment (int? commentId, CommentForApiContract contract);
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="id">ID of the album for which to create the comment.</param>
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param>
        /// <returns>CommentForApiContract</returns>
        CommentForApiContract AlbumApiPostNewComment (int? id, CommentForApiContract contract);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reviewContract"></param>
        /// <returns>AlbumReviewContract</returns>
        AlbumReviewContract AlbumApiPostReview (int? id, AlbumReviewContract reviewContract);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class AlbumApiApi : IAlbumApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public AlbumApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="AlbumApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public AlbumApiApi(String basePath)
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
        /// Deletes an album. 
        /// </summary>
        /// <param name="id">ID of the album to be deleted.</param> 
        /// <param name="notes">Notes.</param> 
        /// <returns></returns>            
        public void AlbumApiDelete (int? id, string notes)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling AlbumApiDelete");
            
    
            var path = "/api/albums/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (notes != null) queryParams.Add("notes", ApiClient.ParameterToString(notes)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param> 
        /// <returns></returns>            
        public void AlbumApiDeleteComment (int? commentId)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling AlbumApiDeleteComment");
            
    
            var path = "/api/albums/comments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiDeleteComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiDeleteComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="reviewId"></param> 
        /// <param name="id"></param> 
        /// <returns></returns>            
        public void AlbumApiDeleteReview (int? reviewId, string id)
        {
            
            // verify the required parameter 'reviewId' is set
            if (reviewId == null) throw new ApiException(400, "Missing required parameter 'reviewId' when calling AlbumApiDeleteReview");
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling AlbumApiDeleteReview");
            
    
            var path = "/api/albums/{id}/reviews/{reviewId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "reviewId" + "}", ApiClient.ParameterToString(reviewId));
path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiDeleteReview: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiDeleteReview: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Gets a list of comments for an album. Pagination and sorting might be added later.
        /// </summary>
        /// <param name="id">ID of the album whose comments to load.</param> 
        /// <returns>List&lt;CommentForApiContract&gt;</returns>            
        public List<CommentForApiContract> AlbumApiGetComments (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling AlbumApiGetComments");
            
    
            var path = "/api/albums/{id}/comments";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetComments: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetComments: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<CommentForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<CommentForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Gets a page of albums. 
        /// </summary>
        /// <param name="query">Album name query (optional).</param> 
        /// <param name="discTypes">Disc type. By default nothing. Possible values are Album, Single, EP, SplitAlbum, Compilation, Video, Other. Note: only one type supported for now.</param> 
        /// <param name="tagName">Filter by tag name (optional). This filter can be specified multiple times.</param> 
        /// <param name="tagId">Filter by tag Id (optional). This filter can be specified multiple times.</param> 
        /// <param name="childTags">Include child tags, if the tags being filtered by have any.</param> 
        /// <param name="artistId">Filter by artist Id (optional).</param> 
        /// <param name="artistParticipationStatus">Filter by artist participation status. Only valid if artistId is specified.              Everything (default): Show all albums by that artist (no filter).              OnlyMainAlbums: Show only main albums by that artist.              OnlyCollaborations: Show only collaborations by that artist.</param> 
        /// <param name="childVoicebanks">Include child voicebanks, if the artist being filtered by has any.</param> 
        /// <param name="includeMembers">Include members of groups. This applies if {artistId} is a group.</param> 
        /// <param name="barcode">Filter by album barcode (optional).</param> 
        /// <param name="status">Filter by entry status (optional).</param> 
        /// <param name="releaseDateAfter">Filter by albums whose release date is after this date (inclusive).</param> 
        /// <param name="releaseDateBefore">Filter by albums whose release date is before this date (exclusive).</param> 
        /// <param name="advancedFilters">List of advanced filters (optional).</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="sort">Sort rule (optional, defaults to Name).               Possible values are None, Name, ReleaseDate, ReleaseDateWithNulls, AdditionDate, RatingAverage, RatingTotal, NameThenReleaseDate.</param> 
        /// <param name="preferAccurateMatches">Whether the search should prefer accurate matches.               If this is true, entries that match by prefix will be moved first, instead of being sorted alphabetically.              Requires a text query. Does not support pagination.              This is mostly useful for autocomplete boxes.</param> 
        /// <param name="deleted">Whether to search for deleted entries.              If this is true, only deleted entries will be returned.              If this is false (default), deleted entries are not returned.</param> 
        /// <param name="nameMatchMode">Match mode for artist name (optional, defaults to Exact).</param> 
        /// <param name="fields">Optional fields (optional). Possible values are artists, names, pvs, tags, tracks, webLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>PartialFindResultAlbumForApiContract</returns>            
        public PartialFindResultAlbumForApiContract AlbumApiGetList (string query, string discTypes, List<string> tagName, List<int?> tagId, bool? childTags, List<int?> artistId, string artistParticipationStatus, bool? childVoicebanks, bool? includeMembers, string barcode, string status, DateTime? releaseDateAfter, DateTime? releaseDateBefore, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, bool? preferAccurateMatches, bool? deleted, string nameMatchMode, string fields, string lang)
        {
            
    
            var path = "/api/albums";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (discTypes != null) queryParams.Add("discTypes", ApiClient.ParameterToString(discTypes)); // query parameter
 if (tagName != null) queryParams.Add("tagName", ApiClient.ParameterToString(tagName)); // query parameter
 if (tagId != null) queryParams.Add("tagId", ApiClient.ParameterToString(tagId)); // query parameter
 if (childTags != null) queryParams.Add("childTags", ApiClient.ParameterToString(childTags)); // query parameter
 if (artistId != null) queryParams.Add("artistId", ApiClient.ParameterToString(artistId)); // query parameter
 if (artistParticipationStatus != null) queryParams.Add("artistParticipationStatus", ApiClient.ParameterToString(artistParticipationStatus)); // query parameter
 if (childVoicebanks != null) queryParams.Add("childVoicebanks", ApiClient.ParameterToString(childVoicebanks)); // query parameter
 if (includeMembers != null) queryParams.Add("includeMembers", ApiClient.ParameterToString(includeMembers)); // query parameter
 if (barcode != null) queryParams.Add("barcode", ApiClient.ParameterToString(barcode)); // query parameter
 if (status != null) queryParams.Add("status", ApiClient.ParameterToString(status)); // query parameter
 if (releaseDateAfter != null) queryParams.Add("releaseDateAfter", ApiClient.ParameterToString(releaseDateAfter)); // query parameter
 if (releaseDateBefore != null) queryParams.Add("releaseDateBefore", ApiClient.ParameterToString(releaseDateBefore)); // query parameter
 if (advancedFilters != null) queryParams.Add("advancedFilters", ApiClient.ParameterToString(advancedFilters)); // query parameter
 if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
 if (sort != null) queryParams.Add("sort", ApiClient.ParameterToString(sort)); // query parameter
 if (preferAccurateMatches != null) queryParams.Add("preferAccurateMatches", ApiClient.ParameterToString(preferAccurateMatches)); // query parameter
 if (deleted != null) queryParams.Add("deleted", ApiClient.ParameterToString(deleted)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultAlbumForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultAlbumForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of album names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param> 
        /// <param name="nameMatchMode">Name match mode.</param> 
        /// <param name="maxResults">Maximum number of results.</param> 
        /// <returns>List&lt;string&gt;</returns>            
        public List<string> AlbumApiGetNames (string query, string nameMatchMode, int? maxResults)
        {
            
    
            var path = "/api/albums/names";
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
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetNames: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetNames: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
    
        /// <summary>
        /// Gets list of upcoming or recent albums, same as front page. Output is cached for 1 hour.
        /// </summary>
        /// <param name="languagePreference"></param> 
        /// <param name="fields"></param> 
        /// <returns>List&lt;AlbumForApiContract&gt;</returns>            
        public List<AlbumForApiContract> AlbumApiGetNewAlbums (string languagePreference, string fields)
        {
            
    
            var path = "/api/albums/new";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (languagePreference != null) queryParams.Add("languagePreference", ApiClient.ParameterToString(languagePreference)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetNewAlbums: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetNewAlbums: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<AlbumForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<AlbumForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Gets an album by Id. 
        /// </summary>
        /// <param name="id">Album Id (required).</param> 
        /// <param name="fields">Optional fields (optional). Possible values are artists, names, pvs, tags, tracks, webLinks.</param> 
        /// <param name="songFields">Optional fields for tracks, if included (optional).</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>AlbumForApiContract</returns>            
        public AlbumForApiContract AlbumApiGetOne (int? id, string fields, string songFields, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling AlbumApiGetOne");
            
    
            var path = "/api/albums/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (songFields != null) queryParams.Add("songFields", ApiClient.ParameterToString(songFields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetOne: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetOne: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AlbumForApiContract) ApiClient.Deserialize(response.Content, typeof(AlbumForApiContract), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="languageCode"></param> 
        /// <returns>List&lt;AlbumReviewContract&gt;</returns>            
        public List<AlbumReviewContract> AlbumApiGetReviews (int? id, string languageCode)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling AlbumApiGetReviews");
            
    
            var path = "/api/albums/{id}/reviews";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (languageCode != null) queryParams.Add("languageCode", ApiClient.ParameterToString(languageCode)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetReviews: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetReviews: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<AlbumReviewContract>) ApiClient.Deserialize(response.Content, typeof(List<AlbumReviewContract>), response.Headers);
        }
    
        /// <summary>
        /// Gets list of top rated albums, same as front page. Output is cached for 1 hour.
        /// </summary>
        /// <param name="ignoreIds"></param> 
        /// <param name="languagePreference"></param> 
        /// <param name="fields"></param> 
        /// <returns>List&lt;AlbumForApiContract&gt;</returns>            
        public List<AlbumForApiContract> AlbumApiGetTopAlbums (List<int?> ignoreIds, string languagePreference, string fields)
        {
            
            // verify the required parameter 'ignoreIds' is set
            if (ignoreIds == null) throw new ApiException(400, "Missing required parameter 'ignoreIds' when calling AlbumApiGetTopAlbums");
            
    
            var path = "/api/albums/top";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (languagePreference != null) queryParams.Add("languagePreference", ApiClient.ParameterToString(languagePreference)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
                                    postBody = ApiClient.Serialize(ignoreIds); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetTopAlbums: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetTopAlbums: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<AlbumForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<AlbumForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Gets tracks for an album. 
        /// </summary>
        /// <param name="id">Album ID (required).</param> 
        /// <param name="fields">List of optional fields (optional). Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>List&lt;SongInAlbumForApiContract&gt;</returns>            
        public List<SongInAlbumForApiContract> AlbumApiGetTracks (int? id, string fields, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling AlbumApiGetTracks");
            
    
            var path = "/api/albums/{id}/tracks";
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
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetTracks: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetTracks: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<SongInAlbumForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<SongInAlbumForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Gets tracks for an album formatted using the CSV format string. 
        /// </summary>
        /// <param name="id">Album ID.</param> 
        /// <param name="field">Field to be included, for example \&quot;featvocalists\&quot; or \&quot;url\&quot;. Can be specified multiple times.</param> 
        /// <param name="lang">Language preference.</param> 
        /// <returns>List&lt;Dictionary&lt;string, string&gt;&gt;</returns>            
        public List<Dictionary<string, string>> AlbumApiGetTracksFormatted (int? id, List<string> field, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling AlbumApiGetTracksFormatted");
            
    
            var path = "/api/albums/{id}/tracks/fields";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (field != null) queryParams.Add("field", ApiClient.ParameterToString(field)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetTracksFormatted: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetTracksFormatted: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<Dictionary<string, string>>) ApiClient.Deserialize(response.Content, typeof(List<Dictionary<string, string>>), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="languagePreference"></param> 
        /// <returns>List&lt;AlbumForUserForApiContract&gt;</returns>            
        public List<AlbumForUserForApiContract> AlbumApiGetUserCollections (int? id, string languagePreference)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling AlbumApiGetUserCollections");
            
    
            var path = "/api/albums/{id}/user-collections";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (languagePreference != null) queryParams.Add("languagePreference", ApiClient.ParameterToString(languagePreference)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetUserCollections: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiGetUserCollections: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<AlbumForUserForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<AlbumForUserForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param> 
        /// <param name="contract">New comment data. Only message can be edited.</param> 
        /// <returns></returns>            
        public void AlbumApiPostEditComment (int? commentId, CommentForApiContract contract)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling AlbumApiPostEditComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling AlbumApiPostEditComment");
            
    
            var path = "/api/albums/comments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiPostEditComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiPostEditComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="id">ID of the album for which to create the comment.</param> 
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param> 
        /// <returns>CommentForApiContract</returns>            
        public CommentForApiContract AlbumApiPostNewComment (int? id, CommentForApiContract contract)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling AlbumApiPostNewComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling AlbumApiPostNewComment");
            
    
            var path = "/api/albums/{id}/comments";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiPostNewComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiPostNewComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CommentForApiContract) ApiClient.Deserialize(response.Content, typeof(CommentForApiContract), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="id"></param> 
        /// <param name="reviewContract"></param> 
        /// <returns>AlbumReviewContract</returns>            
        public AlbumReviewContract AlbumApiPostReview (int? id, AlbumReviewContract reviewContract)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling AlbumApiPostReview");
            
            // verify the required parameter 'reviewContract' is set
            if (reviewContract == null) throw new ApiException(400, "Missing required parameter 'reviewContract' when calling AlbumApiPostReview");
            
    
            var path = "/api/albums/{id}/reviews";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(reviewContract); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiPostReview: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AlbumApiPostReview: " + response.ErrorMessage, response.ErrorMessage);
    
            return (AlbumReviewContract) ApiClient.Deserialize(response.Content, typeof(AlbumReviewContract), response.Headers);
        }
    
    }
}
