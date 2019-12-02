using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.Number
{
    /// <summary>
    /// number extensions
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Fits a number in range if it is outside (to the closest bound)
        /// </summary>
        /// <param name="n"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static double FitInRange(this double n, double min, double max)
        {
            if (n > max) n = max;
            if (n < min) n = min;

            return n;
        }

        /// <summary>
        /// fits a number in range
        /// </summary>
        /// <param name="n"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float FitInRange(this float n, double min, double max)
        {
            return (float) ((double) n).FitInRange(min, max);
        }
    }
}
