using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaQuadrant
{
    public class WGPlanetInfo
    {
        #region Fields
        private int maxTerraform;
        #endregion

        #region Properties
        public string[] Textures { get; private set; }
        public int MaxTemperature { get; private set; }
        public int MinTemperature { get; private set; }
        public int GenTerraform { get; private set; }
        private int AdditionalTemperature {get; set;}
        public int MinRadius { get; private set; }
        public int MaxRadius { get; private set; }
        //Тут я снаркоманил и вместо свойства сделал лямбду, но это просто у меня настроение такое было лямбдовое.
        public Func<WGPlanetInfo, int, int> MaxTerraform = 
            (info, temp) => { return (info.AdditionalTemperature != 0 && temp >= info.AdditionalTemperature) ? info.maxTerraform + 1 : info.maxTerraform; };
        #endregion

        #region Construct
        public WGPlanetInfo(string[] textures, int maxT, int minT, int genTerraform, int maxTerraform, int minRadius, int maxRadius, int additional = 0)
        {
            Textures = textures;
            MaxTemperature = maxT;
            MinTemperature = minT;
            GenTerraform = genTerraform;
            this.maxTerraform = maxTerraform;
            AdditionalTemperature = additional;
            MinRadius = minRadius;
            MaxRadius = maxRadius;
        }
        #endregion
    }
}