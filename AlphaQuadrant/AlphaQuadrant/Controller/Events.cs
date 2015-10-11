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
        #region Events
        //---------------------------------------------------------------
        //EXIT BUTTON CLICK
        //---------------------------------------------------------------
        private void ExitBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            ((Popup)screens["Menu"].Objects["ExitPopup"]).IsVisible = true;
            ((Button)(((Popup)screens["Menu"].Objects["ExitPopup"]).Objects["ExitPopupBtnYes"])).OnClick += new Button.OnClickHandler(ExitYesBtn);
            ((Button)(((Popup)screens["Menu"].Objects["ExitPopup"]).Objects["ExitPopupBtnNo"])).OnClick += new Button.OnClickHandler(ExitNoBtn);
            UnloadEvents("Menu");
        }
        //---------------------------------------------------------------
        //EXIT BUTTON YES CLICK
        //---------------------------------------------------------------
        private void ExitYesBtn(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            this.Exit();
        }
        //---------------------------------------------------------------
        //EXIT BUTTON NO CLICK
        //---------------------------------------------------------------
        private void ExitNoBtn(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            ((Popup)screens["Menu"].Objects["ExitPopup"]).IsVisible = false;
            ((Button)(((Popup)screens["Menu"].Objects["ExitPopup"]).Objects["ExitPopupBtnYes"])).OnClick -= ExitYesBtn;
            ((Button)(((Popup)screens["Menu"].Objects["ExitPopup"]).Objects["ExitPopupBtnNo"])).OnClick -= ExitNoBtn;
            LoadEvents("Menu");
        }
        //---------------------------------------------------------------
        //NEW GAME BUTTON CLICK
        //---------------------------------------------------------------
        private void NewGameBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            UnloadEvents("Menu");
            CreateChooseRace();
            LoadEvents("Choose");
            currentScreen = "Choose";
            /*CreateGen();
            currentScreen = "Gen";
            Thread t = new Thread(WorldGenerator);
            t.Start();*/
        }
        //---------------------------------------------------------------
        //MULTI PLAYER BUTTON CLICK
        //---------------------------------------------------------------
        private void MultiPlayerBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }
        }
        //---------------------------------------------------------------
        //LOAD GAME BUTTON CLICK
        //---------------------------------------------------------------
        private void LoadGameBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }
        }
        //---------------------------------------------------------------
        //PROPERTIES BUTTON CLICK
        //---------------------------------------------------------------
        private void PropertiesBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            ((Popup)screens["Menu"].Objects["TestPopup"]).IsVisible = true;
            /*SoundEffect snd = Content.Load<SoundEffect>("Sound/Click");
            snd.Play();*/
        }
        //---------------------------------------------------------------
        //BLACKHOLE ON MAP OVER
        //---------------------------------------------------------------
        private void BlackHoleOnMapOver(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            ((GameString)((Popup)screens["GalaxyMap"].Objects["InfoPopup"]).Objects["InfoString"]).Str = "Name: " + ((BlackHoleOnMap)sender).Name;
            ((Popup)screens["GalaxyMap"].Objects["InfoPopup"]).X = ((BlackHoleOnMap)sender).X + ((BlackHoleOnMap)sender).Width * popupPositionCoef;
            ((Popup)screens["GalaxyMap"].Objects["InfoPopup"]).Y = ((BlackHoleOnMap)sender).Y + ((BlackHoleOnMap)sender).Height * popupPositionCoef;
            ((Popup)screens["GalaxyMap"].Objects["InfoPopup"]).IsVisible = true;
        }
        //---------------------------------------------------------------
        //BLACKHOLE ON MAP ON OVER END
        //---------------------------------------------------------------
        private void BlackHoleOnMapOnOverEnd()
        {
            ((Popup)screens["GalaxyMap"].Objects["InfoPopup"]).IsVisible = false;
        }
        //---------------------------------------------------------------
        //BLACKHOLE ON MAP CLICK
        //---------------------------------------------------------------
        private void BlackHoleOnMapClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            CreateBlackHoleSystem(((BlackHoleOnMap)sender).System);
            UnloadEvents("GalaxyMap");
            LoadEvents("BlackHoleSystem");
            currentScreen = "BlackHoleSystem";
        }
        //---------------------------------------------------------------
        //STAR ON MAP OVER
        //---------------------------------------------------------------
        private void StarOnMapOver(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            ((GameString)((Popup)screens["GalaxyMap"].Objects["InfoPopup"]).Objects["InfoString"]).Str = "Name:   " + ((StarOnMap)sender).Name + "\n";
            ((GameString)((Popup)screens["GalaxyMap"].Objects["InfoPopup"]).Objects["InfoString"]).Str += "Owner:  " + ((StarOnMap)sender).Owner + "\n";
            ((Popup)screens["GalaxyMap"].Objects["InfoPopup"]).X = ((StarOnMap)sender).X + ((StarOnMap)sender).Width * popupPositionCoef;
            ((Popup)screens["GalaxyMap"].Objects["InfoPopup"]).Y = ((StarOnMap)sender).Y + ((StarOnMap)sender).Height * popupPositionCoef;
            ((Popup)screens["GalaxyMap"].Objects["InfoPopup"]).IsVisible = true;
        }
        //---------------------------------------------------------------
        //STAR ON MAP ON OVER END
        //---------------------------------------------------------------
        private void StarOnMapOnOverEnd()
        {
            ((Popup)screens["GalaxyMap"].Objects["InfoPopup"]).IsVisible = false;
        }
        //---------------------------------------------------------------
        //STAR ON MAP CLICK
        //---------------------------------------------------------------
        private void StarOnMapClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            SolarSystemOpen((StarOnMap)sender);
            UnloadEvents("GalaxyMap");
        }
        //---------------------------------------------------------------
        //SYSTEM STAR CLICK
        //---------------------------------------------------------------
        private void SystemStarClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }
        }
        //---------------------------------------------------------------
        //SYSTEM STAR ON OVER
        //---------------------------------------------------------------
        private void SystemStarOnOver(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            ((GameString)((Popup)screens["SolarSystem"].Objects["InfoPopup"]).Objects["InfoString"]).Str = "Name: " + ((Star)sender).Name;
            ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).X = ((Star)sender).X + ((Star)sender).Width * popupPositionCoef;
            ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).Y = ((Star)sender).Y + ((Star)sender).Height * popupPositionCoef;
            ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).IsVisible = true;
        }
        //---------------------------------------------------------------
        //PLANET ON OVER
        //---------------------------------------------------------------
        private void PlanetOnOver(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            ((GameString)((Popup)screens["SolarSystem"].Objects["InfoPopup"]).Objects["InfoString"]).Str = "Name:  " + ((Planet)sender).Name + "\n";
            ((GameString)((Popup)screens["SolarSystem"].Objects["InfoPopup"]).Objects["InfoString"]).Str += "Owner: " + ((Planet)sender).Owner + "\n";
            ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).X = ((Planet)sender).X + ((Planet)sender).Width * popupPositionCoef;
            ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).Y = ((Planet)sender).Y + ((Planet)sender).Height * popupPositionCoef;
            ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).IsVisible = true;
        }
        //---------------------------------------------------------------
        //BLACKHOLE ON CLICK
        //---------------------------------------------------------------
        private void BlackHoleOnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }
        }
        //---------------------------------------------------------------
        //BLACKHOLE ON OVER
        //---------------------------------------------------------------
        private void BlackHoleOnOver(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            if (currentScreen == "SolarSystem")
            {
                ((GameString)((Popup)screens["SolarSystem"].Objects["InfoPopup"]).Objects["InfoString"]).Str = "Name: " + ((BlackHole)sender).Name;
                ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).X = ((BlackHole)sender).X + ((BlackHole)sender).Width * popupPositionCoef;
                ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).Y = ((BlackHole)sender).Y + ((BlackHole)sender).Height * popupPositionCoef;
                ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).IsVisible = true;
            }
            else
            {
                ((GameString)((Popup)screens["BlackHoleSystem"].Objects["InfoPopup"]).Objects["InfoString"]).Str = "Name: " + ((BlackHole)sender).Name;
                ((Popup)screens["BlackHoleSystem"].Objects["InfoPopup"]).X = ((BlackHole)sender).X + ((BlackHole)sender).Width * popupPositionCoef;
                ((Popup)screens["BlackHoleSystem"].Objects["InfoPopup"]).Y = ((BlackHole)sender).Y + ((BlackHole)sender).Height * popupPositionCoef;
                ((Popup)screens["BlackHoleSystem"].Objects["InfoPopup"]).IsVisible = true;
            }
        }
        //---------------------------------------------------------------
        //BLACKHOLE ON OVER END
        //---------------------------------------------------------------
        private void BlackHoleOnOverEnd()
        {
            if (currentScreen == "SolarSystem")
            {
                ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).IsVisible = false;
            }
            else
            {
                ((Popup)screens["BlackHoleSystem"].Objects["InfoPopup"]).IsVisible = false;
            }
        }
        //---------------------------------------------------------------
        //SYSTEM STAR ON OVER END
        //---------------------------------------------------------------
        private void SystemStarOnOverEnd()
        {
            ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).IsVisible = false;
        }
        //---------------------------------------------------------------
        //PLANET ON OVER END
        //---------------------------------------------------------------
        private void PlanetOnOverEnd()
        {
            ((Popup)screens["SolarSystem"].Objects["InfoPopup"]).IsVisible = false;
        }
        //---------------------------------------------------------------
        //PLANET INFO CLOSE CLICK
        //---------------------------------------------------------------
        private void PlanetInfoCloseClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            HidePlanetInfoPopup();
        }
        //---------------------------------------------------------------
        //ESC CONTINUE CLICK
        //---------------------------------------------------------------
        private void EscContinueClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            //Always on galaxy map
            EscEvents(screens["GalaxyMap"]);
            if (screens.ContainsKey("SolarSystem"))
            {
                EscEvents(screens["SolarSystem"]);
            }
            if (screens.ContainsKey("BlackHoleSystem"))
            {
                EscEvents(screens["BlackHoleSystem"]);
            }
        }
        //---------------------------------------------------------------
        //ESC EXIT CLICK
        //---------------------------------------------------------------
        private void EscExitClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            this.Exit();
        }
        //---------------------------------------------------------------
        //ESC SAVE CLICK
        //---------------------------------------------------------------
        private void EscSaveClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            SaveGame();
        }
        //---------------------------------------------------------------
        //ESC LOAD CLICK
        //---------------------------------------------------------------
        private void EscLoadClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }
        }
        //---------------------------------------------------------------
        //ESC OPTIONS CLICK
        //---------------------------------------------------------------
        private void EscOptionsClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }
        }
        //---------------------------------------------------------------
        //INFO BTN CLICK
        //---------------------------------------------------------------
        private void InfoBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            ((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).IsVisible = true;
            ((Button)((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).Objects["CrossBtn"]).OnClick += new Button.OnClickHandler(PlayerInfoCrossClick);
            ((GameString)((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).Objects["PlayerName"]).Str = currentPlayer;
            ((GameString)((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).Objects["RaceName"]).Str = players[currentPlayer].Race.Name;
            ((GameString)((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).Objects["DamageString"]).Str = players[currentPlayer].Race.Damage.ToString();
            ((GameString)((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).Objects["DefenceString"]).Str = players[currentPlayer].Race.Defence.ToString();
            ((GameString)((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).Objects["SpeedString"]).Str = players[currentPlayer].Race.Speed.ToString();
            ((GameString)((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).Objects["ScienceString"]).Str = players[currentPlayer].Race.Science.ToString();
            ((GameString)((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).Objects["ProductString"]).Str = players[currentPlayer].Race.Product.ToString();
            /*((GameString)((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).Objects["Info"]).Str = "Player: " + currentPlayer
                + "\nRace: " + players[currentPlayer].PRace.Name;*/
        }
        //---------------------------------------------------------------
        //PLAYER INFO CROSS CLICK
        //---------------------------------------------------------------
        private void PlayerInfoCrossClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            ((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).IsVisible = false;
            ((Button)((Popup)screens[currentScreen].Objects["PlayerInfoPopup"]).Objects["CrossBtn"]).OnClick -= PlayerInfoCrossClick;
        }
        //---------------------------------------------------------------
        //MENU BTN CLICK
        //---------------------------------------------------------------
        private void MenuBtnClick(object sender, MouseState ms)
        {
            if (!IsHigher(sender.ToIDraw(), ms))
            {
                return;
            }

            EscEvents(screens[currentScreen]);
        }

        //---------------------------------------------------------------
        //SHIP ON CLICK
        //---------------------------------------------------------------
        private bool ShipOnClick(Ship sender, MouseState ms)
        {
            return IsHigher(sender.ToIDraw(), ms);
        }

        //---------------------------------------------------------------
        //SHIP ON MAP ON CLICK
        //---------------------------------------------------------------
        private bool ShipOnMapClick(ShipOnMap sender, MouseState ms)
        {
            return IsHigher(sender.ToIDraw(), ms);
        }

        //---------------------------------------------------------------
        //ON OBJECT DESTROY
        //---------------------------------------------------------------
        private void OnDestroy(IDamagable sender)
        {
            try
            {
                IDraw system = FindSolarOrBlackHoleSystemWithObject(sender.ToIDraw());
                if (system == null)
                {
                    throw new SystemNotFoundException();
                }
                else if (system is SolarSystem)
                {
                    system.ToSolarSystem().Objects.Remove(
                        system.ToSolarSystem().Objects.Find(x => Object.ReferenceEquals(sender, x)));
                    if (sender is Asteroid)
                    {
                        Asteroid temp = CreateAsteroid();
                        temp.OnDestroy += new OnDestroyHandler(OnDestroy);
                        system.ToSolarSystem().Objects.Add(temp);
                    }
                }
                else if (system is BlackHoleSystem)
                {
                    system.ToBlackHoleSystem().Objects.Remove(
                        system.ToBlackHoleSystem().Objects.Find(x => Object.ReferenceEquals(sender, x)));
                    if (sender is Asteroid)
                    {
                        Asteroid temp = CreateAsteroid();
                        temp.OnDestroy += new OnDestroyHandler(OnDestroy);
                        system.ToBlackHoleSystem().Objects.Add(temp);
                    }
                }
                else
                {
                    throw new UnknownException();
                }
            }
            //TODO: Запилить тут обработку исключений нормальную.
            catch
            {
                return;
            }
            
        }
        #endregion

        #region EventHelpers
        //Я буду гореть в аду за это название.
        private IDraw FindSolarOrBlackHoleSystemWithObject(IDraw toFind)
        {
            foreach (IDraw obj in screens["GalaxyMap"].Objects["Map"].ToMap().Objects)
            {
                if (obj is StarOnMap)
                {
                    foreach (IDraw obj2 in obj.ToStarOnMap().SS.Objects)
                    {
                        if (Object.ReferenceEquals(toFind, obj2))
                        {
                            return obj.ToStarOnMap().SS;
                        }
                    }
                }
                else if (obj is BlackHoleOnMap)
                {
                    foreach (IDraw obj2 in obj.ToBlackHoleOnMap().System.Objects)
                    {
                        if (Object.ReferenceEquals(toFind, obj2))
                        {
                            return obj.ToBlackHoleOnMap().System;
                        }
                    }
                }
            }
            return null;
        }
        #endregion
    }
}
