using ELearningPlatform_Authentication.Handlers;
using ELearningPlatform_Authentication.TokenValidators;
using ELearningPlatform_Domain.Features.UserFeatures.Commands;
using ELearningPlatform_Domain.Service.Interface.User;
using ELearningPlatform_Domain.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ELearningPlatform_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        #region Private Fields 
        private readonly IUserService _userService;
        private readonly JwtModel _jwt;
        private readonly IProfileService _profileService;
        private readonly IHttpContextAccessor _httpContext;
        #endregion

        public UsersController (IUserService userService, IOptions<JwtModel> jwt, 
            IProfileService profileService, IHttpContextAccessor httpContext)
        {
            _userService = userService;
            _jwt = jwt.Value;
            _profileService = profileService;
            _httpContext = httpContext;
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenViewModel>> signin([FromBody] LoginModelCommand model)
        {
            #region Validation
            // Parameter hasn't been initialized
            if (model == null)
            {
                model = new LoginModelCommand();
                TryValidateModel(model);
            }

            // Invalid modelState
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            #endregion

            var loginResult = await _userService.Login(model);

            if (loginResult == null)
            {
                return Ok(false);
            }

            var account = loginResult.UserProfile;
            var claims = new List<Claim>
            {
                new Claim(nameof(account.Id), account.Id.ToString()),
                new Claim(nameof(account.FullName), account.FullName)
            };

            //Add role claim    
            var userRoles = account.Roles.Select(r => r.RoleName);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            loginResult.Code = _profileService.GenerateJwt(claims.ToArray(), _jwt);
            loginResult.LifeTime = _jwt.LifeTime;

            return Ok(loginResult);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> signup([FromBody] CreateUserCommand model)
        {
            #region Parameters validation

            // Parameter hasn't been initialized
            if (model == null)
            {
                model = new CreateUserCommand();
                TryValidateModel(model);
            }

            // Invalid modelState
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            #endregion

            // Gọi UserService để thực hiện việc tạo tài khoản
            var newAccountResult = await _userService.CreateUser(model);

            return Ok(newAccountResult);
        }
    }
}
