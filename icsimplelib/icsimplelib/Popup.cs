using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace icsimplelib
{
    [Serializable]
    public class Popup : IDraw, IMoveble, IUpdateble
    {
        #region Fields
        private Vector2 position;
        private Vector2 scale;
        private Dictionary<string, IDraw> objects;
        private float finalWidth;
        private float finalHeight;
        private float width; //equals background image width;
        private float height; //equals background image height;
        private Dictionary<string, Vector2> coord;
        private bool isVisible;
        private bool isDragable;
        private MouseState ms;
        #endregion

        #region Properties
        public Dictionary<string, IDraw> Objects
        {
            get{ return objects; }
            set
            {
                foreach (KeyValuePair<string, IDraw> kvp in value)
                {
                    if (kvp.Value is BackGround)
                    {
                        width = ((BackGround)kvp.Value).Width;
                        height = ((BackGround)kvp.Value).Height;
                        finalHeight = position.Y + height * scale.Y;
                        finalWidth = position.X + width * scale.X;
                    }
                    objects.Add(kvp.Key, kvp.Value);
                }
            }
        }

        public bool IsDragable
        {
            get { return isDragable; }
            set
            {
                isDragable = value;
            }
        }

        public bool IsVisible
        {
            get { return isVisible; }
            set 
            {
                isVisible = value;
                ChangeCoordinates();
            }
        }

        public float X
        {
            get { return position.X; }
            set
            {
                if (value > 0)
                {
                    position.X = value;
                }
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                if (value > 0)
                {
                    position.Y = value;
                }
            }
        }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
            }
        }
        public float Width
        {
            get { return width; }
        }
        public float Height
        {
            get { return height; }
        }
        #endregion

        #region Events
        #endregion

        #region Constructors
        public Popup(Vector2 position, Vector2 scale, float width, float height, bool isVisible, bool isDragable)
        {
            IsDragable = isDragable;
            this.isVisible = isVisible;
            objects = new Dictionary<string, IDraw>();
            coord = new Dictionary<string, Vector2>();
            this.position = position;
            this.scale = scale;
            this.width = width;
            this.height = height;
            finalHeight = position.Y + height * scale.Y;
            finalWidth = position.X + width * scale.X;
        }
        public Popup(Vector2 position, Vector2 scale, float width, float height, bool isVisible)
            : this(position, scale, width, height, isVisible, false) { }
        public Popup(Vector2 position, Vector2 scale, float width, float height)
            : this(position, scale, width, height, false) { }
        #endregion

        #region Else
        private void ChangeCoordinates()
        {
            SetCoord();
            foreach (KeyValuePair<string, IDraw> kvp in objects)
            {
                ((IMoveble)kvp.Value).X = coord[kvp.Key].X + position.X;
                ((IMoveble)kvp.Value).Y = coord[kvp.Key].Y + position.Y;
                if (kvp.Value is Button)
                { 
                    if (((Button)kvp.Value).Str != null)
                    {
                        ((IMoveble)((Button)kvp.Value).Str).X = coord[kvp.Key + "Str"].X + position.X;
                        ((IMoveble)((Button)kvp.Value).Str).Y = coord[kvp.Key + "Str"].Y + position.Y;
                    }
                }

                if (kvp.Value is TextBox)
                {
                    /*((IMoveble)((TextBox)kvp.Value).ContentSource).X = coord[kvp.Key + "Str"].X + position.X;
                    ((IMoveble)((TextBox)kvp.Value).ContentSource).Y = coord[kvp.Key + "Str"].Y + position.Y;*/
                    ((IMoveble)((TextBox)kvp.Value).ContentSource).X = coord[kvp.Key].X + 15 + position.X;
                    ((IMoveble)((TextBox)kvp.Value).ContentSource).Y = coord[kvp.Key].Y + (((TextBox)kvp.Value).Height/2) - 12 + position.Y;
                }
            }
            finalHeight = position.Y + height * scale.Y;
            finalWidth = position.X + width * scale.X;
        }

        private void SetCoord()
        {
            try
            {
                foreach (KeyValuePair<string, IDraw> kvp in objects)
                {
                    if (!coord.ContainsKey(kvp.Key))
                    {
                        coord.Add(kvp.Key, new Vector2(((IMoveble)kvp.Value).X, ((IMoveble)kvp.Value).Y));
                        if (kvp.Value is Button)
                        {
                            if (((Button)kvp.Value).Str != null)
                            {
                                coord.Add(kvp.Key + "Str", new Vector2(((IMoveble)((Button)kvp.Value).Str).X, ((IMoveble)((Button)kvp.Value).Str).Y));
                            }
                        }

                        if (kvp.Value is TextBox)
                        {
                            coord.Add(kvp.Key + "Str", new Vector2(((IMoveble)((TextBox)kvp.Value).ContentSource).X, ((IMoveble)((TextBox)kvp.Value).ContentSource).Y));
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (isVisible)
            {
                foreach (KeyValuePair<string, IDraw> kvp in objects)
                {
                    if (kvp.Value is Sprite)
                    {
                        Sprite temp = (Sprite)kvp.Value;
                        temp.Play(gameTime);
                    }
                    else if (kvp.Value is Button)
                    {
                        Button temp = (Button)kvp.Value;
                        temp.Update(gameTime);
                    }
                }
                if (isDragable)
                {
                    ms = Mouse.GetState();
                    if (ms.X <= finalWidth && ms.Y <= finalHeight && ms.X > position.X && ms.Y > position.Y)
                    {
                        if (ms.LeftButton == ButtonState.Pressed)
                        {
                            position.X = ms.X-((width*scale.X)/2);
                            position.Y = ms.Y-((height*scale.Y)/2);
                            ChangeCoordinates();
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                foreach (KeyValuePair<string, IDraw> drawable in objects)
                {
                    drawable.Value.Draw(spriteBatch);
                }
            }
        }
        #endregion
    }
}
