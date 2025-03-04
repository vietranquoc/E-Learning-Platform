using ELearningPlatform_Domain.ViewModel.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Domain.Features.UserFeatures.Commands
{
    public class LoginModelCommand : IRequest<TokenViewModel>
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
