using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class PowerUp
    {
        public int spawnTime;
        public int time;
        public Animation2D animation;
        public bool Active;
        public Vector2 position;
        public int height;
        public int width;
        public int type;
        public int lastSpawn;
        public int elapsed;
        public void Initialize(Texture2D texture, Vector2 Position,
                                   int frameWidth, int frameHeight, int frameCount,
                                    int frametime, Color color, float scale, bool looping,int Row,int Col,int variety)
        {
            animation = new Animation2D();
            height = frameHeight;
            position = Position;
            width = frameWidth;
            type = variety;
            spawnTime = 10000;
            time = 5000;
            lastSpawn = 0;
            animation.Initialize(texture, position, frameWidth, frameHeight, frameCount, frametime, color, scale, looping, Row, Col);
            elapsed = 0;
            Active = false;
        }
        public void Update(GameTime gameTime)
        {
            if (elapsed - lastSpawn > spawnTime)
            {
                lastSpawn = elapsed;
                Active = true;
            }
            else if (elapsed - lastSpawn > time && Active )
            {
                Active = false;
                lastSpawn = elapsed;
                
            }
            
            elapsed += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if(Active)
                animation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(Active)
                animation.Draw(spriteBatch);
        }
        
    }
}
