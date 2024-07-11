using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Journey.Exception.ExceptionsBase
{
    public class ErrorOnValidationException : JourneyException
    {

        private IList<string> _errors;

        public ErrorOnValidationException(IList<string> message) : base(string.Empty)
        {
            _errors = message;
        }
        public override IList<string> GetErrorMessages()
        {
            return _errors;
        }
        public override HttpStatusCode GetStatusCode()
        {
            return HttpStatusCode.BadRequest;
        }

    }
}
