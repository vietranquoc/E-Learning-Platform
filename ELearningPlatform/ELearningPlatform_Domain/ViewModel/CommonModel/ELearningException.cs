using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Authentication.Domain.ViewModel.CommonModel
{
    public class ELearningException : Exception
    {
        public int StatusCode { get; private set; }
        public string? Detail {  get; private set; }

        public ELearningException(int statusCode, string detail) : base(detail) 
        { 
            StatusCode = statusCode;
            Detail = detail;
        }
    }
}
