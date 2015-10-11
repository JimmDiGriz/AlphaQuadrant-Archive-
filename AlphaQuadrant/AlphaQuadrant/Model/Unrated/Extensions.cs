using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using icsimplelib;

namespace AlphaQuadrant
{
    public static class Extensions
    {
        public static SolarSystem ToSolarSystem(this IDraw obj)
        {
            return (SolarSystem)obj;
        }

        public static Popup ToPopup(this IDraw obj)
        {
            return (Popup)obj;
        }

        public static BackGround ToBackGround(this IDraw obj)
        {
            return (BackGround)obj;
        }

        public static GameString ToGameString(this IDraw obj)
        {
            return (GameString)obj;
        }

        public static TextBox ToTextBox(this IDraw obj)
        {
            return (TextBox)obj;
        }

        public static Star ToStar(this IDraw obj)
        {
            return (Star)obj;
        }

        public static Planet ToPlanet(this IDraw obj)
        {
            return (Planet)obj;
        }

        public static Ship ToShip(this IDraw obj)
        {
            return (Ship)obj;
        }

        public static Asteroid ToAsteroid(this IDraw obj)
        {
            return (Asteroid)obj;
        }

        public static Asteroid ToAsteroid(this IMoveble obj)
        {
            return (Asteroid)obj;
        }

        public static StarOnMap ToStarOnMap(this IDraw obj)
        {
            return (StarOnMap)obj;
        }

        public static Map ToMap(this IDraw obj)
        {
            return (Map)obj;
        }

        public static IDamagable ToIDamagable(this IDraw obj)
        {
            return (IDamagable)obj;
        }

        public static BlackHoleOnMap ToBlackHoleOnMap(this IDraw obj)
        {
            return (BlackHoleOnMap)obj;
        }

        public static IDraw ToIDraw(this IDamagable obj)
        {
            return (IDraw)obj;
        }

        public static BlackHoleSystem ToBlackHoleSystem(this IDraw obj)
        {
            return (BlackHoleSystem)obj;
        }

        public static IDamagable ToIDamagable(this IMoveble obj)
        {
            return (IDamagable)obj;
        }

        public static ICoordUpdateble ToICoordUpdateble(this IDraw obj)
        {
            return (ICoordUpdateble)obj;
        }

        public static Planet ToPlanet(this IMoveble obj)
        {
            return (Planet)obj;
        }

        public static ShipOnMap ToShipOnMap(this IDraw obj)
        {
            return (ShipOnMap)obj;
        }

        public static BlackHole ToBlackHole(this IDraw obj)
        {
            return (BlackHole)obj;
        }

        public static IUpdateble ToIUpdateble(this IDraw obj)
        {
            return (IUpdateble)obj;
        }

        public static StarOnMap ToStarOnMap(this IMoveble obj)
        {
            return (StarOnMap)obj;
        }

        public static Station ToStation(this IDraw obj)
        {
            return (Station)obj;
        }

        public static IMoveble ToIMoveble(this IDraw obj)
        {
            return (IMoveble)obj;
        }

        public static IDraw ToIDraw(this object obj)
        {
            return (IDraw)obj;
        }

        public static ProductQuery ToProductQuery(this IDraw obj)
        {
            return (ProductQuery)obj;
        }

        public static IDraw ToIDraw(this ShipOnMap obj)
        {
            return (IDraw)obj;
        }

        public static Button ToButton(this IDraw obj)
        {
            return (Button)obj;
        }

        public static StationBuilder ToStationBuilder(this IDraw obj)
        {
            return (StationBuilder)obj;
        }

        public static StationOnBuilding ToStationOnBuilding(this IDraw obj)
        {
            return (StationOnBuilding)obj;
        }

        public static StationOnBuilding ToStationOnbuilding(this IMoveble obj)
        {
            return (StationOnBuilding)obj;
        }

        public static Station ToStation(this IMoveble obj)
        {
            return (Station)obj;
        }
    }
}
