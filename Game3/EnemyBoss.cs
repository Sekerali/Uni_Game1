using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game3
{
    class EnemyBoss : Entity
    {
        Vector3 moveDir;
        float moveDur = 0;

        bool isEntering = true;
        public float health = 320.0f;

        Random rand = new Random();

        float shootCooldown = 0;
        float moveCooldown = 0;
        Vector3 bulletpos;
        Vector3 diffuseC;
        Vector3 ambientC;

        EnemyBossCore core1;
        EnemyBossCore core2;
        EnemyBossCore core3;
        EnemyBossCore core4;

        EnemyBossTurret turret1;
        EnemyBossTurret turret2;

        EnemyBossShield shield1;
        EnemyBossShield shield2;
        EnemyBossShield shield3;
        EnemyBossShield shield4;

        private static Random rnd = new Random();
        public static double GetRandom()
        {
            return rnd.NextDouble();
        }

        public EnemyBoss(Vector3 position)
        {
            Model = Game1.BossShip;
            diffuseColor = Color.Gray.ToVector3();
            ambientLightColor = Color.Black.ToVector3();
            Position = position;
            Orientation = (float)Math.PI * 90 / 180;
            AngleY = 0;
            AngleX = 0;
            Radius = 0;
            Scale = 10;
            IsExpired = false;

            core1 = new EnemyBossCore(Vector3.Zero);
            core2 = new EnemyBossCore(Vector3.Zero);
            core3 = new EnemyBossCore(Vector3.Zero);
            core4 = new EnemyBossCore(Vector3.Zero);

            EntityManager.Add(core1);
            EntityManager.Add(core2);
            EntityManager.Add(core3);
            EntityManager.Add(core4);

            turret1 = new EnemyBossTurret(Vector3.Zero);
            turret2 = new EnemyBossTurret(Vector3.Zero);

            EntityManager.Add(turret1);
            EntityManager.Add(turret2);

            shield1 = new EnemyBossShield(Vector3.Zero);
            shield2 = new EnemyBossShield(Vector3.Zero);
            shield3 = new EnemyBossShield(Vector3.Zero);
            shield4 = new EnemyBossShield(Vector3.Zero);

            EntityManager.Add(shield1);
            EntityManager.Add(shield2);
            EntityManager.Add(shield3);
            EntityManager.Add(shield4);

            shield1.Orientation += (float)Math.PI * 32 / 180;
            shield2.Orientation += (float)Math.PI * 12 / 180;
            shield4.Orientation -= (float)Math.PI * 15 / 180;

        }

        public override void Draw(Camera camera)
        {
            base.Draw(camera);
        }

        public override void Update(GameTime gameTime)
        {
            turret1.bosshealth = health;
            turret2.bosshealth = health;


            if (isEntering == false)
            {
                //Random Movement
                if (moveCooldown <= moveDur)
                {
                    moveCooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    moveDur = RandomDuration(gameTime);
                    moveDir = RandomDirection(gameTime);
                    moveCooldown = 0;
                }
                Position = Move(Position);
            }
            else
            {
                MoveCentre(gameTime, Position);
            }

            Vector2 start = new Vector2(Position.Y, Position.Z);
            Vector2 end = new Vector2(Ship.Instance.Position.Y, Ship.Instance.Position.Z);

            float distance = Vector2.Distance(start, end);
            Vector2 directionV2 = Vector2.Normalize(end - start);
            Vector3 direction = new Vector3(0, directionV2.X, directionV2.Y);

            //Shooting
            if (shootCooldown <= (0.8f * (health/320.0f) + 0.3f))
            {
                shootCooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                diffuseC = Color.Purple.ToVector3();
                ambientC = Color.Red.ToVector3();
                bulletpos = Position + new Vector3(-35, 15, 22);
                EntityManager.Add(new Bullet(bulletpos, diffuseC, ambientC, 8, 80, direction));
                bulletpos = Position + new Vector3(-45, 15, 22);
                EntityManager.Add(new Bullet(bulletpos, diffuseC, ambientC, 8, 80, direction));
                bulletpos = Position + new Vector3(31, 19, 22);
                EntityManager.Add(new Bullet(bulletpos, diffuseC, ambientC, 8, 80, direction));
                bulletpos = Position + new Vector3(41, 19, 22);
                EntityManager.Add(new Bullet(bulletpos, diffuseC, ambientC, 8, 80, direction));

                shootCooldown = 0;

                Game1.EnemyShoot.Play(0.4f, Game1.NextFloat(rand, 0.4f, 0.4f), 0);
            }

            core1.Position = Position + new Vector3(-46, 14, 2);
            core2.Position = Position + new Vector3(-19, 23, 2);
            core3.Position = Position + new Vector3(14, 23, 2);
            core4.Position = Position + new Vector3(44, 14, 2);

            turret1.Position = Position + new Vector3(6, 17, 22);
            turret2.Position = Position + new Vector3(-9, 17, 22);

            shield1.Position = Position + new Vector3(-48, 18, 2);
            shield2.Position = Position + new Vector3(-18, 27, 5);
            shield3.Position = Position + new Vector3(14, 28, 5);
            shield4.Position = Position + new Vector3(47, 19, 4);

        }

        public void MoveCentre(GameTime gameTime, Vector3 position)
        {
            Vector2 start = new Vector2(position.X, position.Y);
            Vector2 end = new Vector2(0, -30);

            float distance = Vector2.Distance(start, end);
            Vector2 directionV2 = Vector2.Normalize(end - start);
            Vector3 direction = new Vector3(directionV2.X, directionV2.Y, 0);

            Position += direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 50;

            if ((Position.X < 60) && (Position.X > -60) && (Position.Y < 0) && (Position.Y > -90))
            {
                isEntering = false;
            }
        }

        public void HandleCollision(EnemyShip1 enemy)
        {
            moveDir = new Vector3(0 - moveDir.X, 0 - moveDir.Y, 0);
            Position = Position + moveDir;
        }

        public Vector3 Move(Vector3 position)
        {
            float boundaryX = 70;
            float boundaryY1 = -10;
            float boundaryY2 = -80;

            Vector3 newPos = position + moveDir;

            if ((newPos.X > boundaryX) || (newPos.X < 0 - boundaryX))
            {
                moveDir = new Vector3(0 - moveDir.X, moveDir.Y, 0);
            }

            if ((newPos.Y > boundaryY1) || (newPos.Y < boundaryY2))
            {
                moveDir = new Vector3(moveDir.X, 0 - moveDir.Y, 0);
            }
            return (position + moveDir);
        }

        public Vector3 RandomDirection(GameTime gameTime)
        {
            float directionX;
            float directionY;

            int speed = (int)(GetRandom() * 20);

            directionX = (float)gameTime.ElapsedGameTime.TotalSeconds * 50;
            directionY = (float)gameTime.ElapsedGameTime.TotalSeconds * speed;

            double random = GetRandom();
            if (random < 0.5)
            {
                directionX = 0 - directionX;
            }

            random = GetRandom();
            if (random < 0.5)
            {
                directionY = 0 - directionY;
            }

            return new Vector3(directionX, directionY, 0);
        }

        public float RandomDuration(GameTime gameTime)
        {
            float duration;
            double random = GetRandom();

            if (random < 0.33)
            {
                duration = 1.0f;
            }
            else if (random < 0.66)
            {
                duration = 2.0f;
            }
            else
            {
                duration = 3.0f;
            }

            return duration;
        }
    }


}

