using Robworld.PsPublicLibrary.Extensions;
using Robworld.PsPublicLibrary.Utilities;
using System.Diagnostics;
using Tecnomatix.Engineering;

namespace Robworld.PsCommands.OpenMDD
{
    /// <summary>
    /// Open a machine data directory in explorer
    /// </summary>
    internal static class RwOpenMDD
    {
        /// <summary>
        /// Opens the machine data directory of the selected robot
        /// </summary>
        internal static void OpenMachineDataDirectory()
        {
            TxObjectList selectedObjects = TxApplication.ActiveSelection.GetItems();
            if (selectedObjects.Count == 1)
            {
                if (selectedObjects[0] is ITxRobot robot)
                {
                    string path = robot.GetMachineDataDirectory();

                    if (RwFileAndDirectoryUtilities.DoesDirectoryExist(path))
                    {
                        Process.Start("explorer.exe", path);
                    }
                    else
                    {
                        throw new TxArgumentException($"The directory {path} does not exist!");
                    }
                }
            }
        }
    }
}
