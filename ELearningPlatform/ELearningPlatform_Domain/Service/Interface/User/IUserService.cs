using ELearningPlatform_Domain.Features.UserFeatures.Commands;
using ELearningPlatform_Domain.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Domain.Service.Interface.User
{
    public interface IUserService
    {
        Task<bool> CreateUser(CreateUserCommand request);
        Task<TokenViewModel> Login(LoginModelCommand request);
    }
}
