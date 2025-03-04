using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Authentication.Domain.ViewModel.CommonModel
{
    public class ELearningValidationException : ELearningException
    {
        public ELearningValidationException(IEnumerable<ErrorViewModel> errors) : base(400, "DataInvalid")
        {
            Errors = errors;
        }

        public IEnumerable<ErrorViewModel> Errors { get; private set; }
    }
}
