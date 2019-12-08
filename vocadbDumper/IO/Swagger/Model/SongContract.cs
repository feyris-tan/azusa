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
  public class SongContract {
    /// <summary>
    /// Gets or Sets AdditionalNames
    /// </summary>
    [DataMember(Name="additionalNames", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "additionalNames")]
    public string AdditionalNames { get; set; }

    /// <summary>
    /// Gets or Sets ArtistString
    /// </summary>
    [DataMember(Name="artistString", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artistString")]
    public string ArtistString { get; set; }

    /// <summary>
    /// Gets or Sets CreateDate
    /// </summary>
    [DataMember(Name="createDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createDate")]
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// Gets or Sets Deleted
    /// </summary>
    [DataMember(Name="deleted", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "deleted")]
    public bool? Deleted { get; set; }

    /// <summary>
    /// Gets or Sets FavoritedTimes
    /// </summary>
    [DataMember(Name="favoritedTimes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "favoritedTimes")]
    public int? FavoritedTimes { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets LengthSeconds
    /// </summary>
    [DataMember(Name="lengthSeconds", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lengthSeconds")]
    public int? LengthSeconds { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets NicoId
    /// </summary>
    [DataMember(Name="nicoId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "nicoId")]
    public string NicoId { get; set; }

    /// <summary>
    /// Gets or Sets PublishDate
    /// </summary>
    [DataMember(Name="publishDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "publishDate")]
    public DateTime? PublishDate { get; set; }

    /// <summary>
    /// Gets or Sets PvServices
    /// </summary>
    [DataMember(Name="pvServices", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pvServices")]
    public string PvServices { get; set; }

    /// <summary>
    /// Gets or Sets RatingScore
    /// </summary>
    [DataMember(Name="ratingScore", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ratingScore")]
    public int? RatingScore { get; set; }

    /// <summary>
    /// Gets or Sets SongType
    /// </summary>
    [DataMember(Name="songType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "songType")]
    public string SongType { get; set; }

    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name="status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

    /// <summary>
    /// Gets or Sets ThumbUrl
    /// </summary>
    [DataMember(Name="thumbUrl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "thumbUrl")]
    public string ThumbUrl { get; set; }

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
      sb.Append("class SongContract {\n");
      sb.Append("  AdditionalNames: ").Append(AdditionalNames).Append("\n");
      sb.Append("  ArtistString: ").Append(ArtistString).Append("\n");
      sb.Append("  CreateDate: ").Append(CreateDate).Append("\n");
      sb.Append("  Deleted: ").Append(Deleted).Append("\n");
      sb.Append("  FavoritedTimes: ").Append(FavoritedTimes).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  LengthSeconds: ").Append(LengthSeconds).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  NicoId: ").Append(NicoId).Append("\n");
      sb.Append("  PublishDate: ").Append(PublishDate).Append("\n");
      sb.Append("  PvServices: ").Append(PvServices).Append("\n");
      sb.Append("  RatingScore: ").Append(RatingScore).Append("\n");
      sb.Append("  SongType: ").Append(SongType).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  ThumbUrl: ").Append(ThumbUrl).Append("\n");
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
