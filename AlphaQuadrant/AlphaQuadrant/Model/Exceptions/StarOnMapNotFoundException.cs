using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AlphaQuadrant
{
    [Serializable]
    public class StarOnMapNotFoundException : Exception
    {
        public StarOnMapNotFoundException() : this("Error code 002: Star on map was not found!") { }

        public StarOnMapNotFoundException(string message) : base(message) { }

        public StarOnMapNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected StarOnMapNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
