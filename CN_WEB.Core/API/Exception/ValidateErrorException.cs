using System;

namespace CN_WEB.Core.API
{
    public class ValidateErrorException : Exception
    {
        public ValidateErrorException(string message) : base(message) { }
    }
}
