using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using icsimplelib;
using System.Threading;
using Microsoft.Xna.Framework;

namespace AlphaQuadrant
{
    public partial class AlphaQuadrant : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// Текстуры звезд на глобальной карте.
        /// </summary>
        private string[] starsTextures = {"BlueStar", "BlueStar2", "BlueStar3", "BlueStar4", "BlueStar5", "BlueStar6", "BlueStar7", "BlueStar8", "BlueStar9", "BlueStar10", "BlueStar11", "BlueStar12", "BlueStar13", "BlueStar14", "BlueStar15", "BlueStar16", "BlueStar17",
                                         "GreenStar", "GreenStar2", "GreenStar3", "GreenStar4", "GreenStar5", "GreenStar6", "GreenStar7", "GreenStar8", "GreenStar9", "GreenStar10", "GreenStar11", "GreenStar12", "GreenStar13", "GreenStar14",
                                         "LightBlueStar", "LightBlueStar2", "LightBlueStar3", "LightBlueStar4", "LightBlueStar5", "LightBlueStar6", "LightBlueStar7", "LightBlueStar8", "LightBlueStar9", "LightBlueStar10", "LightBlueStar11", "LightBlueStar12", "LightBlueStar13", "LightBlueStar14",
                                         "LilacStar", "LilacStar2", "LilacStar3", "LilacStar4", "LilacStar5", "LilacStar6", "LilacStar7", "LilacStar8", "LilacStar9", "LilacStar10", "LilacStar11", "LilacStar12", "LilacStar13",
                                         "PurpleStar", "PurpleStar2", "PurpleStar3", "PurpleStar4", "PurpleStar5", "PurpleStar6", "PurpleStar7", "PurpleStar8", "PurpleStar9", "PurpleStar10", "PurpleStar11", "PurpleStar12", "PurpleStar13", "PurpleStar14", "PurpleStar15", "PurpleStar16", "PurpleStar17",
                                         "RedStar", "RedStar2", "RedStar3", "RedStar4", "RedStar5", "RedStar6", "RedStar7", "RedStar8", "RedStar9", "RedStar10", "RedStar11", "RedStar12", "RedStar13", "RedStar14", "RedStar15", "RedStar16",
                                         "OrangeStar", "OrangeStar2", "OrangeStar3", "OrangeStar4", "OrangeStar5", "OrangeStar6", "OrangeStar7", "OrangeStar8", "OrangeStar9", "OrangeStar10", "OrangeStar11", "OrangeStar12", "OrangeStar13", "OrangeStar14",
                                         "YellowStar", "YellowStar2", "YellowStar3", "YellowStar4", "YellowStar5", "YellowStar6", "YellowStar7", "YellowStar8", "YellowStar9", "YellowStar10", "YellowStar11", "YellowStar12"};
        /// <summary>
        /// Текстуры звезд в звездных системах.
        /// </summary>
        private string[] systemStarsTextures = {"BlueTemporaryStar", "GreenTemporaryStar", "LightBlueTemporaryStar",
                                               "PinkTemporaryStar", "PurpleTemporaryStar", "RedTemporaryStar",
                                               "YellowTemporaryStar"};
        /// <summary>
        /// Текстуры мелких звезд. Которые, похоже, уже не нужны, но пока пусть будет.
        /// </summary>
        private string[] backgroundStarsTextures = { "Medium/SmallStarElloy", "Medium/SmallStarLightBlue",
                                                   "Medium/SmallStarPink", "Medium/SmallStarWhite",
                                                    "Small/VerySmallStar"};
        private static string hot = "Planets/Hot/";
        private static string medium = "Planets/Middle/";
        private static string cold = "Planets/Cold/";
        private static string outsiders = "Planets/Outsiders/";
        /// <summary>
        /// Текстуры планет, расположенных ближе всего к звезде.
        /// </summary>
        private string[] hotPlanetTextures = {hot+"Cercury", hot+"HotPlanet", hot+"LavaPlanet", hot+"RedHotPlanet",
                                             hot+"SandPlanet", hot+"Venum2", hot+"BloodPlanet"};
        /// <summary>
        /// Текстуры планет, находящихся на среднем расстоянии от звезды.
        /// </summary>
        private string[] mediumPlanetTextures = {medium+"Foam", medium+"ICPlanet", medium+"LightBluePearl", medium+"LivePlanet",
                                                medium+"Marc", medium+"Marc2", medium+"RedPlanet", medium+"SeaPlanet", medium+"SeaPlanet2",
                                                medium+"Seldon", medium+"Tangerine", medium+"Venum", medium+"experimentplanet",
                                                medium+"mineralplanet", medium+"BloodQueenPlanet"};
        /// <summary>
        /// Текстуры планет, находящихся достаточно далеко от звезды.
        /// </summary>
        private string[] coldPlanetTextures = {cold+"GasGiant", cold+"Lilac", cold+"Lucy", cold+"RingPlanet", cold+"RingPlanet2",
                                              cold+"FreezePlanet", cold+"EnemyPlanet"};
        /// <summary>
        /// Текстуры планет, находящихся в полной жопе.
        /// </summary>
        private string[] outsidersPlanetTextures = {outsiders+"ColdPlanet", outsiders+"IcePlanet", outsiders+"Marc3", outsiders+"NoNamePlanet",
                                                   outsiders+"Rain", outsiders+"SmallPlanet", outsiders+"SoilPlanet", outsiders+"FlowPlanet"};
        /// <summary>
        /// Текстуры фонов звездных систем.
        /// </summary>
        private string[] systemBackTextures = {"back1", "back2", "Back4", "Back5"};

        /// <summary>
        /// Текстуры черных дыр в системах.
        /// </summary>
        private string[] blackHolesTextures = {"BlackHole"};

        /// <summary>
        /// Текстуры черных дыр на карте.
        /// </summary>
        private string[] blackHolesOnMapTextures = {"BlackHoleOnMap1"};

        /// <summary>
        /// Текстуры метеоритов.
        /// </summary>
        private string[] asteroidTextures = {"Meteorite"};

        /// <summary>
        /// Click sounds
        /// </summary>
        private string[] clicks = { "Sound/Click" };

        /// <summary>
        /// Planet State Clouds
        /// </summary>
        private string[] clouds = {"BlueClouds", "GreenClouds", "LightBlueClouds", "LilacClouds",
                                  "OrangeClouds", "PurpleClouds", "RedClouds", "YellowClouds"};

        /// <summary>
        /// Planet State Fieelds
        /// </summary>
        private string[] fields = {"BlueField", "GreenField", "LightBlueField", "LilacField",
                                  "OrangeField", "PurpleField", "RedField", "YellowField"};

        /// <summary>
        /// Planet State Mountains
        /// </summary>
        private string[] mountains = {"BlueDesertMountain", "GreenDesertMountain", "LightBlueDesertMountain",
                                     "LilacDesertMountain", "OrangeDesertMountain", "PurpleDesertMountain",
                                     "RedDesertMountain", "YellowDesertMountain", "BlueSnowMountains",
                                     "GreenSnowMountains", "LightBlueSnowMountains", "LilacSnowMountains",
                                     "OrangeSnowMountains", "PurpleSnowMountains", "RedSnowMountains",
                                     "YellowSnowMountains", "BlueStoneMountains", "GreenStoneMountains",
                                     "LightBlueStoneMountains", "LilacStoneMountains", "OrangeStoneMountains",
                                     "PurpleStoneMountains", "RedStoneMountains", "YellowStoneMountains"};

        /// <summary>
        /// Planet State Rocky Desert
        /// </summary>
        private string[] rockyDesert = {"BlueRockyDesert", "GreenRockyDesert", "LightBlueRockyDesert",
                                       "LilacRockyDesert", "OrangeRockyDesert", "PurpleRockyDesert",
                                       "RedRockyDesert", "YellowRockyDesert"};

        /// <summary>
        /// Planet State Sands
        /// </summary>
        private string[] sands = {"BlueSands", "GreenSands", "LightBlueSands", "LilacSands",
                                 "OrangeSands", "PurpleSands", "RedSands", "YellowSands"};

        /// <summary>
        /// Planet State Skys
        /// </summary>
        private string[] skys = {"BlueSky", "GreenSky", "LightBlueSky", "LilacSky", "OrangeSky", "PurpleSky",
                                "RedSky", "YellowSky"};

        /// <summary>
        /// Planet State Snow
        /// </summary>
        private string[] snow = {"BlueSnow", "GreenSnow", "LightBlueSnow", "LilacSnow", "OrangeSnow",
                                "PurpleSnow", "RedSnow", "YellowSnow"};
    }
}