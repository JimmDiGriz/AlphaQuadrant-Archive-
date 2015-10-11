using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AlphaQuadrant
{
    [Serializable]
    public class BlackHoleOnMapNotFoundException : Exception
    {
        public BlackHoleOnMapNotFoundException() : this("Error code 003: Black hole on map was not found!") { }

        public BlackHoleOnMapNotFoundException(string message) : base(message) { }

        public BlackHoleOnMapNotFoundException(string message, Exception inner) : base(message, inner) { }

        protected BlackHoleOnMapNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
