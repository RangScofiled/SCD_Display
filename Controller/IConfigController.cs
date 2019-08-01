using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WindowsFormsApplication3.Model;
using WindowsFormsApplication3.Common;

namespace WindowsFormsApplication3.Controller
{
    interface IConfigController
    {
        /// <summary>
        /// Return all configuration data to be stored into XML file
        /// </summary>
        /// <returns></returns>
        CommValues GetCommData();

        IedValues GetIedData();
        IProcessingResult SaveCfgFile(string fileName, string filePath);
    }
}
