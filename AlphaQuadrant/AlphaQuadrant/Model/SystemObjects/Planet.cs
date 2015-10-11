using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using icsimplelib;
using System.Threading;
using System.Threading.Tasks;

namespace AlphaQuadrant
{
    [Serializable]
    public class Planet : IDraw, IUpdateble, IMoveble, IScaleble, ICoordUpdateble
    {
        #region GamePlayFields
        private /*className*/ object consist; //состав
        private /*className*/ object atmosphere; //атмосфера
        private /*className*/ object gettingResources; //получаемые ресурсы.
        private int terraform;
        #endregion

        #region Fields
        private Vector2 position;
        //private bool isPressed = false;
        private bool isOver = false;
        //private float rotation = 0;
        #endregion

        #region Properties
        public float X
        {
            get { return position.X; }
            set
            {
                position.X = value;
                FinalWidth = position.X + Texture.Width * Scale.X;
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                FinalHeight = position.Y + Texture.Height * Scale.Y;
            }
        }
        
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                FinalHeight = position.Y + Texture.Height * Scale.Y;
                FinalWidth = position.X + Texture.Width * Scale.X;
            }
        }
        
        private bool IsOver
        {
            get { return isOver; }
            set
            {
                if (value == false)
                {
                    if (OnOverEnd != null)
                    {
                        OnOverEnd();
                    }
                }
                isOver = value;
            }
        }
        public float Width
        {
            get { return Texture.Width * Scale.X; }
        }
        public float Height
        {
            get { return Texture.Height * Scale.Y; }
        }

        public Vector2 Center
        {
            get { return new Vector2(FinalWidth - ((Texture.Width * Scale.X) / 2), FinalHeight - ((Texture.Height * Scale.Y) / 2)); }
        }

        private Texture2D Texture { get; set; }
        private float FinalWidth { get; set; }
        private float FinalHeight { get; set; }
        private MouseState MS { get; set; }
        public Star CenterStar { get; private set; }
        public Vector2 Scale { get; set; }
        public string Name { get; set; }
        private int Radius { get; set; }
        private float Interval { get; set; }
        private float Timer { get; set; }
        private float Counter { get; set; }
        private float Radians { get; set; }
        private Vector2 Origin { get; set; }
        private bool IsPressed { get; set; }
        private float Rotation { get; set; }
        public Texture2D Circle { get; set; }
        public List<Ship> ShipsOnOrbit { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTerraforming { get; set; }
        #endregion

        #region GamePlayProperties
        public int Terraform
        {
            get { return terraform; }
            set
            {
                if (value <= 4)
                {
                    terraform = value;
                }
            }
        }

        public int PlanetSize { get; set; }
        public int MinTemperature { get; set; }
        public int MaxTemperature { get; set; }
        public bool IsAborigens { get; set; }
        public float Mass { get; set; }
        public string Owner { get; set; }
        public string Race { get; set; }
        public float Gravity { get; set; }
        public string Climat { get; set; }
        public float Stability { get; set; }
        public float Fertility { get; set; }
        public float Radioactivity { get; set; }
        public ProductQuery Query { get; set;}
        public Vector2 PositionFromCenter { get; set; }
        public int MaxTerraform { get; set; }
        #endregion

        #region Events
        public delegate void OnClickHandler(Planet sender, MouseState ms);
        public delegate void OnOverHandler(Planet sender, MouseState ms);
        public delegate void OnOverEndHandler();
        public event OnClickHandler OnClick;
        public event OnOverHandler OnOver;
        public event OnOverEndHandler OnOverEnd;
        #endregion

        #region Constructors
        public Planet(Texture2D texture, Vector2 scale, int radius, float interval, float degrees, Star centerStar, int terraform, Texture2D alphaTexture)
        {
            position = new Vector2();
            Counter = degrees * 10;
            position.X = (float)((centerStar.CenterX) + radius * Math.Cos(Counter / 10 * Math.PI / 180) - ((texture.Width * scale.X) / 2));
            position.Y = (float)((centerStar.CenterY) + radius * Math.Sin(Counter / 10 * Math.PI / 180) - ((texture.Height * scale.Y) / 2));
            Scale = scale;
            FinalWidth = position.X + texture.Width * scale.X;
            FinalHeight = position.Y + texture.Height * scale.Y;
            Radius = radius;
            Interval = interval;
            CenterStar = centerStar;
            this.terraform = terraform;
            Owner = "Unknown";
            Race = "No race";
            Texture = texture;
            Origin = new Vector2(texture.Width / 2, texture.Height / 2);
            Query = new ProductQuery();
            PositionFromCenter = Vector2.Zero;
            IsPressed = false;
            Rotation = 0;
            ShipsOnOrbit = new List<Ship>();
            IsVisible = true;
            IsTerraforming = false;
        }
        public Planet(Texture2D texture, Vector2 scale, int radius, float interval, float degrees, Star centerStar, int terraform)
            : this(texture, scale, radius, interval, degrees, centerStar, terraform, (Texture2D)new object()) { }
        public Planet(Texture2D texture, Vector2 scale, int radius, float interval, float degrees, Star centerStar)
            : this(texture, scale, radius, interval, degrees, centerStar, 0) { }
        public Planet(Texture2D texture, Vector2 scale, int radius, float interval, float degrees)
            : this(texture, scale, radius, interval, degrees, new Star(texture)) { }
        public Planet(Texture2D texture, Vector2 scale, int radius, float interval)
            : this(texture, scale, radius, interval, 0f) { }
        public Planet(Texture2D texture, Vector2 scale, int radius)
            : this(texture, scale, radius, 100) { }
        public Planet(Texture2D texture, Vector2 scale)
            : this(texture, scale, 1000) { }
        public Planet(Texture2D texture)
            : this(texture, new Vector2(1, 1)) { }
        #endregion

        #region Else
        public void Update(GameTime gameTime)
        {
            CoordsUpdate(gameTime);
            EventUpdate();
        }

        public void CoordsUpdate(GameTime gameTime)
        {
            //Code for moving in orbit
            Timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (Timer > Interval)
            {
                Rotation += 3;
                Timer -= Interval;
                Counter++;
                position.X = (float)((CenterStar.CenterX) + Radius * Math.Cos(Counter / 10 * Math.PI / 180) - ((Texture.Width * Scale.X) / 2));
                position.Y = (float)((CenterStar.CenterY) + Radius * Math.Sin(Counter / 10 * Math.PI / 180) - ((Texture.Height * Scale.Y) / 2));
                FinalWidth = position.X + Texture.Width * Scale.X;
                FinalHeight = position.Y + Texture.Height * Scale.Y;
                //Установка относительных координат
                PositionFromCenter = CenterStar.Position - position;
                if (Counter == 3600)
                {
                    Counter = 0;
                }
            }
            //End moving.
        }

        private void EventUpdate()
        {
            MS = Mouse.GetState();
            if (MS.LeftButton == ButtonState.Released && IsPressed == true)
            {
                IsPressed = false;
                if (MS.X <= FinalWidth && MS.Y <= FinalHeight && MS.X > position.X && MS.Y > position.Y)
                {
                    if (OnClick != null)
                    {
                        OnClick(this, MS);
                    }
                }
            }

            if (MS.X <= FinalWidth && MS.Y <= FinalHeight && MS.X > position.X && MS.Y > position.Y)
            {
                Task.Factory.StartNew(Over);
                Click();
            }
            else
            {
                if (IsOver == true)
                {
                    IsOver = false;
                }
            }
        }

        private void Over()
        {
            if (!IsOver)
            {
                Thread.Sleep(300);
                IsOver = true;
                //texture = overTexture;
                if (OnOver != null)
                {
                    OnOver(this, MS);
                }
            }
        }

        private void Click()
        {
            if (MS.LeftButton == ButtonState.Pressed && IsPressed == false)
            {
                //texture = pressTexture;
                IsPressed = true;
                /*if (click != null)
                {
                    Thread t = new Thread(() => { click.Play(); });
                    t.Start();
                    //sound.Start();
                }*/
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Circle, position, Color.Red);
            Radians = Rotation / 10 * (float)Math.PI / 180;
            spriteBatch.Draw(Texture, 
                new Vector2(position.X + (Origin.X * Scale.X), position.Y + (Origin.Y * Scale.Y)), 
                null, Color.White, Radians, Origin, Scale, 0, 0);
            //spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
            /*spriteBatch.Draw(planetTexture, position, null, Color.White, radians, 
                new Vector2(planetTexture.Width / 2, planetTexture.Height / 2), scale, 0, 0);*/
        }
        #endregion
    }
}
