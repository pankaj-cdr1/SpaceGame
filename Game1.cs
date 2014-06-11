using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Shooter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Player player;
        KeyboardState currentKeyboardState;
        KeyboardState prevKeyboardState;
        float playerMoveSpeed;
        Texture2D mainBackground;
        ParallaxingBackground bgLayer1;
        ParallaxingBackground bgLayer2;
        Texture2D enemyTexture;
        Texture2D projectileTexture;
        Texture2D explosionTexture;
        Texture2D endScreen;
        List<projectile> Projectiles;
        List<Enemy> enemies;
        List<Animation> explosions;
        PowerUp powerUp;
        TimeSpan enemySpawnTime;
        TimeSpan prevSpawnTime;
        TimeSpan stageChangeTime;
        TimeSpan projectileSpawnTime;
        TimeSpan prevProjectileTime;
        TimeSpan prevStageChange;
        Random random;
        SoundEffect laserSound;
        SoundEffect explosionSound;
        Song gameplayMusic;
        int score;
        SpriteFont font;
        bool gameOver;
        int stage;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            player = new Player();
            currentKeyboardState = new KeyboardState();
            prevKeyboardState = new KeyboardState();
            playerMoveSpeed = 8.0f;
            bgLayer1 = new ParallaxingBackground();
            bgLayer2 = new ParallaxingBackground();
            enemies = new List<Enemy>();
            Projectiles = new List<projectile>();
            explosions = new List<Animation>();
            prevSpawnTime = TimeSpan.Zero;
            enemySpawnTime = TimeSpan.FromSeconds(1.0f);
            stageChangeTime = TimeSpan.FromSeconds(20.0f);
            prevStageChange = TimeSpan.Zero;
            projectileSpawnTime = TimeSpan.FromSeconds(0.15f);
            prevProjectileTime = TimeSpan.Zero;
            random = new Random();
            score = 0;
            gameOver = false;
            stage = 1;
            powerUp = new PowerUp();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X, GraphicsDevice.Viewport.TitleSafeArea.Y +
                                        GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            Animation playerAnimation = new Animation();
            Texture2D playerTexture = Content.Load<Texture2D>("shipAnimation");
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 115, 69, 8, 30, Color.White, 1f, true);
            player.Initialize(playerAnimation, playerPosition);
            mainBackground = Content.Load<Texture2D>("mainbackground");
            bgLayer1.Initialize(Content, "bgLayer1", -1, GraphicsDevice.Viewport.Width);
            bgLayer2.Initialize(Content, "bgLayer2", -2, GraphicsDevice.Viewport.Width);
            enemyTexture = Content.Load<Texture2D>("mineAnimation");
            endScreen = Content.Load<Texture2D>("endMenu");
            projectileTexture = Content.Load<Texture2D>("laser");
            explosionTexture = Content.Load<Texture2D>("explosion");
            gameplayMusic = Content.Load<Song>("sounds/gameMusic");
            laserSound = Content.Load<SoundEffect>("sounds/laserFire");
            explosionSound = Content.Load<SoundEffect>("sounds/explosion");
            font = Content.Load<SpriteFont>("gameFont");
            PlayMusic(gameplayMusic);
            powerUp.Initialize(Content.Load<Texture2D>("spriteSheet"),new Vector2(random.Next(100,GraphicsDevice.Viewport.Width),
                                random.Next(100,GraphicsDevice.Viewport.Height)),300/5,360/6,30,20,Color.White,1.0f,true,6,5,random.Next(1,5));

            // TODO: use this.Content to load your game content here
        }
        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (!gameOver)
            {
                prevKeyboardState = currentKeyboardState;
                currentKeyboardState = Keyboard.GetState();
                updatePlayer(gameTime);
                updateEnemies(gameTime);
                bgLayer1.Update();
                bgLayer2.Update();
                updateCollision();
                updateProjectiles(gameTime);
                updateExplosions(gameTime);
                powerUp.Update(gameTime);
            }
           
            base.Update(gameTime);
        }
        private void updatePlayer(GameTime gameTime)
        {

            player.Update(gameTime);
            Rectangle rect1 = new Rectangle((int)player.Position.X,(int) player.Position.Y, (int)player.Width,(int) player.Height);
            Rectangle rect2 = new Rectangle((int)powerUp.position.X, (int)powerUp.position.Y, (int)powerUp.width, (int)powerUp.height);
            if (powerUp.Active && rect1.Intersects(rect2))
            {

                powerUpAdvantage(powerUp.type);
                powerUp.Initialize(Content.Load<Texture2D>("spriteSheet"), new Vector2(random.Next(100, GraphicsDevice.Viewport.Width),
                                random.Next(100, GraphicsDevice.Viewport.Height)), 300 / 5, 360 / 6, 30, 20, Color.White, 1.0f, true, 6, 5, random.Next(1,5));

            }

            if (currentKeyboardState.IsKeyDown(Keys.Up))
            {
                player.Position.Y -= playerMoveSpeed;
                player.Position.Y = player.Position.Y < player.Height / 2 ? player.Height / 2 : player.Position.Y;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Down))
            {
                player.Position.Y += playerMoveSpeed;
                player.Position.Y = player.Position.Y > GraphicsDevice.Viewport.Height ? GraphicsDevice.Viewport.Height : player.Position.Y;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Left))
            {
                player.Position.X -= playerMoveSpeed;
                player.Position.X = player.Position.X < player.Width / 2 ? player.Width / 2 : player.Position.X;
            }
            else if (currentKeyboardState.IsKeyDown(Keys.Right))
            {
                player.Position.X += playerMoveSpeed;
                player.Position.X = player.Position.X > GraphicsDevice.Viewport.Width ? GraphicsDevice.Viewport.Width : player.Position.X;
            }
        }
        private void AddEnemy()
        {
            Animation enemyAnimation = new Animation();
            enemyAnimation.Initialize(enemyTexture, Vector2.Zero, 47, 61, 8, 30, Color.White, 1f, true);
            Vector2 position = new Vector2(GraphicsDevice.Viewport.Width,
                                            random.Next(0, GraphicsDevice.Viewport.Height ));
            Enemy enemy = new Enemy();
            enemy.Initialize(enemyAnimation, position, random.Next(1,stage+1));
            enemies.Add(enemy);
        }
        void updateEnemies(GameTime gameTime)
        {
            if (gameTime.TotalGameTime - prevSpawnTime  > enemySpawnTime)
            {
                prevSpawnTime = gameTime.TotalGameTime;
                AddEnemy();
                
            }
            if (gameTime.TotalGameTime - prevStageChange > stageChangeTime)
            {
                prevStageChange = gameTime.TotalGameTime;
                stage = stage + 1;
                enemySpawnTime -= TimeSpan.FromSeconds(0.15f);
            }

            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime,GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                if (!enemies[i].Active)
                {
                    enemies.RemoveAt(i);
                }
            }

        }
        void updateCollision()
        {
            Rectangle rect1;
            Rectangle rect2;
            rect1 = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);
            for (int i = 0; i < enemies.Count; i++)
            {
                rect2 = new Rectangle((int)enemies[i].Position.X, (int)enemies[i].Position.Y, enemies[i].Width, enemies[i].Height);
                if (rect1.Intersects(rect2))
                {
                    player.Health -= enemies[i].Damage;
                    enemies[i].Active = false;
                    AddExplosion(enemies[i].Position);
                    if (player.Health <= 0)
                    {
                        player.Active = false;
                        gameOver = true;
                        AddExplosion(new Vector2((rect1.X + rect2.X) / 2, (rect1.Y + rect2.Y) / 2));
                    }
                }
                
            }
            for (int i = 0; i < Projectiles.Count; i++)
            {
                rect1 = new Rectangle((int)Projectiles[i].position.X -
                                        Projectiles[i].Width / 2, (int)Projectiles[i].position.Y -
                                        Projectiles[i].Height / 2, Projectiles[i].Width, Projectiles[i].Height);
                for (int j = 0; j < enemies.Count; j++)
                {
                    rect2 = new Rectangle((int)enemies[j].Position.X - enemies[j].Width / 2,
                                            (int)enemies[j].Position.Y - enemies[j].Height / 2,
                                            enemies[j].Width, enemies[j].Height);

                 
                    if (rect1.Intersects(rect2))
                    {
                        enemies[j].Health -= Projectiles[i].Damage;
                        if (enemies[j].Health <= 0)
                        {
                            AddExplosion(enemies[j].Position);
                            score += enemies[j].Value;
                            enemies[j].Active = false;
                        }
                        Projectiles[i].Active = false;
                        
                    }
                }
            }
        }
        private void updateExplosions(GameTime gameTime)
        {
            for (int i = explosions.Count - 1; i >= 0; i--)
            {
                explosions[i].Update(gameTime);
                if (explosions[i].Active == false)
                {
                    explosions.RemoveAt(i);
                }
            }
        }
        void updateProjectiles(GameTime gametime)
        {
            if (gametime.TotalGameTime - prevProjectileTime > projectileSpawnTime)
            {
                prevProjectileTime = gametime.TotalGameTime;
                AddProjectile(player.Position + new Vector2(player.Width / 2, 0));
            }
            for (int i = Projectiles.Count - 1; i >= 0; i--)
            {
                Projectiles[i].Update();
                if (!Projectiles[i].Active)
                {
                    Projectiles.RemoveAt(i);
                }
            }
            
        }
        private void AddProjectile(Vector2 Position)
        {
            projectile Proj = new projectile();
            laserSound.Play();
            Proj.Initialize(GraphicsDevice.Viewport, projectileTexture, Position, player.damage);
            Projectiles.Add(Proj);
        }
        private void AddExplosion(Vector2 Position)
        {
            Animation explo = new Animation();
            explosionSound.Play();
            explo.Initialize(explosionTexture, Position, 134, 134, 12, 45, Color.White, 1f, false);
            explosions.Add(explo);
        }
        private void PlayMusic(Song song)
        {
            try
            {
                MediaPlayer.Play(song);
                MediaPlayer.IsRepeating = true;
            }
            catch { }
        }
        private void powerUpAdvantage(int type)
        {
            if (type == 1)
            {
                player.Health += 50;
            }
            else if (type == 2)
            {
                player.damage += 2;
            }
            else if (type == 3)
            {
                playerMoveSpeed += 2.0f;
            }
            else if (type == 4)
            {
                for (int i = enemies.Count - 1; i >= 0; i--)
                {
                    AddExplosion(enemies[i].Position);
                    score += enemies[i].Value;
                    enemies.RemoveAt(i);
                }
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            // TODO: Add your drawing code here
            if (!gameOver)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin();

                spriteBatch.Draw(mainBackground, Vector2.Zero, Color.White);
                bgLayer1.Draw(spriteBatch);
                bgLayer2.Draw(spriteBatch);
                player.Draw(spriteBatch);
                powerUp.Draw(spriteBatch);
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Draw(spriteBatch);
                }
                for (int i = 0; i < Projectiles.Count; i++)
                {
                    Projectiles[i].Draw(spriteBatch);
                }
                for (int i = 0; i < explosions.Count; i++)
                {
                    explosions[i].Draw(spriteBatch);
                }
                spriteBatch.DrawString(font, "Score: " + score, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                                        GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);
                spriteBatch.DrawString(font, "Health: " + player.Health, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X,
                                        GraphicsDevice.Viewport.TitleSafeArea.Y + 30), Color.White);
                spriteBatch.DrawString(font, "Stage: " + stage, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X+650,
                                        GraphicsDevice.Viewport.TitleSafeArea.Y), Color.White);
                spriteBatch.End();
            }
            else
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin();
                spriteBatch.Draw(endScreen, Vector2.Zero, Color.White);

                spriteBatch.DrawString(font, "Final Score: " + score, new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X+300,
                                        GraphicsDevice.Viewport.TitleSafeArea.Y+200), Color.White);
                spriteBatch.End();

            }
            
            base.Draw(gameTime);
        }
    }
}
