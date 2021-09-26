using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace X.CQRS.Commands
{
    public class CommandResult : ObjectResult
    {
        public Exception Exception { get; set; }

        public CommandResult(HttpStatusCode code, object content = null) : base(content) 
        {
            StatusCode = (int)code;
        }

        public CommandResult(HttpStatusCode code, Exception exception) : this(code, exception.Message)
        {
            Exception = exception;
        }
    }
}