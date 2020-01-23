using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game3
{
    class Healthbar
    {
        Texture2D HealthFront;
        Texture2D HealthBack;
        SpriteFont HealthName;
        SpriteBatch Health;
        GraphicsDevice graphicsDevice;

        public static double health;
        static double healthRemain;
        public static float transparent = 1;
        public static float endtime = 0;
        public static bool ending = false;

        Random rand = new Random();

        public Healthbar(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Initialize()
        {
            HealthFront = Game1.HealthFront;
            HealthBack = Game1.HealthBack;
            HealthName = Game1.HealthName;
            
            Health = new SpriteBatch(graphicsDevice);

            health = 100;
            healthRemain = 259 * (health / 100);
        }

        public void Update(GameTime gameTime)
        {
            if ((health == 0) && (ending == false))
            {
                ending = true;
                Game1.Explode.Play(0.4f, Game1.NextFloat(rand, 0.4f, 0.4f), 0);
                ExplosionManager.Add(new Explosion(Ship.Instance.Position, 0, 63));
                Ship.Instance.IsExpired = true;
            }


            if (ending == true)
            {
                if (endtime <= 3.0f)
                {
                    endtime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

        }


        public void Draw()
        {
            Health.Begin();
            Health.Draw(HealthBack, new Rectangle(10, 900 - 259 - 10, 32, 259), Color.White * transparent);
            Health.Draw(HealthFront, new Rectangle(10, 900 - (int)healthRemain - 10, 32, (int)healthRemain), Color.White * transparent);
            Health.DrawString(HealthName, "HP: " + health, new Vector2(15, 900 - 20), Color.White * transparent, (float)Math.PI * -90 / 180, Vector2.Zero, 1, SpriteEffects.None, 1);
            Health.End();
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;
        }

        public static void Hit()
        {
            if (health > 0) {
                health -= 10;
            }
            healthRemain = 259 * (health / 100);
        }

        public static double getHealth()
        {
            return health;
        }
    }
}
