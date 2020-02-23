using Tecnomatix.Engineering.Ui.WPF;

namespace Robworld.PsCommands.MfgIEQ
{
    /// <summary>
    /// Interaktionslogik für RwMfgImportView.xaml
    /// </summary>
    public partial class RwImportMfgView : TxWindow
    {
        public RwImportMfgView()
        {
            InitializeComponent();
            MfgLibraryBox.SetValidator(new RwMfgLibraryValidator());
        }
    }
}
