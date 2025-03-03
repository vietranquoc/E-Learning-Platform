using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Entity.EntityModel
{
    [Table("Users")]
    public class User : EntityBase
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string HashedPassword { get; set; }

        public virtual UserDetail UserDetail { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
        public virtual ICollection<Course> CreatedCourse { get; set; }
        public virtual ICollection<Enrollment> Enrollment { get; set; }
        public virtual ICollection<UserQuizResult> UserQuizResult { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
    }
}
