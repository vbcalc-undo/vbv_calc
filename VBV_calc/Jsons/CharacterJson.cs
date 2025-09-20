using Newtonsoft.Json;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;

namespace JsonFileIO.Jsons
{
    [JsonObject("CharacterJson")]
    public sealed class CharacterJson
    {
        [JsonProperty("名称")]
        public string 名称 { get; set; }
        [JsonProperty("加護")]
        public string 加護 { get; set; }
        [JsonProperty("職業")]
        public string 職業 { get; set; }
        [JsonProperty("基本パラメータ_HP")]
        public string 基本パラメータ_HP { get; set; }
        [JsonProperty("基本パラメータ_攻")]
        public string 基本パラメータ_攻 { get; set; }
        [JsonProperty("基本パラメータ_防")]
        public string 基本パラメータ_防 { get; set; }
        [JsonProperty("基本パラメータ_速")]
        public string 基本パラメータ_速 { get; set; }
        [JsonProperty("基本パラメータ_知")]
        public string 基本パラメータ_知 { get; set; }
        [JsonProperty("種族")]
        public string 種族 { get; set; }
        [JsonProperty("特攻")]
        public string 特攻 { get; set; }
        [JsonProperty("装備")]
        public List<string> 装備 { get; set; }
        [JsonProperty("ランク")]
        public string ランク { get; set; }
        [JsonProperty("コスト")]
        public string コスト { get; set; }
        [JsonProperty("パッシブスキル")]
        public List<string> パッシブスキル { get; set; }
        [JsonProperty("リーダースキル")]
        public List<string> リーダースキル { get; set; }
        [JsonProperty("アシストスキル")]
        public List<string> アシストスキル { get; set; }
        [JsonProperty("内政スキル")]
        public string 内政スキル { get; set; }
        [JsonProperty("スタンス")]
        public string スタンス { get; set; }
        [JsonProperty("備考")]
        public string 加入条件 { get; set; }
        public bool キャラクター { get; set; } =false;
    }
}