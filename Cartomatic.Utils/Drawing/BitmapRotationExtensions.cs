using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cartomatic.Utils.Number;

namespace Cartomatic.Utils.Drawing
{
    public static partial class BitmapExtensions
    {

        private const int ExifOrientationId = 0x112; //274


        /// <summary>
        /// Applies img rotation as specified in an exif tag
        /// </summary>
        /// <param name="bitmap"></param>
        /// <remarks>Borrowed from https://stackoverflow.com/a/48347653 </remarks>
        /// <returns></returns>
        public static Bitmap ApplyExifRotation(this Bitmap bitmap)
        {
            if (!bitmap.PropertyIdList.Contains(ExifOrientationId))
                return bitmap;

            var prop = bitmap.GetPropertyItem(ExifOrientationId);
            int val = BitConverter.ToUInt16(prop.Value, 0);
            var rot = RotateFlipType.RotateNoneFlipNone;

            if (val == 3 || val == 4)
                rot = RotateFlipType.Rotate180FlipNone;
            else if (val == 5 || val == 6)
                rot = RotateFlipType.Rotate90FlipNone;
            else if (val == 7 || val == 8)
                rot = RotateFlipType.Rotate270FlipNone;

            if (val == 2 || val == 4 || val == 5 || val == 7)
                rot |= RotateFlipType.RotateNoneFlipX;

            if (rot != RotateFlipType.RotateNoneFlipNone)
                bitmap.RotateFlip(rot);

            return bitmap;
        }

    }
}
