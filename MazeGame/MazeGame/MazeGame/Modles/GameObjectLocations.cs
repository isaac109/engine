using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame
{
    public class GameObjectLocations
    {
        public GameObjectLocations()
        {
            GameObs = new List<Levels>();
        }
        public List<Levels> GameObs;
    }

    public class Levels
    {
        public Levels()
        {
            GameObjects = new List<GameObLoc>();
        }
        public List<GameObLoc> GameObjects;
    }

    public class GameObLoc
    {
        public GameObLoc()
        {
        }
        public int id;
        public int XLoc;
        public int YLoc;
        public int Width;
        public int Height;
        public bool Collision;
    }
}
