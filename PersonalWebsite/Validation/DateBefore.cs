using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PersonalWebsite.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class DateBefore : DateCompareAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "{0} cannot be after {1}.";

        public DateBefore(string dateBeforeProperty) : base(dateBeforeProperty, DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(dateBeforeProperty))
            {
                throw new ArgumentNullException("dateBeforeProperty");
            }
        }

        protected override bool CompareDates(DateTime date1, DateTime date2)
        {
            return date1 < date2;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelClientValidationDateBeforeRule(FormatErrorMessage(metadata.DisplayName), DateCompareProperty) };
        }

        public class ModelClientValidationDateBeforeRule : ModelClientValidationRule
        {
            public ModelClientValidationDateBeforeRule(string errorMessage, string dateBeforeProperty)
            {
                ErrorMessage = errorMessage;
                ValidationType = "datebefore";
                ValidationParameters.Add("datebeforeproperty", dateBeforeProperty);
            }
        }
    }
}