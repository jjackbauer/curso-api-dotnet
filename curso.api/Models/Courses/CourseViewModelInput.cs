using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace curso.api.Models.Courses
{
    public class CourseViewModelInput
    {
        [Required(ErrorMessage ="Name is mandatory")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is mandatory")]
        public string Description { get; set; }

    }
}
