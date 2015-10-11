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
    public class QueryFactory
    {
        #region Fields
        #endregion

        #region Properties
        private Dictionary<string, Screen> Screens;
        #endregion

        #region Construct
        public QueryFactory(Dictionary<string, Screen> screens)
        {
            Screens = screens;
        }
        #endregion

        #region Factory
        #endregion

        #region Else
        private void CheckPopupVisibility(Planet planet)
        {
            if (Screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup().IsVisible)
            {
                RefreshPopup(Screens["SolarSystem"].Objects["MoreInfoPopup"].ToPopup(), planet);
            }
        }

        private void RefreshPopup(Popup popup, Planet planet)
        { 
            
        }
        #endregion
    }
}
