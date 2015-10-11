using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaQuadrant
{
    [Serializable]
    public class Race
    {
        #region Fields
        #endregion

        #region Properties
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Defence { get; set; }
        public int Speed { get; set; }
        public int Science { get; set; }
        public int Product { get; set; }
        public int Money { get; set; }
        public int Energy { get; set; }
        public int Material { get; set; }
        #endregion

        #region Constructors
        public Race(string name, int damage, int defence, int speed, int science, int product)
        {
            Name = name;
            Damage = damage;
            Defence = defence;
            Speed = speed;
            Science = science;
            Product = product;
            Money = Energy = Material = 1000;
        }
        public Race(string name) : this(name, 5, 5, 5, 5, 5) { }
        public Race() : this("Unknown life form") { }
        #endregion

        #region Else
        #endregion
    }
}
