using System.Collections.Generic;

namespace Robworld.PsCommands.MfgIEQ
{
    /// <summary>
    /// Represents the relationship between a Mfg feature and a Mfg library
    /// </summary>
    public class RwMfgFeature
    {
        #region Properties
        /// <summary>
        /// Get the name of the Mfg feature
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Get the Mfg libraries that holds the Mfg feature
        /// </summary>
        public List<string> MfgLibraries { get; }

        /// <summary>
        /// Get the string representation of the libraries that holds the Mfg feature
        /// </summary>
        public string MfgLibrariesAsString { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of relationship between a Mfg feature and a Mfg library 
        /// </summary>
        /// <param name="name">The name of the Mfg feature</param>
        /// <param name="mfgLibraries">The Mfg libraries that holds the Mfg feature</param>
        public RwMfgFeature(string name, List<string> mfgLibraries)
        {
            Name = name;
            MfgLibraries = mfgLibraries;
            CreateMfgLibrariesString();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Create the string representation of Mfg libraries
        /// </summary>
        private void CreateMfgLibrariesString()
        {
            if(MfgLibraries == null || MfgLibraries.Count == 0) return;
            MfgLibrariesAsString = string.Join(";", MfgLibraries);
        }
        #endregion
    }
}
