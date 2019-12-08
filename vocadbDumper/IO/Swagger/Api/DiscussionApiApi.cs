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
    public interface IDiscussionApiApi
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        void DiscussionApiDeleteComment (int? commentId);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        void DiscussionApiDeleteTopic (int? topicId);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="fields"></param>
        /// <returns>List&lt;DiscussionFolderContract&gt;</returns>
        List<DiscussionFolderContract> DiscussionApiGetFolders (string fields);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="fields"></param>
        /// <returns>DiscussionTopicContract</returns>
        DiscussionTopicContract DiscussionApiGetTopic (int? topicId, string fields);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="start"></param>
        /// <param name="maxResults"></param>
        /// <param name="getTotalCount"></param>
        /// <param name="sort"></param>
        /// <param name="fields"></param>
        /// <returns>PartialFindResultDiscussionTopicContract</returns>
        PartialFindResultDiscussionTopicContract DiscussionApiGetTopics (int? folderId, int? start, int? maxResults, bool? getTotalCount, string sort, string fields);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="fields"></param>
        /// <returns>List&lt;DiscussionTopicContract&gt;</returns>
        List<DiscussionTopicContract> DiscussionApiGetTopicsForFolder (int? folderId, string fields);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        void DiscussionApiPostEditComment (int? commentId, CommentForApiContract contract);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        void DiscussionApiPostEditTopic (int? topicId, DiscussionTopicContract contract);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="contract"></param>
        /// <returns>CommentForApiContract</returns>
        CommentForApiContract DiscussionApiPostNewComment (int? topicId, CommentForApiContract contract);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="contract"></param>
        /// <returns>DiscussionFolderContract</returns>
        DiscussionFolderContract DiscussionApiPostNewFolder (DiscussionFolderContract contract);
        /// <summary>
        ///  
        /// </summary>
        /// <param name="folderId"></param>
        /// <param name="contract"></param>
        /// <returns>DiscussionTopicContract</returns>
        DiscussionTopicContract DiscussionApiPostNewTopic (int? folderId, DiscussionTopicContract contract);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class DiscussionApiApi : IDiscussionApiApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscussionApiApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public DiscussionApiApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscussionApiApi"/> class.
        /// </summary>
        /// <returns></returns>
        public DiscussionApiApi(String basePath)
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
        /// <param name="commentId"></param> 
        /// <returns></returns>            
        public void DiscussionApiDeleteComment (int? commentId)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling DiscussionApiDeleteComment");
            
    
            var path = "/api/discussions/comments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiDeleteComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiDeleteComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="topicId"></param> 
        /// <returns></returns>            
        public void DiscussionApiDeleteTopic (int? topicId)
        {
            
            // verify the required parameter 'topicId' is set
            if (topicId == null) throw new ApiException(400, "Missing required parameter 'topicId' when calling DiscussionApiDeleteTopic");
            
    
            var path = "/api/discussions/topics/{topicId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "topicId" + "}", ApiClient.ParameterToString(topicId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiDeleteTopic: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiDeleteTopic: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="fields"></param> 
        /// <returns>List&lt;DiscussionFolderContract&gt;</returns>            
        public List<DiscussionFolderContract> DiscussionApiGetFolders (string fields)
        {
            
    
            var path = "/api/discussions/folders";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiGetFolders: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiGetFolders: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<DiscussionFolderContract>) ApiClient.Deserialize(response.Content, typeof(List<DiscussionFolderContract>), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="topicId"></param> 
        /// <param name="fields"></param> 
        /// <returns>DiscussionTopicContract</returns>            
        public DiscussionTopicContract DiscussionApiGetTopic (int? topicId, string fields)
        {
            
            // verify the required parameter 'topicId' is set
            if (topicId == null) throw new ApiException(400, "Missing required parameter 'topicId' when calling DiscussionApiGetTopic");
            
    
            var path = "/api/discussions/topics/{topicId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "topicId" + "}", ApiClient.ParameterToString(topicId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiGetTopic: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiGetTopic: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DiscussionTopicContract) ApiClient.Deserialize(response.Content, typeof(DiscussionTopicContract), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="folderId"></param> 
        /// <param name="start"></param> 
        /// <param name="maxResults"></param> 
        /// <param name="getTotalCount"></param> 
        /// <param name="sort"></param> 
        /// <param name="fields"></param> 
        /// <returns>PartialFindResultDiscussionTopicContract</returns>            
        public PartialFindResultDiscussionTopicContract DiscussionApiGetTopics (int? folderId, int? start, int? maxResults, bool? getTotalCount, string sort, string fields)
        {
            
    
            var path = "/api/discussions/topics";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (folderId != null) queryParams.Add("folderId", ApiClient.ParameterToString(folderId)); // query parameter
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
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiGetTopics: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiGetTopics: " + response.ErrorMessage, response.ErrorMessage);
    
            return (PartialFindResultDiscussionTopicContract) ApiClient.Deserialize(response.Content, typeof(PartialFindResultDiscussionTopicContract), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="folderId"></param> 
        /// <param name="fields"></param> 
        /// <returns>List&lt;DiscussionTopicContract&gt;</returns>            
        public List<DiscussionTopicContract> DiscussionApiGetTopicsForFolder (int? folderId, string fields)
        {
            
            // verify the required parameter 'folderId' is set
            if (folderId == null) throw new ApiException(400, "Missing required parameter 'folderId' when calling DiscussionApiGetTopicsForFolder");
            
    
            var path = "/api/discussions/folders/{folderId}/topics";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "folderId" + "}", ApiClient.ParameterToString(folderId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiGetTopicsForFolder: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiGetTopicsForFolder: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<DiscussionTopicContract>) ApiClient.Deserialize(response.Content, typeof(List<DiscussionTopicContract>), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="commentId"></param> 
        /// <param name="contract"></param> 
        /// <returns></returns>            
        public void DiscussionApiPostEditComment (int? commentId, CommentForApiContract contract)
        {
            
            // verify the required parameter 'commentId' is set
            if (commentId == null) throw new ApiException(400, "Missing required parameter 'commentId' when calling DiscussionApiPostEditComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling DiscussionApiPostEditComment");
            
    
            var path = "/api/discussions/comments/{commentId}";
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
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiPostEditComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiPostEditComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="topicId"></param> 
        /// <param name="contract"></param> 
        /// <returns></returns>            
        public void DiscussionApiPostEditTopic (int? topicId, DiscussionTopicContract contract)
        {
            
            // verify the required parameter 'topicId' is set
            if (topicId == null) throw new ApiException(400, "Missing required parameter 'topicId' when calling DiscussionApiPostEditTopic");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling DiscussionApiPostEditTopic");
            
    
            var path = "/api/discussions/topics/{topicId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "topicId" + "}", ApiClient.ParameterToString(topicId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiPostEditTopic: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiPostEditTopic: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="topicId"></param> 
        /// <param name="contract"></param> 
        /// <returns>CommentForApiContract</returns>            
        public CommentForApiContract DiscussionApiPostNewComment (int? topicId, CommentForApiContract contract)
        {
            
            // verify the required parameter 'topicId' is set
            if (topicId == null) throw new ApiException(400, "Missing required parameter 'topicId' when calling DiscussionApiPostNewComment");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling DiscussionApiPostNewComment");
            
    
            var path = "/api/discussions/topics/{topicId}/comments";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "topicId" + "}", ApiClient.ParameterToString(topicId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiPostNewComment: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiPostNewComment: " + response.ErrorMessage, response.ErrorMessage);
    
            return (CommentForApiContract) ApiClient.Deserialize(response.Content, typeof(CommentForApiContract), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="contract"></param> 
        /// <returns>DiscussionFolderContract</returns>            
        public DiscussionFolderContract DiscussionApiPostNewFolder (DiscussionFolderContract contract)
        {
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling DiscussionApiPostNewFolder");
            
    
            var path = "/api/discussions/folders";
            path = path.Replace("{format}", "json");
                
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
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiPostNewFolder: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiPostNewFolder: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DiscussionFolderContract) ApiClient.Deserialize(response.Content, typeof(DiscussionFolderContract), response.Headers);
        }
    
        /// <summary>
        ///  
        /// </summary>
        /// <param name="folderId"></param> 
        /// <param name="contract"></param> 
        /// <returns>DiscussionTopicContract</returns>            
        public DiscussionTopicContract DiscussionApiPostNewTopic (int? folderId, DiscussionTopicContract contract)
        {
            
            // verify the required parameter 'folderId' is set
            if (folderId == null) throw new ApiException(400, "Missing required parameter 'folderId' when calling DiscussionApiPostNewTopic");
            
            // verify the required parameter 'contract' is set
            if (contract == null) throw new ApiException(400, "Missing required parameter 'contract' when calling DiscussionApiPostNewTopic");
            
    
            var path = "/api/discussions/folders/{folderId}/topics";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "folderId" + "}", ApiClient.ParameterToString(folderId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiPostNewTopic: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DiscussionApiPostNewTopic: " + response.ErrorMessage, response.ErrorMessage);
    
            return (DiscussionTopicContract) ApiClient.Deserialize(response.Content, typeof(DiscussionTopicContract), response.Headers);
        }
    
    }
}
