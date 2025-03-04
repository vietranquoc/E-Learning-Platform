using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Domain.ViewModel.User
{
    public class AccountViewModel
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PassWord { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public string Gender { get; set; }
    }

    public class RoleViewModel
    {
        public Guid Id { get; set; }
        public string RoleName { get; set; }
    }
}
