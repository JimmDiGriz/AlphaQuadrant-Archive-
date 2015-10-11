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
    public class BlackHole : IDraw, IUpdateble, IMoveble, IScaleble, ICoordUpdateble
    {
        #region GamePlay Fields
        #endregion

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
            get { return FinalWidth - ((Texture.Width * Scale.X) / 2); }
        }

        public float CenterY
        {
            get { return FinalHeight - ((Texture.Height * Scale.Y) / 2); }
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
        private MouseState MS { get; set; }
        private bool IsPressed { get; set; }
        public Vector2 Scale { get; set; }

        private float Mass { get; set; }
        private float Gravity { get; set; }
        private float Size { get; set; }
        public string Name { get; set; }
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
        public BlackHole(Texture2D texture, Vector2 position, Vector2 scale)
        {
            Texture = texture;
            Position = position;
            Scale = scale;
            FinalWidth = Position.X + Texture.Width * Scale.X;
            FinalHeight = Position.Y + Texture.Height * Scale.Y;

            IsPressed = false;
            IsVisible = true;
        }
        public BlackHole(Texture2D texture, Vector2 position)
            : this(texture, position, new Vector2(1, 1)) { }
        public BlackHole(Texture2D texture)
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
            
        }

        private void EventUpdate()
        {
            MS = Mouse.GetState();
            if (MS.LeftButton == ButtonState.Released)
            {
                IsPressed = false;
            }
            if (MS.X <= FinalWidth && MS.Y <= FinalHeight && MS.X > position.X && MS.Y > position.Y)
            {
                Task.Factory.StartNew(Over);
                Click();
            }
            else
            {
                /*texture = staticTexture;*/
                if (isOver == true)
                {
                    IsOver = false;
                }
            }
        }

        private void Over()
        {
            if (!isOver)
            {
                Thread.Sleep(300);
                isOver = true;
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
                if (OnClick != null)
                {
                    OnClick(this, MS);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
        }
        #endregion
    }
}
