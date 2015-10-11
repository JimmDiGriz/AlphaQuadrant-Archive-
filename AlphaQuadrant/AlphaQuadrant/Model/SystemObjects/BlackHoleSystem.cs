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
    public class BlackHoleSystem : IDraw, IUpdateble, ICoordUpdateble
    {
        #region Fields
        private List<IDraw> objects;
        //private BackGround background;
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

        public BackGround BackGround { private get; set; }
        public bool IsVisible { get; set; }
        #endregion

        #region Events
        #endregion

        #region Constructors
        public BlackHoleSystem(BlackHole hole)
        {
            objects = new List<IDraw>();
            objects.Add(hole);
            IsVisible = true;
        }
        #endregion

        #region Else
        public void Update(GameTime gameTime)
        {
            foreach (IUpdateble obj in objects)
            {
                if (obj is BlackHole)
                {
                    ((BlackHole)obj).Update(gameTime);
                }
                obj.Update(gameTime);
            }
        }

        public void CoordsUpdate(GameTime gameTime)
        { 
            foreach (IDraw obj in objects)
            {
                if (obj is ICoordUpdateble)
                {
                    obj.ToICoordUpdateble().CoordsUpdate(gameTime);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            BackGround.Draw(spriteBatch);
            foreach (IDraw item in objects)
            {
                item.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
