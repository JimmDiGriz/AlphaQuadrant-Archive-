using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using icsimplelib;
using System.Threading;
using System.Threading.Tasks;

namespace AlphaQuadrant
{
    [Serializable]
    public class BlackHoleOnMap: IDraw, IMoveble, IScaleble, IUpdateble
    {
        #region Fields
        private Vector2 position;
        private bool isOver = false;
        #endregion

        #region Properties
        public float X
        {
            get { return position.X; }
            set
            {
                position.X = value;
                FinalWidth = position.X + Texture.Width * Scale.X;
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                FinalHeight = position.Y + Texture.Height * Scale.Y;
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                FinalHeight = position.Y + Texture.Height * Scale.Y;
                FinalWidth = position.X + Texture.Width * Scale.X;
            }
        }

        private bool IsOver
        {
            get { return isOver; }
            set
            {
                if (value == false)
                {
                    if (OnOverEnd != null)
                    {
                        OnOverEnd();
                    }
                }
                isOver = value;
            }
        }
        public float Width
        {
            get { return Texture.Width * Scale.X; }
        }
        public float Height
        {
            get { return Texture.Height * Scale.Y; }
        }

        private Texture2D Texture { get; set; }
        private float FinalWidth { get; set; }
        private float FinalHeight { get; set; }
        public BlackHoleSystem System { get; set; }
        public Vector2 Scale { get; set; }
        public string Name { get; set; }
        private MouseState MS { get; set; }
        private bool IsPressed { get; set; }
        public bool IsVisible { get; set; }
        #endregion

        #region Events
        public delegate void OnClickHandler(object sender, MouseState ms);
        public delegate void OnOverHandler(object sender, MouseState ms);
        public delegate void OnOverEndHandler();
        public event OnClickHandler OnClick;
        public event OnOverHandler OnOver;
        public event OnOverEndHandler OnOverEnd;
        #endregion

        #region Constructors
        public BlackHoleOnMap(Texture2D texture, Vector2 position, Vector2 scale)
        {
            Texture = texture;
            this.position = position;
            Scale = scale;
            FinalWidth = position.X + texture.Width * scale.X;
            FinalHeight = position.Y + texture.Height * scale.Y;
            IsPressed = false;
            IsVisible = true;
        }
        public BlackHoleOnMap(Texture2D texture, Vector2 position) 
            : this(texture, position, new Vector2(1, 1)) { }
        #endregion

        #region Else
        public void Update(GameTime gameTime)
        {
            EventUpdate();
        }

        private void EventUpdate()
        {
            MS = Mouse.GetState();
            if (MS.LeftButton == ButtonState.Released && IsPressed == true)
            {
                IsPressed = false;
                if (MS.X <= FinalWidth && MS.Y <= FinalHeight && MS.X > position.X && MS.Y > position.Y)
                {
                    if (OnClick != null)
                    {
                        OnClick(this, MS);
                    }
                }
            }

            if (MS.X <= FinalWidth && MS.Y <= FinalHeight && MS.X > position.X && MS.Y > position.Y)
            {
                Task.Factory.StartNew(Over);
                Click();
            }
            else
            {
                if (IsOver == true)
                {
                    IsOver = false;
                }
            }
        }

        private void Over()
        {
            if (!IsOver)
            {
                Thread.Sleep(300);
                IsOver = true;
                //texture = overTexture;
                if (OnOver != null)
                {
                    OnOver(this, MS);
                }
            }
        }

        private void Click()
        {
            if (MS.LeftButton == ButtonState.Pressed && IsPressed == false)
            {
                //texture = pressTexture;
                IsPressed = true;
                /*if (click != null)
                {
                    Thread t = new Thread(() => { click.Play(); });
                    t.Start();
                    //sound.Start();
                }*/
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
        }
        #endregion
    }
}
