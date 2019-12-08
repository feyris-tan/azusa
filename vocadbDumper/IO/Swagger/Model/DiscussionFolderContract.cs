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
  public class DiscussionFolderContract {
    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets LastTopicAuthor
    /// </summary>
    [DataMember(Name="lastTopicAuthor", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastTopicAuthor")]
    public UserForApiContract LastTopicAuthor { get; set; }

    /// <summary>
    /// Gets or Sets LastTopicDate
    /// </summary>
    [DataMember(Name="lastTopicDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lastTopicDate")]
    public DateTime? LastTopicDate { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets TopicCount
    /// </summary>
    [DataMember(Name="topicCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "topicCount")]
    public int? TopicCount { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class DiscussionFolderContract {\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  LastTopicAuthor: ").Append(LastTopicAuthor).Append("\n");
      sb.Append("  LastTopicDate: ").Append(LastTopicDate).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  TopicCount: ").Append(TopicCount).Append("\n");
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
