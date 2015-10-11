using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace icsimplelib
{
    public interface IMoveble
    {
        float X { get; set; }
        float Y { get; set; }
        float Width { get; }
        float Height { get; }
        Vector2 Position { get; set; }
    }
}
