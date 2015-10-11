using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AlphaQuadrant
{
    public interface IDamagable
    {
        int MaxHP { get; }
        int HP { get; set; }
        Vector2 Center { get; }
        bool TakeDamage(int damage);
        event OnDestroyHandler OnDestroy;
    }
}
