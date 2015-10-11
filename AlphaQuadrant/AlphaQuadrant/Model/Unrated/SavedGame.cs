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

namespace AlphaQuadrant
{
    [Serializable]
    public class SavedGame
    {
        public Dictionary<string, Player> Players { get; private set; }
        public string CurrentPlayer { get; private set; }
        public string CurrentScreen { get; private set; }
        public Dictionary<string, Screen> Screens { get; private set; }

        public SavedGame(Dictionary<string, Player> players, Dictionary<string, Screen> screens, string currentPlayer, string currentScreen)
        {
            Players = players;
            CurrentPlayer = currentPlayer;
            CurrentScreen = currentScreen;
            Screens = screens;
        }
    }
}
