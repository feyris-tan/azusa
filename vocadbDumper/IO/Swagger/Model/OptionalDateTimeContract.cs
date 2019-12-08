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
  public class OptionalDateTimeContract {
    /// <summary>
    /// Gets or Sets Day
    /// </summary>
    [DataMember(Name="day", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "day")]
    public int? Day { get; set; }

    /// <summary>
    /// Gets or Sets Formatted
    /// </summary>
    [DataMember(Name="formatted", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "formatted")]
    public string Formatted { get; set; }

    /// <summary>
    /// Gets or Sets IsEmpty
    /// </summary>
    [DataMember(Name="isEmpty", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "isEmpty")]
    public bool? IsEmpty { get; set; }

    /// <summary>
    /// Gets or Sets Month
    /// </summary>
    [DataMember(Name="month", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "month")]
    public int? Month { get; set; }

    /// <summary>
    /// Gets or Sets Year
    /// </summary>
    [DataMember(Name="year", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "year")]
    public int? Year { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class OptionalDateTimeContract {\n");
      sb.Append("  Day: ").Append(Day).Append("\n");
      sb.Append("  Formatted: ").Append(Formatted).Append("\n");
      sb.Append("  IsEmpty: ").Append(IsEmpty).Append("\n");
      sb.Append("  Month: ").Append(Month).Append("\n");
      sb.Append("  Year: ").Append(Year).Append("\n");
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
