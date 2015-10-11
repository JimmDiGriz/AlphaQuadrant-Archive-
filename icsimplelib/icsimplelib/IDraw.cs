using System;
using Microsoft.Xna.Framework.Graphics;

namespace icsimplelib
{
    public interface IDraw
    {
        bool IsVisible { get; set; }
        void Draw(SpriteBatch spriteBatch);
    }
}
