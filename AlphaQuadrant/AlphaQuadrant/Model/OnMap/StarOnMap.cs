using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using icsimplelib;
using Microsoft.Xna.Framework.Input;
using System.Threading.Tasks;
using System.Threading;

namespace AlphaQuadrant
{
    [Serializable]
    public class StarOnMap: IDraw, IMoveble, IScaleble, IUpdateble, ICoordUpdateble
    {
        #region GamePlay Fields
        #endregion

        #region Fields
        private Vector2 position;
        private bool isOver = false;
        #endregion

        #region Properties
        private Texture2D Texture { get; set; }
        public Vector2 Scale { get; set; }
        private float FinalWidth { get; set; }
        private float FinalHeight { get; set; }
        private MouseState MS { get; set; }
        private bool IsPressed { get; set; }
        public SolarSystem SS { get; set; }
        public Texture2D Circle { get; set; }
        public bool IsVisited { get; set; }

        public string Name { get; set; }
        public int CountOfPlanets { get; set; }
        public string Owner { get; set; }
        public bool IsVisible { get; set; }

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
        public StarOnMap(Texture2D texture, Vector2 position, Vector2 scale)
        {
            Texture = texture;
            this.position = position;
            Scale = scale;
            FinalWidth = position.X + texture.Width * scale.X;
            FinalHeight = position.Y + texture.Height * scale.Y;
            IsPressed = false;
            IsVisited = false;
            IsVisible = true;
        }
        public StarOnMap(Texture2D texture, Vector2 position) 
            : this(texture, position, new Vector2(1, 1)) { }
        #endregion

        #region Else
        public void Update(GameTime gameTime)
        {
            EventUpdate();
        }

        public void CoordsUpdate(GameTime gameTime)
        {
            SS.CoordsUpdate(gameTime);
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
            if (IsVisited)
            {
                spriteBatch.Draw(Circle, position, null, Color.White, 0f, Vector2.Zero, Scale, 0, 0);
            }
        }
        #endregion
    }
}
