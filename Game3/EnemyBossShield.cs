using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game3
{
    class EnemyBossShield: Entity
    {
        public int health = 20;

        public EnemyBossShield(Vector3 position)
        {
            Model = Game1.BossShield;
            diffuseColor = Color.Gray.ToVector3();
            ambientLightColor = Color.Black.ToVector3();
            Position = position;
            Orientation = (float)Math.PI * 90 / 180;
            AngleY = 0;
            AngleX = 0;
            Radius = 6;
            Scale = 10;
            IsExpired = false;
        }

        public override void Update(GameTime gameTime)
        {

        }

    }
}
