using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class DiscussionTopicContract {
    /// <summary>
    /// Gets or Sets Author
    /// </summary>
    [DataMember(Name="author", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "author")]
    public UserForApiContract Author { get; set; }

    /// <summary>
    /// Gets or Sets CommentCount
    /// </summary>
    [DataMember(Name="commentCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "commentCount")]
    public int? CommentCount { get; set; }

    /// <summary>
    /// Gets or Sets Comments
    /// </summary>
    [DataMember(Name="comments", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "comments")]
    public List<CommentForApiContract> Comments { get; set; }

    /// <summary>
    /// Gets or Sets Content
    /// </summary>
    [DataMember(Name="content", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "content")]
    public string Content { get; set; }

    /// <summary>
    /// Gets or Sets Created
    /// </summary>
    [DataMember(Name="created", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "created")]
    public DateTime? Created { get; set; }

    /// <summary>
    /// Gets or Sets FolderId
    /// </summary>
    [DataMember(Name="folderId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "folderId")]
    public int? FolderId { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets LastComment
    /// </summary>
    [DataMember(Name="lastComment", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastComment")]
    public CommentForApiContract LastComment { get; set; }

    /// <summary>
    /// Gets or Sets Locked
    /// </summary>
    [DataMember(Name="locked", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "locked")]
    public bool? Locked { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DiscussionTopicContract {\n");
      sb.Append("  Author: ").Append(Author).Append("\n");
      sb.Append("  CommentCount: ").Append(CommentCount).Append("\n");
      sb.Append("  Comments: ").Append(Comments).Append("\n");
      sb.Append("  Content: ").Append(Content).Append("\n");
      sb.Append("  Created: ").Append(Created).Append("\n");
      sb.Append("  FolderId: ").Append(FolderId).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  LastComment: ").Append(LastComment).Append("\n");
      sb.Append("  Locked: ").Append(Locked).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
