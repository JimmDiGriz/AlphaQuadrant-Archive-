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
    [Serializable]
    public class Ammo : IUpdateble, IDraw
    {
        #region Fields
        #endregion

        #region Properties
        private Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 TargetPosition { get; set; }
        private Vector2 Velocity { get; set; }
        private float Radians { get; set; }
        private float HitRange { get; set; }
        public bool IsVisible { get; set; }
        #endregion

        #region Events
        public delegate void OnHitHandler(Ammo sender);
        public event OnHitHandler OnHit;
        public delegate void OnGoingTooFarHandler(Ammo sender);
        public event OnGoingTooFarHandler OnGoingTooFar;
        #endregion

        #region Construct
        public Ammo(Texture2D texture, Vector2 position, Vector2 origin, Vector2 velocity, float speedMod, float radians, Vector2 targetPosition)
        {
            Texture = texture;
            Position = position;
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Velocity = velocity;
            HitRange = speedMod;
            Radians = radians;
            TargetPosition = targetPosition;
            IsVisible = true;
        }
        #endregion

        #region Else
        public void Update(GameTime gameTime)
        {
            CoordsUpdate(gameTime);
        }

        private void CoordsUpdate(GameTime gameTime)
        {
            Position += Velocity;
            if ((Position - TargetPosition).Length() <= HitRange)
            {
                if (OnHit != null)
                {
                    OnHit(this);
                }
            }
            if (Position.X > 5000 || Position.X < -5000 || Position.Y > 5000 || Position.Y < -5000)
            {
                if (OnGoingTooFar != null)
                {
                    OnGoingTooFar(this);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,
                   new Vector2(Position.X + Origin.X, Position.Y + Origin.Y),
                   null, Color.White, Radians, Origin, new Vector2(1f), 0, 0);
        }
        #endregion
    }
}
