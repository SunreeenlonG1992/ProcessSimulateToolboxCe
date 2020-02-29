using Robworld.PsPublicLibrary.Mvvm;
using Robworld.PsPublicLibrary.Utilities;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using Tecnomatix.Engineering;

namespace Robworld.PsCommands.FrameCreationFromList
{
    /// <summary>
    /// Create frames from a list
    /// </summary>
    public class RwCreateFramesFromListViewModel : RwViewModelBase
    {
        #region Fields
        private string frameGroupBoxHeader;
        private string sourceFilename;
        private string framePrefix;
        private string frameSuffix;
        private TxTransformation referenceFrame;
        private readonly TxTransformation originalFrame;
        #endregion

        #region Properties
        /// <summary>
        /// Get the title of the window
        /// </summary>
        public string Title { get { return "Create frames from list"; } }

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
        public string FrameGroupBoxHeader
        {
            get { return frameGroupBoxHeader; }
            private set
            {
                if (frameGroupBoxHeader != value)
                {
                    frameGroupBoxHeader = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the frame data
        /// </summary>
        public ObservableCollection<RwFrameCreationViewModel> FramesData { get; set; }

        /// <summary>
        /// Get or set the prefix for all frames in the collection
        /// </summary>
        public string FramePrefix
        {
            get { return framePrefix; }
            set
            {
                string prefix = value.Trim();
                if (framePrefix != prefix)
                {
                    framePrefix = prefix;
                    UpdateFrameNames();
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the suffix  for all frames in the collection
        /// </summary>
        public string FrameSuffix
        {
            get { return frameSuffix; }
            set
            {
                string suffix = value.Trim();
                if (frameSuffix != suffix)
                {
                    frameSuffix = suffix;
                    UpdateFrameNames();
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the full filename of the source file
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
        /// Get or set the reference frame for the frame creation process
        /// </summary>
        public TxTransformation ReferenceFrame
        {
            get { return referenceFrame; }
            set
            {
                if(referenceFrame != value)
                {
                    referenceFrame = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Represents the command for choosing the source file
        /// </summary>
        public ICommand ChooseSourceFileCommand { get; private set; }

        /// <summary>
        /// Represents the command for executing the frame creation process
        /// </summary>
        public ICommand ExecuteFrameCreationCommand { get; private set; }

        /// <summary>
        /// Represents the command for choosing the reference frame
        /// </summary>
        public ICommand ReferenceFramePickedEvent { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create an instance of the frame creation command
        /// </summary>
        public RwCreateFramesFromListViewModel()
        {
            FrameGroupBoxHeader = "Frame data";
            SourceFilename = string.Empty;
            FramesData = new ObservableCollection<RwFrameCreationViewModel>();
            FramePrefix = "fr_";
            FrameSuffix = string.Empty;
            ReferenceFrame = null;
            originalFrame = TxApplication.ActiveDocument.WorkingFrame;
            ChooseSourceFileCommand = new RwActionCommand(ChooseSourceFileExecuted, ChooseSourceFileCanExecute);
            ExecuteFrameCreationCommand = new RwActionCommand(FrameCreationExecuted, FrameCreationCanExecute);
            ReferenceFramePickedEvent = new RwActionCommand(ReferenceFramePickedExecuted, ReferenceFramePickedCanExecute);
            FramesData.CollectionChanged += FrameCollectionChanged;
        }
        #endregion

        #region Command enablers
        /// <summary>
        /// Decide if the command for picking a reference frame can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns always true</returns>
        private bool ReferenceFramePickedCanExecute(object arg)
        {
            return true;
        }

        /// <summary>
        /// Decide if the command for executing the frame creation can be enabled
        /// </summary>
        /// <param name="arg">Not in use</param>
        /// <returns>Returns true if frames are defined</returns>
        private bool FrameCreationCanExecute(object arg)
        {
            return FramesData != null && FramesData.Count > 0;
        }

        /// <summary>
        /// Decide if the command for choosing a source file can be enabled
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
        /// Set the reference frame for the frame creation process
        /// </summary>
        /// <param name="obj">The TxObjEditBoxControl of the view</param>
        private void ReferenceFramePickedExecuted(object obj)
        {
            if(obj is Tecnomatix.Engineering.Ui.WPF.TxObjEditBoxControl)
            {
                ReferenceFrame = ((obj as Tecnomatix.Engineering.Ui.WPF.TxObjEditBoxControl).Object as ITxLocatableObject).AbsoluteLocation;
            }
        }

        /// <summary>
        /// Execute the frame creation process
        /// </summary>
        /// <param name="obj">Not in use</param>
        private void FrameCreationExecuted(object obj)
        {
            if(ReferenceFrame != null && ReferenceFrame != originalFrame)
            {
                TxApplication.ActiveDocument.WorkingFrame = ReferenceFrame;
            }
            try
            {
                foreach (RwFrameCreationViewModel frameViewModel in FramesData)
                {
                    TxTransformation location = RwMathUtilities.CreateRpyTransformation(frameViewModel.X, frameViewModel.Y, frameViewModel.Z, frameViewModel.Rx, frameViewModel.Ry, frameViewModel.Rz, true);
                    TxFrameCreationData frameCreationData = new TxFrameCreationData(frameViewModel.Name, location);
                    TxApplication.ActiveDocument.PhysicalRoot.CreateFrame(frameCreationData);
                }
                string message = $"Successsfully created {FramesData.Count} frames";
                TxMessageBox.Show(message, "Frame creation", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
            }
            catch
            {
                string message = "An error occured during frame creation";
                TxMessageBox.Show(message, "Frame creation", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                TxApplication.ActiveDocument.WorkingFrame = originalFrame;
            }
        }

        /// <summary>
        /// Choose a source file on the file system
        /// </summary>
        /// <param name="obj">Not in use</param>
        private void ChooseSourceFileExecuted(object obj)
        {
            RwOpenFileDialogCreationData creationData = new RwOpenFileDialogCreationData
            {
                Title = "Select the files with frame informations",
                Multiselect = false,
                Filter = "Frame file|*.csv"
            };
            SourceFilename = RwFileAndDirectoryUtilities.OpenFileDialog(creationData);
            if (!string.IsNullOrEmpty(SourceFilename))
            {
                ReadFrameData();
            }
        }

        /// <summary>
        /// Read the frame datas of the source file
        /// </summary>
        private void ReadFrameData()
        {
            try
            {
                string[] framesData = System.IO.File.ReadAllLines(SourceFilename);
                foreach (string frameData in framesData)
                {
                    string[] frameDetails = frameData.Split(new char[] { ';' });
                    if (frameDetails.Length < 7) continue;

                    string frameNameInFile = frameDetails[0].Trim();
                    double transX = 0.0;
                    double transY = 0.0;
                    double transZ = 0.0;
                    double rotX = 0.0;
                    double rotY = 0.0;
                    double rotZ = 0.0;

                    if (RwDataConversionUtilities.ConvertToDouble(frameDetails[1], out double x))
                    {
                        transX = x;
                    }
                    if (RwDataConversionUtilities.ConvertToDouble(frameDetails[2], out double y))
                    {
                        transY = y;
                    }
                    if (RwDataConversionUtilities.ConvertToDouble(frameDetails[3], out double z))
                    {
                        transZ = z;
                    }
                    if (RwDataConversionUtilities.ConvertToDouble(frameDetails[4], out double rx))
                    {
                        rotX = rx;
                    }
                    if (RwDataConversionUtilities.ConvertToDouble(frameDetails[5], out double ry))
                    {
                        rotY = ry;
                    }
                    if (RwDataConversionUtilities.ConvertToDouble(frameDetails[6], out double rz))
                    {
                        rotZ = rz;
                    }

                    if(!string.IsNullOrEmpty(frameNameInFile))
                    {
                        RwFrameCreationViewModel creationData = new RwFrameCreationViewModel(frameNameInFile, transX, transY, transZ, rotX, rotY, rotZ)
                        {
                            Name = $"{ FramePrefix }{ frameNameInFile }{ FrameSuffix }"
                        };
                        FramesData.Add(creationData);
                    }
                }
            }
            catch (ArgumentException exception)
            {
                string message = exception.Message;
            }
        }

        /// <summary>
        /// Update the frame names in the collection depending on changes on frame prefix of frame suffix
        /// </summary>
        private void UpdateFrameNames()
        {
            if (FramesData == null || FramesData.Count == 0)
            {
                return;
            }
            foreach (RwFrameCreationViewModel frameData in FramesData)
            {
                frameData.Name = $"{ FramePrefix }{ frameData.DefaultName }{ FrameSuffix }";
            }
        }

        /// <summary>
        /// Update the groupbox header of the frame collection
        /// </summary>
        /// <param name="sender">The frame collection</param>
        /// <param name="e">The collection changed event arguments</param>
        private void FrameCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (FramesData != null)
            {
                FrameGroupBoxHeader = $"Frame data ({FramesData.Count})";
            }
        }
        #endregion
    }
}
