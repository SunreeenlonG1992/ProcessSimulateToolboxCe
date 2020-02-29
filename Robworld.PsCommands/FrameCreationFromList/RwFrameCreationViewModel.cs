using Robworld.PsPublicLibrary.Mvvm;

namespace Robworld.PsCommands.FrameCreationFromList
{
    /// <summary>
    /// Represents the frame details
    /// </summary>
    public class RwFrameCreationViewModel : RwViewModelBase
    {
        #region Fields
        private string name;
        private double x;
        private double y;
        private double z;
        private double rx;
        private double ry;
        private double rz;
        #endregion

        #region Properties
        /// <summary>
        /// Get the original name of the frame
        /// </summary>
        public string DefaultName { get; }

        /// <summary>
        /// Get or set the name of the frame
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the X position of the frame
        /// </summary>
        public double X
        {
            get { return x; }
            set
            {
                if (x != value)
                {
                    x = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the Y position of the frame
        /// </summary>
        public double Y
        {
            get { return y; }
            set
            {
                if (y != value)
                {
                    y = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the Z position of the frame
        /// </summary>
        public double Z
        {
            get { return z; }
            set
            {
                if (z != value)
                {
                    z = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the X rotation of the frame
        /// </summary>
        public double Rx
        {
            get { return rx; }
            set
            {
                if (rx != value)
                {
                    rx = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the Y rotation of the frame
        /// </summary>
        public double Ry
        {
            get { return ry; }
            set
            {
                if (ry != value)
                {
                    ry = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Get or set the Z rotation of the frame
        /// </summary>
        public double Rz
        {
            get { return rz; }
            set
            {
                if (rz != value)
                {
                    rz = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new frame data instance
        /// </summary>
        /// <param name="originalName">The original name. This name will not change</param>
        public RwFrameCreationViewModel(string originalName)
        {
            DefaultName = originalName;
        }

        /// <summary>
        /// Create a new frame data instance
        /// </summary>
        /// <param name="originalName">The original name. This name will not change</param>
        /// <param name="x">The X coordinate of the frame</param>
        /// <param name="y">The Y coordinate of the frame</param>
        /// <param name="z">The Z coordinate of the frame</param>
        /// <param name="rx">The rotation around the X axis</param>
        /// <param name="ry">The rotation around the Y axis</param>
        /// <param name="rz">The rotation around the Z axis</param>
        public RwFrameCreationViewModel(string originalName, double x, double y, double z, double rx, double ry, double rz) : this(originalName)
        {
            X = x;
            Y = y;
            Z = z;
            Rx = rx;
            Ry = ry;
            Rz = rz;
        }
        #endregion
    }
}
