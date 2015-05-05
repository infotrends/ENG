using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace InfoTrendsAPI.Map
{
    public class Polygon
    {
        public const int ZOOM = 10;


        /// <summary>
        /// Using List of LatLng
        /// </summary>
        /// <param name="latLngArray">List of LatLng</param>
        /// <param name="testLatLng">LatLng to test for</param>
        /// <returns></returns>
        public static bool isIntersect(List<LatLng> latLngArray, LatLng testLatLng)
        {
            int count = latLngArray.Count - 1;
			int counter = 0;
			double xinters = 0;
			Pixel p, p1, p2;

            p = TileConverter.FromLatLngToPixel(testLatLng, Polygon.ZOOM); ;
            p1 = TileConverter.FromLatLngToPixel(latLngArray[0], Polygon.ZOOM);
			for (int i=0; i<= count; i++){
                p2 = TileConverter.FromLatLngToPixel(latLngArray[i % count], Polygon.ZOOM);
                if (p.Y > Math.Min(p1.Y, p2.Y))
                {
                    if (p.Y <= Math.Max(p1.Y, p2.Y))
                    {
                        if (p.X <= Math.Max(p1.X, p2.X))
                        {
                            xinters = (p.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
							if (p1.X == p2.X || p.X <= xinters) counter++;
						}
					}
				}
				p1 = p2;
			}
			
			if (counter % 2 == 0) return false;
			return true;
        }


        /// <summary>
        /// Using List of Pixel & Pixel
        /// </summary>
        /// <param name="pixelArray">Array of Pixel</param>
        /// <param name="testPixel">Pixel to test for</param>
        /// <returns></returns>
        public static bool isIntersect(List<Pixel> pixelArray, Pixel testPixel)
        {
            int count = pixelArray.Count - 1;
            int counter = 0;
            double xinters = 0;
            Pixel p, p1, p2;

            p = testPixel;
            p1 = pixelArray[0];
            for (int i = 0; i <= count; i++)
            {
                p2 = pixelArray[i % count];
                if (p.Y > Math.Min(p1.Y, p2.Y))
                {
                    if (p.Y <= Math.Max(p1.Y, p2.Y))
                    {
                        if (p.X <= Math.Max(p1.X, p2.X))
                        {
                            xinters = (p.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
                            if (p1.X == p2.X || p.X <= xinters) counter++;
                        }
                    }
                }
                p1 = p2;
            }

            if (counter % 2 == 0) return false;
            return true;
        }


        /// <summary>
        /// Using String of LatLng & LatLng
        /// </summary>
        /// <param name="latLngArrayStr">Format: lat,lng;lat,lng;...</param>
        /// <param name="testLatLng">LatLng to test for</param>
        /// <returns></returns>
        public static bool isIntersect(String latLngArrayStr, LatLng testLatLng)
        {
            List<Pixel> pixelArray = new List<Pixel>();
            Pixel p = TileConverter.FromLatLngToPixel(testLatLng, Polygon.ZOOM);

            //split
            string[] splitStrArray = latLngArrayStr.Split(
                new string[] { ";" }, 
                StringSplitOptions.RemoveEmptyEntries
                );

            for (int i = 0; i < splitStrArray.Length; i++)
            {
                string[] latLngStrs = splitStrArray[i].Split(',');
                Pixel convertedPixel = TileConverter.FromLatLngToPixel(
                    double.Parse(latLngStrs[0]), 
                    double.Parse(latLngStrs[1]), 
                    Polygon.ZOOM
                    );

                pixelArray.Add(convertedPixel);
            }

            return Polygon.isIntersect(pixelArray, p);
        }

        /// <summary>
        /// Using String of LatLng & LatLng
        /// </summary>
        /// <param name="latLngArrayStr">Format: lat,lng;lat,lng;...</param>
        /// <param name="testLatLng">LatLng to test for</param>
        /// <returns></returns>
        public static bool isIntersectGeometry(String latLngArrayStr, LatLng testLatLng)
        {
            List<Pixel> pixelArray = new List<Pixel>();
            Pixel p = TileConverter.FromLatLngToPixel(testLatLng, Polygon.ZOOM);

            //split
            string[] splitStrArray = latLngArrayStr.Split(
                new string[] { ";" },
                StringSplitOptions.RemoveEmptyEntries
                );

            for (int i = 0; i < splitStrArray.Length; i++)
            {
                string[] latLngStrs = splitStrArray[i].Split(',');
                Pixel convertedPixel = TileConverter.FromLatLngToPixel(
                    double.Parse(latLngStrs[1]),
                    double.Parse(latLngStrs[0]),
                    Polygon.ZOOM
                    );

                pixelArray.Add(convertedPixel);
            }

            return Polygon.isIntersect(pixelArray, p);
        }


    }
}
