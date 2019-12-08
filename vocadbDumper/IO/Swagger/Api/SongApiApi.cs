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
    public interface ISongApiApi
    {
        /// <summary>
        /// Deletes a song. 
        /// </summary>
        /// <param name="id">ID of the song to be deleted.</param>
        /// <param name="notes">Notes.</param>
        /// <returns></returns>
        void SongApiDelete (int? id, string notes);
        /// <summary>
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param>
        /// <returns></returns>
        void SongApiDeleteComment (int? commentId);
        /// <summary>
        /// Gets a song by Id. 
        /// </summary>
        /// <param name="id">Song Id (required).</param>
        /// <param name="fields">List of optional fields (optional).               Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>SongForApiContract</returns>
        SongForApiContract SongApiGetById (int? id, string fields, string lang);
        /// <summary>
        /// Gets a song by PV. 
        /// </summary>
        /// <param name="pvService">PV service (required). Possible values are NicoNicoDouga, Youtube, SoundCloud, Vimeo, Piapro, Bilibili.</param>
        /// <param name="pvId">PV Id (required). For example sm123456.</param>
        /// <param name="fields">List of optional fields (optional). Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>SongForApiContract</returns>
        SongForApiContract SongApiGetByPV (string pvService, string pvId, string fields, string lang);
        /// <summary>
        /// Gets a list of comments for a song. Pagination and sorting might be added later.
        /// </summary>
        /// <param name="id">ID of the song whose comments to load.</param>
        /// <returns>List&lt;CommentForApiContract&gt;</returns>
        List<CommentForApiContract> SongApiGetComments (int? id);
        /// <summary>
        /// Gets derived (alternate versions) of a song. Pagination and sorting might be added later.
        /// </summary>
        /// <param name="id">Song Id (required).</param>
        /// <param name="fields">List of optional fields (optional).               Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>List&lt;SongForApiContract&gt;</returns>
        List<SongForApiContract> SongApiGetDerived (int? id, string fields, string lang);
        /// <summary>
        /// Gets list of highlighted songs, same as front page. Output is cached for 1 hour.
        /// </summary>
        /// <param name="languagePreference"></param>
        /// <param name="fields"></param>
        /// <returns>List&lt;SongForApiContract&gt;</returns>
        List<SongForApiContract> SongApiGetHighlightedSongs (string languagePreference, string fields);
        /// <summary>
        /// Find songs. 
        /// </summary>
        /// <param name="query">Song name query (optional).</param>
        /// <param name="songTypes">Filtered song types (optional).               Possible values are Original, Remaster, Remix, Cover, Instrumental, Mashup, MusicPV, DramaPV, Other.</param>
        /// <param name="afterDate">Filter by songs published after this date (inclusive).</param>
        /// <param name="beforeDate">Filter by songs published before this date (exclusive).</param>
        /// <param name="tagName">Filter by one or more tag names (optional).</param>
        /// <param name="tagId">Filter by one or more tag Ids (optional).</param>
        /// <param name="childTags">Include child tags, if the tags being filtered by have any.</param>
        /// <param name="artistId">Filter by artist Id.</param>
        /// <param name="artistParticipationStatus">Filter by artist participation status. Only valid if artistId is specified.              Everything (default): Show all songs by that artist (no filter).              OnlyMainAlbums: Show only main songs by that artist.              OnlyCollaborations: Show only collaborations by that artist.</param>
        /// <param name="childVoicebanks">Include child voicebanks, if the artist being filtered by has any.</param>
        /// <param name="includeMembers">Include members of groups. This applies if {artistId} is a group.</param>
        /// <param name="onlyWithPvs">Whether to only include songs with at least one PV.</param>
        /// <param name="pvServices">Filter by one or more PV services (separated by commas). The song will pass the filter if it has a PV for any of the matched services.</param>
        /// <param name="since">Allow only entries that have been created at most this many hours ago. By default there is no filtering.</param>
        /// <param name="minScore">Minimum rating score. Optional.</param>
        /// <param name="userCollectionId">Filter by user&#39;s rated songs. By default there is no filtering.</param>
        /// <param name="releaseEventId">Filter by release event. By default there is no filtering.</param>
        /// <param name="status">Filter by entry status (optional).</param>
        /// <param name="advancedFilters">List of advanced filters (optional).</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, AdditionDate, FavoritedTimes, RatingScore.</param>
        /// <param name="preferAccurateMatches">Whether the search should prefer accurate matches.               If this is true, entries that match by prefix will be moved first, instead of being sorted alphabetically.              Requires a text query. Does not support pagination.              This is mostly useful for autocomplete boxes.</param>
        /// <param name="nameMatchMode">Match mode for song name (optional, defaults to Exact).</param>
        /// <param name="fields">List of optional fields (optional). Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>PartialFindResultSongForApiContract</returns>
        PartialFindResultSongForApiContract SongApiGetList (string query, string songTypes, DateTime? afterDate, DateTime? beforeDate, List<string> tagName, List<int?> tagId, bool? childTags, List<int?> artistId, string artistParticipationStatus, bool? childVoicebanks, bool? includeMembers, bool? onlyWithPvs, string pvServices, int? since, int? minScore, int? userCollectionId, int? releaseEventId, string status, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, bool? preferAccurateMatches, string nameMatchMode, string fields, string lang);
        /// <summary>
        /// Gets lyrics by ID. Output is cached. Specify song version as parameter to refresh.
        /// </summary>
        /// <param name="lyricsId">Lyrics ID.</param>
        /// <returns>LyricsForSongContract</returns>
        LyricsForSongContract SongApiGetLyrics (int? lyricsId);
        /// <summary>
        /// Gets a list of song names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param>
        /// <param name="nameMatchMode">Name match mode.</param>
        /// <param name="maxResults">Maximum number of results.</param>
        /// <returns>List&lt;string&gt;</returns>
        List<string> SongApiGetNames (string query, string nameMatchMode, int? maxResults);
        /// <summary>
        /// Get ratings for a song. The result includes ratings and user information.              For users who have requested not to make their ratings public, the user will be empty.
        /// </summary>
        /// <param name="id">Song ID.</param>
        /// <param name="userFields">Optional fields for the users.</param>
        /// <param name="lang">Content language preference.</param>
        /// <returns>List&lt;RatedSongForUserForApiContract&gt;</returns>
        List<RatedSongForUserForApiContract> SongApiGetRatings (int? id, string userFields, string lang);
        /// <summary>
        /// Gets related songs. 
        /// </summary>
        /// <param name="id">Song whose related songs are to be queried.</param>
        /// <param name="fields">Optional song fields.</param>
        /// <param name="lang">Content language preference.</param>
        /// <returns>RelatedSongsContract</returns>
        RelatedSongsContract SongApiGetRelated (int? id, string fields, string lang);
        /// <summary>
        /// Gets top rated songs. 
        /// </summary>
        /// <param name="durationHours">Duration in hours from which to get songs.</param>
        /// <param name="startDate">Lower bound of the date. Optional.</param>
        /// <param name="filterBy">Filtering mode.</param>
        /// <param name="vocalist">Vocalist selection.</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional).</param>
        /// <param name="fields">Optional song fields to load.</param>
        /// <param name="languagePreference">Language preference.</param>
        /// <returns>List&lt;SongForApiContract&gt;</returns>
        List<SongForApiContract> SongApiGetTopSongs (int? durationHours, DateTime? startDate, string filterBy, string vocalist, int? maxResults, string fields, string languagePreference);
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param>
        /// <param name="contract">New comment data. Only message can be edited.</param>
        /// <returns></returns>
        void SongApiPostEditComment (int? commentId, CommentForApiContract contract);
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="id">ID of the song for which to create the comment.</param>
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param>
        /// <returns>CommentForApiContract</returns>
        CommentForApiContract SongApiPostNewComment (int? id, CommentForApiContract contract);
        /// <summary>
        /// Add or update rating for a specific song, for the currently logged in user. If the user has already rated the song, any previous rating is replaced.              Authorization cookie must be included.              This API supports CORS.
        /// </summary>
        /// <param name="id">ID of the song to be rated.</param>
        /// <param name="rating">Rating to be given. Possible values are Nothing, Like, Favorite.</param>
        /// <returns></returns>
        void SongApiPostRating (int? id, SongRatingContract rating);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class SongApiApi : ISongApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SongApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public SongApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="SongApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public SongApiApi(String basePath)
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
        /// Deletes a song. 
        /// </summary>
        /// <param name="id">ID of the song to be deleted.</param> 
        /// <param name="notes">Notes.</param> 
        /// <returns></returns>            
        public void SongApiDelete (int? id, string notes)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SongApiDelete");
            
    
            var path = "/api/songs/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param> 
        /// <returns></returns>            
        public void SongApiDeleteComment (int? commentId)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling SongApiDeleteComment");
            
    
            var path = "/api/songs/comments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiDeleteComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiDeleteComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Gets a song by Id. 
        /// </summary>
        /// <param name="id">Song Id (required).</param> 
        /// <param name="fields">List of optional fields (optional).               Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>SongForApiContract</returns>            
        public SongForApiContract SongApiGetById (int? id, string fields, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SongApiGetById");
            
    
            var path = "/api/songs/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SongForApiContract) ApiClient.Deserialize(response.Content, typeof(SongForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a song by PV. 
        /// </summary>
        /// <param name="pvService">PV service (required). Possible values are NicoNicoDouga, Youtube, SoundCloud, Vimeo, Piapro, Bilibili.</param> 
        /// <param name="pvId">PV Id (required). For example sm123456.</param> 
        /// <param name="fields">List of optional fields (optional). Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>SongForApiContract</returns>            
        public SongForApiContract SongApiGetByPV (string pvService, string pvId, string fields, string lang)
        {
            
            // verify the required parameter 'pvService' is set
            if (pvService == null) throw new ApiException(400, "Missing required parameter 'pvService' when calling SongApiGetByPV");
            
            // verify the required parameter 'pvId' is set
            if (pvId == null) throw new ApiException(400, "Missing required parameter 'pvId' when calling SongApiGetByPV");
            
    
            var path = "/api/songs/byPv";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (pvService != null) queryParams.Add("pvService", ApiClient.ParameterToString(pvService)); // query parameter
 if (pvId != null) queryParams.Add("pvId", ApiClient.ParameterToString(pvId)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetByPV: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetByPV: " + response.ErrorMessage, response.ErrorMessage);
    
            return (SongForApiContract) ApiClient.Deserialize(response.Content, typeof(SongForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of comments for a song. Pagination and sorting might be added later.
        /// </summary>
        /// <param name="id">ID of the song whose comments to load.</param> 
        /// <returns>List&lt;CommentForApiContract&gt;</returns>            
        public List<CommentForApiContract> SongApiGetComments (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SongApiGetComments");
            
    
            var path = "/api/songs/{id}/comments";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetComments: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetComments: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<CommentForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<CommentForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Gets derived (alternate versions) of a song. Pagination and sorting might be added later.
        /// </summary>
        /// <param name="id">Song Id (required).</param> 
        /// <param name="fields">List of optional fields (optional).               Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>List&lt;SongForApiContract&gt;</returns>            
        public List<SongForApiContract> SongApiGetDerived (int? id, string fields, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SongApiGetDerived");
            
    
            var path = "/api/songs/{id}/derived";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetDerived: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetDerived: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<SongForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<SongForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Gets list of highlighted songs, same as front page. Output is cached for 1 hour.
        /// </summary>
        /// <param name="languagePreference"></param> 
        /// <param name="fields"></param> 
        /// <returns>List&lt;SongForApiContract&gt;</returns>            
        public List<SongForApiContract> SongApiGetHighlightedSongs (string languagePreference, string fields)
        {
            
    
            var path = "/api/songs/highlighted";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetHighlightedSongs: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetHighlightedSongs: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<SongForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<SongForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Find songs. 
        /// </summary>
        /// <param name="query">Song name query (optional).</param> 
        /// <param name="songTypes">Filtered song types (optional).               Possible values are Original, Remaster, Remix, Cover, Instrumental, Mashup, MusicPV, DramaPV, Other.</param> 
        /// <param name="afterDate">Filter by songs published after this date (inclusive).</param> 
        /// <param name="beforeDate">Filter by songs published before this date (exclusive).</param> 
        /// <param name="tagName">Filter by one or more tag names (optional).</param> 
        /// <param name="tagId">Filter by one or more tag Ids (optional).</param> 
        /// <param name="childTags">Include child tags, if the tags being filtered by have any.</param> 
        /// <param name="artistId">Filter by artist Id.</param> 
        /// <param name="artistParticipationStatus">Filter by artist participation status. Only valid if artistId is specified.              Everything (default): Show all songs by that artist (no filter).              OnlyMainAlbums: Show only main songs by that artist.              OnlyCollaborations: Show only collaborations by that artist.</param> 
        /// <param name="childVoicebanks">Include child voicebanks, if the artist being filtered by has any.</param> 
        /// <param name="includeMembers">Include members of groups. This applies if {artistId} is a group.</param> 
        /// <param name="onlyWithPvs">Whether to only include songs with at least one PV.</param> 
        /// <param name="pvServices">Filter by one or more PV services (separated by commas). The song will pass the filter if it has a PV for any of the matched services.</param> 
        /// <param name="since">Allow only entries that have been created at most this many hours ago. By default there is no filtering.</param> 
        /// <param name="minScore">Minimum rating score. Optional.</param> 
        /// <param name="userCollectionId">Filter by user&#39;s rated songs. By default there is no filtering.</param> 
        /// <param name="releaseEventId">Filter by release event. By default there is no filtering.</param> 
        /// <param name="status">Filter by entry status (optional).</param> 
        /// <param name="advancedFilters">List of advanced filters (optional).</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, AdditionDate, FavoritedTimes, RatingScore.</param> 
        /// <param name="preferAccurateMatches">Whether the search should prefer accurate matches.               If this is true, entries that match by prefix will be moved first, instead of being sorted alphabetically.              Requires a text query. Does not support pagination.              This is mostly useful for autocomplete boxes.</param> 
        /// <param name="nameMatchMode">Match mode for song name (optional, defaults to Exact).</param> 
        /// <param name="fields">List of optional fields (optional). Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>PartialFindResultSongForApiContract</returns>            
        public PartialFindResultSongForApiContract SongApiGetList (string query, string songTypes, DateTime? afterDate, DateTime? beforeDate, List<string> tagName, List<int?> tagId, bool? childTags, List<int?> artistId, string artistParticipationStatus, bool? childVoicebanks, bool? includeMembers, bool? onlyWithPvs, string pvServices, int? since, int? minScore, int? userCollectionId, int? releaseEventId, string status, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, bool? preferAccurateMatches, string nameMatchMode, string fields, string lang)
        {
            
    
            var path = "/api/songs";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (songTypes != null) queryParams.Add("songTypes", ApiClient.ParameterToString(songTypes)); // query parameter
 if (afterDate != null) queryParams.Add("afterDate", ApiClient.ParameterToString(afterDate)); // query parameter
 if (beforeDate != null) queryParams.Add("beforeDate", ApiClient.ParameterToString(beforeDate)); // query parameter
 if (tagName != null) queryParams.Add("tagName", ApiClient.ParameterToString(tagName)); // query parameter
 if (tagId != null) queryParams.Add("tagId", ApiClient.ParameterToString(tagId)); // query parameter
 if (childTags != null) queryParams.Add("childTags", ApiClient.ParameterToString(childTags)); // query parameter
 if (artistId != null) queryParams.Add("artistId", ApiClient.ParameterToString(artistId)); // query parameter
 if (artistParticipationStatus != null) queryParams.Add("artistParticipationStatus", ApiClient.ParameterToString(artistParticipationStatus)); // query parameter
 if (childVoicebanks != null) queryParams.Add("childVoicebanks", ApiClient.ParameterToString(childVoicebanks)); // query parameter
 if (includeMembers != null) queryParams.Add("includeMembers", ApiClient.ParameterToString(includeMembers)); // query parameter
 if (onlyWithPvs != null) queryParams.Add("onlyWithPvs", ApiClient.ParameterToString(onlyWithPvs)); // query parameter
 if (pvServices != null) queryParams.Add("pvServices", ApiClient.ParameterToString(pvServices)); // query parameter
 if (since != null) queryParams.Add("since", ApiClient.ParameterToString(since)); // query parameter
 if (minScore != null) queryParams.Add("minScore", ApiClient.ParameterToString(minScore)); // query parameter
 if (userCollectionId != null) queryParams.Add("userCollectionId", ApiClient.ParameterToString(userCollectionId)); // query parameter
 if (releaseEventId != null) queryParams.Add("releaseEventId", ApiClient.ParameterToString(releaseEventId)); // query parameter
 if (status != null) queryParams.Add("status", ApiClient.ParameterToString(status)); // query parameter
 if (advancedFilters != null) queryParams.Add("advancedFilters", ApiClient.ParameterToString(advancedFilters)); // query parameter
 if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
 if (sort != null) queryParams.Add("sort", ApiClient.ParameterToString(sort)); // query parameter
 if (preferAccurateMatches != null) queryParams.Add("preferAccurateMatches", ApiClient.ParameterToString(preferAccurateMatches)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultSongForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultSongForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets lyrics by ID. Output is cached. Specify song version as parameter to refresh.
        /// </summary>
        /// <param name="lyricsId">Lyrics ID.</param> 
        /// <returns>LyricsForSongContract</returns>            
        public LyricsForSongContract SongApiGetLyrics (int? lyricsId)
        {
            
            // verify the required parameter 'lyricsId' is set
            if (lyricsId == null) throw new ApiException(400, "Missing required parameter 'lyricsId' when calling SongApiGetLyrics");
            
    
            var path = "/api/songs/lyrics/{lyricsId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "lyricsId" + "}", ApiClient.ParameterToString(lyricsId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetLyrics: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetLyrics: " + response.ErrorMessage, response.ErrorMessage);
    
            return (LyricsForSongContract) ApiClient.Deserialize(response.Content, typeof(LyricsForSongContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of song names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param> 
        /// <param name="nameMatchMode">Name match mode.</param> 
        /// <param name="maxResults">Maximum number of results.</param> 
        /// <returns>List&lt;string&gt;</returns>            
        public List<string> SongApiGetNames (string query, string nameMatchMode, int? maxResults)
        {
            
    
            var path = "/api/songs/names";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetNames: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetNames: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
    
        /// <summary>
        /// Get ratings for a song. The result includes ratings and user information.              For users who have requested not to make their ratings public, the user will be empty.
        /// </summary>
        /// <param name="id">Song ID.</param> 
        /// <param name="userFields">Optional fields for the users.</param> 
        /// <param name="lang">Content language preference.</param> 
        /// <returns>List&lt;RatedSongForUserForApiContract&gt;</returns>            
        public List<RatedSongForUserForApiContract> SongApiGetRatings (int? id, string userFields, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SongApiGetRatings");
            
            // verify the required parameter 'userFields' is set
            if (userFields == null) throw new ApiException(400, "Missing required parameter 'userFields' when calling SongApiGetRatings");
            
    
            var path = "/api/songs/{id}/ratings";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (userFields != null) queryParams.Add("userFields", ApiClient.ParameterToString(userFields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetRatings: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetRatings: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<RatedSongForUserForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<RatedSongForUserForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Gets related songs. 
        /// </summary>
        /// <param name="id">Song whose related songs are to be queried.</param> 
        /// <param name="fields">Optional song fields.</param> 
        /// <param name="lang">Content language preference.</param> 
        /// <returns>RelatedSongsContract</returns>            
        public RelatedSongsContract SongApiGetRelated (int? id, string fields, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SongApiGetRelated");
            
    
            var path = "/api/songs/{id}/related";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetRelated: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetRelated: " + response.ErrorMessage, response.ErrorMessage);
    
            return (RelatedSongsContract) ApiClient.Deserialize(response.Content, typeof(RelatedSongsContract), response.Headers);
        }
    
        /// <summary>
        /// Gets top rated songs. 
        /// </summary>
        /// <param name="durationHours">Duration in hours from which to get songs.</param> 
        /// <param name="startDate">Lower bound of the date. Optional.</param> 
        /// <param name="filterBy">Filtering mode.</param> 
        /// <param name="vocalist">Vocalist selection.</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional).</param> 
        /// <param name="fields">Optional song fields to load.</param> 
        /// <param name="languagePreference">Language preference.</param> 
        /// <returns>List&lt;SongForApiContract&gt;</returns>            
        public List<SongForApiContract> SongApiGetTopSongs (int? durationHours, DateTime? startDate, string filterBy, string vocalist, int? maxResults, string fields, string languagePreference)
        {
            
    
            var path = "/api/songs/top-rated";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (durationHours != null) queryParams.Add("durationHours", ApiClient.ParameterToString(durationHours)); // query parameter
 if (startDate != null) queryParams.Add("startDate", ApiClient.ParameterToString(startDate)); // query parameter
 if (filterBy != null) queryParams.Add("filterBy", ApiClient.ParameterToString(filterBy)); // query parameter
 if (vocalist != null) queryParams.Add("vocalist", ApiClient.ParameterToString(vocalist)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (languagePreference != null) queryParams.Add("languagePreference", ApiClient.ParameterToString(languagePreference)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetTopSongs: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiGetTopSongs: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<SongForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<SongForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param> 
        /// <param name="contract">New comment data. Only message can be edited.</param> 
        /// <returns></returns>            
        public void SongApiPostEditComment (int? commentId, CommentForApiContract contract)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling SongApiPostEditComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling SongApiPostEditComment");
            
    
            var path = "/api/songs/comments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiPostEditComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiPostEditComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="id">ID of the song for which to create the comment.</param> 
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param> 
        /// <returns>CommentForApiContract</returns>            
        public CommentForApiContract SongApiPostNewComment (int? id, CommentForApiContract contract)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SongApiPostNewComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling SongApiPostNewComment");
            
    
            var path = "/api/songs/{id}/comments";
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
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiPostNewComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiPostNewComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CommentForApiContract) ApiClient.Deserialize(response.Content, typeof(CommentForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Add or update rating for a specific song, for the currently logged in user. If the user has already rated the song, any previous rating is replaced.              Authorization cookie must be included.              This API supports CORS.
        /// </summary>
        /// <param name="id">ID of the song to be rated.</param> 
        /// <param name="rating">Rating to be given. Possible values are Nothing, Like, Favorite.</param> 
        /// <returns></returns>            
        public void SongApiPostRating (int? id, SongRatingContract rating)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling SongApiPostRating");
            
            // verify the required parameter 'rating' is set
            if (rating == null) throw new ApiException(400, "Missing required parameter 'rating' when calling SongApiPostRating");
            
    
            var path = "/api/songs/{id}/ratings";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(rating); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiPostRating: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling SongApiPostRating: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}
