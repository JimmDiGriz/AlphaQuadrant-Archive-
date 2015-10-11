using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using icsimplelib;
using System.Threading;

namespace AlphaQuadrant
{
    public partial class AlphaQuadrant : Microsoft.Xna.Framework.Game
    {
        private void GalaxyMapControls(Screen galaxyMap, Map map)
        {
            KeyboardState state = Keyboard.GetState();
            if ((state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D)) 
                && Math.Abs(((BackGround)galaxyMap.Objects["Back"]).X)
                < ((BackGround)galaxyMap.Objects["Back"]).Width - graphics.PreferredBackBufferWidth)
            {
                ((IMoveble)galaxyMap.Objects["Back"]).X -= MOVEMENT;
                foreach (IDraw obj in map.Objects)
                {
                    ((IMoveble)obj).X -= MOVEMENT;
                }
            }
            if ((state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A)) 
                && ((BackGround)galaxyMap.Objects["Back"]).X < 0)
            {
                ((IMoveble)galaxyMap.Objects["Back"]).X += MOVEMENT;
                foreach (IDraw obj in map.Objects)
                {
                    ((IMoveble)obj).X += MOVEMENT;
                }
            }
            if ((state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
                && ((BackGround)galaxyMap.Objects["Back"]).Y < 0)
            {
                ((IMoveble)galaxyMap.Objects["Back"]).Y += MOVEMENT;
                foreach (IDraw obj in map.Objects)
                {
                    ((IMoveble)obj).Y += MOVEMENT;
                }
            }
            if ((state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
                && Math.Abs(((BackGround)galaxyMap.Objects["Back"]).Y)
                < ((BackGround)galaxyMap.Objects["Back"]).Height - graphics.PreferredBackBufferHeight)
            {
                ((IMoveble)galaxyMap.Objects["Back"]).Y -= MOVEMENT;
                foreach (IDraw obj in map.Objects)
                {
                    ((IMoveble)obj).Y -= MOVEMENT;
                }
            }
            //Scale
            MouseState ms  = Mouse.GetState();
            if (ms.ScrollWheelValue > originalMs.ScrollWheelValue)
            {
                ((IScaleble)galaxyMap.Objects["Back"]).Scale = ((IScaleble)galaxyMap.Objects["Back"]).Scale / new Vector2(0.9f, 0.9f);
                ((IMoveble)galaxyMap.Objects["Back"]).Position = new Vector2(((IMoveble)galaxyMap.Objects["Back"]).Position.X * 1 / 0.9f,
                                    ((IMoveble)galaxyMap.Objects["Back"]).Position.Y * 1 / 0.9f);
                if (((BackGround)galaxyMap.Objects["Back"]).Width < 4000)
                {
                    map.Bigger(0.9f, stepX, stepY);
                }
                else
                {
                    ((IScaleble)galaxyMap.Objects["Back"]).Scale = ((IScaleble)galaxyMap.Objects["Back"]).Scale * new Vector2(0.9f, 0.9f);
                    ((IMoveble)galaxyMap.Objects["Back"]).Position = GalaxyShift(((IMoveble)galaxyMap.Objects["Back"]).Position, new Vector2(0.9f, 0.9f));
                }
                originalMs = ms;
            }
            if (ms.ScrollWheelValue < originalMs.ScrollWheelValue)
            {
                ((IScaleble)galaxyMap.Objects["Back"]).Scale = ((IScaleble)galaxyMap.Objects["Back"]).Scale * new Vector2(0.9f, 0.9f);
                ((IMoveble)galaxyMap.Objects["Back"]).Position = GalaxyShift(((IMoveble)galaxyMap.Objects["Back"]).Position, new Vector2(0.9f, 0.9f));
                if (((BackGround)galaxyMap.Objects["Back"]).Width > graphics.PreferredBackBufferWidth)
                {
                    map.Smaller(0.9f, stepX, stepY);
                }
                else
                {
                    ((IScaleble)galaxyMap.Objects["Back"]).Scale = ((IScaleble)galaxyMap.Objects["Back"]).Scale / new Vector2(0.9f, 0.9f);
                    ((IMoveble)galaxyMap.Objects["Back"]).Position = new Vector2(((IMoveble)galaxyMap.Objects["Back"]).Position.X * 1 / 0.9f,
                       ((IMoveble)galaxyMap.Objects["Back"]).Position.Y * 1 / 0.9f);
                }
                originalMs = ms;
            }

            if (Math.Abs(((BackGround)galaxyMap.Objects["Back"]).X)
                > ((BackGround)galaxyMap.Objects["Back"]).Width - graphics.PreferredBackBufferWidth)
            {
                float temp = Math.Abs(((BackGround)galaxyMap.Objects["Back"]).X) - (((BackGround)galaxyMap.Objects["Back"]).Width - graphics.PreferredBackBufferWidth);
                ((IMoveble)galaxyMap.Objects["Back"]).X += temp;
                foreach (IDraw obj in map.Objects)
                {
                    ((IMoveble)obj).X += temp;
                }
            }
            if (((BackGround)galaxyMap.Objects["Back"]).X > 0)
            {
                float temp = ((BackGround)galaxyMap.Objects["Back"]).X;
                ((IMoveble)galaxyMap.Objects["Back"]).X -= temp;
                foreach (IDraw obj in map.Objects)
                {
                    ((IMoveble)obj).X -= temp;
                }
            }
            if (((BackGround)galaxyMap.Objects["Back"]).Y > 0)
            {
                float temp = ((BackGround)galaxyMap.Objects["Back"]).Y;
                ((IMoveble)galaxyMap.Objects["Back"]).Y -= temp;
                foreach (IDraw obj in map.Objects)
                {
                    ((IMoveble)obj).Y -= temp;
                }
            }
            if (Math.Abs(((BackGround)galaxyMap.Objects["Back"]).Y)
                > ((BackGround)galaxyMap.Objects["Back"]).Height - graphics.PreferredBackBufferHeight)
            {
                float temp = Math.Abs(((BackGround)galaxyMap.Objects["Back"]).Y) - (((BackGround)galaxyMap.Objects["Back"]).Height - graphics.PreferredBackBufferHeight);
                ((IMoveble)galaxyMap.Objects["Back"]).Y += temp;
                foreach (IDraw obj in map.Objects)
                {
                    ((IMoveble)obj).Y += temp;
                }
            }
            //End scale
            if (state.IsKeyDown(Keys.Escape) && !isEscDown)
            {
                isEscDown = true;
                EscEvents(galaxyMap);
            }
            if (state.IsKeyUp(Keys.Escape))
            {
                isEscDown = false;
            }
        }

        private void SolarSystemControls(Screen solarSystem, SolarSystem ss)
        {
            KeyboardState state = Keyboard.GetState();
            if ((state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                && Math.Abs(((BackGround)solarSystem.Objects["Back"]).X)
                < /*((BackGround)solarSystem.Objects["Back"]).Width*/ /*HERE*/SystemSize.X - graphics.PreferredBackBufferWidth)
            {
                ((IMoveble)solarSystem.Objects["Back"]).X -= MOVEMENT;
                foreach (IDraw obj in ss.Objects)
                {
                    ((IMoveble)obj).X -= MOVEMENT;
                    if (obj is StationOnBuilding)
                    {
                        obj.ToStationOnBuilding().Progress.X -= MOVEMENT;
                    }
                }
            }
            if ((state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
                && ((BackGround)solarSystem.Objects["Back"]).X < 0)
            {
                ((IMoveble)solarSystem.Objects["Back"]).X += MOVEMENT;
                foreach (IDraw obj in ss.Objects)
                {
                    ((IMoveble)obj).X += MOVEMENT;
                    if (obj is StationOnBuilding)
                    {
                        obj.ToStationOnBuilding().Progress.X += MOVEMENT;
                    }
                }
            }
            if ((state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
                && ((BackGround)solarSystem.Objects["Back"]).Y < 0)
            {
                ((IMoveble)solarSystem.Objects["Back"]).Y += MOVEMENT;
                foreach (IDraw obj in ss.Objects)
                {
                    ((IMoveble)obj).Y += MOVEMENT;
                    if (obj is StationOnBuilding)
                    {
                        obj.ToStationOnBuilding().Progress.Y += MOVEMENT;
                    }
                }
            }
            if ((state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
                && Math.Abs(((BackGround)solarSystem.Objects["Back"]).Y)
                < /*((BackGround)solarSystem.Objects["Back"]).Height*//*HERE*/SystemSize.Y - graphics.PreferredBackBufferHeight)
            {
                ((IMoveble)solarSystem.Objects["Back"]).Y -= MOVEMENT;
                foreach (IDraw obj in ss.Objects)
                {
                    ((IMoveble)obj).Y -= MOVEMENT;
                    if (obj is StationOnBuilding)
                    {
                        obj.ToStationOnBuilding().Progress.Y -= MOVEMENT;
                    }
                }
            }

            if (state.IsKeyDown(Keys.M))
            {
                SolarSystemClose(solarSystem);
                GalaxyMapOpen(screens["GalaxyMap"].Objects["Map"].ToMap());
            }

            if (state.IsKeyDown(Keys.Escape) && !isEscDown)
            {
                isEscDown = true;
                EscEvents(solarSystem);
            }

            if (state.IsKeyUp(Keys.Escape))
            {
                isEscDown = false;
            }
        }

        private void BlackHoleSystemControls(Screen blackHoleSystem, BlackHoleSystem bhs)
        {
            KeyboardState state = Keyboard.GetState();
            if ((state.IsKeyDown(Keys.Right) || state.IsKeyDown(Keys.D))
                && Math.Abs(((BackGround)blackHoleSystem.Objects["Back"]).X)
                < ((BackGround)blackHoleSystem.Objects["Back"]).Width - graphics.PreferredBackBufferWidth)
            {
                ((IMoveble)blackHoleSystem.Objects["Back"]).X -= MOVEMENT;
                foreach (IDraw obj in bhs.Objects)
                {
                    ((IMoveble)obj).X -= MOVEMENT;
                    if (obj is StationOnBuilding)
                    {
                        obj.ToStationOnBuilding().Progress.X -= MOVEMENT;
                    }
                }
            }
            if ((state.IsKeyDown(Keys.Left) || state.IsKeyDown(Keys.A))
                && ((BackGround)blackHoleSystem.Objects["Back"]).X < 0)
            {
                ((IMoveble)blackHoleSystem.Objects["Back"]).X += MOVEMENT;
                foreach (IDraw obj in bhs.Objects)
                {
                    ((IMoveble)obj).X += MOVEMENT;
                    if (obj is StationOnBuilding)
                    {
                        obj.ToStationOnBuilding().Progress.X += MOVEMENT;
                    }
                }
            }
            if ((state.IsKeyDown(Keys.Up) || state.IsKeyDown(Keys.W))
                && ((BackGround)blackHoleSystem.Objects["Back"]).Y < 0)
            {
                ((IMoveble)blackHoleSystem.Objects["Back"]).Y += 6;
                foreach (IDraw obj in bhs.Objects)
                {
                    ((IMoveble)obj).Y += MOVEMENT;
                    if (obj is StationOnBuilding)
                    {
                        obj.ToStationOnBuilding().Progress.Y += MOVEMENT;
                    }
                }
            }
            if ((state.IsKeyDown(Keys.Down) || state.IsKeyDown(Keys.S))
                && Math.Abs(((BackGround)blackHoleSystem.Objects["Back"]).Y)
                < ((BackGround)blackHoleSystem.Objects["Back"]).Height - graphics.PreferredBackBufferHeight)
            {
                ((IMoveble)blackHoleSystem.Objects["Back"]).Y -= MOVEMENT;
                foreach (IDraw obj in bhs.Objects)
                {
                    ((IMoveble)obj).Y -= MOVEMENT;
                    if (obj is StationOnBuilding)
                    {
                        obj.ToStationOnBuilding().Progress.Y -= MOVEMENT;
                    }
                }
            }
            if (state.IsKeyDown(Keys.M))
            {
                UnloadEvents("BlackHoleSystem");
                GalaxyMapOpen(screens["GalaxyMap"].Objects["Map"].ToMap());

                ((Popup)screens["BlackHoleSystem"].Objects["MoreInfoPopup"]).IsVisible = false;
                ((BackGround)blackHoleSystem.Objects["Back"]).X = 0;
                ((BackGround)blackHoleSystem.Objects["Back"]).Y = 0;
                foreach (IDraw obj in bhs.Objects)
                {
                    if (obj is BlackHole)
                    {
                        ((BlackHole)obj).X = 1000;
                        ((BlackHole)obj).Y = 1000;
                    }
                }
            }
            if (state.IsKeyDown(Keys.Escape) && !isEscDown)
            {
                isEscDown = true;
                EscEvents(blackHoleSystem);
            }
            if (state.IsKeyUp(Keys.Escape))
            {
                isEscDown = false;
            }
        }
    }
}
