using System;
using System.ComponentModel.DataAnnotations;

namespace Forum.WebApi.Models.Identity
{
    public class BirthDateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            var birthDate = Convert.ToDateTime(value);
            return birthDate > new DateTime(1980, 1, 1) && birthDate < new DateTime(2015, 1, 1);
        }
    }
}
