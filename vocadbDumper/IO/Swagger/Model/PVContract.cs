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
  public class PVContract {
    /// <summary>
    /// Gets or Sets Author
    /// </summary>
    [DataMember(Name="author", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "author")]
    public string Author { get; set; }

    /// <summary>
    /// Gets or Sets CreatedBy
    /// </summary>
    [DataMember(Name="createdBy", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createdBy")]
    public int? CreatedBy { get; set; }

    /// <summary>
    /// Gets or Sets Disabled
    /// </summary>
    [DataMember(Name="disabled", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "disabled")]
    public bool? Disabled { get; set; }

    /// <summary>
    /// Gets or Sets ExtendedMetadata
    /// </summary>
    [DataMember(Name="extendedMetadata", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "extendedMetadata")]
    public PVExtendedMetadata ExtendedMetadata { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Length
    /// </summary>
    [DataMember(Name="length", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "length")]
    public int? Length { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets PublishDate
    /// </summary>
    [DataMember(Name="publishDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "publishDate")]
    public DateTime? PublishDate { get; set; }

    /// <summary>
    /// Gets or Sets PvId
    /// </summary>
    [DataMember(Name="pvId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pvId")]
    public string PvId { get; set; }

    /// <summary>
    /// Gets or Sets Service
    /// </summary>
    [DataMember(Name="service", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "service")]
    public string Service { get; set; }

    /// <summary>
    /// Gets or Sets PvType
    /// </summary>
    [DataMember(Name="pvType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "pvType")]
    public string PvType { get; set; }

    /// <summary>
    /// Gets or Sets ThumbUrl
    /// </summary>
    [DataMember(Name="thumbUrl", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "thumbUrl")]
    public string ThumbUrl { get; set; }

    /// <summary>
    /// Gets or Sets Url
    /// </summary>
    [DataMember(Name="url", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "url")]
    public string Url { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PVContract {\n");
      sb.Append("  Author: ").Append(Author).Append("\n");
      sb.Append("  CreatedBy: ").Append(CreatedBy).Append("\n");
      sb.Append("  Disabled: ").Append(Disabled).Append("\n");
      sb.Append("  ExtendedMetadata: ").Append(ExtendedMetadata).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Length: ").Append(Length).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  PublishDate: ").Append(PublishDate).Append("\n");
      sb.Append("  PvId: ").Append(PvId).Append("\n");
      sb.Append("  Service: ").Append(Service).Append("\n");
      sb.Append("  PvType: ").Append(PvType).Append("\n");
      sb.Append("  ThumbUrl: ").Append(ThumbUrl).Append("\n");
      sb.Append("  Url: ").Append(Url).Append("\n");
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
