using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Galaga.Models
{
    public class GameState
    {
        public const float SizeMod = 2.5f;
        public const int MaxX = 630;
        public const int MaxY = 810;
        public readonly Vector2 ShipSpeed = new Vector2(2.5f, 0);
        public readonly Vector2 BulletSpeed = new Vector2(0, 2.5f);
        public readonly TimeSpan BulletDelay = new TimeSpan(0,0,0,0,500);
        public DateTime LastBullet { get; set; }
        

        public bool Paused { get; set; }
        public bool ShowStats { get; set; }

        public SpriteFont Font { get; set; }

        public KeyboardState CurrentKeyState { get; set; }
        public KeyboardState PreviousKeyState { get; set; }

        public GameState()
        {
            PreviousKeyState = Keyboard.GetState();
        }
    }
}