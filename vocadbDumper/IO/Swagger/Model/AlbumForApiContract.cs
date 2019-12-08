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
  public class AlbumForApiContract {
    /// <summary>
    /// Gets or Sets AdditionalNames
    /// </summary>
    [DataMember(Name="additionalNames", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "additionalNames")]
    public string AdditionalNames { get; set; }

    /// <summary>
    /// Gets or Sets Artists
    /// </summary>
    [DataMember(Name="artists", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artists")]
    public List<ArtistForAlbumForApiContract> Artists { get; set; }

    /// <summary>
    /// Gets or Sets ArtistString
    /// </summary>
    [DataMember(Name="artistString", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artistString")]
    public string ArtistString { get; set; }

    /// <summary>
    /// Gets or Sets Barcode
    /// </summary>
    [DataMember(Name="barcode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "barcode")]
    public string Barcode { get; set; }

    /// <summary>
    /// Gets or Sets CatalogNumber
    /// </summary>
    [DataMember(Name="catalogNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "catalogNumber")]
    public string CatalogNumber { get; set; }

    /// <summary>
    /// Gets or Sets CreateDate
    /// </summary>
    [DataMember(Name="createDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createDate")]
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// Gets or Sets DefaultName
    /// </summary>
    [DataMember(Name="defaultName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "defaultName")]
    public string DefaultName { get; set; }

    /// <summary>
    /// Gets or Sets DefaultNameLanguage
    /// </summary>
    [DataMember(Name="defaultNameLanguage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "defaultNameLanguage")]
    public string DefaultNameLanguage { get; set; }

    /// <summary>
    /// Gets or Sets Deleted
    /// </summary>
    [DataMember(Name="deleted", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "deleted")]
    public bool? Deleted { get; set; }

    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets Discs
    /// </summary>
    [DataMember(Name="discs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "discs")]
    public List<AlbumDiscPropertiesContract> Discs { get; set; }

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
    /// Gets or Sets Identifiers
    /// </summary>
    [DataMember(Name="identifiers", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "identifiers")]
    public List<AlbumIdentifierContract> Identifiers { get; set; }

    /// <summary>
    /// Gets or Sets MainPicture
    /// </summary>
    [DataMember(Name="mainPicture", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mainPicture")]
    public EntryThumbForApiContract MainPicture { get; set; }

    /// <summary>
    /// Gets or Sets MergedTo
    /// </summary>
    [DataMember(Name="mergedTo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mergedTo")]
    public int? MergedTo { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Names
    /// </summary>
    [DataMember(Name="names", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "names")]
    public List<LocalizedStringContract> Names { get; set; }

    /// <summary>
    /// Gets or Sets Pvs
    /// </summary>
    [DataMember(Name="pvs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pvs")]
    public List<PVContract> Pvs { get; set; }

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
    /// Gets or Sets Tags
    /// </summary>
    [DataMember(Name="tags", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tags")]
    public List<TagUsageForApiContract> Tags { get; set; }

    /// <summary>
    /// Gets or Sets Tracks
    /// </summary>
    [DataMember(Name="tracks", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tracks")]
    public List<SongInAlbumForApiContract> Tracks { get; set; }

    /// <summary>
    /// Gets or Sets Version
    /// </summary>
    [DataMember(Name="version", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "version")]
    public int? Version { get; set; }

    /// <summary>
    /// Gets or Sets WebLinks
    /// </summary>
    [DataMember(Name="webLinks", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "webLinks")]
    public List<WebLinkForApiContract> WebLinks { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AlbumForApiContract {\n");
      sb.Append("  AdditionalNames: ").Append(AdditionalNames).Append("\n");
      sb.Append("  Artists: ").Append(Artists).Append("\n");
      sb.Append("  ArtistString: ").Append(ArtistString).Append("\n");
      sb.Append("  Barcode: ").Append(Barcode).Append("\n");
      sb.Append("  CatalogNumber: ").Append(CatalogNumber).Append("\n");
      sb.Append("  CreateDate: ").Append(CreateDate).Append("\n");
      sb.Append("  DefaultName: ").Append(DefaultName).Append("\n");
      sb.Append("  DefaultNameLanguage: ").Append(DefaultNameLanguage).Append("\n");
      sb.Append("  Deleted: ").Append(Deleted).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  Discs: ").Append(Discs).Append("\n");
      sb.Append("  DiscType: ").Append(DiscType).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Identifiers: ").Append(Identifiers).Append("\n");
      sb.Append("  MainPicture: ").Append(MainPicture).Append("\n");
      sb.Append("  MergedTo: ").Append(MergedTo).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Names: ").Append(Names).Append("\n");
      sb.Append("  Pvs: ").Append(Pvs).Append("\n");
      sb.Append("  RatingAverage: ").Append(RatingAverage).Append("\n");
      sb.Append("  RatingCount: ").Append(RatingCount).Append("\n");
      sb.Append("  ReleaseDate: ").Append(ReleaseDate).Append("\n");
      sb.Append("  ReleaseEvent: ").Append(ReleaseEvent).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  Tags: ").Append(Tags).Append("\n");
      sb.Append("  Tracks: ").Append(Tracks).Append("\n");
      sb.Append("  Version: ").Append(Version).Append("\n");
      sb.Append("  WebLinks: ").Append(WebLinks).Append("\n");
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
