using Newtonsoft.Json;

namespace ImageRecognition.Model
{
    public class Mood
    {
        [JsonProperty("happy")]
        public string happy { get; set; }
        [JsonProperty("upset")]
        public string upset { get; set; }
    }
}