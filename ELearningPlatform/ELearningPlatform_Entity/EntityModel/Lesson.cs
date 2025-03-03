using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Entity.EntityModel
{
    [Table("Lessons")]
    public class Lesson : EntityBase
    {
        [Required]
        public Guid CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        [Required]
        public string Title { get; set; }
        public string? VideoUrl { get; set; }
        public string? Content { get; set; }
        [Required]
        public int OrderNumber { get; set; }
    }
}
