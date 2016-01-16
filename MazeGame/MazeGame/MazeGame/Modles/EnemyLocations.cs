using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame
{
    public class EnemyLocations
    {
        public EnemyLocations()
        {
            Enemies = new List<EnemyLevels>();
        }
        public List<EnemyLevels> Enemies;
    }

    public class EnemyLevels
    {
        public EnemyLevels()
        {
            EnemyObjects = new List<EnemyInfo>();
        }
        public List<EnemyInfo> EnemyObjects;
    }

    public class EnemyInfo
    {
        public EnemyInfo()
        {
        }
        public int id;
        public int XLoc;
        public int YLoc;
        public int Width;
        public int Height;
        public bool Collision;
        public bool Horizontal;
        public bool Left;
        public bool Up;
        public int Speed;
    }
}
