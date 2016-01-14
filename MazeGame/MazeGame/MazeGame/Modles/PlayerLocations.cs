using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame
{
    public class PlayerLocations
    {
        public PlayerLocations()
        {
            Players = new List<PlayerLoc>();
        }
        public List<PlayerLoc> Players;
    }
    public class PlayerLoc
    {
        public PlayerLoc()
        {
        }
        public int XLoc;
        public int YLoc;
    }

}
