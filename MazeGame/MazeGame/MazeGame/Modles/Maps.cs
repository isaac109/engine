using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MazeGame
{
    public class Maps
    {
        public Maps()
        {
            maps = new List<Map>();
        }
        public List<Map> maps;
    }
    public class Map
    {
        public Map()
        {
            rows = new List<row>();
            Player = new List<PlayerLoc>();
            EndGoal = new List<EndGoalLoc>();
            GameObjects = new List<GameObLoc>();
            Triggers = new List<TriggerLoc>();
        }
        public int id;
        public List<row> rows;
        public List<PlayerLoc> Player;
        public List<EndGoalLoc> EndGoal;
        public List<EnemyInfo> Enemies;
        public List<GameObLoc> GameObjects;
        public List<TriggerLoc> Triggers;
    }
    public class Info
    {
        public int Item;
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
    public class EndGoalLoc
    {
        public EndGoalLoc()
        {
        }
        public int XLoc;
        public int YLoc;
    }
    public class PlayerLoc
    {
        public int XLoc;
        public int YLoc;
    }
    public class row
    {
        public string text;
    }

}
