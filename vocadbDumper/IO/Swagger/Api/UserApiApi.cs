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
    public interface IUserApiApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        void UserApiDeleteFollowedTag (int? tagId);
        /// <summary>
        /// Deletes a list of user messages. 
        /// </summary>
        /// <param name="id">ID of the user whose messages to delete.</param>
        /// <param name="messageId">IDs of messages.</param>
        /// <returns></returns>
        void UserApiDeleteMessages (int? id, List<int?> messageId);
        /// <summary>
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param>
        /// <returns></returns>
        void UserApiDeleteProfileComment (int? commentId);
        /// <summary>
        /// Gets a list of albums in a user&#39;s collection. This includes albums that have been rated by the user as well as albums that the user has bought or wishlisted.              Note that the user might have set his album ownership status and media type as private, in which case those properties are not included.
        /// </summary>
        /// <param name="id">ID of the user whose albums are to be browsed.</param>
        /// <param name="query">Album name query (optional).</param>
        /// <param name="tagId">Filter by tag Id (optional).</param>
        /// <param name="tag">Filter by tag (optional).</param>
        /// <param name="artistId">Filter by album artist (optional).</param>
        /// <param name="purchaseStatuses">Filter by a comma-separated list of purchase statuses (optional). Possible values are Nothing, Wishlisted, Ordered, Owned, and all combinations of these.</param>
        /// <param name="releaseEventId">Filter by release event. Optional.</param>
        /// <param name="albumTypes">Filter by album type (optional).</param>
        /// <param name="advancedFilters">List of advanced filters (optional).</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, ReleaseDate, AdditionDate, RatingAverage, RatingTotal, CollectionCount.</param>
        /// <param name="nameMatchMode">Match mode for album name (optional, defaults to Auto).</param>
        /// <param name="fields">List of optional fields (optional). Possible values are Artists, MainPicture, Names, PVs, Tags, WebLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>PartialFindResultAlbumForUserForApiContract</returns>
        PartialFindResultAlbumForUserForApiContract UserApiGetAlbumCollection (int? id, string query, int? tagId, string tag, int? artistId, string purchaseStatuses, int? releaseEventId, string albumTypes, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, string nameMatchMode, string fields, string lang);
        /// <summary>
        /// Gets information about the currently logged in user. Requires login information.              This API supports CORS, and is restricted to specific origins.
        /// </summary>
        /// <param name="fields">Optional fields.</param>
        /// <returns>UserForApiContract</returns>
        UserForApiContract UserApiGetCurrent (string fields);
        /// <summary>
        /// Gets a list of events a user has subscribed to. 
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <param name="relationshipType">Type of event subscription.</param>
        /// <returns>List&lt;ReleaseEventForApiContract&gt;</returns>
        List<ReleaseEventForApiContract> UserApiGetEvents (int? id, string relationshipType);
        /// <summary>
        /// Gets a list of artists followed by a user. 
        /// </summary>
        /// <param name="id">ID of the user whose followed artists are to be browsed.</param>
        /// <param name="query">Artist name query (optional).</param>
        /// <param name="artistType">Filter by artist type.</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, AdditionDate, AdditionDateAsc.</param>
        /// <param name="nameMatchMode">Match mode for artist name (optional, defaults to Auto).</param>
        /// <param name="fields">List of optional fields (optional). Possible values are Description, Groups, Members, Names, Tags, WebLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>PartialFindResultArtistForUserForApiContract</returns>
        PartialFindResultArtistForUserForApiContract UserApiGetFollowedArtists (int? id, string query, string artistType, int? start, int? maxResults, bool? getTotalCount, string sort, string nameMatchMode, string fields, string lang);
        /// <summary>
        /// Gets a list of users. 
        /// </summary>
        /// <param name="query">User name query (optional).</param>
        /// <param name="groups">Filter by user group. Only one value supported for now. Optional.</param>
        /// <param name="joinDateAfter">Filter by users who joined after this date (inclusive).</param>
        /// <param name="joinDateBefore">Filter by users who joined before this date (exclusive).</param>
        /// <param name="nameMatchMode">Name match mode.</param>
        /// <param name="start">Index of the first entry to be loaded.</param>
        /// <param name="maxResults">Maximum number of results to be loaded.</param>
        /// <param name="getTotalCount">Whether to get total number of results.</param>
        /// <param name="sort">Sort rule.</param>
        /// <param name="includeDisabled">Whether to include disabled user accounts.</param>
        /// <param name="onlyVerified">Whether to only include verified artists.</param>
        /// <param name="knowsLanguage">Filter by known language (optional). This is the ISO 639-1 language code, for example \&quot;en\&quot; or \&quot;zh\&quot;.</param>
        /// <param name="fields">Optional fields. Possible values are None and MainPicture. Optional.</param>
        /// <returns>PartialFindResultUserForApiContract</returns>
        PartialFindResultUserForApiContract UserApiGetList (string query, string groups, DateTime? joinDateAfter, DateTime? joinDateBefore, string nameMatchMode, int? start, int? maxResults, bool? getTotalCount, string sort, bool? includeDisabled, bool? onlyVerified, string knowsLanguage, string fields);
        /// <summary>
        /// Gets a user message. The message will be marked as read.              User can only load messages from their own inbox.
        /// </summary>
        /// <param name="messageId">ID of the message.</param>
        /// <returns>UserMessageContract</returns>
        UserMessageContract UserApiGetMessage (int? messageId);
        /// <summary>
        /// Gets a list of messages. 
        /// </summary>
        /// <param name="id">User ID. Must be the currently logged in user (loading messages for another user is not allowed).</param>
        /// <param name="inbox">Type of inbox. Possible values are Nothing (load all, default), Received, Sent, Notifications.</param>
        /// <param name="unread">Whether to only load unread messages. Loading unread messages is only possible for received messages and notifications (not sent messages).</param>
        /// <param name="anotherUserId">Filter by id of the other user (either sender or receiver).</param>
        /// <param name="start">Index of the first entry to be loaded.</param>
        /// <param name="maxResults">Maximum number of results to be loaded.</param>
        /// <param name="getTotalCount">Whether to get total number of results.</param>
        /// <returns>PartialFindResultUserMessageContract</returns>
        PartialFindResultUserMessageContract UserApiGetMessages (int? id, string inbox, bool? unread, int? anotherUserId, int? start, int? maxResults, bool? getTotalCount);
        /// <summary>
        /// Gets a list of user names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param>
        /// <param name="nameMatchMode">Name match mode. Words is treated the same as Partial.</param>
        /// <param name="maxResults">Maximum number of results.</param>
        /// <param name="includeDisabled">Whether to include disabled user accounts. If false, disabled accounts are excluded.</param>
        /// <returns>List&lt;string&gt;</returns>
        List<string> UserApiGetNames (string query, string nameMatchMode, int? maxResults, bool? includeDisabled);
        /// <summary>
        /// Gets user by ID. 
        /// </summary>
        /// <param name="id">User ID.</param>
        /// <param name="fields">Optional fields.</param>
        /// <returns>UserForApiContract</returns>
        UserForApiContract UserApiGetOne (int? id, string fields);
        /// <summary>
        /// Gets a list of comments posted on user&#39;s profile. 
        /// </summary>
        /// <param name="id">ID of the user whose comments are to be retrieved.</param>
        /// <param name="start">Index of the first comment to be loaded.</param>
        /// <param name="maxResults">Maximum number of comments to load.</param>
        /// <param name="getTotalCount">Whether to load the total number of comments.</param>
        /// <returns>PartialFindResultCommentForApiContract</returns>
        PartialFindResultCommentForApiContract UserApiGetProfileComments (int? id, int? start, int? maxResults, bool? getTotalCount);
        /// <summary>
        /// Gets a list of songs rated by a user. 
        /// </summary>
        /// <param name="id">ID of the user whose songs are to be browsed.</param>
        /// <param name="query">Song name query (optional).</param>
        /// <param name="tagName">Filter by tag name (optional).</param>
        /// <param name="tagId">Filter by tag Id (optional). This filter can be specified multiple times.</param>
        /// <param name="artistId">Filter by song artist (optional).</param>
        /// <param name="childVoicebanks">Include child voicebanks, if the artist being filtered by has any.</param>
        /// <param name="artistGrouping">Logical grouping for artists.</param>
        /// <param name="rating">Filter songs by given rating (optional).</param>
        /// <param name="songListId">Filter songs by song list (optional).</param>
        /// <param name="groupByRating">Group results by rating so that highest rated are first.</param>
        /// <param name="pvServices">Filter by one or more PV services (separated by commas). The song will pass the filter if it has a PV for any of the matched services.</param>
        /// <param name="advancedFilters">List of advanced filters (optional).</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, AdditionDate, FavoritedTimes, RatingScore.</param>
        /// <param name="nameMatchMode">Match mode for song name (optional, defaults to Auto).</param>
        /// <param name="fields">List of optional fields (optional). Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>PartialFindResultRatedSongForUserForApiContract</returns>
        PartialFindResultRatedSongForUserForApiContract UserApiGetRatedSongs (int? id, string query, string tagName, List<int?> tagId, List<int?> artistId, bool? childVoicebanks, string artistGrouping, string rating, int? songListId, bool? groupByRating, string pvServices, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, string nameMatchMode, string fields, string lang);
        /// <summary>
        /// Gets a list of song lists for a user. 
        /// </summary>
        /// <param name="id">User whose song lists are to be loaded.</param>
        /// <param name="query">Song list name query (optional).</param>
        /// <param name="nameMatchMode">Match mode for song name (optional, defaults to Auto).</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="sort">Sort option for the song lists. Possible values are None, Name, Date, CreateDate. Default is Name.</param>
        /// <param name="fields">List of optional fields.</param>
        /// <returns>PartialFindResultSongListForApiContract</returns>
        PartialFindResultSongListForApiContract UserApiGetSongLists (int? id, string query, string nameMatchMode, int? start, int? maxResults, bool? getTotalCount, string sort, string fields);
        /// <summary>
        /// Gets a specific user&#39;s rating for a song. 
        /// </summary>
        /// <param name="id">User whose rating is to be checked.</param>
        /// <param name="songId">ID of the song whose rating is to be checked.</param>
        /// <returns>string</returns>
        string UserApiGetSongRating (int? id, int? songId);
        /// <summary>
        /// Gets currently logged in user&#39;s rating for a song. Requires authentication.
        /// </summary>
        /// <param name="songId">ID of the song whose rating is to be checked.</param>
        /// <returns>string</returns>
        string UserApiGetSongRatingForCurrent (int? songId);
        /// <summary>
        /// Add or update collection status, media type and rating for a specific album, for the currently logged in user. If the user has already rated the album, any previous rating is replaced.              Authorization cookie must be included.
        /// </summary>
        /// <param name="albumId">ID of the album to be rated.</param>
        /// <param name="collectionStatus">Collection status. Possible values are Nothing, Wishlisted, Ordered and Owned.</param>
        /// <param name="mediaType">Album media type. Possible values are PhysicalDisc, DigitalDownload and Other.</param>
        /// <param name="rating">Rating to be given. Possible values are between 0 and 5.</param>
        /// <returns>string</returns>
        string UserApiPostAlbumStatus (int? albumId, string collectionStatus, string mediaType, int? rating);
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param>
        /// <param name="contract">New comment data. Only message can be edited.</param>
        /// <returns></returns>
        void UserApiPostEditComment (int? commentId, CommentForApiContract contract);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        void UserApiPostFollowedTag (int? tagId);
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="id">ID of the user for whom to create the comment.</param>
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param>
        /// <returns>CommentForApiContract</returns>
        CommentForApiContract UserApiPostNewComment (int? id, CommentForApiContract contract);
        /// <summary>
        /// Creates a new message. 
        /// </summary>
        /// <param name="id">User ID. Must be logged in user.</param>
        /// <param name="contract">Message data.</param>
        /// <returns>UserMessageContract</returns>
        UserMessageContract UserApiPostNewMessage (int? id, UserMessageContract contract);
        /// <summary>
        /// Refresh entry edit status, indicating that the current user is still editing that entry. 
        /// </summary>
        /// <param name="entryType">Type of entry.</param>
        /// <param name="entryId">Entry ID.</param>
        /// <returns></returns>
        void UserApiPostRefreshEntryEdit (string entryType, int? entryId);
        /// <summary>
        /// Updates user setting. 
        /// </summary>
        /// <param name="id">ID of the user to be updated. This must match the current user OR be unspecified (or 0) if the user is not logged in.</param>
        /// <param name="settingName">Name of the setting to be updated, for example &#39;showChatBox&#39;.</param>
        /// <param name="settingValue">Setting value, for example &#39;false&#39;.</param>
        /// <returns></returns>
        void UserApiPostSetting (int? id, string settingName, string settingValue);
        /// <summary>
        /// Appends tags for a song, by the currently logged in user. This can only be used to add tags - existing tags will not be removed.               Nothing will be done for tags that are already applied by the current user for the song.              Authorization cookie is required.
        /// </summary>
        /// <param name="songId">ID of the song to be tagged.</param>
        /// <param name="tags">List of tags to be appended.</param>
        /// <returns></returns>
        void UserApiPostSongTags (int? songId, List<TagBaseContract> tags);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class UserApiApi : IUserApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public UserApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="UserApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public UserApiApi(String basePath)
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
        ///  
        /// </summary>
        /// <param name="tagId"></param> 
        /// <returns></returns>            
        public void UserApiDeleteFollowedTag (int? tagId)
        {
            
            // verify the required parameter 'tagId' is set
            if (tagId == null) throw new ApiException(400, "Missing required parameter 'tagId' when calling UserApiDeleteFollowedTag");
            
    
            var path = "/api/users/current/followedTags/{tagId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tagId" + "}", ApiClient.ParameterToString(tagId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiDeleteFollowedTag: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiDeleteFollowedTag: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes a list of user messages. 
        /// </summary>
        /// <param name="id">ID of the user whose messages to delete.</param> 
        /// <param name="messageId">IDs of messages.</param> 
        /// <returns></returns>            
        public void UserApiDeleteMessages (int? id, List<int?> messageId)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiDeleteMessages");
            
            // verify the required parameter 'messageId' is set
            if (messageId == null) throw new ApiException(400, "Missing required parameter 'messageId' when calling UserApiDeleteMessages");
            
    
            var path = "/api/users/{id}/messages";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (messageId != null) queryParams.Add("messageId", ApiClient.ParameterToString(messageId)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiDeleteMessages: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiDeleteMessages: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param> 
        /// <returns></returns>            
        public void UserApiDeleteProfileComment (int? commentId)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling UserApiDeleteProfileComment");
            
    
            var path = "/api/users/profileComments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiDeleteProfileComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiDeleteProfileComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Gets a list of albums in a user&#39;s collection. This includes albums that have been rated by the user as well as albums that the user has bought or wishlisted.              Note that the user might have set his album ownership status and media type as private, in which case those properties are not included.
        /// </summary>
        /// <param name="id">ID of the user whose albums are to be browsed.</param> 
        /// <param name="query">Album name query (optional).</param> 
        /// <param name="tagId">Filter by tag Id (optional).</param> 
        /// <param name="tag">Filter by tag (optional).</param> 
        /// <param name="artistId">Filter by album artist (optional).</param> 
        /// <param name="purchaseStatuses">Filter by a comma-separated list of purchase statuses (optional). Possible values are Nothing, Wishlisted, Ordered, Owned, and all combinations of these.</param> 
        /// <param name="releaseEventId">Filter by release event. Optional.</param> 
        /// <param name="albumTypes">Filter by album type (optional).</param> 
        /// <param name="advancedFilters">List of advanced filters (optional).</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, ReleaseDate, AdditionDate, RatingAverage, RatingTotal, CollectionCount.</param> 
        /// <param name="nameMatchMode">Match mode for album name (optional, defaults to Auto).</param> 
        /// <param name="fields">List of optional fields (optional). Possible values are Artists, MainPicture, Names, PVs, Tags, WebLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>PartialFindResultAlbumForUserForApiContract</returns>            
        public PartialFindResultAlbumForUserForApiContract UserApiGetAlbumCollection (int? id, string query, int? tagId, string tag, int? artistId, string purchaseStatuses, int? releaseEventId, string albumTypes, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, string nameMatchMode, string fields, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiGetAlbumCollection");
            
    
            var path = "/api/users/{id}/albums";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (tagId != null) queryParams.Add("tagId", ApiClient.ParameterToString(tagId)); // query parameter
 if (tag != null) queryParams.Add("tag", ApiClient.ParameterToString(tag)); // query parameter
 if (artistId != null) queryParams.Add("artistId", ApiClient.ParameterToString(artistId)); // query parameter
 if (purchaseStatuses != null) queryParams.Add("purchaseStatuses", ApiClient.ParameterToString(purchaseStatuses)); // query parameter
 if (releaseEventId != null) queryParams.Add("releaseEventId", ApiClient.ParameterToString(releaseEventId)); // query parameter
 if (albumTypes != null) queryParams.Add("albumTypes", ApiClient.ParameterToString(albumTypes)); // query parameter
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetAlbumCollection: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetAlbumCollection: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultAlbumForUserForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultAlbumForUserForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets information about the currently logged in user. Requires login information.              This API supports CORS, and is restricted to specific origins.
        /// </summary>
        /// <param name="fields">Optional fields.</param> 
        /// <returns>UserForApiContract</returns>            
        public UserForApiContract UserApiGetCurrent (string fields)
        {
            
    
            var path = "/api/users/current";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetCurrent: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetCurrent: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserForApiContract) ApiClient.Deserialize(response.Content, typeof(UserForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of events a user has subscribed to. 
        /// </summary>
        /// <param name="id">User ID.</param> 
        /// <param name="relationshipType">Type of event subscription.</param> 
        /// <returns>List&lt;ReleaseEventForApiContract&gt;</returns>            
        public List<ReleaseEventForApiContract> UserApiGetEvents (int? id, string relationshipType)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiGetEvents");
            
            // verify the required parameter 'relationshipType' is set
            if (relationshipType == null) throw new ApiException(400, "Missing required parameter 'relationshipType' when calling UserApiGetEvents");
            
    
            var path = "/api/users/{id}/events";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (relationshipType != null) queryParams.Add("relationshipType", ApiClient.ParameterToString(relationshipType)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetEvents: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetEvents: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<ReleaseEventForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<ReleaseEventForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of artists followed by a user. 
        /// </summary>
        /// <param name="id">ID of the user whose followed artists are to be browsed.</param> 
        /// <param name="query">Artist name query (optional).</param> 
        /// <param name="artistType">Filter by artist type.</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, AdditionDate, AdditionDateAsc.</param> 
        /// <param name="nameMatchMode">Match mode for artist name (optional, defaults to Auto).</param> 
        /// <param name="fields">List of optional fields (optional). Possible values are Description, Groups, Members, Names, Tags, WebLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>PartialFindResultArtistForUserForApiContract</returns>            
        public PartialFindResultArtistForUserForApiContract UserApiGetFollowedArtists (int? id, string query, string artistType, int? start, int? maxResults, bool? getTotalCount, string sort, string nameMatchMode, string fields, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiGetFollowedArtists");
            
    
            var path = "/api/users/{id}/followedArtists";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (artistType != null) queryParams.Add("artistType", ApiClient.ParameterToString(artistType)); // query parameter
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetFollowedArtists: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetFollowedArtists: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultArtistForUserForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultArtistForUserForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of users. 
        /// </summary>
        /// <param name="query">User name query (optional).</param> 
        /// <param name="groups">Filter by user group. Only one value supported for now. Optional.</param> 
        /// <param name="joinDateAfter">Filter by users who joined after this date (inclusive).</param> 
        /// <param name="joinDateBefore">Filter by users who joined before this date (exclusive).</param> 
        /// <param name="nameMatchMode">Name match mode.</param> 
        /// <param name="start">Index of the first entry to be loaded.</param> 
        /// <param name="maxResults">Maximum number of results to be loaded.</param> 
        /// <param name="getTotalCount">Whether to get total number of results.</param> 
        /// <param name="sort">Sort rule.</param> 
        /// <param name="includeDisabled">Whether to include disabled user accounts.</param> 
        /// <param name="onlyVerified">Whether to only include verified artists.</param> 
        /// <param name="knowsLanguage">Filter by known language (optional). This is the ISO 639-1 language code, for example \&quot;en\&quot; or \&quot;zh\&quot;.</param> 
        /// <param name="fields">Optional fields. Possible values are None and MainPicture. Optional.</param> 
        /// <returns>PartialFindResultUserForApiContract</returns>            
        public PartialFindResultUserForApiContract UserApiGetList (string query, string groups, DateTime? joinDateAfter, DateTime? joinDateBefore, string nameMatchMode, int? start, int? maxResults, bool? getTotalCount, string sort, bool? includeDisabled, bool? onlyVerified, string knowsLanguage, string fields)
        {
            
    
            var path = "/api/users";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (groups != null) queryParams.Add("groups", ApiClient.ParameterToString(groups)); // query parameter
 if (joinDateAfter != null) queryParams.Add("joinDateAfter", ApiClient.ParameterToString(joinDateAfter)); // query parameter
 if (joinDateBefore != null) queryParams.Add("joinDateBefore", ApiClient.ParameterToString(joinDateBefore)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
 if (sort != null) queryParams.Add("sort", ApiClient.ParameterToString(sort)); // query parameter
 if (includeDisabled != null) queryParams.Add("includeDisabled", ApiClient.ParameterToString(includeDisabled)); // query parameter
 if (onlyVerified != null) queryParams.Add("onlyVerified", ApiClient.ParameterToString(onlyVerified)); // query parameter
 if (knowsLanguage != null) queryParams.Add("knowsLanguage", ApiClient.ParameterToString(knowsLanguage)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultUserForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultUserForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a user message. The message will be marked as read.              User can only load messages from their own inbox.
        /// </summary>
        /// <param name="messageId">ID of the message.</param> 
        /// <returns>UserMessageContract</returns>            
        public UserMessageContract UserApiGetMessage (int? messageId)
        {
            
            // verify the required parameter 'messageId' is set
            if (messageId == null) throw new ApiException(400, "Missing required parameter 'messageId' when calling UserApiGetMessage");
            
    
            var path = "/api/users/messages/{messageId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "messageId" + "}", ApiClient.ParameterToString(messageId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetMessage: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetMessage: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserMessageContract) ApiClient.Deserialize(response.Content, typeof(UserMessageContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of messages. 
        /// </summary>
        /// <param name="id">User ID. Must be the currently logged in user (loading messages for another user is not allowed).</param> 
        /// <param name="inbox">Type of inbox. Possible values are Nothing (load all, default), Received, Sent, Notifications.</param> 
        /// <param name="unread">Whether to only load unread messages. Loading unread messages is only possible for received messages and notifications (not sent messages).</param> 
        /// <param name="anotherUserId">Filter by id of the other user (either sender or receiver).</param> 
        /// <param name="start">Index of the first entry to be loaded.</param> 
        /// <param name="maxResults">Maximum number of results to be loaded.</param> 
        /// <param name="getTotalCount">Whether to get total number of results.</param> 
        /// <returns>PartialFindResultUserMessageContract</returns>            
        public PartialFindResultUserMessageContract UserApiGetMessages (int? id, string inbox, bool? unread, int? anotherUserId, int? start, int? maxResults, bool? getTotalCount)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiGetMessages");
            
    
            var path = "/api/users/{id}/messages";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (inbox != null) queryParams.Add("inbox", ApiClient.ParameterToString(inbox)); // query parameter
 if (unread != null) queryParams.Add("unread", ApiClient.ParameterToString(unread)); // query parameter
 if (anotherUserId != null) queryParams.Add("anotherUserId", ApiClient.ParameterToString(anotherUserId)); // query parameter
 if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetMessages: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetMessages: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultUserMessageContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultUserMessageContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of user names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param> 
        /// <param name="nameMatchMode">Name match mode. Words is treated the same as Partial.</param> 
        /// <param name="maxResults">Maximum number of results.</param> 
        /// <param name="includeDisabled">Whether to include disabled user accounts. If false, disabled accounts are excluded.</param> 
        /// <returns>List&lt;string&gt;</returns>            
        public List<string> UserApiGetNames (string query, string nameMatchMode, int? maxResults, bool? includeDisabled)
        {
            
    
            var path = "/api/users/names";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (includeDisabled != null) queryParams.Add("includeDisabled", ApiClient.ParameterToString(includeDisabled)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetNames: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetNames: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
    
        /// <summary>
        /// Gets user by ID. 
        /// </summary>
        /// <param name="id">User ID.</param> 
        /// <param name="fields">Optional fields.</param> 
        /// <returns>UserForApiContract</returns>            
        public UserForApiContract UserApiGetOne (int? id, string fields)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiGetOne");
            
    
            var path = "/api/users/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetOne: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetOne: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserForApiContract) ApiClient.Deserialize(response.Content, typeof(UserForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of comments posted on user&#39;s profile. 
        /// </summary>
        /// <param name="id">ID of the user whose comments are to be retrieved.</param> 
        /// <param name="start">Index of the first comment to be loaded.</param> 
        /// <param name="maxResults">Maximum number of comments to load.</param> 
        /// <param name="getTotalCount">Whether to load the total number of comments.</param> 
        /// <returns>PartialFindResultCommentForApiContract</returns>            
        public PartialFindResultCommentForApiContract UserApiGetProfileComments (int? id, int? start, int? maxResults, bool? getTotalCount)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiGetProfileComments");
            
    
            var path = "/api/users/{id}/profileComments";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetProfileComments: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetProfileComments: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultCommentForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultCommentForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of songs rated by a user. 
        /// </summary>
        /// <param name="id">ID of the user whose songs are to be browsed.</param> 
        /// <param name="query">Song name query (optional).</param> 
        /// <param name="tagName">Filter by tag name (optional).</param> 
        /// <param name="tagId">Filter by tag Id (optional). This filter can be specified multiple times.</param> 
        /// <param name="artistId">Filter by song artist (optional).</param> 
        /// <param name="childVoicebanks">Include child voicebanks, if the artist being filtered by has any.</param> 
        /// <param name="artistGrouping">Logical grouping for artists.</param> 
        /// <param name="rating">Filter songs by given rating (optional).</param> 
        /// <param name="songListId">Filter songs by song list (optional).</param> 
        /// <param name="groupByRating">Group results by rating so that highest rated are first.</param> 
        /// <param name="pvServices">Filter by one or more PV services (separated by commas). The song will pass the filter if it has a PV for any of the matched services.</param> 
        /// <param name="advancedFilters">List of advanced filters (optional).</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, AdditionDate, FavoritedTimes, RatingScore.</param> 
        /// <param name="nameMatchMode">Match mode for song name (optional, defaults to Auto).</param> 
        /// <param name="fields">List of optional fields (optional). Possible values are Albums, Artists, Names, PVs, Tags, ThumbUrl, WebLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>PartialFindResultRatedSongForUserForApiContract</returns>            
        public PartialFindResultRatedSongForUserForApiContract UserApiGetRatedSongs (int? id, string query, string tagName, List<int?> tagId, List<int?> artistId, bool? childVoicebanks, string artistGrouping, string rating, int? songListId, bool? groupByRating, string pvServices, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, string nameMatchMode, string fields, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiGetRatedSongs");
            
    
            var path = "/api/users/{id}/ratedSongs";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (tagName != null) queryParams.Add("tagName", ApiClient.ParameterToString(tagName)); // query parameter
 if (tagId != null) queryParams.Add("tagId", ApiClient.ParameterToString(tagId)); // query parameter
 if (artistId != null) queryParams.Add("artistId", ApiClient.ParameterToString(artistId)); // query parameter
 if (childVoicebanks != null) queryParams.Add("childVoicebanks", ApiClient.ParameterToString(childVoicebanks)); // query parameter
 if (artistGrouping != null) queryParams.Add("artistGrouping", ApiClient.ParameterToString(artistGrouping)); // query parameter
 if (rating != null) queryParams.Add("rating", ApiClient.ParameterToString(rating)); // query parameter
 if (songListId != null) queryParams.Add("songListId", ApiClient.ParameterToString(songListId)); // query parameter
 if (groupByRating != null) queryParams.Add("groupByRating", ApiClient.ParameterToString(groupByRating)); // query parameter
 if (pvServices != null) queryParams.Add("pvServices", ApiClient.ParameterToString(pvServices)); // query parameter
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetRatedSongs: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetRatedSongs: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultRatedSongForUserForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultRatedSongForUserForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of song lists for a user. 
        /// </summary>
        /// <param name="id">User whose song lists are to be loaded.</param> 
        /// <param name="query">Song list name query (optional).</param> 
        /// <param name="nameMatchMode">Match mode for song name (optional, defaults to Auto).</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 50).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="sort">Sort option for the song lists. Possible values are None, Name, Date, CreateDate. Default is Name.</param> 
        /// <param name="fields">List of optional fields.</param> 
        /// <returns>PartialFindResultSongListForApiContract</returns>            
        public PartialFindResultSongListForApiContract UserApiGetSongLists (int? id, string query, string nameMatchMode, int? start, int? maxResults, bool? getTotalCount, string sort, string fields)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiGetSongLists");
            
    
            var path = "/api/users/{id}/songLists";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
 if (sort != null) queryParams.Add("sort", ApiClient.ParameterToString(sort)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetSongLists: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetSongLists: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultSongListForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultSongListForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a specific user&#39;s rating for a song. 
        /// </summary>
        /// <param name="id">User whose rating is to be checked.</param> 
        /// <param name="songId">ID of the song whose rating is to be checked.</param> 
        /// <returns>string</returns>            
        public string UserApiGetSongRating (int? id, int? songId)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiGetSongRating");
            
            // verify the required parameter 'songId' is set
            if (songId == null) throw new ApiException(400, "Missing required parameter 'songId' when calling UserApiGetSongRating");
            
    
            var path = "/api/users/{id}/ratedSongs/{songId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
path = path.Replace("{" + "songId" + "}", ApiClient.ParameterToString(songId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetSongRating: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetSongRating: " + response.ErrorMessage, response.ErrorMessage);
    
            return (string) ApiClient.Deserialize(response.Content, typeof(string), response.Headers);
        }
    
        /// <summary>
        /// Gets currently logged in user&#39;s rating for a song. Requires authentication.
        /// </summary>
        /// <param name="songId">ID of the song whose rating is to be checked.</param> 
        /// <returns>string</returns>            
        public string UserApiGetSongRatingForCurrent (int? songId)
        {
            
            // verify the required parameter 'songId' is set
            if (songId == null) throw new ApiException(400, "Missing required parameter 'songId' when calling UserApiGetSongRatingForCurrent");
            
    
            var path = "/api/users/current/ratedSongs/{songId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "songId" + "}", ApiClient.ParameterToString(songId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetSongRatingForCurrent: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiGetSongRatingForCurrent: " + response.ErrorMessage, response.ErrorMessage);
    
            return (string) ApiClient.Deserialize(response.Content, typeof(string), response.Headers);
        }
    
        /// <summary>
        /// Add or update collection status, media type and rating for a specific album, for the currently logged in user. If the user has already rated the album, any previous rating is replaced.              Authorization cookie must be included.
        /// </summary>
        /// <param name="albumId">ID of the album to be rated.</param> 
        /// <param name="collectionStatus">Collection status. Possible values are Nothing, Wishlisted, Ordered and Owned.</param> 
        /// <param name="mediaType">Album media type. Possible values are PhysicalDisc, DigitalDownload and Other.</param> 
        /// <param name="rating">Rating to be given. Possible values are between 0 and 5.</param> 
        /// <returns>string</returns>            
        public string UserApiPostAlbumStatus (int? albumId, string collectionStatus, string mediaType, int? rating)
        {
            
            // verify the required parameter 'albumId' is set
            if (albumId == null) throw new ApiException(400, "Missing required parameter 'albumId' when calling UserApiPostAlbumStatus");
            
            // verify the required parameter 'collectionStatus' is set
            if (collectionStatus == null) throw new ApiException(400, "Missing required parameter 'collectionStatus' when calling UserApiPostAlbumStatus");
            
            // verify the required parameter 'mediaType' is set
            if (mediaType == null) throw new ApiException(400, "Missing required parameter 'mediaType' when calling UserApiPostAlbumStatus");
            
            // verify the required parameter 'rating' is set
            if (rating == null) throw new ApiException(400, "Missing required parameter 'rating' when calling UserApiPostAlbumStatus");
            
    
            var path = "/api/users/current/albums/{albumId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "albumId" + "}", ApiClient.ParameterToString(albumId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (collectionStatus != null) queryParams.Add("collectionStatus", ApiClient.ParameterToString(collectionStatus)); // query parameter
 if (mediaType != null) queryParams.Add("mediaType", ApiClient.ParameterToString(mediaType)); // query parameter
 if (rating != null) queryParams.Add("rating", ApiClient.ParameterToString(rating)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostAlbumStatus: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostAlbumStatus: " + response.ErrorMessage, response.ErrorMessage);
    
            return (string) ApiClient.Deserialize(response.Content, typeof(string), response.Headers);
        }
    
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param> 
        /// <param name="contract">New comment data. Only message can be edited.</param> 
        /// <returns></returns>            
        public void UserApiPostEditComment (int? commentId, CommentForApiContract contract)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling UserApiPostEditComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling UserApiPostEditComment");
            
    
            var path = "/api/users/profileComments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostEditComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostEditComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="tagId"></param> 
        /// <returns></returns>            
        public void UserApiPostFollowedTag (int? tagId)
        {
            
            // verify the required parameter 'tagId' is set
            if (tagId == null) throw new ApiException(400, "Missing required parameter 'tagId' when calling UserApiPostFollowedTag");
            
    
            var path = "/api/users/current/followedTags/{tagId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tagId" + "}", ApiClient.ParameterToString(tagId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostFollowedTag: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostFollowedTag: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="id">ID of the user for whom to create the comment.</param> 
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param> 
        /// <returns>CommentForApiContract</returns>            
        public CommentForApiContract UserApiPostNewComment (int? id, CommentForApiContract contract)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiPostNewComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling UserApiPostNewComment");
            
    
            var path = "/api/users/{id}/profileComments";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostNewComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostNewComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CommentForApiContract) ApiClient.Deserialize(response.Content, typeof(CommentForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Creates a new message. 
        /// </summary>
        /// <param name="id">User ID. Must be logged in user.</param> 
        /// <param name="contract">Message data.</param> 
        /// <returns>UserMessageContract</returns>            
        public UserMessageContract UserApiPostNewMessage (int? id, UserMessageContract contract)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiPostNewMessage");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling UserApiPostNewMessage");
            
    
            var path = "/api/users/{id}/messages";
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
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostNewMessage: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostNewMessage: " + response.ErrorMessage, response.ErrorMessage);
    
            return (UserMessageContract) ApiClient.Deserialize(response.Content, typeof(UserMessageContract), response.Headers);
        }
    
        /// <summary>
        /// Refresh entry edit status, indicating that the current user is still editing that entry. 
        /// </summary>
        /// <param name="entryType">Type of entry.</param> 
        /// <param name="entryId">Entry ID.</param> 
        /// <returns></returns>            
        public void UserApiPostRefreshEntryEdit (string entryType, int? entryId)
        {
            
            // verify the required parameter 'entryType' is set
            if (entryType == null) throw new ApiException(400, "Missing required parameter 'entryType' when calling UserApiPostRefreshEntryEdit");
            
            // verify the required parameter 'entryId' is set
            if (entryId == null) throw new ApiException(400, "Missing required parameter 'entryId' when calling UserApiPostRefreshEntryEdit");
            
    
            var path = "/api/users/current/refreshEntryEdit";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (entryType != null) queryParams.Add("entryType", ApiClient.ParameterToString(entryType)); // query parameter
 if (entryId != null) queryParams.Add("entryId", ApiClient.ParameterToString(entryId)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostRefreshEntryEdit: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostRefreshEntryEdit: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Updates user setting. 
        /// </summary>
        /// <param name="id">ID of the user to be updated. This must match the current user OR be unspecified (or 0) if the user is not logged in.</param> 
        /// <param name="settingName">Name of the setting to be updated, for example &#39;showChatBox&#39;.</param> 
        /// <param name="settingValue">Setting value, for example &#39;false&#39;.</param> 
        /// <returns></returns>            
        public void UserApiPostSetting (int? id, string settingName, string settingValue)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling UserApiPostSetting");
            
            // verify the required parameter 'settingName' is set
            if (settingName == null) throw new ApiException(400, "Missing required parameter 'settingName' when calling UserApiPostSetting");
            
            // verify the required parameter 'settingValue' is set
            if (settingValue == null) throw new ApiException(400, "Missing required parameter 'settingValue' when calling UserApiPostSetting");
            
    
            var path = "/api/users/{id}/settings/{settingName}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
path = path.Replace("{" + "settingName" + "}", ApiClient.ParameterToString(settingName));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(settingValue); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostSetting: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostSetting: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Appends tags for a song, by the currently logged in user. This can only be used to add tags - existing tags will not be removed.               Nothing will be done for tags that are already applied by the current user for the song.              Authorization cookie is required.
        /// </summary>
        /// <param name="songId">ID of the song to be tagged.</param> 
        /// <param name="tags">List of tags to be appended.</param> 
        /// <returns></returns>            
        public void UserApiPostSongTags (int? songId, List<TagBaseContract> tags)
        {
            
            // verify the required parameter 'songId' is set
            if (songId == null) throw new ApiException(400, "Missing required parameter 'songId' when calling UserApiPostSongTags");
            
            // verify the required parameter 'tags' is set
            if (tags == null) throw new ApiException(400, "Missing required parameter 'tags' when calling UserApiPostSongTags");
            
    
            var path = "/api/users/current/songTags/{songId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "songId" + "}", ApiClient.ParameterToString(songId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(tags); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostSongTags: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UserApiPostSongTags: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}
