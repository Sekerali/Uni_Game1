using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game3
{
    class EnemyBossCore : Entity
    {
        public int health = 60;

        public EnemyBossCore(Vector3 position)
        {
            Model = Game1.BossCore;
            diffuseColor = Color.Gray.ToVector3();
            ambientLightColor = Color.White.ToVector3();
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
            if (health <= 20)
            {
                ambientLightColor = Color.Red.ToVector3();
            }
            else if (health <= 40)
            {
                ambientLightColor = Color.Yellow.ToVector3();
            }
        }

    }
}
