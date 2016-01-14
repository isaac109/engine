using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame
{
    public class EndGoalLocations
    {
        public EndGoalLocations()
        {
            EndGoals = new List<EndGoalLoc>();
        }
        public List<EndGoalLoc> EndGoals;
    }
    public class EndGoalLoc
    {
        public EndGoalLoc()
        {
        }
        public int XLoc;
        public int YLoc;
    }
}
