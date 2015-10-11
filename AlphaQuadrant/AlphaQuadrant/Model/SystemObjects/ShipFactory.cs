using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using icsimplelib;

namespace AlphaQuadrant
{
    public class ShipFactory
    {
        #region Fields
        //private Player player;
        //private ContentManager content;
        //private float dX;
        //private float dY;
        //private Texture2D circle;
        #endregion

        #region Properties
        private Player Player { get; set; }
        private ContentManager Content { get; set; }
        private float DX { get; set; }
        private float DY { get; set; }
        private Texture2D Circle { get; set; }
        private Texture2D HPBar { get; set; }
        private Texture2D ShieldBar { get; set; }
        #endregion

        #region Construct
        public ShipFactory(Player player, ContentManager content, float dX, float dY)
        {
            Player = player;
            Content = content;
            DX = dX;
            DY = dY;
            Circle = Content.Load<Texture2D>("Ships/OnOverCircle");
            HPBar = Content.Load<Texture2D>("UI/Lines/GreenLine");
            ShieldBar = Content.Load<Texture2D>("UI/Lines/BlueLine");
        }
        #endregion

        #region Памятка
        /*
         * Скорость корабля - просто скорость корабля, она фиксированна в зависимости от чертежа.
         * Модификатор скорости - параметр расы, который оказывает дополнительное влияние на скорость корабля.
         */
        #endregion

        #region Creators
        public Ship GetWorkerShip(Planet planet)
        {
            List<Weapon> weapons = new List<Weapon>();
            weapons.Add(new Weapon(Content.Load<Texture2D>("Weapons/RedLaser"), 90f, 100f, 10, Vector2.Zero, 1000f));

            Ship temp = new Ship(Content.Load<Texture2D>("Ships/SpaceShipExperimentVersion"),
                            new Vector2(planet.X + planet.Width + 30, planet.Y + planet.Height + 30),
                            new Vector2(Scales.ThreeTenth), 3f, Player.Race.Speed, Player.Race.Defence, 1f, 100, 200, "Worker Ship", Player.Name,
                            Circle, weapons);
            temp.PositionFromCenter = planet.PositionFromCenter;
            return temp;
        }

        public StationBuilder GetStationBuilder(Planet planet)
        {
            List<Weapon> weapons = new List<Weapon>();

            StationBuilder temp = new StationBuilder(Content.Load<Texture2D>("Stations/StationBuilder"),
                            new Vector2(planet.X + planet.Width + 30, planet.Y + planet.Height + 30),
                            new Vector2(Scales.EightTenth), 0.1f, Player.Race.Speed, Player.Race.Defence, 1f, 1000, 2000, "Station Builder", Player.Name,
                            Circle, weapons);
            temp.PositionFromCenter = planet.PositionFromCenter;
            return temp;
        }

        public void GetShipFromBluePrint(/*BluePrint bluePrint*/)
        {

        }
        #endregion

        #region Helpers (SHIT)
        private Vector2 Shift(float x, float y)
        {
            return new Vector2(x * DX, y * DY);
        }

        private Vector2 Shift(Vector2 position)
        {
            return new Vector2(position.X * DX, position.Y * DY);
        }

        private Vector2 Shift(Vector2 position, Vector2 scale)
        {
            return new Vector2(position.X * DX * scale.X, position.Y * DY * scale.Y);
        }

        private float ShiftX(float x)
        {
            return x * DX;
        }

        private float ShiftY(float y)
        {
            return y * DY;
        }

        private Vector2 GalaxyShift(Vector2 position, Vector2 scale)
        {
            return new Vector2(position.X * 1 * scale.X, position.Y * 1 * scale.Y);
        }
        private Vector2 Resize(float startScale)
        {
            return new Vector2(startScale * DX, startScale * DY);
        }
        #endregion
    }
}