using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace icsimplelib
{
    [Serializable]
    public class Button : IDraw, IMoveble, IUpdateble
    {
        #region Fields
        private Texture2D texture;
        private Texture2D staticTexture;
        private Texture2D overTexture;
        private Texture2D pressTexture;
        private Vector2 position;
        private MouseState ms;
        private float finalWidth;
        private float finalHeight;
        private Vector2 scale;
        private bool isPressed = false;
        private bool isVisible = true;
        private SoundEffect click;
        private GameString str;
        //private Thread sound;
        //private bool focus; //need realization.
        #endregion
        
        #region Properties
        public float X
        {
            get { return position.X; }
            set
            {
                if (value > 0) //need check for screen width.
                {
                    position.X = value;
                    finalWidth = position.X + texture.Width*scale.X;
                }
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                if (value > 0)//need check for screen height.
                {
                    position.Y = value;
                    finalHeight = position.Y + texture.Height*scale.Y;
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
        public float Width
        {
            get { return texture.Width * scale.X; }
        }
        public float Height
        {
            get { return texture.Height * scale.Y; }
        }
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
            }
        }
        public GameString Str
        {
            get { return str; }
            set
            {
                str = value;
            }
        }
        #endregion

        #region Events
        public delegate void OnClickHandler(object sender, MouseState ms);
        public delegate void OnOverHandler(object sender, MouseState ms);
        public event OnClickHandler OnClick;
        public event OnOverHandler OnOver;
        #endregion
        
        #region Constructors
        public Button(Texture2D staticTexture, Texture2D overTexture, Texture2D pressTexture, Vector2 position, Vector2 scale, SoundEffect click = null, GameString gs = null)
        {
            this.texture = this.staticTexture = staticTexture;
            this.overTexture = overTexture;
            this.pressTexture = pressTexture;
            this.position = position;
            this.scale = scale;
            finalWidth = position.X + texture.Width*scale.X;
            finalHeight = position.Y + texture.Height*scale.Y;
            this.click = click;
            //sound = new Thread(() => { click.Play(); });
            str = gs;
            Vector2 measure = gs.Font.MeasureString(gs.Str);
            gs.Position = new Vector2(position.X + (Width/2) - (measure.X / 2), position.Y + (Height/2) - (measure.Y/2));
        }
        /*public Button(Texture2D staticTexture, Texture2D overTexture, Texture2D pressTexture, Vector2 position, Vector2 scale)
            : this(staticTexture, overTexture, pressTexture, position, scale, null) { }*/
        public Button(Texture2D staticTexture, Texture2D overTexture, Texture2D pressTexture, Vector2 position)
            : this(staticTexture, overTexture, pressTexture, position, new Vector2(1,1)) { }
        //public Button(Texture2D staticTexture, Texture2D overTexture, Texture2D pressTexture, int x)
            //: this(staticTexture, overTexture, pressTexture, x, 0) { }

        public Button(Texture2D staticTexture, Texture2D overTexture, Texture2D pressTexture)
            : this(staticTexture, overTexture, pressTexture, new Vector2(0,0)) { }

        public Button(Texture2D staticTexture, Texture2D overTexture)
            : this(staticTexture, overTexture, staticTexture) { }

        public Button(Texture2D staticTexture)
            : this(staticTexture, staticTexture) { }
        public Button(Texture2D staticTexture, Vector2 position, Vector2 scale)
        {
            this.texture = this.staticTexture = this.overTexture = this.pressTexture = staticTexture;
            this.position = position;
            this.scale = scale;
            finalWidth = position.X + texture.Width * scale.X;
            finalHeight = position.Y + texture.Height * scale.Y;
        }
        #endregion
        //in code need to generate mouse entering event.
        #region Else
        public void Update(GameTime gameTime)
        {
            if (isVisible)
            {
                ms = Mouse.GetState();
                if (ms.LeftButton == ButtonState.Released && isPressed == true )
                {
                    isPressed = false;
                    if (ms.X <= finalWidth && ms.Y <= finalHeight && ms.X > position.X && ms.Y > position.Y)
                    {
                        if (OnClick != null)
                        {
                            OnClick(this, ms);
                        }
                    }
                }
                if (ms.X <= finalWidth && ms.Y <= finalHeight && ms.X > position.X && ms.Y > position.Y)
                {
                    Over();
                }
                else
                    texture = staticTexture;
            }
        }

        private void Over()
        {
            texture = overTexture;
            if (OnOver != null)
                OnOver(this, ms);
            Click();
        }

        private void Click()
        {
            if (ms.LeftButton == ButtonState.Pressed && isPressed == false)
            {
                texture = pressTexture;
                isPressed = true;
                if (click != null)
                {
                    Thread t = new Thread(() => { click.Play(); });
                    t.Start();
                    //sound.Start();
                }
                /*if (OnClick != null)
                    OnClick(this, ms);*/
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                if (str != null)
                {
                    str.Draw(spriteBatch);
                }
            }
        }
        #endregion
    }
}
