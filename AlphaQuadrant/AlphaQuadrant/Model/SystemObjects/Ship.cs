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

namespace AlphaQuadrant
{
    /// <summary>
    /// Класс корабля. Очень надеюсь, что смогу описать основную логику внутри класса.
    /// Так же пиздец как надеюсь, что смогу реализовать вменяемые поворот.
    /// </summary>
    [Serializable]
    public class Ship: IDraw, IUpdateble, IMoveble, IScaleble, ICoordUpdateble
    {
        #region GamePlay Fields
        #endregion

        #region Fields
        protected Vector2 position;
        protected const float timerCoef = 10;

        protected Vector2 nextPosition;
        protected Vector2 velocity;
        #endregion

        #region Properites
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

        public float Width
        {
            get { return Texture.Width * Scale.X; }
        }

        public float Height
        {
            get { return Texture.Height * Scale.Y; }
        }

        public Vector2 NextPosition
        {
            get { return nextPosition; }
            set { nextPosition = value; }
        }

        public Vector2 Velocity
        { get { return velocity; } }

        public List<Weapon> Weapons { get; private set; }
        public IMoveble Target { get; private set; }
        protected bool IsRightButtonPressed { get; set; }
        protected bool IsMoving { get; set; }
        public bool IsAttacking { get; private set; }
        public Vector2 Scale { get; set; }
        protected MouseState MS { get; set; }
        protected KeyboardState State { get; set; }
        protected Vector2 Origin { get; set; }
        protected float Timer { get; set; }
        protected float Radians { get; set; }
        protected float Rotation { get; set; }
        protected float FinalWidth { get; set; }
        protected float FinalHeight { get; set; }
        protected bool IsPressed { get; set; }
        protected bool IsActive { get; set; }
        protected Vector2 CircleOrigin { get; set; }
        protected Vector2 CircleScale { get; set; }
        protected Texture2D Circle { get; set; }
        protected Texture2D Texture { get; set; }
        public string Name { get; set; }
        public Vector2 PositionFromCenter { get; set; }
        public Planet OrbitPlanet { get; set; }
        protected bool IsMovingToOrbit { get; set; }
        protected bool IsOnOrbit { get; set; }
        protected bool IsKeyPressed { get; set; }

        protected bool IsWeaponsWaiting { get; set; }
        protected float ShootingRange { get; set; }
        protected string Owner { get; set; }
        protected int Shield { get; set; }
        protected int HP { get; set; }
        public float SpeedMod { get; private set; }
        protected float Mobility { get; set; }
        protected float Defence { get; set; }
        public bool IsVisible { get; set; }
        #endregion

        #region Events
        public delegate IMoveble OnRightButtonClickHandler(Ship sender, MouseState ms);
        public event OnRightButtonClickHandler OnRightButtonClick;
        public delegate void OnSystemQuitHandler(Ship sender);
        public event OnSystemQuitHandler OnSystemQuit;
        public delegate bool OnClickHandler(Ship sender, MouseState ms);
        public event OnClickHandler OnClick;
        #endregion

        #region Constructors
        /// <summary>
        /// Создает ебучий кораблик, всем бояться.
        /// </summary>
        /// <param name="texture">Текстура, просто и ясно.</param>
        /// <param name="position">Позиция, задается начальная, потом ясное дело будет много раз меняться.</param>
        /// <param name="velocity">Передвижение корабля.</param>
        /// <param name="origin">Нужно для поворота, при создании же указывает то, в какую сторону смотрит нос корабля.</param>
        /// <param name="scale">Масштабирование текстурки корабля, как всегда.
        /// Но в данном случае это будет иметь некоторый геймплейный смысл.</param>
        /// <param name="speed">Скорость корабля. Является собственно самой скоростью.</param>
        /// <param name="speedMod">Модификатор скорости корабля. Зависит от параметры расы.</param>
        /// <param name="defence">Защита корабя. Выступает модификатором при получении урона.</param>
        /// <param name="mobility">Маневренность корабля. Модификатор при повороте.</param>
        /// <param name="hp">Хп корабля, а именно корпуса.</param>
        /// <param name="shield">Щит корабля.</param>
        /// <param name="name">Имя корабля. Можно будет менять после создания.</param>
        public Ship(Texture2D texture, Vector2 position, 
            Vector2 scale, float speed, float speedMod, float defence, float mobility, int hp, int shield, string name, string owner, Texture2D circle, List<Weapon> weapons)
        {
            Texture = texture;
            this.position = position;
            this.velocity = new Vector2(speed, speed);
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            Scale = scale;
            //this.speed = speed;
            SpeedMod = speedMod;
            Defence = defence;
            Mobility = mobility;
            HP = hp;
            Shield = shield;
            Name = name;
            Owner = owner;

            Circle = circle;
            float tempScale = Texture.Width >= Texture.Height ? (Circle.Width / Texture.Width) * Scale.X : (Circle.Width / Texture.Height) * Scale.Y;
            CircleScale = new Vector2(tempScale);
            CircleOrigin = new Vector2((Circle.Width - Texture.Width )/ 2, (Circle.Height - Texture.Height)/ 2);
            //circleScale *= scale;

            FinalWidth = position.X + texture.Width * scale.X;
            FinalHeight = position.Y + texture.Height * scale.Y;

            PositionFromCenter = Vector2.Zero;

            Weapons = weapons;
            if (Weapons.Count > 0)
            {
                ShootingRange = Weapons[0].Distance;
                foreach (Weapon weapon in weapons)
                {
                    if (weapon.Distance < ShootingRange)
                    {
                        ShootingRange = weapon.Distance;
                    }
                    weapon.OnShoot += new Weapon.OnShootHandler(OnWeaponShoot);
                    weapon.OwnerShip = this;
                }
            }
            else
            {
                ShootingRange = 0;
            }

            IsAttacking = false;
            Target = null;
            IsRightButtonPressed = false;
            IsMoving = false;
            Rotation = 0;
            IsWeaponsWaiting = false;
            IsMovingToOrbit = false;
            IsOnOrbit = false;
            IsKeyPressed = false;
            IsVisible = true;
            IsActive = false;
        }
        #endregion

        #region Else
        protected void StartMoving()
        {
            nextPosition = new Vector2((float)MS.X - Width / 2, (float)MS.Y - Height / 2);
            Vector2 oneStep = nextPosition - position;
            Radians = (float)Math.Atan2(oneStep.Y, oneStep.X) + (90 * (float)Math.PI / 180);
            oneStep.Normalize();
            velocity = oneStep * (SpeedMod / 2);
            ChangeFinals();
        }

        public void Update(GameTime gameTime)
        {
            CoordsUpdate(gameTime);
            EventUpdate();
        }

        public void CoordsUpdate(GameTime gameTime)
        {
            if (IsAttacking)
            {
                FollowTarget();
                Attack(gameTime);
            }
            if (IsMovingToOrbit)
            {
                FollowPlanet();
            }
            if (IsOnOrbit)
            {
                position = OrbitPlanet.Position;
                PositionFromCenter = OrbitPlanet.PositionFromCenter;
                ChangeFinals();
            }

            WeaponsUpdate(gameTime);

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

        protected virtual void EventUpdate()
        {
            MS = Mouse.GetState();
            State = Keyboard.GetState();

            if (State.IsKeyDown(Keys.Q) && !IsKeyPressed && IsActive)
            {
                IsKeyPressed = true;
                if (OnSystemQuit != null)
                {
                    OnSystemQuit(this);
                    return;
                }
            }
            if (State.IsKeyUp(Keys.Q))
            {
                IsKeyPressed = false;
            }

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

                if (IsOnOrbit)
                {
                    GoneFromOrbit();
                }

                IsRightButtonPressed = true;
                IsMoving = true;

                if (Target == null)
                {
                    IsAttacking = false;
                    StartMoving();
                }
                else if (Target is Planet)
                {
                    OrbitPlanet = Target.ToPlanet();
                    IsMovingToOrbit = true;
                }
                else
                {
                    IsAttacking = true;
                }
            }
        }

        protected void WeaponsUpdate(GameTime gameTime)
        {
            if (Weapons.Count(x => x.IsWaiting) > 0)
            {
                IsWeaponsWaiting = true;
                foreach (Weapon weapon in Weapons)
                {
                    weapon.Update(gameTime);
                }
            }
            else
            {
                IsWeaponsWaiting = false;
            }
        }

        /// <summary>
        /// Установка корабля на орбиту планеты.
        /// </summary>
        /// <param name="planet">Планета, очевидная.</param>
        public void ToOrbit()
        {
            OrbitPlanet.ShipsOnOrbit.Add(this);
            IsMovingToOrbit = false;
            IsOnOrbit = true;
        }

        public void GoneFromOrbit()
        {
            OrbitPlanet.ShipsOnOrbit.Remove(this);
            IsOnOrbit = false;
        }

        protected void Over()
        {
            Click();
        }

        protected void UnfocusClick()
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

        protected void Click()
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

        protected void MoveTo()
        {
            Position += velocity;
            FinalWidth = Position.X + Texture.Width * Scale.X;
            FinalHeight = Position.Y + Texture.Height * Scale.Y;
            if ((Position - NextPosition).Length() <= (SpeedMod/2))
            {
                IsMoving = false;
            }
        }

        protected void ChangeFinals()
        {
            FinalHeight = position.Y + Texture.Height * Scale.Y;
            FinalWidth = position.X + Texture.Width * Scale.X;
        }

        /// <summary>
        /// Следует за целью и атакует.
        /// </summary>
        protected void FollowTarget()
        {
            nextPosition = new Vector2(Target.ToIDamagable().Center.X - Width / 2, Target.ToIDamagable().Center.Y - Height / 2);
            Vector2 oneStep = nextPosition - position;
            Radians = (float)Math.Atan2(oneStep.Y, oneStep.X) + (90 * (float)Math.PI / 180);
            oneStep.Normalize();
            velocity = oneStep * (SpeedMod/2);
            if (IsAttacking && (position - nextPosition).Length() <= ShootingRange-30f)
            {
                IsMoving = false;
            }
            else
            {
                IsMoving = true;
            }
            ChangeFinals();
        }

        /// <summary>
        /// Следует к планете, чтобы встать на орбиту.
        /// </summary>
        protected void FollowPlanet()
        {
            nextPosition = new Vector2(OrbitPlanet.Center.X - Width / 2, OrbitPlanet.Center.Y - Height / 2);
            Vector2 oneStep = nextPosition - position;
            Radians = (float)Math.Atan2(oneStep.Y, oneStep.X) + (90 * (float)Math.PI / 180);
            oneStep.Normalize();
            velocity = oneStep * (SpeedMod / 2);
            if (IsMovingToOrbit && (position - nextPosition).Length() <= OrbitPlanet.Width/2)
            {
                ToOrbit();
            }
        }

        protected void Attack(GameTime gameTime)
        {
            foreach (Weapon weapon in Weapons)
            {
                if ((position - nextPosition).Length() <= weapon.Distance && !weapon.IsShooting)
                {
                    weapon.IsShooting = true;
                }
                if ((position - nextPosition).Length() > weapon.Distance && weapon.IsShooting)
                {
                    weapon.IsShooting = false;
                }
                weapon.Update(gameTime);
            }
        }

        protected void OnWeaponShoot(Weapon weapon)
        {
            if (Target.ToIDamagable().TakeDamage(weapon.Damage))
            {
                IsAttacking = false;
                Target = null;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //radians = rotation / 10 * (float)Math.PI / 180;
            spriteBatch.Draw(Texture, 
                new Vector2(position.X + (Origin.X * Scale.X), position.Y + (Origin.Y * Scale.Y)),
                null, Color.White, Radians, Origin, Scale, 0, 0);
            if (IsActive)
            {
                spriteBatch.Draw(Circle, position, null, Color.White, 0f, CircleOrigin, CircleScale, 0, 0);
            }
            if (IsAttacking || IsWeaponsWaiting)
            {
                foreach (Weapon weapon in Weapons)
                {
                    weapon.Draw(spriteBatch);
                }
            }
        }
        #endregion
    }
}
