using System;
using System.Collections.Generic;
using System.Text;

namespace Cartomatic.Utils
{
    public class Mercator
    {
        //borrowed from https://gist.github.com/onderaltintas/6649521

        /// <summary>
        /// 180 equatorial degrees in metres
        /// </summary>
        private const double SphericalMercatorHalphSphere = 20037508.34;

        /// <summary>
        /// Converts lon lat to spherical mercator
        /// </summary>
        /// <param name="lon"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public static (double x, double y) FromLonLat(double lon, double lat)
        {
            //20037508.34 is spherical mercator  half sphere (0 - 180)
            var x = lon * SphericalMercatorHalphSphere / 180;

            var y = Math.Log(Math.Tan((90 + lat) * Math.PI / 360)) / (Math.PI / 180);
            y *= 20037508.34 / 180;

            return (x, y);
        }

        /// <summary>
        /// Converts spherical mercator to lon lat
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static (double lon, double lat) ToLonLat(double x, double y)
        {
            var lon = x * 180 / SphericalMercatorHalphSphere;
            var lat = Math.Atan(Math.Exp(y * Math.PI / SphericalMercatorHalphSphere)) * 360 / Math.PI - 90;

            return (lon, lat);
        }
    }
}
