using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobsAdmin.Framework.Exceptions
{
    public class InvalidParametersException: Exception
    {
        public InvalidParametersException()
            : this("Invalid parameters")
        { }

        public InvalidParametersException(string message) 
            : base(message)
        {}

        public InvalidParametersException(string message, Exception innerException) 
            : base(message, innerException)
        {}
    }
}
