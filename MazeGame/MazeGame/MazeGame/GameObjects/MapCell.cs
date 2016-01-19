using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;


namespace MazeGame
{
    class MapCell : GameObject
    {
        public int _tileID;

        public string _name;

        public MapCell(int tileID, string name, int x, int y, int width, int height):
            base( x, y, Engine.tileTypes.Single(p => p._tileID == tileID)._collider, width, height)
        {
            _tileID = tileID;
            _name = name;
            _texture = Engine.tileTypes.Single(p => p._tileID == tileID)._texture;
        }
    }
}
