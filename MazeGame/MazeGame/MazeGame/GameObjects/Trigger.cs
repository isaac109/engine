using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame
{
    class Trigger:GameObject
    {
        public int _tileID;

        public string _name;

        public bool _isInTrigger = false;

        public List<int> _items;

        public bool enemyKilled = false;

        public Trigger(int tileID, string name, int x, int y, int width, int height, List<int> items): 
            base(x, y, Engine.tileTypes.Single(p => p._tileID == tileID)._collider, width, height)
        {
            _tileID = tileID;
            _name = name;
            _texture = Engine.tileTypes.Single(p => p._tileID == tileID)._texture;
            _items = items;
        }

    }
}
