using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;

namespace Game3
{
    public class Menu
    {
        Texture2D MainBack;
        Texture2D MainButton;
        Texture2D MainHeader;
        SpriteFont MainButtonName;
        SpriteFont CreditsText;
        SpriteBatch MainMenu;
        GraphicsDevice graphicsDevice;

        Floor floor;
        public Camera camera;
        Healthbar health;

        Rectangle Level1 = new Rectangle(200, 250, 400, 100);
        Rectangle Level2 = new Rectangle(200, 400, 400, 100);
        Rectangle Level3 = new Rectangle(200, 550, 400, 100);
        Rectangle Credits = new Rectangle(200, 700, 400, 100);

        Point mousePos;

        public Game1 game;

        float locked2 = 0.5f;
        float locked3 = 0.5f;
        public static float cooldown = 0;
        public static bool spawnedboss = false;
        public static float speed = -100;
        public static int remain = 50;
        int level2spawn = 3;
        float ending = 0;
        bool clicked = false;

        public int gameState = 0; //0 menu, 1 credits, 2 level1, 3 level2, 4 level3

        public Menu(GraphicsDevice graphicsDevice, Game1 game)
        {
            this.graphicsDevice = graphicsDevice;
            this.game = game;
        }

        public void Initialize()
        {
            MainBack = Game1.MainBack;
            MainButton = Game1.MainButton;
            MainHeader = Game1.MainHeader;
            MainButtonName = Game1.MainButtonName;
            CreditsText = Game1.CreditsText;

            game.IsMouseVisible = true;

            MainMenu = new SpriteBatch(graphicsDevice);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.5f;
            MediaPlayer.Play(Game1.Main);
        }

        public void InitializeGame()
        {
            health = new Healthbar(graphicsDevice);
            health.Initialize();

            floor = new Floor(graphicsDevice);
            floor.Initialize();

            ExplosionManager.Initialize(graphicsDevice);

            EntityManager.Add(Ship.Instance);

            camera = new Camera(graphicsDevice);

        }

        public void Update(GameTime gameTime)
        {

            if ((Mouse.GetState().LeftButton == ButtonState.Pressed) && (gameState < 2))
            {
                clicked = true;
            }

            if ((Mouse.GetState().LeftButton == ButtonState.Released) && (gameState < 2) && (clicked == true))
            {
                clicked = false;
                mousePos = new Point(Mouse.GetState().X, Mouse.GetState().Y);
                if (Level1.Contains(mousePos))
                {
                    InitializeGame();
                    gameState = 2;
                    game.IsMouseVisible = false;
                    MediaPlayer.Play(Game1.Ingame);
                    Game1.Select.Play(0.2f, 0, 0);
                }
                else if ((Level2.Contains(mousePos)) && (locked2 == 1f))
                {
                    InitializeGame();
                    gameState = 3;
                    game.IsMouseVisible = false;
                    MediaPlayer.Play(Game1.Ingame);
                    Game1.Select.Play(0.2f, 0, 0);
                }
                else if ((Level3.Contains(mousePos)) && (locked3 == 1f))
                {
                    InitializeGame();
                    gameState = 4;
                    game.IsMouseVisible = false;
                    MediaPlayer.Play(Game1.Boss);
                    Game1.Select.Play(0.2f, 0, 0);
                }
                else if (Credits.Contains(mousePos))
                {
                    
                    Game1.SelectCredits.Play(0.2f, 0, 0);
                    if (gameState == 0)
                    {
                        gameState = 1;
                    }
                    else
                    {
                        gameState = 0;
                    }
                    
                }
            }

            if (gameState > 1)
            {
                EntityManager.Update(gameTime);
                floor.Update(gameTime);
                ExplosionManager.Update(gameTime);
                camera.Update(gameTime);
                health.Update(gameTime);
                if (speed == -100)
                {
                    speed = 3.0f;
                } else if (speed <= 0.15f)
                {
                    speed = 0.15f;
                    remain--;
                }
            }

            if (gameState == 2)
            {
                if (cooldown <= speed)
                {
                    cooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    if (remain > 0)
                    {
                        Spawning(0);
                        cooldown = 0;
                        speed -= speed * 0.05f;
                    }
                    else
                    {
                        if (EntityManager.enemy1.Count == 0)
                        {
                            if (ending <= 3.0f)
                            {
                                ending += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            }
                            else
                            {
                                gameState = 0;
                                EntityManager.ResetAll();
                                locked2 = 1f;
                                ending = 0;
                            }
                        }
                    }
                }
            }
            else if (gameState == 3)
            {
                if (cooldown <= speed)
                {
                    cooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    if (remain > 0)
                    {
                        if (level2spawn > 1)
                        {
                            Spawning(0);
                            level2spawn--;
                        }
                        else
                        {
                            Spawning(1);
                            level2spawn = 3;
                        }
                        
                        cooldown = 0;
                        speed -= speed * 0.05f;
                    }
                    else
                    {
                        if ((EntityManager.enemy1.Count == 0) && (EntityManager.enemy2.Count == 0))
                        {
                            if (ending <= 3.0f)
                            {
                                ending += (float)gameTime.ElapsedGameTime.TotalSeconds;
                            }
                            else
                            {
                                gameState = 0;
                                EntityManager.ResetAll();
                                locked3 = 1f;
                                ending = 0;
                            }
                        }
                    }
                }
            }
            else if (gameState == 4)
            {
                if (spawnedboss == false)
                {
                    if (cooldown <= 4.0f)
                    {
                        cooldown += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        EntityManager.Add(new EnemyBoss(new Vector3(90, -40, 20)));
                        cooldown = 0;
                        spawnedboss = true;
                    }
                }

                if ((EntityManager.boss.Count == 0) && (spawnedboss == true))
                {
                    if (ending <= 3.0f)
                    {
                        ending += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        gameState = 0;
                        EntityManager.ResetAll();
                        locked3 = 1f;
                        ending = 0;
                    }
                }

            }


        }

        public void Spawning(int level) //0 = level1, 1 = level2
        {
            Vector3 position;
            double randSide = GetRandom();

            int side;
            if (GetRandom() < 0.5)
            {
                side = 1;
            }
            else
            {
                side = -1;
            }

            if (randSide < 0.5)
            {
                position = new Vector3(70 * side, (float)GetRandom() * -90, 20);
            }
            else
            {
                position = new Vector3((float)GetRandom() * 70 * side, -90, 20);
            }

            if (level == 0)
            {
                EntityManager.Add(new EnemyShip1(position));
            }
            else if (level == 1)
            {
                EntityManager.Add(new EnemyShip2(position));
            }
        }

        private static Random rnd = new Random();
        public static double GetRandom()
        {
            return rnd.NextDouble();
        }

        public void Draw()
        {
            if (gameState == 0)
            {
                MainMenu.Begin();
                MainMenu.Draw(MainBack, new Rectangle(0, 0, 800, 900), Color.White);
                MainMenu.Draw(MainHeader, new Rectangle(0, 0, 800, 200), Color.White);
                MainMenu.Draw(MainButton, new Rectangle(200, 250, 400, 100), Color.White);
                MainMenu.DrawString(MainButtonName, "Level 1", new Vector2(230, 265), Color.Black);
                MainMenu.Draw(MainButton, new Rectangle(200, 400, 400, 100), Color.White * locked2);
                MainMenu.DrawString(MainButtonName, "Level 2", new Vector2(230, 415), Color.Black * locked2);
                MainMenu.Draw(MainButton, new Rectangle(200, 550, 400, 100), Color.White * locked3);
                MainMenu.DrawString(MainButtonName, "Level 3", new Vector2(230, 565), Color.Black * locked3);
                MainMenu.Draw(MainButton, new Rectangle(200, 700, 400, 100), Color.White);
                MainMenu.DrawString(MainButtonName, "Credits", new Vector2(230, 715), Color.Black);
                MainMenu.End();
                graphicsDevice.BlendState = BlendState.Opaque;
                graphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else if (gameState == 1)
            {
                MainMenu.Begin();
                MainMenu.Draw(MainBack, new Rectangle(0, 0, 800, 900), Color.White);
                MainMenu.Draw(MainHeader, new Rectangle(0, 0, 800, 200), Color.White);
                MainMenu.Draw(MainButton, new Rectangle(200, 700, 400, 100), Color.White);
                MainMenu.DrawString(MainButtonName, "Credits:", new Vector2(200, 265), Color.Black);
                MainMenu.DrawString(CreditsText, "Explosion Texture: \r\n\r\nhttps://opengameart.org/content/explosion-sheet", new Vector2(200, 365), Color.Black * locked2);
                MainMenu.DrawString(CreditsText, "Music and Sounds: \r\n\r\nhttp://musmus.main.jp/english.html", new Vector2(200, 515), Color.Black * locked2);
                MainMenu.DrawString(MainButtonName, "Back", new Vector2(230, 715), Color.Black);
                MainMenu.End();
                graphicsDevice.BlendState = BlendState.Opaque;
                graphicsDevice.DepthStencilState = DepthStencilState.Default;
            }
            else
            {
                EntityManager.Draw(camera);
                floor.Draw(camera);
                ExplosionManager.Draw(camera);
                health.Draw();
            }
        }
    }
}
