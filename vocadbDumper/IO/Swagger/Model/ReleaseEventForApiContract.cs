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
  public class ReleaseEventForApiContract {
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
    public List<ArtistForEventContract> Artists { get; set; }

    /// <summary>
    /// Gets or Sets Category
    /// </summary>
    [DataMember(Name="category", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "category")]
    public string Category { get; set; }

    /// <summary>
    /// Gets or Sets Date
    /// </summary>
    [DataMember(Name="date", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "date")]
    public DateTime? Date { get; set; }

    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

    /// <summary>
    /// Gets or Sets EndDate
    /// </summary>
    [DataMember(Name="endDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "endDate")]
    public DateTime? EndDate { get; set; }

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
    /// Gets or Sets Series
    /// </summary>
    [DataMember(Name="series", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "series")]
    public ReleaseEventSeriesContract Series { get; set; }

    /// <summary>
    /// Gets or Sets SeriesId
    /// </summary>
    [DataMember(Name="seriesId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "seriesId")]
    public int? SeriesId { get; set; }

    /// <summary>
    /// Gets or Sets SeriesNumber
    /// </summary>
    [DataMember(Name="seriesNumber", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "seriesNumber")]
    public int? SeriesNumber { get; set; }

    /// <summary>
    /// Gets or Sets SeriesSuffix
    /// </summary>
    [DataMember(Name="seriesSuffix", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "seriesSuffix")]
    public string SeriesSuffix { get; set; }

    /// <summary>
    /// Gets or Sets SongList
    /// </summary>
    [DataMember(Name="songList", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "songList")]
    public SongListBaseContract SongList { get; set; }

    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name="status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

    /// <summary>
    /// Gets or Sets UrlSlug
    /// </summary>
    [DataMember(Name="urlSlug", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "urlSlug")]
    public string UrlSlug { get; set; }

    /// <summary>
    /// Gets or Sets VenueName
    /// </summary>
    [DataMember(Name="venueName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "venueName")]
    public string VenueName { get; set; }

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
      sb.Append("class ReleaseEventForApiContract {\n");
      sb.Append("  AdditionalNames: ").Append(AdditionalNames).Append("\n");
      sb.Append("  Artists: ").Append(Artists).Append("\n");
      sb.Append("  Category: ").Append(Category).Append("\n");
      sb.Append("  Date: ").Append(Date).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  EndDate: ").Append(EndDate).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  MainPicture: ").Append(MainPicture).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Names: ").Append(Names).Append("\n");
      sb.Append("  Series: ").Append(Series).Append("\n");
      sb.Append("  SeriesId: ").Append(SeriesId).Append("\n");
      sb.Append("  SeriesNumber: ").Append(SeriesNumber).Append("\n");
      sb.Append("  SeriesSuffix: ").Append(SeriesSuffix).Append("\n");
      sb.Append("  SongList: ").Append(SongList).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  UrlSlug: ").Append(UrlSlug).Append("\n");
      sb.Append("  VenueName: ").Append(VenueName).Append("\n");
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
