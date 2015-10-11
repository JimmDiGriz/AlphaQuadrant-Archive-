using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaQuadrant
{
    [Serializable]
    public class Player
    {
        #region Fields
        //private string name;
        //private Race race;
        //private bool homePlanet = false;
        //private int planetCounter;
        #endregion

        #region Properties
        public string Name { get; set; }
        public Race Race { get; set; }
        public bool HasHome { get; set; }
        public int CountOfPlanets { get; set; }
        /*public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        public Race PRace
        {
            get { return race; }
            set
            {
                if (value is Race)
                {
                    race = value;
                }
            }
        }

        public bool HomePlanet
        {
            get { return homePlanet; }
            set
            {
                homePlanet = value;
            }
        }

        public int PlanetCounter
        {
            get { return planetCounter; }
            set
            {
                if (value >= 0)
                {
                    planetCounter = value;
                }
            }
        }*/
        #endregion

        #region Constructors
        public Player(string name, Race race)
        {
            Name = name;
            Race = race;
            CountOfPlanets = 0;
            HasHome = false;
        }
        #endregion

        #region Else
        #endregion
    }
}
