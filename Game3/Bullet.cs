using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game3
{
    class Bullet : Entity
    {

        int Speed;
        Vector3 Direction;

        public Bullet(Vector3 position, Vector3 ambientdiffusecolor, Vector3 ambientlightcolor, float scale, int speed, Vector3 direction)
        {
            Speed = speed;
            Direction = direction;

            Model = Game1.Bullet;
            diffuseColor = ambientdiffusecolor;
            ambientLightColor = ambientlightcolor;
            Position = position;
            Orientation = (float)Math.Atan2(direction.X, 0 - direction.Y) + (float)Math.PI * -90 / 180;
            AngleY = 0;
            AngleX = 0;
            Radius = 1;
            Scale = scale;
            IsExpired = false;
        }

        public override void Draw(Camera camera)
        {
            base.Draw(camera);
        }

        public override void Update(GameTime gameTime)
        {

            Position += Direction * (float)gameTime.ElapsedGameTime.TotalSeconds * Speed;

            if ((Position.Y < -100) || (Position.Y > 100))
            {
                IsExpired = true;
            }

        }
    }
}
