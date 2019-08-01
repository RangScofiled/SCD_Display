using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication3.Model
{
    class CommValues
    {
        #region Fields

        private SubNetValues m_SubNetData = null;

        #endregion Fields

        #region Construction/Destruction/Initialisation
        /// <summary>
        /// define all configuration data
        /// </summary>
        public CommValues()
        {
            m_SubNetData = new SubNetValues();
        }

        #endregion Construction/Destruction/Initialisation

        #region Public Methods
        /// <summary>
        /// Get SubNetwork data
        /// </summary>
        /// <returns>Return SubNetwork data</returns>
        public SubNetValues GetSubNetValues()
        {
            return m_SubNetData;
        }

        #endregion Public Methods
    }
}
