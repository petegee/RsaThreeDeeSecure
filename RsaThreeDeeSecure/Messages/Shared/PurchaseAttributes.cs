using Newtonsoft.Json;

namespace RsaThreeDeeSecure.Messages.Shared
{
    public class PurchaseAttributes
    {
        [JsonProperty("amount")] 
        public string Amount { get; set; }

        [JsonProperty("merchantName")] 
        public string MerchantName { get; set; }

        [JsonProperty("currency")] 
        public string Currency { get; set; }

        [JsonProperty("merchantCountryCode")] 
        public string MerchantCountryCode { get; set; }

        [JsonProperty("date")] 
        public string Date { get; set; }
    }
}