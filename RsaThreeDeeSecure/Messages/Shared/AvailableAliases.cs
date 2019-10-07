using Newtonsoft.Json;

namespace RsaThreeDeeSecure.Messages.Shared
{
    public class AvailableAliases
    {
        public enum AliasTypes
        {
            SMS, EMAIL, SOFT_TOKEN, HARD_TOKEN, IVR, OOB 
        }
        
        [JsonProperty("alias")] 
        public string Alias { get; set; }
        
        [JsonProperty("displayAlias")] 
        public string DisplayAlias { get; set; }
        
        [JsonProperty("aliasType")] 
        public AliasTypes AliasType { get; set; }

        [JsonProperty("displayAliasType")] 
        public string DisplayAliasType { get; set; }
        
        [JsonProperty("aliasId")] 
        public string AliasId { get; set; }
    }
}