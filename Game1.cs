using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Collections.Generic;
using System.IO;
using System;

namespace Fruit
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D hover, filled, currentImage, eraser, optTab, folder;
        int mouseX, mouseY, squareX, squareY;
        List<Rectangle> placed = new List<Rectangle>();
        List<Rectangle> pallet = new List<Rectangle>();
        Rectangle hoverTangle = new Rectangle();
        List<BoundingBox> palletBox = new List<BoundingBox>();
        BoundingBox mouseBox = new BoundingBox();
        List<Noid> noids = new List<Noid>();
        List<Texture2D> textures = new List<Texture2D>();
        BoundingBox eraserBox = new BoundingBox(new Vector3(1024, 0, 0), new Vector3(1056, 32, 1));
        Rectangle eraserTangle = new Rectangle(1024, 0, 32, 32);
        SpriteFont theFont;
        KeyboardState state, oldState;
        Tab optionTab;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = 1400;
            graphics.PreferredBackBufferHeight = 800;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hover = Content.Load<Texture2D>("square");
            filled = Content.Load<Texture2D>("redSquare");
            eraser = Content.Load<Texture2D>("eraser");
            theFont = Content.Load<SpriteFont>("theFont");
            currentImage = Content.Load<Texture2D>("tileset1big");
            textures.Add(Content.Load<Texture2D>("grasstile"));
            textures.Add(Content.Load<Texture2D>("redblock"));
            textures.Add(Content.Load<Texture2D>("orangeblock"));
            textures.Add(Content.Load<Texture2D>("yellowblock"));
            textures.Add(Content.Load<Texture2D>("blueblock"));
            folder = Content.Load<Texture2D>("folder");
            optTab = Content.Load<Texture2D>("optTitle");

            for (int i = 0; i < textures.Count; i++)
            {
                pallet.Add(new Rectangle(1056 + (32 * i), 0, 32, 32));
            }

            for (int i = 0; i < textures.Count; i++)
            {
                palletBox.Add(new BoundingBox(new Vector3(1088 + (32 * i), 0, 0), new Vector3(1056 + (32 * i) + 32, 32, 1)));
            }

            optionTab = new Tab(folder, new Rectangle(1300, 0, folder.Width, folder.Height), optTab, new BoundingBox(new Vector3(1340, 60, 0), new Vector3(1390, 160, 1)), 325);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            state = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //get mouse position
            mouseX = Mouse.GetState().Position.X;
            mouseY = Mouse.GetState().Position.Y;

            //Delete a tile from the grid
            if (mouseX < 1024)
            {
                squareX = mouseX / 32 % 32 * 32;
                squareY = mouseY / 32 % 32 * 32;

                //remove tiles from the grid
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && currentImage == null)
                {
                    for (int i = 0; i < noids.Count; i++)
                    {
                        if (noids[i].rectangle == new Rectangle(squareX, squareY, 32, 32))
                        {
                            noids.RemoveAt(i);
                        }
                    }
                }
            }

                    //add tiles onto grid
                    if (mouseX < 1024)
            {
                squareX = mouseX / 32 % 32 * 32;
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && currentImage != null)
                {
                    for (int i = 0; i < noids.Count; i++)
                    {
                        if (noids[i].rectangle == new Rectangle(squareX, squareY, 32, 32))
                        {
                            noids.RemoveAt(i);
                        }
                    }
                    placed.Add(new Rectangle(squareX, squareY, 32, 32));
                    noids.Add(new Noid(currentImage, new Rectangle(squareX, squareY, 32, 32), 1));
                }
            }


            //The square on the grid that follows the mouse
            hoverTangle = new Rectangle(squareX, squareY, hover.Width, hover.Height);
            mouseBox = new BoundingBox(new Vector3(mouseX, mouseY, 0), new Vector3(mouseX + 32, mouseY + 32, 1));
            
            //chooses from the pallet
            for (int i = 0; i < palletBox.Count; i++)
            {
                if (mouseBox.Intersects(palletBox[i]) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    currentImage = textures[i];
                }
            }
            if (mouseBox.Intersects(eraserBox) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                currentImage = null;
            }

            //Hit the option button
            if (mouseBox.Intersects(optionTab.openBox) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                optionTab.Clicked();
            }

            //File Save
            if (state.IsKeyDown(Keys.S)&& oldState.IsKeyUp(Keys.S))
            {
                string toWrite = "";
                foreach (Noid texture in noids)
                {
                    toWrite += "" + texture.texture + "," + texture.rectangle.X + ","
                        + texture.rectangle.Y + "," + texture.rectangle.Width + "," +
                        texture.rectangle.Height + "," + texture.layer + "\n";
                }
                File.WriteAllText("map2.csv", toWrite);
            }

            //File Load
            if (state.IsKeyDown(Keys.L) && oldState.IsKeyUp(Keys.L))
            {
                int tempNoidCount = noids.Count;
                for (int i = 0; i < tempNoidCount; i++)
                {
                    noids.RemoveAt(0);
                }

                string[] toRead = File.ReadAllLines("map2.csv");
                for (int i = 0; i < toRead.Length; i++)
                {
                    string[] bits = toRead[i].Split(',');
                    for (int j = 0; j < textures.Count; j++)
                    {
                        if (bits[0] == textures[j].Name)
                        {
                            noids.Add(new Noid(textures[j], new Rectangle(Convert.ToInt32(bits[1]),
                                Convert.ToInt32(bits[2]), 32, 32), Convert.ToInt32(bits[5]))); 
                        }
                    }
                }
            }


                oldState = Keyboard.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            //draw the pallet of tiles
            spriteBatch.Draw(eraser, eraserTangle, Color.White);
            for (int i = 0; i < textures.Count; i++)
            {
                spriteBatch.Draw(textures[i], pallet[i], Color.White);
            }

            //draw the hovering rectangle where the mouse is
            spriteBatch.Draw(hover, hoverTangle, Color.White);

            //Draw the tiles that have been added
            for (int i = 0; i < noids.Count; i++)
            {
                noids[i].Draw(spriteBatch);
            }

            optionTab.Draw(spriteBatch);

            //Draw for errors
            if (optionTab.open == true)
            {
                spriteBatch.DrawString(theFont, "Number of Noids: " + noids.Count, new Vector2(1180, 25), Color.Black);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
