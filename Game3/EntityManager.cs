using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Media;

namespace Game3
{
    static class EntityManager
    {

        static List<Bullet> bullets = new List<Bullet>();
        static List<Bullet> playerbullets = new List<Bullet>();
        static List<Bullet> enemybullets = new List<Bullet>();

        public static List<Entity> entities = new List<Entity>();
        public static List<EnemyShip1> enemy1 = new List<EnemyShip1>();
        public static List<EnemyShip2> enemy2 = new List<EnemyShip2>();
        //static List<EnemyShip1> enemy3 = new List<EnemyShip1>();

        public static List<EnemyBoss> boss = new List<EnemyBoss>();
        static List<EnemyBossShield> shield = new List<EnemyBossShield>();
        static List<EnemyBossCore> core = new List<EnemyBossCore>();
        static List<EnemyBossTurret> turret = new List<EnemyBossTurret>();

        public static bool isUpdating;
        static List<Entity> addedEntities = new List<Entity>();

        static bool bossSpawned = false;

        static Random rand = new Random();

        public static void Add(Entity entity)
        {
            if (!isUpdating)
                AddEntity(entity);
            else
                addedEntities.Add(entity);
        }

        private static void AddEntity(Entity entity)
        {
            entities.Add(entity);
            if (entity is Bullet)
                bullets.Add(entity as Bullet);
            else if (entity is EnemyShip1)
                enemy1.Add(entity as EnemyShip1);
            else if (entity is EnemyShip2)
                enemy2.Add(entity as EnemyShip2);
            else if (entity is EnemyBoss)
                boss.Add(entity as EnemyBoss);
            else if (entity is EnemyBossShield)
                shield.Add(entity as EnemyBossShield);
            else if (entity is EnemyBossCore)
                core.Add(entity as EnemyBossCore);
            else if (entity is EnemyBossTurret)
                turret.Add(entity as EnemyBossTurret);


            playerbullets = bullets.Where(x => (x.diffuseColor == Color.DarkCyan.ToVector3())).ToList();
            enemybullets = bullets.Where(x => (x.diffuseColor == Color.Pink.ToVector3())).ToList();

        }

        public static void Update(GameTime gameTime)
        {
            //endscreen finished
            if (Healthbar.endtime >= 3.0f)
            {
                ResetAll();
            }
            //update
            else
            {
                isUpdating = true;
                HandleCollisions();

                foreach (var entity in entities)
                    entity.Update(gameTime);

                isUpdating = false;

                foreach (var entity in addedEntities)
                    AddEntity(entity);

                addedEntities.Clear();

                entities = entities.Where(x => !x.IsExpired).ToList();

                bullets = bullets.Where(x => !x.IsExpired).ToList();
                playerbullets = bullets.Where(x => (x.diffuseColor == Color.DarkCyan.ToVector3())).ToList();
                enemybullets = bullets.Where(x => (x.diffuseColor == Color.Purple.ToVector3())).ToList();

                enemy1 = enemy1.Where(x => !x.IsExpired).ToList();
                enemy2 = enemy2.Where(x => !x.IsExpired).ToList();
                boss = boss.Where(x => !x.IsExpired).ToList();
                core = core.Where(x => !x.IsExpired).ToList();
                shield = shield.Where(x => !x.IsExpired).ToList();
                turret = turret.Where(x => !x.IsExpired).ToList();
            }
        }

        public static void ResetAll()
        {
            entities.Clear();
            bullets.Clear();
            playerbullets.Clear();
            enemybullets.Clear();
            enemy1.Clear();
            enemy2.Clear();
            boss.Clear();
            shield.Clear();
            core.Clear();
            turret.Clear();
            Game1.menu.gameState = 0;
            Game1.menu.game.IsMouseVisible = true;
            Healthbar.endtime = 0;
            Healthbar.ending = false;
            Healthbar.health = 100;
            Ship.Instance.IsExpired = false;
            bossSpawned = false;
            Menu.spawnedboss = false;
            Menu.speed = -100;
            Menu.remain = 20;
            Menu.cooldown = 0;
            ExplosionManager.explosions.Clear();
            ExplosionManager.addingexplosions.Clear();
            MediaPlayer.Play(Game1.Main);
        }

        static void HandleCollisions()
        {
            // handle collisions between bullets and enemy1
            for (int i = 0; i < enemy1.Count; i++)
            { 
                for (int j = 0; j < playerbullets.Count; j++)
                {
                    if (IsColliding(enemy1[i], playerbullets[j]))
                    {
                        enemy1[i].health--;
                        playerbullets[j].IsExpired = true;
                        Game1.PlayerShoot.Play(0.4f, 0.8f, 0);
                        ExplosionManager.Add(new Explosion(enemy1[i].Position, 0, 2));
                        if (enemy1[i].health <= 0)
                        {
                            Game1.Explode.Play(0.4f, Game1.NextFloat(rand, 0.4f, 0.4f), 0);
                            ExplosionManager.Add(new Explosion(enemy1[i].Position, 0, 63));
                            enemy1[i].IsExpired = true;
                        }
                    }
                }
            }

            // handle collisions between bullets and enemy2
            for (int i = 0; i < enemy2.Count; i++)
            {
                for (int j = 0; j < playerbullets.Count; j++)
                {
                    if (IsColliding(enemy2[i], playerbullets[j]))
                    {
                        enemy2[i].health--;
                        playerbullets[j].IsExpired = true;
                        Game1.PlayerShoot.Play(0.4f, 0.8f, 0);
                        ExplosionManager.Add(new Explosion(enemy2[i].Position, 0, 2));
                        if (enemy2[i].health <= 0) {
                            Game1.Explode.Play(0.4f, Game1.NextFloat(rand, 0.4f, 0.4f), 0);
                            ExplosionManager.Add(new Explosion(enemy2[i].Position, 0, 63));
                            enemy2[i].IsExpired = true;
                        }
                    }
                }
            }

            // handle collisions between bullets and player
            for (int i = 0; i < enemybullets.Count; i++)
            {
                if (IsColliding(Ship.Instance, enemybullets[i]))
                {
                    Healthbar.Hit();
                    enemybullets[i].IsExpired = true;
                    Game1.EnemyShoot.Play(0.4f, -0.8f, 0);
                }
            }

            // handle collisions between bullets and boss shield
            for (int i = 0; i < shield.Count; i++)
            {
                for (int j = 0; j < playerbullets.Count; j++)
                {
                    if (IsColliding(shield[i], playerbullets[j]))
                    {
                        shield[i].health--;
                        boss[0].health--;
                        playerbullets[j].IsExpired = true;
                        Game1.PlayerShoot.Play(0.4f, 0.8f, 0);
                        ExplosionManager.Add(new Explosion(shield[i].Position, 0, 2));
                        if (shield[i].health <= 0)
                        {
                            Game1.Explode.Play(0.4f, Game1.NextFloat(rand, 0.4f, 0.4f), 0);
                            ExplosionManager.Add(new Explosion(shield[i].Position, 0, 63));
                            shield[i].IsExpired = true;
                        }
                    }
                }
            }

            // handle collisions between bullets and boss core
            for (int i = 0; i < core.Count; i++)
            {
                for (int j = 0; j < playerbullets.Count; j++)
                {
                    if (IsColliding(core[i], playerbullets[j]))
                    {
                        core[i].health--;
                        boss[0].health--;
                        playerbullets[j].IsExpired = true;
                        Game1.PlayerShoot.Play(0.4f, 0.8f, 0);
                        ExplosionManager.Add(new Explosion(core[i].Position, 0, 2));
                        if (core[i].health <= 0)
                        {
                            Game1.Explode.Play(0.4f, Game1.NextFloat(rand, 0.4f, 0.4f), 0);
                            ExplosionManager.Add(new Explosion(core[i].Position, 0, 63));
                            core[i].IsExpired = true;
                        }
                    }
                }
            }

            //remove boss when cores destroyed
            if (core.Count > 0)
            {
                bossSpawned = true;
            }
            else if ((core.Count == 0) && (bossSpawned == true))
            {
                for (int i = 0; i < turret.Count; i++)
                {
                    turret[i].IsExpired = true;
                }
                ExplosionManager.Add(new Explosion(boss[0].Position, 0, 63));
                boss[0].IsExpired = true;
                bossSpawned = false;
            }

        }

        private static bool IsColliding(Entity a, Entity b)
        {
            float radius = a.Radius + b.Radius;
            Vector2 aPos = new Vector2(a.Position.X, a.Position.Y);
            Vector2 bPos = new Vector2(b.Position.X, b.Position.Y);
            return !a.IsExpired && !b.IsExpired && Vector2.DistanceSquared(aPos, bPos) < radius * radius;

        }

        public static void Draw(Camera camera)
        {
            foreach (var entity in entities)
                entity.Draw(camera);
        }

    }
}
