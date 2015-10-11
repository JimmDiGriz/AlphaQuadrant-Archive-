using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace icsimplelib
{
    [Serializable]
    public class TextBox: IDraw, IMoveble, IScaleble, IUpdateble
    {
        #region Fields
        private Texture2D texture;
        private Texture2D activeTexture;
        private Texture2D overTexture;
        private Texture2D staticTexture;
        private GameString content;
        private bool isMultiline = false;
        private bool isReadOnly = false;
        private Vector2 position;
        private Vector2 scale;
        private float finalWidth;
        private float finalHeight;
        private MouseState ms;
        private KeyboardState state;
        private bool isPressed = false;
        private bool isActive = false;
        private bool isKeyDown = false;
        private string lastString;
        #endregion

        #region Properties
        public bool IsMultiline
        {
            get { return isMultiline; }
            set
            {
                isMultiline = value;
            }
        }

        public bool IsReadOnly
        {
            get { return isReadOnly; }
            set
            {
                isReadOnly = value;
            }
        }

        public Vector2 Scale
        {
            get { return scale; }
            set
            {
                scale = value;
            }
        }

        public float X
        {
            get { return position.X; }
            set
            {
                position.X = value;
                finalWidth = position.X + texture.Width * scale.X;
            }
        }

        public float Y
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                finalHeight = position.Y + texture.Height * scale.Y;
            }
        }

        public string Content
        {
            get { return content.Str; }
            set
            {
                content.Str = value;
            }
        }

        public GameString ContentSource
        { 
            get { return content; }
            set
            {
                content = value;
            }
        }

        public Color TextColor
        {
            get { return content.Color; }
            set
            {
                content.Color = value;
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

        public bool IsActive
        {
            get { return isActive; }
        }
        public float Width
        {
            get { return texture.Width * scale.X; }
        }
        public float Height
        {
            get { return texture.Height * scale.Y; }
        }

        public bool IsVisible { get; set; }
        #endregion

        #region Events
        public delegate void OnEnterHandler(TextBox sender);
        public event OnEnterHandler OnEnter;
        #endregion

        #region Costructors
        public TextBox(Texture2D staticTexture, Texture2D overTexture, Texture2D activeTexture, SpriteFont font, Vector2 position, Vector2 scale)
        {
            texture = this.staticTexture = staticTexture;
            this.overTexture = overTexture;
            this.activeTexture = activeTexture;
            this.position = position;
            this.scale = scale;
            Vector2 contentPosition = new Vector2(X + 15, Y + (Height/2) - 12);
            this.content = new GameString(font, "", contentPosition, Color.Azure);
            finalHeight = position.Y + texture.Height * scale.Y;
            finalWidth = position.X + texture.Width * scale.X;
            lastString = "";
            IsVisible = true;
        }

        /*public TextBox(Texture2D texture, SpriteFont font, Vector2 position)
            : this(texture, font, position, new Vector2(1, 1)) { }
        public TextBox(Texture2D texture, SpriteFont font)
            : this(texture, font, Vector2.Zero) { }*/
        #endregion

        #region Else
        public void Update(GameTime gameTime)
        {
            ms = Mouse.GetState();
            if (ms.LeftButton == ButtonState.Released)
            {
                isPressed = false;
            }
            if (Keyboard.GetState().GetPressedKeys().Length == 0)
            {
                isKeyDown = false;
            }
            if (ms.X <= finalWidth && ms.Y <= finalHeight && ms.X > position.X && ms.Y > position.Y)
            {
                Over();
            }
            else
            {
                UnfocusClick();
            }
            if (isActive == true && isKeyDown == false)
            {
                state = Keyboard.GetState();
                char a = GetChar();
                if (a != '^')
                {
                    isKeyDown = true;
                    if (content.Font.MeasureString(lastString).X >= ((texture.Width * scale.X) - 30) && a != '`')
                    {
                        if (isMultiline == true)
                        {
                            content.Str += '\n';
                            lastString = "";
                        }
                        else
                        {
                            return;
                        }
                    }
                    if (a == '`')
                    {
                        if (content.Str.Length != 0)
                        {
                            content.Str = content.Str.Substring(0, content.Str.Length - 1);
                        }
                        if (lastString.Length != 0)
                        {
                            lastString = lastString.Substring(0, lastString.Length - 1);
                        }
                    }
                    else
                    {
                        content.Str += a;
                        lastString += a;
                    }
                }
            }
                
        }

        private void Over()
        {
            if (isActive == false)
            {
                texture = overTexture;
            }
            Click();
        }

        private void Click()
        {
            if (ms.LeftButton == ButtonState.Pressed && isPressed == false && isActive == false)
            {
                isActive = true;
                isPressed = true;
                texture = activeTexture;
            }
        }

        private void UnfocusClick()
        {
            if (ms.LeftButton == ButtonState.Pressed && isPressed == false && isActive == true)
            {
                isActive = false;
                texture = staticTexture;
            }
            if (isActive == false)
            {
                texture = staticTexture;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, scale, 0, 0);
                content.Draw(spriteBatch);
            }
        }

        private char GetChar()
        {
            /*
             * I hope i dont start eating carry before this.
             */
            if (state.IsKeyDown(Keys.A))
            {
                return 'a';           
            }
            else if (state.IsKeyDown(Keys.B))
            {
                return 'b';        
            }
            else if (state.IsKeyDown(Keys.C))
            {
                return 'c';
            }
            else if (state.IsKeyDown(Keys.D))
            {
                return 'd';
            }
            else if (state.IsKeyDown(Keys.E))
            {
                return 'e';
            }
            else if (state.IsKeyDown(Keys.F))
            {
                return 'f';
            }
            else if (state.IsKeyDown(Keys.G))
            {
                return 'g';
            }
            else if (state.IsKeyDown(Keys.H))
            {
                return 'h';
            }
            else if (state.IsKeyDown(Keys.I))
            {
                return 'i';
            }
            else if (state.IsKeyDown(Keys.J))
            {
                return 'j';
            }
            else if (state.IsKeyDown(Keys.K))
            {
                return 'k';
            }
            else if (state.IsKeyDown(Keys.L))
            {
                return 'l';
            }
            else if (state.IsKeyDown(Keys.M))
            {
                return 'm';
            }
            else if (state.IsKeyDown(Keys.N))
            {
                return 'n';
            }
            else if (state.IsKeyDown(Keys.O))
            {
                return 'o';
            }
            else if (state.IsKeyDown(Keys.P))
            {
                return 'p';
            }
            else if (state.IsKeyDown(Keys.Q))
            {
                return 'q';
            }
            else if (state.IsKeyDown(Keys.R))
            {
                return 'r';
            }
            else if (state.IsKeyDown(Keys.S))
            {
                return 's';
            }
            else if (state.IsKeyDown(Keys.T))
            {
                return 't';
            }
            else if (state.IsKeyDown(Keys.U))
            {
                return 'u';
            }
            else if (state.IsKeyDown(Keys.V))
            {
                return 'v';
            }
            else if (state.IsKeyDown(Keys.W))
            {
                return 'w';
            }
            else if (state.IsKeyDown(Keys.X))
            {
                return 'x';
            }
            else if (state.IsKeyDown(Keys.Y))
            {
                return 'y';
            }
            else if (state.IsKeyDown(Keys.Z))
            {
                return 'z';
            }
            else if (state.IsKeyDown(Keys.Space))
            {
                return ' ';
            }
            else if (state.IsKeyDown(Keys.Back))
            { 
                return '`';
            }
            else if (state.IsKeyDown(Keys.Enter))
            {
                isActive = false;
                if (OnEnter != null)
                {
                    OnEnter(this);
                }
                return '^';
            }
            else
            {
                return '^';
            }
        }
        #endregion
    }
}
