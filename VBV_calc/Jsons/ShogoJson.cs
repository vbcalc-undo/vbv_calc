using Newtonsoft.Json;
using System.Collections.Generic;

namespace JsonFileIO.Jsons
{
    public sealed class ShogoJson
    {
        [JsonProperty("���_���I��")]
        public string ���_���I�� { get; set; }
        [JsonProperty("���A")]
        public string ���A { get; set; }
        [JsonProperty("���")]
        public string ��� { get; set; }
        [JsonProperty("�ڑ�")]
        public Dictionary<string, string> �ڑ� { get; set; }
        [JsonProperty("�X�e�[�^�X�ω�")]
        public Dictionary<string, string> �X�e�[�^�X�ω� { get; set; }
        [JsonProperty("�\�͕t�^")]
        public Dictionary<string, string> �\�͕t�^ { get; set; }

    }
}