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
  public class ArtistForApiContract {
    /// <summary>
    /// Gets or Sets AdditionalNames
    /// </summary>
    [DataMember(Name="additionalNames", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "additionalNames")]
    public string AdditionalNames { get; set; }

    /// <summary>
    /// Gets or Sets ArtistLinks
    /// </summary>
    [DataMember(Name="artistLinks", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artistLinks")]
    public List<ArtistForArtistForApiContract> ArtistLinks { get; set; }

    /// <summary>
    /// Gets or Sets ArtistLinksReverse
    /// </summary>
    [DataMember(Name="artistLinksReverse", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artistLinksReverse")]
    public List<ArtistForArtistForApiContract> ArtistLinksReverse { get; set; }

    /// <summary>
    /// Gets or Sets ArtistType
    /// </summary>
    [DataMember(Name="artistType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artistType")]
    public string ArtistType { get; set; }

    /// <summary>
    /// Gets or Sets BaseVoicebank
    /// </summary>
    [DataMember(Name="baseVoicebank", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "baseVoicebank")]
    public ArtistContract BaseVoicebank { get; set; }

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
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

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
    /// Gets or Sets PictureMime
    /// </summary>
    [DataMember(Name="pictureMime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pictureMime")]
    public string PictureMime { get; set; }

    /// <summary>
    /// Gets or Sets Relations
    /// </summary>
    [DataMember(Name="relations", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "relations")]
    public ArtistRelationsForApi Relations { get; set; }

    /// <summary>
    /// Gets or Sets ReleaseDate
    /// </summary>
    [DataMember(Name="releaseDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "releaseDate")]
    public DateTime? ReleaseDate { get; set; }

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
      sb.Append("class ArtistForApiContract {\n");
      sb.Append("  AdditionalNames: ").Append(AdditionalNames).Append("\n");
      sb.Append("  ArtistLinks: ").Append(ArtistLinks).Append("\n");
      sb.Append("  ArtistLinksReverse: ").Append(ArtistLinksReverse).Append("\n");
      sb.Append("  ArtistType: ").Append(ArtistType).Append("\n");
      sb.Append("  BaseVoicebank: ").Append(BaseVoicebank).Append("\n");
      sb.Append("  CreateDate: ").Append(CreateDate).Append("\n");
      sb.Append("  DefaultName: ").Append(DefaultName).Append("\n");
      sb.Append("  DefaultNameLanguage: ").Append(DefaultNameLanguage).Append("\n");
      sb.Append("  Deleted: ").Append(Deleted).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  MainPicture: ").Append(MainPicture).Append("\n");
      sb.Append("  MergedTo: ").Append(MergedTo).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Names: ").Append(Names).Append("\n");
      sb.Append("  PictureMime: ").Append(PictureMime).Append("\n");
      sb.Append("  Relations: ").Append(Relations).Append("\n");
      sb.Append("  ReleaseDate: ").Append(ReleaseDate).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  Tags: ").Append(Tags).Append("\n");
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
