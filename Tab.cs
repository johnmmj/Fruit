using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Fruit
{
    class Tab
    {
        Texture2D texture;
        Rectangle rec;
        Texture2D title;
        public BoundingBox openBox;
        public int offsetWidth;
        public bool open = false;

        public Tab(Texture2D Texture, Rectangle Rec, Texture2D Title, BoundingBox OpenBox, int OffsetWidth )
        {
            texture = Texture;
            rec = Rec;
            title = Title;
            openBox = OpenBox;
            offsetWidth = OffsetWidth;
        }

        public void Clicked()
        {
            if (open == true)
            {
                Close();
            }
            else if (open == false)
            {
                Open();
            }
        }

        public void Close()
        {
            rec.X += offsetWidth;
            openBox.Min.X += offsetWidth;
            openBox.Max.X += offsetWidth;
            open = false;
        }
        public void Open()
        {
            rec.X -= offsetWidth;
            openBox.Min.X -= offsetWidth;
            openBox.Max.X -= offsetWidth;
            open = true;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rec, Color.White);
            spriteBatch.Draw(title, new Rectangle(Convert.ToInt32(openBox.Min.X), Convert.ToInt32(openBox.Min.Y), title.Width, title.Height), Color.White);
        }
    }
}
