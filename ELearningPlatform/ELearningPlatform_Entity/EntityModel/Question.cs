using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Entity.EntityModel
{
    [Table("Questions")]
    public class Question : EntityBase
    {
        [Required]
        public Guid QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz Quiz { get; set; }
        [Required]
        public string Content { get; set; }
        public virtual ICollection<Answer> Answer { get; set; }
    }
}
