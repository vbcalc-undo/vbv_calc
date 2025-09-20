using Newtonsoft.Json;
using System.Collections.Generic;

namespace JsonFileIO.Jsons
{
    [JsonObject("CharacterJson")]
    public sealed class EquipmentJson
    {
            [JsonProperty("���A")]
            public string ���A { get; set; }
            [JsonProperty("�w��")]
            public string �w�� { get; set; }
            [JsonProperty("����")]
            public string ���� { get; set; }
            [JsonProperty("���p")]
            public string ���p { get; set; }
            [JsonProperty("���\�ω�_�U")]
            public string ���\�ω�_�U { get; set; }
            [JsonProperty("���\�ω�_�h")]
            public string ���\�ω�_�h { get; set; }
            [JsonProperty("���\�ω�_��")]
            public string ���\�ω�_�� { get; set; }
            [JsonProperty("���\�ω�_�m")]
            public string ���\�ω�_�m { get; set; }
            [JsonProperty("�\�͕t��")]
            public List<string> �\�͕t�� { get; set; }
            [JsonProperty("����")]
            public string ���� { get; set; }
    }
}