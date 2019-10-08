using Newtonsoft.Json;
using RsaThreeDeeSecure.Messages.Shared;

namespace RsaThreeDeeSecure.Messages.Requests
{
    public class FetchAvailableAliasesRequests
    {
        [JsonProperty("service")] 
        public string Service { get; set; }
        
        [JsonProperty("rsaSessionId")] 
        public string RsaSessionId { get; set; }
        
        [JsonProperty("issuerSessionId")] 
        public string IssuerSessionId { get; set; }
        
        [JsonProperty("dsSessionId")] 
        public string DsSessionId { get; set; }
        
        [JsonProperty("pan")] 
        public string Pan { get; set; }
        
        [JsonProperty("panExpirationYear")] 
        public string PanExpirationYear { get; set; }
        
        [JsonProperty("panExpirationMonth")] 
        public string PanExpirationMonth { get; set; }
        
        [JsonProperty("purchaseAttributes")] 
        public PurchaseAttributes PurchaseAttributes { get; set; }
        
        [JsonProperty("timeStamp")] 
        public string TimeStamp { get; set; }
        
        [JsonProperty("version")] 
        public string Version { get; set; }
        
        [JsonProperty("transactionType")] 
        public string TransactionType { get; set; }
        
    }
}