using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PersonalWebsite.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DateAfter : DateCompareAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "{0} cannot be before {1}.";

        public DateAfter(string dateAfterProperty) : base(dateAfterProperty, DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(dateAfterProperty))
            {
                throw new ArgumentNullException("dateAfterProperty");
            }
        }

        protected override bool CompareDates(DateTime date1, DateTime date2)
        {
            return date1 > date2;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelClientValidationDateAfterRule(FormatErrorMessage(metadata.DisplayName), DateCompareProperty) };
        }

        public class ModelClientValidationDateAfterRule : ModelClientValidationRule
        {
            public ModelClientValidationDateAfterRule(string errorMessage, string dateAfterProperty)
            {
                ErrorMessage = errorMessage;
                ValidationType = "dateafter";
                ValidationParameters.Add("dateafterproperty", dateAfterProperty);
            }
        }
    }
}