using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using icsimplelib;

namespace AlphaQuadrant
{
    /// <summary>
    /// Класс оружия, очевидно.
    /// </summary>
    [Serializable]
    public class Weapon : IDraw, IUpdateble
    {
        #region Fields
        private bool isShooting = false;
        private Ship ownerShip;
        #endregion

        #region Propereties
        public bool IsShooting
        {
            get { return isShooting; }
            set
            {
                isShooting = value;
            }
        }

        public Ship OwnerShip
        {
            get { return ownerShip; }
            set
            {
                Position = value.Position;
                ownerShip = value;
            }
        }

        public float Angle { get; set; }
        public float Distance { get; private set; }
        private Vector2 Position { get; set; }
        public int Damage { get; private set; }
        private List<Ammo> Ammo { get; set; }
        private bool CollisionMode { get; set; }
        public bool IsWaiting { get; private set; }
        private Texture2D Texture { get; set; }
        private Vector2 Velocity { get; set; }
        private Vector2 Origin { get; set; }
        private float Interval { get; set; }
        private float Timer { get; set; }
        private bool IsDrawing { get; set; }
        private float Radians { get; set; }
        private float SpeedMod { get; set; }
        public bool IsVisible { get; set; }
        #endregion

        #region Events
        public delegate void OnShootHandler(Weapon weapon);
        public event OnShootHandler OnShoot;

        private void OnHit(Ammo ammo)
        {
            Ammo.Remove(ammo);
            if (OnShoot != null)
            {
                OnShoot(this);
            }
        }

        private void OnGoingTooFar(Ammo ammo)
        {
            Ammo.Remove(ammo);
            if (IsWaiting && Ammo.Count == 0)
            {
                IsWaiting = false;
            }
        }
        #endregion

        #region Construct
        public Weapon(Texture2D texture, float angle, float distance, int damage, Vector2 speed, float interval)
        {
            Texture = texture;
            Angle = angle;
            Distance = distance;
            Damage = damage;

            Origin = new Vector2(texture.Width / 2, texture.Height / 2);

            Velocity = speed;
            Interval = interval;
            Timer = 0f;

            SpeedMod = 5f;
            Ammo = new List<Ammo>();
            CollisionMode = false;
            IsWaiting = false;
            IsDrawing = false;
            IsVisible = true;
        }
        #endregion

        #region Else

        public void Update(GameTime gameTime)
        {
            Position = OwnerShip.Center;
            if (isShooting)
            {
                foreach (Ammo ammo in Ammo)
                {
                    if (CollisionMode)
                    {
                        ammo.TargetPosition = OwnerShip.NextPosition;
                    }
                    ammo.Update(gameTime);
                }

                Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (Timer > Interval)
                {
                    Timer -= Interval;
                    if (!IsDrawing)
                    {
                        ShootingStart();
                    }
                    else
                    {
                        ShootingProcess();
                    }
                }
            }
        }

        private void CreateNewAmmo()
        {
            Vector2 oneStep = OwnerShip.NextPosition - Position;
            Radians = (float)Math.Atan2(oneStep.Y, oneStep.X) + (90 * (float)Math.PI / 180);
            oneStep.Normalize();
            Velocity = oneStep * (SpeedMod / 2f);
            Ammo temp = new Ammo(Texture, Position, Origin, Velocity, SpeedMod, Radians, OwnerShip.NextPosition);
            temp.OnHit += new Ammo.OnHitHandler(OnHit);
            temp.OnGoingTooFar += new Ammo.OnGoingTooFarHandler(OnGoingTooFar);
            Ammo.Add(temp);
        }

        private void DeleteAllAmmo()
        {
            Ammo = new List<Ammo>();
            IsDrawing = false;
        }

        private void ShootingStart()
        {
            IsDrawing = true;
            CreateNewAmmo();
        }

        private void ShootingProcess()
        {
            if (OwnerShip.Target == null)
            {
                ShootingEnd();
                return;
            }
            CreateNewAmmo();
        }

        private void ShootingEnd()
        {
            if (Ammo.Count > 0)
            {
                IsWaiting = true;
            }
            else
            { 
                IsDrawing = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsDrawing)
            {
                foreach (Ammo ammo in Ammo)
                {
                    ammo.Draw(spriteBatch);
                }
            }
        }
        #endregion
    }
}
