using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game3
{
    class EnemyShip2 : Entity
    {
        Vector3 moveDir;
        float moveDur = 0;

        bool isEntering = true;

        float shootCooldown = 0;
        float shootCooldown2 = 0;
        int shotcount = 3;
        float moveCooldown = 0;
        Vector3 bulletpos;
        Vector3 diffuseC;
        Vector3 ambientC;

        Random rand = new Random();

        public int health = 5;

        private static Random rnd = new Random();
        public static double GetRandom()
        {
            return rnd.NextDouble();
        }

        public override void Draw(Camera camera)
        {
            base.Draw(camera);
        }

        public EnemyShip2(Vector3 position)
        {
            Model = Game1.Enemy2;
            diffuseColor = Color.Pink.ToVector3();
            ambientLightColor = Color.Blue.ToVector3();
            Position = position;
            Orientation = (float)Math.PI * 90 / 180;
            AngleX = 0;
            AngleY = 0;
            Radius = 5;
            Scale = 1.3f;
            IsExpired = false;
        }


        public override void Update(GameTime gameTime)
        {
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

            Vector2 start = new Vector2(Position.X, Position.Y);
            Vector2 end = new Vector2(Ship.Instance.Position.X, Ship.Instance.Position.Y);

            float distance = Vector2.Distance(start, end);
            Vector2 directionV2 = Vector2.Normalize(end - start);
            Vector3 direction = new Vector3(directionV2.X, directionV2.Y, 0);

            //Shooting
            if (shootCooldown <= 1.5f)
            {
                shootCooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {   
                if (shootCooldown2 < 0.1f)
                {
                    shootCooldown2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else if (shotcount > 0)
                {
                    bulletpos = Position + new Vector3(0, 6, 0);
                    diffuseC = Color.Purple.ToVector3();
                    ambientC = Color.Red.ToVector3();
                    EntityManager.Add(new Bullet(bulletpos, diffuseC, ambientC, 8, 80, direction));
                    shootCooldown2 = 0;
                    shotcount--;

                    Game1.EnemyShoot.Play(0.2f, Game1.NextFloat(rand, 0.4f, 0.4f), 0);
                }
                else
                {
                    shotcount = 3;
                    shootCooldown = 0;
                }
            }

            //turning
            AngleY = Rotate(AngleY, moveDir.X / 25);
            Orientation = (float)Math.PI * 90 / 180 - (float)Math.Atan2(direction.X, direction.Y);

        }

        public void MoveCentre(GameTime gameTime, Vector3 position)
        {
            Vector2 start = new Vector2(position.X, position.Y);
            Vector2 end = new Vector2(0, 0);

            float distance = Vector2.Distance(start, end);
            Vector2 directionV2 = Vector2.Normalize(end - start);
            Vector3 direction = new Vector3(directionV2.X, directionV2.Y, 0);

            Position += direction * (float)gameTime.ElapsedGameTime.TotalSeconds * 50;

            if ((Position.X < 60) && (Position.X > -60) && (Position.Y < 18) && (Position.Y > -70))
            {
                isEntering = false;
            }
        }

        public void HandleCollision(EnemyShip1 enemy)
        {
            moveDir = new Vector3(0 - moveDir.X, 0 - moveDir.Y, 0);
            Position = Position + moveDir;
        }

        public float Rotate(float angle, float increment)
        {
            angle += increment;
            if (angle < -0.6 + (float)Math.PI * 180 / 180)
            {
                angle -= increment;
            }
            else if (angle > 0.6 + (float)Math.PI * 180 / 180)
            {
                angle -= increment;
            }

            return angle;
        }

        public Vector3 Move(Vector3 position)
        {
            float boundaryX = 70;
            float boundaryY1 = 28;
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

            int speed = (int)(GetRandom() * 50);

            double random = GetRandom();
            if (random < 0.5)
            {
                directionX = (float)gameTime.ElapsedGameTime.TotalSeconds * 50;
                directionY = (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
            }
            else
            {
                directionX = (float)gameTime.ElapsedGameTime.TotalSeconds * speed;
                directionY = (float)gameTime.ElapsedGameTime.TotalSeconds * 50;
            }

            random = GetRandom();
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

