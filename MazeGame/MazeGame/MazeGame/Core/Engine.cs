using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MazeGame
{
    class Engine
    {
        public const int TILE_WIDTH = 64;
        public const int TILE_HEIGHT = 64;
        public const int PLAYER_WIDTH = 32;
        public const int PLAYER_HEIGHT = 32;
        public const int PLAYER_SPEED = 5;
        public const int ENDGOAL_WIDTH = 32;
        public const int ENDGOAL_HEIGHT = 32;
        public const int MAP_LENGTH = 6;
        public const int MAP_HEIGHT = 6;
        public static List<TileType> tileTypes = new List<TileType>();
        public class TileType: GameObjectType
        {
            public int _tileID;
           

            public TileType(string textureName, bool collider, int id):base(textureName, collider)
            {
                _tileID = id;
            }

            
        }
        public class GameObjectType
        {
            public Texture2D _texture;
            public string _textureName;
            public bool _collider;
            
            public GameObjectType(string textureName, bool collider)
            {
                _collider = collider;
                _textureName = textureName;
            }

            public void LoadContent(ContentManager content)
            {
                _texture = content.Load<Texture2D>(_textureName);
            }
        }
    }
    public enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    };
}
