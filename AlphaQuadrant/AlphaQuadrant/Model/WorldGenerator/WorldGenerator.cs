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
        private void WorldGenerator()
        {
            //Generating a system stars.
            Texture2D circle = Content.Load<Texture2D>("LittleStars/CurrentPlayerCircle");
            r = new Random();
            r2 = new Random();
            int objectsCount = r.Next(150, 300);
            int holesCount = r.Next(5, 15);
            int holes = 0;
            double pixelsTemp = 4000 / objectsCount;
            //Calculating pixels between two stars.
            int pixels = 0;
            map = new Map();
            Texture2D texture;
            //int temp = 1;
            for (int i = 0; i < objectsCount; i++)
            {
                //BlackHole or Star
                bool isBlackHole = false;
                if (holesCount > holes)
                {
                    isBlackHole = new Random().Next(0, 2) == 1 ? true : false;
                }
                if (isBlackHole)
                {
                    texture = Content.Load<Texture2D>("BlackHoles/OnMap/" +
                        blackHolesOnMapTextures[r2.Next(0, blackHolesOnMapTextures.Length)]);
                }
                else
                {
                    texture = Content.Load<Texture2D>("LittleStars/SystemStars/" +
                        systemStarsTextures[r2.Next(0, systemStarsTextures.Length)]);
                }
               //Checking for range between stars.
               //First version of algorithm, where stars adding only if have place to it.
               bool isNear = true;
               float starX = 0;
               float starY = 0;
               if (map.Objects.Count == 0)
               {
                   starX = r.Next(0, 4000);
                   starY = r.Next(0, 4000);
               }
               else
               {
                   while (isNear)
                   {
                       starX = r.Next(0, 4000);
                       starY = r.Next(0, 4000);
                       isNear = false;
                       foreach (IDraw star in map.Objects)
                       {
                           if (Math.Abs(((IMoveble)star).X - starX) < pixels ||
                               Math.Abs(((IMoveble)star).Y - starY) < pixels)
                           {
                               isNear = true;
                               break;
                           }
                       }
                   }
               }
               //Adding stars
               if (isBlackHole)
               {
                   BlackHoleOnMap tempHole = new BlackHoleOnMap(texture, new Vector2(starX, starY), new Vector2(Scales.FourTenth));
                   GenerateBlackHoleSystem(tempHole);
                   map.Objects.Add(tempHole);
                   if (pixels == 0)
                   {
                       pixels = (int)tempHole.Width / 2;
                   }
                   holes++;
               }
               else
               {
                   StarOnMap tempStar = new StarOnMap(texture, new Vector2(starX, starY), new Vector2(Scales.FourTenth));
                   tempStar.Circle = circle;
                   tempStar.Owner = "Unknown";
                   GenerateSolarSystem(tempStar);
                   map.Objects.Add(tempStar);
                   if (pixels == 0)
                   {
                       pixels = (int)tempStar.Width / 2;
                   }
               }
               ((ProgressBar)screens["Gen"].Objects["ProgressBar"]).PWidth
                   = (int)((float)i / objectsCount * ((ProgressBar)screens["Gen"].Objects["ProgressBar"]).TextureWidth);
               ((GameString)screens["Gen"].Objects["String"]).Str = "Generating stars... " + i.ToString() + "/" + objectsCount;
            }
            
            GalaxyMap(map);
            currentScreen = "GalaxyMap";
            LoadEvents("GalaxyMap");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        //--------------------------------------------------------------
        //GENERATE BLACKHOLE SYSTEM
        //--------------------------------------------------------------
        private void GenerateBlackHoleSystem(BlackHoleOnMap hole)
        { 
            //Construct black hole name
            string tempName = ConstructName();
            hole.Name = tempName;
            //End name construction
            //Generationg BlackHole System
            //Generating black hole
            BlackHole bh = new BlackHole(Content.Load<Texture2D>("BlackHoles/"+
                blackHolesTextures[new Random().Next(0, blackHolesTextures.Length)]), CenterPoint, 
                new Vector2(Scales.TwoTenth));
            bh.Name = tempName;
            //End generating black hole
            BlackHoleSystem bhs = new BlackHoleSystem(bh);
            bhs.BackGround = CreateBackground("SystemBacks/" +
                systemBackTextures[new Random().Next(0, systemBackTextures.Length)], Scales.None, 255f);
            //End generating
            GeneratingAsteroids(bhs);
            hole.System = bhs;
        }

        #region Памятка
        /*
         * ТЕМПЕРАТУРЫ ПЛАНЕТ. (МИНИМАЛЬНАЯ ТЕМПЕРАТУРА ВСЕГДА ЗАВИСИТ ОТ МАКСИМАЛЬНОЙ. ЕСЛИ МАКСИМАЛЬНАЯ ВЫШЕ, ТО И МИНИМАЛЬАНЯ ВЫШЕ И ТД).
         * В случае если их будет 10.
         * 1. Самая близкая. 
         * Горячая пиздец, ни в коем случае не пригодна для жизни.
         * Ни в коем случае не пригодна для жизни, пока не будет проведен терраформ.
         * Генерируемый тераформ - 0.
         * Максимальный терраформ - 1. 
         * Температуры - 600-400.
         * Жизнь та еще веселая будет тут.
         * 2. Тоже близкая и горячая.
         * Так же никаким местом не пригодна для жизни.
         * Генерируемый тераформ - 0.
         * Максимальный тераформ - 1-2 (зависит от температуры).
         * Температуры - 400-100. Если максимальная температура меньше 200, то максимальный тераформ 2.
         * 3. Теплая и пригодная для жизни.
         * Те самые родные и пиздатые для жизни планеты.
         * Генерируемый тераформ - 4.
         * Максимальный тераформ - 4.
         * Температуры - 50 - -40
         * 4. Менее теплая, но, возможно тоже пригодная для жизни.
         * Сорт оф марс.
         * Не так далеко от пригодной для жизни планеты, потому выглядит соблазнительно.
         * Генерируемый тераформ - 3.
         * Максимальный тераформ - 4.
         * Температуры - 20 - -70 
         * 5. Холодная, возможно пригодная для жизни в не лучших условиях.
         * Генерируемый тераформ - 3.
         * Максимальный тераформ - 3-4.
         * Температуры - -20 - -100 (если максималка больше -30, то тераформ 4)
         * 6. Ледяная. 
         * Пригодна для жизни под куполами.
         * Генерируемый тераформ - 2
         * Максимальный тераформ - 3.
         * Температуры - -70 - -150
         * 7. Льдина ебаная.
         * Непригодна для жизни.
         * Генерируемый тераформ - 0.
         * Максимальный тераформ - 1-2 (зависит от температуры).
         * Температуры - -150 - -273 (если температура выше -200, то тераформ 2 возможен).
         * 8. Газовый гигант.
         * Само собой для жизни непригоден.
         * В сущности космический мусор, однако с введением гарвитации начнет приносить пользу.
         * Генерируемый тераформ - 0.
         * Максимальный тераформ - 0.
         * Температуры - хз стоит ли.
         * 9. Еще один газовый гигант.
         * Точно такой же космический мусор, как и прошлый.
         * Генерируемый тераформ - 0.
         * Максимальный тераформ - 0.
         * Температуры - хз стоит ли.
         * 10. Кусок камня.
         * Сорт оф плутон, большой астероид крутящийся вокруг звезды.
         * Однако пригодность для жизни на нем большая, чем на газовых гигантах.
         * Генерируемый тераформ - 0.
         * Максимальный тераформ - 1.
         * Температуры - -273 и точка по той причине, что на нем нет атмосферы вообще.
         * Расстояния между планетами - около 100 пикселей.
         * Растояние первой планеты от звезды - около 250-300 пикселей + ширина звезды / 2.
         */
        #endregion

        //--------------------------------------------------------------
        //GENERATE SOLAR SYSTEM
        //--------------------------------------------------------------
        private void GenerateSolarSystem(StarOnMap star)
        {
            //Construct star name
            string tempName = ConstructName();
            star.Name = tempName;
            //End name construction
            //Generating Solar System
            Random starType = new Random();

            string starTextureStr = starsTextures[starType.Next(0, starsTextures.Length)];
            Texture2D starTexture = Content.Load<Texture2D>
                ("Stars/"+starTextureStr);

            //Here take a starcolor'
            string color = GetColor(starTextureStr);

            Star tempStar = new Star(starTexture, CenterPoint, new Vector2(Scales.None), color);
            //Copy star name to solar star name
            tempStar.Name = tempName;
            SolarSystem ss = new SolarSystem(tempStar);
            ss.BackGround = CreateBackground("SystemBacks/"+
                systemBackTextures[new Random().Next(0, systemBackTextures.Length)], Scales.None, 255f);

            //NEW PLANET GENERATING
            GeneratingPlanets(ss);
            //END NEW PLANET GENERATING
            GeneratingAsteroids(ss);
            //End generating solar system
            star.SS = ss;
        }

        private void GeneratingAsteroids(IDraw system)
        {
            Random posRandom = new Random();
            Random velRandom = new Random();
            int asteroidsCount = posRandom.Next(10, 30);
            for (int i = 0; i < asteroidsCount; i++)
            {
                float size = posRandom.Next(100, 500);
                float mass = size * 10;
                float astSize = (float)(500 - size) / 500;
                astSize = 0.15f * astSize;
                astSize = 0.3f - astSize;
                Asteroid ast = new Asteroid(Content.Load<Texture2D>("Asteroids/" +
                    asteroidTextures[new Random().Next(0, asteroidTextures.Length)]),
                    new Vector2(posRandom.Next(100, 1900), posRandom.Next(100, 1900)), new Vector2(astSize), mass);
                ast.Size = size;
                ast.Name = ConstructName();
                ast.Velocity = new Vector2((float)velRandom.Next(-3, 4) / 10, (float)velRandom.Next(-3, 4) / 10);

                if (system is SolarSystem)
                {
                    system.ToSolarSystem().Objects.Add(ast);
                }
                else
                {
                    system.ToBlackHoleSystem().Objects.Add(ast);
                }
            }
        }

        private Asteroid CreateAsteroid()
        {
            Random posRandom = new Random();
            Random velRandom = new Random();
            float size = posRandom.Next(100, 500);
            float mass = size * 10;
            float astSize = (float)(500 - size) / 500;
            astSize = 0.15f * astSize;
            astSize = 0.3f - astSize;
            Asteroid ast = new Asteroid(Content.Load<Texture2D>("Asteroids/" +
                asteroidTextures[new Random().Next(0, asteroidTextures.Length)]),
                new Vector2(posRandom.Next(100, 1900), posRandom.Next(100, 1900)), new Vector2(astSize), mass);
            ast.Size = size;
            ast.Name = ConstructName();
            ast.Velocity = new Vector2((float)velRandom.Next(-3, 4) / 10, (float)velRandom.Next(-3, 4) / 10);

            return ast;
        }

        private void GeneratingPlanets(SolarSystem system)
        {
            #region LocalVariables
            Star star = system.Objects[0].ToStar();
            int width = (int)star.Width / 2;
            Random count = new Random();
            Random temperature = new Random();
            Random textureRnd = new Random();
            Random radiusRnd = new Random();
            Random planetInterval = new Random();
            Random planetDegrees = new Random();
            int planetCount = count.Next(1, 11);
            #endregion

            for (int i = 0; i < planetCount; i++)
            {
                int degrees = planetDegrees.Next(0, 360);
                int interval = planetInterval.Next(50, 120);
                int planetSize = new Random().Next(2000, 5000);
                float planetScale = (float)(5000 - planetSize) / 3000f;
                planetScale = 0.05f * planetScale;
                planetScale = 0.1f - planetScale;
                int radius = radiusRnd.Next(WGInfoHelper[i].MinRadius + width, WGInfoHelper[i].MaxRadius + width);

                Planet tempPlanet = new Planet(
                    Content.Load<Texture2D>(WGInfoHelper[i].Textures[textureRnd.Next(0, WGInfoHelper[i].Textures.Length)]), 
                    new Vector2(planetScale), radius, interval, degrees, star, WGInfoHelper[i].GenTerraform,
                    Content.Load<Texture2D>("Textures/AlphaPlanet"));

                //tempPlanet.Circle = CreateCircle(radius);

                tempPlanet.Name = ConstructName();

                //Planet Properties
                tempPlanet.PlanetSize = planetSize;
                tempPlanet.MaxTemperature = temperature.Next(WGInfoHelper[i].MinTemperature, WGInfoHelper[i].MaxTemperature);
                tempPlanet.MinTemperature = temperature.Next(WGInfoHelper[i].MinTemperature, tempPlanet.MaxTemperature);
                tempPlanet.MaxTerraform = WGInfoHelper[i].MaxTerraform(WGInfoHelper[i], tempPlanet.MaxTemperature);

                tempPlanet.IsAborigens = new Random().Next(0, 4) == 1 ? true : false;
                tempPlanet.Mass = (tempPlanet.PlanetSize * tempPlanet.PlanetSize) / 10;
                tempPlanet.Gravity = tempPlanet.Mass / 1000000;
                tempPlanet.Climat = "Unknown";

                #region Shit Properties
                int temp = new Random().Next(1, 100);
                int temp2;
                tempPlanet.Stability = (float)temp;
                bool isEqual;
                do
                {
                    temp2 = new Random().Next(1, 100);
                    if (temp == temp2)
                    {
                        isEqual = true;
                    }
                    else
                    {
                        isEqual = false;
                    }
                } while (isEqual);
                tempPlanet.Fertility = (float)temp2;
                do
                {
                    temp = new Random().Next(1, 100);
                    if (temp == temp2)
                    {
                        isEqual = true;
                    }
                    else
                    {
                        isEqual = false;
                    }
                } while (isEqual);
                tempPlanet.Radioactivity = (float)temp;
                #endregion
                //End Planet Properties

                system.Objects.Add(tempPlanet);
            }
        }
        //--------------------------------------------------------------
        //SIMPLE NAME CONSTRUCTOR
        //--------------------------------------------------------------
        private string ConstructName()
        {
            Random nameAlpha = new Random();
            Random nameDigit = new Random();
            if (names == null)
            {
                names = new List<string>();
            }
            bool existing = true;
            string tempName = "";
            while (existing)
            {
                tempName = "";
                for (int i = 0; i < 3; i++)
                {
                    tempName += symbols[nameAlpha.Next(0, symbols.Length)];
                }
                for (int i = 0; i < 5; i++)
                {
                    tempName += nameDigit.Next(0, 9);
                }
                existing = false;
                foreach (string name in names)
                {
                    if (name == tempName)
                    {
                        existing = true;
                        break;
                    }
                }
            }
            names.Add(tempName);
            return tempName;
        }

        private string GetColor(string name)
        {
            name = name.Replace("Star", "");
            name = new string(name.ToCharArray().Where(c => !char.IsDigit(c)).ToArray());
            return name;
        }
    }
}
