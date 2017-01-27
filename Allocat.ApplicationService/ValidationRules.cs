using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace Allocat.ApplicationService
{
    public class ValidationRules
    {
        private Boolean _validationStatus { get; set; }
        private List<String> _validationMessage { get; set; }
        private Hashtable _validationErrors;

        public Boolean ValidationStatus { get { return _validationStatus; } }
        public List<String> ValidationMessage { get { return _validationMessage; } }
        public Hashtable ValidationErrors { get { return _validationErrors; } }

        public ValidationRules()
        {
            _validationStatus = true;
            _validationMessage = new List<string>();
            _validationErrors = new Hashtable();
        }

        public void UpdateMessages(Boolean validationStatus, List<String> validationMessages, Hashtable validationErrors)
        {
            if (validationStatus == false) _validationStatus = false;

            foreach (string validationMessage in validationMessages)
            {
                _validationMessage.Add(validationMessage);
            }

            foreach (DictionaryEntry validationError in validationErrors)
            {
                if (_validationErrors.ContainsKey(validationError.Key) == false)
                {
                    _validationErrors.Add(validationError.Key, validationError.Value);
                }
            }

        }
      
        public void InitializeValidationRules()
        {
            _validationStatus = true;
            _validationMessage = new List<string>();
            _validationErrors = new Hashtable();
        }

        public Boolean ValidateRequired(object entity, string friendlyName)
        {
            if (Validations.ValidateRequired(entity) == false)
            {
                string errorMessage = friendlyName + " is a required field.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;

        }

        public Boolean ValidateGuidRequired(object entity, string friendlyName, string displayPropertyName)
        {
            if (Validations.ValidateRequiredGuid(entity) == false)
            {
                string errorMessage = friendlyName + " is a required field.";
                if (displayPropertyName == string.Empty)
                {
                    AddValidationError(friendlyName, errorMessage);
                }
                else
                {
                    AddValidationError(displayPropertyName, errorMessage);
                }
                return false;
            }

            return true;

        }

        public void ValidationError(string propertyName, string errorMessage)
        {
            AddValidationError(propertyName, errorMessage);
        }
        
        public Boolean ValidateLength(object entity, string friendlyName, int maxLength)
        {
            
            if (Validations.ValidateLength(entity, maxLength) == false)
            {
                string errorMessage = friendlyName + " exceeds the maximum of " + maxLength + " characters long.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;
        }

        public Boolean ValidateNumeric(object entity, string friendlyName)
        {
            if (Validations.IsInteger(entity) == false)
            {
                string errorMessage = friendlyName + " is not a valid number.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;
        }

        public Boolean ValidateGreaterThanZero(object entity, string friendlyName)
        {
            
            if (Validations.ValidateGreaterThanZero(entity) == false)
            {
                string errorMessage = friendlyName + " must be greater than zero.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;
        }
       
        public Boolean ValidateDecimalGreaterThanZero(object entity, string friendlyName)
        {
            
            if (Validations.ValidateDecimalGreaterThanZero(entity) == false)
            {
                string errorMessage = friendlyName + " must be greater than zero.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;
        }

        public Boolean ValidateDecimalIsNotZero(object entity, string friendlyName)
        {
            
            if (Validations.ValidateDecimalIsNotZero(entity) == false)
            {
                string errorMessage = friendlyName + " must not equal zero.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;
        }

        public Boolean ValidateSelectedValue(object entity, string friendlyName)
        {
            
            if (Validations.ValidateGreaterThanZero(entity) == false)
            {
                string errorMessage = friendlyName + " not selected.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;
        }

        public Boolean ValidateIsDate(object entity, string friendlyName)
        {
            
            if (Validations.IsDate(entity) == false)
            {
                string errorMessage = friendlyName + " is not a valid date.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;
        }

        public Boolean ValidateIsDateOrNullDate(object entity, string friendlyName)
        {
            
            if (Validations.IsDateOrNullDate(entity) == false)
            {
                string errorMessage = friendlyName + " is not a valid date.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;
        }

        public Boolean ValidateRequiredDate(object entity, string friendlyName)
        {
            
            if (Validations.IsDateGreaterThanDefaultDate(entity) == false)
            {
                string errorMessage = friendlyName + " is a required field.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;
        }

        public Boolean ValidateDateGreaterThanOrEqualToToday(object entity, string friendlyName)
        {
            
            if (Validations.IsDateGreaterThanOrEqualToToday(entity) == false)
            {
                string errorMessage = friendlyName + " must be greater than or equal to today.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;
        }

        public Boolean ValidateEmailAddress(object entity, string friendlyName)
        {
            
            string stringValue;

            if (entity == null) return true;

            stringValue = entity.ToString();
            if (stringValue == string.Empty) return true;

            if (Validations.ValidateEmailAddress(entity.ToString()) == false)
            {
                string emailAddressErrorMessage = friendlyName + " is not a valid email address";
                AddValidationError(friendlyName, emailAddressErrorMessage);
                return false;
            }

            return true;
        }
  
        public Boolean ValidateURL(object entity, string friendlyName)
        {
            
            string stringValue;

            if (entity == null) return true;

            stringValue = entity.ToString();
            if (stringValue == string.Empty) return true;

            if (Validations.ValidateURL(entity.ToString()) == false)
            {
                string urlErrorMessage = friendlyName + " is not a valid URL address";
                AddValidationError(friendlyName, urlErrorMessage);
                return false;
            }

            return true;
        }

        public Boolean ValidateRequiredForTable(object entity, string friendlyName)
        {
            if (Validations.ValidateRequired(entity) == false)
            {
                string errorMessage = friendlyName + " is a required field.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;

        }

        public Boolean ValidateIsDateOrNullOrEmptyDate(object entity, string friendlyName)
        {
            if (Validations.IsDateOrNullOrEmptyDate(entity) == false)
            {
                string errorMessage = friendlyName + " is not a valid date.";
                AddValidationError(friendlyName, errorMessage);
                return false;
            }

            return true;
        }

        public void AddValidationError(string propertyName, string errorMessage)
        {
            if (_validationErrors.Contains(propertyName) == false)
            {
                _validationErrors.Add(propertyName, errorMessage);
                _validationMessage.Add(errorMessage);
            }

            _validationStatus = false;
        }
    }
}
