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
        }
        public int id;
        public List<row> rows;
    }
    public class row
    {
        public string text;
    }
}
