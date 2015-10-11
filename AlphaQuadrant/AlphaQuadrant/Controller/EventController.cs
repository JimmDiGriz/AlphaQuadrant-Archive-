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
        #region EventController
        private void LoadEvents(string screen)
        {
            switch (screen)
            {
                case "Menu":
                    ((Button)screens["Menu"].Objects["ExitBtn"]).OnClick += new Button.OnClickHandler(ExitBtnClick);
                    ((Button)screens["Menu"].Objects["NewGameBtn"]).OnClick += new Button.OnClickHandler(NewGameBtnClick);
                    ((Button)screens["Menu"].Objects["LoadGameBtn"]).OnClick += new Button.OnClickHandler(LoadGameBtnClick);
                    ((Button)screens["Menu"].Objects["PropertiesBtn"]).OnClick += new Button.OnClickHandler(PropertiesBtnClick);
                    //((Button)screens["Menu"].Objects["TestBtn"]).OnClick += new Button.OnClickHandler(TestBtnClick);
                    ((Button)screens["Menu"].Objects["MultiPlayerBtn"]).OnClick += new Button.OnClickHandler(MultiPlayerBtnClick);
                    break;
                case "GalaxyMap":
                    foreach (IDraw obj in ((Map)screens["GalaxyMap"].Objects["Map"]).Objects)
                    {
                        if (obj is StarOnMap)
                        {
                            obj.ToStarOnMap().OnClick += new StarOnMap.OnClickHandler(StarOnMapClick);
                            obj.ToStarOnMap().OnOver += new StarOnMap.OnOverHandler(StarOnMapOver);
                            obj.ToStarOnMap().OnOverEnd += new StarOnMap.OnOverEndHandler(StarOnMapOnOverEnd);
                        }
                        else if (obj is BlackHoleOnMap)
                        {
                            obj.ToBlackHoleOnMap().OnClick += new BlackHoleOnMap.OnClickHandler(BlackHoleOnMapClick);
                            obj.ToBlackHoleOnMap().OnOver += new BlackHoleOnMap.OnOverHandler(BlackHoleOnMapOver);
                            obj.ToBlackHoleOnMap().OnOverEnd += new BlackHoleOnMap.OnOverEndHandler(BlackHoleOnMapOnOverEnd);
                        }
                        else if (obj is ShipOnMap)
                        {
                            obj.ToShipOnMap().OnRightButtonClick += new ShipOnMap.OnRightButtonClickHandler(ShipOnMapRightButtonClick);
                            obj.ToShipOnMap().OnSystemEnter += new ShipOnMap.OnSystemEnterHandler(ShipSystemEnter);
                            obj.ToShipOnMap().OnClick += new ShipOnMap.OnClickHandler(ShipOnMapClick);
                        }
                    }
                    HUDEvents("GalaxyMap", "Load");
                    break;
                case "SolarSystem":
                    foreach (IDraw obj in ((SolarSystem)screens["SolarSystem"].Objects["SolarSystem"]).Objects)
                    {
                        if (obj is Star)
                        { 
                            obj.ToStar().OnClick += new Star.OnClickHandler(SystemStarClick);
                            obj.ToStar().OnOver += new Star.OnOverHandler(SystemStarOnOver);
                            obj.ToStar().OnOverEnd += new Star.OnOverEndHandler(SystemStarOnOverEnd);
                        }
                        else if (obj is Planet)
                        { 
                            obj.ToPlanet().OnClick += new Planet.OnClickHandler(PlanetClick);
                            obj.ToPlanet().OnOver += new Planet.OnOverHandler(PlanetOnOver);
                            obj.ToPlanet().OnOverEnd += new Planet.OnOverEndHandler(PlanetOnOverEnd);
                        }
                        else if (obj is BlackHole)
                        {
                            obj.ToBlackHole().OnClick += new BlackHole.OnClickHandler(BlackHoleOnClick);
                            obj.ToBlackHole().OnOver += new BlackHole.OnOverHandler(BlackHoleOnOver);
                            obj.ToBlackHole().OnOverEnd += new BlackHole.OnOverEndHandler(BlackHoleOnOverEnd);
                        }
                        else if (obj is Ship)
                        {
                            obj.ToShip().OnRightButtonClick += new Ship.OnRightButtonClickHandler(ShipRightButtonClick);
                            obj.ToShip().OnSystemQuit += new Ship.OnSystemQuitHandler(ShipSystemQuit);
                            obj.ToShip().OnClick += new Ship.OnClickHandler(ShipOnClick);
                            if (obj is StationBuilder)
                            {
                                obj.ToStationBuilder().OnBuildingStart += new StationBuilder.OnBuildingStartHandler(StationBuilderBuildingStart);
                            }
                        }

                        if (obj is IDamagable)
                        {
                            obj.ToIDamagable().OnDestroy -= OnDestroy;
                            obj.ToIDamagable().OnDestroy += new OnDestroyHandler(OnDestroy);
                        }
                    }
                    HUDEvents("SolarSystem", "Load");
                    break;
                case "BlackHoleSystem":
                    //Debug.Assert(false, "Called BlackHoleSystem event controller");
                    foreach (IDraw obj in ((BlackHoleSystem)screens["BlackHoleSystem"].Objects["BlackHoleSystem"]).Objects)
                    {
                        //Debug.Assert(false, "Try to found black holes: "+obj.GetType());
                        if (obj is BlackHole)
                        {
                            //Debug.Assert(false, "I found black hole!" + ((BlackHole)obj).Name);
                            obj.ToBlackHole().OnClick += new BlackHole.OnClickHandler(BlackHoleOnClick);
                            obj.ToBlackHole().OnOver += new BlackHole.OnOverHandler(BlackHoleOnOver);
                            obj.ToBlackHole().OnOverEnd += new BlackHole.OnOverEndHandler(BlackHoleOnOverEnd);
                        }
                    }
                    HUDEvents("BlackHoleSystem", "Load");
                    break;
                case "Choose":
                    ((Button)screens["Choose"].Objects["StarBtn"]).OnClick += new Button.OnClickHandler(StartGameBtnClick);
                    ((Button)screens["Choose"].Objects["StarBtn"]).OnClick += new Button.OnClickHandler(SaveRaceBtnClick);
                    ((Button)screens["Choose"].Objects["StarBtn"]).OnClick += new Button.OnClickHandler(LoadRaceBtnClick);
                    //Properties buttons events
                    ((Button)screens["Choose"].Objects["DamagePlusBtn"]).OnClick += new Button.OnClickHandler(DamageButtonPlusClick);
                    ((Button)screens["Choose"].Objects["DefencePlusBtn"]).OnClick += new Button.OnClickHandler(DefenceButtonPlusClick);
                    ((Button)screens["Choose"].Objects["SpeedPlusBtn"]).OnClick += new Button.OnClickHandler(SpeedButtonPlusClick);
                    ((Button)screens["Choose"].Objects["SciencePlusBtn"]).OnClick += new Button.OnClickHandler(ScienceButtonPlusClick);
                    ((Button)screens["Choose"].Objects["ProductPlusBtn"]).OnClick += new Button.OnClickHandler(ProductButtonPlusClick);
                    ((Button)screens["Choose"].Objects["DamageMinusBtn"]).OnClick += new Button.OnClickHandler(DamageButtonMinusClick);
                    ((Button)screens["Choose"].Objects["DefenceMinusBtn"]).OnClick += new Button.OnClickHandler(DefenceButtonMinusClick);
                    ((Button)screens["Choose"].Objects["SpeedMinusBtn"]).OnClick += new Button.OnClickHandler(SpeedButtonMinusClick);
                    ((Button)screens["Choose"].Objects["ScienceMinusBtn"]).OnClick += new Button.OnClickHandler(ScienceButtonMinusClick);
                    ((Button)screens["Choose"].Objects["ProductMinusBtn"]).OnClick += new Button.OnClickHandler(ProductButtonMinusClick);
                    break;
                default:
                    Debug.Assert(false, "For some reason called default event controoler");
                    break;
            }
        }

        private void UnloadEvents(string screen)
        {
            switch (screen)
            {
                case "Menu":
                    ((Button)screens["Menu"].Objects["ExitBtn"]).OnClick -= ExitBtnClick;
                    ((Button)screens["Menu"].Objects["NewGameBtn"]).OnClick -= NewGameBtnClick;
                    ((Button)screens["Menu"].Objects["LoadGameBtn"]).OnClick -= LoadGameBtnClick;
                    ((Button)screens["Menu"].Objects["PropertiesBtn"]).OnClick -= PropertiesBtnClick;
                    //((Button)screens["Menu"].Objects["TestBtn"]).OnClick -= TestBtnClick;
                    ((Button)screens["Menu"].Objects["MultiPlayerBtn"]).OnClick -= MultiPlayerBtnClick;
                    break;
                case "GalaxyMap":
                    foreach (IDraw obj in ((Map)screens["GalaxyMap"].Objects["Map"]).Objects)
                    {
                        if (obj is StarOnMap)
                        {
                            obj.ToStarOnMap().OnClick -= StarOnMapClick;
                            obj.ToStarOnMap().OnOver -= StarOnMapOver;
                            obj.ToStarOnMap().OnOverEnd -= StarOnMapOnOverEnd;
                        }
                        else if (obj is BlackHoleOnMap)
                        {
                            obj.ToBlackHoleOnMap().OnClick -= BlackHoleOnMapClick;
                            obj.ToBlackHoleOnMap().OnOver -= BlackHoleOnMapOver;
                            obj.ToBlackHoleOnMap().OnOverEnd -= BlackHoleOnMapOnOverEnd;
                        }
                        else if (obj is ShipOnMap)
                        {
                            obj.ToShipOnMap().OnRightButtonClick -= ShipOnMapRightButtonClick;
                            obj.ToShipOnMap().OnSystemEnter -= ShipSystemEnter;
                            obj.ToShipOnMap().OnClick -= ShipOnMapClick;
                        }
                    }
                    HUDEvents("GalaxyMap", "Unload");
                    break;
                case "SolarSystem":
                    foreach (IDraw obj in ((SolarSystem)screens["SolarSystem"].Objects["SolarSystem"]).Objects)
                    {
                        if (obj is Star)
                        { 
                            obj.ToStar().OnClick -= SystemStarClick;
                            obj.ToStar().OnOver -= SystemStarOnOver;
                            obj.ToStar().OnOverEnd -= SystemStarOnOverEnd;
                        }
                        else if (obj is Planet)
                        {
                            obj.ToPlanet().OnClick -= PlanetClick;
                            obj.ToPlanet().OnOver -= PlanetOnOver;
                            obj.ToPlanet().OnOverEnd -= PlanetOnOverEnd;
                        }
                        else if (obj is BlackHole)
                        {
                            obj.ToBlackHole().OnClick -= BlackHoleOnClick;
                            obj.ToBlackHole().OnOver -= BlackHoleOnOver;
                            obj.ToBlackHole().OnOverEnd -= BlackHoleOnOverEnd;
                        }
                        else if (obj is Ship)
                        {
                            obj.ToShip().OnRightButtonClick -= ShipRightButtonClick;
                            obj.ToShip().OnSystemQuit -= ShipSystemQuit;
                            obj.ToShip().OnClick -= ShipOnClick;
                            if (obj is StationBuilder)
                            {
                                obj.ToStationBuilder().OnBuildingStart -= StationBuilderBuildingStart;
                            }
                        }
                    }
                    HUDEvents("SolarSystem", "Unload");
                    break;
                case "BlackHoleSystem":
                    foreach (IDraw obj in ((BlackHoleSystem)screens["BlackHoleSystem"].Objects["BlackHoleSystem"]).Objects)
                    {
                        if (obj is BlackHole)
                        {
                            obj.ToBlackHole().OnClick -= BlackHoleOnClick;
                            obj.ToBlackHole().OnOver -= BlackHoleOnOver;
                            obj.ToBlackHole().OnOverEnd -= BlackHoleOnOverEnd;
                        }
                    }
                    HUDEvents("BlackHoleSystem", "Unload");
                    break;
                case "Choose":
                    ((Button)screens["Choose"].Objects["StarBtn"]).OnClick -= StartGameBtnClick;
                    ((Button)screens["Choose"].Objects["StarBtn"]).OnClick -= SaveRaceBtnClick;
                    ((Button)screens["Choose"].Objects["StarBtn"]).OnClick -= LoadRaceBtnClick;
                    //Properties buttons events
                    ((Button)screens["Choose"].Objects["DamagePlusBtn"]).OnClick -= DamageButtonPlusClick;
                    ((Button)screens["Choose"].Objects["DefencePlusBtn"]).OnClick -= DefenceButtonPlusClick;
                    ((Button)screens["Choose"].Objects["SpeedPlusBtn"]).OnClick -= SpeedButtonPlusClick;
                    ((Button)screens["Choose"].Objects["SciencePlusBtn"]).OnClick -= ScienceButtonPlusClick;
                    ((Button)screens["Choose"].Objects["ProductPlusBtn"]).OnClick -= ProductButtonPlusClick;
                    ((Button)screens["Choose"].Objects["DamageMinusBtn"]).OnClick -= DamageButtonMinusClick;
                    ((Button)screens["Choose"].Objects["DefenceMinusBtn"]).OnClick -= DefenceButtonMinusClick;
                    ((Button)screens["Choose"].Objects["SpeedMinusBtn"]).OnClick -= SpeedButtonMinusClick;
                    ((Button)screens["Choose"].Objects["ScienceMinusBtn"]).OnClick -= ScienceButtonMinusClick;
                    ((Button)screens["Choose"].Objects["ProductMinusBtn"]).OnClick -= ProductButtonMinusClick;
                    break;
                default:
                    break;
            }
        }

        private void HUDEvents(string screen, string action = "Unload")
        {
            if (action == "Load")
            {
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["InfoBtn"]).OnClick += new Button.OnClickHandler(InfoBtnClick);
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["ScienceBtn"]).OnClick += new Button.OnClickHandler(ScienceBtnClick);
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["OwnBtn"]).OnClick += new Button.OnClickHandler(OwnBtnClick);
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["DiplomacyBtn"]).OnClick += new Button.OnClickHandler(DiplomacyBtnClick);
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["BlueprintsBtn"]).OnClick += new Button.OnClickHandler(BlueprintsBtnClick);
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["MenuBtn"]).OnClick += new Button.OnClickHandler(MenuBtnClick);
            }
            else
            {
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["InfoBtn"]).OnClick -= InfoBtnClick;
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["ScienceBtn"]).OnClick -= ScienceBtnClick;
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["OwnBtn"]).OnClick -= OwnBtnClick;
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["DiplomacyBtn"]).OnClick -= DiplomacyBtnClick;
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["BlueprintsBtn"]).OnClick -= BlueprintsBtnClick;
                ((Button)((Popup)screens[screen].Objects["UpperPanel"]).Objects["MenuBtn"]).OnClick -= MenuBtnClick;
            }
        }

        private void EscEvents(Screen screen)
        {
            if (((Popup)screen.Objects["EscPopup"]).IsVisible == false)
            {
                ((Popup)screen.Objects["EscPopup"]).IsVisible = true;
                ((Button)((Popup)screen.Objects["EscPopup"]).Objects["ContinueBtn"]).OnClick += new Button.OnClickHandler(EscContinueClick);
                ((Button)((Popup)screen.Objects["EscPopup"]).Objects["SaveBtn"]).OnClick += new Button.OnClickHandler(EscSaveClick);
                ((Button)((Popup)screen.Objects["EscPopup"]).Objects["LoadBtn"]).OnClick += new Button.OnClickHandler(EscLoadClick);
                ((Button)((Popup)screen.Objects["EscPopup"]).Objects["OptionsBtn"]).OnClick += new Button.OnClickHandler(EscOptionsClick);
                ((Button)((Popup)screen.Objects["EscPopup"]).Objects["ExitBtn"]).OnClick += new Button.OnClickHandler(EscExitClick);
            }
            else if (((Popup)screen.Objects["EscPopup"]).IsVisible == true)
            {
                ((Popup)screen.Objects["EscPopup"]).IsVisible = false ;
                ((Button)((Popup)screen.Objects["EscPopup"]).Objects["ContinueBtn"]).OnClick -= EscContinueClick;
                ((Button)((Popup)screen.Objects["EscPopup"]).Objects["ExitBtn"]).OnClick -= EscExitClick;
                ((Button)((Popup)screen.Objects["EscPopup"]).Objects["SaveBtn"]).OnClick -= EscSaveClick;
                ((Button)((Popup)screen.Objects["EscPopup"]).Objects["LoadBtn"]).OnClick -= EscLoadClick;
                ((Button)((Popup)screen.Objects["EscPopup"]).Objects["OptionsBtn"]).OnClick -= EscOptionsClick;
            }
        }

        private bool IsHigher(IDraw sender, MouseState ms)
        {
            bool isAfterSender = false;
            bool isAfterSenderChangingInPopup = false;
            List<IDraw> objectsAfterSender = new List<IDraw>();

            foreach (KeyValuePair<string, IDraw> obj in screens[currentScreen].Objects)
            {
                isAfterSenderChangingInPopup = false;
                if (isSkipingObject(obj.Value)) 
                { 
                    continue; 
                }

                if (!isAfterSender && Object.ReferenceEquals(obj.Value, sender))
                {
                    isAfterSender = true;
                }

                if (obj.Value is Popup)
                {
                    if (obj.Value.ToPopup().IsVisible)
                    {
                        foreach (KeyValuePair<string, IDraw> popupObj in obj.Value.ToPopup().Objects)
                        {
                            if (obj.Value is ProductQuery)
                            { 
                                if (obj.Value.ToProductQuery().IsVisible)
                                {
                                    foreach (IDraw pqObj in obj.Value.ToProductQuery().Buttons)
                                    {
                                        if (!isAfterSender && Object.ReferenceEquals(pqObj, sender))
                                        {
                                            isAfterSender = true;
                                            isAfterSenderChangingInPopup = true;
                                        }

                                        if (isAfterSender && !Object.ReferenceEquals(pqObj, sender) && pqObj.IsVisible)
                                        {
                                            objectsAfterSender.Add(pqObj);
                                        }
                                    }
                                }
                            }
                            if (isSkipingObject(popupObj.Value))
                            {
                                continue;
                            }

                            if (!isAfterSender && Object.ReferenceEquals(popupObj.Value, sender))
                            {
                                isAfterSender = true;
                                isAfterSenderChangingInPopup = true;
                            }

                            if (isAfterSender && !Object.ReferenceEquals(popupObj.Value, sender) && popupObj.Value.IsVisible)
                            {
                                objectsAfterSender.Add(popupObj.Value);
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                if (isAfterSender && !Object.ReferenceEquals(obj.Value, sender) && !isAfterSenderChangingInPopup)
                {
                    objectsAfterSender.Add(obj.Value);
                }

                if (obj.Value is SolarSystem)
                {
                    foreach (IDraw ssObj in obj.Value.ToSolarSystem().Objects)
                    {
                        if (isSkipingObject(ssObj))
                        {
                            continue;
                        }

                        if (!isAfterSender && Object.ReferenceEquals(ssObj, sender))
                        {
                            isAfterSender = true;
                        }

                        if (isAfterSender && !Object.ReferenceEquals(ssObj, sender))
                        {
                            objectsAfterSender.Add(ssObj);
                        }
                    }
                }

                if (obj.Value is Map)
                {
                    foreach (IDraw gmObj in obj.Value.ToMap().Objects)
                    {
                        if (isSkipingObject(gmObj))
                        {
                            continue;
                        }

                        if (!isAfterSender && Object.ReferenceEquals(gmObj, sender))
                        {
                            isAfterSender = true;
                        }

                        if (isAfterSender && !Object.ReferenceEquals(gmObj, sender))
                        {
                            objectsAfterSender.Add(gmObj);
                        }
                    }
                }
            }

            foreach (IDraw obj in objectsAfterSender)
            {
                if (!CheckCoordinates(obj.ToIMoveble(), ms))
                {
                    return false;
                }
            }
            return true;
        }

        private bool isSkipingObject(IDraw obj)
        { 
            if (obj is GameString || obj is BackGround || obj is ProductQuery)
            {
                return true;
            }
            return false;
        }

        private bool CheckCoordinates(IMoveble obj, MouseState ms)
        {
            float finalWidth = obj.X + obj.Width;
            float finalHeight = obj.Y + obj.Height;
            if (ms.X >= obj.X && ms.X <= finalWidth && ms.Y >= obj.Y && ms.Y <= finalHeight)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
