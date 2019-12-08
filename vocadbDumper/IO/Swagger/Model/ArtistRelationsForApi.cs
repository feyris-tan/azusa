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
  public class ArtistRelationsForApi {
    /// <summary>
    /// Gets or Sets LatestAlbums
    /// </summary>
    [DataMember(Name="latestAlbums", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "latestAlbums")]
    public List<AlbumForApiContract> LatestAlbums { get; set; }

    /// <summary>
    /// Gets or Sets LatestEvents
    /// </summary>
    [DataMember(Name="latestEvents", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "latestEvents")]
    public List<ReleaseEventForApiContract> LatestEvents { get; set; }

    /// <summary>
    /// Gets or Sets LatestSongs
    /// </summary>
    [DataMember(Name="latestSongs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "latestSongs")]
    public List<SongForApiContract> LatestSongs { get; set; }

    /// <summary>
    /// Gets or Sets PopularAlbums
    /// </summary>
    [DataMember(Name="popularAlbums", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "popularAlbums")]
    public List<AlbumForApiContract> PopularAlbums { get; set; }

    /// <summary>
    /// Gets or Sets PopularSongs
    /// </summary>
    [DataMember(Name="popularSongs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "popularSongs")]
    public List<SongForApiContract> PopularSongs { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ArtistRelationsForApi {\n");
      sb.Append("  LatestAlbums: ").Append(LatestAlbums).Append("\n");
      sb.Append("  LatestEvents: ").Append(LatestEvents).Append("\n");
      sb.Append("  LatestSongs: ").Append(LatestSongs).Append("\n");
      sb.Append("  PopularAlbums: ").Append(PopularAlbums).Append("\n");
      sb.Append("  PopularSongs: ").Append(PopularSongs).Append("\n");
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
