using Newtonsoft.Json;

namespace RsaThreeDeeSecure.Messages.Shared
{
    public class CustomFacts
    {
        [JsonProperty("CustomStringFact1")] 
        public string CustomStringFact1 { get; set; }
            
        [JsonProperty("CustomStringFact2")] 
        public string CustomStringFact2 { get; set; }
            
        [JsonProperty("CustomNumberFact1")] 
        public string CustomNumberFact1 { get; set; }
            
        [JsonProperty("CustomNumberFact2")] 
        public string CustomNumberFact2 { get; set; }
            
        [JsonProperty("CustomBooleanFact")] 
        public string CustomBooleanFact { get; set; }
    }
}