using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoTrendsAPI.Map
{
    public class TileReference
    {
        /// <summary>
        /// X reference, 0 is left. 
        /// </summary>
        public int X
        { get; set; }



        /// <summary>
        /// Y reference. 0 is top.
        /// </summary>
        public int Y
        { get; set; }
        

        /// <summary>
        /// Zoom level. 0 is global, increase to detail.
        /// </summary>
        public int Zoom
        { get; set; }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="zoom"></param>
        public TileReference(int x, int y, int zoom)
        {
            this.X = x;
            this.Y = y;
            this.Zoom = zoom;
        }



        /// <summary>
        /// Google's tile reference system. Same as output. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public static TileReference fromGoogleTile(int x, int y, int zoom)
        {
            return new TileReference(x, y, zoom);
        }


        public override bool Equals(object o)
        {
            if (!(o is TileReference) || o == null) return false;
            TileReference tile = (TileReference)o;
            return this.X == tile.X && this.Y == tile.Y && this.Zoom == tile.Zoom;
        }


        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
