using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MazeGame
{
    class TileLayer
    {
        public MapCell[,] map;
        Random rand = new Random();
        public EndGoal endGoal;
        public Player player;
        public List<GameObjectChild> gameObjects;

        public TileLayer()
        {
            
        }

        public enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        };

        public bool constructMap(int counter, Maps maps)
        {
            map = new MapCell[Engine.MAP_LENGTH, Engine.MAP_HEIGHT];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                row row = maps.maps[counter].rows[i];
                var chars = row.text.ToCharArray();
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    int type = rand.Next(0, 2);
                    char numchar = chars[j];
                    type = Convert.ToInt32(new string(numchar,1));
                    map[i,j] = new MapCell(type, i.ToString() + j.ToString(), j*Engine.TILE_WIDTH, i*Engine.TILE_HEIGHT, Engine.TILE_WIDTH, Engine.TILE_HEIGHT);
                }
            }
            return false;
        }

        public void constructEnd(int counter, EndGoalLocations endGoals)
        {
            endGoal = new EndGoal(3, "Target", endGoals.EndGoals[counter].XLoc, endGoals.EndGoals[counter].YLoc, Engine.ENDGOAL_WIDTH, Engine.ENDGOAL_HEIGHT);
        }

        public void constructPlayer(int counter, PlayerLocations playerLoc)
        {
            player = new Player(4, "Player", playerLoc.Players[counter].XLoc, playerLoc.Players[counter].YLoc, Engine.PLAYER_WIDTH, Engine.PLAYER_HEIGHT);
        }

        public void constructGameObjects(int counter, GameObjectLocations gameObLocs)
        {
            gameObjects = new List<GameObjectChild>();
            for (int i = 0; i < gameObLocs.GameObs[counter].GameObjects.Count; i++)
            {
                GameObjectChild temp = new GameObjectChild(gameObLocs.GameObs[counter].GameObjects[i].id, "Object", gameObLocs.GameObs[counter].GameObjects[i].XLoc, gameObLocs.GameObs[counter].GameObjects[i].YLoc, gameObLocs.GameObs[counter].GameObjects[i].Width, gameObLocs.GameObs[counter].GameObjects[i].Height);
                temp._collider = gameObLocs.GameObs[counter].GameObjects[i].Collision;
                gameObjects.Add(temp);
            }
        }

        public void loadTextures()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j]._texture = Engine.tileTypes.Single(p => p._tileID == map[i,j]._tileID)._texture;
                }
            }
            endGoal._texture = Engine.tileTypes.Single(p => p._tileID == endGoal._tileID)._texture;
            player._texture = Engine.tileTypes.Single(p => p._tileID == player._tileID)._texture;
            foreach (GameObjectChild temp in gameObjects)
            {
                temp._texture = Engine.tileTypes.Single(p => p._tileID == temp._tileID)._texture;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j].draw(spriteBatch, 0, 0);
                }
            }
            endGoal.draw(spriteBatch, 0, 0);
            player.draw(spriteBatch, 0, 0);
            foreach (GameObjectChild temp in gameObjects)
            {
                temp.draw(spriteBatch, 0, 0);
            }
            spriteBatch.End();
        }

        public void moveObject(GameObject obj, Direction dir, int speed)
        {
            switch (dir)
            {
                case Direction.UP:
                    obj._y -= speed;
                    break;
                case Direction.DOWN:
                    obj._y += speed;
                    break;
                case Direction.LEFT:
                    obj._x -= speed;
                    break;
                case Direction.RIGHT:
                    obj._x += speed;
                    break;
            }
        }

        public bool checkIfWin()
        {
            int x = (player._x + player._x + Engine.PLAYER_WIDTH)/2;
            int y = (player._y + player._y + Engine.PLAYER_HEIGHT)/2;
            if( x >= endGoal._x && x <= endGoal._x + Engine.ENDGOAL_WIDTH && y >= endGoal._y && y <= endGoal._y + Engine.ENDGOAL_HEIGHT)
            {
                return true;
            }

            return false;
        }

        public void checkIfCollision(GameObject obj, Direction dir, int speed)
        {
            int xmin = obj._x;
            int xmax = obj._x + obj._width;
            int ymin = obj._y;
            int ymax = obj._y + obj._height;

            //check if hitting border
            switch (dir)
            {
                case Direction.UP:
                    if (ymin - speed < 0)
                    {
                        moveObject(obj, dir, 0 + ymin - speed);
                    }
                    break;
                case Direction.DOWN:
                    if (ymax + speed > Engine.TILE_HEIGHT * Engine.MAP_HEIGHT)
                    {
                        moveObject(obj, dir, (Engine.TILE_HEIGHT * Engine.MAP_HEIGHT) - ymax - speed);
                    }
                    break;
                case Direction.LEFT:
                    if (xmin - speed < 0)
                    {
                        moveObject(obj, dir, 0 + xmin - speed);
                    }
                    break;
                case Direction.RIGHT:
                    if (xmax + speed > Engine.TILE_WIDTH * Engine.MAP_LENGTH)
                    {
                        moveObject(obj, dir, (Engine.TILE_WIDTH * Engine.MAP_LENGTH) - xmax - speed);
                    }
                    break;
            }

            //check if hitting collidable background
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (dir)
                    {
                        case Direction.UP:
                            if (map[i, j]._collider)
                            {
                                if ((ymin - speed >= map[i, j]._y && ymin - speed <= map[i, j]._y + map[i, j]._height) &&
                                    ((xmin >= map[i, j]._x && xmin <= map[i, j]._x + map[i, j]._width) || (xmax >= map[i, j]._x && xmax <= map[i, j]._x + map[i, j]._width)))
                                {
                                    moveObject(obj, dir, ymin - (map[i, j]._y + map[i, j]._height) - speed - 1);
                                }
                            }
                            break;
                        case Direction.DOWN:
                            if (map[i, j]._collider)
                            {
                                if ((ymax + speed >= map[i, j]._y && ymax + speed <= map[i, j]._y + map[i, j]._height) &&
                                    ((xmin >= map[i, j]._x && xmin <= map[i, j]._x + map[i, j]._width) || (xmax >= map[i, j]._x && xmax <= map[i, j]._x + map[i, j]._width)))
                                {
                                    moveObject(obj, dir, (map[i, j]._y) - ymax - speed - 1);
                                }
                            }
                            break;
                        case Direction.LEFT:
                            if (map[i, j]._collider)
                            {
                                if ((xmin - speed >= map[i, j]._x && xmin - speed <= map[i, j]._x + map[i, j]._width) &&
                                    ((ymin >= map[i, j]._y && ymin <= map[i, j]._y + map[i, j]._height) || (ymax >= map[i, j]._y && ymax <= map[i, j]._y + map[i, j]._height)))
                                {
                                    moveObject(obj, dir, xmin - (map[i, j]._x + map[i, j]._width) - speed - 1);
                                }
                            }
                            break;
                        case Direction.RIGHT:
                            if (map[i, j]._collider)
                            {
                                if ((xmax + speed >= map[i, j]._x && xmax + speed <= map[i, j]._x + map[i, j]._width) &&
                                    ((ymin >= map[i, j]._y && ymin <= map[i, j]._y + map[i, j]._height) || (ymax >= map[i, j]._y && ymax <= map[i, j]._y + map[i, j]._height)))
                                {
                                    moveObject(obj, dir, (map[i, j]._x) - xmax - speed - 1);
                                }
                            }
                            break;
                    }
                }
            }

            //check if colliding with collidable gameobject
            for (int i = 0; i < gameObjects.Count; i++)
            {
                switch (dir)
                {
                    case Direction.UP:
                        if (gameObjects[i]._collider)
                        {
                            if ((ymin - speed >= gameObjects[i]._y && ymin - speed <= gameObjects[i]._y + gameObjects[i]._height) &&
                                ((xmin >= gameObjects[i]._x && xmin <= gameObjects[i]._x +gameObjects[i]._width) || (xmax >=gameObjects[i]._x && xmax <=gameObjects[i]._x +gameObjects[i]._width)))
                            {
                                moveObject(obj, dir, ymin - (gameObjects[i]._y +gameObjects[i]._height) - speed - 1);
                            }
                        }
                        break;
                    case Direction.DOWN:
                        if (gameObjects[i]._collider)
                        {
                            if ((ymax + speed >=gameObjects[i]._y && ymax + speed <=gameObjects[i]._y +gameObjects[i]._height) &&
                                ((xmin >=gameObjects[i]._x && xmin <=gameObjects[i]._x +gameObjects[i]._width) || (xmax >=gameObjects[i]._x && xmax <=gameObjects[i]._x +gameObjects[i]._width)))
                            {
                                moveObject(obj, dir, (gameObjects[i]._y) - ymax - speed - 1);
                            }
                        }
                        break;
                    case Direction.LEFT:
                        if (gameObjects[i]._collider)
                        {
                            if ((xmin - speed >=gameObjects[i]._x && xmin - speed <=gameObjects[i]._x +gameObjects[i]._width) &&
                                ((ymin >=gameObjects[i]._y && ymin <=gameObjects[i]._y +gameObjects[i]._height) || (ymax >=gameObjects[i]._y && ymax <=gameObjects[i]._y +gameObjects[i]._height)))
                            {
                                moveObject(obj, dir, xmin - (gameObjects[i]._x +gameObjects[i]._width) - speed - 1);
                            }
                        }
                        break;
                    case Direction.RIGHT:
                        if (gameObjects[i]._collider)
                        {
                            if ((xmax + speed >=gameObjects[i]._x && xmax + speed <=gameObjects[i]._x +gameObjects[i]._width) &&
                                ((ymin >=gameObjects[i]._y && ymin <=gameObjects[i]._y +gameObjects[i]._height) || (ymax >=gameObjects[i]._y && ymax <=gameObjects[i]._y +gameObjects[i]._height)))
                            {
                                moveObject(obj, dir, (gameObjects[i]._x) - xmax - speed - 1);
                            }
                        }
                        break;
                }
            }

            moveObject(obj, dir, speed);

        }
    }
}
