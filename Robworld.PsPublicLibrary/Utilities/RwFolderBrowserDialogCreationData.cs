using System;
using System.ComponentModel;

namespace Robworld.PsPublicLibrary.Utilities
{
    /// <summary>
    /// Represents the data for using a FolderBrowserDialog
    /// </summary>
    public class RwFolderBrowserDialogCreationData
    {
        #region Properties
        /// <summary>
        /// The description that appears on the top of the FolderBrowserDialog
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The selected path in the FolderBrowserDialog
        /// </summary>
        public string SelectedPath { get; set; }

        /// <summary>
        /// Show or hide the button for creating new folders
        /// </summary>
        public bool ShowNewFolderButton { get; set; }

        /// <summary>
        /// Get or set an object with informations about the control
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// Defines the start location in the FolderBrowserDialog
        /// </summary>
        public Environment.SpecialFolder RootFolder { get; set; }

        /// <summary>
        /// Unknown
        /// </summary>
        public ISite Site { get; set; }
        #endregion
    }
}
