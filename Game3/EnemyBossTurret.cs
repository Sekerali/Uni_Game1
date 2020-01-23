using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game3
{
    class EnemyBossTurret : Entity
    {
        float shootCooldown = 0;
        Vector3 bulletpos;
        Vector3 diffuseC;
        Vector3 ambientC;
        public float bosshealth = 320.0f;

        Random rand = new Random();

        public EnemyBossTurret(Vector3 position)
        {
            Model = Game1.BossTurret;
            diffuseColor = Color.Gray.ToVector3();
            ambientLightColor = Color.Black.ToVector3();
            Position = position;
            Orientation = (float)Math.PI * 90 / 180;
            AngleY = 0;
            AngleX = 0;
            Radius = 3;
            Scale = 10;
            IsExpired = false;
        }

        public override void Update(GameTime gameTime)
        {
            Vector2 start = new Vector2(Position.X, Position.Y);
            Vector2 end = new Vector2(Ship.Instance.Position.X, Ship.Instance.Position.Y);

            float distance = Vector2.Distance(start, end);
            Vector2 directionV2 = Vector2.Normalize(end - start);
            Vector3 direction = new Vector3(directionV2.X, directionV2.Y, 0);

            //Shooting
            if (shootCooldown <= (1.6f * (bosshealth/320.0f) + 0.6f))
            {
                shootCooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {

                bulletpos = Position + new Vector3(0, 2, 0);
                diffuseC = Color.Purple.ToVector3();
                ambientC = Color.Red.ToVector3();
                EntityManager.Add(new Bullet(bulletpos, diffuseC, ambientC, 8, 80, direction));
                shootCooldown = 0;

                Game1.EnemyShoot.Play(0.2f, Game1.NextFloat(rand, 0.4f, 0.4f), 0);
            }


            Orientation = (float)Math.PI * 90 / 180 - (float)Math.Atan2(direction.X, direction.Y);
        }

    }
}
