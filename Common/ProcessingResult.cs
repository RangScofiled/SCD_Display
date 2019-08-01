using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3.Common
{
    class ProcessingResult: IProcessingResult
    {
        #region Fields
        /// <summary>
        /// The result code of the processing result.
        /// </summary>
        private ProcessingResultCode ResultCode;

        /// <summary>
        /// The message of the processing result.
        /// </summary>
        private string ResultMessage;

        #endregion Fields


        #region Construction/Destruction/Initialisation
        /// <summary>
        /// Create a processing result object.
        /// </summary>
        /// <param name="ResultCode">The result code of the processing result</param>
        /// <param name="ResultMessage">The long message of the processing result</param>
        public ProcessingResult(ProcessingResultCode ResultCode, string ResultMessage)
        {
            this.ResultCode    = ResultCode;
            this.ResultMessage = ResultMessage;
        }
        #endregion Construction/Destruction/Initialisation


        #region IProcessingResult Members

        ProcessingResultCode IProcessingResult.GetResultCode()
        {
            return this.ResultCode;
        }

        string IProcessingResult.GetResultMessage()
        {
            return this.ResultMessage;
        }

        #endregion IProcessingResult Members
    }
}
