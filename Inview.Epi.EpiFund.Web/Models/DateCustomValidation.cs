using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Inview.Epi.EpiFund.Web.Models
{
	public class DateCustomValidation
	{
		public DateCustomValidation()
		{
		}

		[AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
		public class DateValidationAttribute : ValidationAttribute
		{
			public DateValidationAttribute()
			{
			}

			protected override ValidationResult IsValid(object value, ValidationContext validationContext)
			{
				ValidationResult success = ValidationResult.Success;
				try
				{
					if ((DateTime)value <= new DateTime(1900, 1, 1))
					{
						success = new ValidationResult("Invalid date. Must be greater than 1/1/1900");
					}
				}
				catch (Exception exception)
				{
					throw exception;
				}
				return success;
			}

			[AttributeUsage(AttributeTargets.Property, AllowMultiple=true)]
			public class DateGreaterThanAttribute : ValidationAttribute
			{
				private string otherPropertyName;

				public DateGreaterThanAttribute(string otherPropertyName, string errorMessage) : base(errorMessage)
				{
					this.otherPropertyName = otherPropertyName;
				}

				protected override ValidationResult IsValid(object value, ValidationContext validationContext)
				{
					ValidationResult success = ValidationResult.Success;
					try
					{
						PropertyInfo property = validationContext.ObjectType.GetProperty(this.otherPropertyName);
						if (!property.PropertyType.Equals(new DateTime().GetType()))
						{
							success = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
						}
						else
						{
							DateTime dateTime = (DateTime)value;
							DateTime dateTime1 = (DateTime)property.GetValue(validationContext.ObjectInstance, null);
							if (dateTime <= new DateTime(1900, 1, 1))
							{
								success = new ValidationResult("Invalid date. Must be greater than 1/1/1900");
							}
							if (dateTime.CompareTo(dateTime1) < 1)
							{
								success = new ValidationResult(base.ErrorMessageString);
							}
						}
					}
					catch (Exception exception)
					{
						throw exception;
					}
					return success;
				}
			}
		}
	}
}