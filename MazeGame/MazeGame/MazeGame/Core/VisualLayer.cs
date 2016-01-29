using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

// this is where the basic logic for the game's level occurs
namespace MazeGame
{
    class TileLayer
    {
        public MapCell[,] map;
        Random rand = new Random();
        public EndGoal endGoal;
        public Player player;
        public List<Enemy> enemies;
        public List<GameObjectChild> gameObjects;
        public List<Trigger> triggers;

        public TileLayer()
        {
            
        }

        public bool constructMap(int counter, Maps maps)
        {
            map = new MapCell[Engine.MAP_LENGTH, Engine.MAP_HEIGHT];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                row row = maps.maps[counter].rows[i];
                var chars = row.text.ToCharArray();
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    int type = 0;
                    char numchar = chars[j];
                    type = Convert.ToInt32(new string(numchar,1));
                    map[i,j] = new MapCell(type, i.ToString() + j.ToString(), j*Engine.TILE_WIDTH, i*Engine.TILE_HEIGHT, Engine.TILE_WIDTH, Engine.TILE_HEIGHT);
                }
            }
            constructPlayer(counter, maps);
            constructEnd(counter, maps);
            constructEnemies(counter, maps);
            constructGameObjects(counter, maps);
            constructTriggers(counter, maps);
            return false;
        }

        public void constructPlayer(int counter, Maps maps)
        {
            player = new Player(4, "Player", maps.maps[counter].Player[0].XLoc, maps.maps[counter].Player[0].YLoc, Engine.PLAYER_WIDTH, Engine.PLAYER_HEIGHT);
        }

        public void constructEnd(int counter, Maps maps)
        {
            endGoal = new EndGoal(3, "Target", maps.maps[counter].EndGoal[0].XLoc, maps.maps[counter].EndGoal[0].YLoc, Engine.ENDGOAL_WIDTH, Engine.ENDGOAL_HEIGHT);
        }

        public void constructEnemies(int counter, Maps maps)
        {
            enemies = new List<Enemy>();
            for (int i = 0; i < maps.maps[counter].Enemies.Count; i++)
            {
                Enemy temp = new Enemy(maps.maps[counter].Enemies[i].id, "Object",
                    maps.maps[counter].Enemies[i].XLoc, maps.maps[counter].Enemies[i].YLoc,
                    maps.maps[counter].Enemies[i].Width, maps.maps[counter].Enemies[i].Height,
                    maps.maps[counter].Enemies[i].Horizontal, maps.maps[counter].Enemies[i].Left,
                    maps.maps[counter].Enemies[i].Up, maps.maps[counter].Enemies[i].Speed);
                if (temp._horizontal)
                {
                    if (temp._left)
                    {
                        temp.direction = Direction.LEFT;
                    }
                    else
                    {
                        temp.direction = Direction.RIGHT;
                    }
                }
                else
                {
                    if (temp._up)
                    {
                        temp.direction = Direction.UP;
                    }
                    else
                    {
                        temp.direction = Direction.DOWN;
                    }
                }

                enemies.Add(temp);
            }
        }

        public void constructGameObjects(int counter, Maps maps)
        {
            gameObjects = new List<GameObjectChild>();
            for (int i = 0; i < maps.maps[counter].GameObjects.Count; i++)
            {
                GameObjectChild temp = new GameObjectChild(maps.maps[counter].GameObjects[i].id,
                    "Object", maps.maps[counter].GameObjects[i].XLoc, maps.maps[counter].GameObjects[i].YLoc,
                    maps.maps[counter].GameObjects[i].Width, maps.maps[counter].GameObjects[i].Height);
                temp._collider = maps.maps[counter].GameObjects[i].Collision;
                gameObjects.Add(temp);
            }
        }

        public void constructTriggers(int counter, Maps maps)
        {
            triggers = new List<Trigger>();
            for (int i = 0; i < maps.maps[counter].Triggers.Count; i++)
            {
                List<int> tempList = new List<int>();
                for (int j = 0; j < maps.maps[counter].Triggers[i].InfoItems.Count; j++)
                {
                    int tempint = maps.maps[counter].Triggers[i].InfoItems[j].Item;
                    tempList.Add(tempint);
                }
                Trigger temp = new Trigger(maps.maps[counter].Triggers[i].id,
                    "Trigger", maps.maps[counter].Triggers[i].XLoc, maps.maps[counter].Triggers[i].YLoc,
                    maps.maps[counter].Triggers[i].Width, maps.maps[counter].Triggers[i].Height,
                    tempList);
                temp._collider = false;
                triggers.Add(temp);
            }
        }
      
        public void runTrigger(List<int> items, Trigger temp)
        {
            switch (items[0])
            {
                case 0:
                    changeMap(items[1], items[2]);
                    break;
                case 1:
                    killEnemy(items[1], temp);
                    break;
            }
        }

        public void changeMap(int x, int y)
        {
            map[x, y]._collider = !map[x, y]._collider;
            if (map[x, y]._tileID == 0)
            {
                map[x, y]._tileID = 1;
            }
            else
            {
                map[x, y]._tileID = 0;
            }
            map[x, y]._texture = Engine.tileTypes.Single(p => p._tileID == map[x, y]._tileID)._texture;
        }

        public void killEnemy(int i, Trigger temp)
        {
            if (!temp.enemyKilled)
            {
                temp.enemyKilled = true;
                enemies.RemoveAt(i);
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
            foreach (Enemy temp in enemies)
            {
                temp._texture = Engine.tileTypes.Single(p => p._tileID == temp._tileID)._texture;
            }
            foreach (Trigger temp in triggers)
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
            foreach (Trigger temp in triggers)
            {
                temp.draw(spriteBatch, 0, 0);
            }
            player.draw(spriteBatch, 0, 0);
            foreach (Enemy temp in enemies)
            {
                temp.draw(spriteBatch, 0, 0);
            }
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
            int[] xcorners = { player._x, player._x + player._width };
            int[] ycorners = { player._y, player._y + player._height };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (xcorners[i] >= endGoal._x && xcorners[i] <= endGoal._x + endGoal._width && 
                        ycorners[j] >= endGoal._y && ycorners[j] <= endGoal._y + endGoal._height)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool checkIfHitEnemy()
        {
            int[] xcorners = { player._x, player._x + player._width };
            int[] ycorners = { player._y, player._y + player._height };

            foreach (Enemy temp in enemies)
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (xcorners[i] >= temp._x && xcorners[i] <= temp._x + temp._width && 
                            ycorners[j] >= temp._y && ycorners[j] <= temp._y + temp._height)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public void checkIfHitTrigger()
        {
            int[] xcorners = { player._x, player._x + player._width };
            int[] ycorners = { player._y, player._y + player._height };

            foreach (Trigger temp in triggers)
            {
                bool hit = false;
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (xcorners[i] >= temp._x && xcorners[i] <= temp._x + temp._width &&
                            ycorners[j] >= temp._y && ycorners[j] <= temp._y + temp._height)
                        {
                            hit = true;
                        }
                    }
                }
                if (hit)
                {
                    if (!temp._isInTrigger)
                    {
                        temp._isInTrigger = true;
                        runTrigger(temp._items, temp);
                    }
                }
                else
                {
                    temp._isInTrigger = false;
                }
            }
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
                        obj._isColliding = true;
                    }
                    break;
                case Direction.DOWN:
                    if (ymax + speed > Engine.TILE_HEIGHT * Engine.MAP_HEIGHT)
                    {
                        moveObject(obj, dir, (Engine.TILE_HEIGHT * Engine.MAP_HEIGHT) - ymax - speed);
                        obj._isColliding = true;
                    }
                    break;
                case Direction.LEFT:
                    if (xmin - speed < 0)
                    {
                        moveObject(obj, dir, 0 + xmin - speed);
                        obj._isColliding = true;
                    }
                    break;
                case Direction.RIGHT:
                    if (xmax + speed > Engine.TILE_WIDTH * Engine.MAP_LENGTH)
                    {
                        moveObject(obj, dir, (Engine.TILE_WIDTH * Engine.MAP_LENGTH) - xmax - speed);
                        obj._isColliding = true;
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
                                    ((xmin >= map[i, j]._x && xmin <= map[i, j]._x + map[i, j]._width) || 
                                    (xmax >= map[i, j]._x && xmax <= map[i, j]._x + map[i, j]._width)))
                                {
                                    moveObject(obj, dir, ymin - (map[i, j]._y + map[i, j]._height) - speed - 1);
                                    obj._isColliding = true;
                                }
                            }
                            break;
                        case Direction.DOWN:
                            if (map[i, j]._collider)
                            {
                                if ((ymax + speed >= map[i, j]._y && ymax + speed <= map[i, j]._y + map[i, j]._height) &&
                                    ((xmin >= map[i, j]._x && xmin <= map[i, j]._x + map[i, j]._width) || 
                                    (xmax >= map[i, j]._x && xmax <= map[i, j]._x + map[i, j]._width)))
                                {
                                    moveObject(obj, dir, (map[i, j]._y) - ymax - speed - 1);
                                    obj._isColliding = true;
                                }
                            }
                            break;
                        case Direction.LEFT:
                            if (map[i, j]._collider)
                            {
                                if ((xmin - speed >= map[i, j]._x && xmin - speed <= map[i, j]._x + map[i, j]._width) &&
                                    ((ymin >= map[i, j]._y && ymin <= map[i, j]._y + map[i, j]._height) || 
                                    (ymax >= map[i, j]._y && ymax <= map[i, j]._y + map[i, j]._height)))
                                {
                                    moveObject(obj, dir, xmin - (map[i, j]._x + map[i, j]._width) - speed - 1);
                                    obj._isColliding = true;
                                }
                            }
                            break;
                        case Direction.RIGHT:
                            if (map[i, j]._collider)
                            {
                                if ((xmax + speed >= map[i, j]._x && xmax + speed <= map[i, j]._x + map[i, j]._width) &&
                                    ((ymin >= map[i, j]._y && ymin <= map[i, j]._y + map[i, j]._height) || 
                                    (ymax >= map[i, j]._y && ymax <= map[i, j]._y + map[i, j]._height)))
                                {
                                    moveObject(obj, dir, (map[i, j]._x) - xmax - speed - 1);
                                    obj._isColliding = true;
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
                                ((xmin >= gameObjects[i]._x && xmin <= gameObjects[i]._x +gameObjects[i]._width) || 
                                (xmax >=gameObjects[i]._x && xmax <=gameObjects[i]._x +gameObjects[i]._width)))
                            {
                                moveObject(obj, dir, ymin - (gameObjects[i]._y +gameObjects[i]._height) - speed - 1);
                                obj._isColliding = true;
                            }
                        }
                        break;
                    case Direction.DOWN:
                        if (gameObjects[i]._collider)
                        {
                            if ((ymax + speed >=gameObjects[i]._y && ymax + speed <=gameObjects[i]._y +gameObjects[i]._height) &&
                                ((xmin >=gameObjects[i]._x && xmin <=gameObjects[i]._x +gameObjects[i]._width) || 
                                (xmax >=gameObjects[i]._x && xmax <=gameObjects[i]._x +gameObjects[i]._width)))
                            {
                                moveObject(obj, dir, (gameObjects[i]._y) - ymax - speed - 1);
                                obj._isColliding = true;
                            }
                        }
                        break;
                    case Direction.LEFT:
                        if (gameObjects[i]._collider)
                        {
                            if ((xmin - speed >=gameObjects[i]._x && xmin - speed <=gameObjects[i]._x +gameObjects[i]._width) &&
                                ((ymin >=gameObjects[i]._y && ymin <=gameObjects[i]._y +gameObjects[i]._height) || 
                                (ymax >=gameObjects[i]._y && ymax <=gameObjects[i]._y +gameObjects[i]._height)))
                            {
                                moveObject(obj, dir, xmin - (gameObjects[i]._x +gameObjects[i]._width) - speed - 1);
                                obj._isColliding = true;
                            }
                        }
                        break;
                    case Direction.RIGHT:
                        if (gameObjects[i]._collider)
                        {
                            if ((xmax + speed >=gameObjects[i]._x && xmax + speed <=gameObjects[i]._x +gameObjects[i]._width) &&
                                ((ymin >=gameObjects[i]._y && ymin <=gameObjects[i]._y +gameObjects[i]._height) || 
                                (ymax >=gameObjects[i]._y && ymax <=gameObjects[i]._y +gameObjects[i]._height)))
                            {
                                moveObject(obj, dir, (gameObjects[i]._x) - xmax - speed - 1);
                                obj._isColliding = true;
                            }
                        }
                        break;
                }
            }

            moveObject(obj, dir, speed);

        }
    }
}
