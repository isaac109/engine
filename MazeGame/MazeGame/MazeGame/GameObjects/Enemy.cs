using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame
{
    class Enemy : GameObject
    {
        public int _tileID;

        public string _name;

        public bool _horizontal;
        public bool _left;
        public bool _up;
        public int _speed;
        public Direction direction;

        public Enemy(int tileID, string name, int x, int y, int width, int height, bool horizontal, bool left, bool up, int speed): base(x, y, Engine.tileTypes.Single(p => p._tileID == tileID)._collider, width, height)
        {
            _tileID = tileID;
            _name = name;
            _texture = Engine.tileTypes.Single(p => p._tileID == tileID)._texture;
            _horizontal = horizontal;
            _left = left;
            _up = up;
            _speed = speed;
        }

        public void checkIfIsColliding()
        {
            if (this._isColliding)
            {
                if (_horizontal)
                {
                    if (direction == Direction.LEFT)
                    {
                        direction = Direction.RIGHT;
                        this._isColliding = false;
                    }
                    else
                    {
                        direction = Direction.LEFT;
                        this._isColliding = false;
                    }
                }
                else
                {
                    if (direction == Direction.UP)
                    {
                        direction = Direction.DOWN;
                        this._isColliding = false;
                    }
                    else
                    {
                        direction = Direction.UP;
                        this._isColliding = false;
                    }
                }
            }
        }
    }
}
