using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace icsimplelib
{
    [Serializable]
    public class CheckBox : IDraw, IUpdateble
    {
        #region Fields
        private Texture2D texture;
        private Texture2D staticTexture;
        private Texture2D overTexture;
        private Texture2D flagTexture;
        private Vector2 position;
        private Vector2 scale;
        private MouseState ms;
        private float finalWidth;
        private float finalHeight;
        private bool isOn;
        #endregion

        #region Properties
        public float X
        {
            get { return position.X; }
            set
            {
                if (value > 0)
                {
                    position.X = value;
                    finalWidth = position.X + texture.Width * scale.X;
                }
            }
        }

        public float Y
        {
            get { return position.Y; }
            set 
            {
                if (value > 0)
                {
                    position.Y = value;
                    finalHeight = position.Y + texture.Height * scale.Y;
                }
            }
        }

        public bool IsOn
        {
            get { return isOn; }
            set { isOn = value; }
        }

        public bool IsVisible { get; set; }
        #endregion

        #region Events
        public delegate void OnClickHandler(object sender, MouseState ms);
        public event OnClickHandler OnClick;
        #endregion

        #region Constructors
        public CheckBox(Texture2D staticTexture, Texture2D overTexture, Texture2D flagTexture, Vector2 position, Vector2 scale, bool isOn)
        {
            this.texture = this.staticTexture = staticTexture;
            this.overTexture = overTexture;
            this.flagTexture = flagTexture;
            this.position = position;
            this.scale = scale;
            this.isOn = isOn;
            IsVisible = true;
        }
        public CheckBox(Texture2D staticTexture, Texture2D overTexture, Texture2D flagTexture, Vector2 position, Vector2 scale)
            : this(staticTexture, overTexture, flagTexture, position, scale, false) { }
        public CheckBox(Texture2D staticTexture, Texture2D overTexture, Texture2D flagTexture, Vector2 position)
            : this(staticTexture, overTexture, flagTexture, position, new Vector2(1, 1)) { }
        public CheckBox(Texture2D staticTexture, Texture2D overTexture, Texture2D flagTexture)
            : this(staticTexture, overTexture, flagTexture, Vector2.Zero) { }
        #endregion

        #region Else
        public void Update(GameTime gameTime)
        {
            ms = Mouse.GetState();
            ms = Mouse.GetState();
            if (ms.X <= finalWidth && ms.Y <= finalHeight && ms.X > position.X && ms.Y > position.Y)
            {
                texture = overTexture;
                Click();
            }
            else
                texture = staticTexture;
        }

        private void Click()
        {
            if (ms.LeftButton == ButtonState.Pressed)
            {
                isOn = !isOn;
                if (OnClick != null)
                    OnClick(this, ms);
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                if (isOn)
                    spriteBatch.Draw(flagTexture, position, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
            }
        }
        #endregion
    }
}
