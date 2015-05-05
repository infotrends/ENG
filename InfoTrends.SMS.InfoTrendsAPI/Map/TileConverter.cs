using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InfoTrendsAPI.Map
{
    public class TileConverter
    {
        public static int WORLD_MERCATOR 
            = 54004;

        public static int GCS_WGS_1984 
            = 4326;

        public static int NAD_1983_To_WGS_1984_1 
            = 1188;

        /// <summary>
        /// Convert lat, lng of a point to the Google Mercator unit (pixel). 0,0 is upper left of the expanded global at each zoom level. 
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public static Pixel FromLatLngToPixel(double lat, double lng, int zoom)
        {
            long numberOfTiles = 1 << zoom;
            long originX = numberOfTiles * 256 / 2;
            long originY = numberOfTiles * 256 / 2;
            double pixelsPerLngDegree = numberOfTiles * 256 / 360.0;
            double pixelsPerLngRadian = numberOfTiles * 256 / (2 * Math.PI);
            int x = System.Convert.ToInt32(originX + lng * pixelsPerLngDegree);
            double f = Math.Min(Math.Max(Math.Sin(lat * Math.PI / 180), -0.99999), 0.9999);
            int y = System.Convert.ToInt32(originY + 0.5 * Math.Log((1 + f) / (1 - f)) * (-1) * pixelsPerLngRadian);
            return new Pixel(x, y);
            //double sinLat = Math.Sin(lat * Math.PI / 180.0);
            //int totalPixels=256 << zoom; // 256 * 2^zoom;
            //double x = ((lng + 180.0) / 360.0) * totalPixels;
            //double y = (0.5 - Math.Log((1 + sinLat) / (1 - sinLat)) / (4 * Math.PI)) * totalPixels;
            //return new Pixel((int)(x+0.5), (int)(y+0.5));
        }

        public static Pixel FromLatLngToPixel(LatLng latlng, int zoom)
        {
            return FromLatLngToPixel(latlng.Lat, latlng.Lng, zoom);
        }

        /// <summary>
        /// Convert Google unit(pixel) to Lat Long. 0,0 is upper left of the expanded global at each zoom level. 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public static LatLng FromPixelToLatLng(int pixelx, int pixely, int zoom)
        {
            long numberOfTiles = 1 << zoom;
            long originX = numberOfTiles * 256 / 2;
            long originY = numberOfTiles * 256 / 2;
            double pixelsPerLngDegree = numberOfTiles * 256 / 360.0;
            double pixelsPerLngRadian = numberOfTiles * 256 / (2 * Math.PI);
            double lng = (pixelx - originX) / pixelsPerLngDegree;
            double g = (pixely - originY) / pixelsPerLngRadian * -1;
            double lat = (2 * Math.Atan(Math.Exp(g)) - Math.PI / 2) / (Math.PI / 180);
            return new LatLng(lat, lng);



        }
        public static LatLng FromPixelToLatLng(Pixel p, int zoom)
        {
            return FromPixelToLatLng(p.X, p.Y, zoom);
        }

        /// <summary>
        /// Get the tile reference for a lat lng point
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public static TileReference GetTileReferenceByLatLng(LatLng latlng, int zoom, int tileSize)
        {
            return GetTileReferenceByLatLng(latlng.Lat, latlng.Lng, zoom, tileSize);
        }
        public static TileReference GetTileReferenceByLatLng(double lat, double lng, int zoom, int tileSize)
        {
            Pixel pixel = FromLatLngToPixel(lat, lng, zoom);
            int x = System.Convert.ToInt32(pixel.X / tileSize);
            int y = System.Convert.ToInt32(pixel.Y / tileSize);
            return new TileReference(x, y, zoom);
        }


        /// <summary>
        /// Get the lat/lon of the southwest, northeast corner of a tile
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public static LatLngBounds GetLatLngBoundsByTileReference(int x, int y, int zoom, int tileSize, int steps, int pixelBuffer)
        {
            LatLng sw = FromPixelToLatLng(x * tileSize - pixelBuffer, (y + 1) * tileSize + pixelBuffer, zoom);
            LatLng ne = FromPixelToLatLng((x + steps) * tileSize + pixelBuffer, (y + 1 - steps) * tileSize - pixelBuffer, zoom);
            return new LatLngBounds(sw, ne);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="tileSize"></param>
        /// <param name="pixelBuffer"></param>
        /// <returns></returns>
        public static LatLngBounds GetLatLngBoundsByTileReference(TileReference tile, int tileSize, int step, int pixelBuffer)
        {
            return GetLatLngBoundsByTileReference(tile.X, tile.Y, tile.Zoom, tileSize, step, pixelBuffer);
        }

    }
}
