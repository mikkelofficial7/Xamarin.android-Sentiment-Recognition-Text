using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ImageRecognition.Model
{
    public class Sentiment
    {
        [JsonProperty("positive")]
        public string positive { get; set; }
        [JsonProperty("negative")]
        public string negative { get; set; }
    }
}