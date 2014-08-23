using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyWebApp.ValidationAttributes
{
    public class CorrectSetOfAnswersAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            HashSet<string> set= new HashSet<string>(); 
            string strValue = (string)value;
            if (!string.IsNullOrEmpty(strValue))
            {
                string[] answers= strValue.Split(';');
                foreach(var ans in answers)
                {
                    ans.Trim();
                    if (string.IsNullOrEmpty(ans))
                    {
                        ErrorMessage="Answer cannot be empty or whitespace";
                        return false;
                    }
                    if(set.Contains(ans))
                    {
                        ErrorMessage="Answers must be unique.";
                        return false;
                    }
                    set.Add(ans);
                }
                return true;
               
            }
            return true;
        }
    }
}