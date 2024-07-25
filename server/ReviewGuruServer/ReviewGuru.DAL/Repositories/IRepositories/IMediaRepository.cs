using ReviewGuru.DAL.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewGuru.DAL.Repositories.IRepositories
{
    public interface IMediaRepository : IGenericRepository<Media>
    {
        public Task<Media?> GetMediaWithReviewsAsync(int mediaId, CancellationToken cancellationToken = default);
    }
}
