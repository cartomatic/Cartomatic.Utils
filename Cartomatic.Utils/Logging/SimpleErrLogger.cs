using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cartomatic.Utils
{
    /// <summary>
    /// Logging utils
    /// </summary>
    public partial class Logging
    {
        /// <summary>
        /// Simplistic exception logger that also dumps all the inner exceptions along with their stack trace
        /// </summary>
        /// <param name="e"></param>
        public static void LogExceptions(Exception e)
        {
            try
            {
                var dir = "_err_log".SolvePath();
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var errs = new List<string>{
                    DateTime.Now.ToLongTimeString()
                };

                while (e != null)
                {
                    errs.Add(e.Message);
                    errs.Add(e.StackTrace);
                    errs.Add(new string('-', 50));
                    errs.Add(Environment.NewLine);

                    e = e.InnerException;
                }

                errs.Add(new string('=', 150));
                errs.Add(Environment.NewLine);
                errs.Add(Environment.NewLine);

                System.IO.File.AppendAllLines(Path.Combine(dir, $"{DateTime.Now:yyyyMMdd}.custom.log"), errs);
            }
            catch
            {
                //ignore
            }
        }
    }
}
