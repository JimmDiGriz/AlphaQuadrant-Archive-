using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AlphaQuadrant
{
    [Serializable]
    public class SystemNotFoundException : Exception
    {
        public SystemNotFoundException() : this("Error code 001: System was not found!") { }

        public SystemNotFoundException(string message) : base(message) { }

        public SystemNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected SystemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
