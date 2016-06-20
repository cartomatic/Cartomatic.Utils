using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.Number
{
    public static class StringExtensions
    {
        public static bool IsNumeric(this string wouldBeANumber)
        {
            double num;

            return double.TryParse(wouldBeANumber, System.Globalization.NumberStyles.Any, NumberFormatInfo.InvariantInfo, out num);
        }

        public static bool IsNumeric(this object wouldBeANumber)
        {
            return Convert.ToString(wouldBeANumber, CultureInfo.InvariantCulture).IsNumeric();
        }
    }
}
