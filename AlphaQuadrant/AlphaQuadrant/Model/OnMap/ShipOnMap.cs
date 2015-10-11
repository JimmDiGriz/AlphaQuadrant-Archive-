using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using icsimplelib;
using Microsoft.Xna.Framework.Input;

namespace AlphaQuadrant
{
    [Serializable]
    public class ShipOnMap : IDraw, IMoveble, IScaleble, IUpdateble, ICoordUpdateble
    {
        #region Fields
        private Vector2 position;
        private bool isOver = false;
        private const float timerCoef = 10;

        private Vector2 nextPosition;
        private Vector2 velocity;
        #endregion

        #region Properties
        private Texture2D Texture { get; set; }
        private Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        private float FinalWidth { get; set; }
        private float FinalHeight { get; set; }
        private MouseState MS { get; set; }
        private bool IsPressed { get; set; }
        private bool IsActive { get; set; }
        public Texture2D Circle { get; set; }
        private Vector2 CircleOrigin { get; set; }
        private Vector2 CircleScale { get; set; }
        public bool IsOwnByThisPlayer { get; set; }
        private float Radians { get; set; }
        private float Rotation { get; set; }
        private bool IsRightButtonPressed { get; set; }
        private bool IsMoving { get; set; }
        private float Timer { get; set; }

        private float SpeedMod { get; set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        private float Mobility { get; set; }
        private IMoveble Target { get; set; }
        public List<Ship> Ships { get; set; }
        private bool IsMovingToSystem { get; set; }
        public bool IsVisible { get; set; }

        public float X
        {
            get { return position.X; }
            set
            {
                float temp = position.X - value;
                position.X = value;
                nextPosition.X -= temp;
                FinalWidth = position.X + Texture.Width * Scale.X;
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                float temp = position.Y - value;
                position.Y = value;
                nextPosition.Y -= temp;
                FinalHeight = position.Y + Texture.Height * Scale.Y;
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                ChangeFinals();
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

        private Vector2 NextPosition
        { 
            get { return nextPosition; }
            set { nextPosition = value; }
        }
        #endregion

        #region Events
        public delegate IMoveble OnRightButtonClickHandler(ShipOnMap sender, MouseState ms);
        public event OnRightButtonClickHandler OnRightButtonClick;
        public delegate void OnSystemEnterHandler(ShipOnMap sender, IMoveble target);
        public event OnSystemEnterHandler OnSystemEnter;
        public delegate bool OnClickHandler(ShipOnMap sender, MouseState ms);
        public event OnClickHandler OnClick;
        #endregion

        #region Construct
        public ShipOnMap(Texture2D texture, Texture2D circle, Vector2 position, Vector2 scale, List<Ship> ships)
        {
            Texture = texture;
            Circle = circle;
            Scale = scale;
            this.position = position;
            this.velocity = ships.Min(x => x.Velocity);

            FinalWidth = position.X + texture.Width * scale.X;
            FinalHeight = position.Y + texture.Height * scale.Y;

            Ships = ships;

            IsPressed = false;
            IsOwnByThisPlayer = false;
            IsRightButtonPressed = false;
            IsMoving = false;

            SpeedMod = ships.Min(x => x.SpeedMod);

            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            CircleOrigin = new Vector2((Circle.Width - Texture.Width) / 2, (Circle.Height - Texture.Height) / 2);
            CircleScale = Scale;

            Rotation = 0;
            Timer = 0;
            Mobility = 0;
            Target = null;
            IsMovingToSystem = false;
            IsVisible = true;
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
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (IsMoving)
            {
                if (Timer > timerCoef)
                {
                    MoveTo();
                }
            }

            if (Timer > timerCoef)
            {
                Rotation += Mobility + 6f;
                Timer -= timerCoef;
            }
        }

        private void EventUpdate()
        {
            MS = Mouse.GetState();
            if (MS.LeftButton == ButtonState.Released)
            {
                IsPressed = false;
            }

            if (IsRightButtonPressed == true && MS.RightButton == ButtonState.Released)
            {
                IsRightButtonPressed = false;
            }

            if (MS.X <= FinalWidth && MS.Y <= FinalHeight && MS.X > position.X && MS.Y > position.Y)
            {
                Over();
            }
            else
            {
                UnfocusClick();
            }


            if (IsActive == true && MS.RightButton == ButtonState.Pressed && IsRightButtonPressed == false)
            {
                if (OnRightButtonClick != null)
                {
                    Target = OnRightButtonClick(this, MS);
                }

                IsRightButtonPressed = true;
                IsMoving = true;
                IsMovingToSystem = false;

                if (Target == null)
                {
                    StartMoving();
                }
                else
                {
                    IsMovingToSystem = true;
                    StartMoving();
                }
            }
        }

        private void Over()
        {
            Click();
        }

        private void UnfocusClick()
        {
            if (MS.LeftButton == ButtonState.Pressed && IsPressed == false && IsActive == true)
            {
                IsActive = false;
            }
            if (IsActive == false)
            {
                //texture = staticTexture;
            }
        }

        private void Click()
        {
            if (OnClick != null)
            {
                if (!OnClick(this, MS))
                {
                    return;
                }
            }
            if (MS.LeftButton == ButtonState.Pressed && IsPressed == false && IsActive == false)
            {
                IsActive = true;
                IsPressed = true;
            }
        }
        #endregion

        #region Moving
        private void StartMoving()
        {
            nextPosition = new Vector2((float)MS.X - Width / 2, (float)MS.Y - Height / 2);
            Vector2 oneStep = nextPosition - position;
            Radians = (float)Math.Atan2(oneStep.Y, oneStep.X) + (90 * (float)Math.PI / 180);
            oneStep.Normalize();
            velocity = oneStep * (SpeedMod / 2);
            ChangeFinals();
        }

        private void MoveTo()
        {
            Position += velocity;
            FinalWidth = Position.X + Texture.Width * Scale.X;
            FinalHeight = Position.Y + Texture.Height * Scale.Y;
            if ((Position - NextPosition).Length() <= (SpeedMod / 2))
            {
                IsMoving = false;
                if (IsMovingToSystem)
                {
                    IsMovingToSystem = false;
                    if (OnSystemEnter != null)
                    {
                        OnSystemEnter(this, Target);
                    }
                }
            }
        }
        #endregion

        #region Else
        private void ChangeFinals()
        {
            FinalHeight = position.Y + Texture.Height * Scale.Y;
            FinalWidth = position.X + Texture.Width * Scale.X;
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture,
                new Vector2(position.X + (Origin.X * Scale.X), position.Y + (Origin.Y * Scale.Y)),
                null, Color.White, Radians, Origin, Scale, 0, 0);
            if (IsActive)
            {
                spriteBatch.Draw(Circle, position, null, Color.White, 0f, CircleOrigin, CircleScale, 0, 0);
            }
        }
        #endregion
    }
}
