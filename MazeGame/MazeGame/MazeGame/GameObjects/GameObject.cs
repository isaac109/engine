using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MazeGame
{
    class GameObject
    {
        public Texture2D _texture;
        public int _x;
        public int _y;
        public int _width;
        public int _height;
        public bool _collider;
        public List<GameObject> decorations = new List<GameObject>();

        public GameObject()
        { 
        }

        public GameObject(int x, int y, bool collider, int width, int height)
        {
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _collider = collider;
        }

        public void draw(SpriteBatch spriteBatch, int x, int y)
        {
            spriteBatch.Draw(_texture, new Rectangle(_x + x, _y + y, _width, _height), Color.White);
            foreach (GameObject dec in decorations)
            {
                dec.draw(spriteBatch,_x + x,_y+y);
            }
        }
    }
}
