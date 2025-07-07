using System;
using Newtonsoft.Json;

namespace TestTask.Weather
{
    [Serializable]
    public class WeatherApiResponse
    {
        [JsonProperty("properties")]
        public WeatherProperties Properties { get; set; }
    }
}