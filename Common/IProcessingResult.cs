using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3.Common
{
    interface IProcessingResult
    {
        /// <summary>
        /// This Method return the result code of a processing result.
        /// </summary>
        /// <returns>The result code</returns>
        ProcessingResultCode GetResultCode();

        /// <summary>
        /// This Method returns the result message of a processing result.
        /// </summary>
        /// <returns>The long result message</returns>
        String GetResultMessage();
    }

    /// <summary>
    /// The possible processing result values.
    /// </summary>
    public enum ProcessingResultCode
    {
        /// <summary>
        /// processing was finished without any error or warning.
        /// </summary>
        ProcessingOK,

        /// <summary>
        /// processing was finished with warnings.
        /// </summary>
        ProcessingFinishedWithWarning,

        /// <summary>
        /// processing was aborted.
        /// </summary>
        ProcessingAbortedWithError
    };
}
