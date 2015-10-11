using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AlphaQuadrant
{
    [Serializable]
    class UnknownException : Exception
    {
        public UnknownException() : this("Error code 000: Unknown exception thrown!") { }

        public UnknownException(string message) : base(message) { }

        public UnknownException(string message, Exception inner) : base(message, inner) { }

        protected UnknownException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
