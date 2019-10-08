namespace RsaThreeDeeSecure.Constants
{
    public static class RsaErrorCodes
    {
        public const int IssuerError = 99999;
        public const int PanNotAvailableAtServiceSide = 00001;
        public const int PanIsLockedAtServiceSide = 00002;
        public const int IssuerSessionIsInvalid = 00003;
        public const int NoAvailableAliasOnServiceSide = 20001;
        public const int OtpGenerationFailed = 30001;
        public const int OtpSendingFailed = 30002;
        public const int OtpNoLongerValid = 40001;
        public const int EncryptionFailed = 51001;
        public const int SignFailed = 51002;
        public const int DecryptionFailed = 51003;
        public const int VerifySignatureFailed = 51004;
        public const int InitiateOobFlowFailed = 60001;
        public const int OobServiceFailed = 60002;
        public const int GeneratingTokenFailed = 70001;
    }
}