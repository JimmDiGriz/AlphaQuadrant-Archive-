using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using icsimplelib;

namespace AlphaQuadrant
{
    [Serializable]
    public class SolarSystem: IDraw, IUpdateble, ICoordUpdateble
    {
        #region Fields
        private List<IDraw> objects;
        #endregion

        #region Properties

        public List<IDraw> Objects
        {
            get { return objects; }
            set
            {
                objects.Add(value[0]);
                if (!(value[0] is Ship))
                {
                    Collision.Add((IMoveble)value[0]);
                    if (value[0] is Asteroid)
                    {
                        Asteroids.Add(value[0]);
                    }
                    else if (value[0] is Planet)
                    {
                        Planets.Add(value[0]);
                    }
                }
                else
                {
                    Ships.Add(value[0]);
                }
            }
        }

        public BackGround BackGround { get; set; }
        public List<IMoveble> Collision { get; set; }
        public List<IDraw> Planets { get; set; }
        public List<IDraw> Asteroids { get; set; }
        public List<IDraw> Ships { get; set; }
        public bool IsVisible { get; set; }
        #endregion

        #region Events
        #endregion

        #region Constructors
        public SolarSystem(Star star)
        {
            objects = new List<IDraw>();
            Collision = new List<IMoveble>();
            Planets = new List<IDraw>();
            Asteroids = new List<IDraw>();
            Ships = new List<IDraw>();
            objects.Add(star);
            IsVisible = true;
        }
        #endregion

        #region Else
        public void Update(GameTime gameTime)
        {
            try
            {
                foreach (IUpdateble obj in objects)
                {
                    if (obj is Planet)
                    {
                        ((Planet)obj).Update(gameTime);
                    }
                    obj.Update(gameTime);
                }
            }
            catch
            {
                return;
            }
        }

        public void CoordsUpdate(GameTime gameTime)
        {
            try
            {
                foreach (IDraw obj in objects)
                {
                    if (obj is ICoordUpdateble)
                    {
                        obj.ToICoordUpdateble().CoordsUpdate(gameTime);
                    }
                }
            }
            catch 
            {
                return;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            BackGround.Draw(spriteBatch);
            try
            {
                foreach (IDraw item in objects)
                {
                    item.Draw(spriteBatch);
                }
            }
            catch
            {
                return;
            }
        }
        #endregion
    }
}
