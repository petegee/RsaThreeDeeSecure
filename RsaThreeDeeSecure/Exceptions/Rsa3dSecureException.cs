using System;
using RsaThreeDeeSecure.Messages.Shared;

namespace RsaThreeDeeSecure.Exceptions
{
    public class Rsa3dSecureException : Exception
    {
        public int RsaErrorCode { get; }
        
        public string ErrorName { get; } 
        
        public string ErrorDescription { get; }
        
        public int HttpCode  { get; }
        
        public ErrorDetails ErrorDetails { get;  }

        public Rsa3dSecureException(int rsaErrorCode, string exceptionMessage)
            : base(exceptionMessage)
        {
            RsaErrorCode = rsaErrorCode;

            var (errorName, errorDescription, httpCode) = ErrorDetails.ErrorCodes[rsaErrorCode];

            ErrorName = errorName;
            ErrorDescription = errorDescription;
            HttpCode = httpCode;

            ErrorDetails = new ErrorDetails
            {
                RsaErrorId = rsaErrorCode.ToString(),
                IssuerErrorDescription = ErrorDescription
            };
        }

        public Rsa3dSecureException(int rsaErrorCode, string exceptionMessage, Exception innerException)
            : base(exceptionMessage, innerException)
        {
            RsaErrorCode = rsaErrorCode;

            var (errorName, errorDescription, httpCode) = ErrorDetails.ErrorCodes[rsaErrorCode];

            ErrorName = errorName;
            ErrorDescription = errorDescription;
            HttpCode = httpCode;

            ErrorDetails = new ErrorDetails
            {
                RsaErrorId = rsaErrorCode.ToString(),
                IssuerErrorDescription = ErrorDescription
            };
        }
    }
}