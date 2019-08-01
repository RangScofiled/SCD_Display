using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication3.Controller;

namespace WindowsFormsApplication3.View
{
    interface IMainView
    {
        /// <summary>
        /// This method sets up the main view.
        /// </summary>
        /// <param name="control">The IMainViewController which will be used for communication.</param>
        void StartView(IMainViewController control);
    }
}
