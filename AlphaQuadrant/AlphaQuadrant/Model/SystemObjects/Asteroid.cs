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
    public class Asteroid : IDraw, IUpdateble, IMoveble, IScaleble, IDamagable, ICoordUpdateble
    {
        #region GamePlay Fields
        private Vector2 velocity; //the moving vector
        #endregion

        #region Fields
        private Vector2 position;
        private bool isOver = false;
        private float timer;
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

        public Vector2 Scale { get; set; }

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

        public string Name { get; set; }

        public float CenterX
        {
            get { return FinalWidth - ((Texture.Width * Scale.X) / 2); }
        }

        public float CenterY
        {
            get { return FinalHeight - ((Texture.Height * Scale.Y) / 2); }
        }

        public Vector2 Center
        {
            get { return new Vector2(FinalWidth - ((Texture.Width * Scale.X) / 2), FinalHeight - ((Texture.Height * Scale.Y) / 2)); }
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

        /*public Vector2 PositionFromCenter
        {
            get { return positionFromCenter; }
            set
            {
                positionFromCenter = value;
            }
        }*/
        public Vector2 PositionFromCenter { get; set; }
        private float FinalWidth { get; set; }
        private float FinalHeight { get; set; }
        private MouseState MS { get; set; }
        private bool IsPressed { get; set; }
        private Texture2D Texture { get; set; }
        public bool IsVisible { get; set; }
        #endregion

        #region GamePlay Properties
        public Vector2 Velocity
        {
            get { return velocity; }
            set
            {
                velocity = value;
            }
        }

        public float SpeedX
        {
            get { return Velocity.X; }
            set
            {
                velocity.X = value;
            }
        }

        public float SpeedY
        {
            get { return Velocity.Y; }
            set
            {
                velocity.Y = value;
            }
        }

        public int MaxHP { get; private set; }
        public int HP { get; set; }
        public float Mass { get; set; }
        public int Damage { get; set; }
        public float Size { get; set; }
        #endregion

        #region Events
        public delegate void OnClickHandler(object sender, MouseState ms);
        public delegate void OnOverHandler(object sender, MouseState ms);
        public delegate void OnOverEndHandler();
        //public delegate void OnDestroyHandler(IDamagable sender);
        public event OnClickHandler OnClick;
        public event OnOverHandler OnOver;
        public event OnOverEndHandler OnOverEnd;
        public event OnDestroyHandler OnDestroy;
        #endregion

        #region Constructors
        public Asteroid(Texture2D texture, Vector2 position, Vector2 scale, float mass)
        {
            Texture = texture;
            Position = position;
            Scale = scale;
            FinalWidth = Position.X + Texture.Width * Scale.X;
            FinalHeight = Position.Y + Texture.Height * Scale.Y;
            PositionFromCenter = Vector2.Zero;

            Mass = mass;
            HP = MaxHP = /*Convert.ToInt32(mass);*/ 50;
            Damage = Convert.ToInt32(mass/100);

            IsPressed = false;
            IsVisible = true;
        }
        public Asteroid(Texture2D texture, Vector2 position, Vector2 scale) 
            : this(texture, position, scale, 1000f) { }
        public Asteroid(Texture2D texture, Vector2 position)
            : this(texture, position, new Vector2(1, 1)) { }
        public Asteroid(Texture2D texture)
            : this(texture, new Vector2(0, 0)) { }
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

        #region Else
        public void Update(GameTime gameTime)
        {
            CoordsUpdate(gameTime);
            EventUpdate();
        }

        public void CoordsUpdate(GameTime gameTime)
        {
            //Need to realize asteroid velocity here.
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (timer > 30)
            {
                position.X += Velocity.X;
                position.Y += Velocity.Y;
                FinalHeight = Position.Y + Texture.Height * Scale.Y;
                FinalWidth = Position.X + Texture.Width * Scale.X;
                timer -= 30;
            }
            if (Position.X > 5000 || Position.X < -5000 || Position.Y > 5000 || Position.Y < -5000)
            {
                TakeDamage(MaxHP+1);
                if (OnDestroy != null)
                {
                    OnDestroy(this);
                }
            }
            //End
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
                    OnClick(this, MS);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, position, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
        }
        #endregion
    }
}
