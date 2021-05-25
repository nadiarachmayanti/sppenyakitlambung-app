using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using sppenyakitlambung.Services;
using sppenyakitlambung.Utilities.Models.Validation;

namespace sppenyakitlambung.Utilities.Models
{
    public class ValidatableModel : BaseModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidatableModel"/> class.
        /// </summary>
        public ValidatableModel()
        {
            _errors = new ObservableCollection<ValidationError>();
            Validations = new ObservableCollection<ValidationRule>();
            Validations.CollectionChanged += Validations_CollectionChanged;
        }

        /// <summary>
        /// Raises the property changed event if the property has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        public override void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName != nameof(Errors) & Validations.Count > 0)
            {
                Validate();
            }

            base.RaisePropertyChanged(propertyName);
        }

        public bool Validate()
        {
            try
            {
                foreach (ValidationRule validationRule in Validations)
                {
                    if (validationRule.Check(this))
                    {
                        if (_errors.FirstOrDefault(x => x.PropertyPath == validationRule.PropertyPath && x.ValidationMessage == validationRule.ValidationMessage) is ValidationError validationErrorToRemove)
                        {
                            _errors.Remove(validationErrorToRemove);
                            RaisePropertyChanged(nameof(Errors));
                        }
                    }
                    else
                    {
                        if (!_errors.Any(x => x.PropertyPath == validationRule.PropertyPath && x.ValidationMessage == validationRule.ValidationMessage))
                        {
                            _errors.Add(new ValidationError(validationRule.PropertyPath, validationRule.ValidationMessage));
                            RaisePropertyChanged(nameof(Errors));
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                LoggingService.LogErrorMessage(exception, $"ValidatableModel.Validate() '{GetType().Name}'");
            }

            return !_errors.Any();
        }

        private void Validations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Validate();
        }

        private ObservableCollection<ValidationError> _errors;
        public ObservableCollection<ValidationError> Errors
        {
            get => _errors;
            private set
            {
                if (_errors != value)
                {
                    _errors = value;
                    RaisePropertyChanged(nameof(Errors));
                }
            }
        }
        public ObservableCollection<ValidationRule> Validations { get; }
    }
}
