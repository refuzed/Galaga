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
        public readonly Vector2 BulletSpeed = new Vector2(0, 4.5f);
        public readonly TimeSpan BulletDelay = new TimeSpan(0,0,0,0,500);

        public uint[] RibbonCount;
        public static readonly uint[] RibbonValue = new uint[] {1, 5, 10, 25, 50, 100};

        public DateTime LastBullet { get; set; }
        public int BulletCount { get; set; }
        public bool DualShips { get; set; }

        public DateTime StageInfoStartTime { get; set; }

        public bool ShowStartMenu { get; set; }
        public bool ShowCredits { get; set; }
        public bool BlinkPlayer { get; set; }
        public bool ShowStageInfo { get; set; }
        public bool ShowLivesAndLevel { get; set; }
        public bool InPlay { get; set; }

        public uint Credits { get; set; }
        public uint HighScore { get; set; }
        public uint CurrentPlayer { get; set; }
        public uint CurrentStage { get; set; }
        public uint[] PlayerLives { get; set; }
        public uint[] PlayerScore { get; set; }

        public SpriteFont Font { get; set; }

        public KeyboardState CurrentKeyState { get; set; }
        public KeyboardState PreviousKeyState { get; set; }

        public GameState()
        {
            PreviousKeyState = Keyboard.GetState();

            ShowStartMenu = true;
            ShowCredits = true;

            HighScore = 20000;
            Credits = 1;
            CurrentPlayer = 1;
            CurrentStage = 1;
            PlayerLives = new uint[] { 2,2 };
            PlayerScore = new uint[] { 0,0 };
        }
    }
}