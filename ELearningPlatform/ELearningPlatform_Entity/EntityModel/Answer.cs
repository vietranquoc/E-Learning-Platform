using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Entity.EntityModel
{
    [Table("Answers")]
    public class Answer : EntityBase
    {
        [Required]
        public Guid QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question Question { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public bool IsCorrect { get; set; } = false;
        [Required]
        public int OrderNumber { get; set; }
    }
}
