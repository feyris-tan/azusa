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
  public class RelatedSongsContract {
    /// <summary>
    /// Gets or Sets ArtistMatches
    /// </summary>
    [DataMember(Name="artistMatches", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artistMatches")]
    public List<SongForApiContract> ArtistMatches { get; set; }

    /// <summary>
    /// Gets or Sets LikeMatches
    /// </summary>
    [DataMember(Name="likeMatches", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "likeMatches")]
    public List<SongForApiContract> LikeMatches { get; set; }

    /// <summary>
    /// Gets or Sets TagMatches
    /// </summary>
    [DataMember(Name="tagMatches", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tagMatches")]
    public List<SongForApiContract> TagMatches { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class RelatedSongsContract {\n");
      sb.Append("  ArtistMatches: ").Append(ArtistMatches).Append("\n");
      sb.Append("  LikeMatches: ").Append(LikeMatches).Append("\n");
      sb.Append("  TagMatches: ").Append(TagMatches).Append("\n");
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
