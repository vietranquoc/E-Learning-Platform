using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Authentication.Domain.ViewModel.CommonModel
{
    public class ErrorViewModel
    {
        public string Code { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? Detail {  get; set; } 
    }
}
