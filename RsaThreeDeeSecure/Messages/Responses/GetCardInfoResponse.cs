using Newtonsoft.Json;
using RsaThreeDeeSecure.Messages.Shared;

namespace RsaThreeDeeSecure.Messages.Responses
{
    public class GetCardInfoResponse
    {
        [JsonProperty("rsaSessionId")] 
        public string RsaSessionId { get; set; }
        
        [JsonProperty("issuerSessionId")] 
        public string IssuerSessionId { get; set; }
        
        [JsonProperty("isEligible")] 
        public string IsEligible { get; set; }
        
        [JsonProperty("timeStamp")] 
        public string TimeStamp { get; set; }
        
        [JsonProperty("version")] 
        public string Version { get; set; }
        
        [JsonProperty("configName")] 
        public string ConfigName { get; set; }
        
        [JsonProperty("customFacts")] 
        public CustomFacts[] CustomFacts { get; set; }
    }
}