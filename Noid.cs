using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;


namespace Fruit
{
    class Noid
    {
        public Texture2D texture;
        public Rectangle rectangle;
        public int layer;

        public Noid(Texture2D Image, Rectangle Rec, int Layer)
        {
            texture = Image;
            rectangle = Rec;
            layer = Layer;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, Color.White);
        }
    }
}
