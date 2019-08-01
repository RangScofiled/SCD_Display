using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3.Model
{
    interface IProcessingConfig
    {
        /// <summary>
        /// Loads the XML file.
        /// </summary>
        /// <param name="filename">File name of xml file to load (and save).</param>
        void LoadCfgFile(string filename);
        void SaveCfgFile(string filename, string filePath);

        CommValues CommCfgSet();
        IedValues IedCfgSet();
    }
}
