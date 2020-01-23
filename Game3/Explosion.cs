using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game3
{
    public class Explosion
    {
        GraphicsDevice graphicsDevice = ExplosionManager.graphicsDevice;

        AlphaTestEffect effect;
        VertexPositionTexture[] verts;

        float cooldown = 1;
        int startPic;
        int endPic;
        Vector3 Position;
        public bool IsExpired = false;

        public Explosion(Vector3 position, int startpic, int endpic)
        {

            int floorWidth = 24 / 2;
            int floorHeight = 24 / 2;

            verts = new VertexPositionTexture[6];
            verts[0].Position = new Vector3(0 - floorWidth, 0 - floorHeight, 0);
            verts[1].Position = new Vector3(0 - floorWidth, floorHeight, 0);
            verts[2].Position = new Vector3(floorWidth, 0 - floorHeight, 0);
            verts[3].Position = verts[1].Position;
            verts[4].Position = new Vector3(floorWidth, floorHeight, 0);
            verts[5].Position = verts[2].Position;

            verts[0].TextureCoordinate = new Vector2(0, 0);
            verts[1].TextureCoordinate = new Vector2(0, 1);
            verts[2].TextureCoordinate = new Vector2(1, 0);
            verts[3].TextureCoordinate = verts[1].TextureCoordinate;
            verts[4].TextureCoordinate = new Vector2(1, 1);
            verts[5].TextureCoordinate = verts[2].TextureCoordinate;

            startPic = startpic;
            endPic = endpic;
            Position = position;

            effect = new AlphaTestEffect(graphicsDevice);
        }

        public void Update(GameTime gameTime)
        {
            if (startPic > endPic)
            {
                IsExpired = true;
            }

            if (cooldown <= 0.001f)
            {
                cooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                cooldown = 0;
                startPic++;
            }
        }

        public void Draw(Camera camera)
        {
            if (startPic < endPic)
            {
                effect.World = Matrix.CreateTranslation(Position);
                effect.View = camera.ViewMatrix;
                effect.Projection = camera.ProjectionMatrix;

                effect.Texture = Game1.Explosion[startPic];

                foreach (var pass in effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, verts, 0, 2);
                }
            }
        }
    }
}
