using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestTask.Weather
{
    [Serializable]
    public class WeatherProperties
    {
        [JsonProperty("periods")]
        public WeatherPeriod[] Periods { get; set; }
    }
}