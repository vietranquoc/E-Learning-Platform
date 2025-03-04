using ELearningPlatform_Data.Infracstructure.Interfaces;
using ELearningPlatform_Domain.Features.UserFeatures.Commands;
using ELearningPlatform_Domain.ViewModel.User;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELearningPlatform_Domain.Handler.Users
{
    public class LoginHandler : UserBaseHandler, IRequestHandler<LoginModelCommand, TokenViewModel>
    {
        public LoginHandler(IHttpContextAccessor httpContext, IUnitOfWork unitOfWork) : base(httpContext, unitOfWork)
        {
        }

        public async Task<TokenViewModel> Handle(LoginModelCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ArgumentException("Password is required.");


            var account = await _unitOfWork.Users.GetQueryable(x => x.Username == request.Username)
                .Select(x => new AccountViewModel
                {
                    Id = x.Id,
                    Username = x.Username,
                    PassWord = x.HashedPassword,
                    Email = x.UserDetail.Email,
                    FullName = string.Concat(x.UserDetail.FirstName, " ", x.UserDetail.LastName),
                    Phone = x.UserDetail.Phone,
                    Roles = x.UserRole
                    .Select(r => new RoleViewModel
                    {
                        Id = r.Role.Id,
                        RoleName = r.Role.RoleName
                    }).ToList(),
                    Gender = x.UserDetail.Gender,
                }).FirstOrDefaultAsync();

            if (account == null)
            {
                throw new ArgumentException("Username does not exist!");
            }

            if (account.PassWord != Encrypt(request.Password))
            {
                throw new ArgumentException("Invalid Password");
            }

            account.PassWord = Decrypt(account.PassWord);

            var token = new TokenViewModel();
            token.UserProfile = account;

            return token;
        }
    }
}
