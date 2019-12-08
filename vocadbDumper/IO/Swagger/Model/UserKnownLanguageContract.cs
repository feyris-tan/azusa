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
  public class UserKnownLanguageContract {
    /// <summary>
    /// Gets or Sets CultureCode
    /// </summary>
    [DataMember(Name="cultureCode", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cultureCode")]
    public string CultureCode { get; set; }

    /// <summary>
    /// Gets or Sets Proficiency
    /// </summary>
    [DataMember(Name="proficiency", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "proficiency")]
    public string Proficiency { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UserKnownLanguageContract {\n");
      sb.Append("  CultureCode: ").Append(CultureCode).Append("\n");
      sb.Append("  Proficiency: ").Append(Proficiency).Append("\n");
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
