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
    public class StationBuilder : Ship
    {
        public delegate void OnBuildingStartHandler(StationBuilder builder);
        public event OnBuildingStartHandler OnBuildingStart;

        public StationBuilder(Texture2D texture, Vector2 position,
            Vector2 scale, float speed, float speedMod, float defence, float mobility, int hp, int shield, string name, string owner, Texture2D circle, List<Weapon> weapons)
            : base(texture, position, scale, speed, speedMod, defence, mobility, hp, shield, name, owner, circle, weapons) { }

        protected override void EventUpdate()
        {
            State = Keyboard.GetState();

            if (State.IsKeyDown(Keys.B) && !IsKeyPressed && IsActive)
            {
                IsKeyPressed = true;
                if (OnBuildingStart != null)
                {
                    OnBuildingStart(this);
                    return;
                }
            }
            if (State.IsKeyUp(Keys.B))
            {
                IsKeyPressed = false;
            }

            base.EventUpdate();
        }
    }
}
