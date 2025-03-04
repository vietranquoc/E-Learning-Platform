using ELearningPlatform_Data.Infracstructure.Interfaces;
using ELearningPlatform_Domain.Features.UserFeatures.Commands;
using ELearningPlatform_Entity.EntityModel;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ELearningPlatform_Authentication.Domain.ViewModel.CommonModel;
using FluentValidation;

namespace ELearningPlatform_Domain.Handler.Users
{
    public class CreateUserHandler : UserBaseHandler, IRequestHandler<CreateUserCommand, bool>
    {
        public CreateUserHandler(IHttpContextAccessor httpContext, IUnitOfWork unitOfWork) : base(httpContext, unitOfWork)
        {
        }
        public async Task<bool> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            
            if (await CheckUserExist(request.Username))
            {
                throw new ELearningValidationException(new List<ErrorViewModel>
                {
                    new ErrorViewModel 
                    { 
                        Code = "USERNAME_EXISTS", 
                        Message = "Username already exists." 
                    }
                });
            }

            var id = Guid.NewGuid();
            var user = new User
            {
                Id = id,
                Username = request.Username,
                HashedPassword = Encrypt(request.Password),
                IsActive = true
            };

            var userDetail = new UserDetail
            {
                UserId = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Gender = request.Gender,
                Phone = request.Phone,
            };

            _unitOfWork.Users.Insert(user);
            _unitOfWork.UserDetails.Insert(userDetail);

            var result = await _unitOfWork.CommitAsync();
            if (result <= 0)
            {
                throw new Exception("Failed to save user to database.");
            }

            return true;
        }
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required.");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email format.");
            RuleFor(x => x.Phone).Matches(@"^(0[3|5|7|8|9])\d{8,9}$").WithMessage("Invalid phone number format.");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender is required.")
                .Must(g => g == "Male" || g == "Female" || g == "Other")
                .WithMessage("Gender must be 'Male', 'Female', or 'Other'.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username is required.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("Password must be at least 6 characters.");
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Confirm password does not match.");
        }
    }
}
