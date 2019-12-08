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
  public class AlbumForUserForApiContract {
    /// <summary>
    /// Gets or Sets Album
    /// </summary>
    [DataMember(Name="album", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "album")]
    public AlbumForApiContract Album { get; set; }

    /// <summary>
    /// Gets or Sets MediaType
    /// </summary>
    [DataMember(Name="mediaType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mediaType")]
    public string MediaType { get; set; }

    /// <summary>
    /// Gets or Sets PurchaseStatus
    /// </summary>
    [DataMember(Name="purchaseStatus", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "purchaseStatus")]
    public string PurchaseStatus { get; set; }

    /// <summary>
    /// Gets or Sets Rating
    /// </summary>
    [DataMember(Name="rating", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "rating")]
    public int? Rating { get; set; }

    /// <summary>
    /// Gets or Sets User
    /// </summary>
    [DataMember(Name="user", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "user")]
    public UserForApiContract User { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AlbumForUserForApiContract {\n");
      sb.Append("  Album: ").Append(Album).Append("\n");
      sb.Append("  MediaType: ").Append(MediaType).Append("\n");
      sb.Append("  PurchaseStatus: ").Append(PurchaseStatus).Append("\n");
      sb.Append("  Rating: ").Append(Rating).Append("\n");
      sb.Append("  User: ").Append(User).Append("\n");
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
