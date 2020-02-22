using Tecnomatix.Engineering;

namespace Robworld.PsPublicLibrary.CommandEnablers
{
    /// <summary>
    /// Represents a command enabler that is always true
    /// </summary>
    public class RwAlwaysTrueCommandEnabler : TxCommandEnabler
    {
        #region Constructors
        /// <summary>
        /// Create an enabler that is always true
        /// </summary>
        public RwAlwaysTrueCommandEnabler()
        {
            _enable = true;
        }
        #endregion
    }
}
