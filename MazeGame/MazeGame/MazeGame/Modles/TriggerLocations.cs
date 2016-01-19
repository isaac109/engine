using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame
{
    public class TriggerLocations
    {
        public List<TriggerLevels> Triggers;
        public TriggerLocations()
        {
            Triggers = new List<TriggerLevels>();
        }
    }
    public class TriggerLevels
    {
        public List<TriggerLoc> TriggerObjects;
        public TriggerLevels()
        {
            TriggerObjects = new List<TriggerLoc>();
        }
    }
    public class TriggerLoc
    {
        public List<Info> InfoItems;
        public int id;
        public int XLoc;
        public int YLoc;
        public int Width;
        public int Height;
        public TriggerLoc()
        {
            InfoItems = new List<Info>();
        }
    }
    public class Info
    {
        public int Item;
    }
}
