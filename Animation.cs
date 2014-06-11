using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace Shooter
{
    class Animation
    {
        Texture2D spriteStrip;
        float scale;
        int elapsedTime;
        int frameTime;
        int frameCount;
        int currentFrame;
        int time;
        int currentTime;
        Color color;
        Rectangle sourceRect = new Rectangle();
        Rectangle destinationRect = new Rectangle();
        public int FrameWidth;
        public int FrameHeight;
        public bool Active;
        public bool Looping;
        public Vector2 Position;
        public void Initialize(Texture2D texture, Vector2 position,
                                   int frameWidth, int frameHeight, int frameCount,
                                    int frametime, Color color, float scale, bool looping)
        {
            
            this.color = color;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.frameTime = frametime;
            this.scale = scale;

            Looping = looping;
            Position = position;
            spriteStrip = texture;

            elapsedTime = 0;
            currentFrame = 0;

            Active = true;

            time = 2;
            currentTime = 0;
        }
        public void Update(GameTime gameTime)
        {
            if (!Active)
            {
                return;
            }
            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsedTime > frameTime)
            {
                currentTime++;
                if (currentTime == time)
                {
                    currentFrame++;
                    currentTime = 0;
                }

                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    if (!Looping)
                    {
                        Active = false;
                    }
                }
                elapsedTime = 0;
            }

            sourceRect = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);

            destinationRect = new Rectangle((int)Position.X - (int)(FrameWidth * scale) / 2,
                                            (int)Position.Y - (int)(FrameHeight * scale) / 2,
                                            (int)(FrameWidth * scale),
                                            (int)(FrameHeight * scale));
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteStrip, destinationRect, sourceRect, color);
        }

    }
}
