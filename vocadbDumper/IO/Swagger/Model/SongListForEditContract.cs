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
  public class SongListForEditContract {
    /// <summary>
    /// Gets or Sets SongLinks
    /// </summary>
    [DataMember(Name="songLinks", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "songLinks")]
    public List<SongInListEditContract> SongLinks { get; set; }

    /// <summary>
    /// Gets or Sets UpdateNotes
    /// </summary>
    [DataMember(Name="updateNotes", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "updateNotes")]
    public string UpdateNotes { get; set; }

    /// <summary>
    /// Gets or Sets Author
    /// </summary>
    [DataMember(Name="author", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "author")]
    public UserWithEmailContract Author { get; set; }

    /// <summary>
    /// Gets or Sets CanEdit
    /// </summary>
    [DataMember(Name="canEdit", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "canEdit")]
    public bool? CanEdit { get; set; }

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
    /// Gets or Sets EventDate
    /// </summary>
    [DataMember(Name="eventDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "eventDate")]
    public DateTime? EventDate { get; set; }

    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name="status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

    /// <summary>
    /// Gets or Sets Thumb
    /// </summary>
    [DataMember(Name="thumb", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "thumb")]
    public EntryThumbContract Thumb { get; set; }

    /// <summary>
    /// Gets or Sets Version
    /// </summary>
    [DataMember(Name="version", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "version")]
    public int? Version { get; set; }

    /// <summary>
    /// Gets or Sets FeaturedCategory
    /// </summary>
    [DataMember(Name="featuredCategory", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "featuredCategory")]
    public string FeaturedCategory { get; set; }

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
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SongListForEditContract {\n");
      sb.Append("  SongLinks: ").Append(SongLinks).Append("\n");
      sb.Append("  UpdateNotes: ").Append(UpdateNotes).Append("\n");
      sb.Append("  Author: ").Append(Author).Append("\n");
      sb.Append("  CanEdit: ").Append(CanEdit).Append("\n");
      sb.Append("  Deleted: ").Append(Deleted).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  EventDate: ").Append(EventDate).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  Thumb: ").Append(Thumb).Append("\n");
      sb.Append("  Version: ").Append(Version).Append("\n");
      sb.Append("  FeaturedCategory: ").Append(FeaturedCategory).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
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
