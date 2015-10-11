using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using icsimplelib;
using Microsoft.Xna.Framework.Input;

namespace AlphaQuadrant
{
    class LittleStarOnMap: IDraw, IMoveble, IScaleble
    {
        #region Fields
        private Vector2 position;
        private Texture2D texture;
        private Vector2 scale;
        #endregion

        #region Properties
        public float X
        {
            get { return position.X; }
            set
            {
                position.X = value;
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
            }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set
            {
                scale = value;
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set 
            {
                position = value;
            }
        }
        public float Width
        {
            get { return texture.Width * scale.X; }
        }
        public float Height
        {
            get { return texture.Height * scale.Y; }
        }

        public bool IsVisible { get; set; }
        #endregion

        #region Constructors
        public LittleStarOnMap(Texture2D texture, Vector2 position, Vector2 scale)
        {
            this.texture = texture;
            this.position = position;
            this.scale = scale;
            IsVisible = true;
        }
        public LittleStarOnMap(Texture2D texture, Vector2 position) 
            : this(texture, position, new Vector2(1, 1)) { }
        #endregion

        #region Else
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
        }
        #endregion
    }
}
