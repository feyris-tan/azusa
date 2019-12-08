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
  public class ArtistForSongContract {
    /// <summary>
    /// Gets or Sets Artist
    /// </summary>
    [DataMember(Name="artist", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artist")]
    public ArtistContract Artist { get; set; }

    /// <summary>
    /// Gets or Sets Categories
    /// </summary>
    [DataMember(Name="categories", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categories")]
    public string Categories { get; set; }

    /// <summary>
    /// Gets or Sets EffectiveRoles
    /// </summary>
    [DataMember(Name="effectiveRoles", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "effectiveRoles")]
    public string EffectiveRoles { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets IsCustomName
    /// </summary>
    [DataMember(Name="isCustomName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isCustomName")]
    public bool? IsCustomName { get; set; }

    /// <summary>
    /// Gets or Sets IsSupport
    /// </summary>
    [DataMember(Name="isSupport", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isSupport")]
    public bool? IsSupport { get; set; }

    /// <summary>
    /// Gets or Sets Name
    /// </summary>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets Roles
    /// </summary>
    [DataMember(Name="roles", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "roles")]
    public string Roles { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ArtistForSongContract {\n");
      sb.Append("  Artist: ").Append(Artist).Append("\n");
      sb.Append("  Categories: ").Append(Categories).Append("\n");
      sb.Append("  EffectiveRoles: ").Append(EffectiveRoles).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  IsCustomName: ").Append(IsCustomName).Append("\n");
      sb.Append("  IsSupport: ").Append(IsSupport).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Roles: ").Append(Roles).Append("\n");
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
