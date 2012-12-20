using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaga.Models
{
    public static class GraphicSource
    {
        public static Rectangle[] PlayerShipWhite = new[] { new Rectangle(160, 55, 16, 16), new Rectangle(184, 55, 16, 16) };
        public static Rectangle[] PlayerShipRed = new[] { new Rectangle(160, 79, 16, 16), new Rectangle(184, 79, 16, 16) };

        public static Rectangle[] PlayerExplode = new[] { new Rectangle(208, 47, 32, 32), 
                                                          new Rectangle(247, 47, 32, 32),
                                                          new Rectangle(288, 47, 32, 32), 
                                                          new Rectangle(327, 47, 32, 32)};

        public static Rectangle[] TractorBeam = new[] { new Rectangle(209,103, 46, 80), 
                                                        new Rectangle(265,103, 46, 80),
                                                        new Rectangle(321,103, 46, 80)};

        public static Rectangle[] RedBullet = new[] { new Rectangle(366, 195, 3, 8) };
        public static Rectangle[] BlueBullet = new[] { new Rectangle(374, 50, 3, 8) };

        public static Rectangle[] EnemyLv1 = new[] { new Rectangle(160, 103, 16, 16), new Rectangle(184, 103, 16, 16) };
        public static Rectangle[] EnemyLv2 = new[] { new Rectangle(160, 127, 16, 16), new Rectangle(184, 127, 16, 16) };
        public static Rectangle[] EnemyLv3 = new[] { new Rectangle(160, 151, 16, 16), new Rectangle(184, 151, 16, 16) };
        public static Rectangle[] EnemyLv4 = new[] { new Rectangle(160, 175, 16, 16), new Rectangle(184, 175, 16, 16) };
        public static Rectangle[] EnemyLv5 = new[] { new Rectangle(160, 199, 16, 16), new Rectangle(160, 199, 16, 16) };
        public static Rectangle[] EnemyLv6 = new[] { new Rectangle(160, 223, 16, 16), new Rectangle(160, 199, 16, 16) };
        public static Rectangle[] EnemyLv7 = new[] { new Rectangle(160, 247, 16, 16), new Rectangle(160, 199, 16, 16) };
        public static Rectangle[] EnemyLv8 = new[] { new Rectangle(160, 271, 16, 16), new Rectangle(160, 271, 16, 16) };

        public static Rectangle[] EnemyExlode = new[] {new Rectangle(206, 199, 16, 16),
                                                       new Rectangle(232, 199, 16, 16),
                                                       new Rectangle(256, 199, 16, 16)};

        public static Rectangle[] LevelRibbons = new[] { new Rectangle(372, 290, 16, 16),
                                                         new Rectangle(356, 288, 16, 16),
                                                         new Rectangle(336, 288, 16, 16),
                                                         new Rectangle(312, 287, 16, 16),
                                                         new Rectangle(288, 288, 16, 16),
                                                         new Rectangle(264, 288, 16, 16)};

    }
}
