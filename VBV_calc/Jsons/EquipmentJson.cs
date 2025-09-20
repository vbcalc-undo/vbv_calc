using Newtonsoft.Json;
using System.Collections.Generic;

namespace JsonFileIO.Jsons
{
    [JsonObject("CharacterJson")]
    public sealed class EquipmentJson
    {
            [JsonProperty("レア")]
            public string レア { get; set; }
            [JsonProperty("購入")]
            public string 購入 { get; set; }
            [JsonProperty("名称")]
            public string 名称 { get; set; }
            [JsonProperty("売却")]
            public string 売却 { get; set; }
            [JsonProperty("性能変化_攻")]
            public string 性能変化_攻 { get; set; }
            [JsonProperty("性能変化_防")]
            public string 性能変化_防 { get; set; }
            [JsonProperty("性能変化_速")]
            public string 性能変化_速 { get; set; }
            [JsonProperty("性能変化_知")]
            public string 性能変化_知 { get; set; }
            [JsonProperty("能力付加")]
            public List<string> 能力付加 { get; set; }
            [JsonProperty("説明")]
            public string 説明 { get; set; }
    }
}