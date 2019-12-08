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
  public class UserForApiContract {
    /// <summary>
    /// Gets or Sets Active
    /// </summary>
    [DataMember(Name="active", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "active")]
    public bool? Active { get; set; }

    /// <summary>
    /// Gets or Sets GroupId
    /// </summary>
    [DataMember(Name="groupId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupId")]
    public string GroupId { get; set; }

    /// <summary>
    /// Gets or Sets KnownLanguages
    /// </summary>
    [DataMember(Name="knownLanguages", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "knownLanguages")]
    public List<UserKnownLanguageContract> KnownLanguages { get; set; }

    /// <summary>
    /// Gets or Sets MainPicture
    /// </summary>
    [DataMember(Name="mainPicture", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mainPicture")]
    public EntryThumbForApiContract MainPicture { get; set; }

    /// <summary>
    /// Gets or Sets MemberSince
    /// </summary>
    [DataMember(Name="memberSince", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "memberSince")]
    public DateTime? MemberSince { get; set; }

    /// <summary>
    /// Gets or Sets OldUsernames
    /// </summary>
    [DataMember(Name="oldUsernames", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "oldUsernames")]
    public List<OldUsernameContract> OldUsernames { get; set; }

    /// <summary>
    /// Gets or Sets VerifiedArtist
    /// </summary>
    [DataMember(Name="verifiedArtist", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "verifiedArtist")]
    public bool? VerifiedArtist { get; set; }

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
      sb.Append("class UserForApiContract {\n");
      sb.Append("  Active: ").Append(Active).Append("\n");
      sb.Append("  GroupId: ").Append(GroupId).Append("\n");
      sb.Append("  KnownLanguages: ").Append(KnownLanguages).Append("\n");
      sb.Append("  MainPicture: ").Append(MainPicture).Append("\n");
      sb.Append("  MemberSince: ").Append(MemberSince).Append("\n");
      sb.Append("  OldUsernames: ").Append(OldUsernames).Append("\n");
      sb.Append("  VerifiedArtist: ").Append(VerifiedArtist).Append("\n");
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
