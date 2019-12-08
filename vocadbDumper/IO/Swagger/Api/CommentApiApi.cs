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
    public interface ICommentApiApi
    {
        /// <summary>
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="entryType">Entry type.</param>
        /// <param name="commentId">ID of the comment to be deleted.</param>
        /// <returns></returns>
        void CommentApiDeleteComment (string entryType, int? commentId);
        /// <summary>
        /// Gets a list of comments for an entry. 
        /// </summary>
        /// <param name="entryType">Entry type.</param>
        /// <param name="entryId">ID of the entry whose comments to load.</param>
        /// <returns>PartialFindResultCommentForApiContract</returns>
        PartialFindResultCommentForApiContract CommentApiGetComments (string entryType, int? entryId);
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="entryType">Entry type.</param>
        /// <param name="commentId">ID of the comment to be edited.</param>
        /// <param name="contract">New comment data. Only message can be edited.</param>
        /// <returns></returns>
        void CommentApiPostEditComment (string entryType, int? commentId, CommentForApiContract contract);
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="entryType">Entry type.</param>
        /// <param name="contract">Comment data. Message, entry and author must be specified. Author must match the logged in user.</param>
        /// <returns>CommentForApiContract</returns>
        CommentForApiContract CommentApiPostNewComment (string entryType, CommentForApiContract contract);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class CommentApiApi : ICommentApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public CommentApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public CommentApiApi(String basePath)
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
        /// Deletes a comment. Normal users can delete their own comments, moderators can delete all comments.              Requires login.
        /// </summary>
        /// <param name="entryType">Entry type.</param> 
        /// <param name="commentId">ID of the comment to be deleted.</param> 
        /// <returns></returns>            
        public void CommentApiDeleteComment (string entryType, int? commentId)
        {
            
            // verify the required parameter 'entryType' is set
            if (entryType == null) throw new ApiException(400, "Missing required parameter 'entryType' when calling CommentApiDeleteComment");
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling CommentApiDeleteComment");
            
    
            var path = "/api/comments/{entryType}-comments/{commentId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "entryType" + "}", ApiClient.ParameterToString(entryType));
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
                throw new ApiException ((int)response.StatusCode, "Error calling CommentApiDeleteComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CommentApiDeleteComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Gets a list of comments for an entry. 
        /// </summary>
        /// <param name="entryType">Entry type.</param> 
        /// <param name="entryId">ID of the entry whose comments to load.</param> 
        /// <returns>PartialFindResultCommentForApiContract</returns>            
        public PartialFindResultCommentForApiContract CommentApiGetComments (string entryType, int? entryId)
        {
            
            // verify the required parameter 'entryType' is set
            if (entryType == null) throw new ApiException(400, "Missing required parameter 'entryType' when calling CommentApiGetComments");
            
            // verify the required parameter 'entryId' is set
            if (entryId == null) throw new ApiException(400, "Missing required parameter 'entryId' when calling CommentApiGetComments");
            
    
            var path = "/api/comments/{entryType}-comments";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "entryType" + "}", ApiClient.ParameterToString(entryType));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (entryId != null) queryParams.Add("entryId", ApiClient.ParameterToString(entryId)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CommentApiGetComments: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CommentApiGetComments: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultCommentForApiContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultCommentForApiContract), response.Headers);
        }
    
        /// <summary>
        /// Updates a comment. Normal users can edit their own comments, moderators can edit all comments.              Requires login.
        /// </summary>
        /// <param name="entryType">Entry type.</param> 
        /// <param name="commentId">ID of the comment to be edited.</param> 
        /// <param name="contract">New comment data. Only message can be edited.</param> 
        /// <returns></returns>            
        public void CommentApiPostEditComment (string entryType, int? commentId, CommentForApiContract contract)
        {
            
            // verify the required parameter 'entryType' is set
            if (entryType == null) throw new ApiException(400, "Missing required parameter 'entryType' when calling CommentApiPostEditComment");
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling CommentApiPostEditComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling CommentApiPostEditComment");
            
    
            var path = "/api/comments/{entryType}-comments/{commentId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "entryType" + "}", ApiClient.ParameterToString(entryType));
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
                throw new ApiException ((int)response.StatusCode, "Error calling CommentApiPostEditComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CommentApiPostEditComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Posts a new comment. 
        /// </summary>
        /// <param name="entryType">Entry type.</param> 
        /// <param name="contract">Comment data. Message, entry and author must be specified. Author must match the logged in user.</param> 
        /// <returns>CommentForApiContract</returns>            
        public CommentForApiContract CommentApiPostNewComment (string entryType, CommentForApiContract contract)
        {
            
            // verify the required parameter 'entryType' is set
            if (entryType == null) throw new ApiException(400, "Missing required parameter 'entryType' when calling CommentApiPostNewComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling CommentApiPostNewComment");
            
    
            var path = "/api/comments/{entryType}-comments";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "entryType" + "}", ApiClient.ParameterToString(entryType));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling CommentApiPostNewComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CommentApiPostNewComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CommentForApiContract) ApiClient.Deserialize(response.Content, typeof(CommentForApiContract), response.Headers);
        }
    
    }
}
