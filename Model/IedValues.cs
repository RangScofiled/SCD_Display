using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3.Model
{
    class IedValues
    {
        #region Fields

        private AccValues m_AccData = null;

        #endregion Fields

        #region Construction/Destruction/Initialisation
        /// <summary>
        /// define all configuration data
        /// </summary>
        public IedValues()
        {
            m_AccData = new AccValues();
        }

        #endregion Construction/Destruction/Initialisation

        #region Public Methods
        /// <summary>
        /// Get AccessPoint data
        /// </summary>
        /// <returns>Return AccessPoint data</returns>
        public AccValues GetAccValues()
        {
            return m_AccData;
        }

        #endregion Public Methods
    }
}
