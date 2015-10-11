using System;
using System.IO;
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

namespace AlphaQuadrant
{
    public partial class AlphaQuadrant : Microsoft.Xna.Framework.Game
    {

        public AlphaQuadrant()
        {
            graphics = new GraphicsDeviceManager(this);
            preferences = new Dictionary<string, string>();

            LoadPreferences();
            SetPreferences();

            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);

            scaleX = stepX = (float)graphics.PreferredBackBufferWidth / 1280f;
            scaleY = stepY = (float)graphics.PreferredBackBufferHeight / 1024f;
            IsMouseVisible = true;

            Content.RootDirectory = "Content";
            currentScreen = "Menu";//Start screen
            screens = new Dictionary<string, Screen>();
            players = new Dictionary<string, Player>();
            originalMs = Mouse.GetState();

            Song tempSong = Content.Load<Song>("Sound/Fly Theme");

            SystemSize = new Vector2(2500);
            CenterPoint = SystemSize / 2;
            CenterPoint -= new Vector2(250);

            SetWGInfoHelper();

            MediaPlayer.IsRepeating = true;
            try
            {
                MediaPlayer.Play(tempSong);
            }
            catch
            {
                return;
            }
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;
        }

        private void LoadPreferences()
        {
            try
            {
                using (StreamReader sr = new StreamReader("cfg.ini"))
                {
                    string line;

                    /*if (sr.ReadToEnd() == "")
                    {
                        throw new Exception("File is empty!");
                    }*/

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] param = line.Split('=');
                        preferences.Add(param[0], param[1]);
                    }
                }
            }
            catch
            {
                FileInfo fi = new FileInfo("cfg.ini");

                if (fi.Exists)
                {
                    fi.Delete();
                }
                fi.Create().Close();

                using (StreamWriter sw = fi.CreateText())
                {
                    //Default config
                    sw.WriteLine("local=eng");
                    sw.WriteLine("height={0}", GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height);
                    sw.WriteLine("width={0}", GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
                    sw.WriteLine("fullscreen=false");
                    sw.WriteLine("music=7");
                    sw.WriteLine("effects=10");
                }
                LoadPreferences();
            }
        }

        private void SetPreferences()
        {
            try
            {
                graphics.PreferredBackBufferWidth = /*1280;*//*1024;*/800;//Convert.ToInt32(preferences["width"]);
                graphics.PreferredBackBufferHeight = /*1024;*//*768;*/600;//Convert.ToInt32(preferences["height"]);
            }
            catch
            {
                graphics.PreferredBackBufferWidth = /*1280;*//*1366;*/800;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.PreferredBackBufferHeight = /*1024;*//*768;*/600;//GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            }

            try
            {
                graphics.IsFullScreen = /*true;*/Convert.ToBoolean(preferences["fullscreen"]);
            }
            catch
            {
                graphics.IsFullScreen = /*true;*/false;
            }

            try
            {
                MediaPlayer.Volume = Convert.ToSingle(preferences["music"]) / 100;
            }
            catch
            {
                MediaPlayer.Volume = 0.07f;
            }

            try
            {
                locals = new Locals(preferences["local"]);
            }
            catch
            {
                locals = new Locals("eng");
            }
        }

        private void SetWGInfoHelper()
        {
            WGInfoHelper = new Dictionary<int, WGPlanetInfo>();

            WGInfoHelper.Add(0, new WGPlanetInfo(hotPlanetTextures, 600, 400, 0, 1, 100, 150));
            WGInfoHelper.Add(1, new WGPlanetInfo(hotPlanetTextures, 400, 100, 0, 1, 250, 300, 200));
            WGInfoHelper.Add(2, new WGPlanetInfo(mediumPlanetTextures, 50, -40, 4, 4, 400, 450));
            WGInfoHelper.Add(3, new WGPlanetInfo(mediumPlanetTextures, 20, -70, 3, 4, 550, 600));
            WGInfoHelper.Add(4, new WGPlanetInfo(coldPlanetTextures, -20, -100, 3, 3, 700, 750, -30));
            WGInfoHelper.Add(5, new WGPlanetInfo(coldPlanetTextures, -70, -150, 2, 3, 850, 900));
            WGInfoHelper.Add(6, new WGPlanetInfo(coldPlanetTextures, -150, -273, 0, 1, 1000, 1050, -200));
            WGInfoHelper.Add(7, new WGPlanetInfo(outsidersPlanetTextures, -274, -274, 0, 0, 1150, 1200));
            WGInfoHelper.Add(8, new WGPlanetInfo(outsidersPlanetTextures, -274, -274, 0, 0, 1300, 1350));
            WGInfoHelper.Add(9, new WGPlanetInfo(outsidersPlanetTextures, -273, -273, 0, 1, 1450, 1500));
        }
    }
}