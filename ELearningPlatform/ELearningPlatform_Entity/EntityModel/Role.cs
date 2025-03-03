using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Entity.EntityModel
{
    [Table("Roles")]
    public class Role : EntityBase
    {
        [Required]
        public string RoleName { get; set; }

        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
