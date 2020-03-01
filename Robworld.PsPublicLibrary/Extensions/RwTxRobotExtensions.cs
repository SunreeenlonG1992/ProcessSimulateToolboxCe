using Robworld.PsPublicLibrary.Utilities;
using Tecnomatix.Engineering;

namespace Robworld.PsPublicLibrary.Extensions
{
    /// <summary>
    /// Extension methods for TxRobot
    /// </summary>
    public static class RwTxRobotExtensions
    {
        /// <summary>
        /// Get the directory of a selected robot in the RobotsMachineDataFiles directory
        /// </summary>
        /// <param name="robot">The robot</param>
        /// <returns>The path to the robot machine data directory</returns>
        public static string GetMachineDataDirectory(this ITxRobot robot)
        {
            string path = string.Empty;
            string systemRoot = TxApplication.SystemRootDirectory;
            if (robot is ITxProcessModelObject processModelObject)
            {
                path = RwFileAndDirectoryUtilities.CombinePathSegments(systemRoot.TrimEnd('\\'), "RobotsMachineDataFiles", processModelObject.ProcessModelId.ExternalId);
            }
            return path;
        }
    }
}
