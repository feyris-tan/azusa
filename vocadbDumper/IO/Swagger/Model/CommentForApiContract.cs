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
  public class CommentForApiContract {
    /// <summary>
    /// Gets or Sets Author
    /// </summary>
    [DataMember(Name="author", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "author")]
    public UserForApiContract Author { get; set; }

    /// <summary>
    /// Gets or Sets AuthorName
    /// </summary>
    [DataMember(Name="authorName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "authorName")]
    public string AuthorName { get; set; }

    /// <summary>
    /// Gets or Sets Created
    /// </summary>
    [DataMember(Name="created", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "created")]
    public DateTime? Created { get; set; }

    /// <summary>
    /// Gets or Sets Entry
    /// </summary>
    [DataMember(Name="entry", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "entry")]
    public EntryRefContract Entry { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Message
    /// </summary>
    [DataMember(Name="message", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "message")]
    public string Message { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CommentForApiContract {\n");
      sb.Append("  Author: ").Append(Author).Append("\n");
      sb.Append("  AuthorName: ").Append(AuthorName).Append("\n");
      sb.Append("  Created: ").Append(Created).Append("\n");
      sb.Append("  Entry: ").Append(Entry).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
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
