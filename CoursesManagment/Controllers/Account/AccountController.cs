using CoursesManagment.Core.Common;
using CoursesManagment.Core.Interfaces;
using CoursesManagment.Data.DbModels.SecuritySchema;
using CoursesManagment.DTO.Security.User;
using CoursesManagment.Services.Security.Account;
using CoursesManagment.Validators.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CoursesManagment.Core.Extensions;
using CoursesManagment.Geteway.Extensions;
using Nest;
using CoursesManagment.Data;

namespace CoursesManagment.Geteway.Controllers.Account
{
    [Route("api/v1/account")]
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly AppDbContext _appDbContext;

        public AccountController(
           IAccountService accountService,
          AppDbContext appDbContext
           ,
           IResponseDTO response,

           UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher,
           IHttpContextAccessor httpContextAccessor) : base(response, httpContextAccessor)
        {
            _accountService = accountService;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _appDbContext = appDbContext;
        }


        [AllowAnonymous]
        [Route("login")]
        [HttpPost]
        public async Task<IResponseDTO> Login([FromBody] LoginParamsDto loginParams)
        {
            // Validate
            var validationResult = await (new LoginValidator(_userManager, _passwordHasher)).ValidateAsync(loginParams);
            if (!validationResult.IsValid)
            {
                _response.IsPassed = false;
                _response.Errors = validationResult.ErrorMsg();
                return _response;
            }
            _response.Data = await _accountService.Login(loginParams);
            return Ok(_response.Data) ;
        }

        [AllowAnonymous]
        [Route("resetpassword")]
        [HttpPost]
        public async Task<IResponseDTO> ResetPassword([FromBody] ResetPasswordParamsDto options)
        {
            _response = await _accountService.ResetPassword(options);
            return _response;
        }

        [AllowAnonymous]
        [Route("forgetpassword")]
        [HttpPost]
        public async Task<IResponseDTO> ForgetPassword([FromBody] ForgetPassParamsDto forgetPassParamsDto)
        {
            _response = await _accountService.ForgetPassword(forgetPassParamsDto.Email.Trim());
            return _response;
        }

        [Route("me/changepassword")]
        [HttpPost]
        public async Task<IResponseDTO> ChangePassword([FromBody] ChangePasswordParamsDto options)
        {
            _response = await _accountService.ChangePassword(LoggedInUserId, options);
            return _response;
        }

        [Route("me/updateprofile")]
        [HttpPut]
        public async Task<IResponseDTO> UpdateUserProfile(CreateUpdateUser options)
        {
            var file = Request.Form?.Files?.Count > 0 ? Request.Form?.Files[0] : null;
            _response = await _accountService.UpdateUser(LoggedInUserId, options);
            return _response;
        }

        [Route("users/list")]
        [HttpGet]
        public IResponseDTO GetUsersDropdown([FromQuery] UserFilterDto filterDto)
        {
            _response = _accountService.GetUsersDropdown(filterDto);
            return _response;
        }

        #region User CRUD
        [AllowAnonymous]
        [Route("users")]
        [HttpGet]
        public Task<PaginationResult<UserDto>> SearchUsers([FromQuery] UserFilterDto filterDto)
        {
            return _accountService.GetUsers(filterDto);
        }

        [Authorize(Roles = "Administrative")]
        [Route("users")]
        [HttpPost]
        public async Task<IResponseDTO> CreateUser([FromBody] CreateUpdateUser options)
        {
            // Validate
            var validationResult = await (new CreateUserValidator(_appDbContext)).ValidateAsync(options);
            if (!validationResult.IsValid)
            {
                _response.IsPassed = false;
                _response.Errors = validationResult.ErrorMsg();
                return _response;
            }
            _response = await _accountService.CreateUser(options, LoggedInUserId);
            return _response;
        }

        [Authorize(Roles = "Administrative")]
        [Route("users/{id}/deactivate")]
        [HttpPost]
        public async Task<IResponseDTO> DeactivateUser([FromRoute] Guid id)
        {
            _response = await _accountService.DeactivateUser(id);
            return _response;
        }


        [Authorize(Roles = "Administrative")]
        [Route("users/{id}/activate")]
        [HttpPost]
        public async Task<IResponseDTO> ActivateUser([FromRoute] Guid id)
        {
            _response = await _accountService.ActivateUser(id);
            return _response;
        }


        [Authorize(Roles = "Administrative")]
        [Route("users/{id}")]
        [HttpPut]
        public async Task<IResponseDTO> UpdateUser([FromRoute] Guid id, [FromBody] CreateUpdateUser options)
        {
            _response = await _accountService.UpdateUser(id, options);
            return _response;
        }
        #endregion
    }
}