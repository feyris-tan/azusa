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
  public class SongForApiContract {
    /// <summary>
    /// Gets or Sets AdditionalNames
    /// </summary>
    [DataMember(Name="additionalNames", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "additionalNames")]
    public string AdditionalNames { get; set; }

    /// <summary>
    /// Gets or Sets Albums
    /// </summary>
    [DataMember(Name="albums", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "albums")]
    public List<AlbumContract> Albums { get; set; }

    /// <summary>
    /// Gets or Sets Artists
    /// </summary>
    [DataMember(Name="artists", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artists")]
    public List<ArtistForSongContract> Artists { get; set; }

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
    /// Gets or Sets Lyrics
    /// </summary>
    [DataMember(Name="lyrics", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "lyrics")]
    public List<LyricsForSongContract> Lyrics { get; set; }

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
    /// Gets or Sets OriginalVersionId
    /// </summary>
    [DataMember(Name="originalVersionId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "originalVersionId")]
    public int? OriginalVersionId { get; set; }

    /// <summary>
    /// Gets or Sets PublishDate
    /// </summary>
    [DataMember(Name="publishDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "publishDate")]
    public DateTime? PublishDate { get; set; }

    /// <summary>
    /// Gets or Sets Pvs
    /// </summary>
    [DataMember(Name="pvs", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pvs")]
    public List<PVContract> Pvs { get; set; }

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
    /// Gets or Sets ReleaseEvent
    /// </summary>
    [DataMember(Name="releaseEvent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "releaseEvent")]
    public ReleaseEventForApiContract ReleaseEvent { get; set; }

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
    /// Gets or Sets Tags
    /// </summary>
    [DataMember(Name="tags", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "tags")]
    public List<TagUsageForApiContract> Tags { get; set; }

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
      sb.Append("class SongForApiContract {\n");
      sb.Append("  AdditionalNames: ").Append(AdditionalNames).Append("\n");
      sb.Append("  Albums: ").Append(Albums).Append("\n");
      sb.Append("  Artists: ").Append(Artists).Append("\n");
      sb.Append("  ArtistString: ").Append(ArtistString).Append("\n");
      sb.Append("  CreateDate: ").Append(CreateDate).Append("\n");
      sb.Append("  DefaultName: ").Append(DefaultName).Append("\n");
      sb.Append("  DefaultNameLanguage: ").Append(DefaultNameLanguage).Append("\n");
      sb.Append("  Deleted: ").Append(Deleted).Append("\n");
      sb.Append("  FavoritedTimes: ").Append(FavoritedTimes).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  LengthSeconds: ").Append(LengthSeconds).Append("\n");
      sb.Append("  Lyrics: ").Append(Lyrics).Append("\n");
      sb.Append("  MainPicture: ").Append(MainPicture).Append("\n");
      sb.Append("  MergedTo: ").Append(MergedTo).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Names: ").Append(Names).Append("\n");
      sb.Append("  OriginalVersionId: ").Append(OriginalVersionId).Append("\n");
      sb.Append("  PublishDate: ").Append(PublishDate).Append("\n");
      sb.Append("  Pvs: ").Append(Pvs).Append("\n");
      sb.Append("  PvServices: ").Append(PvServices).Append("\n");
      sb.Append("  RatingScore: ").Append(RatingScore).Append("\n");
      sb.Append("  ReleaseEvent: ").Append(ReleaseEvent).Append("\n");
      sb.Append("  SongType: ").Append(SongType).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  Tags: ").Append(Tags).Append("\n");
      sb.Append("  ThumbUrl: ").Append(ThumbUrl).Append("\n");
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
