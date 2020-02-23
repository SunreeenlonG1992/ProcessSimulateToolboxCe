using Robworld.PsPublicLibrary.Mvvm;
using Tecnomatix.Engineering;

namespace Robworld.PsCommands.LogicalGroups
{
    /// <summary>
    /// Represents a logical group
    /// </summary>
    public class RwLogicalGroupViewModel : RwViewModelBase
    {
        #region Fields
        private string groupname;
        #endregion

        #region Properties
        /// <summary>
        /// Get or set the name of the logical group
        /// </summary>
        public string GroupName
        {
            get { return groupname; }
            set
            {
                if (groupname != value)
                {
                    groupname = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the type name of the logical group
        /// </summary>
        public string GroupTypeName { get; set; }

        /// <summary>
        /// Get or set the items that belongs to the logical group
        /// </summary>
        public TxObjectList Items { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of the logical group view model
        /// </summary>
        public RwLogicalGroupViewModel()
        {
            Items = new TxObjectList();
        } 
        #endregion
    }
}
