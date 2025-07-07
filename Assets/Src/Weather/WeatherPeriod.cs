using System;
using Newtonsoft.Json;

namespace TestTask.Weather
{
    [Serializable]
    public class WeatherPeriod
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    
        [JsonProperty("temperature")]
        public int Temperature { get; set; }
    
        [JsonProperty("temperatureUnit")]
        public string TemperatureUnit { get; set; }
    
        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }
    
        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }
    
        [JsonProperty("isDaytime")]
        public bool IsDaytime { get; set; }
    }
}