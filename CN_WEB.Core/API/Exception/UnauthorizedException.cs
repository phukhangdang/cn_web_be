using System;

namespace CN_WEB.Core.API
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base("Unauthorized access.")
        {
        }
    }
}
