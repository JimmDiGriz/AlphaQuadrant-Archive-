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
    public class Star : IDraw, IUpdateble, IMoveble, IScaleble, ICoordUpdateble
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
        public float CenterX
        {
            get { return FinalWidth - ((Texture.Width * Scale.X)/2); }
        }
        public float CenterY
        {
            get { return FinalHeight - ((Texture.Height * Scale.Y)/2); }
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

        public Vector2 Scale { get; set; }
        public string Name { get; set; }
        public string StarColor { get; set; }
        private Texture2D Texture { get; set; }
        private float FinalWidth { get; set; }
        private float FinalHeight { get; set; }
        private MouseState MS { get; set; }
        private bool IsPressed { get; set; }
        private float Rotation { get; set; }
        private float Radians { get; set; }
        private Vector2 Origin { get; set; }
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
        public Star(Texture2D texture, Vector2 position, Vector2 scale, string color)
        {
            Texture = texture;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.position = position;
            Scale = scale;
            //this.position = new Vector2(position.X - (origin.X * scale.X), position.Y - (origin.Y * scale.Y));
            FinalWidth = position.X + texture.Width * scale.X /*- (origin.X * scale.X)*/;
            FinalHeight = position.Y + texture.Height * scale.Y /*- (origin.Y * scale.Y)*/;
            StarColor = color;
            Rotation = 0;
            IsPressed = false;
            IsVisible = true;
        }
        public Star(Texture2D texture, Vector2 position, Vector2 scale)
            : this(texture, position, scale, "Blue") { }
        public Star(Texture2D texture, Vector2 position)
            : this(texture, position, new Vector2(1, 1)) { }
        public Star(Texture2D texture)
            : this(texture, new Vector2(0, 0)) { }
        #endregion

        #region Else
        public void Update(GameTime gameTime)
        {
            CoordsUpdate(gameTime);
            EventUpdate();
        }

        public void CoordsUpdate(GameTime gameTime)
        {
            //timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            //if (timer > 50)
            //{
                Rotation += 0.3f;
                //timer -= 50;
            //}
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
            Radians = Rotation / 10 * (float)Math.PI / 180;
            spriteBatch.Draw(Texture, 
                new Vector2(position.X + (Origin.X * Scale.X), position.Y + (Origin.Y * Scale.Y)),
                null, Color.White, Radians, Origin, Scale, 0, 0);
        }
        #endregion
    }
}
