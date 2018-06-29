using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cartomatic.Utils.Ef
{
    public static class StringExtensions
    {
        /// <summary>
        /// changes a property name into a lower case underscored pgsql like column name
        /// basically each upper case letter is replaced with _ and + its lowerCase equivaluent
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static string ToColumnName(this string propertyName)
        {
            List<char> chars = new List<char>();
            for (var idx = 0; idx < propertyName.Length; idx ++)
            {
                if ( 
                    char.IsUpper(propertyName[idx]) && //if char is upper case
                    idx > 0 && !char.IsUpper(propertyName[idx - 1]) && //and previous was not upper case 
                    (
                        (idx < propertyName.Length - 2 && !char.IsUpper(propertyName[idx + 1])) || //and next is lowercase
                        idx == propertyName.Length - 1 // or this is the last one
                    ) 
                )
                {
                    chars.Add('_');
                }

                chars.Add(char.ToLower(propertyName[idx]));
            }

            return string.Join("", chars);
        }
    }
}
