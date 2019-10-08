using System.Collections.Generic;
using Newtonsoft.Json;
using RsaThreeDeeSecure.Constants;

namespace RsaThreeDeeSecure.Messages.Shared
{
    public class ErrorDetails
    {
        [JsonIgnore]
        public static Dictionary<int, (string ErrorName, string ErrorDescription, int HttpCode)> ErrorCodes = 
            new Dictionary<int, (string, string, int)>
            {
                { RsaErrorCodes.IssuerError, ("Issuer error", "An error on the issuer's side.", 500) },
                { RsaErrorCodes.PanNotAvailableAtServiceSide, ("Pan is not available at service side", "The pan is not known at the issuer's service", 400 ) },
                { RsaErrorCodes.PanIsLockedAtServiceSide, ("Pan is locked at service side", "The pan is locked at the issuer", 423 ) },
                { RsaErrorCodes.IssuerSessionIsInvalid, ("IssuerSessionId is not valid", "The issuer SessionId sent from RSA to the issuer is not known at issuer's side", 404 ) },
                { RsaErrorCodes.NoAvailableAliasOnServiceSide, ("No available aliases on service side", "No available aliases is attached to the PAN at the issuer service", 404 ) },
                { RsaErrorCodes.OtpGenerationFailed, ("OTP generation failed", "The issuer could not generate the OTP and it could not been sent", 500 ) },
                { RsaErrorCodes.OtpSendingFailed, ("OTP sending failed", "The issuer could not send the OTP to the cardholder", 500 ) },
                { RsaErrorCodes.OtpNoLongerValid, ("OTP no longer valid", "The OTP is no longer valid because it expired", 400 ) },
                { RsaErrorCodes.EncryptionFailed, ("Encryption failed", "There was a problem with encryption at the issuer.", 401 ) },
                { RsaErrorCodes.SignFailed, ("Sign failed", "There was a problem with signature at the issuer", 401 ) },
                { RsaErrorCodes.DecryptionFailed, ("Decryption failed ", "There was a problem with decryption at the issuer", 401 ) },
                { RsaErrorCodes.VerifySignatureFailed, ("Verify signature failed", "There was a problem with verifying the signature at the issuer.", 401 ) },
                { RsaErrorCodes.InitiateOobFlowFailed, ("Initiate OOB flow failed", "There was a problem with the initiate OOB flow.", 500 ) },
                { RsaErrorCodes.OobServiceFailed, ("OOB service failed", "Issue occurred at OOB service.", 500 ) },
                { RsaErrorCodes.GeneratingTokenFailed, ("Generating Token value failed", "There was a problem with generating the Token at the issuer.", 500 ) },
            };
        
        
        [JsonProperty("rsaErrorId")] 
        public string RsaErrorId { get; set; }
        
        [JsonProperty("issuerErrorDescription")] 
        public string IssuerErrorDescription { get; set; }

        public (string ErrorName, string ErrorDescription, int HttpCode) GetErrorDetails()
        {
            return ErrorCodes[int.Parse(RsaErrorId)];
        }
    }
}