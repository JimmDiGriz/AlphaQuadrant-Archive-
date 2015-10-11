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
using System.Diagnostics;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AlphaQuadrant
{
    public partial class AlphaQuadrant : Microsoft.Xna.Framework.Game
    {
        private bool SaveGame()
        {
            SavedGame save = new SavedGame(players, screens, currentPlayer, currentScreen);
            using (FileStream stream = new FileStream("save.bin", FileMode.OpenOrCreate))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(stream, save);
                return true;
            }
        }

        private bool LoadGame()
        {
            return false;
        }
    }
}
