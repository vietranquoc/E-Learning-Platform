using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Entity.EntityModel
{
    [Table("Courses")]
    public class Course : EntityBase
    {
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public Guid TeacherId { get; set; }
        [ForeignKey("TeacherId")]
        public virtual User Teacher { get; set; }
        public int MaxStudents { get; set; } = 0;

        public virtual ICollection<Enrollment> Enrollment { get; set; }
        public virtual ICollection<Lesson> Lesson { get; set; }
        public virtual ICollection<Quiz> Quiz { get; set; }
    }
}
