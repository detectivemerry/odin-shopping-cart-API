using OdinShopping.Exceptions;
using System.Security.Claims;

namespace OdinShopping.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserName()
        {
            var result = string.Empty;

            if (_httpContextAccessor != null)
            {
                if(_httpContextAccessor.HttpContext != null)
                    result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            }

            if (result != null)
                return result;
            else
                throw new OdinShoppingException();
        }

        public string GetUserId()
        {
            var result = string.Empty;

            if (_httpContextAccessor != null)
            {
                if (_httpContextAccessor.HttpContext != null)
                    result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            if (result != null)
                return result;
            else
                return string.Empty;
        }

    }
}
