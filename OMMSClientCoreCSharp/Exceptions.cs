using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMMSClientCoreCSharp
{
    namespace Exceptions
    {
        public class CannotConnectToServerException : Exception
        {
            public CannotConnectToServerException(string message) : base(message)
            {
            }

            public CannotConnectToServerException(string message, Exception innerException) : base(message, innerException)
            {
            }
        }
    }
}
