using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DeployerLib
{
    [Serializable]
    public class UiException : Exception
    {
        public UiException(string message, Exception innerException = null)
            : base(message, innerException)
        {

        }

        protected UiException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {

        }
    }
}
