using Robworld.PsPublicLibrary.CommandEnablers;
using Tecnomatix.Engineering;

namespace Robworld.PsCommands
{
    /// <summary>
    /// Create frames from list command class
    /// </summary>
    public class CreateFramesFromListCommand : TxButtonCommand
    {
        #region Fields
        private RwAlwaysTrueCommandEnabler enabler;
        #endregion

        #region Properties
        /// <summary>
        /// Get the name of the command
        /// </summary>
        public override string Name
        {
            get { return "Create frames from list"; }
        }

        /// <summary>
        /// Get the Category under which the command appears
        /// </summary>
        public override string Category
        {
            get { return "Robworld GmbH & Co. KG"; }
        }

        /// <summary>
        /// Get the description of the command
        /// </summary>
        public override string Description
        {
            get { return "Create frames from a list"; }
        }

        /// <summary>
        /// Shows the tooltip of the command
        /// </summary>
        public override string Tooltip
        {
            get { return "Create frames from a list"; }
        }

        /// <summary>
        /// Get the 16x16 bitmap of the command
        /// </summary>
        public override string Bitmap
        {
            get { return "Images.Commands.CreateFramesFromList16x16.bmp"; }
        }

        /// <summary>
        /// Get the 32x32 bitmap of the command
        /// </summary>
        public override string LargeBitmap
        {
            get { return "Images.Commands.CreateFramesFromList32x32.png"; }
        }

        /// <summary>
        /// Get the command enabler
        /// </summary>
        public override ITxCommandEnabler CommandEnabler
        {
            get { return enabler; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates an instance of the command
        /// </summary>
        public CreateFramesFromListCommand()
        {
            enabler = new RwAlwaysTrueCommandEnabler();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Execute the selected command
        /// </summary>
        /// <param name="cmdParams"></param>
        public override void Execute(object cmdParams)
        {
            try
            {
                FrameCreationFromList.RwCreateFramesFromListView view = new FrameCreationFromList.RwCreateFramesFromListView();
                view.Show();
            }
            catch (TxException ex)
            {
                string caption = "An Exception occured!!";
                TxMessageBox.ShowModal(ex.Message, caption, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
            }
        }

        /// <summary>
        /// Activate the command if conditions are okay
        /// </summary>
        /// <returns>The boolean value of the conditions check</returns>
        public override bool Connect()
        {
            return true;
        }
        #endregion
    }
}
