using Newtonsoft.Json;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;

namespace JsonFileIO.Jsons
{
    [JsonObject("CharacterJson")]
    public sealed class CharacterJson
    {
        [JsonProperty("����")]
        public string ���� { get; set; }
        [JsonProperty("����")]
        public string ���� { get; set; }
        [JsonProperty("�E��")]
        public string �E�� { get; set; }
        [JsonProperty("��{�p�����[�^_HP")]
        public string ��{�p�����[�^_HP { get; set; }
        [JsonProperty("��{�p�����[�^_�U")]
        public string ��{�p�����[�^_�U { get; set; }
        [JsonProperty("��{�p�����[�^_�h")]
        public string ��{�p�����[�^_�h { get; set; }
        [JsonProperty("��{�p�����[�^_��")]
        public string ��{�p�����[�^_�� { get; set; }
        [JsonProperty("��{�p�����[�^_�m")]
        public string ��{�p�����[�^_�m { get; set; }
        [JsonProperty("�푰")]
        public string �푰 { get; set; }
        [JsonProperty("���U")]
        public string ���U { get; set; }
        [JsonProperty("����")]
        public List<string> ���� { get; set; }
        [JsonProperty("�����N")]
        public string �����N { get; set; }
        [JsonProperty("�R�X�g")]
        public string �R�X�g { get; set; }
        [JsonProperty("�p�b�V�u�X�L��")]
        public List<string> �p�b�V�u�X�L�� { get; set; }
        [JsonProperty("���[�_�[�X�L��")]
        public List<string> ���[�_�[�X�L�� { get; set; }
        [JsonProperty("�A�V�X�g�X�L��")]
        public List<string> �A�V�X�g�X�L�� { get; set; }
        [JsonProperty("�����X�L��")]
        public string �����X�L�� { get; set; }
        [JsonProperty("�X�^���X")]
        public string �X�^���X { get; set; }
        [JsonProperty("���l")]
        public string �������� { get; set; }
        public bool �L�����N�^�[ { get; set; } =false;
    }
}