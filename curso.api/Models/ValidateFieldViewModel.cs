using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Models
{
    public class ValidateFieldViewModelOutput
    {
        public IEnumerable<string> Errors { get; private set; }
        public ValidateFieldViewModelOutput(IEnumerable<string> Errors)
        {
            this.Errors = Errors;
        }
    }
}
