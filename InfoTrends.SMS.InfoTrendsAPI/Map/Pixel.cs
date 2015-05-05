using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoTrendsAPI.Map
{
    public class Pixel
    {

        public int X
        { get; set; }

        public int Y
        { get; set; }

        public Pixel(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

    }
}
