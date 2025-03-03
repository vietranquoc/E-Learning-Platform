using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Entity.EntityModel
{
    [Table("Quizzes")]
    public class Quiz : EntityBase
    {
        [Required]
        public Guid CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public int TotalQuestions { get; set; }
        public virtual ICollection<Question> Question { get; set; }
        public virtual ICollection<UserQuizResult> UserQuizResult { get; set; }
    }
}
