using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using icsimplelib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AlphaQuadrant
{
    [Serializable]
    public class Map: IDraw, IUpdateble
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
            }
        }

        public bool IsVisible { get; set; }
        #endregion

        #region Events
        #endregion

        #region Constructors
        public Map()
        {
            objects = new List<IDraw>();
            IsVisible = true;
        }
        #endregion

        #region Scales
        public void Bigger(float scale, float stepX, float stepY)
        {
            foreach (IDraw obj in objects)
            {
                ((IScaleble)obj).Scale = ((IScaleble)obj).Scale / new Vector2(scale, scale);
                ((IMoveble)obj).Position = new Vector2(((IMoveble)obj).Position.X * 1 / scale, ((IMoveble)obj).Position.Y * 1 / scale);
            }
        }

        public void Smaller(float scale, float stepX, float stepY)
        {
            foreach (IDraw obj in objects)
            {
                ((IScaleble)obj).Scale = ((IScaleble)obj).Scale * new Vector2(scale, scale);
                ((IMoveble)obj).Position = new Vector2(((IMoveble)obj).Position.X * 1 * scale, ((IMoveble)obj).Position.Y * 1 * scale);
            }
        }
        #endregion

        #region Else
        public void Update(GameTime gameTime)
        {
            try
            {
                foreach (IDraw obj in objects)
                {
                    if (obj is IUpdateble)
                    {
                        obj.ToIUpdateble().Update(gameTime);
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
            foreach (IDraw item in objects)
            {
                item.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
