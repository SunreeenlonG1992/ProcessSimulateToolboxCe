using EMPAPPLICATIONLib;
using EMPMODELLib;
using EMPTYPELIBRARYLib;
using Robworld.PsPublicLibrary.Mvvm;
using Robworld.PsPublicLibrary.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Tecnomatix.Engineering;
using Tecnomatix.Planning;

namespace Robworld.PsCommands.MfgIEQ
{
    /// <summary>
    /// Show the relationship between Mfg features and Mfg libraries
    /// </summary>
    public class RwQueryMfgViewModel : RwViewModelBase
    {
        #region Fields
        private EmpContext empContext;
        private Dictionary<int, EmpObjectKey> empMfgFeatureInMfgLibraries;
        private string exportRule;
        #endregion

        #region Properties
        /// <summary>
        /// Get the title of the view window
        /// </summary>
        public string Title { get { return "Manufacturing feature relations"; } }

        /// <summary>
        /// Get or set the group box header of reference Mfg libraries in the view
        /// </summary>
        public string ReferenceMfgLibrariesGroupBoxHeader { get; private set; }

        /// <summary>
        /// Get or set the group box header of study Mfgs in the view
        /// </summary>
        public string StudyMfgFeaturesGroupBoxHeader { get; private set; }

        /// <summary>
        /// Get or set the group box header for data export in the view
        /// </summary>
        public string ExportGroupBoxHeader { get; private set; }

        /// <summary>
        /// Get or set the list of reference MfG libraries
        /// </summary>
        public ObservableCollection<RwMfGLibrary> ReferenceMfgLibraries { get; set; }

        /// <summary>
        /// Get or set the relationships between loaded Manufacturing feature in the study and Mfg libraries
        /// </summary>
        public ObservableCollection<RwMfgFeature> StudyMfgFeatures { get; set; }

        /// <summary>
        /// Represents the command for adding the selected Mfg libraries to the collection
        /// </summary>
        public ICommand AddReferenceMfgLibrariesCommand { get; private set; }

        /// <summary>
        /// Represents the command for clearing the Mfg libraries collection
        /// </summary>
        public ICommand ClearReferenceMfgLibrariesCommand { get; private set; }

        /// <summary>
        /// Represents the command for setting the export rule
        /// </summary>
        public ICommand ExportRuleCommand { get; private set; }

        /// <summary>
        /// Represents the command for exporting the manufacturing features relationships to Mfg libraries
        /// </summary>
        public ICommand ExportStudyMfgFeaturesCommand { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create an instance of the Mfg to MfgLibraries relationships
        /// </summary>
        public RwQueryMfgViewModel()
        {
            ReferenceMfgLibrariesGroupBoxHeader = "Reference Mfg libraries";
            StudyMfgFeaturesGroupBoxHeader = "Mfg features in current study";
            ExportGroupBoxHeader = "Data export";
            ReferenceMfgLibraries = new ObservableCollection<RwMfGLibrary>();
            StudyMfgFeatures = new ObservableCollection<RwMfgFeature>();
            AddReferenceMfgLibrariesCommand = new RwActionCommand(AddReferenceMfgLibrariesExecuted, AddReferenceMfgLibrariesCanExecute);
            ClearReferenceMfgLibrariesCommand = new RwActionCommand(ClearReferenceMfgLibrariesExecuted, ClearReferenceMfgLibrariesCanExecute);
            ExportRuleCommand = new RwActionCommand(ExportRuleExecuted, ExportRuleCanExecute);
            ExportStudyMfgFeaturesCommand = new RwActionCommand(ExportStudyFeaturesExecuted, ExportStudyFeaturesCanExecute);
            empContext = new EmpApplication().Context;
            exportRule = "A";
        }
        #endregion

        #region Command enablers
        /// <summary>
        /// Enable or disable the button for adding Mfg libraries to the reference list
        /// </summary>
        /// <param name="arg">Not used</param>
        /// <returns>True if the button can be enabled, otherwise false</returns>
        private bool AddReferenceMfgLibrariesCanExecute(object arg)
        {
            return ReferenceMfgLibraries != null && TxApplication.ActiveSelection.PlanningCount > 0;
        }

        /// <summary>
        /// Enable or disable the button for clearing Mfg libraries reference list
        /// </summary>
        /// <param name="arg">Not used</param>
        /// <returns>True if the button can be enabled, otherwise false</returns>
        private bool ClearReferenceMfgLibrariesCanExecute(object arg)
        {
            return ReferenceMfgLibraries != null && ReferenceMfgLibraries.Count > 0;
        }

        /// <summary>
        /// Enable or disable the command for selecting the export rule
        /// </summary>
        /// <param name="arg">Not used</param>
        /// <returns>Always true</returns>
        private bool ExportRuleCanExecute(object arg)
        {
            return true;
        }

        /// <summary>
        /// Enable or disable the button for exporting the Mfg list
        /// </summary>
        /// <param name="arg">Not used</param>
        /// <returns>True if the button can be enabled, otherwise false</returns>
        private bool ExportStudyFeaturesCanExecute(object arg)
        {
            return StudyMfgFeatures != null && StudyMfgFeatures.Count > 0;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Add the selected objects to the reference list if they are of type MfgLibrary
        /// and start analyzing the Manufacturing features in the study
        /// </summary>
        /// <param name="obj">Not used</param>
        private void AddReferenceMfgLibrariesExecuted(object obj)
        {
            if (TxApplication.ActiveSelection.PlanningCount == 0) return;

            foreach (ITxObject item in TxApplication.ActiveSelection.GetPlanningItems())
            {
                if (item is TxPlanningObject planningObject && planningObject.PlanningType.Equals("PmMfgLibrary"))
                {
                    string name = string.Empty;
                    try
                    {
                        name = planningObject.Name;
                    }
                    catch (TxPlanningObjectNotLoadedException exception)
                    {
                        TxEmsServicesProvider service = planningObject.PlatformServicesProvider as TxEmsServicesProvider;
                        int internalId = service.InternalId;
                        EmpObjectKey objectKey = new EmpObjectKey { objectId = internalId };
                        name = new EmpNode().Name[ref empContext, ref objectKey];
                    }
                    ReferenceMfgLibraries.Add(new RwMfGLibrary(name, planningObject.ProcessModelId.ExternalId));
                }
            }

            AnalyzeStudyMfgFeatures();
        }

        /// <summary>
        /// Analyze the manufacturing features
        /// </summary>
        private void AnalyzeStudyMfgFeatures()
        {
            StudyMfgFeatures.Clear();
            foreach (TxMfgFeature mfgFeature in TxApplication.ActiveDocument.MfgRoot)
            {
                ITxPlanningObject mfgFeaturePlanningRepresentation = GetObjectPlanningRepresentation(mfgFeature);
                int mfgFeatureInternalId = GetPlanningObjectInternalID(mfgFeaturePlanningRepresentation);
                if (mfgFeatureInternalId == 0) return;

                EmpObjectKey empMfgFeatureObjectKey = new EmpObjectKey { objectId = mfgFeatureInternalId };
                List<string> rootMfgLibraryExternalId = GetRootMfgLibraryExternalIDs(empMfgFeatureObjectKey);
                StudyMfgFeatures.Add(new RwMfgFeature(mfgFeature.Name, rootMfgLibraryExternalId));
            };
        }

        /// <summary>
        /// Get the external Id of the root Mfg library for a Mfg
        /// </summary>
        /// <param name="empObjectKey">The key for a Mfg</param>
        /// <returns>A list of Mfg libraries that Mfg belongs to</returns>
        private List<string> GetRootMfgLibraryExternalIDs(EmpObjectKey empObjectKey)
        {
            empMfgFeatureInMfgLibraries = new Dictionary<int, EmpObjectKey>();
            //EmpObjectKey empRootMfgLibrary = GetRootMfGLibrary(empObjectKey);
            GetMfGLibraries(empObjectKey);
            List<string> externalIDs = new List<string>();
            foreach (KeyValuePair<int, EmpObjectKey> pair in empMfgFeatureInMfgLibraries)
            {
                EmpObjectKey value = pair.Value;
                externalIDs.Add(new EmpNode().ExternalID[ref empContext, ref value]);
            }
            return externalIDs;
        }

        /// <summary>
        /// Get the Mfg libraries for a MFg or Mfg library
        /// Recursive call
        /// </summary>
        /// <param name="empObjectKey">The key for a Mfg or Mfg library </param>
        private void GetMfGLibraries(EmpObjectKey empObjectKey)
        {
            EmpObjectKey[] empMfgLibraries = new EmpNode().GetCollections(ref empContext, ref empObjectKey);
            for (int i = 0; i < empMfgLibraries.Length; i++)
            {
                if(new EmpNode().IsKindOf(ref empContext, ref empMfgLibraries[i], "MfgLibrary"))
                {
                    GetMfGLibraries(empMfgLibraries[i]); // recursive call
                }
                else
                {
                    if(!empMfgFeatureInMfgLibraries.ContainsKey(empMfgLibraries[i].objectId))
                    {
                        //empMfgFeatureInMfgLibraries.Add(empObjectKey.objectId, empObjectKey);
                        empMfgFeatureInMfgLibraries.Add(empMfgLibraries[i].objectId, empObjectKey);
                    }
                }
            }
        }

        /// <summary>
        /// Get the planning representation of an object
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The planning representation of the object</returns>
        private ITxPlanningObject GetObjectPlanningRepresentation(ITxObject obj)
        {
            ITxProcessModelObject proccesObject = obj as ITxProcessModelObject;
            ITxPlanningObject planningObject = obj as ITxPlanningObject;
            if (planningObject == null && proccesObject != null)
            {
                planningObject = proccesObject.PlanningRepresentation;
            }
            return planningObject;
        }

        /// <summary>
        /// Get the internal Id of the object
        /// </summary>
        /// <param name="obj">The object</param>
        /// <returns>The internal Id of the object</returns>
        private int GetPlanningObjectInternalID(ITxPlanningObject obj)
        {
            int internalID = 0;
            if (obj != null)
            {
                TxEmsServicesProvider service = obj.PlatformServicesProvider as TxEmsServicesProvider;
                internalID = service.InternalId;
            }
            return internalID;
        }

        /// <summary>
        /// Clear the Mfg libraries reference list
        /// </summary>
        /// <param name="obj">Not used</param>
        private void ClearReferenceMfgLibrariesExecuted(object obj)
        {
            ReferenceMfgLibraries.Clear();
        }

        /// <summary>
        /// Set the data export rule
        /// </summary>
        /// <param name="obj">The parameter of the selected export rule in the view</param>
        private void ExportRuleExecuted(object obj)
        {
            exportRule = obj.ToString();           
        }

        /// <summary>
        /// Export the Mfg list to a file based onthe export rule
        /// </summary>
        /// <param name="obj">Not used</param>
        private void ExportStudyFeaturesExecuted(object obj)
        {
            RwSaveFileDialogCreationData saveFileCreationData = new RwSaveFileDialogCreationData
            {
                Title = "Select the file where to store the informations",
                Filter = "CSV file|*.csv"
            };
            string targetFilename = RwFileAndDirectoryUtilities.SaveFileDialog(saveFileCreationData);
            if (string.IsNullOrEmpty(targetFilename)) return;

            try
            {
                using (StreamWriter file = new StreamWriter(targetFilename))
                {
                    file.WriteLine("Mfg library name; Mfg library external ID");
                    foreach (RwMfGLibrary mfgLibrary in ReferenceMfgLibraries)
                    {
                        file.WriteLine($"{ mfgLibrary.Name }; { mfgLibrary.ExternalID }");
                    }

                    file.WriteLine("Mfg name; Mfg libraries external IDs");
                    foreach (RwMfgFeature mfg in StudyMfgFeatures)
                    {
                        bool isExportMfg = exportRule.Equals("A") || ValidateForExport(mfg);
                        if(isExportMfg)
                        {
                            file.WriteLine($"{ mfg.Name }; { mfg.MfgLibrariesAsString }");
                        }
                    };
                }
                string message = $"Successsfully saved data to file {targetFilename}";
                TxMessageBox.Show(message, "Mfg export", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch
            {
                string message = "An error occured during export";
                TxMessageBox.Show(message, "Data export", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Validate a Mfg based on the export rule if it must be exported or not
        /// </summary>
        /// <param name="mfg">The Mfg to validate</param>
        /// <returns>True if the Mfg must be exported, otherwise false</returns>
        private bool ValidateForExport(RwMfgFeature mfg)
        {
            if (!exportRule.Equals("S")) return true;

            int counter = 0;
            foreach (RwMfGLibrary referenceMfgLibrary in ReferenceMfgLibraries)
            {
                if(mfg.MfgLibrariesAsString.Contains(referenceMfgLibrary.ExternalID))
                {
                    counter++;
                }
            }
            return counter != 1 ? true : false;
        }
        #endregion
    }
}
