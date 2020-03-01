using Tecnomatix.Engineering;

namespace Robworld.PsPublicLibrary.CommandEnablers
{
    /// <summary>
    /// Selection enabler class for robots with more than 3 axis
    /// </summary>
    public class RwSingleRobotCommandEnabler : TxCommandSelectionEnabler
    {
        #region Constructors
        /// <summary>
        /// Creates a SingleRobotCommandEnabler instance
        /// </summary>
        public RwSingleRobotCommandEnabler() : base()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Disable the command if command is initialized by the application
        /// </summary>
        protected override void OnSelectionCleared()
        {
            _enable = false;
        }

        /// <summary>
        /// Disable the command if items are added to the selection
        /// </summary>
        protected override void OnSelectionItemsAdded(TxObjectList objects)
        {
            _enable = false;
        }

        /// <summary>
        /// Check the remaining selection if it contains only one item and if it is a robot with more than three axis
        /// </summary>
        protected override void OnSelectionItemsRemoved(TxObjectList objects)
        {
            CheckSelection(TxApplication.ActiveSelection.GetAllItems());
        }

        /// <summary>
        /// Check the new selection if it contains only one item and if it is a robot with more than three axis
        /// </summary>
        protected override void OnSelectionItemsSet(TxObjectList objects)
        {
            CheckSelection(objects);
        }

        /// <summary>
        /// Check the selection if it is a robot with more than three joints.
        /// Only the first object in the list will be checked.
        /// It is assumed that a device with more than three joints represents a robot.
        /// </summary>
        /// <param name="objects">The list of objects</param>
        private void CheckSelection(TxObjectList objects)
        {
            bool flag = false;
            if (objects.Count == 1)
            {
                if (objects[0] is ITxRobot robot)
                {
                    if ((robot as ITxDevice).DrivingJoints.Count > 3)
                    {
                        flag = true;
                    }
                }
            }
            _enable = flag;
        }
        #endregion
    }
}
