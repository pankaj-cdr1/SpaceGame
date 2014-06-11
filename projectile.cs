using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class projectile
    {
        public Texture2D texture;
        public Vector2 position;
        public int Damage;
        Viewport viewport;
        public bool Active;
        public int Width
        {
            get { return texture.Width; }
        }

        public int Height
        {
            get { return texture.Height; }
        }
        float projectileSpeed;
        public void Initialize(Viewport Viewport, Texture2D Texture, Vector2 Position,int damage)
        {
            texture = Texture;
            position = Position;
            viewport = Viewport;
            Active = true;
            Damage = damage;
            projectileSpeed = 20f;

        }
        public void Update()
        {
            position.X += projectileSpeed;
            if (position.X + texture.Width / 2 > viewport.Width)
                Active = false;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture,position,null,Color.White,0f,
                                new Vector2(Width/2,Height/2),1f,SpriteEffects.None,0f);

        }
    }
}
