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
    class StationFactory
    {
        #region Fields
        #endregion

        #region Properties
        private Player Player { get; set; }
        private ContentManager Content { get; set; }
        private float DX { get; set; }
        private float DY { get; set; }
        //private Texture2D Circle { get; set; }
        #endregion

        #region Construct
        public StationFactory(Player player, ContentManager content, float dX, float dY)
        {
            Player = player;
            Content = content;
            DX = dX;
            DY = dY;
        }
        #endregion

        #region Creators
        public StationOnBuilding GetStationOnBuilding(StationBuilder builder)
        { 
            Texture2D first = Content.Load<Texture2D>("Stations/StationOnBuildingStage1");
            Texture2D second = Content.Load<Texture2D>("Stations/StationOnBuildingStage2");
            Texture2D third = Content.Load<Texture2D>("Stations/StationOnBuildingStage3");
            Texture2D fourth = Content.Load<Texture2D>("Stations/StationOnBuildingStage4");

            Texture2D progressTexture = Content.Load<Texture2D>("UI/Lines/YellowLine");
            Vector2 progressPosition = new Vector2(builder.Position.X, builder.Position.Y - progressTexture.Height - 5);

            ProgressBar progress = new ProgressBar(progressTexture, progressPosition, new Vector2(Scales.None));

            StationOnBuilding temp = new StationOnBuilding(first, second, third, fourth, builder.Position, new Vector2(Scales.None), progress, 100);
            temp.Owner = "Enemy";//Player.Name;
            temp.Name = "Station On Build";
            temp.PositionFromCenter = builder.PositionFromCenter;

            return temp;
        }

        public Station GetEmptyStation(StationOnBuilding building)
        {
            Texture2D texture = Content.Load<Texture2D>("Stations/SpaceStation");

            Station temp = new Station(texture, building.Position, new Vector2(Scales.None), 500);
            temp.Owner = "Enemy";//Player.Name;
            temp.PositionFromCenter = building.PositionFromCenter;
            temp.Name = "Empty Station";

            return temp;
        }
        #endregion

        #region Helpers
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
