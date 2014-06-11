using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Shooter
{
    class Player
    {
        public Animation PlayerAnimation;
        public Vector2 Position;
        public bool Active;
        public int Health;
        public int damage;
        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }
        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }
        public void Initialize(Animation animation,Vector2 position)
        {
            PlayerAnimation = animation;
            Position = position;
            Active = true;
            Health = 100;
            damage = 2;
        }
        public void Update(GameTime gameTime) 
        {
            if (Active)
            {
                PlayerAnimation.Position = Position;
                PlayerAnimation.Update(gameTime);
            }
            else
            {
                Position = new Vector2(-100, -100);
            }
        }
        
        public void Draw(SpriteBatch spriteBatch) 
        {
            if(Active)
                PlayerAnimation.Draw(spriteBatch);
        }

    }
}
