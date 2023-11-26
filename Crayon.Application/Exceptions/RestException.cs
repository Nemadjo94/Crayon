using System.Net;

namespace Crayon.Application.Exceptions
{
    public sealed class RestException : Exception
    {
        public HttpStatusCode Code { get; }

        public override string? Message { get; }

        public RestException(HttpStatusCode code, string? message = null)
        {
            Code = code;
            Message = message;
        }
    }
}
