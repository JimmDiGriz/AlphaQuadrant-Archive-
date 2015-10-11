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
using System.Threading;
using System.Diagnostics;

namespace AlphaQuadrant
{
    public partial class AlphaQuadrant : Microsoft.Xna.Framework.Game
    {
        private void ShowPlanetInfoPopup(Planet planet)
        {
            InfoPopupSettings settings = new InfoPopupSettings(true, true);
            FillPlanetState(planet);

            tempPlanetName = planet.Name;

            //Buttons
            if (planet.Owner != currentPlayer && planet.Query.Buttons.Count == 0 &&
                ((!players[currentPlayer].HasHome && planet.Terraform == 4) ||
                (players[currentPlayer].HasHome && planet.ShipsOnOrbit.Count > 0 && planet.Terraform != 0)))
            {
                settings.IsColonize = true;
            }

            if (planet.Owner == currentPlayer)
            {
                settings.IsWorkerShipCreate = true;
            }

            if (players[currentPlayer].HasHome && planet.Terraform != 4 
                && planet.Terraform < planet.MaxTerraform && !planet.IsTerraforming &&
                planet.Owner == currentPlayer)
            {
                settings.IsTerraform = true;
            }

            if (planet.Terraform == 4 && planet.Owner == currentPlayer)
            {
                settings.IsStationBuilder = true;
            }
            //End buttons

            AcceptPopupSettings(settings);
        }

        private void HidePlanetInfoPopup()
        {
            InfoPopupSettings settings = new InfoPopupSettings();
            AcceptPopupSettings(settings, false);
        }
        //---------------------------------------------------------------
        //PLANET CLICK
        //---------------------------------------------------------------
        private void PlanetClick(Planet sender, MouseState ms)
        {
            #region Опять какие-то ебучие рассуждения
            /*
             * Колонизировать не первую планету можно только в том случае, если рядом с ней находится корабль.
             * А именно корабль находится на орбите (уже научил, хоть и криво).
             * Корабль в процессе колонизации исчезает.
             * Кстати сам прцоесс колонизации длится определенное время, пока там колонисты освоятся.
             * Не просто же так они вывалили свои жёёёппы и начали ябаться.
             * После колонизации становится доступен тераформ, но лишь настолько, насколько позволяет планета.
             * Строительство рабочих кораблей и прототипов станций становится доступно сразу после колонизации.
             * Однако время для строительства всего этого напрямую зависит от стадии тераформа (насколько тяжелые условия).
             * Так же от стадии тераформа зависят приносимые планетой ресурсы (это тоже надо отображать).
             * Приносимые планетой ресурсы так же зависят от того, производится ли на ней в данный момент что-либо.
             * Если производится, то приносимых ресурсов немного меньше.
             * Грубо говоря это связано с тем, что те же люди, которые приносят ресурсы вдруг становятся заняты на производстве.
             * Однако их число не велико.
             * Тоже самое касается и строительства зданий - часть людей уходит строить.
             * Вообще это касается любого производства на планете.
             * ОБЯЗАТЕЛЬНО ЗАМУТИТЬ КЛАСС НАСТРОЕК, КОТОРЫЙ БУДЕТ СОБИРАТЬСЯ СНАЧАЛА И ЛИШЬ ПОТОМ ОТДАВАТЬСЯ НА ОТРИСОВКУ.
             * А ТО ГОВНОКОДА БУДЕТ СТОЛЬКО, ЧТО ПОТЕРЯТЬСЯ БУДЕТ МОЖНО. ЧАСТЬ ЕГО НУЖНО ОТДЕЛИТЬ.
             * Еще надо сделат ьмелкую панельку управления кораблем (имя, хп, щит ,команды всякие, как во всех ртс), а то на ожну правую кнопку многому не научишь.
             */
            #endregion
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            ShowPlanetInfoPopup(sender);
        }
        //---------------------------------------------------------------
        //NAME TEXT BOX ENTER
        //---------------------------------------------------------------
        private void NameTextBoxEnter(TextBox sender)
        {
            Planet planet = FindPlanet(((SolarSystem)screens["SolarSystem"].Objects["SolarSystem"]));
            if (planet == null) { return; }
            planet.Name = sender.Content;
            RefreshPlanetInfoPopup(planet);
        }
        //---------------------------------------------------------------
        //COLONIZE BTN CLICK
        //---------------------------------------------------------------
        private void ColonizeBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            Planet planet = FindPlanet(((SolarSystem)screens["SolarSystem"].Objects["SolarSystem"]));
            if (planet == null) { return; }
            ProgressBar progress = ((ProgressBar)((Popup)screens[currentScreen].Objects["MoreInfoPopup"]).Objects["Progress"]);

            screens["SolarSystem"].Objects["SolarSystem"].ToSolarSystem().Objects.Remove(planet.ShipsOnOrbit.Find(x => x.Name == "WorkerShip"));
            planet.ShipsOnOrbit.Remove(planet.ShipsOnOrbit.Find(x => x.Name == "WorkerShip"));

            planet.Query.Add(new GameTimer(progress, 0.1f, () =>
                {
                    planet.Owner = currentPlayer;
                    planet.Race = players[currentPlayer].Race.Name;
                    StarOnMap star = FindStarOnMap(((SolarSystem)screens["SolarSystem"].Objects["SolarSystem"]));
                    if (star == null) { return; }
                    star.Owner = currentPlayer;
                    star.Circle = StarOnMapCircles["CurrentPlayer"];
                    players[currentPlayer].HasHome = true;
                    //star.IsVisited = true;
                    players[currentPlayer].CountOfPlanets++;
                    ((Button)sender).IsVisible = false;
                    ((Button)sender).OnClick -= ColonizeBtnClick;
                    progress.PWidth = 0;
                    if (screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().IsVisible)
                    {
                        RefreshPlanetInfoPopup(planet);
                    }
                }, 
                CreateButton("QueryBtns/blacksquare", "QueryBtns/blacksquare", 0, 0, 1f)));

            if (screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().IsVisible)
            {
                RefreshPlanetInfoPopup(planet);
            }
        }
        //---------------------------------------------------------------
        //TERRAFORM BTN CLICK
        //---------------------------------------------------------------
        private void TerraformBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            Planet planet = FindPlanet(((SolarSystem)screens["SolarSystem"].Objects["SolarSystem"]));
            if (planet == null) { return; }
            ProgressBar progress = ((ProgressBar)((Popup)screens[currentScreen].Objects["MoreInfoPopup"]).Objects["Progress"]);

            if (planet.Terraform < planet.MaxTerraform)
            {
                TemperatureBounds bounds = GetNewTemperatureBounds(planet);
                planet.IsTerraforming = true;
                planet.Query.Add(new GameTimer(progress, 0.1f, () =>
                    {
                        planet.Terraform += 1;
                        planet.MaxTemperature = bounds.Max;
                        planet.MinTemperature = bounds.Min;
                        progress.PWidth = 0;
                        planet.IsTerraforming = false;

                        if (screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().IsVisible)
                        {
                            RefreshPlanetInfoPopup(planet);
                        }
                    },
                    CreateButton("QueryBtns/blacksquare", "QueryBtns/blacksquare", 0, 0, 1f)));
            }

            if (screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().IsVisible)
            {
                RefreshPlanetInfoPopup(planet);
            }
        }

        private TemperatureBounds GetNewTemperatureBounds(Planet planet)
        {
            Random r = new Random();
            int min, max;
            if (planet.Terraform == 1)
            {
                if (planet.MaxTemperature < 0)
                {
                    min = r.Next(-120, -100);
                    max = r.Next(min, min + 50);
                }
                else
                {
                    min = r.Next(90, 110);
                    max = r.Next(min, min + 50);
                }
            }
            else if (planet.Terraform == 2)
            {
                if (planet.MaxTemperature < 0)
                {
                    min = r.Next(-70, -60);
                    max = r.Next(min, min + 50);
                }
                else
                {
                    min = r.Next(60, 70);
                    max = r.Next(min, min + 50);
                }
            }
            else
            {
                min = r.Next(-40, -10);
                max = r.Next(min, min + 50);
            }

            return new TemperatureBounds(min, max);
        }
        //---------------------------------------------------------------
        //CREATE SHIP BTN CLICK
        //---------------------------------------------------------------
        private void CreateShipBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            Planet tempPlanet = FindPlanet(((SolarSystem)screens[currentScreen].Objects["SolarSystem"]));
            if (tempPlanet == null) { return; }
            ProgressBar progress = ((ProgressBar)((Popup)screens[currentScreen].Objects["MoreInfoPopup"]).Objects["Progress"]);
            SolarSystem ss = ((SolarSystem)screens["SolarSystem"].Objects["SolarSystem"]);

            tempPlanet.Query.Add(new GameTimer(progress, 0.1f, () =>
                {
                    ss.Objects.Add(ShipCreator.GetWorkerShip(tempPlanet));
                    progress.PWidth = 0;
                    if (screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().IsVisible)
                    {
                        RefreshPlanetInfoPopup(tempPlanet);
                    }
                    //Для задания ивентов корабля.
                    UnloadEvents("SolarSystem");
                    LoadEvents("SolarSystem");
                }, 
                CreateButton("QueryBtns/blacksquare", "QueryBtns/blacksquare", 0, 0, 1f)));

            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        //---------------------------------------------------------------
        //CREATE SHIP BTN CLICK
        //---------------------------------------------------------------
        private void StationBuilderBtnOnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            Planet tempPlanet = FindPlanet(((SolarSystem)screens[currentScreen].Objects["SolarSystem"]));
            if (tempPlanet == null) { return; }
            ProgressBar progress = ((ProgressBar)((Popup)screens[currentScreen].Objects["MoreInfoPopup"]).Objects["Progress"]);
            SolarSystem ss = ((SolarSystem)screens["SolarSystem"].Objects["SolarSystem"]);

            tempPlanet.Query.Add(new GameTimer(progress, 0.1f, () =>
            {
                ss.Objects.Add(ShipCreator.GetStationBuilder(tempPlanet));
                progress.PWidth = 0;
                if (screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().IsVisible)
                {
                    RefreshPlanetInfoPopup(tempPlanet);
                }
                //Для задания ивентов корабля.
                UnloadEvents("SolarSystem");
                LoadEvents("SolarSystem");
            },
                CreateButton("QueryBtns/blacksquare", "QueryBtns/blacksquare", 0, 0, 1f)));

            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        //---------------------------------------------------------------
        //SCIENCE BTN CLICK
        //---------------------------------------------------------------
        private void ScienceBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }
        }

        //---------------------------------------------------------------
        //OWN BTN CLICK
        //---------------------------------------------------------------
        private void OwnBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }
        }

        //---------------------------------------------------------------
        //DIPLOMACY BTN CLICK
        //---------------------------------------------------------------
        private void DiplomacyBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }
        }

        //---------------------------------------------------------------
        //BLUEPRINTS BTN CLICK
        //---------------------------------------------------------------
        private void BlueprintsBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }
        }

        //---------------------------------------------------------------
        //SHIP RIGHT BUTTON CLICK
        //---------------------------------------------------------------
        private IMoveble ShipRightButtonClick(Ship sender, MouseState ms)
        {
            return FindObjectUnderCursor(ms);
        }

        //---------------------------------------------------------------
        //SHIP SYSTEM QUIT
        //---------------------------------------------------------------
        private void ShipSystemQuit(Ship sender)
        {
            Texture2D texture = Content.Load<Texture2D>("LittleStars/ShipOnMap");
            Texture2D circle = Content.Load<Texture2D>("LittleStars/CurrentPlayerCircle");
            StarOnMap SOM = FindStarOnMapWithThisObject(sender);

            List<Ship> ships = new List<Ship>();
            ships.Add(sender);

            Vector2 position = new Vector2(SOM.Position.X + SOM.Width + 5, SOM.Position.Y + SOM.Height + 5);

            ShipOnMap temp = new ShipOnMap(texture, circle, position, new Vector2(Scales.FourTenth), ships);

            sender.OnClick -= ShipOnClick;
            sender.OnRightButtonClick -= ShipRightButtonClick;
            sender.OnSystemQuit -= ShipSystemQuit;

            screens["GalaxyMap"].Objects["Map"].ToMap().Objects.Add(temp.ToIDraw());
            screens["SolarSystem"].Objects["SolarSystem"].ToSolarSystem().Objects.Remove(sender.ToIDraw());
            //screens["SolarSystem"].Objects["SolarSystem"].ToSolarSystem().Objects = screens["SolarSystem"].Objects["SolarSystem"].ToSolarSystem().Objects.ToList<IDraw>();
        }

        //---------------------------------------------------------------
        //SHIP SYSTEM ENTER
        //---------------------------------------------------------------
        private void ShipSystemEnter(ShipOnMap sender, IMoveble target)
        {
            //Заставлять его нормально считать координаты.
            //И устанавливать IsMoving в фалс.
            foreach (Ship ship in sender.Ships)
            {
                target.ToStarOnMap().SS.Objects.Add(ship);
            }

            sender.OnSystemEnter -= ShipSystemEnter;
            sender.OnRightButtonClick -= ShipOnMapRightButtonClick;

            screens["GalaxyMap"].Objects["Map"].ToMap().Objects.Remove(sender);
        }

        //---------------------------------------------------------------
        //SHIP ON MAP RIGHT BUTTON CLICK
        //---------------------------------------------------------------
        private IMoveble ShipOnMapRightButtonClick(ShipOnMap sender, MouseState ms)
        {
            return FindObjectUnderCursor(ms);
        }

        //---------------------------------------------------------------
        //STATION BUILDER BUILDING START
        //---------------------------------------------------------------
        private void StationBuilderBuildingStart(StationBuilder builder)
        {
            //Добавлять стэйшнонбилд до всех корабелй в системе (вообще надо сделать сортировку, ато заебаться так можно)
            StationOnBuilding temp = StationCreator.GetStationOnBuilding(builder);

            screens[currentScreen].Objects[currentScreen].ToSolarSystem().Objects.Remove(builder);

            temp.OnBuildingComplete += new StationOnBuilding.OnBuildingCompleteHandler(StationOnBuildingComplete);

            screens[currentScreen].Objects[currentScreen].ToSolarSystem().Objects.Add(temp);
        }

        //---------------------------------------------------------------
        //STATION ON BUILDING COMPLETE
        //---------------------------------------------------------------
        private void StationOnBuildingComplete(StationOnBuilding sender)
        {
            try
            {
                StarOnMap SOM = FindStarOnMapWithThisObject(sender);
                if (SOM != null)
                {
                    SolarSystem ss = SOM.SS;
                    Station temp = StationCreator.GetEmptyStation(sender);
                    temp.OnDestroy += new OnDestroyHandler(OnDestroy);
                    ss.Objects.Add(temp);
                    ss.Objects.Remove(sender);
                }
                else
                {
                    throw new StarOnMapNotFoundException();
                }
            }
            catch
            {
                try
                {
                    BlackHoleOnMap BHOM = FindBlackHoleOnMapWithThisObject(sender);
                    if (BHOM != null)
                    {
                        BlackHoleSystem bhs = BHOM.System;
                        Station temp = StationCreator.GetEmptyStation(sender);
                        temp.OnDestroy += new OnDestroyHandler(OnDestroy);
                        bhs.Objects.Add(temp);
                        bhs.Objects.Remove(sender);
                    }
                    else
                    {
                        throw new BlackHoleOnMapNotFoundException();
                    }
                }
                catch
                {
                    return;
                }
            }
        }

        #region Helpers
        private Planet FindPlanet(SolarSystem ss)
        {
            foreach (IDraw obj in ss.Objects)
            {
                if (obj is Planet)
                {
                    if (((Planet)obj).Name == tempPlanetName)
                    {
                        return (Planet)obj;
                    }
                }
            }
            return null;
        }

        private StarOnMap FindStarOnMap(SolarSystem ss)
        {
            foreach (IDraw obj in ((Map)screens["GalaxyMap"].Objects["Map"]).Objects)
            {
                if (obj is StarOnMap)
                {
                    if (((StarOnMap)obj).SS == ss)
                    {
                        return (StarOnMap)obj;
                    }
                }
            }
            //Another epic crutch
            return null;
        }

        private string ConstructPlayerName()
        {
            if (players.Count != 0)
            {
                return "Player" + (players.Count + 1).ToString();
            }
            else
            {
                return "Player1";
            }
        }

        private IMoveble FindObjectUnderCursor(MouseState ms)
        {
            if (currentScreen == "SolarSystem")
            {
                SolarSystem ss = screens[currentScreen].Objects["SolarSystem"].ToSolarSystem();

                foreach (IMoveble obj in ss.Objects)
                {
                    float finalWidth = obj.X + obj.Width;
                    float finalHeight = obj.Y + obj.Height;
                    if (ms.X >= obj.X && ms.X <= finalWidth && ms.Y >= obj.Y && ms.Y <= finalHeight)
                    {
                        if (obj is Asteroid)
                        {
                            return obj;
                        }
                        else if (obj is Planet)
                        {
                            if (obj.ToPlanet().Owner == currentPlayer || obj.ToPlanet().Owner == "Nobody")
                            {
                                return obj;
                            }
                        }
                        else if (obj is StationOnBuilding)
                        {
                            if (obj.ToStationOnbuilding().Owner != currentPlayer)
                            {
                                return obj;
                            }
                        }
                        else if (obj is Station)
                        {
                            if (obj.ToStation().Owner != currentPlayer)
                            {
                                return obj;
                            }
                        }
                    }
                }
                return null;
            }
            else
            {
                foreach (IMoveble obj in screens["GalaxyMap"].Objects["Map"].ToMap().Objects)
                {
                    float finalWidth = obj.X + obj.Width;
                    float finalHeight = obj.Y + obj.Height;
                    if (ms.X >= obj.X && ms.X <= finalWidth && ms.Y >= obj.Y && ms.Y <= finalHeight)
                    {
                        if (obj is StarOnMap)
                        {
                            return obj;
                        }
                    }
                }
                return null;
            }
        }

        private StarOnMap FindStarOnMapWithThisObject(IDraw toFind)
        {
            foreach (IDraw obj in screens["GalaxyMap"].Objects["Map"].ToMap().Objects)
            {
                if (obj is StarOnMap)
                {
                    foreach (IDraw obj2 in obj.ToStarOnMap().SS.Objects)
                    {
                        if (Object.ReferenceEquals(toFind, obj2))
                        {
                            return obj.ToStarOnMap();
                        }
                    }
                }
            }
            return null;
        }

        private BlackHoleOnMap FindBlackHoleOnMapWithThisObject(IDraw toFind)
        {
            foreach (IDraw obj in screens["GalaxyMap"].Objects["Map"].ToMap().Objects)
            {
                if (obj is BlackHoleOnMap)
                {
                    foreach (IDraw obj2 in obj.ToBlackHoleOnMap().System.Objects)
                    {
                        if (Object.ReferenceEquals(toFind, obj2))
                        {
                            return obj.ToBlackHoleOnMap();
                        }
                    }
                }
            }
            return null;
        }

        private void AcceptPopupSettings(InfoPopupSettings settings, bool isOn = true)
        {
            Button colonize = screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().Objects["ColonizeBtn"].ToButton();
            Button terraform = screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().Objects["TerraformBtn"].ToButton();
            Button workerShip = screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().Objects["CreateShipBtn"].ToButton();
            Button cross = screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().Objects["CrossBtn"].ToButton();
            Button stationBuilder = screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().Objects["StationBuilderBtn"].ToButton();
            TextBox name = screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().Objects["NameTextBox"].ToTextBox();
            Planet planet = FindPlanet(screens["SolarSystem"].Objects["SolarSystem"].ToSolarSystem());
            if (planet == null) { return; }

            colonize.IsVisible = settings.IsColonize;
            terraform.IsVisible = settings.IsTerraform;
            workerShip.IsVisible = settings.IsWorkerShipCreate;
            cross.IsVisible = settings.IsCross;
            name.IsVisible = settings.IsName;
            stationBuilder.IsVisible = settings.IsStationBuilder;

            if (settings.IsColonize)
            {
                colonize.OnClick += new Button.OnClickHandler(ColonizeBtnClick);
            }
            else
            {
                colonize.OnClick -= ColonizeBtnClick;
            }

            if (settings.IsTerraform)
            {
                terraform.OnClick += new Button.OnClickHandler(TerraformBtnClick);
            }
            else
            {
                terraform.OnClick -= TerraformBtnClick;
            }

            if (settings.IsWorkerShipCreate)
            {
                workerShip.OnClick += new Button.OnClickHandler(CreateShipBtnClick);
            }
            else
            {
                workerShip.OnClick -= CreateShipBtnClick;
            }

            if (settings.IsCross)
            {
                cross.OnClick += new Button.OnClickHandler(PlanetInfoCloseClick);
            }
            else
            {
                cross.OnClick -= PlanetInfoCloseClick;
            }

            if (settings.IsName)
            {
                name.Content = planet.Name;
                name.OnEnter += new TextBox.OnEnterHandler(NameTextBoxEnter);
            }
            else
            {
                name.OnEnter -= NameTextBoxEnter;
            }

            if (settings.IsStationBuilder)
            {
                stationBuilder.OnClick += new Button.OnClickHandler(StationBuilderBtnOnClick);
            }
            else
            {
                stationBuilder.OnClick -= StationBuilderBtnOnClick;
            }

            if (isOn)
            {
                screens["SolarSystem"].Objects.Add("ProductQuery", planet.Query);
                planet.Query.IsVisible = true;
                screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().IsVisible = true;
            }
            else
            {
                planet.Query.IsVisible = false;
                screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().IsVisible = false;
                screens["SolarSystem"].Objects.Remove("ProductQuery");
            }
        }
        #endregion
    }
}
