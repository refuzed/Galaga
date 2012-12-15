using Microsoft.Xna.Framework;

namespace Galaga.Models
{
    public class GameObject
    {
        private Rectangle[] _spriteRef;
        private Rectangle _hitbox;

        public Rectangle[] SpriteRef
        {
            get { return _spriteRef; }
            set
            {
                _spriteRef = value;
                _hitbox.Width = value[0].Width;
                _hitbox.Height = value[0].Height;
            }
        }

        public Vector2 Position
        {
            get { return new Vector2(_hitbox.X, _hitbox.Y); }
            set
            {
                _hitbox.X = (int)value.X;
                _hitbox.Y = (int)value.Y;
            }
        }

        public Rectangle Hitbox
        {
            get { return _hitbox; }
            set
            {
                _hitbox = value;
            }
        }

        public float Rotation { get; set; }
        public bool IsEnemy { get; set; }
        public bool IsImmune { get; set; }
        public int TickMod { get { return SpriteRef.Length; } }

        public GameObject(Vector2 position, Rectangle[] spriteRef, bool isEnemy, bool isImmune)
        {
            SpriteRef = spriteRef;
            Position = position;
            IsEnemy = isEnemy;
            IsImmune = isImmune;
        }
    }
}
