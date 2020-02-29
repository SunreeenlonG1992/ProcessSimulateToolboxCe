using System.Globalization;

namespace Robworld.PsPublicLibrary.Utilities
{
    /// <summary>
    /// Methods for conversion of a data type to another data type
    /// </summary>
    public static class RwDataConversionUtilities
    {
        #region Fields
        private static readonly NumberStyles doubleNumberStyle = NumberStyles.Float;
        private static readonly CultureInfo culture = CultureInfo.InvariantCulture;
        #endregion

        /// <summary>
        /// Convert a string to a double
        /// </summary>
        /// <param name="value">The string to be converted</param>
        /// <param name="number">The variable where the converted value is stored</param>
        /// <returns>The conversion result</returns>
        public static bool ConvertToDouble(string value, out double number)
        {
            if(double.TryParse(value, doubleNumberStyle, culture, out number))
            {
                return true;
            }
            else
            {
                number = double.NaN;
                return false;
            }
        }

        /// <summary>
        /// Convert a string to an integer
        /// </summary>
        /// <param name="value">The string to be converted</param>
        /// <param name="number">The variable where the converted value is stored</param>
        /// <returns>The conversion result</returns>
        public static bool ConvertToInteger(string value, out int number)
        {
            if (int.TryParse(value, NumberStyles.None, culture, out number))
            {
                return true;
            }
            else
            {
                number = 0;
                return false;
            }
        }
    }
}
