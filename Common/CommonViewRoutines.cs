using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.IO;

namespace WindowsFormsApplication3.Common
{
    class CommonViewRoutines
    {
        /// <summary>
        /// This method retrieves the path where all template files are stored.
        /// </summary>
        /// <returns>Path of templates.</returns>
        public static string GetFilesDirectory()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
