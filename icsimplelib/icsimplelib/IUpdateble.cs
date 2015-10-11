using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace icsimplelib
{
    public interface IUpdateble
    {
        void Update(GameTime gameTime = null);
    }
}
