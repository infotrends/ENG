using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoTrendsAPI.Error
{
    public class OAuthError
    {
        public const string API_KEY_EXPIRED
            = "[OA100]: API Key has been expired";

        public const string API_OVER_USAGE
            = "[OA200]: This API key has been reached the limit per day";

        public const string INVALID_API_KEY
            = "[OA300]: Invalid API Key";

        public const string INVALID_API_SIGNATURE
            = "[OA400]: Invalid API Signature";

        public const string INVALID_METHOD
            = "[OA500]: Invalid Method";

        public const string INVALID_METHOD_SIGNATURE
            = "[OA600]: Invalid Method Signature";

        public const string INVALID_NONCE
            = "[OA700]: Invalid Nonce";

        public const string INVALID_ROLE = "800]: Invalid Role";

        public const string INVALID_ROLE_METHOD
            = "[OA900]: Invalid Role. Method is currently not enabled in supplied role.";

        public const string INVALID_SESSION_KEY
            = "[OA1000]: Invalid Session Key";

        public const string INVALID_TIMESTAMP
            = "[OA1100]: Invalid Timestamp";

        public const string INVALID_USERNAME_AND_PASSWORD
            = "[OA1200]: Invalid Username & Password";

        public const string VERIFY_EMAIL
            = "[OA1300]: Please check your email for activating instruction.";
    }
}
