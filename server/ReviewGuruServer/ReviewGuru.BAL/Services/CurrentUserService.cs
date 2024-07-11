using Microsoft.AspNetCore.Http;
using ReviewGuru.BLL.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int Id
        {
            get
            {
                if (IsAuthenticated)
                {
                    var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("UserId");
                    if (userIdClaim != null)
                    {
                        return int.Parse(userIdClaim.Value);
                    }
                }
                throw new InvalidOperationException("User is not authenticated.");
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
