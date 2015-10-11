using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaQuadrant
{
    public class Scales
    {
        public float Value { get; set; }

        public static float Double { get { return dOuble.Value; } }
        public static float OneWithHalf { get { return oneWithHalf.Value; } }
        public static float None { get { return none.Value; } }
        public static float NineTenth { get { return nineTenth.Value; } }
        public static float EightTenth { get { return eightTenth.Value; } }
        public static float SevenTenth { get { return sevenTenth.Value; } }
        public static float SixTenth { get { return sixTenth.Value; } }
        public static float Half { get { return half.Value; } }
        public static float FourTenth { get { return fourTenth.Value; } }
        public static float ThreeWithHalfTenth { get { return threeWithHalfTenth.Value; } }
        public static float ThreeTenth { get { return threeTenth.Value; } }
        public static float Quarter { get { return quarter.Value; } }
        public static float TwoTenth { get { return twoTenth.Value; } }
        public static float OneTent { get { return oneTenth.Value; } }

        private static readonly Scales dOuble = new Scales(2f);
        private static readonly Scales oneWithHalf = new Scales(1.5f);
        private static readonly Scales none = new Scales(1f);
        private static readonly Scales nineTenth = new Scales(0.9f);
        private static readonly Scales eightTenth = new Scales(0.8f);
        private static readonly Scales sevenTenth = new Scales(0.7f);
        private static readonly Scales sixTenth = new Scales(0.6f);
        private static readonly Scales half = new Scales(0.5f);
        private static readonly Scales fourTenth = new Scales(0.4f);
        private static readonly Scales threeWithHalfTenth = new Scales(0.35f);
        private static readonly Scales threeTenth = new Scales(0.3f);
        private static readonly Scales quarter = new Scales(0.25f);
        private static readonly Scales twoTenth = new Scales(0.2f);
        private static readonly Scales oneTenth = new Scales(0.1f);

        private Scales(float value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
