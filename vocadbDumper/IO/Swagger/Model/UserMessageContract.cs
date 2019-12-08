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
  public class UserMessageContract {
    /// <summary>
    /// Gets or Sets Body
    /// </summary>
    [DataMember(Name="body", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "body")]
    public string Body { get; set; }

    /// <summary>
    /// Gets or Sets CreatedFormatted
    /// </summary>
    [DataMember(Name="createdFormatted", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createdFormatted")]
    public string CreatedFormatted { get; set; }

    /// <summary>
    /// Gets or Sets HighPriority
    /// </summary>
    [DataMember(Name="highPriority", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "highPriority")]
    public bool? HighPriority { get; set; }

    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets Inbox
    /// </summary>
    [DataMember(Name="inbox", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "inbox")]
    public string Inbox { get; set; }

    /// <summary>
    /// Gets or Sets Read
    /// </summary>
    [DataMember(Name="read", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "read")]
    public bool? Read { get; set; }

    /// <summary>
    /// Gets or Sets Receiver
    /// </summary>
    [DataMember(Name="receiver", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "receiver")]
    public UserForApiContract Receiver { get; set; }

    /// <summary>
    /// Gets or Sets Sender
    /// </summary>
    [DataMember(Name="sender", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sender")]
    public UserForApiContract Sender { get; set; }

    /// <summary>
    /// Gets or Sets Subject
    /// </summary>
    [DataMember(Name="subject", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "subject")]
    public string Subject { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class UserMessageContract {\n");
      sb.Append("  Body: ").Append(Body).Append("\n");
      sb.Append("  CreatedFormatted: ").Append(CreatedFormatted).Append("\n");
      sb.Append("  HighPriority: ").Append(HighPriority).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  Inbox: ").Append(Inbox).Append("\n");
      sb.Append("  Read: ").Append(Read).Append("\n");
      sb.Append("  Receiver: ").Append(Receiver).Append("\n");
      sb.Append("  Sender: ").Append(Sender).Append("\n");
      sb.Append("  Subject: ").Append(Subject).Append("\n");
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
