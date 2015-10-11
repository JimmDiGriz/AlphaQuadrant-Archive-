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
    public class StationOnBuilding : IDraw, IMoveble, IScaleble, IUpdateble, ICoordUpdateble, IDamagable
    {
        #region Fields
        private Vector2 position;
        private ProgressBar progress;
        #endregion

        #region Properties
        private Texture2D CurrentTexture { get; set; }
        private Texture2D FirstStageTexture { get; set; }
        private Texture2D SecondStageTexture { get; set; }
        private Texture2D ThirdStageTexture { get; set; }
        private Texture2D FourthStageTexture { get; set; }
        public Vector2 PositionFromCenter { get; set; }
        private float FinalWidth { get; set; }
        private float FinalHeight { get; set; }
        public bool IsVisible { get; set; }
        public Vector2 Scale { get; set; }
        public ProductQuery Query { get; set; }
        public int MaxHP { get; private set; }
        public int HP { get; set; }

        public ProgressBar Progress 
        { 
            get { return progress; } 
            set { progress = value; } 
        }

        public string Owner { get; set; }
        public string Name { get; set; }

        public float X
        {
            get { return position.X; }
            set
            {
                position.X = value;
                FinalWidth = position.X + CurrentTexture.Width * Scale.X;
               // ProgressCoordsUpdate();
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                FinalHeight = position.Y + CurrentTexture.Height * Scale.Y;
                //ProgressCoordsUpdate();
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                FinalHeight = position.Y + CurrentTexture.Height * Scale.Y;
                FinalWidth = position.X + CurrentTexture.Width * Scale.X;
                //ProgressCoordsUpdate();
            }
        }

        public float Width
        {
            get { return CurrentTexture.Width * Scale.X; }
        }

        public float Height
        {
            get { return CurrentTexture.Height * Scale.Y; }
        }

        public Vector2 Center
        {
            get { return new Vector2(FinalWidth - ((CurrentTexture.Width * Scale.X) / 2), FinalHeight - ((CurrentTexture.Height * Scale.Y) / 2)); }
        }
        #endregion

        #region Events
        public delegate void OnBuildingCompleteHandler(StationOnBuilding sender);
        public event OnBuildingCompleteHandler OnBuildingComplete;
        public event OnDestroyHandler OnDestroy;
        #endregion

        #region Construct
        public StationOnBuilding(Texture2D first, Texture2D second, Texture2D third, Texture2D fourth, Vector2 position, Vector2 scale, ProgressBar progress, int hp)
        {
            FirstStageTexture = first;
            SecondStageTexture = second;
            ThirdStageTexture = third;
            FourthStageTexture = fourth;
            CurrentTexture = FirstStageTexture;
            Position = position;
            Scale = scale;
            FinalWidth = Position.X + CurrentTexture.Width * Scale.X;
            FinalHeight = Position.Y + CurrentTexture.Height * Scale.Y;
            IsVisible = true;

            MaxHP = HP = hp;

            this.progress = progress;

            Query = new ProductQuery();
            Query.IsVisible = true;
            Query.Add(new GameTimer(Progress, 0.1f, () =>
                {
                    Progress.PWidth = 0;
                    if (OnBuildingComplete != null)
                    {
                        OnBuildingComplete(this);
                    }
                }, null));
        }
        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
            CoordsUpdate(gameTime);
            Query.Update(gameTime);
            StageUpdate();
        }

        public void CoordsUpdate(GameTime gameTime)
        {
            //ProgressCoordsUpdate();
        }

        private void ProgressCoordsUpdate()
        {
            Progress.Position = new Vector2(position.X, position.Y - Progress.Height - 5);
        }

        private void StageUpdate()
        {
            float delta = Progress.PWidth / Progress.TextureWidth;
            if (delta >= 0.25 && delta < 0.5)
            {
                CurrentTexture = SecondStageTexture;
            }
            else if (delta >= 0.5 && delta < 0.75)
            {
                CurrentTexture = ThirdStageTexture;
            }
            else if (delta >= 0.75)
            {
                CurrentTexture = FourthStageTexture;
            }
        }
        #endregion

        #region IDamagable
        public bool TakeDamage(int damage)
        {
            HP -= damage;
            if (HP <= 0)
            {
                if (OnDestroy != null)
                {
                    OnDestroy(this);
                }
                //TODO: Проиграть взрыв, когда будет.
                return true; //уничтожен.
            }
            return false; //Не уничтожен.
        }
        #endregion

        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(CurrentTexture, position, null, Color.White, 0, Vector2.Zero, Scale, 0, 0);
            Progress.Draw(spriteBatch);
        }
        #endregion

        #region Else
        #endregion
    }
}
