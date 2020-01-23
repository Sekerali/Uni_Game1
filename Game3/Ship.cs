using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game3
{
    class Ship : Entity
    {

        float angle = 0;
        float xPos = 0;
        float yPos = 0;

        int fade = 10;

        float cooldown = 0;

        Vector3 bulletpos;
        Vector3 diffuseC;
        Vector3 ambientC;

        Random rand = new Random();

        private static Ship instance;
        public static Ship Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Ship();
                }
                return instance;
            }
        }

        private Ship()
        {
            Model = Game1.Player;
            diffuseColor = Color.DarkCyan.ToVector3();
            ambientLightColor = Color.LightGreen.ToVector3();
            Position = new Vector3(0, 55, 20);
            Orientation = (float)Math.PI * 180 / 180;
            AngleY = 0;
            AngleX = 0;
            Radius = 6;
            Scale = 1f;
            IsExpired = false;
        }

        public override void Update(GameTime gameTime)
        {

            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.D))
            {
                xPos = Movement(xPos, -2, 0);
                angle = Rotate(angle, 0 - (float)gameTime.ElapsedGameTime.TotalSeconds * 2);
            }
            else if (state.IsKeyDown(Keys.A))
            {
                xPos = Movement(xPos, 2, 0);
                angle = Rotate(angle, (float)gameTime.ElapsedGameTime.TotalSeconds * 2);
            }
            else
            {
                if (angle > 0)
                {
                    angle = Rotate(angle, 0 - (float)gameTime.ElapsedGameTime.TotalSeconds * 2);
                }
                else if (angle < 0)
                {
                    angle = angle = Rotate(angle, (float)gameTime.ElapsedGameTime.TotalSeconds * 2);
                }
            }

            if ((xPos > 50) && (fade > 0))
            {
                Healthbar.transparent -= 0.07f;
                fade--;
            }
            else if (xPos < 50)
            {
                Healthbar.transparent = 1;
                fade = 10;
            }

            if (state.IsKeyDown(Keys.S))
            {
                yPos = Movement(yPos, 2, 1);
            }
            else if (state.IsKeyDown(Keys.W))
            {
                yPos = Movement(yPos, -2, 1);
            }

            if (state.IsKeyDown(Keys.Space))
            {
                if (cooldown <= 0.1f)
                {
                    cooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    bulletpos = Position + new Vector3(4, -2, 0);
                    diffuseC = Color.DarkCyan.ToVector3();
                    ambientC = Color.Blue.ToVector3();
                    EntityManager.Add(new Bullet(bulletpos, diffuseC, ambientC, 4, 150, new Vector3(0, -1, 0)));
                    bulletpos = Position + new Vector3(-4, -2, 0);
                    EntityManager.Add(new Bullet(bulletpos, diffuseC, ambientC, 4, 150, new Vector3(0, -1, 0)));

                    cooldown = 0;

                    Game1.PlayerShoot.Play(0.15f, Game1.NextFloat(rand, 0.4f, 0.4f), 0);
                }
            }

            Position = new Vector3(0 + xPos, 55 + yPos, 20);
            AngleY = angle;
        }

        public override void Draw(Camera camera)
        {
            base.Draw(camera);
        }

        public float Rotate(float angle, float increment)
        {
            angle += increment;
            if (angle < -0.6)
            {
                angle -= increment;
            }
            else if (angle > 0.6)
            {
                angle -= increment;
            }

            return angle;
        }

        public float Movement(float x, int value, int axis)
        {
            int constraintA = 0;
            int constraintB = 0;
            if (axis == 0)
            {
                constraintA = -60;
                constraintB = 60;
            }
            else if (axis == 1)
            {
                constraintA = -20;
                constraintB = 18;
            }


            x += value;
            if (x < constraintA)
            {
                x -= value;
            }
            else if (x > constraintB)
            {
                x -= value;
            }

            return x;
        }

    }
}