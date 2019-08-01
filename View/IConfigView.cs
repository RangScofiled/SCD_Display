using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WindowsFormsApplication3.Controller;

namespace WindowsFormsApplication3.View
{
    interface IConfigView
    {
        /// <summary>
        /// This method sets the controller interface, which should be used for communication.
        /// </summary>
        /// <param name="controller">the controller object</param>
        void SetControllerInterface(IConfigController controller);
        void SaveCfgFile(string filename, string oldFilePath);
    }
}
