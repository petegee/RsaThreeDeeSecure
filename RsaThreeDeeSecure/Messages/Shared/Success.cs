using Newtonsoft.Json;
using RsaThreeDeeSecure.Jwe;

namespace RsaThreeDeeSecure.Messages.Shared
{
    public class Success
    {
        [JsonProperty("success")] 
        public object SuccessObject { get; set; }

        public static Success WrapResponse<T>(JweMessage response)
        {
            return new Success {SuccessObject = response.GetDecryptedJsonObjectAs<T>()};
        }
        
        public static Success WrapResponse(object response)
        {
            return new Success {SuccessObject = response};
        }
    }
}