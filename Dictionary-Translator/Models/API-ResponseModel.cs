using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Dictionary_Translator.Models
{



    public class Def
    {
        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string text { get; set; }

        [JsonProperty("pos")]
        [JsonPropertyName("pos")]
        public string pos { get; set; }

        [JsonProperty("ts")]
        [JsonPropertyName("ts")]
        public string ts { get; set; }

        [JsonProperty("tr")]
        [JsonPropertyName("tr")]
        public List<Tr> tr { get; set; }
    }

    public class Ex
    {
        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string text { get; set; }

        [JsonProperty("tr")]
        [JsonPropertyName("tr")]
        public List<Tr> tr { get; set; }
    }

    public class Head
    {
    }

    public class Mean
    {
        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string text { get; set; }
    }

    public class Root
    {
        [JsonProperty("head")]
        [JsonPropertyName("head")]
        public Head head { get; set; }

        [JsonProperty("def")]
        [JsonPropertyName("def")]
        public List<Def> def { get; set; }
    }

    public class Syn
    {
        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string text { get; set; }

        [JsonProperty("pos")]
        [JsonPropertyName("pos")]
        public string pos { get; set; }

        [JsonProperty("fr")]
        [JsonPropertyName("fr")]
        public int? fr { get; set; }

        [JsonProperty("gen")]
        [JsonPropertyName("gen")]
        public string gen { get; set; }
    }

    public class Tr
    {
        [JsonProperty("text")]
        [JsonPropertyName("text")]
        public string text { get; set; }

        [JsonProperty("pos")]
        [JsonPropertyName("pos")]
        public string pos { get; set; }

        [JsonProperty("gen")]
        [JsonPropertyName("gen")]
        public string gen { get; set; }

        [JsonProperty("fr")]
        [JsonPropertyName("fr")]
        public int? fr { get; set; }

        [JsonProperty("syn")]
        [JsonPropertyName("syn")]
        public List<Syn> syn { get; set; }

        [JsonProperty("mean")]
        [JsonPropertyName("mean")]
        public List<Mean> mean { get; set; }

        [JsonProperty("ex")]
        [JsonPropertyName("ex")]
        public List<Ex> ex { get; set; }

        [JsonProperty("asp")]
        [JsonPropertyName("asp")]
        public string asp { get; set; }
    }
}