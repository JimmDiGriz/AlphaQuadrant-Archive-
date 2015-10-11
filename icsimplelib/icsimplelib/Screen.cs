using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace icsimplelib
{
    [Serializable]
    public class Screen : IDraw
    {
        #region Properties
        public Dictionary<string, IDraw> Objects{ get; set; }
        public bool IsVisible { get; set; }
        #endregion

        #region Events
        #endregion

        #region Constructors
        public Screen()
        {
            Objects = new Dictionary<string, IDraw>();
            IsVisible = true;
        }
        #endregion

        #region Else
        public void Update(GameTime gameTime)
        {
            try
            {
                foreach (KeyValuePair<string, IDraw> kvp in Objects)
                {
                    if (kvp.Value is Sprite)
                    {
                        Sprite temp = (Sprite)kvp.Value;
                        temp.Play(gameTime);
                    }
                    /*else if (kvp.Value is Button)
                    {
                        Button temp = (Button)kvp.Value;
                        temp.Update(gameTime);
                    }
                    else if (kvp.Value is Popup)
                    {
                        Popup temp = (Popup)kvp.Value;
                        temp.Update(gameTime);
                    }*/
                    else if (kvp.Value is IUpdateble)
                    {
                        IUpdateble upd = (IUpdateble)kvp.Value;
                        upd.Update(gameTime);
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
            foreach (KeyValuePair<string, IDraw> drawable in Objects)
            {
                drawable.Value.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
