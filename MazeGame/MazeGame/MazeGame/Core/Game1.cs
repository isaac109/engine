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
using System.Xml;
using System.Xml.Serialization;
using System.IO;


namespace MazeGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
         
        TileLayer layer = new TileLayer();

        MouseInput mouseInput = new MouseInput();
        KeyInput keyInput = new KeyInput();
        bool leftButtonPressed;
        bool won = false;
        int lvlConter = 0;
        SpriteFont spriteFont;
        Maps maps;
        PlayerLocations playerLocs;
        EndGoalLocations endGoals;
        GameObjectLocations gameObLocations;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            FileStream fileStream = new FileStream("Content\\GameContent\\Maps.xml", FileMode.Open);
            XmlSerializer xml = new XmlSerializer(typeof(Maps));
            maps = (Maps)xml.Deserialize(fileStream);
            fileStream = new FileStream("Content\\GameContent\\PlayerLocations.xml", FileMode.Open);
            xml = new XmlSerializer(typeof(PlayerLocations));
            playerLocs = (PlayerLocations)xml.Deserialize(fileStream);
            fileStream = new FileStream("Content\\GameContent\\EndGoalLocations.xml", FileMode.Open);
            xml = new XmlSerializer(typeof(EndGoalLocations));
            endGoals = (EndGoalLocations)xml.Deserialize(fileStream);
            fileStream = new FileStream("Content\\GameContent\\GameObjectLocations.xml", FileMode.Open);
            xml = new XmlSerializer(typeof(GameObjectLocations));
            gameObLocations = (GameObjectLocations)xml.Deserialize(fileStream);
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
            this.IsMouseVisible = true;
            
            leftButtonPressed = false;
            Engine.tileTypes.Add(new Engine.TileType("Textures/black", true, 1));
            Engine.tileTypes.Add(new Engine.TileType("Textures/white", false, 0));
            Engine.tileTypes.Add(new Engine.TileType("Textures/red", false, 3));
            Engine.tileTypes.Add(new Engine.TileType("Textures/yellow", false, 4));
            Engine.tileTypes.Add(new Engine.TileType("Textures/tree",false, 5));
            Engine.tileTypes.Add(new Engine.TileType("Textures/tree2", false, 6));
            initilaizeTiles();
            base.Initialize();
        }

        public void initilaizeTiles()
        {
            layer.constructMap(lvlConter, maps);
            layer.constructEnd(lvlConter, endGoals);
            layer.constructPlayer(lvlConter, playerLocs);
            layer.constructGameObjects(lvlConter, gameObLocations);
            lvlConter++;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (Engine.TileType type in Engine.tileTypes)
            {
                type.LoadContent(Content);
            }
            layer.loadTextures();
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");            

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
            if (!won)
            {
                mouseInput.updateState();
                mouseInput.updatePosition();

                if (mouseInput.state.LeftButton == ButtonState.Pressed && !leftButtonPressed)
                {
                    leftButtonPressed = true;
                    getCurrentTile();
                }
                if (mouseInput.state.LeftButton != ButtonState.Pressed)
                {
                    leftButtonPressed = false;
                }

                keyInput.updateState();

                if (keyInput.oldState.IsKeyDown(Keys.Up) || keyInput.oldState.IsKeyDown(Keys.W))
                {
                    layer.checkIfCollision(layer.player, TileLayer.Direction.UP, Engine.PLAYER_SPEED);
                }
                if (keyInput.oldState.IsKeyDown(Keys.Down) || keyInput.oldState.IsKeyDown(Keys.S))
                {
                    layer.checkIfCollision(layer.player, TileLayer.Direction.DOWN, Engine.PLAYER_SPEED);
                }
                if (keyInput.oldState.IsKeyDown(Keys.Left) || keyInput.oldState.IsKeyDown(Keys.A))
                {
                    layer.checkIfCollision(layer.player, TileLayer.Direction.LEFT, Engine.PLAYER_SPEED);
                }
                if (keyInput.oldState.IsKeyDown(Keys.Right) || keyInput.oldState.IsKeyDown(Keys.D))
                {
                    layer.checkIfCollision(layer.player, TileLayer.Direction.RIGHT, Engine.PLAYER_SPEED);
                }
                if (maps.maps.Count == lvlConter)
                {
                    won = layer.checkIfWin();
                }
                else
                {
                    if (layer.checkIfWin())
                    {
                        initilaizeTiles();
                    }
                }
            }
            base.Update(gameTime);
        }
        void printMouse(int x, int y)
        {
            Console.WriteLine("x= " + x.ToString() + ",y= " + y.ToString());
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if (!won)
            {
                layer.Draw(spriteBatch);
            }
            else
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(spriteFont, "You Solved The Maze!", new Vector2(150, 150), Color.Black);
                spriteBatch.End();
            }
            base.Draw(gameTime);
        }

        void getCurrentTile()
        {
            int x = mouseInput.mousePosition.X / 64;
            int y = mouseInput.mousePosition.Y / 64;
            //Console.WriteLine(layer.map[x, y]._name);
            //printMouse(x, y);
        }
    }
}
