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
    class Enemy
    {
        public Animation EnemyAnimation;
        public Vector2 Position;
        public bool Active;
        public int Health;
        public int Damage;
        public int Value;
        public int style;
        public int Width
        {
            get { return EnemyAnimation.FrameWidth; }
        }
        public int Height
        {
            get { return EnemyAnimation.FrameHeight; }
        }
        float enemyMoveSpeedX;
        float enemyMoveSpeedY;

        public void Initialize(Animation animation,Vector2 position,int Style)
        {
            EnemyAnimation = animation;
            Position = position;
            Active = true;
            Health = 10;
            Damage = 10;
            enemyMoveSpeedX = 6f;
            enemyMoveSpeedY = 0f;
            Value = 100 * Style;
            style = Style;
            if (style >= 2)
            {
                enemyMoveSpeedX = 4f;
                enemyMoveSpeedY = 4f;
            }

        }

        public void Update(GameTime gameTime,int screenWidth,int screenHeight)
        {
            if (style == 2)
            {
                Position.X -= enemyMoveSpeedX;
                Position.Y -= enemyMoveSpeedY;
            }
            else if (style == 3)
            {
                Position.X -= enemyMoveSpeedX;
                Position.Y += enemyMoveSpeedY;
            }
            else {
                Position.X -= enemyMoveSpeedX;
            }
            
            if (Position.Y < 0 || Position.Y>screenHeight)
                enemyMoveSpeedY *= -1f;
            
            EnemyAnimation.Position = Position;
            EnemyAnimation.Update(gameTime);

            if (Position.X < -Width || Health <= 0 )
            {
                Active = false;
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            EnemyAnimation.Draw(spriteBatch);
        }
     
    }
}
