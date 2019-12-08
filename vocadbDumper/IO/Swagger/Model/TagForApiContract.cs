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
  public class TagForApiContract {
    /// <summary>
    /// Gets or Sets AdditionalNames
    /// </summary>
    [DataMember(Name="additionalNames", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "additionalNames")]
    public string AdditionalNames { get; set; }

    /// <summary>
    /// Gets or Sets AliasedTo
    /// </summary>
    [DataMember(Name="aliasedTo", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aliasedTo")]
    public TagBaseContract AliasedTo { get; set; }

    /// <summary>
    /// Gets or Sets CategoryName
    /// </summary>
    [DataMember(Name="categoryName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "categoryName")]
    public string CategoryName { get; set; }

    /// <summary>
    /// Gets or Sets CreateDate
    /// </summary>
    [DataMember(Name="createDate", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "createDate")]
    public DateTime? CreateDate { get; set; }

    /// <summary>
    /// Gets or Sets DefaultNameLanguage
    /// </summary>
    [DataMember(Name="defaultNameLanguage", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "defaultNameLanguage")]
    public string DefaultNameLanguage { get; set; }

    /// <summary>
    /// Gets or Sets Description
    /// </summary>
    [DataMember(Name="description", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "description")]
    public string Description { get; set; }

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
    public List<LocalizedStringWithIdContract> Names { get; set; }

    /// <summary>
    /// Gets or Sets Parent
    /// </summary>
    [DataMember(Name="parent", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "parent")]
    public TagBaseContract Parent { get; set; }

    /// <summary>
    /// Gets or Sets RelatedTags
    /// </summary>
    [DataMember(Name="relatedTags", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "relatedTags")]
    public List<TagBaseContract> RelatedTags { get; set; }

    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name="status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

    /// <summary>
    /// Gets or Sets Targets
    /// </summary>
    [DataMember(Name="targets", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "targets")]
    public int? Targets { get; set; }

    /// <summary>
    /// Gets or Sets TranslatedDescription
    /// </summary>
    [DataMember(Name="translatedDescription", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "translatedDescription")]
    public EnglishTranslatedStringContract TranslatedDescription { get; set; }

    /// <summary>
    /// Gets or Sets UrlSlug
    /// </summary>
    [DataMember(Name="urlSlug", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "urlSlug")]
    public string UrlSlug { get; set; }

    /// <summary>
    /// Gets or Sets UsageCount
    /// </summary>
    [DataMember(Name="usageCount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "usageCount")]
    public int? UsageCount { get; set; }

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
      sb.Append("class TagForApiContract {\n");
      sb.Append("  AdditionalNames: ").Append(AdditionalNames).Append("\n");
      sb.Append("  AliasedTo: ").Append(AliasedTo).Append("\n");
      sb.Append("  CategoryName: ").Append(CategoryName).Append("\n");
      sb.Append("  CreateDate: ").Append(CreateDate).Append("\n");
      sb.Append("  DefaultNameLanguage: ").Append(DefaultNameLanguage).Append("\n");
      sb.Append("  Description: ").Append(Description).Append("\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  MainPicture: ").Append(MainPicture).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  Names: ").Append(Names).Append("\n");
      sb.Append("  Parent: ").Append(Parent).Append("\n");
      sb.Append("  RelatedTags: ").Append(RelatedTags).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  Targets: ").Append(Targets).Append("\n");
      sb.Append("  TranslatedDescription: ").Append(TranslatedDescription).Append("\n");
      sb.Append("  UrlSlug: ").Append(UrlSlug).Append("\n");
      sb.Append("  UsageCount: ").Append(UsageCount).Append("\n");
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
