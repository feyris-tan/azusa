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
  public class AdvancedSearchFilter {
    /// <summary>
    /// Gets or Sets FilterType
    /// </summary>
    [DataMember(Name="filterType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "filterType")]
    public string FilterType { get; set; }

    /// <summary>
    /// Gets or Sets Negate
    /// </summary>
    [DataMember(Name="negate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "negate")]
    public bool? Negate { get; set; }

    /// <summary>
    /// Gets or Sets Param
    /// </summary>
    [DataMember(Name="param", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "param")]
    public string Param { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AdvancedSearchFilter {\n");
      sb.Append("  FilterType: ").Append(FilterType).Append("\n");
      sb.Append("  Negate: ").Append(Negate).Append("\n");
      sb.Append("  Param: ").Append(Param).Append("\n");
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
