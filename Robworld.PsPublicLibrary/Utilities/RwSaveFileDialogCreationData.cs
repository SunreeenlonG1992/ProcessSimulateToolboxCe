namespace Robworld.PsPublicLibrary.Utilities
{
    /// <summary>
    /// Represents some data for using a SaveFileDialog
    /// </summary>
    public class RwSaveFileDialogCreationData
    {
        #region Properties
        /// <summary>
        /// Get or set the title of the OpenFileDialog
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Get or set the filter for files
        /// </summary>
        public string Filter { get; set; }
        #endregion
    }
}
