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
  public class EntryThumbForApiContract {
    /// <summary>
    /// Gets or Sets Mime
    /// </summary>
    [DataMember(Name="mime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mime")]
    public string Mime { get; set; }

    /// <summary>
    /// Gets or Sets UrlSmallThumb
    /// </summary>
    [DataMember(Name="urlSmallThumb", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "urlSmallThumb")]
    public string UrlSmallThumb { get; set; }

    /// <summary>
    /// Gets or Sets UrlThumb
    /// </summary>
    [DataMember(Name="urlThumb", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "urlThumb")]
    public string UrlThumb { get; set; }

    /// <summary>
    /// Gets or Sets UrlTinyThumb
    /// </summary>
    [DataMember(Name="urlTinyThumb", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "urlTinyThumb")]
    public string UrlTinyThumb { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EntryThumbForApiContract {\n");
      sb.Append("  Mime: ").Append(Mime).Append("\n");
      sb.Append("  UrlSmallThumb: ").Append(UrlSmallThumb).Append("\n");
      sb.Append("  UrlThumb: ").Append(UrlThumb).Append("\n");
      sb.Append("  UrlTinyThumb: ").Append(UrlTinyThumb).Append("\n");
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
