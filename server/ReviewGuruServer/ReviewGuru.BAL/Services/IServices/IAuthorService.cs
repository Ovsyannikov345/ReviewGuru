using ReviewGuru.BLL.DTOs;
using ReviewGuru.BLL.Utilities.Constants;
using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.BLL.Services.IServices
{

    public interface IAuthorService
    {
        public Task<IEnumerable<Author>> GetAuthorListAsync(
            int pageNumber = Pagination.PageNumber,
            int pageSize = Pagination.MaxPageSize,
            CancellationToken cancellationToken = default);
    }
}
