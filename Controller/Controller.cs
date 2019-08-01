using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WindowsFormsApplication3.View;
using WindowsFormsApplication3.Common;
using WindowsFormsApplication3.Resources;
using WindowsFormsApplication3.Model;

namespace WindowsFormsApplication3.Controller
{
    class Controller : IMainViewController, IConfigController
    {
        /* view */
        /// <summary> the main view instance </summary>
        protected IMainView m_MdiWindow = null;

        /// <summary> the Config view instance </summary>
        protected IConfigView m_ConfigView = null;

        /* model */
        /// <summary> the parameter generator model</summary>
        protected IProcessingConfig m_ProcessingConfig;

        private string m_MdiTile;

        #region Delegates and Events

        /// <summary>
        /// This delegate will only be used by TryCall. See TryCall for more
        /// information.
        /// </summary>
        /// <returns>
        /// An IProcessingResult class that provides information
        /// about success or failure of this task.
        /// </returns>
        protected delegate IProcessingResult RealTask();

        #endregion Delegates and Events

        #region Construction/Destruction/Initialisation

        /// <summary>
        /// Standard constructor.
        /// </summary>
        public Controller()
        {
            m_ProcessingConfig = new ProcessingConfig();
        }

        #endregion Construction/Destruction/Initialisation

        /// <summary>
        /// Creates main view and presents it to the user.
        /// </summary>
        public void Start(IMainView mainView)
        {
            m_MdiWindow = mainView;
            m_MdiWindow.StartView(this);
        }

        void IMainViewController.setMdiText(string headText)
        {
            m_MdiTile = headText;
        }
        /// <summary>
        /// Method is called if user selects the 'Config' mode.
        /// </summary>
        /// <param name="view">communication from controller to the view will be handled through 
        /// the 'view' object</param>
        void IMainViewController.SetConfigView(IConfigView view)
        {
            m_ConfigView = view;
            view.SetControllerInterface(this);
        }

        IProcessingResult IMainViewController.OpenConfig(string fileName)
        {
            return TryCall(Messages.ErrorFileLoad
                , delegate()
                {
                    m_ProcessingConfig.LoadCfgFile(fileName);
                    return new ProcessingResult(ProcessingResultCode.ProcessingOK, Messages.InfoFileLoaded);
                });
        }

        void IMainViewController.SaveCfgFile(string fileName, string oldFilePath)
        {
            m_ConfigView.SaveCfgFile(fileName, oldFilePath);
        }

        CommValues IConfigController.GetCommData()
        {
            return m_ProcessingConfig.CommCfgSet();
        }

        IedValues IConfigController.GetIedData()
        {
            return m_ProcessingConfig.IedCfgSet();
        }

        IProcessingResult IConfigController.SaveCfgFile(string fileName, string filePath)
        {
            m_ProcessingConfig.SaveCfgFile(fileName, filePath);
            return new ProcessingResult(ProcessingResultCode.ProcessingOK, Messages.InfoFileLoaded);
        }

        #region Protected Implementation
        /// <summary>
        /// This method composes and shows an error Messages to the user, if some necessary
        /// file couldn't be found.
        /// </summary>
        /// <param name="headMessage">This string should explain where this error occured.</param>
        /// <param name="exception">The actual exception that has been caught by the controller.</param>
        protected void ReportFileNotFoundErrorMessage(string headMessage, System.IO.FileNotFoundException exception)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(headMessage);
            builder.AppendFormat(Messages.ErrorFileNotFoundWithName, exception.FileName);
        }
        /// <summary>
        /// This method will be used to catch all certain typs of exceptions.
        /// It also generates a dialog to inform the user about certain errors.
        /// </summary>
        /// <param name="headErrorMessage">This Messages informs the user about
        /// where this errors happened. This should help to realize possible wrong user input.</param>
        /// <param name="task">The actual task to do. (This will be a anonymouse method, see remarks section)</param>
        /// <returns>A ProcessingResult object that specifies if this call was successful or not.</returns>
        /// <remarks>
        /// This method will be used by IParGenViewController, IMergeViewController and IEn100CfgViewController
        /// by most of their methods, to ensure that errors that can't be handled by
        /// this application will always be propergated in a user-readable form.
        /// </remarks>
        protected IProcessingResult TryCall(string headErrorMessage, RealTask task)
        {
            try
            {
                return task();
            }
            // here will add other exception
            catch (System.IO.FileNotFoundException ex)
            {
                ReportFileNotFoundErrorMessage(headErrorMessage, ex);
                return new ProcessingResult(ProcessingResultCode.ProcessingAbortedWithError, Messages.ErrorFileNotFound);
            }
        }
        #endregion Protected Implementation
    }
}
