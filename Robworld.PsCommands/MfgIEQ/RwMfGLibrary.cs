namespace Robworld.PsCommands.MfgIEQ
{
    /// <summary>
    /// Represents Details of a MfG library
    /// </summary>
    public class RwMfGLibrary
    {
        #region Properties
        /// <summary>
        /// Get the name of a Mfg library
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Get the external Id of the Mfg library
        /// </summary>
        public string ExternalID { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a MfG library
        /// </summary>
        /// <param name="name">The name of the Mfg library</param>
        /// <param name="externalId">The external id of the Mfg library</param>
        public RwMfGLibrary(string name, string externalId)
        {
            Name = name;
            ExternalID = externalId;
        }
        #endregion
    }
}
