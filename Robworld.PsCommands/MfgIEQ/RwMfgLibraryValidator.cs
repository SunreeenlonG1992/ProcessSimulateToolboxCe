using System;
using Tecnomatix.Planning;
using Tecnomatix.Engineering;
using Tecnomatix.Engineering.Ui;

namespace Robworld.PsCommands.MfgIEQ
{
    /// <summary>
    /// Represents a custom validator for a Mfg library 
    /// </summary>
    public class RwMfgLibraryValidator : ITxValidator
    {
        #region Fields
        private readonly string planningType = "PmMfgLibrary";
        private readonly string errorMessageInvalid = "INVALID_MFG_CONTAINER";
        #endregion

        #region Methods
        /// <summary>
        /// Get the name of the picked object
        /// </summary>
        /// <param name="obj">The picked object</param>
        /// <returns>The name of the picked object</returns>
        public string GetText(ITxObject obj)
        {
            string name = string.Empty;
            try
            {
                if (obj != null)
                {
                    name = obj.Name;
                }
            }
            catch (TxPlanningObjectNotLoadedException)
            {
                if (obj is ITxPlanningObject planningObject)
                {
                    if (planningObject.PlanningType.Equals(planningType) && planningObject.HasFields)
                    {
                        name = planningObject.GetField("name").ToString();
                    }
                }
            }
            catch (TxPlanningObjectFieldNotLoadedException)
            {
                name = "Error";
            }
            return name;
        }

        /// <summary>
        /// Check the picked object if it is of valid type
        /// </summary>
        /// <param name="obj">The picked object</param>
        /// <param name="errorMessage">An errormessage</param>
        /// <returns>True if the picked object is valid otherwise false</returns>
        public bool IsValidObject(ITxObject obj, out string errorMessage)
        {
            bool isValid = false;
            errorMessage = null;
            if (obj != null)
            {
                if (obj is ITxPlanningObject planningObject)
                {
                    if (planningObject.PlanningType.Equals(planningType))
                    {
                        isValid = true;
                    }
                    else
                    {
                        errorMessage = errorMessageInvalid;
                    }
                }
            }
            return isValid;
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public ITxObject GetObject(string text)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Not implemented
        /// </summary>
        /// <param name="text"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public bool IsValidText(string text, out string errorMessage)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
