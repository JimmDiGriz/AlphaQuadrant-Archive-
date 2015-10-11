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
        //---------------------------------------------------------------
        //START GAME BUTTON CLICK
        //---------------------------------------------------------------
        private void StartGameBtnClick(object sender, MouseState ms)
        {
            if (((TextBox)screens["Choose"].Objects["RaceName"]).Content == "")
            {
                return;
            }
            UnloadEvents("Choose");
            int damage, defence, speed, science, product;
            try
            {
                damage = Int32.Parse(((GameString)screens["Choose"].Objects["DamageString"]).Str);
                defence = Int32.Parse(((GameString)screens["Choose"].Objects["DefenceString"]).Str);
                speed = Int32.Parse(((GameString)screens["Choose"].Objects["SpeedString"]).Str);
                science = Int32.Parse(((GameString)screens["Choose"].Objects["ScienceString"]).Str);
                product = Int32.Parse(((GameString)screens["Choose"].Objects["ProductString"]).Str);
            }
            catch
            {
                return;
            }
            Race race = new Race(((TextBox)screens["Choose"].Objects["RaceName"]).Content,
                damage, defence, speed, science, product);
            Player player = new Player(ConstructPlayerName(), race);
            //Создание фабрики кораблей для данного игрока.
            ShipCreator = new ShipFactory(player, Content, scaleX, scaleY);
            StationCreator = new StationFactory(player, Content, scaleX, scaleY);

            currentPlayer = player.Name;
            players.Add(player.Name, player);
            CreateGen();
            currentScreen = "Gen";
            Thread t = new Thread(WorldGenerator);
            t.Start();
        }

        //---------------------------------------------------------------
        //SAVE RACE BUTTON CLICK
        //---------------------------------------------------------------
        private void SaveRaceBtnClick(object sender, MouseState ms)
        { 
            
        }

        //---------------------------------------------------------------
        //LOAD RACE BUTTON CLICK
        //---------------------------------------------------------------
        private void LoadRaceBtnClick(object sender, MouseState ms)
        { 
            
        }

        #region Params Button Events
        //---------------------------------------------------------------
        //DAMAGE BUTTON PLUS CLICK
        //---------------------------------------------------------------
        private void DamageButtonPlusClick(object sender, MouseState ms)
        {
            Plus("Damage");
        }
        //---------------------------------------------------------------
        //DEFENCE BUTTON PLUS CLICK
        //---------------------------------------------------------------
        private void DefenceButtonPlusClick(object sender, MouseState ms)
        {
            Plus("Defence");
        }
        //---------------------------------------------------------------
        //SPEED BUTTON PLUS CLICK
        //---------------------------------------------------------------
        private void SpeedButtonPlusClick(object sender, MouseState ms)
        {
            Plus("Speed");
        }
        //---------------------------------------------------------------
        //SCIENCE BUTTON PLUS CLICK
        //---------------------------------------------------------------
        private void ScienceButtonPlusClick(object sender, MouseState ms)
        {
            Plus("Science");
        }
        //---------------------------------------------------------------
        //PRODUCT BUTTON PLUS CLICK
        //---------------------------------------------------------------
        private void ProductButtonPlusClick(object sender, MouseState ms)
        {
            Plus("Product");
        }
        //---------------------------------------------------------------
        //DAMAGE BUTTON MINUS CLICK
        //---------------------------------------------------------------
        private void DamageButtonMinusClick(object sender, MouseState ms)
        {
            Minus("Damage");
        }
        //---------------------------------------------------------------
        //DEFENCE BUTTON MINUS CLICK
        //---------------------------------------------------------------
        private void DefenceButtonMinusClick(object sender, MouseState ms)
        {
            Minus("Defence");
        }
        //---------------------------------------------------------------
        //SPEED BUTTON MINUS CLICK
        //---------------------------------------------------------------
        private void SpeedButtonMinusClick(object sender, MouseState ms)
        {
            Minus("Speed");
        }
        //---------------------------------------------------------------
        //SCIENCE BUTTON MINUS CLICK
        //---------------------------------------------------------------
        private void ScienceButtonMinusClick(object sender, MouseState ms)
        {
            Minus("Science");
        }
        //---------------------------------------------------------------
        //PRODUCT BUTTON MINUS CLICK
        //---------------------------------------------------------------
        private void ProductButtonMinusClick(object sender, MouseState ms)
        {
            Minus("Product");
        }
        #endregion

        #region Helpers
        private void Plus(string param)
        {
            int points, value;
            try
            {
                points = Int32.Parse(((GameString)screens["Choose"].Objects["PointsString2"]).Str);
                value = Int32.Parse(((GameString)screens["Choose"].Objects[param + "String"]).Str);
            }
            catch
            {
                return;
            }
            if (points != 0)
            {
                ((GameString)screens["Choose"].Objects[param+"String"]).Str = (value + 1).ToString();
                ((GameString)screens["Choose"].Objects["PointsString2"]).Str = (points - 1).ToString();
            }
        }
        private void Minus(string param)
        {
            int points, value;
            try
            {
                points = Int32.Parse(((GameString)screens["Choose"].Objects["PointsString2"]).Str);
                value = Int32.Parse(((GameString)screens["Choose"].Objects[param + "String"]).Str);
            }
            catch
            {
                return;
            }
            if (value != 0)
            {
                ((GameString)screens["Choose"].Objects[param + "String"]).Str = (value - 1).ToString();
                ((GameString)screens["Choose"].Objects["PointsString2"]).Str = (points + 1).ToString();
            }
        }
        #endregion
    }
}
