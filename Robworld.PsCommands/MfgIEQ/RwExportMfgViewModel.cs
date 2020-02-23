using Robworld.PsPublicLibrary.Mvvm;
using Robworld.PsPublicLibrary.Utilities;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows.Input;
using Tecnomatix.Engineering;

namespace Robworld.PsCommands.MfgIEQ
{
    /// <summary>
    /// Export manufacturing features to the file system
    /// </summary>
    public class RwExportMfgViewModel : RwViewModelBase
    {
        #region Fields
        private string mfgGroupBoxHeader;
        private string targetFilename;
        #endregion

        #region Properties
        /// <summary>
        /// Get the title of the window
        /// </summary>
        public string Title { get { return "Manufacturing feature exporter"; } }

        /// <summary>
        /// Get the height of the window
        /// </summary>
        public int WindowHeight { get { return 450; } }

        /// <summary>
        /// Get the width of the window
        /// </summary>
        public int WindowWidth { get { return 250; } }

        /// <summary>
        /// Get the caption of the frame data groupbox
        /// </summary>
        public string MfgGroupBoxHeader
        {
            get { return mfgGroupBoxHeader; }
            private set
            {
                if (mfgGroupBoxHeader != value)
                {
                    mfgGroupBoxHeader = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the manufacturing features collection
        /// </summary>
        public ObservableCollection<TxMfgFeature> Mfgs { get; set; }

        /// <summary>
        /// Get or set the export filename 
        /// </summary>
        public string TargetFilename
        {
            get { return targetFilename; }
            set
            {
                if (targetFilename != value)
                {
                    targetFilename = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Represents the command for adding the selected manufacturing features to the export collection
        /// </summary>
        public ICommand AddSelectionToCollectionCommand { get; private set; }

        /// <summary>
        /// Represents the command for clearing the export collection
        /// </summary>
        public ICommand ClearCollectionCommand { get; private set; }

        /// <summary>
        /// Represents the command for choosing the target file
        /// </summary>
        public ICommand ChooseTargetFileCommand { get; private set; }

        /// <summary>
        /// Represents the command for executing the manufacturing features export process
        /// </summary>
        public ICommand ExecuteExportMfgsCommand { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create an instance of the mfg export command
        /// </summary>
        public RwExportMfgViewModel()
        {
            MfgGroupBoxHeader = "Manufacturing features export list";
            Mfgs = new ObservableCollection<TxMfgFeature>();
            Mfgs.CollectionChanged += MfgCollectionChanged;
            TargetFilename = string.Empty;
            AddMfgsToCollection();
            AddSelectionToCollectionCommand = new RwActionCommand(AddSelectionToCollectionExecuted, AddSelectionToCollectionCanExecute);
            ClearCollectionCommand = new RwActionCommand(ClearCollectionExecuted, ClearCollectionCanExecute);
            ChooseTargetFileCommand = new RwActionCommand(ChooseTargetFileExecuted, ChooseTargetFileCanExecute);
            ExecuteExportMfgsCommand = new RwActionCommand(ExportMfgsExecuted, ExportMfgsCanExecute);
        }
        #endregion

        #region Command enablers
        /// <summary>
        /// Decide if the command for choosing the target file can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns always true</returns>
        private bool ChooseTargetFileCanExecute(object arg)
        {
            return true;
        }

        /// <summary>
        /// Decide if the command for exporting the mfg collection can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns true or false</returns>
        private bool ExportMfgsCanExecute(object arg)
        {
            return Mfgs != null && Mfgs.Count > 0 && !string.IsNullOrEmpty(TargetFilename);
        }

        /// <summary>
        /// Decide if the command for clearing the export collection can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns true or false</returns>
        private bool ClearCollectionCanExecute(object arg)
        {
            return Mfgs != null && Mfgs.Count > 0;
        }

        /// <summary>
        /// Decide if the command for adding the selecting items to the export collection can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns true or false</returns>
        private bool AddSelectionToCollectionCanExecute(object arg)
        {
            return TxApplication.ActiveDocument.Selection.GetFilteredItems(new TxTypeFilter(typeof(TxMfgFeature))).Count > 0;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Clears the export collection
        /// </summary>
        /// <param name="obj">Not in use</param>
        private void ClearCollectionExecuted(object obj)
        {
            Mfgs.Clear();
        }

        /// <summary>
        /// Add the selected items to the export collection
        /// </summary>
        /// <param name="obj">Not in use</param>
        private void AddSelectionToCollectionExecuted(object obj)
        {
            AddMfgsToCollection();
        }

        /// <summary>
        /// Choose the target file
        /// </summary>
        /// <param name="obj">Not in use</param>
        private void ChooseTargetFileExecuted(object obj)
        {
            RwSaveFileDialogCreationData saveFileCreationData = new RwSaveFileDialogCreationData
            {
                Title = "Select the file where to store the informations",
                Filter = "Mfg file|*.txt|Textfile|*.txt"
            };
            TargetFilename = RwFileAndDirectoryUtilities.SaveFileDialog(saveFileCreationData);
        }

        /// <summary>
        /// Write the mfg collection to the target file
        /// </summary>
        /// <param name="obj">Not in use</param>
        private void ExportMfgsExecuted(object obj)
        {
            try
            {
                using (StreamWriter file = new StreamWriter(TargetFilename))
                {
                    foreach (TxMfgFeature mfg in Mfgs)
                    {
                        file.WriteLine(mfg.Name);
                    }
                }
                string message = $"Successsfully saved Manufacturing features to file {TargetFilename}";
                TxMessageBox.Show(message, "Mfg export", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch
            {
                string message = "An error occured during Mfg export";
                TxMessageBox.Show(message, "Mfg export", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Add the selected items to the export collection
        /// </summary>
        private void AddMfgsToCollection()
        {
            if (TxApplication.ActiveDocument.Selection == null || TxApplication.ActiveDocument.Selection.Count == 0) return;

            TxObjectList selectedMfgs = TxApplication.ActiveDocument.Selection.GetFilteredItems(new TxTypeFilter(typeof(TxMfgFeature)));
            if (selectedMfgs == null || selectedMfgs.Count == 0) return;

            foreach (TxMfgFeature selectedMfg in selectedMfgs)
            {
                if (!Mfgs.Contains(selectedMfg))
                {
                    Mfgs.Add(selectedMfg);
                }
            }
        }

        /// <summary>
        /// Update the groupbox header of the Manufacturing features collection
        /// </summary>
        /// <param name="sender">The frame collection</param>
        /// <param name="e">The collection changed event arguments</param>
        private void MfgCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (Mfgs != null)
            {
                MfgGroupBoxHeader = $"Manufacturing features: ({Mfgs.Count})";
            }
        }
        #endregion
    }
}
