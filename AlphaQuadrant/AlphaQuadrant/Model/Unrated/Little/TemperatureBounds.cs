using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaQuadrant
{
    public class TemperatureBounds
    {
        #region Properties
        public int Min { get; set; }
        public int Max { get; set; }
        #endregion

        #region Construct
        public TemperatureBounds(int min, int max)
        {
            Min = min;
            Max = max;
        }
        #endregion
    }
}
