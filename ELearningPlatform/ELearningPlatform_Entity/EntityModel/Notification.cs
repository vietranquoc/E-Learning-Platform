using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Entity.EntityModel
{
    [Table("Notifications")]
    public class Notification : EntityBase
    {
        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public bool IsRead { get; set; } = false;
    }
}
