using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyWebApp.ValidationAttributes
{
    public class CorrectSetOfAnswersAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string strValue = (string)value;
            if (!string.IsNullOrEmpty(strValue))
            {
                string[] answers= strValue.Split(';');
                foreach(var ans in answers)
                {
                    ans.Trim();
                    if (string.IsNullOrEmpty(ans))
                        return false;
                }
                return true;
               
            }
            return true;
        }
    }
}