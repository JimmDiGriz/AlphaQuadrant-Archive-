using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace icsimplelib
{
    [Serializable]
    public class BackGround : IDraw, IMoveble, IScaleble
    {
        #region Fields
        private Texture2D texture;
        private Vector2 position;
        private Vector2 scale;
        private float alpha;
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
        public BackGround(Texture2D texture, Vector2 scale, float alpha)
        {
            this.texture = texture;
            this.scale = scale;
            this.alpha = alpha;
            this.position = Vector2.Zero;
            IsVisible = true;
        }
        #endregion

        #region Else
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(texture, position, null, new Color(255, 255, 255, alpha), 0, Vector2.Zero, scale, 0, 0);
            }
        }
        #endregion
    }
}
