using Newtonsoft.Json;
using RsaThreeDeeSecure.Messages.Shared;

namespace RsaThreeDeeSecure.Messages.Responses
{
    public class ErrorResponse
    {
        [JsonProperty("rsaSessionId")] 
        public string RsaSessionId { get; set; }
        
        [JsonProperty("issuerSessionId")] 
        public string IssuerSessionId { get; set; }

        [JsonProperty("timeStamp")] 
        public string TimeStamp { get; set; }
        
        [JsonProperty("version")] 
        public string Version { get; set; }
        
        [JsonProperty("configName")] 
        public string ConfigName { get; set; }
        
        [JsonProperty("errorDetails")] 
        public ErrorDetails ErrorDetails { get; set; }
    }
}