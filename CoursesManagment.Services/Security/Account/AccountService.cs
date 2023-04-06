using CoursesManagment.Core.Common;
using CoursesManagment.Core.CurrentUser;
using CoursesManagment.Core.Extensions;
using CoursesManagment.Core.Interfaces;
using CoursesManagment.Data;
using CoursesManagment.Data.DbModels.SecuritySchema;
using CoursesManagment.DTO.Security.User;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CoursesManagment.Services.Security.Account
{
    public class AccountService : IAccountService
    {
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly IResponseDTO _response;
        public AccountService(
          IConfiguration configuration,
          UserManager<ApplicationUser> userManager,
          IPasswordHasher<ApplicationUser> passwordHasher,
          IResponseDTO responseDTO,
          AppDbContext appDbContext
         )
        {
            _configuration = configuration;
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _response = responseDTO;
            _appDbContext = appDbContext;

        }

        public async Task<string> Login(LoginParamsDto loginParams)
        {
            try
            {
                var appUser = await _appDbContext.Users.Include(x => x.UserRoles).ThenInclude(x => x.Role)
                                                       .FirstOrDefaultAsync(x => x.Email == loginParams.Email);
                if (appUser == null)
                {
                    _response.Errors.Add("Email is not found".ErrorMsg());
                    _response.IsPassed = false;
                    return _response.Message;
                }
                if (appUser != null &&
                    _passwordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash, loginParams.Password) !=
                    PasswordVerificationResult.Success)
                {
                    _response.Errors.Add($"For the Invalid password".ErrorMsg());
                    _response.IsPassed = false;
                    return _response.Message;
                }

                var token = GenerateJSONWebToken(appUser);

                _response.IsPassed = true;
                _response.Message = "You are logged in successfully.";
                _response.Data = token;
                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsPassed = false;
                _response.Errors.Add($"Error {ex.Message}".ErrorMsg());
                return _response.Message;
            }
            return _response.Data;
        }
        public async Task<IResponseDTO> ForgetPassword(string email)
        {
            try
            {
                var appUser = await _userManager.FindByEmailAsync(email);
                if (appUser == null)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("Invalid Email".ErrorMsg());
                    return _response;
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);

                // encode the token
                string validToken = HttpUtility.UrlEncode(token);

                _appDbContext.SaveChanges();

                // send email
                //await _emailService.RequestToResetPassword(email, validToken);
                _response.IsPassed = true;
                _response.Message = "Done";
                _response.Data = validToken;
            }
            catch (Exception ex)
            {
                _response.Data = null;
                _response.IsPassed = false;
                _response.Errors.Add($"Error {ex.Message}".ErrorMsg());
            }
            return _response;
        }
        public async Task<IResponseDTO> ResetPassword(ResetPasswordParamsDto options)
        {
            var user = await _userManager.FindByEmailAsync(options.Email);
            var result = await _userManager.ResetPasswordAsync(user, options.Token, options.NewPassword);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                throw new Exception(string.Join(",", errors));
            }
            _response.IsPassed = true;
            _response.Message = "Your password is setted successfully";
            return _response;
        }
        public async Task<IResponseDTO> ChangePassword(Guid userId, ChangePasswordParamsDto options)
        {
               try
            {
                if (string.IsNullOrEmpty(options.CurrentPassword))
                {
                    _response.Errors.Add("Currnet password should not be empty".ErrorMsg());
                    _response.IsPassed = false;
                    return _response;
                }
                else if (string.IsNullOrEmpty(options.NewPassword))
                {
                    _response.Errors.Add("New password should not be empty".ErrorMsg());
                    _response.IsPassed = false;
                    return _response;
                }

                var appUser = await _userManager.FindByIdAsync(userId.ToString());
                if (appUser == null)
                {
                    _response.Errors.Add("User is not found".ErrorMsg());
                    _response.IsPassed = false;
                    return _response;
                }
                var result = await _userManager.ChangePasswordAsync(appUser, options.CurrentPassword, options.NewPassword);
                if (!result.Succeeded)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add($"{result.Errors.Select(x => x.Description).ToList()}".ErrorMsg());
                    return _response;
                }

                _response.IsPassed = true;
                _response.Message = "Your password is changed successfully";
            }
            catch (Exception ex)
            {
                _response.IsPassed = false;
                _response.Errors.Add($"Error {ex.Message}".ErrorMsg());
                return _response;
            }
            return _response;
        }
        public async Task<IResponseDTO> UpdateUser(Guid id, CreateUpdateUser options)
        {
            try
            {
                var appUser = await _userManager.FindByIdAsync(id.ToString());
                if (appUser == null)
                {
                    _response.Errors.Add("User id is not exist.".ErrorMsg());
                    _response.IsPassed = false;
                    return _response;
                }
                else if (appUser.IsActive == false)
                {
                    _response.Errors.Add("Your Account is not activated. Please contact your administration".ErrorMsg());
                    _response.IsPassed = false;
                    return _response;
                }

                else if (options.FirstName.Length > 80 || options.LastName.Length > 80)
                {
                    _response.Errors.Add("First and last name fields should be limited to 80 characters at maximum.".ErrorMsg());
                    _response.IsPassed = false;
                    return _response;
                }
                else if (options.Email.Trim().ToLower() != appUser.Email.Trim().ToLower())
                {
                    // Check email duplicate
                    var isDuplicated = _appDbContext.Users.Any(x => x.Id != id && x.Email.Trim().ToLower() == options.Email.Trim().ToLower());
                    if (isDuplicated)
                    {
                        _response.Errors.Add("Email already exists, please try a new one.".ErrorMsg());
                        _response.IsPassed = false;
                        return _response;
                    }

                    // Update user info
                    appUser.Email = options.Email;
                    appUser.EmailConfirmed = false;
                }


                // Update user info
                appUser.FirstName = options.FirstName;
                appUser.LastName = options.LastName;
                appUser.PhoneNumber = options.PhoneNumber;
                appUser.DateOfBirth = options.DateOfBirth;

                // Update DB
                IdentityResult result = await _userManager.UpdateAsync(appUser);
                if (!result.Succeeded)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add($"{result.Errors.Select(x => x.Description).ToList()}".ErrorMsg());
                    return _response;
                }

                _response.IsPassed = true;
                _response.Message = "Your profile is updated successfully";
            }
            catch (Exception ex)
            {
                _response.Data = null;
                _response.IsPassed = false;
                _response.Errors.Add($"Error {ex.Message}".ErrorMsg());
            }

            return _response;
        }
        public async Task<IResponseDTO> CreateUser(CreateUpdateUser options, Guid userId)
        {
            try
            {
                // Generate user password
                var password = GeneratePassword();
                var appUser = options.Adapt<ApplicationUser>();
                appUser.CreatedOn = DateTime.Now;
                appUser.UserName = appUser.Email;
                appUser.IsActive = true;
                var rolesFromDb = await _appDbContext.Roles.ToListAsync();
                var role = rolesFromDb.FirstOrDefault(x => x.Name == options.Role.ToString());

                appUser.UserRoles = new List<ApplicationUserRole>
                {
                    new ApplicationUserRole
                    {
                    Role = role,
                    RoleId = role.Id
                    }
                };

                // Create the user
                IdentityResult result = await _userManager.CreateAsync(appUser, password);
                if (!result.Succeeded)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add($"{result.Errors.Select(x => x.Description).ToList()}".ErrorMsg()) ;
                    return _response;
                }
                // send email to the user to change his password
                // var verifyEmailToken = await _userManager.GeneratePasswordResetTokenAsync(appUser);
                // verifyEmailToken = WebUtility.UrlEncode(verifyEmailToken);
                // await _emailService.AfterRegistiration(appUser.Email, verifyEmailToken);

                _response.IsPassed = true;
                _response.Message = "The Email was sent to the user email so he can set his password.";
            }
            catch (Exception ex)
            {
                _response.Data = null;
                _response.IsPassed = false;
                _response.Errors.Add($"Error {ex.Message}".ErrorMsg());
            }

            return _response;
        }

        #region User CRUD
        public async Task<PaginationResult<UserDto>> GetUsers(UserFilterDto filterDto)
        {

            var query = _appDbContext.Users.Include(x=>x.UserRoles).ThenInclude(x=>x.Role).AsQueryable().Filter(filterDto)
                                                         .Paginate(filterDto.PageSize, filterDto.PageNumber);
            try
            {
                Mapper.ApplyMapping();
                var dataList = query.list.Adapt<List<UserDto>>();
                return new PaginationResult<UserDto>(dataList, query.total);

            }
            catch (Exception ex)
            {

                throw;
            }


        }
        public IResponseDTO GetUsersDropdown(UserFilterDto filterDto)
        {
            try
            {
                var query = _userManager.Users
                                .Include(x => x.UserRoles)
                                .AsQueryable();

                var datalist = query.ToList().Adapt<List<LookupDto>>();

                _response.IsPassed = true;
                _response.Data = datalist;
            }
            catch (Exception ex)
            {
                _response.Data = null;
                _response.IsPassed = false;
                _response.Errors.Add($"Error {ex.Message}".ErrorMsg());
            }

            return _response;
        }
        public IResponseDTO GetUser(string rootPath, Guid id)
        {
            try
            {
                var user = _appDbContext.Users.Find(id);

                var userDto = user.Adapt<UserDto>();

                _response.IsPassed = true;
                _response.Data = userDto;
            }
            catch (Exception ex)
            {
                _response.Data = null;
                _response.IsPassed = false;
                _response.Errors.Add($"Error {ex.Message}".ErrorMsg());
            }

            return _response;
        }
        public async Task<IResponseDTO> DeactivateUser(Guid id)
        {
            try
            {
                var appUser = await _userManager.FindByIdAsync(id.ToString());
                if (appUser == null)
                {
                    _response.Errors.Add("A User id is not exist.".ErrorMsg());
                    _response.IsPassed = false;
                    return _response;
                }


                // Update user info
                appUser.IsActive = false;

                // Update DB
                _appDbContext.Users.Update(appUser);
                var save = await _appDbContext.SaveChangesAsync();
                if (save == 0)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("There are no changes to be saved".ErrorMsg());
                    return _response;
                }

                _response.IsPassed = true;
                _response.Message = "User account is deactivated successfully";
            }
            catch (Exception ex)
            {
                _response.Data = null;
                _response.IsPassed = false;
                _response.Errors.Add($"Error {ex.Message}".ErrorMsg());
            }

            return _response;
        }
        public async Task<IResponseDTO> ActivateUser(Guid id)
        {
            try
            {
                var appUser = await _userManager.FindByIdAsync(id.ToString());

                if (appUser == null)
                {
                    _response.Errors.Add("User id is not exist.".ErrorMsg());
                    _response.IsPassed = false;
                    return _response;
                }
                appUser.IsActive = true;
                // Update DB
                _appDbContext.Users.Update(appUser);
                var save = await _appDbContext.SaveChangesAsync();
                if (save == 0)
                {
                    _response.IsPassed = false;
                    _response.Errors.Add("There are no changes to be saved".ErrorMsg());
                    return _response;
                }

                _response.IsPassed = true;
                _response.Message = "User account is activated successfully";
            }
            catch (Exception ex)
            {
                _response.Data = null;
                _response.IsPassed = false;
                _response.Errors.Add($"Error {ex.Message}".ErrorMsg());
            }

            return _response;
        }
        #endregion


        #region Helper methds
        public string GenerateJSONWebToken(ApplicationUser user)
        {
            var signingKey = Convert.FromBase64String(_configuration["Jwt:Key"]);
            var expiryDuration = int.Parse(_configuration["Jwt:ExpiryDuration"]);

            var claims = new List<Claim>
            {
            new(ClaimKeys.Id, user.Id.ToString()),
            new(ClaimKeys.FirstName, user.FirstName),
            new(ClaimKeys.LastName, user.LastName),
            new(ClaimKeys.Name, $"{user.FirstName} {user.LastName}"),
            new(ClaimKeys.Status, user.IsActive.ToString()),
            new(ClaimKeys.Email, user.Email),
            };
            // User Roles
            claims.AddRange(user.UserRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole.Role.Name)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,              // Not required as no third-party is involved
                Audience = null,            // Not required as no third-party is involved
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(jwtToken);
            return token;
        }
        private string GeneratePassword()
        {
            var options = _userManager.Options.Password;

            int length = options.RequiredLength;

            bool nonAlphanumeric = options.RequireNonAlphanumeric;
            bool digit = options.RequireDigit;
            bool lowercase = options.RequireLowercase;
            bool uppercase = options.RequireUppercase;

            StringBuilder password = new StringBuilder();
            Random random = new Random();

            while (password.Length < length)
            {
                char c = (char)random.Next(32, 126);

                password.Append(c);

                if (char.IsDigit(c))
                    digit = false;
                else if (char.IsLower(c))
                    lowercase = false;
                else if (char.IsUpper(c))
                    uppercase = false;
                else if (!char.IsLetterOrDigit(c))
                    nonAlphanumeric = false;
            }

            if (nonAlphanumeric)
                password.Append((char)random.Next(33, 48));
            if (digit)
                password.Append((char)random.Next(48, 58));
            if (lowercase)
                password.Append((char)random.Next(97, 123));
            if (uppercase)
                password.Append((char)random.Next(65, 91));

            var result = Regex.Replace(password.ToString(), @"[^0-9a-zA-Z]+", "$");
            result += RandomString(6);

            return result;
        }
        private string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdrfghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #endregion
    }
}
