using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using icsimplelib;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Threading;

namespace AlphaQuadrant
{
    public partial class AlphaQuadrant : Microsoft.Xna.Framework.Game
    {
        #region Update
        protected override void Update(GameTime gameTime)
        {
            screens[currentScreen].Update(gameTime);
            CoordsUpdate(gameTime);

            if (currentScreen == "GalaxyMap")
            {
                ((Map)screens["GalaxyMap"].Objects["Map"]).Update(gameTime);
                GalaxyMapControls(screens["GalaxyMap"], ((Map)screens["GalaxyMap"].Objects["Map"]));
            }
            else if (currentScreen == "SolarSystem")
            {
                ((SolarSystem)screens["SolarSystem"].Objects["SolarSystem"]).Update(gameTime);
                if (((TextBox)((Popup)screens["SolarSystem"].Objects["MoreInfoPopup"]).Objects["NameTextBox"]).IsActive == false)
                {
                    SolarSystemControls(screens["SolarSystem"], ((SolarSystem)screens["SolarSystem"].Objects["SolarSystem"]));
                }
                if (((Popup)screens["SolarSystem"].Objects["MoreInfoPopup"]).IsVisible == true)
                {
                    ((TextBox)((Popup)screens["SolarSystem"].Objects["MoreInfoPopup"]).Objects["NameTextBox"]).Update(gameTime); 
                }
                //SolarSystemCollisions((SolarSystem)screens["SolarSystem"].Objects["SolarSystem"]);
            }
            else if (currentScreen == "BlackHoleSystem")
            {
                ((BlackHoleSystem)screens["BlackHoleSystem"].Objects["BlackHoleSystem"]).Update(gameTime);
                BlackHoleSystemControls(screens["BlackHoleSystem"], ((BlackHoleSystem)screens["BlackHoleSystem"].Objects["BlackHoleSystem"]));
            }
            else if (currentScreen == "Menu")
            {
                //((TextBox)screens["Menu"].Objects["TestTextBox"]).Update(gameTime);
                //((TextBox)screens["Menu"].Objects["asdasdasd"]).Update(gameTime);
            }
            else if (currentScreen == "Choose")
            {
                ((TextBox)screens["Choose"].Objects["RaceName"]).Update(gameTime);
            }

            if (screens[currentScreen].Objects.ContainsKey("ResourcesPanel"))
            {
                //((GameString)((Popup)screens[currentScreen].Objects["ResourcesPanel"]).Objects["MoneyStr"]).Str
                ((GameString)((Popup)screens[currentScreen].Objects["ResourcesPanel"]).Objects["MoneyStr"]).Str = players[currentPlayer].Race.Money.ToString();
                ((GameString)((Popup)screens[currentScreen].Objects["ResourcesPanel"]).Objects["EnergyStr"]).Str = players[currentPlayer].Race.Energy.ToString();
                ((GameString)((Popup)screens[currentScreen].Objects["ResourcesPanel"]).Objects["MaterialStr"]).Str = players[currentPlayer].Race.Material.ToString();
            }
            //resources
            if (currentPlayer != null && players.ContainsKey(currentPlayer))
            {
                ResourceTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (ResourceTimer >= ResourceInterval)
                {
                    players[currentPlayer].Race.Money += players[currentPlayer].CountOfPlanets;
                    players[currentPlayer].Race.Energy += players[currentPlayer].CountOfPlanets;
                    players[currentPlayer].Race.Material += players[currentPlayer].CountOfPlanets;
                    ResourceTimer -= ResourceInterval;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// Аптдейт координат в системах в то время, когда игрок на них не смотрит.
        /// Поидее надо бы изменить логику для мультиплеера. Но это еще не щас.
        /// </summary>
        /// <param name="gameTime"></param>
        private void CoordsUpdate(GameTime gameTime)
        {
            //TODO: Изменить для логики мультиплеера, да и вообще сделать умнее.
            /*
             * Эта еболайка должна либо просто стать умнее и апдейтить только нужные системы.
             * Либо же для мультиплеера она целиком и полностью должна работать только на сервере.
             * А клиенты лишь получать нужную им инфу при обращении.
             */
            try
            {
                if (currentScreen == "GalaxyMap")
                {
                    foreach (IDraw obj in screens["GalaxyMap"].Objects["Map"].ToMap().Objects)
                    {
                        if (obj is ICoordUpdateble)
                        {
                            obj.ToICoordUpdateble().CoordsUpdate(gameTime);
                        }
                    }
                }
                else if (currentScreen == "SolarSystem")
                {
                    SolarSystem tempSS = screens[currentScreen].Objects["SolarSystem"].ToSolarSystem();
                    foreach (IDraw obj in screens["GalaxyMap"].Objects["Map"].ToMap().Objects)
                    {
                        if (obj is ICoordUpdateble && !Object.ReferenceEquals(tempSS, obj))
                        {
                            obj.ToICoordUpdateble().CoordsUpdate(gameTime);
                        }
                    }
                }
                else if (currentScreen == "BlackHoleSystem")
                {
                    BlackHoleSystem tempBHS = screens[currentScreen].Objects["BlackHoleSystem"].ToBlackHoleSystem();
                    foreach (IDraw obj in screens["GalaxyMap"].Objects["Map"].ToMap().Objects)
                    { 
                        if (obj is ICoordUpdateble && !Object.ReferenceEquals(tempBHS, obj))
                        {
                            obj.ToICoordUpdateble().CoordsUpdate(gameTime);
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }

        //Maybe there will be gravity simulation code.
        //And maybe there being collision controller code.
        private void SolarSystemCollisions(SolarSystem ss)
        {
            List<IMoveble> except = new List<IMoveble>();
            List<IDraw> forDelete = new List<IDraw>();
            foreach (IMoveble obj in ss.Collision)
            {
                foreach (IMoveble obj2 in ss.Collision)
                {
                    bool isExcept = false;
                    foreach (IMoveble ex in except)
                    {
                        if (System.Object.ReferenceEquals(ex, obj2))
                        {
                            isExcept = true;
                        }
                    }
                    if (isExcept)
                    {
                        continue;
                    }
                    if (!System.Object.ReferenceEquals(obj, obj2))
                    {
                        float width;
                        float height;
                        if ((obj is Asteroid && obj2 is Star) || (obj is Star && obj2 is Asteroid)
                            || (obj is Asteroid && obj2 is Planet) || (obj is Planet && obj2 is Asteroid))
                        {
                            width = obj2.X < obj.X ? obj2.Width * 0.6f : obj.Width * 0.6f;
                            height = obj2.Y < obj.Y ? obj2.Height * 0.6f : obj.Height * 0.6f;
                        }
                        else if (obj is Asteroid && obj2 is Asteroid)
                        {
                            width = obj2.X < obj.X ? obj2.Width: obj.Width;
                            height = obj2.Y < obj.Y ? obj2.Height: obj.Height;
                        }
                        else
                        {
                            width = obj2.X < obj.X ? obj2.Width * 0.6f : obj.Width * 0.6f;
                            height = obj2.Y < obj.Y ? obj2.Height * 0.6f : obj.Height * 0.6f;
                        }
                        if (Math.Abs(obj.X - obj2.X) <= width && Math.Abs(obj.Y - obj2.Y) <= height)
                        {
                            //Debug.Assert(false, "Collision!");
                            if (obj is Asteroid && obj2 is Asteroid)
                            {
                                //Debug.Assert(false, "Asteroid Collision!");
                                ((Asteroid)obj).Velocity = -((Asteroid)obj).Velocity;
                                ((Asteroid)obj2).Velocity = -((Asteroid)obj).Velocity;
                            }

                            if ((obj is Asteroid && obj2 is Star) || (obj is Star && obj2 is Asteroid)
                            || (obj is Asteroid && obj2 is Planet) || (obj is Planet && obj2 is Asteroid))
                            {
                                if (obj is Asteroid)
                                {
                                    forDelete.Add((IDraw)obj);
                                }
                                else
                                {
                                    forDelete.Add((IDraw)obj2);
                                }
                                //Debug.Assert(false, "Asteroid with Object collision!");
                            }
                        }
                    }
                }
                except.Add(obj);
            }
            //Remove destroyed objects
            if (forDelete.Count != 0)
            {
                foreach (IDraw del in forDelete)
                {
                    ss.Objects.Remove(del);
                }
            }
        }
        #endregion



        #region ScreenChangers
        /// <summary>
        /// Открытие экрана звездной системы.
        /// </summary>
        /// <param name="star">Звезда, к которой привязана звзедная система.</param>
        private void SolarSystemOpen(StarOnMap star)
        { 
            /*
             * Проверки на известность системы, есть ли владелец и все такое.
             * Так же загрузка координат кораблей относительно звезды.
             * может еще чего с планетами сделать подобное.
             * Потому что до первого тика они имеют ебнутые координаты и лишь потом выстраиваются вокруг звезды.
             * Ну и видимо еще точно так же загрузка астероидов из относительных координат.
             * Загрузка ивентов звездной системы.
             */

            //Проверка на известность, владельца.
            SolarSystem(star.SS);
            bool isUnknown = true;
            foreach (IDraw obj in star.SS.Objects)
            {
                if (obj is Planet)
                {
                    if (obj.ToPlanet().Owner != "Unknown")
                    {
                        isUnknown = false;
                        break;
                    }
                }
            }
            if (isUnknown)
            {
                foreach (IDraw obj in star.SS.Objects)
                {
                    if (obj is Planet)
                    {
                        obj.ToPlanet().Owner = "Nobody";
                    }
                }
                star.Owner = "Nobody";
                star.IsVisited = true;
                star.Circle = StarOnMapCircles["Nobody"];
            }

            
            foreach (IDraw obj in star.SS.Objects)
            {
                //Загрузка относительных координат кораблей.
                if (obj is Ship)
                {
                    if (obj.ToShip().PositionFromCenter != Vector2.Zero)
                    {
                        obj.ToShip().Position = /*HERE*//*Shift(CenterPoint)*/CenterPoint - obj.ToShip().PositionFromCenter;
                        obj.ToShip().NextPosition = CenterPoint - obj.ToShip().NextPosition;
                    }
                }
                //Загрузка относительных координат астероидов.
                else if (obj is Asteroid)
                {
                    if (obj.ToAsteroid().PositionFromCenter != Vector2.Zero)
                    {
                        obj.ToAsteroid().Position = /*HERE*//*Shift(CenterPoint)*/CenterPoint - obj.ToAsteroid().PositionFromCenter;
                    }
                }
                //Загрузка относительных координат планет.
                else if (obj is Planet)
                {
                    if (obj.ToPlanet().PositionFromCenter != Vector2.Zero)
                    {
                        obj.ToPlanet().Position = /*HERE*//*Shift(CenterPoint)*/CenterPoint - obj.ToPlanet().PositionFromCenter;
                    }
                }
                else if (obj is StationBuilder)
                {
                    if (obj.ToStationBuilder().PositionFromCenter != Vector2.Zero)
                    {
                        obj.ToStationBuilder().Position = CenterPoint - obj.ToStationBuilder().PositionFromCenter;
                    }
                }
                else if (obj is StationOnBuilding)
                {
                    if (obj.ToStationOnBuilding().PositionFromCenter != Vector2.Zero)
                    {
                        obj.ToStationOnBuilding().Position = CenterPoint - obj.ToStationOnBuilding().PositionFromCenter;
                        obj.ToStationOnBuilding().Progress.Position = new Vector2(obj.ToStationOnBuilding().Position.X, obj.ToStationOnBuilding().Position.Y - obj.ToStationOnBuilding().Progress.Height - 5);
                    }
                }
                else if (obj is Station)
                {
                    if (obj.ToStation().PositionFromCenter != Vector2.Zero)
                    {
                        obj.ToStation().Position = CenterPoint - obj.ToStation().PositionFromCenter;
                    }
                }
            }

            //Загрузка ивентов и установка текущего экрана.
            LoadEvents("SolarSystem");
            currentScreen = "SolarSystem";
        }

        /// <summary>
        /// Закрытие экрана звездной системы.
        /// </summary>
        /// <param name="solarSystem">Экран, на котором происходит действо. Нужен, чтобы получить доступ к бэкграунду.</param>
        private void SolarSystemClose(Screen solarSystem)
        { 
            /*
             * Установка координат звезды в центральное положение.
             * Установка относительных координат кораблей относительно звезды.
             * Установка относительных координат астероидов относительно звезды.
             * Выгрузка ивентов звезднйо системы.
             */

            PlanetInfoCloseClick(null, Mouse.GetState());
            //Выгрузка ивентов и установка кооридинат бэкграунда и звезды по умолчанию.
            UnloadEvents("SolarSystem");
            SolarSystem ss = solarSystem.Objects["SolarSystem"].ToSolarSystem();
            solarSystem.Objects["MoreInfoPopup"].ToPopup().IsVisible = false;
            solarSystem.Objects["Back"].ToBackGround().Position = Vector2.Zero;
            //0 object always star
            Vector2 tempStarPosition = ss.Objects[0].ToStar().Position;
            foreach (IDraw obj in ss.Objects)
            {
                if (obj is Star)
                {
                    obj.ToStar().Position = /*HERE*//*Shift(CenterPoint)*/CenterPoint;
                }
                //Установка относительных координат кораблей.
                else if (obj is Ship)
                {
                    obj.ToShip().PositionFromCenter = /*Shift(CenterPoint)*/tempStarPosition - obj.ToShip().Position;
                    obj.ToShip().NextPosition = tempStarPosition - obj.ToShip().NextPosition;
                }
                //Установка относительных координат астероидов.
                else if (obj is Asteroid)
                {
                    obj.ToAsteroid().PositionFromCenter = tempStarPosition - obj.ToAsteroid().Position;
                }
                else if (obj is StationBuilder)
                {
                    obj.ToStationBuilder().PositionFromCenter = tempStarPosition - obj.ToStationBuilder().Position;
                }
                else if (obj is StationOnBuilding)
                {
                    obj.ToStationOnBuilding().PositionFromCenter = tempStarPosition - obj.ToStationOnBuilding().Position;
                }
                else if (obj is Station)
                {
                    obj.ToStation().PositionFromCenter = tempStarPosition - obj.ToStation().Position;
                }
                //Установка относительных координат планет
                /*else if (obj is Planet)
                {
                    obj.ToPlanet().PositionFromCenter = tempStarPosition - obj.ToPlanet().Position;
                }*/
            }
            //((Popup)solarSystem.Objects["MoreInfoPopup"]).IsVisible = false;
        }

        private void GalaxyMapOpen(Map galaxyMap)
        {
            LoadEvents("GalaxyMap");
            currentScreen = "GalaxyMap";

            /*foreach (IDraw obj in galaxyMap.Objects)
            {
                if (obj is StarOnMap)
                {
                    if (obj.ToStarOnMap().Owner == currentPlayer)
                    {
                        obj.ToStarOnMap().IsOwnByThisPlayer = true;
                    }
                    else
                    {
                        obj.ToStarOnMap().IsOwnByThisPlayer = false;
                    }
                }
            }*/
        }

        #endregion

        #region Else
        /*private void UpdateDebugInfo(Screen screen)
        {
            if (screen.Objects.ContainsKey("DebugString"))
            { 
                screen.Objects["DebugString"].ToGameString().Str =
                    "X = " + screen.Objects["Back"].ToBackGround().X + ", Y = " + screen.Objects["Back"].ToBackGround().Y
                    + ", Width = " + graphics.PreferredBackBufferWidth + "Height = " + graphics.PreferredBackBufferHeight;
            }
        }*/
        #endregion
    }
}
