using Newtonsoft.Json;

namespace TestTask.Dogs
{
    public class DogBreedResponse
    {
        [JsonProperty("data")]
        public BreedData[] Data { get; set; }
    }

    public class BreedData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("attributes")]
        public BreedAttributes Attributes { get; set; }

        [JsonProperty("relationships")]
        public BreedRelationships Relationships { get; set; }
    }

    public class BreedAttributes
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("life")]
        public LifeSpan Life { get; set; }

        [JsonProperty("male_weight")]
        public Weight MaleWeight { get; set; }

        [JsonProperty("female_weight")]
        public Weight FemaleWeight { get; set; }

        [JsonProperty("hypoallergenic")]
        public bool Hypoallergenic { get; set; }
    }

    public class LifeSpan
    {
        [JsonProperty("max")]
        public int Max { get; set; }

        [JsonProperty("min")]
        public int Min { get; set; }
    }

    public class Weight
    {
        [JsonProperty("max")]
        public int Max { get; set; }

        [JsonProperty("min")]
        public int Min { get; set; }
    }

    public class BreedRelationships
    {
        [JsonProperty("group")]
        public GroupData Group { get; set; }
    }

    public class GroupData
    {
        [JsonProperty("data")]
        public GroupInfo Data { get; set; }
    }

    public class GroupInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}