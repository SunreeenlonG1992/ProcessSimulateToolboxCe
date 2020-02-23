using Tecnomatix.Engineering;

namespace Robworld.PsPublicLibrary.CommandEnablers
{
    /// <summary>
    /// Enables a command on eMServer platform only
    /// </summary>
    public class RwEmsCommandEnabler : TxCommandEnabler
    {
        /// <summary>
        /// Create a new instance of an eMServer only command enabler
        /// </summary>
        public RwEmsCommandEnabler()
        {
            _enable = TxApplication.PlatformType == TxPlatformType.EmServer;
        }
    }
}
