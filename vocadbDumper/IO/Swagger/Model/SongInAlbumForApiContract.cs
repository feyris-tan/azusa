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
  public class SongInAlbumForApiContract {
    /// <summary>
    /// Gets or Sets DiscNumber
    /// </summary>
    [DataMember(Name="discNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "discNumber")]
    public int? DiscNumber { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Song
    /// </summary>
    [DataMember(Name="song", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "song")]
    public SongForApiContract Song { get; set; }

    /// <summary>
    /// Gets or Sets TrackNumber
    /// </summary>
    [DataMember(Name="trackNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "trackNumber")]
    public int? TrackNumber { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SongInAlbumForApiContract {\n");
      sb.Append("  DiscNumber: ").Append(DiscNumber).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Song: ").Append(Song).Append("\n");
      sb.Append("  TrackNumber: ").Append(TrackNumber).Append("\n");
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
