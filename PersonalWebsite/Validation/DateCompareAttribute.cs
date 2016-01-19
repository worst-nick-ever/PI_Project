using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PersonalWebsite.Validation
{
    public abstract class DateCompareAttribute : ValidationAttribute
    {
        protected string DateCompareProperty { get; private set; }

        public DateCompareAttribute(string dateCompareProperty, string defaultErrorMessage) : base(defaultErrorMessage)
        {
            if (string.IsNullOrEmpty(dateCompareProperty))
            {
                throw new ArgumentNullException("otherProperty");
            }

            DateCompareProperty = dateCompareProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, DateCompareProperty);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is DateTime)
            {
                var dateCompareProperty = validationContext.ObjectInstance.GetType()
                                   .GetProperty(DateCompareProperty);

                DateTime dateComparePropertyValue = (DateTime)dateCompareProperty
                    .GetValue(validationContext.ObjectInstance, null);

                if (CompareDates(dateComparePropertyValue, (DateTime)value))
                {
                    return new ValidationResult(
                      FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }

        protected abstract bool CompareDates(DateTime date1, DateTime date2);
    }
}