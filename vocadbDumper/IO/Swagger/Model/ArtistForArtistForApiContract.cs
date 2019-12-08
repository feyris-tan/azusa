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
  public class ArtistForArtistForApiContract {
    /// <summary>
    /// Gets or Sets Artist
    /// </summary>
    [DataMember(Name="artist", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artist")]
    public ArtistContract Artist { get; set; }

    /// <summary>
    /// Gets or Sets LinkType
    /// </summary>
    [DataMember(Name="linkType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "linkType")]
    public string LinkType { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ArtistForArtistForApiContract {\n");
      sb.Append("  Artist: ").Append(Artist).Append("\n");
      sb.Append("  LinkType: ").Append(LinkType).Append("\n");
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
