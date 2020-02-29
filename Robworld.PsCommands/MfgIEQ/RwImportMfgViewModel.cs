using EngineeringInternalExtension;
using Robworld.PsPublicLibrary.Mvvm;
using Robworld.PsPublicLibrary.Utilities;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui.WPF;
using Tecnomatix.Engineering.Utilities;
using Tecnomatix.Planning;

namespace Robworld.PsCommands.MfgIEQ
{
    /// <summary>
    /// Notifies the UI about changed Properties in ViewModels
    /// </summary>
    public class RwImportMfgViewModel : RwViewModelBase
    {
        #region Fields
        private string defaultMfgGroupBoxHeader;
        private string mfgGroupBoxHeader;
        private string sourceFilename;
        private ITxPlanningObject planningObject;
        private bool isSelectedPlanningObjectValid;
        #endregion

        #region Properties
        /// <summary>
        /// Get the title of the window
        /// </summary>
        public string Title { get { return "Import Manufacturing features from list"; } }

        /// <summary>
        /// Get the height of the window
        /// </summary>
        public int WindowHeight { get { return 450; } }

        /// <summary>
        /// Get the width of the window
        /// </summary>
        public int WindowWidth { get { return 250; } }

        /// <summary>
        /// Get the caption of the Mfg collection groupbox 
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
        public ObservableCollection<string> MfgCollection { get; set; }

        /// <summary>
        /// Get or set the import filename 
        /// </summary>
        public string SourceFilename
        {
            get { return sourceFilename; }
            set
            {
                if (sourceFilename != value)
                {
                    sourceFilename = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Represents the command for choosing the import file
        /// </summary>
        public ICommand ChooseSourceFileCommand { get; private set; }

        /// <summary>
        /// Represents the command for executing the manufacturing features import process
        /// </summary>
        public ICommand ExecuteMfgImportCommand { get; private set; }

        /// <summary>
        /// Represents the event on picking the ITxPlanningObject
        /// </summary>
        public ICommand MfgLibraryPickedEvent { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create an instance of the mfg import command
        /// </summary>
        public RwImportMfgViewModel()
        {
            planningObject = null;
            isSelectedPlanningObjectValid = false;
            MfgCollection = new ObservableCollection<string>();
            defaultMfgGroupBoxHeader = "Manufacturing features import list";
            MfgGroupBoxHeader = defaultMfgGroupBoxHeader;
            SourceFilename = string.Empty;
            ChooseSourceFileCommand = new RwActionCommand(ChooseSourceFileExecuted, ChooseSourceFileCanExecute);
            ExecuteMfgImportCommand = new RwActionCommand(MfgImportExecuted, MfgImportCanExecute);
            MfgLibraryPickedEvent = new RwActionCommand(MfgLibraryPickedExecuted, MfgLibraryPickedCanExecute);
            MfgCollection.CollectionChanged += OnMfgCollectionChanged;
        }
        #endregion

        #region Command enablers
        /// <summary>
        /// Decide if the pick event for choosing the planning object can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns always true</returns>
        private bool MfgLibraryPickedCanExecute(object arg)
        {
            return true;
        }

        /// <summary>
        /// Decide if the command for choosing the target file can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns true if the selected object is of right type</returns>
        private bool MfgImportCanExecute(object arg)
        {
            return MfgCollection != null && MfgCollection.Count > 0 && isSelectedPlanningObjectValid;
        }

        /// <summary>
        /// Decide if the command for choosing the source/import file can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns always true</returns>
        private bool ChooseSourceFileCanExecute(object arg)
        {
            return true;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Execute the pick event on a selected planning object
        /// </summary>
        /// <param name="obj">The control of the UI</param>
        private void MfgLibraryPickedExecuted(object obj)
        {
            try
            {
                isSelectedPlanningObjectValid = false;
                if (obj is TxObjEditBoxControl control)
                {
                    planningObject = control.Object as ITxPlanningObject;
                    if (planningObject != null)
                    {
                        if(planningObject.PlanningType.Equals("PmMfgLibrary"))
                        {
                            isSelectedPlanningObjectValid = true;
                        }
                    }
                }
            }
            catch (TxPlanningObjectNotLoadedException exception)
            {
                TxMessageBox.Show($"{exception.Message}", "Mfg import - Exception", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Executes the import process
        /// </summary>
        /// <param name="obj">Not in use</param>
        private void MfgImportExecuted(object obj)
        {
            LoadMfgByName();
        }

        /// <summary>
        /// Choose the source/import file
        /// </summary>
        /// <param name="obj">Not in use</param>
        private void ChooseSourceFileExecuted(object obj)
        {
            RwOpenFileDialogCreationData dialogCreationData = new RwOpenFileDialogCreationData
            {
                Multiselect = false,
                Title = "Select Mfg file",
                Filter = "Textfile|*.txt"
            };
            SourceFilename = RwFileAndDirectoryUtilities.OpenFileDialog(dialogCreationData);
            if (!string.IsNullOrEmpty(SourceFilename))
            {
                ReadMfgData();
            }
        }

        /// <summary>
        /// Read the name of the manufacturing features of the import list
        /// </summary>
        private void ReadMfgData()
        {
            try
            {
                string[] mfgData = System.IO.File.ReadAllLines(SourceFilename);
                if(mfgData != null && mfgData.Length > 0)
                {
                    foreach (string name in mfgData)
                    {
                        MfgCollection.Add(name);
                    }
                }
            }
            catch
            {
                TxMessageBox.Show("Error in reading import file", "Mfg import", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Load the manufacturing features by their name to the study
        /// </summary>
        private void LoadMfgByName()
        {
            TxObjectList mfgsToLoad = new TxObjectList();

            TxEmsCacheManager emsCacheManager = new TxEmsCacheManager();
            QueryEmsCache(emsCacheManager, "wpMfgLibrary", "children", planningObject, null);

            TxObjectList mfgsInLibrary = planningObject.GetField("children") as TxObjectList;
            QueryEmsCache(emsCacheManager, "wpMfgLibrary", "name", null, mfgsInLibrary);

            foreach (ITxPlanningObject mfg in mfgsInLibrary)
            {
                string name = mfg.GetField("name") as string;
                if (MfgCollection.Contains(name))
                {
                    mfgsToLoad.Add(mfg);
                }
            }

            if (mfgsToLoad.Count > 0)
            {
                TxDocumentEx document = new TxDocumentEx();
                document.LoadComplete(mfgsToLoad, true);
                TxMessageBox.Show($"{mfgsToLoad.Count} Mfgs loaded!", "Mfg import", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            else
            {
                TxMessageBox.Show("No wanted Mfgs found in library", "Mfg import", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Query the eMServer fields
        /// </summary>
        /// <param name="emsCacheManager">Cache manager for emServer fields</param>
        /// <param name="index">The string indexer, normally "wpMfgLibrary"</param>
        /// <param name="attribute">The attribue for setting on eMServer cached field</param>
        /// <param name="planningObject">The planning object, here it is the MfgLibrary</param>
        /// <param name="collection">The collection with required objects, in this case the mfg names</param>
        private static void QueryEmsCache(TxEmsCacheManager emsCacheManager, string index, string attribute, ITxPlanningObject planningObject, TxObjectList collection)
        {
            emsCacheManager.Clear();
            emsCacheManager[index].SetAttributes(attribute);
            if (planningObject != null && collection == null)
            {
                emsCacheManager[index].SetRootObject(planningObject);
            }
            else if (planningObject == null && collection != null)
            {
                emsCacheManager[index].SetRootObjects(collection);
            }
            emsCacheManager.CacheData();
        }

        /// <summary>
        /// Update the groupbox header of the Mfg collection
        /// </summary>
        /// <param name="sender">The Mfg collection</param>
        /// <param name="e">The collection changed event arguments</param>
        private void OnMfgCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            MfgGroupBoxHeader = $"{defaultMfgGroupBoxHeader} ({MfgCollection.Count})";
        }
        #endregion
    }
}
