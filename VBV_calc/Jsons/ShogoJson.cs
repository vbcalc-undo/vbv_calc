using Newtonsoft.Json;
using System.Collections.Generic;

namespace JsonFileIO.Jsons
{
    public sealed class ShogoJson
    {
        [JsonProperty("メダリオン")]
        public string メダリオン { get; set; }
        [JsonProperty("レア")]
        public string レア { get; set; }
        [JsonProperty("二つ名")]
        public string 二つ名 { get; set; }
        [JsonProperty("接続")]
        public Dictionary<string, string> 接続 { get; set; }
        [JsonProperty("ステータス変化")]
        public Dictionary<string, string> ステータス変化 { get; set; }
        [JsonProperty("能力付与")]
        public Dictionary<string, string> 能力付与 { get; set; }

    }
}