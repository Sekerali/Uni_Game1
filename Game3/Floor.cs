using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Game3
{

    public class Floor
    {
        GraphicsDevice graphicsDevice;

        Texture2D Ground;
        BasicEffect effect;
        BasicEffect effect2;
        VertexPositionTexture[] floorVerts;

        float floor1Pos = 0;
        float floor2Pos = -600;

        public Floor(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }

        public void Initialize()
        {
            int floorWidth = 400 / 2;
            int floorHeight = 600 / 2;

            floorVerts = new VertexPositionTexture[6];
            floorVerts[0].Position = new Vector3(0 - floorWidth, 0 - floorHeight, 0);
            floorVerts[1].Position = new Vector3(0 - floorWidth, floorHeight, 0);
            floorVerts[2].Position = new Vector3(floorWidth, 0 - floorHeight, 0);
            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(floorWidth, floorHeight, 0);
            floorVerts[5].Position = floorVerts[2].Position;

            floorVerts[0].TextureCoordinate = new Vector2(0, 0);
            floorVerts[1].TextureCoordinate = new Vector2(0, 1);
            floorVerts[2].TextureCoordinate = new Vector2(1, 0);
            floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
            floorVerts[4].TextureCoordinate = new Vector2(1, 1);
            floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;

            effect = new BasicEffect(graphicsDevice);
            effect2 = new BasicEffect(graphicsDevice);

            Ground = Game1.Floor;

        }

        public void Draw(Camera camera)
        {

            effect.World = Matrix.CreateTranslation(0, floor1Pos, 0);
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            effect.TextureEnabled = true;
            effect.Texture = Ground;

            effect2.World = Matrix.CreateTranslation(0, floor2Pos, 0);
            effect2.View = camera.ViewMatrix;
            effect2.Projection = camera.ProjectionMatrix;

            effect2.TextureEnabled = true;
            effect2.Texture = Ground;


            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphicsDevice.DrawUserPrimitives(
                    PrimitiveType.TriangleList, floorVerts, 0, 2);
            }

            foreach (var pass in effect2.CurrentTechnique.Passes)
            {
                pass.Apply();

                graphicsDevice.DrawUserPrimitives(
                    PrimitiveType.TriangleList, floorVerts, 0, 2);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (floor1Pos < 600)
            {
                floor1Pos += (float)gameTime.ElapsedGameTime.TotalSeconds * 200;
            } else
            {
                floor1Pos = floor2Pos - 595;
            }

            if (floor2Pos < 600)
            {
                floor2Pos += (float)gameTime.ElapsedGameTime.TotalSeconds * 200;
            }
            else
            {
                floor2Pos = floor1Pos - 595;
            }
        }
    }
}
