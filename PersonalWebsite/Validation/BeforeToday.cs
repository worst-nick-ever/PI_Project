using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalWebsite.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class BeforeToday : DateCompareAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "{0} cannot be after {1}.";

        public BeforeToday() : base("Today", DefaultErrorMessage)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null && value is DateTime)
            {
                if (CompareDates((DateTime)value, DateTime.Now))
                {
                    return new ValidationResult(
                      FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }

        protected override bool CompareDates(DateTime date1, DateTime date2)
        {
            return date1 > date2;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelClientValidationBeforeTodayRule(FormatErrorMessage(metadata.DisplayName), DateCompareProperty) };
        }

        public class ModelClientValidationBeforeTodayRule : ModelClientValidationRule
        {
            public ModelClientValidationBeforeTodayRule(string errorMessage, string beforeTodayProperty)
            {
                ErrorMessage = errorMessage;
                ValidationType = "beforetoday";
                ValidationParameters.Add("beforetodayproperty", beforeTodayProperty);
            }
        }
    }
}