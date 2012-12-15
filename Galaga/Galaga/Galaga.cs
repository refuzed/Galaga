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

        private SoundEffect _musicTest;

        private GameObject _ship;

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
            _musicTest = Content.Load<SoundEffect>("Sounds/galaga9");

            _ship = new GameObject(new Vector2(GameState.MaxX / 2,GameState.MaxY-100), GraphicSource.PlayerShipWhite, false, false);

            _gameObjects.Add(_ship);
            _gameObjects.Add(new GameObject(new Vector2(GameState.MaxX / 2, 400), GraphicSource.EnemyLv1, true, false));
            _musicTest.Play();
        }


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            _gameState.CurrentKeyState = Keyboard.GetState();

            UpdateShip();
            UpdateBullets();

            _gameState.PreviousKeyState = _gameState.CurrentKeyState;

            base.Update(gameTime);
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
                if (DateTime.Now - _gameState.LastBullet > _gameState.BulletDelay)
                {
                    _gameObjects.Add(new GameObject(_ship.Position, GraphicSource.RedBullet, false, true));
                    _gameState.LastBullet = DateTime.Now;
                }
            }
        }

        private void UpdateBullets()
        {
            var ash = new List<GameObject>();
            var trash = new List<GameObject>();

            foreach(var b in _gameObjects.Where(x=>x.SpriteRef==GraphicSource.RedBullet))
            {
                b.Position += b.IsEnemy ? _gameState.BulletSpeed : -_gameState.BulletSpeed;

                foreach(var e in _gameObjects.Where(x=>!x.IsImmune))
                {
                    // get rid of enemy and bullet if we hit a badguy
                    if(b.Hitbox.Intersects(e.Hitbox) && b.IsEnemy != e.IsEnemy)
                    {
                        ash.Add(new GameObject(b.Position, GraphicSource.PlayerExplode, false, true));
                        trash.Add(e);
                        trash.Add(b);
                    }
                }
                // Get rid of bullet if it goes off screen
                if(b.Position.Y < 0)
                {
                    trash.Add(b); 
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (var o in _gameObjects)
            {
                _spriteBatch.Draw(_spriteSheet, o.Position, o.SpriteRef[_tick % o.TickMod], Color.White, o.Rotation,
                              new Vector2(o.SpriteRef[0].Width / 2f, o.SpriteRef[0].Height / 2f), GameState.SizeMod, SpriteEffects.None, 0);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
