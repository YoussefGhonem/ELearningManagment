using CoursesManagment.Data;
using CoursesManagment.DTO.Security.User;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CoursesManagment.Validators.Account
{
    public class CreateUserValidator : AbstractValidator<CreateUpdateUser>
    {
        private readonly AppDbContext _context;

        public CreateUserValidator(AppDbContext context)
        {
            _context = context;
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty()
                .Must(x => x.Trim().Length >= 2).WithMessage("Length must be greater than or equal to 2");

            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty()
                .Must(x => x.Trim().Length >= 2).WithMessage("Length must be greater than or equal to 2");

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .EmailAddress()
                .MustAsync(BeUniqueIdentity).WithMessage("Email is already exist.");

            RuleFor(r => r.PhoneNumber)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty();

        }

        private async Task<bool> BeUniqueIdentity(string email, CancellationToken arg2)
        {
            return !await _context.Users.AnyAsync(x => x.Email == email, arg2);
        }
    }
}
