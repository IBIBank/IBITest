using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
//[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
//public class UnlikeAttribute : ValidationAttribute, IClientValidatable
//{
//    private const string DefaultErrorMessage = "The value of {0} cannot be the same as the value of the {1}.";

//    public string OtherProperty { get; private set; }
//    public string OtherPropertyName { get; private set; }

//    public UnlikeAttribute(
//        string otherProperty,
//        string otherPropertyName)
//        : base(DefaultErrorMessage)
//    {
//        OtherProperty = otherProperty;
//        OtherPropertyName = otherPropertyName;
//    }
//    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
//    {
//        if (value != null)
//        {
//            var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(OtherProperty);

//            var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

//            if (value.Equals(otherPropertyValue))
//            {
//                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
//            }
//        }

//        return ValidationResult.Success;

//    }
//    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
//    {
//        var rule = new ModelClientValidationRule()
//        {
//            ValidationType = "unlike",
//            ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
//        };

//        rule.ValidationParameters.Add("otherproperty", OtherProperty);
//        rule.ValidationParameters.Add("otherpropertyname", OtherPropertyName);

//        yield return rule;
//    }

//    public override string FormatErrorMessage(string name)
//    {
//        return string.Format(ErrorMessageString, name, OtherPropertyName);
//    }
//}
namespace IBITest.Models
{
    public class FundTransferViewModel
    {
        [Required]
        public long FromAccount { get; set; }

        [Required]
        //[Unlike("FromAccount", "FromAccount")]
        public long ToAccount { get; set; }
        
        [Required]
        [Range(1,25000)]
        public decimal Amount { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string TransactionPassword { get; set; }
        
        [Required]
        [StringLength(100)]
        public string remarks { get; set; }
    }
}