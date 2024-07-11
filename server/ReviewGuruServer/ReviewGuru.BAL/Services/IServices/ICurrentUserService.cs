using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services.IServices
{
    public interface ICurrentUserService
    {
        int Id { get; }
        bool IsAuthenticated { get; }
    }
}
