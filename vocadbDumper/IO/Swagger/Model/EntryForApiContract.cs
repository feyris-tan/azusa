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
  public class EntryForApiContract {
    /// <summary>
    /// Gets or Sets ActivityDate
    /// </summary>
    [DataMember(Name="activityDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "activityDate")]
    public DateTime? ActivityDate { get; set; }

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
    /// Gets or Sets ArtistType
    /// </summary>
    [DataMember(Name="artistType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artistType")]
    public string ArtistType { get; set; }

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
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets DiscType
    /// </summary>
    [DataMember(Name="discType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "discType")]
    public string DiscType { get; set; }

    /// <summary>
    /// Gets or Sets EntryType
    /// </summary>
    [DataMember(Name="entryType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "entryType")]
    public string EntryType { get; set; }

    /// <summary>
    /// Gets or Sets EventCategory
    /// </summary>
    [DataMember(Name="eventCategory", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "eventCategory")]
    public string EventCategory { get; set; }

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
    /// Gets or Sets PVs
    /// </summary>
    [DataMember(Name="pVs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pVs")]
    public List<PVContract> PVs { get; set; }

    /// <summary>
    /// Gets or Sets SongListFeaturedCategory
    /// </summary>
    [DataMember(Name="songListFeaturedCategory", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "songListFeaturedCategory")]
    public string SongListFeaturedCategory { get; set; }

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
    /// Gets or Sets ReleaseEventSeriesName
    /// </summary>
    [DataMember(Name="releaseEventSeriesName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "releaseEventSeriesName")]
    public string ReleaseEventSeriesName { get; set; }

    /// <summary>
    /// Gets or Sets TagCategoryName
    /// </summary>
    [DataMember(Name="tagCategoryName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tagCategoryName")]
    public string TagCategoryName { get; set; }

    /// <summary>
    /// Gets or Sets Tags
    /// </summary>
    [DataMember(Name="tags", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tags")]
    public List<TagUsageForApiContract> Tags { get; set; }

    /// <summary>
    /// Gets or Sets UrlSlug
    /// </summary>
    [DataMember(Name="urlSlug", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "urlSlug")]
    public string UrlSlug { get; set; }

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
    public List<ArchivedWebLinkContract> WebLinks { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EntryForApiContract {\n");
      sb.Append("  ActivityDate: ").Append(ActivityDate).Append("\n");
      sb.Append("  AdditionalNames: ").Append(AdditionalNames).Append("\n");
      sb.Append("  ArtistString: ").Append(ArtistString).Append("\n");
      sb.Append("  ArtistType: ").Append(ArtistType).Append("\n");
      sb.Append("  CreateDate: ").Append(CreateDate).Append("\n");
      sb.Append("  DefaultName: ").Append(DefaultName).Append("\n");
      sb.Append("  DefaultNameLanguage: ").Append(DefaultNameLanguage).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  DiscType: ").Append(DiscType).Append("\n");
      sb.Append("  EntryType: ").Append(EntryType).Append("\n");
      sb.Append("  EventCategory: ").Append(EventCategory).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  MainPicture: ").Append(MainPicture).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Names: ").Append(Names).Append("\n");
      sb.Append("  PVs: ").Append(PVs).Append("\n");
      sb.Append("  SongListFeaturedCategory: ").Append(SongListFeaturedCategory).Append("\n");
      sb.Append("  SongType: ").Append(SongType).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  ReleaseEventSeriesName: ").Append(ReleaseEventSeriesName).Append("\n");
      sb.Append("  TagCategoryName: ").Append(TagCategoryName).Append("\n");
      sb.Append("  Tags: ").Append(Tags).Append("\n");
      sb.Append("  UrlSlug: ").Append(UrlSlug).Append("\n");
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
