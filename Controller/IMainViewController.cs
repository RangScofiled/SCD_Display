using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WindowsFormsApplication3.View;
using WindowsFormsApplication3.Common;

namespace WindowsFormsApplication3.Controller
{
    interface IMainViewController
    {
        void setMdiText(string mdiText);

        /// <summary>
        /// Method is called if user selects the 'parameter generate' mode.
        /// </summary>
        /// <param name="view">communication from controller to the view will be handled through 
        /// the 'view' object</param>
        void SetConfigView(IConfigView view);

        /// <summary>
        /// Request to load given SWT Cfg settings file.
        /// </summary>
        /// <param name="fileName">XML file containing SWT settings</param>
        /// <returns>A ProcessingResult object that specifies if this call was successful or not.</returns>
        IProcessingResult OpenConfig(string fileName);

        void SaveCfgFile(string fileName, string oldFilePath);
    }
}
