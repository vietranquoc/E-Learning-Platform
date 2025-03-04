using ELearningPlatform_Domain.Features.UserFeatures.Commands;
using ELearningPlatform_Domain.Service.Interface.User;
using ELearningPlatform_Domain.ViewModel.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Domain.Service.Implementation.User
{
    public class UserService : IUserService
    {
        private readonly IMediator _mediator;

        public UserService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<bool> CreateUser(CreateUserCommand request)
        {
            return await _mediator.Send(request);
        }

        public async Task<TokenViewModel> Login(LoginModelCommand request)
        {
            try
            {
                return await _mediator.Send(request);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.ToString());
            }
        }
    }
}
