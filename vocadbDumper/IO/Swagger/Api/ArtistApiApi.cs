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
    public interface IArtistApiApi
    {
        /// <summary>
        /// Deletes an artist. 
        /// </summary>
        /// <param name="id">ID of the artist to be deleted.</param>
        /// <param name="notes">Notes.</param>
        /// <returns></returns>
        void ArtistApiDelete (int? id, string notes);
        /// <summary>
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param>
        /// <returns></returns>
        void ArtistApiDeleteComment (int? commentId);
        /// <summary>
        /// Gets a list of comments for an artist. Pagination and sorting might be added later.
        /// </summary>
        /// <param name="id">ID of the artist whose comments to load.</param>
        /// <returns>List&lt;CommentForApiContract&gt;</returns>
        List<CommentForApiContract> ArtistApiGetComments (int? id);
        /// <summary>
        /// Find artists. 
        /// </summary>
        /// <param name="query">Artist name query (optional).</param>
        /// <param name="artistTypes">Filtered artist type (optional).</param>
        /// <param name="allowBaseVoicebanks">Allow base voicebanks. If false, only root voicebanks will be allowed. Only affects voice synthesizers that can have base voicebanks.</param>
        /// <param name="tagName">Filter by tag name (optional).</param>
        /// <param name="tagId">Filter by tag Id (optional). This filter can be specified multiple times.</param>
        /// <param name="childTags">Include child tags, if the tags being filtered by have any.</param>
        /// <param name="followedByUserId">Filter by user following the artists. By default there is no filtering.</param>
        /// <param name="status">Filter by entry status (optional).</param>
        /// <param name="advancedFilters">List of advanced filters (optional).</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 100).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, AdditionDate, AdditionDateAsc.</param>
        /// <param name="preferAccurateMatches">Whether the search should prefer accurate matches.               If this is true, entries that match by prefix will be moved first, instead of being sorted alphabetically.              Requires a text query. Does not support pagination.              This is mostly useful for autocomplete boxes.</param>
        /// <param name="nameMatchMode">Match mode for artist name (optional, defaults to Exact).</param>
        /// <param name="fields">List of optional fields (optional). Possible values are Description, Groups, Members, Names, Tags, WebLinks.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>PartialFindResultArtistForApiContract</returns>
        PartialFindResultArtistForApiContract ArtistApiGetList (string query, string artistTypes, bool? allowBaseVoicebanks, List<string> tagName, List<int?> tagId, bool? childTags, int? followedByUserId, string status, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, bool? preferAccurateMatches, string nameMatchMode, string fields, string lang);
        /// <summary>
        /// Gets a list of artist names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param>
        /// <param name="nameMatchMode">Name match mode.</param>
        /// <param name="maxResults">Maximum number of results.</param>
        /// <returns>List&lt;string&gt;</returns>
        List<string> ArtistApiGetNames (string query, string nameMatchMode, int? maxResults);
        /// <summary>
        /// Gets an artist by Id. 
        /// </summary>
        /// <param name="id">Artist ID (required).</param>
        /// <param name="fields">List of optional fields (optional). Possible values are Description, Groups, Members, Names, Tags, WebLinks.</param>
        /// <param name="relations">List of artist relations (optional). Possible values are LatestAlbums, PopularAlbums, LatestSongs, PopularSongs, All</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>ArtistForApiContract</returns>
        ArtistForApiContract ArtistApiGetOne (int? id, string fields, string relations, string lang);
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param>
        /// <param name="contract">New comment data. Only message can be edited.</param>
        /// <returns></returns>
        void ArtistApiPostEditComment (int? commentId, CommentForApiContract contract);
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="id">ID of the artist for which to create the comment.</param>
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param>
        /// <returns>CommentForApiContract</returns>
        CommentForApiContract ArtistApiPostNewComment (int? id, CommentForApiContract contract);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ArtistApiApi : IArtistApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ArtistApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ArtistApiApi(String basePath)
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
        /// Deletes an artist. 
        /// </summary>
        /// <param name="id">ID of the artist to be deleted.</param> 
        /// <param name="notes">Notes.</param> 
        /// <returns></returns>            
        public void ArtistApiDelete (int? id, string notes)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ArtistApiDelete");
            
    
            var path = "/api/artists/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param> 
        /// <returns></returns>            
        public void ArtistApiDeleteComment (int? commentId)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling ArtistApiDeleteComment");
            
    
            var path = "/api/artists/comments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiDeleteComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiDeleteComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Gets a list of comments for an artist. Pagination and sorting might be added later.
        /// </summary>
        /// <param name="id">ID of the artist whose comments to load.</param> 
        /// <returns>List&lt;CommentForApiContract&gt;</returns>            
        public List<CommentForApiContract> ArtistApiGetComments (int? id)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ArtistApiGetComments");
            
    
            var path = "/api/artists/{id}/comments";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiGetComments: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiGetComments: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<CommentForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<CommentForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Find artists. 
        /// </summary>
        /// <param name="query">Artist name query (optional).</param> 
        /// <param name="artistTypes">Filtered artist type (optional).</param> 
        /// <param name="allowBaseVoicebanks">Allow base voicebanks. If false, only root voicebanks will be allowed. Only affects voice synthesizers that can have base voicebanks.</param> 
        /// <param name="tagName">Filter by tag name (optional).</param> 
        /// <param name="tagId">Filter by tag Id (optional). This filter can be specified multiple times.</param> 
        /// <param name="childTags">Include child tags, if the tags being filtered by have any.</param> 
        /// <param name="followedByUserId">Filter by user following the artists. By default there is no filtering.</param> 
        /// <param name="status">Filter by entry status (optional).</param> 
        /// <param name="advancedFilters">List of advanced filters (optional).</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 100).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="sort">Sort rule (optional, defaults to Name). Possible values are None, Name, AdditionDate, AdditionDateAsc.</param> 
        /// <param name="preferAccurateMatches">Whether the search should prefer accurate matches.               If this is true, entries that match by prefix will be moved first, instead of being sorted alphabetically.              Requires a text query. Does not support pagination.              This is mostly useful for autocomplete boxes.</param> 
        /// <param name="nameMatchMode">Match mode for artist name (optional, defaults to Exact).</param> 
        /// <param name="fields">List of optional fields (optional). Possible values are Description, Groups, Members, Names, Tags, WebLinks.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>PartialFindResultArtistForApiContract</returns>            
        public PartialFindResultArtistForApiContract ArtistApiGetList (string query, string artistTypes, bool? allowBaseVoicebanks, List<string> tagName, List<int?> tagId, bool? childTags, int? followedByUserId, string status, List<Object> advancedFilters, int? start, int? maxResults, bool? getTotalCount, string sort, bool? preferAccurateMatches, string nameMatchMode, string fields, string lang)
        {
            
    
            var path = "/api/artists";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (artistTypes != null) queryParams.Add("artistTypes", ApiClient.ParameterToString(artistTypes)); // query parameter
 if (allowBaseVoicebanks != null) queryParams.Add("allowBaseVoicebanks", ApiClient.ParameterToString(allowBaseVoicebanks)); // query parameter
 if (tagName != null) queryParams.Add("tagName", ApiClient.ParameterToString(tagName)); // query parameter
 if (tagId != null) queryParams.Add("tagId", ApiClient.ParameterToString(tagId)); // query parameter
 if (childTags != null) queryParams.Add("childTags", ApiClient.ParameterToString(childTags)); // query parameter
 if (followedByUserId != null) queryParams.Add("followedByUserId", ApiClient.ParameterToString(followedByUserId)); // query parameter
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
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultArtistForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultArtistForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of artist names. Ideal for autocomplete boxes. 
        /// </summary>
        /// <param name="query">Text query.</param> 
        /// <param name="nameMatchMode">Name match mode.</param> 
        /// <param name="maxResults">Maximum number of results.</param> 
        /// <returns>List&lt;string&gt;</returns>            
        public List<string> ArtistApiGetNames (string query, string nameMatchMode, int? maxResults)
        {
            
    
            var path = "/api/artists/names";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiGetNames: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiGetNames: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
    
        /// <summary>
        /// Gets an artist by Id. 
        /// </summary>
        /// <param name="id">Artist ID (required).</param> 
        /// <param name="fields">List of optional fields (optional). Possible values are Description, Groups, Members, Names, Tags, WebLinks.</param> 
        /// <param name="relations">List of artist relations (optional). Possible values are LatestAlbums, PopularAlbums, LatestSongs, PopularSongs, All</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>ArtistForApiContract</returns>            
        public ArtistForApiContract ArtistApiGetOne (int? id, string fields, string relations, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ArtistApiGetOne");
            
    
            var path = "/api/artists/{id}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "id" + "}", ApiClient.ParameterToString(id));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (relations != null) queryParams.Add("relations", ApiClient.ParameterToString(relations)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiGetOne: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiGetOne: " + response.ErrorMessage, response.ErrorMessage);
    
            return (ArtistForApiContract) ApiClient.Deserialize(response.Content, typeof(ArtistForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param> 
        /// <param name="contract">New comment data. Only message can be edited.</param> 
        /// <returns></returns>            
        public void ArtistApiPostEditComment (int? commentId, CommentForApiContract contract)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling ArtistApiPostEditComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling ArtistApiPostEditComment");
            
    
            var path = "/api/artists/comments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiPostEditComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiPostEditComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="id">ID of the artist for which to create the comment.</param> 
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param> 
        /// <returns>CommentForApiContract</returns>            
        public CommentForApiContract ArtistApiPostNewComment (int? id, CommentForApiContract contract)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling ArtistApiPostNewComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling ArtistApiPostNewComment");
            
    
            var path = "/api/artists/{id}/comments";
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
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiPostNewComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling ArtistApiPostNewComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CommentForApiContract) ApiClient.Deserialize(response.Content, typeof(CommentForApiContract), response.Headers);
        }
    
    }
}
