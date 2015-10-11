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
    public delegate void OnDestroyHandler(IDamagable sender);

    public partial class AlphaQuadrant : Microsoft.Xna.Framework.Game
    {
        private Dictionary<string, Player> players;
        private string currentPlayer;

        //For Model
        private string currentScreen;
        private Dictionary<string, Screen> screens;
        private Locals locals;
        private Dictionary<string, string> preferences;
        private ShipFactory ShipCreator;
        private StationFactory StationCreator { get; set; }

        /// <summary>
        /// Текстура круга вокруг выделенных кораблей.
        /// </summary>
        private Texture2D circleTexture;

        /// <summary>
        /// Размеры систем.
        /// </summary>
        private Vector2 SystemSize;

        /// <summary>
        /// Координатный центр для систем.
        /// </summary>
        private Vector2 CenterPoint;

        /// <summary>
        /// Хелпрер генератора мира, содержащий примерную инфу о планетах на каждом этапе.
        /// </summary>
        private Dictionary<int, WGPlanetInfo> WGInfoHelper;

        /// <summary>
        /// Словарь с текстурами кружков для звезд на карте. Временно.
        /// </summary>
        private Dictionary<string, Texture2D> StarOnMapCircles;

        //For Events
        private string tempPlanetName;
        private float popupPositionCoef = 0.7f;

        //For Controls
        private MouseState originalMs;
        private bool isEscDown = false;
        private const int MOVEMENT = 10;

        //For Controller
        private float ResourceTimer = 0f;
        private float ResourceInterval = 3000f;

        //For View
        private SpriteBatch spriteBatch;
        private GraphicsDeviceManager graphics;
        private float scaleY;
        private float scaleX;
        private float stepY;
        private float stepX;

        //For World Generator
        private Random r, r2;
        Map map;
        private List<string> names;
        private char[] symbols = { 'A', 'B', 'C', 'D', 'F', 'G', 'J', 'H', 'K', 'L', 'U', 'M', 'N', 'Z' };
    }
}
