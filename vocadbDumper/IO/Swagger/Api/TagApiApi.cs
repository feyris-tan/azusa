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
    public interface ITagApiApi
    {
        /// <summary>
        /// Deletes a tag. 
        /// </summary>
        /// <param name="id">ID of the tag to be deleted.</param>
        /// <param name="notes">Notes (optional).</param>
        /// <param name="hardDelete">If true, the entry is hard deleted. Hard deleted entries cannot be restored normally, but they will be moved to trash.              If false, the entry is soft deleted, meaning it can still be restored.</param>
        /// <returns></returns>
        void TagApiDelete (int? id, string notes, bool? hardDelete);
        /// <summary>
        /// Deletes a comment.              Normal users can delete their own comments, moderators can delete all comments.              Requires login. 
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param>
        /// <returns></returns>
        void TagApiDeleteComment (int? commentId);
        /// <summary>
        /// Gets a tag by ID. 
        /// </summary>
        /// <param name="id">Tag ID (required).</param>
        /// <param name="fields">List of optional fields (optional).</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>TagForApiContract</returns>
        TagForApiContract TagApiGetById (int? id, string fields, string lang);
        /// <summary>
        /// DEPRECATED. Gets a tag by name. 
        /// </summary>
        /// <param name="name">Tag name (required).</param>
        /// <param name="fields">List of optional fields (optional).</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>TagForApiContract</returns>
        TagForApiContract TagApiGetByName (string name, string fields, string lang);
        /// <summary>
        /// Gets a list of tag category names. 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="nameMatchMode"></param>
        /// <returns>List&lt;string&gt;</returns>
        List<string> TagApiGetCategoryNamesList (string query, string nameMatchMode);
        /// <summary>
        /// Gets a list of child tags for a tag.              Only direct children will be included. 
        /// </summary>
        /// <param name="tagId">ID of the tag whose children to load.</param>
        /// <param name="fields">List of optional fields (optional).</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>List&lt;TagForApiContract&gt;</returns>
        List<TagForApiContract> TagApiGetChildTags (int? tagId, string fields, string lang);
        /// <summary>
        /// Gets a list of comments for a tag.              Note: pagination and sorting might be added later. 
        /// </summary>
        /// <param name="tagId">ID of the tag whose comments to load.</param>
        /// <returns>PartialFindResultCommentForApiContract</returns>
        PartialFindResultCommentForApiContract TagApiGetComments (int? tagId);
        /// <summary>
        /// Find tags. 
        /// </summary>
        /// <param name="query">Tag name query (optional).</param>
        /// <param name="allowChildren">Whether to allow child tags. If this is false, only root tags (that aren&#39;t children of any other tag) will be included.</param>
        /// <param name="categoryName">Filter tags by category (optional). If specified, this must be an exact match (case insensitive).</param>
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param>
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 30).</param>
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param>
        /// <param name="nameMatchMode">Match mode for song name (optional, defaults to Exact).</param>
        /// <param name="sort">Sort rule (optional, by default tags are sorted by name).Possible values are Name and UsageCount.</param>
        /// <param name="preferAccurateMatches">Whether the search should prefer accurate matches.               If this is true, entries that match by prefix will be moved first, instead of being sorted alphabetically.              Requires a text query. Does not support pagination.              This is mostly useful for autocomplete boxes.</param>
        /// <param name="fields">List of optional fields (optional).</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <param name="target"></param>
        /// <returns>PartialFindResultTagForApiContract</returns>
        PartialFindResultTagForApiContract TagApiGetList (string query, bool? allowChildren, string categoryName, int? start, int? maxResults, bool? getTotalCount, string nameMatchMode, string sort, bool? preferAccurateMatches, string fields, string lang, string target);
        /// <summary>
        /// Find tag names by a part of name.                            Matching is done anywhere from the name. 
        /// </summary>
        /// <param name="query">Tag name query, for example \&quot;rock\&quot;.</param>
        /// <param name="allowAliases">Whether to find tags that are aliases of other tags as well.               If false, only tags that are not aliases will be listed.</param>
        /// <param name="maxResults">Maximum number of search results.</param>
        /// <returns>List&lt;string&gt;</returns>
        List<string> TagApiGetNames (string query, bool? allowAliases, int? maxResults);
        /// <summary>
        /// Gets the most common tags in a category. 
        /// </summary>
        /// <param name="categoryName">Tag category, for example \&quot;Genres\&quot;. Optional - if not specified, no filtering is done.</param>
        /// <param name="maxResults">Maximum number of tags to return.</param>
        /// <param name="lang">Content language preference (optional).</param>
        /// <returns>List&lt;TagBaseContract&gt;</returns>
        List<TagBaseContract> TagApiGetTopTags (string categoryName, int? maxResults, string lang);
        /// <summary>
        /// Updates a comment.              Normal users can edit their own comments, moderators can edit all comments.              Requires login. 
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param>
        /// <param name="contract">New comment data. Only message can be edited.</param>
        /// <returns></returns>
        void TagApiPostEditComment (int? commentId, CommentForApiContract contract);
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="tagId">ID of the tag for which to create the comment.</param>
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param>
        /// <returns>CommentForApiContract</returns>
        CommentForApiContract TagApiPostNewComment (int? tagId, CommentForApiContract contract);
        /// <summary>
        /// Creates a new tag. 
        /// </summary>
        /// <param name="name">Tag English name. Tag names must be unique.</param>
        /// <returns>TagBaseContract</returns>
        TagBaseContract TagApiPostNewTag (string name);
        /// <summary>
        /// Creates a new report. 
        /// </summary>
        /// <param name="tagId">Tag to be reported.</param>
        /// <param name="reportType">Report type.</param>
        /// <param name="notes">Notes. Optional.</param>
        /// <param name="versionNumber">Version to be reported. Optional.</param>
        /// <returns></returns>
        void TagApiPostReport (int? tagId, string reportType, string notes, int? versionNumber);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class TagApiApi : ITagApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TagApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public TagApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="TagApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public TagApiApi(String basePath)
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
        /// Deletes a tag. 
        /// </summary>
        /// <param name="id">ID of the tag to be deleted.</param> 
        /// <param name="notes">Notes (optional).</param> 
        /// <param name="hardDelete">If true, the entry is hard deleted. Hard deleted entries cannot be restored normally, but they will be moved to trash.              If false, the entry is soft deleted, meaning it can still be restored.</param> 
        /// <returns></returns>            
        public void TagApiDelete (int? id, string notes, bool? hardDelete)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling TagApiDelete");
            
    
            var path = "/api/tags/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiDelete: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiDelete: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Deletes a comment.              Normal users can delete their own comments, moderators can delete all comments.              Requires login. 
        /// </summary>
        /// <param name="commentId">ID of the comment to be deleted.</param> 
        /// <returns></returns>            
        public void TagApiDeleteComment (int? commentId)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling TagApiDeleteComment");
            
    
            var path = "/api/tags/comments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiDeleteComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiDeleteComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Gets a tag by ID. 
        /// </summary>
        /// <param name="id">Tag ID (required).</param> 
        /// <param name="fields">List of optional fields (optional).</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>TagForApiContract</returns>            
        public TagForApiContract TagApiGetById (int? id, string fields, string lang)
        {
            
            // verify the required parameter 'id' is set
            if (id == null) throw new ApiException(400, "Missing required parameter 'id' when calling TagApiGetById");
            
    
            var path = "/api/tags/{id}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetById: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetById: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TagForApiContract) ApiClient.Deserialize(response.Content, typeof(TagForApiContract), response.Headers);
        }
    
        /// <summary>
        /// DEPRECATED. Gets a tag by name. 
        /// </summary>
        /// <param name="name">Tag name (required).</param> 
        /// <param name="fields">List of optional fields (optional).</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>TagForApiContract</returns>            
        public TagForApiContract TagApiGetByName (string name, string fields, string lang)
        {
            
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling TagApiGetByName");
            
    
            var path = "/api/tags/byName/{name}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "name" + "}", ApiClient.ParameterToString(name));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetByName: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetByName: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TagForApiContract) ApiClient.Deserialize(response.Content, typeof(TagForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of tag category names. 
        /// </summary>
        /// <param name="query"></param> 
        /// <param name="nameMatchMode"></param> 
        /// <returns>List&lt;string&gt;</returns>            
        public List<string> TagApiGetCategoryNamesList (string query, string nameMatchMode)
        {
            
    
            var path = "/api/tags/categoryNames";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetCategoryNamesList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetCategoryNamesList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of child tags for a tag.              Only direct children will be included. 
        /// </summary>
        /// <param name="tagId">ID of the tag whose children to load.</param> 
        /// <param name="fields">List of optional fields (optional).</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>List&lt;TagForApiContract&gt;</returns>            
        public List<TagForApiContract> TagApiGetChildTags (int? tagId, string fields, string lang)
        {
            
            // verify the required parameter 'tagId' is set
            if (tagId == null) throw new ApiException(400, "Missing required parameter 'tagId' when calling TagApiGetChildTags");
            
    
            var path = "/api/tags/{tagId}/children";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tagId" + "}", ApiClient.ParameterToString(tagId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetChildTags: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetChildTags: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<TagForApiContract>) ApiClient.Deserialize(response.Content, typeof(List<TagForApiContract>), response.Headers);
        }
    
        /// <summary>
        /// Gets a list of comments for a tag.              Note: pagination and sorting might be added later. 
        /// </summary>
        /// <param name="tagId">ID of the tag whose comments to load.</param> 
        /// <returns>PartialFindResultCommentForApiContract</returns>            
        public PartialFindResultCommentForApiContract TagApiGetComments (int? tagId)
        {
            
            // verify the required parameter 'tagId' is set
            if (tagId == null) throw new ApiException(400, "Missing required parameter 'tagId' when calling TagApiGetComments");
            
    
            var path = "/api/tags/{tagId}/comments";
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetComments: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetComments: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultCommentForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultCommentForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Find tags. 
        /// </summary>
        /// <param name="query">Tag name query (optional).</param> 
        /// <param name="allowChildren">Whether to allow child tags. If this is false, only root tags (that aren&#39;t children of any other tag) will be included.</param> 
        /// <param name="categoryName">Filter tags by category (optional). If specified, this must be an exact match (case insensitive).</param> 
        /// <param name="start">First item to be retrieved (optional, defaults to 0).</param> 
        /// <param name="maxResults">Maximum number of results to be loaded (optional, defaults to 10, maximum of 30).</param> 
        /// <param name="getTotalCount">Whether to load total number of items (optional, default to false).</param> 
        /// <param name="nameMatchMode">Match mode for song name (optional, defaults to Exact).</param> 
        /// <param name="sort">Sort rule (optional, by default tags are sorted by name).Possible values are Name and UsageCount.</param> 
        /// <param name="preferAccurateMatches">Whether the search should prefer accurate matches.               If this is true, entries that match by prefix will be moved first, instead of being sorted alphabetically.              Requires a text query. Does not support pagination.              This is mostly useful for autocomplete boxes.</param> 
        /// <param name="fields">List of optional fields (optional).</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <param name="target"></param> 
        /// <returns>PartialFindResultTagForApiContract</returns>            
        public PartialFindResultTagForApiContract TagApiGetList (string query, bool? allowChildren, string categoryName, int? start, int? maxResults, bool? getTotalCount, string nameMatchMode, string sort, bool? preferAccurateMatches, string fields, string lang, string target)
        {
            
    
            var path = "/api/tags";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (allowChildren != null) queryParams.Add("allowChildren", ApiClient.ParameterToString(allowChildren)); // query parameter
 if (categoryName != null) queryParams.Add("categoryName", ApiClient.ParameterToString(categoryName)); // query parameter
 if (start != null) queryParams.Add("start", ApiClient.ParameterToString(start)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (getTotalCount != null) queryParams.Add("getTotalCount", ApiClient.ParameterToString(getTotalCount)); // query parameter
 if (nameMatchMode != null) queryParams.Add("nameMatchMode", ApiClient.ParameterToString(nameMatchMode)); // query parameter
 if (sort != null) queryParams.Add("sort", ApiClient.ParameterToString(sort)); // query parameter
 if (preferAccurateMatches != null) queryParams.Add("preferAccurateMatches", ApiClient.ParameterToString(preferAccurateMatches)); // query parameter
 if (fields != null) queryParams.Add("fields", ApiClient.ParameterToString(fields)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
 if (target != null) queryParams.Add("target", ApiClient.ParameterToString(target)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetList: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetList: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultTagForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultTagForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Find tag names by a part of name.                            Matching is done anywhere from the name. 
        /// </summary>
        /// <param name="query">Tag name query, for example \&quot;rock\&quot;.</param> 
        /// <param name="allowAliases">Whether to find tags that are aliases of other tags as well.               If false, only tags that are not aliases will be listed.</param> 
        /// <param name="maxResults">Maximum number of search results.</param> 
        /// <returns>List&lt;string&gt;</returns>            
        public List<string> TagApiGetNames (string query, bool? allowAliases, int? maxResults)
        {
            
    
            var path = "/api/tags/names";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (query != null) queryParams.Add("query", ApiClient.ParameterToString(query)); // query parameter
 if (allowAliases != null) queryParams.Add("allowAliases", ApiClient.ParameterToString(allowAliases)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetNames: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetNames: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<string>) ApiClient.Deserialize(response.Content, typeof(List<string>), response.Headers);
        }
    
        /// <summary>
        /// Gets the most common tags in a category. 
        /// </summary>
        /// <param name="categoryName">Tag category, for example \&quot;Genres\&quot;. Optional - if not specified, no filtering is done.</param> 
        /// <param name="maxResults">Maximum number of tags to return.</param> 
        /// <param name="lang">Content language preference (optional).</param> 
        /// <returns>List&lt;TagBaseContract&gt;</returns>            
        public List<TagBaseContract> TagApiGetTopTags (string categoryName, int? maxResults, string lang)
        {
            
    
            var path = "/api/tags/top";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (categoryName != null) queryParams.Add("categoryName", ApiClient.ParameterToString(categoryName)); // query parameter
 if (maxResults != null) queryParams.Add("maxResults", ApiClient.ParameterToString(maxResults)); // query parameter
 if (lang != null) queryParams.Add("lang", ApiClient.ParameterToString(lang)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetTopTags: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiGetTopTags: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<TagBaseContract>) ApiClient.Deserialize(response.Content, typeof(List<TagBaseContract>), response.Headers);
        }
    
        /// <summary>
        /// Updates a comment.              Normal users can edit their own comments, moderators can edit all comments.              Requires login. 
        /// </summary>
        /// <param name="commentId">ID of the comment to be edited.</param> 
        /// <param name="contract">New comment data. Only message can be edited.</param> 
        /// <returns></returns>            
        public void TagApiPostEditComment (int? commentId, CommentForApiContract contract)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling TagApiPostEditComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling TagApiPostEditComment");
            
    
            var path = "/api/tags/comments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiPostEditComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiPostEditComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="tagId">ID of the tag for which to create the comment.</param> 
        /// <param name="contract">Comment data. Message and author must be specified. Author must match the logged in user.</param> 
        /// <returns>CommentForApiContract</returns>            
        public CommentForApiContract TagApiPostNewComment (int? tagId, CommentForApiContract contract)
        {
            
            // verify the required parameter 'tagId' is set
            if (tagId == null) throw new ApiException(400, "Missing required parameter 'tagId' when calling TagApiPostNewComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling TagApiPostNewComment");
            
    
            var path = "/api/tags/{tagId}/comments";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tagId" + "}", ApiClient.ParameterToString(tagId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiPostNewComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiPostNewComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CommentForApiContract) ApiClient.Deserialize(response.Content, typeof(CommentForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Creates a new tag. 
        /// </summary>
        /// <param name="name">Tag English name. Tag names must be unique.</param> 
        /// <returns>TagBaseContract</returns>            
        public TagBaseContract TagApiPostNewTag (string name)
        {
            
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling TagApiPostNewTag");
            
    
            var path = "/api/tags";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (name != null) queryParams.Add("name", ApiClient.ParameterToString(name)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiPostNewTag: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiPostNewTag: " + response.ErrorMessage, response.ErrorMessage);
    
            return (TagBaseContract) ApiClient.Deserialize(response.Content, typeof(TagBaseContract), response.Headers);
        }
    
        /// <summary>
        /// Creates a new report. 
        /// </summary>
        /// <param name="tagId">Tag to be reported.</param> 
        /// <param name="reportType">Report type.</param> 
        /// <param name="notes">Notes. Optional.</param> 
        /// <param name="versionNumber">Version to be reported. Optional.</param> 
        /// <returns></returns>            
        public void TagApiPostReport (int? tagId, string reportType, string notes, int? versionNumber)
        {
            
            // verify the required parameter 'tagId' is set
            if (tagId == null) throw new ApiException(400, "Missing required parameter 'tagId' when calling TagApiPostReport");
            
            // verify the required parameter 'reportType' is set
            if (reportType == null) throw new ApiException(400, "Missing required parameter 'reportType' when calling TagApiPostReport");
            
            // verify the required parameter 'notes' is set
            if (notes == null) throw new ApiException(400, "Missing required parameter 'notes' when calling TagApiPostReport");
            
            // verify the required parameter 'versionNumber' is set
            if (versionNumber == null) throw new ApiException(400, "Missing required parameter 'versionNumber' when calling TagApiPostReport");
            
    
            var path = "/api/tags/{tagId}/reports";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "tagId" + "}", ApiClient.ParameterToString(tagId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiPostReport: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling TagApiPostReport: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}
