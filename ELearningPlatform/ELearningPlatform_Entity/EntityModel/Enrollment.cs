using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Entity.EntityModel
{
    public enum EnrollmentStatus
    {
        Active,
        Completed,
        Canceled
    }

    [Table("Enrollments")]
    public class Enrollment : EntityBase
    {
        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Required]
        public Guid CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        [Required]
        public DateTime EnrollmentDate { get; set; }
        [Required]
        public EnrollmentStatus Status { get; set; }
    }
}
