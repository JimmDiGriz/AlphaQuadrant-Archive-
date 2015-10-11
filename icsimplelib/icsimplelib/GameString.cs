using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace icsimplelib
{
    [Serializable]
    public class GameString : IDraw, IMoveble
    {
        #region Fields
        private SpriteFont font;
        private string str;
        private Vector2 position;
        private Color color;
        #endregion

        #region Properties
        public SpriteFont Font
        {
            get { return font; }
            set
            {
                if (value is SpriteFont)
                {
                    font = value;
                }
            }
        }

        public string Str
        {
            get { return str; }
            set
            {
                if (value is string)
                {
                    str = value;
                }
                else
                {
                    str = value.ToString();
                }
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
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
            }
        }
        public float Width
        {
            get { return 0; }
        }
        public float Height
        {
            get { return 0; }
        }

        public bool IsVisible { get; set; }
        #endregion

        #region Constructors
        public GameString(SpriteFont font, string str, Vector2 position, Color color)
        {
            this.str = str;
            this.font = font;
            this.position = position;
            this.color = color;
            IsVisible = true;
        }

        public GameString(SpriteFont font, string str, Vector2 position)
            : this(font, str, position, Color.Black) { }

        public GameString(SpriteFont font, string str)
            : this(font, str, Vector2.Zero) { }
        public GameString(SpriteFont font)
            : this(font, "") { }
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.DrawString(font, str, position, color);
            }
        }
    }
}
