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
  public class AlbumContract {
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
    /// Gets or Sets CoverPictureMime
    /// </summary>
    [DataMember(Name="coverPictureMime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "coverPictureMime")]
    public string CoverPictureMime { get; set; }

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
    /// Gets or Sets DiscType
    /// </summary>
    [DataMember(Name="discType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "discType")]
    public string DiscType { get; set; }

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
    /// Gets or Sets RatingAverage
    /// </summary>
    [DataMember(Name="ratingAverage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ratingAverage")]
    public double? RatingAverage { get; set; }

    /// <summary>
    /// Gets or Sets RatingCount
    /// </summary>
    [DataMember(Name="ratingCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "ratingCount")]
    public int? RatingCount { get; set; }

    /// <summary>
    /// Gets or Sets ReleaseDate
    /// </summary>
    [DataMember(Name="releaseDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "releaseDate")]
    public OptionalDateTimeContract ReleaseDate { get; set; }

    /// <summary>
    /// Gets or Sets ReleaseEvent
    /// </summary>
    [DataMember(Name="releaseEvent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "releaseEvent")]
    public ReleaseEventForApiContract ReleaseEvent { get; set; }

    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name="status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

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
      sb.Append("class AlbumContract {\n");
      sb.Append("  AdditionalNames: ").Append(AdditionalNames).Append("\n");
      sb.Append("  ArtistString: ").Append(ArtistString).Append("\n");
      sb.Append("  CoverPictureMime: ").Append(CoverPictureMime).Append("\n");
      sb.Append("  CreateDate: ").Append(CreateDate).Append("\n");
      sb.Append("  Deleted: ").Append(Deleted).Append("\n");
      sb.Append("  DiscType: ").Append(DiscType).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  RatingAverage: ").Append(RatingAverage).Append("\n");
      sb.Append("  RatingCount: ").Append(RatingCount).Append("\n");
      sb.Append("  ReleaseDate: ").Append(ReleaseDate).Append("\n");
      sb.Append("  ReleaseEvent: ").Append(ReleaseEvent).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
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
