using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infiniminers_v0._0
{
    public class Ore
    {
        public int X {  get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        public int Value { get; set; }
        public Color OreColor { get; set; }

        public Ore(int x, int y, int value, Color color)
        {
            Random rnd = new Random();
            X = x;
            Y = y;
            Value = value;
            Size = rnd.Next(30, 70);
            OreColor = color;
        }
    }
}
