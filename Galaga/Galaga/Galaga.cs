using System;
using System.Collections.Generic;
using System.Linq;
using Galaga.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Galaga
{

    public class Galaga : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private List<GameObject> _gameObjects;
        private GameState _gameState;
        private SpriteBatch _spriteBatch;
        private Texture2D _spriteSheet;

        private SoundEffect _introMusic;

        private GameObject _ship;
        private GameObject _bitchShip;
        private GameObject _bitchRibbon;

        private int _tick { get { return DateTime.Now.Second; } }

        public Galaga()
        {
            _graphics = new GraphicsDeviceManager(this)
                {
                    IsFullScreen = false,
                    PreferredBackBufferWidth = GameState.MaxX,
                    PreferredBackBufferHeight = GameState.MaxY
                };

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            _gameState = new GameState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _gameObjects = new List<GameObject>();
            _gameState = new GameState();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _spriteSheet = Content.Load<Texture2D>("GalagaSheet");
            _introMusic = Content.Load<SoundEffect>("Sounds/galaga9");
            _gameState.Font = Content.Load<SpriteFont>("Default");

            _ship = new GameObject(new Vector2(GameState.MaxX / 2,GameState.MaxY-100), GraphicSource.PlayerShipWhite, false, false);
            _bitchShip = new GameObject(new Vector2(GameState.MaxX/2, GameState.MaxY - 100), GraphicSource.PlayerShipWhite, false, false);
            _bitchRibbon = new GameObject(new Vector2(GameState.MaxX - 25, GameState.MaxY - 26), GraphicSource.LevelRibbons, false, false);
        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            _gameState.CurrentKeyState = Keyboard.GetState();

            CheckMiscKeys();

            if (_gameState.ShowStageInfo)
            {
                UpdateStageInfo();
            }

            if(_gameState.InPlay)
            {
                UpdateShip();
                UpdateBullets();
            }

            _gameState.PreviousKeyState = _gameState.CurrentKeyState;

            base.Update(gameTime);
        }

        private void CheckMiscKeys()
        {
            if (_gameState.ShowStartMenu && _gameState.CurrentKeyState.IsKeyDown(Keys.Space))
            {
                StartGame();
            }
            if (_gameState.CurrentKeyState.IsKeyDown(Keys.A) && _gameState.PreviousKeyState.IsKeyUp(Keys.A))
            {
                _gameState.CurrentStage++;
                UpdateRibbons();
            }
            if (_gameState.CurrentKeyState.IsKeyDown(Keys.Z) && _gameState.PreviousKeyState.IsKeyUp(Keys.Z))
            {
                _gameState.CurrentStage--;
                UpdateRibbons();
            }
        }

        private void StartGame()
        {
            _gameState.ShowStartMenu = false;
            _gameState.ShowStageInfo = true;
            _gameState.StageInfoStartTime = DateTime.Now;
            _gameState.BlinkPlayer = true;
            _gameState.Credits--;
            //_introMusic.Play();  // I CANT TAKE IT ANYMORE!!
            UpdateRibbons();
        }

        private void UpdateStageInfo()
        {
            TimeSpan timeSpan = DateTime.Now - _gameState.StageInfoStartTime;

            if (timeSpan > new TimeSpan(0, 0, 0, 8))
            {
                _gameState.ShowStageInfo = false;
                _gameState.InPlay = true;
                LetTheFightBegin();
            } 
            else if (timeSpan > new TimeSpan(0, 0, 0, 6))
            {
                if (!_gameObjects.Contains(_ship))
                    _gameObjects.Add(_ship);
            }
            else if (timeSpan > new TimeSpan(0, 0, 0, 4))
            {
                _gameState.ShowCredits = false;
                _gameState.ShowLivesAndLevel = true;
            }
        }

        private void LetTheFightBegin()
        {
            _gameObjects.Add(new GameObject(new Vector2(GameState.MaxX / 2, 400), GraphicSource.EnemyLv1, true, false));
        }

        private void UpdateShip()
        {
            // left move key and bounding for ship
            if (_gameState.CurrentKeyState.IsKeyDown(Keys.Left) &&
                _ship.Position.X > 0 + _ship.Hitbox.Width + 20)
            {
                _ship.Position -= _gameState.ShipSpeed;
            }
            // right move key and bounding for ship
            if (_gameState.CurrentKeyState.IsKeyDown(Keys.Right) &&
                _ship.Position.X < GameState.MaxX - _ship.Hitbox.Width - 20)
            {
                _ship.Position += _gameState.ShipSpeed;
            }
            // pew pew!
            if(_gameState.CurrentKeyState.IsKeyDown(Keys.Space))
            {
                if (DateTime.Now - _gameState.LastBullet > _gameState.BulletDelay && _gameState.BulletCount < 2)
                {
                    _gameObjects.Add(new GameObject(_ship.Position, GraphicSource.BlueBullet, false, true));
                    _gameState.LastBullet = DateTime.Now;
                    _gameState.BulletCount++;
                }
            }
        }

        private void UpdateBullets()
        {
            var ash = new List<GameObject>();
            var trash = new List<GameObject>();

            foreach(var bullet in _gameObjects.Where(x=>x.SpriteRef==GraphicSource.BlueBullet))
            {
                bullet.Position += bullet.IsEnemy ? _gameState.BulletSpeed : -_gameState.BulletSpeed;

                foreach(var enemy in _gameObjects.Where(x=>!x.IsImmune))
                {
                    // get rid of enemy and bullet if we hit a badguy
                    if(bullet.Hitbox.Intersects(enemy.Hitbox) && bullet.IsEnemy != enemy.IsEnemy)
                    {
                        ash.Add(new GameObject(bullet.Position, GraphicSource.EnemyExlode, false, true));
                        trash.Add(enemy);
                        trash.Add(bullet);
                        _gameState.BulletCount--;
                    }
                }
                // Get rid of bullet if it goes off screen
                if(bullet.Position.Y < 0)
                {
                    trash.Add(bullet);
                    _gameState.BulletCount--;
                }
            }

            foreach (var a in ash)
            {
                _gameObjects.Add(a);
            }

            foreach(var t in trash)
            {
                _gameObjects.Remove(t);
            }
        }

        private void UpdateRibbons()
        {
            var ribbonPoints = _gameState.CurrentStage;

            _gameState.RibbonCount = new uint[6];

            for (int r = _gameState.RibbonCount.Count() - 1; r >= 0; r--)
            {
                _gameState.RibbonCount[r] = CountRibbon(ribbonPoints, GameState.RibbonValue[r]);
                ribbonPoints -= _gameState.RibbonCount[r] * GameState.RibbonValue[r];
            }
        }

        private uint CountRibbon(uint ribbonPoints, uint ribbonValue)
        {
            uint count = 0;
            uint current = ribbonPoints;

            while (current >= ribbonValue)
            {
                count++;
                current -= ribbonValue;
            }
            return count;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (var o in _gameObjects)
            {
                _spriteBatch.Draw(_spriteSheet, o.Position, o.SpriteRef[_tick % o.TickMod], Color.White, o.Rotation,
                              new Vector2(o.SpriteRef[0].Width / 2f, o.SpriteRef[0].Height / 2f), GameState.SizeMod, SpriteEffects.None, 0);
            }

            DrawUI();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawUI()
        {
            DrawScore();

            //TODO: Show Idle Screen

            if (_gameState.ShowStartMenu)
                DrawStartMenu();

            if (_gameState.ShowStageInfo)
                DrawStageInfo();

            if (_gameState.ShowCredits)
                DrawCredits();

            if (_gameState.ShowLivesAndLevel)
                DrawLivesAndLevel();
        }

        private void DrawScore()
        {
            _spriteBatch.DrawString(_gameState.Font, "HIGH SCORE", new Vector2(225, 5), Color.Red);
            _spriteBatch.DrawString(_gameState.Font, _gameState.HighScore.ToString(), new Vector2(270, 25), Color.White);

            if (_gameState.BlinkPlayer && _tick % 2 == 0 || !_gameState.BlinkPlayer)
                _spriteBatch.DrawString(_gameState.Font, String.Format("{0}UP", _gameState.CurrentPlayer), new Vector2(35, 5), Color.Red);

            if(!_gameState.ShowStartMenu)
            {
                var score = _gameState.PlayerScore[_gameState.CurrentPlayer];
                _spriteBatch.DrawString(_gameState.Font, score == 0 ? "00" : score.ToString(), new Vector2(35, 25), Color.White);
            }
        }

        private void DrawStartMenu()
        {
            _spriteBatch.DrawString(_gameState.Font, "PRESS SPACEBAR TO START", new Vector2(100, 275), Color.Aqua);

            _bitchShip.Position = new Vector2(50,380);
            _spriteBatch.DrawString(_gameState.Font, "1ST BONUS FOR 30000 PTS", new Vector2(100, 375), Color.Gold);
            _spriteBatch.Draw(_spriteSheet, _bitchShip.Position, _bitchShip.SpriteRef[_tick % _bitchShip.TickMod], Color.White, _bitchShip.Rotation,
                              new Vector2(_bitchShip.SpriteRef[0].Width / 2f, _bitchShip.SpriteRef[0].Height / 2f), GameState.SizeMod, SpriteEffects.None, 0);            
            
            _bitchShip.Position = new Vector2(50,455);
            _spriteBatch.DrawString(_gameState.Font, "2ND BONUS FOR 120000 PTS", new Vector2(100, 450), Color.Gold);
            _spriteBatch.Draw(_spriteSheet, _bitchShip.Position, _bitchShip.SpriteRef[_tick % _bitchShip.TickMod], Color.White, _bitchShip.Rotation,
                              new Vector2(_bitchShip.SpriteRef[0].Width / 2f, _bitchShip.SpriteRef[0].Height / 2f), GameState.SizeMod, SpriteEffects.None, 0);      
            
            _bitchShip.Position = new Vector2(50,530);
            _spriteBatch.DrawString(_gameState.Font, "AND FOR EVERY 120000 PTS", new Vector2(100, 525), Color.Gold);
            _spriteBatch.Draw(_spriteSheet, _bitchShip.Position, _bitchShip.SpriteRef[_tick % _bitchShip.TickMod], Color.White, _bitchShip.Rotation,
                              new Vector2(_bitchShip.SpriteRef[0].Width / 2f, _bitchShip.SpriteRef[0].Height / 2f), GameState.SizeMod, SpriteEffects.None, 0);  
        }

        private void DrawStageInfo()
        {
            TimeSpan timeSpan = DateTime.Now - _gameState.StageInfoStartTime;

            if (timeSpan < new TimeSpan(0, 0, 0, 4))
            {
                _spriteBatch.DrawString(_gameState.Font, String.Format("PLAYER {0}", _gameState.CurrentPlayer), new Vector2(250, 400), Color.Aqua);
            } else if (timeSpan < new TimeSpan(0, 0, 0, 6))
            {
                _spriteBatch.DrawString(_gameState.Font, String.Format("STAGE {0}", _gameState.CurrentStage), new Vector2(250, 400), Color.Aqua);
            }
            else
            {
                _spriteBatch.DrawString(_gameState.Font, String.Format("PLAYER {0}", _gameState.CurrentPlayer), new Vector2(250, 370), Color.Aqua);
                _spriteBatch.DrawString(_gameState.Font, String.Format("STAGE {0}", _gameState.CurrentStage), new Vector2(250, 400), Color.Aqua);
            }
        }

        private void DrawCredits()
        {
            _spriteBatch.DrawString(_gameState.Font, String.Format("CREDIT {0}", _gameState.Credits), new Vector2(25, 790), Color.White);
        }

        private void DrawLivesAndLevel()
        {
            // Draw Lives
            _bitchShip.Position = new Vector2(25, GameState.MaxY - 26);
            for (int i = 0; i < _gameState.PlayerLives[_gameState.CurrentPlayer]; i++)
            {
                _spriteBatch.Draw(_spriteSheet, _bitchShip.Position, _bitchShip.SpriteRef[_tick % _bitchShip.TickMod], Color.White, _bitchShip.Rotation,
                              new Vector2(_bitchShip.SpriteRef[0].Width / 2f, _bitchShip.SpriteRef[0].Height / 2f), GameState.SizeMod, SpriteEffects.None, 0);
                _bitchShip.Position += new Vector2(45, 0);
            }

            // Draw Ribbons
            _bitchRibbon.Position = new Vector2(GameState.MaxX, GameState.MaxY - 26);
            for (int r = 0; r < _gameState.RibbonCount.Count(); r++)
            {
                for (int i = 0; i < _gameState.RibbonCount[r]; i++)
                {
                    _bitchRibbon.Position += new Vector2(-_bitchRibbon.SpriteRef[r].Width*GameState.SizeMod, 0);
                    _spriteBatch.Draw(_spriteSheet, _bitchRibbon.Position, _bitchRibbon.SpriteRef[r], Color.White,
                                      _bitchRibbon.Rotation, new Vector2(_bitchRibbon.SpriteRef[r].Width/2f,
                                      _bitchRibbon.SpriteRef[r].Height/2f), GameState.SizeMod, SpriteEffects.None, 0);
                }
            }
        }
    }
}
