namespace Robworld.PsPublicLibrary.Utilities
{
    /// <summary>
    /// Represents some data for using a OpenFileDialog
    /// </summary>
    public class RwOpenFileDialogCreationData
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

        /// <summary>
        /// Choose between single file or multiple files selection
        /// </summary>
        public bool Multiselect { get; set; }
        #endregion
    }
}
