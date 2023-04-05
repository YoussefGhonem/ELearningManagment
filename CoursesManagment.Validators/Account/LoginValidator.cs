using CoursesManagment.Data.DbModels.SecuritySchema;
using CoursesManagment.DTO.Security.User;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace CoursesManagment.Validators.Account
{
    public class LoginValidator : AbstractValidator<LoginParamsDto>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private ApplicationUser _user;

        public LoginValidator(UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            {

                RuleFor(r => r.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .MustAsync(BeExistUser)
                    .WithMessage("Email is not found")
                .Must(BeActiveUser)
                    .WithMessage("You are deactivated, Please contact your administrator to activate your account.")
                .Must(BeExistPassword)
                    .WithMessage("Please reset your password first to be able to login.")
                .MustAsync((command, email, cancellationToken) => BeCorrectPassword(command.Password))
                    .WithMessage("Password is not valid.");

                RuleFor(c => c.Password)
                    .Cascade(CascadeMode.Stop)
                    .NotNull()
                    .NotEmpty();
            }
        }
        private async Task<bool> BeExistUser(string email, CancellationToken cancellationToken)
        {
             _user = await _userManager.FindByEmailAsync(email);
            return _user is not null;
        }

        private bool BeActiveUser(string email)
        {
            if (_user.IsActive == true)
            {
                 return true;
            }   
            else 
            {
                 return false;
            }
        }

        private bool BeExistPassword(string password)
        {
            return _user.PasswordHash is not null;
        }



        private async Task<bool> BeCorrectPassword(string password)
        {
            var isValidPassword = _passwordHasher.VerifyHashedPassword(_user, _user.PasswordHash, password) ==
                                  PasswordVerificationResult.Success;

            if (isValidPassword) return true;
            await _userManager.AccessFailedAsync(_user);
            return false;
        }
    } 
}