using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using System.Threading;
using icsimplelib;
using System.Threading.Tasks;

namespace AlphaQuadrant
{
    [Serializable]
    public class Station : IDraw, IUpdateble, ICoordUpdateble, IMoveble, IScaleble, IDamagable
    {
        #region Fields
        private Vector2 position;
        private bool isOver = false;
        #endregion

        #region Properties
        private Texture2D Texture { get; set; }
        public Vector2 PositionFromCenter { get; set; }
        private float FinalWidth { get; set; }
        private float FinalHeight { get; set; }
        public bool IsVisible { get; set; }
        public Vector2 Scale { get; set; }
        public ProductQuery Query { get; set; }
        public int MaxHP { get; private set; }
        public int HP { get; set; }
        private MouseState MS { get; set; }
        private bool IsPressed { get; set; }

        public string Owner { get; set; }
        public string Name { get; set; }

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

        public float Width
        {
            get { return Texture.Width * Scale.X; }
        }

        public float Height
        {
            get { return Texture.Height * Scale.Y; }
        }

        public Vector2 Center
        {
            get { return new Vector2(FinalWidth - ((Texture.Width * Scale.X) / 2), FinalHeight - ((Texture.Height * Scale.Y) / 2)); }
        }
        #endregion

        #region Events
        public delegate void OnClickHandler(Station sender, MouseState ms);
        public delegate void OnOverHandler(Station sender, MouseState ms);
        public delegate void OnOverEndHandler();
        public event OnClickHandler OnClick;
        public event OnOverHandler OnOver;
        public event OnOverEndHandler OnOverEnd;
        public event OnDestroyHandler OnDestroy;
        #endregion

        #region Construct
        public Station(Texture2D texture, Vector2 position, Vector2 scale, int hp)
        {
            Texture = texture;
            Position = position;
            Scale = scale;
            MaxHP = HP = hp;
            IsPressed = false;
            FinalWidth = position.X + texture.Width * scale.X;
            FinalHeight = position.Y + texture.Height * scale.Y;
        }
        #endregion

        #region Update
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
        #endregion

        #region IDamagable
        public bool TakeDamage(int damage)
        {
            HP -= damage;
            if (HP <= 0)
            {
                if (OnDestroy != null)
                {
                    OnDestroy(this);
                }
                //TODO: Проиграть взрыв, когда будет.
                return true; //уничтожен.
            }
            return false; //Не уничтожен.
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
        }
        #endregion
    }
}
