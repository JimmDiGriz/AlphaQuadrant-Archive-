using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace icsimplelib
{
    [Serializable]
    public class Sprite: IDraw
    {
        #region Fields
        private Texture2D texture;
        private Rectangle rectangle;
        private Vector2 spritePosition;
        private int frameCount; //Count of frames in image;
        private int frame; //current frame;
        private int frameHeight;
        private int frameWidth;
        private float timer; //How much time shoud show one frame;
        //need to realize property;
        private float interval; //how much time run away from showing last frame;
        #endregion

        #region Properties
        public float X
        {
            get { return spritePosition.X; }
            set
            {
                spritePosition.X = value;
            }
        }

        public float Y
        {
            get { return spritePosition.Y; }
            set
            {
                spritePosition.Y = value;
            }
        }

        public bool IsVisible { get; set; }
        #endregion

        #region Constructors
        public Sprite(Texture2D texture, Vector2 spritePosition, int frameWidth, int frameCount, int interval)
        {
            this.texture = texture;
            this.spritePosition = spritePosition;
            this.frameHeight = texture.Height;
            this.frameWidth = frameWidth;
            this.frameCount = frameCount;
            this.interval = interval;
            frame = 0;
            IsVisible = true;
        }

        public Sprite(Texture2D texture, Vector2 spritePosition, int frameWidth, int frameCount)
            : this(texture, spritePosition, frameWidth, frameCount, 50) { }
        #endregion

        public void Play(GameTime gameTime)
        {
            rectangle = new Rectangle(frame * frameWidth, 0, frameWidth, frameHeight);
            timer += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 2;
            if (timer > interval)
            {
                frame++;
                timer -= interval;
                if (frame > frameCount - 1)
                    frame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(texture, spritePosition, rectangle, Color.White);
            }
        }
    }
}
