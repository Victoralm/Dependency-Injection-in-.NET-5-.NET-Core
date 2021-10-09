using System.Collections.Generic;
using System.Threading.Tasks;
using WazeCredit.Models;

namespace WazeCredit.Services
{
    public class CreditValidator : ICreditValidator
    {
        private readonly IEnumerable<IValidationChecker> _validations;

        public CreditValidator(IEnumerable<IValidationChecker> validations)
        {
            this._validations = validations;
        }

        /// <summary>
        /// Check all validations for the CreditApplication model
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A boolean indicating that the operation success or fail. A list of string of all the errors if it fails.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<(bool, IEnumerable<string>)> PassAllValidations(CreditApplication model)
        {
            bool validationsPassed = true;

            // Used to iterate through all the validation error messages
            List<string> errorMessages = new List<string>();

            foreach (var validation in this._validations)
            {
                if (!validation.ValidatorLogic(model))
                {
                    // Error
                    errorMessages.Add(validation.ErrorMessage);
                    validationsPassed = false;
                }
            }

            return (validationsPassed, errorMessages);
        }
    }
}
