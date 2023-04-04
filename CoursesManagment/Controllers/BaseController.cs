using CoursesManagment.Core.Enums;
using CoursesManagment.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoursesManagment.Geteway.Controllers
{

    [ApiController]
    public class BaseController : ControllerBase
    {

        private IHttpContextAccessor _httpContextAccessor;
        public IResponseDTO _response;

        public BaseController(IResponseDTO responseDTO, IHttpContextAccessor httpContextAccessor)
        {
            _response = responseDTO;
            _httpContextAccessor = httpContextAccessor;
        }


        public Guid LoggedInUserId
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User?.Claims?.Where(c => c.Type == "Id")?.SingleOrDefault()?.Value != null)
                {
                    return Guid.Parse(_httpContextAccessor.HttpContext?.User?.Claims?.Where(c => c.Type == "Id")?.SingleOrDefault()?.Value);
                }
                return Guid.NewGuid();
            }
        }
        public List<ApplicationRolesEnum> Roles
        {
            get
            {
                if (_httpContextAccessor.HttpContext?.User?.Claims?.Count(c => c.Type == ClaimTypes.Role) > 0)
                {
                    return _httpContextAccessor.HttpContext?.User?.Claims?.Where(c => c.Type == ClaimTypes.Role)?.Select(x => (ApplicationRolesEnum)Enum.Parse(typeof(ApplicationRolesEnum), x.Value)).ToList();
                }
                return new List<ApplicationRolesEnum>();
            }
        }

        public string LoggedInUserName { get { return _httpContextAccessor.HttpContext?.User?.Identity?.Name; } }
        public string ServerRootPath { get { return $"{Request.Scheme}://{Request.Host}{Request.PathBase}"; } }
        public string IP { get { return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4().ToString(); } }

    }
}