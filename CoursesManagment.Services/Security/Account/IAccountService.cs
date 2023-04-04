using CoursesManagment.Core.Common;
using CoursesManagment.Core.Interfaces;
using CoursesManagment.DTO.Security.User;

namespace CoursesManagment.Services.Security.Account
{
    public interface IAccountService
    {
        Task<string> Login(LoginParamsDto loginParams);
        Task<IResponseDTO> CreateUser(CreateUpdateUser options, Guid userId);
        Task<IResponseDTO> UpdateUser(Guid id, CreateUpdateUser options);
        Task<IResponseDTO> ChangePassword(Guid userId, ChangePasswordParamsDto options);
        Task<IResponseDTO> ForgetPassword(string email);
        Task<IResponseDTO> ResetPassword(ResetPasswordParamsDto options);

        #region User CRUD
        Task<PaginationResult<UserDto>> GetUsers(UserFilterDto filterDto);
        IResponseDTO GetUsersDropdown(UserFilterDto filterDto);
        IResponseDTO GetUser(string rootPath, Guid id);
        Task<IResponseDTO> DeactivateUser(Guid id);
        Task<IResponseDTO> ActivateUser(Guid id);
        #endregion
    }
}
