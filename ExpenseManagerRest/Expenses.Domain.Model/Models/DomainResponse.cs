using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Model.Models
{
    public class DomainResponse<T>
    {
        public DomainResponse()
        {
            IsSuccess = true; //Default success will be overwritten when Errors are added
        }

        public List<ErrorDescription> ErrorDetails { get; set; }
        public T Value { get; set; }
        public bool IsSuccess { get; set; }

        public void SetSuccessResult(T successValue)
        {
            Value = successValue;
            IsSuccess = true;
        }

        public void AddErrorDescription(int errorNumber, string errorMessage, string errorReason = null)
        {
            IsSuccess = false;
            if (ErrorDetails == null)
            {
                if (!string.IsNullOrEmpty(errorReason))
                {
                    ErrorDetails = new List<ErrorDescription>()
                    {
                            new ErrorDescription()
                            {
                                Description = errorMessage + "Reason :"  +  errorReason,
                                Number = errorNumber
                            }
                    };
                }
                else
                {
                    ErrorDetails = new List<ErrorDescription>()
                    {
                            new ErrorDescription()
                            {
                                Description = errorMessage,
                                Number = errorNumber
                            }
                    };

                }
            }
            else
            {
                if (!string.IsNullOrEmpty(errorReason))
                {
                    ErrorDetails.Add
                                (
                                    new ErrorDescription()
                                    {
                                        Description = errorMessage +
                                                        "Reason :" + errorReason
                                                        ,
                                        Number = errorNumber
                                    }
                                );
                }
                else
                {
                    ErrorDetails.Add
                         (
                             new ErrorDescription()
                             {
                                 Description = errorMessage,
                                 Number = errorNumber
                             }
                         );
                }
            }
        }

        public string ErrorSummary
        {
            get
            {
                if (ErrorDetails == null || !ErrorDetails.Any())
                    return null;

                StringBuilder sb = new StringBuilder();
                foreach (ErrorDescription ed in ErrorDetails)
                {
                    sb.AppendLine(ed.Description);
                }
                return sb.ToString();
            }
        }
    }

    public class ErrorDescription
    {
        public string Description { get; set; }
        public int Number { get; set; }      
    }
}

