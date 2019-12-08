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
  public class EntryThumbContract {
    /// <summary>
    /// Gets or Sets EntryType
    /// </summary>
    [DataMember(Name="entryType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "entryType")]
    public string EntryType { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Mime
    /// </summary>
    [DataMember(Name="mime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mime")]
    public string Mime { get; set; }

    /// <summary>
    /// Gets or Sets Version
    /// </summary>
    [DataMember(Name="version", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "version")]
    public int? Version { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EntryThumbContract {\n");
      sb.Append("  EntryType: ").Append(EntryType).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Mime: ").Append(Mime).Append("\n");
      sb.Append("  Version: ").Append(Version).Append("\n");
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
