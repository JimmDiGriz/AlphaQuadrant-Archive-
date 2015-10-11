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

namespace AlphaQuadrant
{
    public partial class AlphaQuadrant : Microsoft.Xna.Framework.Game
    {
        #region LoadScreens etc
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //GraphicsDevice.SamplerStates[0].Filter = TextureFilter.Linear;
            //GraphicsDevice.SamplerStates[0].Filter = TextureFilter.Anisotropic;
            //graphics.GraphicsDevice.SamplerStates[0].Filter = TextureFilter.Anisotropic;
            circleTexture = Content.Load<Texture2D>("Ships/OnOverCircle");
            SetCirclesTextures();
            if (currentScreen == "Menu")
            {
                CreateMenu(/*spriteBatch*/);
            }
        }

        private void SetCirclesTextures()
        {
            StarOnMapCircles = new Dictionary<string, Texture2D>();
            StarOnMapCircles.Add("CurrentPlayer", Content.Load<Texture2D>("LittleStars/CurrentPlayerCircle"));
            StarOnMapCircles.Add("Nobody", Content.Load<Texture2D>("LittleStars/NobodyCircle"));
            StarOnMapCircles.Add("Enemy", Content.Load<Texture2D>("LittleStars/EnemyCircle"));
        }
        #endregion

        protected override void UnloadContent() { }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(255,255,255, 255));
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise);
            spriteBatch.Begin();
            /*graphics.PreferMultiSampling = true;
            GraphicsDevice.SamplerStates[0].Filter = TextureFilter.Anisotropic;
            graphics.GraphicsDevice.SamplerStates[0].Filter = TextureFilter.Anisotropic;*/
            /*graphics.GraphicsDevice.SamplerStates[0].MagFilter = TextureFilter.None;
            graphics.GraphicsDevice.SamplerStates[0].MinFilter = TextureFilter.None;
            graphics.GraphicsDevice.SamplerStates[0].MipFilter = TextureFilter.None;*/
            screens[currentScreen].Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /*private void DrawPlanetCircle()
        {
            VertexPositionColor[] pointList;
            int points = 80;
            short[] lineStripIndices;

        }

        private void CreateLineStrip(VertexPositionColor[] pointList, int points, short[] lineStripIndices)
        {
            //Координаты центра:
            Vector3 CENTER = new Vector3(100, 100, 0);
            //Радиус:
            float R = 100;
            lineStripIndices = new short[points];
            for (int i = 0; i < points; i++) lineStripIndices[i] = (short)(i);
            pointList = new VertexPositionColor[points];
            for (int i = 0; i < points; i++)
            {
                float phi = (float)(i) / (float)(points - 1) * MathHelper.TwoPi;
                pointList[i] = new VertexPositionColor(
                    new Vector3(CENTER.X - R * (float)Math.Sin(phi), CENTER.Y - R * (float)Math.Cos(phi), 0), Color.White);
            }
            vertexBuffer = new VertexBuffer(graphics.GraphicsDevice, vertexDeclaration,
                points, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionColor>(pointList);
        }*/

        #region CreateScreens
        //------------------------------------------------------------
        //MENU CREATE
        //------------------------------------------------------------
        private void CreateMenu()
        {
            Screen menu = new Screen();
            BackGround menuBackground = CreateBackground("Textures/BackgroundPlanet3", Scales.None, 255f);
            menu.Objects.Add("MenuBack", menuBackground);
            BackGround uiBorder = CreateBackground("Textures/Border", Scales.None, 255f);
            menu.Objects.Add("UIBorder", uiBorder);

            Button newGameButton = CreateButton("StaticSinglePlayerBtn", "OnOverSinglePlayerBtn", 50, 425, Scales.SevenTenth);
            Button multiPlayerButton = CreateButton("StaticMultiPlayerBtn", "OnOverMultiPlayerBtn", 50, 475, Scales.SevenTenth);
            Button loadGameButton = CreateButton("StaticLoadGameBtn", "OnOverLoadGameBtn", 50, 525, Scales.SevenTenth);
            Button propertiesButton = CreateButton("StaticPropertiesBtn", "OnOverPropertiesBtn", 50, 575, Scales.SevenTenth);
            Button exitButton = CreateButton("StaticExitBtn", "OnOverExitBtn", 50, 625, Scales.SevenTenth);

            menu.Objects.Add("NewGameBtn", newGameButton);
            menu.Objects.Add("MultiPlayerBtn", multiPlayerButton);
            menu.Objects.Add("LoadGameBtn", loadGameButton);
            menu.Objects.Add("PropertiesBtn", propertiesButton);
            menu.Objects.Add("ExitBtn", exitButton);
            //next code is only test
            BackGround ExitPopupBack = CreateBackground("UI/DialogPopup/Popup", Scales.ThreeTenth, 255f);
            Popup ExitPopup = CreatePopup(ExitPopupBack, Scales.None, 600, 500, false, true);
            Button ExitPopupBtnYes = CreateButton("StaticLeftButton", "OnOVerLeftButton", 87, 162, Scales.ThreeTenth);
            Button ExitPopupBtnNo = CreateButton("StaticRightButton", "OnOverRightButton", 177, 162, Scales.ThreeTenth);

            GameString exitString = CreateGameString("SpriteFont1", "Are you really want to exit?", 30f, 20f, Color.Black);
            ExitPopup.Objects.Add("ExitPopupBack", ExitPopupBack);
            ExitPopup.Objects.Add("ExitPopupBtnYes", ExitPopupBtnYes);
            ExitPopup.Objects.Add("ExitPopupBtnNo", ExitPopupBtnNo);
            ExitPopup.Objects.Add("ExitPopupString", exitString);
            menu.Objects.Add("ExitPopup", ExitPopup);

            //test popup
            BackGround testPopupBack = CreateBackground("UI/SoftPopup", Scales.ThreeTenth, 0f);
            Popup testPopup = CreatePopup(testPopupBack, Scales.None, 600f, 500f, false, true);
            GameString testString = CreateGameString("SpriteFont1", "What are you doing?Тест\nasd", 30f, 20f, Color.Azure);
            testPopup.Objects.Add("TestString", testString);
            menu.Objects.Add("TestPopup", testPopup);
            //end test popup

            screens.Add("Menu", menu);
            LoadEvents("Menu");
        }
        //------------------------------------------------------------
        //GENERATING SCREEN CREATE
        //------------------------------------------------------------
        private void CreateGen()
        {
            Screen Gen = new Screen();
            BackGround genBack = CreateBackground("SystemBacks/" +
                systemBackTextures[new Random().Next(0, systemBackTextures.Length)], Scales.None, 255f);
            ProgressBar progress = CreateProgressBar("FillingBar", 64, 950, Scales.TwoTenth);
            ProgressBar progressBorder = CreateProgressBar("ProgressBarBorder", 50, 940, Scales.TwoTenth);
            progressBorder.PWidth = (int)progressBorder.TextureWidth;
            GameString gs = CreateGameString("SpriteFont1", "Loading...", 50f, 870f, Color.Red);
            Gen.Objects.Add("GenBack", genBack);
            Gen.Objects.Add("ProgressBarBorder", progressBorder);
            Gen.Objects.Add("ProgressBar", progress);
            Gen.Objects.Add("String", gs);
            screens.Add("Gen", Gen);
        }
        //------------------------------------------------------------
        //CHOOSE RACE SCREEN
        //------------------------------------------------------------
        private void CreateChooseRace()
        {
            Screen choose = new Screen();
            BackGround chooseBack = CreateBackground("Textures/CreationScreenRace", Scales.None, 255f);
            Button start = CreateStrButton("blue", 1010, 500, 0.7f, locals.Strings["Start"]);
            choose.Objects.Add("Back", chooseBack);
            choose.Objects.Add("StarBtn", start);

            Button save = CreateStrButton("blue", 1010, 550, Scales.SevenTenth, locals.Strings["Save"]);
            Button load = CreateStrButton("blue", 1010, 600, Scales.SevenTenth, locals.Strings["Load"]);

            choose.Objects.Add("SaveBtn", save);
            choose.Objects.Add("LoadBtn", load);

            TextBox RaceName = CreateTextBox("Square", "SpriteFont1", 195, 810, Scales.ThreeWithHalfTenth);
            choose.Objects.Add("RaceName", RaceName);
            RaceName.Content = "Pidory";
            //Create properties buttons

            Button DamagePlus = CreateButton("Plus", "OnOverPlus", 930, 60, Scales.None);
            Button DefencePlus = CreateButton("Plus", "OnOverPlus", 930, 100, Scales.None);
            Button SpeedPlus = CreateButton("Plus", "OnOverPlus", 930, 140, Scales.None);
            Button SciencePlus = CreateButton("Plus", "OnOverPlus", 930, 180, Scales.None);
            Button ProductPlus = CreateButton("Plus", "OnOverPlus", 930, 220, Scales.None);
            Button DamageMinus = CreateButton("Minus", "OnOverMinus", 750, 60, Scales.None);
            Button DefenceMinus = CreateButton("Minus", "OnOverMinus", 750, 100, Scales.None);
            Button SpeedMinus = CreateButton("Minus", "OnOverMinus", 750, 140, Scales.None);
            Button ScienceMinus = CreateButton("Minus", "OnOverMinus", 750, 180, Scales.None);
            Button ProductMinus = CreateButton("Minus", "OnOverMinus", 750, 220, Scales.None);

            choose.Objects.Add("DamagePlusBtn", DamagePlus);
            choose.Objects.Add("DefencePlusBtn", DefencePlus);
            choose.Objects.Add("SpeedPlusBtn", SpeedPlus);
            choose.Objects.Add("SciencePlusBtn", SciencePlus);
            choose.Objects.Add("ProductPlusBtn", ProductPlus);
            choose.Objects.Add("DamageMinusBtn", DamageMinus);
            choose.Objects.Add("DefenceMinusBtn", DefenceMinus);
            choose.Objects.Add("SpeedMinusBtn", SpeedMinus);
            choose.Objects.Add("ScienceMinusBtn", ScienceMinus);
            choose.Objects.Add("ProductMinusBtn", ProductMinus);

            //End creating and adding properies buttons
            //Creating properties display values

            string ddots = ":";

            GameString DamageString = CreateGameString("SpriteFont1", "5", 850, 65, Color.Azure);
            GameString DefenceString = CreateGameString("SpriteFont1", "5", 850, 105, Color.Azure);
            GameString SpeedString = CreateGameString("SpriteFont1", "5", 850, 145, Color.Azure);
            GameString ScienceString = CreateGameString("SpriteFont1", "5", 850, 185, Color.Azure);
            GameString ProductString = CreateGameString("SpriteFont1", "5", 850, 225, Color.Azure);

            GameString DamageStringDesc = CreateGameString("SpriteFont1", locals.Strings["Damage"]+ddots, 615, 65, Color.Azure);
            GameString DefenceStringDesc = CreateGameString("SpriteFont1", locals.Strings["Defence"]+ddots, 615, 105, Color.Azure);
            GameString SpeedStringDesc = CreateGameString("SpriteFont1", locals.Strings["Speed"] + ddots, 615, 145, Color.Azure);
            GameString ScienceStringDesc = CreateGameString("SpriteFont1", locals.Strings["Science"] + ddots, 615, 185, Color.Azure);
            GameString ProductStringDesc = CreateGameString("SpriteFont1", locals.Strings["Product"] + ddots, 615, 225, Color.Azure);

            choose.Objects.Add("DamageStringDesc", DamageStringDesc);
            choose.Objects.Add("DefenceStringDesc", DefenceStringDesc);
            choose.Objects.Add("SpeedStringDesc", SpeedStringDesc);
            choose.Objects.Add("ScienceStringDesc", ScienceStringDesc);
            choose.Objects.Add("ProductStringDesc", ProductStringDesc);

            choose.Objects.Add("DamageString", DamageString);
            choose.Objects.Add("DefenceString", DefenceString);
            choose.Objects.Add("SpeedString", SpeedString);
            choose.Objects.Add("ScienceString", ScienceString);
            choose.Objects.Add("ProductString", ProductString);
            //End creating and adding strings
            //Create other strings

            GameString PointsString = CreateGameString("SpriteFont1", locals.Strings["Points"]+": ", 840, 270, Color.Azure);
            choose.Objects.Add("PointsString", PointsString);
            GameString PointsString2 = CreateGameString("SpriteFont1", "0", 940, 270, Color.Azure);
            choose.Objects.Add("PointsString2", PointsString2);

            GameString RaceString = CreateGameString("SpriteFont1", locals.Strings["RaceName"] + ddots, 45, 815, Color.Azure);
            choose.Objects.Add("RaceString", RaceString);

            //End creating oither strings
            screens.Add("Choose", choose);
        }
        //------------------------------------------------------------
        //GALAXY MAP CREATE
        //------------------------------------------------------------
        private void GalaxyMap(Map map)
        {
            Screen galaxyMap = new Screen();
            BackGround spaceBack = CreateBackground("Textures/GlobalBack", Scales.None, 255f, false);
            galaxyMap.Objects.Add("Back", spaceBack);
            galaxyMap.Objects.Add("Map", map);
            //info popup
            BackGround infoPopupBack = CreateBackground("UI/SoftPopup", Scales.Quarter, 255f);
            Popup infoPopup = CreatePopup(infoPopupBack, Scales.None, 600f, 500f, false, false);
            GameString infoString = CreateGameString("SpriteFont1", "What are you doing?", 30f, 20f, Color.Azure);
            infoPopup.Objects.Add("InfoPopupBack", infoPopupBack);
            infoPopup.Objects.Add("InfoString", infoString);
            galaxyMap.Objects.Add("InfoPopup", infoPopup);
            //end info popup
            galaxyMap.Objects.Add("EscPopup", CreateEscMenu());
            CreateHUD(galaxyMap);

            CreateDebugInfoWindow(galaxyMap);

            screens.Add("GalaxyMap", galaxyMap);
        }
        //------------------------------------------------------------
        //SOLAR SYSTEM CREATE
        //------------------------------------------------------------
        private void SolarSystem(SolarSystem ss)
        {
            if (!screens.ContainsKey("SolarSystem"))
            {
                Screen SolarSystemScreen = new Screen();
                BackGround solarSystemBack = CreateBackground("Temp/SolarBackTest", Scales.None, 255f, false);
                SolarSystemScreen.Objects.Add("Back", solarSystemBack);

                //info popup
                BackGround infoPopupBack = CreateBackground("UI/SoftPopup", Scales.Quarter, 255f);
                Popup infoPopup = CreatePopup(infoPopupBack, Scales.None, 600, 500);
                GameString infoString = CreateGameString("SpriteFont1", "", 30, 20, Color.Azure);
                infoPopup.Objects.Add("InfoString", infoString);
                //end info popup

                TextBox nameTextBox = CreateTextBox("Square", "SpriteFont1", 332, 40, Scales.ThreeWithHalfTenth);

                Popup moreInfoPopup = CreateInfoPopup();
                //for buttons position
                BackGround moreInfoPopupBack = (BackGround)moreInfoPopup.Objects["Back"];

                GameString name = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                                "Name", Shift(262, 40), Color.Azure);
                ((GameString)nameTextBox.ContentSource).Position = Shift(new Vector2(moreInfoPopup.Position.X + nameTextBox.Position.X + 10,
                    moreInfoPopup.Position.Y + nameTextBox.Position.Y + 5));

                BackGround PlanetStateFrame = CreateIcon("PlanetState/Frame", Scales.SevenTenth, 255f, new Vector2(100, 80));

                Button colonize = CreateStrButton("blue", 654, 487, Scales.SevenTenth, locals.Strings["Colonize"]);
                Button terraform = CreateStrButton("blue", 400, 487, Scales.SevenTenth, locals.Strings["Terraform"]);
                Button stationBuilder = CreateStrButton("blue", 400, 487, Scales.SevenTenth, locals.Strings["StationBuilder"]);
                Button createShip = CreateStrButton("blue", 654, 487, Scales.SevenTenth, locals.Strings["CreateShip"]);


                ProgressBar progress = CreateProgressBar("FillingBar", 262, 380, Scales.TwoTenth);//new ProgressBar(Content.Load<Texture2D>("UI/FillingBar"), /*HERE*/Shift(262, 380), /*HERE*/Resize(/*0.2f*/Scales.TwoTenth.Value));
                moreInfoPopup.Objects.Add("Progress", progress);

                moreInfoPopup.Objects.Add("NameString", name);

                moreInfoPopup.Objects.Add("PlanetStateFrame", PlanetStateFrame);

                moreInfoPopup.Objects.Add("NameTextBox", nameTextBox);
                moreInfoPopup.Objects.Add("ColonizeBtn", colonize);
                moreInfoPopup.Objects.Add("TerraformBtn", terraform);
                moreInfoPopup.Objects.Add("CreateShipBtn", createShip);
                moreInfoPopup.Objects.Add("StationBuilderBtn", stationBuilder);
                //end more info popup

                SolarSystemScreen.Objects.Add("SolarSystem", ss);
                SolarSystemScreen.Objects.Add("InfoPopup", infoPopup);
                CreateHUD(SolarSystemScreen);
                SolarSystemScreen.Objects.Add("MoreInfoPopup", moreInfoPopup);
                SolarSystemScreen.Objects.Add("EscPopup", CreateEscMenu());

                CreateDebugInfoWindow(SolarSystemScreen);

                screens.Add("SolarSystem", SolarSystemScreen);
            }
            else
            {
                screens["SolarSystem"].Objects["SolarSystem"] = ss;
            }
        }
        //------------------------------------------------------------
        //BLACKHOLE SYSTEM CREATE
        //------------------------------------------------------------
        private void CreateBlackHoleSystem(BlackHoleSystem bhs)
        {
            if (!screens.ContainsKey("BlackHoleSystem"))
            {
                Screen BlackHoleSystemScreen = new Screen();
                BackGround BlackHoleSystemBack = CreateBackground("Temp/SolarBackTest", Scales.None, 255f, false);
                BlackHoleSystemScreen.Objects.Add("Back", BlackHoleSystemBack);

                //info popup
                BackGround infoPopupBack = CreateBackground("UI/SoftPopup", Scales.Quarter, 255f);
                Popup infoPopup = CreatePopup(infoPopupBack, Scales.None, 400, 400);
                GameString infoString = CreateGameString("SpriteFont1", "", 30, 20, Color.Azure);
                infoPopup.Objects.Add("InfoString", infoString);
                //end info popup

                //more info popup
                TextBox nameTextBox = CreateTextBox("Square", "SpriteFont1", 332, 40, Scales.ThreeWithHalfTenth);

                Popup moreInfoPopup = CreateInfoPopup();
                //for buttons position
                BackGround moreInfoPopupBack = (BackGround)moreInfoPopup.Objects["Back"];

                GameString name = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                                "Name", Shift(30, 20), Color.Azure);
                ((GameString)nameTextBox.ContentSource).Position = new Vector2(moreInfoPopup.Position.X + nameTextBox.Position.X + 10,
                    moreInfoPopup.Position.Y + nameTextBox.Position.Y + 5);

                GameString PlanetSizeString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Size", Shift(30, 50), Color.Azure);
                GameString MinTemperatureString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Minimum Temperature", Shift(30, 80), Color.Azure);
                GameString MaxTemperatureString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Maximum Temperature", Shift(30, 110), Color.Azure);
                GameString IsAborigensString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Life", Shift(30, 140), Color.Azure);
                GameString MassString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Mass", Shift(30, 170), Color.Azure);
                GameString OwnerString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Owner", Shift(30, 200), Color.Azure);
                GameString RaceString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Race", Shift(30, 230), Color.Azure);
                GameString GravityString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Gravity", Shift(30, 260), Color.Azure);
                GameString ClimatString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Climat", Shift(30, 290), Color.Azure);
                GameString StabilityString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Stability", Shift(30, 320), Color.Azure);
                GameString FertilityString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Fertility", Shift(30, 350), Color.Azure);
                GameString RadioactivityString = new GameString(Content.Load<SpriteFont>("Fonts/SpriteFont1"),
                    "Radioactivity", Shift(30, 380), Color.Azure);

                moreInfoPopup.Objects.Add("NameString", name);

                moreInfoPopup.Objects.Add("PlanetSizeString", PlanetSizeString);
                moreInfoPopup.Objects.Add("MinTemperatureString", MinTemperatureString);
                moreInfoPopup.Objects.Add("MaxTemperatureString", MaxTemperatureString);
                moreInfoPopup.Objects.Add("IsAborigensString", IsAborigensString);
                moreInfoPopup.Objects.Add("MassString", MassString);
                moreInfoPopup.Objects.Add("OwnerString", OwnerString);
                moreInfoPopup.Objects.Add("RaceString", RaceString);
                moreInfoPopup.Objects.Add("GravityString", GravityString);
                moreInfoPopup.Objects.Add("ClimatString", ClimatString);
                moreInfoPopup.Objects.Add("StabilityString", StabilityString);
                moreInfoPopup.Objects.Add("FertilityString", FertilityString);
                moreInfoPopup.Objects.Add("RadioactivityString", RadioactivityString);

                moreInfoPopup.Objects.Add("NameTextBox", nameTextBox);
                //end more info popup

                BlackHoleSystemScreen.Objects.Add("BlackHoleSystem", bhs);
                BlackHoleSystemScreen.Objects.Add("InfoPopup", infoPopup);
                BlackHoleSystemScreen.Objects.Add("MoreInfoPopup", moreInfoPopup);
                BlackHoleSystemScreen.Objects.Add("EscPopup", CreateEscMenu());
                CreateHUD(BlackHoleSystemScreen);
                screens.Add("BlackHoleSystem", BlackHoleSystemScreen);
            }
            else
            {
                screens["BlackHoleSystem"].Objects["BlackHoleSystem"] = bhs;
            }
        }
        //------------------------------------------------------------
        //ESC MENU CREATE
        //------------------------------------------------------------
        private Popup CreateEscMenu()
        {
            BackGround back = CreateBackground("UI/EscMenu", Scales.SevenTenth, 255f);
            Popup escPopup = CreatePopup(back, Scales.None, 500, 400, false, false);

            Button continueBtn = CreateStrButton("grey", 35, 160, Scales.SevenTenth, locals.Strings["Continue"]);
            Button saveBtn = CreateStrButton("grey", 35, 195, Scales.SevenTenth, locals.Strings["Save"]);
            Button loadBtn = CreateStrButton("grey", 35, 230, Scales.SevenTenth, locals.Strings["Load"]);
            Button optionsBtn = CreateStrButton("grey", 35, 265, Scales.SevenTenth, locals.Strings["Options"]);
            Button exitBtn = CreateStrButton("grey", 35, 300, Scales.SevenTenth, locals.Strings["Exit"]);

            escPopup.Objects.Add("ContinueBtn", continueBtn);
            escPopup.Objects.Add("SaveBtn", saveBtn);
            escPopup.Objects.Add("LoadBtn", loadBtn);
            escPopup.Objects.Add("OptionsBtn", optionsBtn);
            escPopup.Objects.Add("ExitBtn", exitBtn);
            return escPopup;
        }
        //------------------------------------------------------------
        //HUD CREATE
        //------------------------------------------------------------
        private void CreateHUD(Screen screen)
        { 
            //Upper Panel
            BackGround UpperPanelBack = CreateBackground("UI/HUD/UpperPanel", Scales.None, 255f);
            Popup UpperPanel = CreatePopup(UpperPanelBack, Scales.None, 320f, 0f);
            Button infoBtn = CreateButton("InfoRace", "OnOverInfoRace", 50, 10, Scales.None);
            Button scienceBtn = CreateButton("Science", "OnOverScience", 150, 10, Scales.None);
            Button ownBtn = CreateButton("Own", "OnOverOwn", 250, 10, Scales.None);
            Button diplomacyBtn = CreateButton("Diplomacy", "OnOverDiplomacy", 350, 10, Scales.None);
            Button blueprintsBtn = CreateButton("Drawings", "OnOverDrawings", 450, 10, Scales.None);
            Button menuBtn = CreateButton("Menu", "OnOverMenu", 550, 10, Scales.None);

            UpperPanel.Objects.Add("InfoBtn", infoBtn);
            UpperPanel.Objects.Add("ScienceBtn", scienceBtn);
            UpperPanel.Objects.Add("OwnBtn", ownBtn);
            UpperPanel.Objects.Add("DiplomacyBtn", diplomacyBtn);
            UpperPanel.Objects.Add("BlueprintsBtn", blueprintsBtn);
            UpperPanel.Objects.Add("MenuBtn", menuBtn);
            UpperPanel.IsVisible = true;
            
            //Resources(Lower) Panel
            BackGround ResourcesPanelBack = CreateBackground("UI/HUD/LowerPanel", Scales.None, 255f);
            Popup ResourcesPanel = CreatePopup(ResourcesPanelBack, Scales.None, 640, 994);

            ResourcesPanel.Objects.Add("MoneyIcon", CreateIcon("Icons/money", Scales.SixTenth, 255f, new Vector2(20, 7)));
            ResourcesPanel.Objects.Add("EnergyIcon", CreateIcon("Icons/energy", Scales.SixTenth, 255f, new Vector2(250, 7)));
            ResourcesPanel.Objects.Add("MaterialIcon", CreateIcon("Icons/material", Scales.SixTenth, 255f, new Vector2(480, 7)));
            ResourcesPanel.Objects.Add("MoneyStr", CreateGameString("SpriteFont1", "12", 60, 5, Color.Azure));
            ResourcesPanel.Objects.Add("EnergyStr", CreateGameString("SpriteFont1", "12", 290, 5, Color.Azure));
            ResourcesPanel.Objects.Add("MaterialStr", CreateGameString("SpriteFont1", "12", 520, 5, Color.Azure));

            ResourcesPanel.IsVisible = true;

            //Chat Window
            BackGround ChatBack = CreateBackground("UI/HUD/ChatWindow", Scales.SevenTenth, 255f);
            Popup ChatWindow = CreatePopup(ChatBack, Scales.None, 0, (float)graphics.PreferredBackBufferHeight - ChatBack.Height);
            ChatWindow.IsVisible = true;

            //Info Popup
            Popup PlayerInfoPopup = CreateInfoPopup();
            //So many gamestring just only for easier tabling
            GameString player = CreateGameString("SpriteFont1", "Player: ", 20, 40, Color.Azure);
            PlayerInfoPopup.Objects.Add("Player", player);
            GameString playerName = CreateGameString("SpriteFont1", "a", 150, 40, Color.Azure);
            PlayerInfoPopup.Objects.Add("PlayerName", playerName);
            GameString race = CreateGameString("SpriteFont1", "Race: ", 20, 70, Color.Azure);
            PlayerInfoPopup.Objects.Add("Race", race);
            GameString raceName = CreateGameString("SpriteFont1", "a", 150, 70, Color.Azure);
            PlayerInfoPopup.Objects.Add("RaceName", raceName);
            //race charackteristics
            GameString damageString = CreateGameString("SpriteFont1", "Damage: ", 20, 100, Color.Azure);
            PlayerInfoPopup.Objects.Add("Damage", damageString);
            GameString damageValue = CreateGameString("SpriteFont1", "", 150, 100, Color.Azure);
            PlayerInfoPopup.Objects.Add("DamageString", damageValue);

            GameString defenceString = CreateGameString("SpriteFont1", "Defence: ", 20, 130, Color.Azure);
            PlayerInfoPopup.Objects.Add("Defence", defenceString);
            GameString defenceValue = CreateGameString("SpriteFont1", "", 150, 130, Color.Azure);
            PlayerInfoPopup.Objects.Add("DefenceString", defenceValue);

            GameString speedString = CreateGameString("SpriteFont1", "Speed: ", 20, 160, Color.Azure);
            PlayerInfoPopup.Objects.Add("Speed", speedString);
            GameString speedValue = CreateGameString("SpriteFont1", "", 150, 160, Color.Azure);
            PlayerInfoPopup.Objects.Add("SpeedString", speedValue);

            GameString scienceString = CreateGameString("SpriteFont1", "Science: ", 20, 190, Color.Azure);
            PlayerInfoPopup.Objects.Add("Science", scienceString);
            GameString scienceValue = CreateGameString("SpriteFont1", "", 150, 190, Color.Azure);
            PlayerInfoPopup.Objects.Add("ScienceString", scienceValue);

            GameString productString = CreateGameString("SpriteFont1", "Product: ", 20, 220, Color.Azure);
            PlayerInfoPopup.Objects.Add("Product", productString);
            GameString productValue = CreateGameString("SpriteFont1", "", 150, 220, Color.Azure);
            PlayerInfoPopup.Objects.Add("ProductString", productValue);

            //Adding
            screen.Objects.Add("UpperPanel", UpperPanel);
            screen.Objects.Add("ResourcesPanel", ResourcesPanel);
            //screen.Objects.Add("ChatWindow", ChatWindow);
            screen.Objects.Add("PlayerInfoPopup", PlayerInfoPopup);
        }

        private Popup CreateInfoPopup()
        {
            BackGround Back = CreateBackground("UI/InfoAll", 0.8f, 255f);
            Popup InfoPopup = CreatePopup(Back, Scales.None, 178, 238.5f);
            Button cross = CreateButton("Cross", "OnOverCross", 885, 7, Scales.None);
            InfoPopup.Objects.Add("CrossBtn", cross);
            return InfoPopup;
        }

        private void CreateDebugInfoWindow(Screen screen)
        {
            GameString gs = CreateGameString("SpriteFont1", "", 20, 20, Color.Red);
            screen.Objects.Add("DebugString", gs);
        }
        #endregion
    }
}
