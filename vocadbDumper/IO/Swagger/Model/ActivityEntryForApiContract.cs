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
  public class ActivityEntryForApiContract {
    /// <summary>
    /// Gets or Sets ArchivedVersion
    /// </summary>
    [DataMember(Name="archivedVersion", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "archivedVersion")]
    public ArchivedObjectVersionForApiContract ArchivedVersion { get; set; }

    /// <summary>
    /// Gets or Sets Author
    /// </summary>
    [DataMember(Name="author", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "author")]
    public UserForApiContract Author { get; set; }

    /// <summary>
    /// Gets or Sets CreateDate
    /// </summary>
    [DataMember(Name="createDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createDate")]
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// Gets or Sets EditEvent
    /// </summary>
    [DataMember(Name="editEvent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "editEvent")]
    public string EditEvent { get; set; }

    /// <summary>
    /// Gets or Sets Entry
    /// </summary>
    [DataMember(Name="entry", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "entry")]
    public EntryForApiContract Entry { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ActivityEntryForApiContract {\n");
      sb.Append("  ArchivedVersion: ").Append(ArchivedVersion).Append("\n");
      sb.Append("  Author: ").Append(Author).Append("\n");
      sb.Append("  CreateDate: ").Append(CreateDate).Append("\n");
      sb.Append("  EditEvent: ").Append(EditEvent).Append("\n");
      sb.Append("  Entry: ").Append(Entry).Append("\n");
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
