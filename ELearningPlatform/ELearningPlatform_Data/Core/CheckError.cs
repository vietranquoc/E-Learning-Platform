using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Data.Core
{
    public class CheckError
    {
        public bool IsError { get; set; }
        public string Code { get; set; }
        public Exception Exception { get; set; }
        public string Message { get; set; }
    }
}
