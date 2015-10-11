using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace icsimplelib
{
    [Serializable]
    public class ProgressBar : IDraw, IMoveble
    {
        #region Fields
        private Texture2D texture;
        private Rectangle recntangle;
        private Vector2 position;
        private Vector2 scale;//end this.
        private int width;
        #endregion

        #region Properties
        public int PWidth
        {
            get { return width; }
            set
            {
                if (value >= 0 && value <= texture.Width)
                {
                    width = value;
                    recntangle = new Rectangle(0, 0, width, texture.Height);
                }
            }
        }

        public float Width
        {
            get { return texture.Width; }
        }

        public float Height
        {
            get { return texture.Height * scale.Y; }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }
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
        public float TextureWidth
        {
            get { return texture.Width; }
        }

        public bool IsVisible { get; set; }
        public Vector2 PositionFromCenter { get; set; }
        #endregion

        #region Constructors
        public ProgressBar(Texture2D texture, Vector2 position, Vector2 scale)
        {
            this.position = position;
            this.texture = texture;
            this.scale = scale;
            PWidth = 0;
            IsVisible = true;
        }
        public ProgressBar(Texture2D texture, Vector2 position)
            : this(texture, position, new Vector2(0, 0)) { }
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                //spriteBatch.Draw(texture, position, recntangle, Color.White);
                spriteBatch.Draw(texture, position, recntangle, Color.White, 0, Vector2.Zero, scale, 0, 0);
            }
        }
    }
}
